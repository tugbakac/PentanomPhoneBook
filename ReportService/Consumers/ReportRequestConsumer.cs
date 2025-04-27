using Confluent.Kafka;
using Newtonsoft.Json;
using Shared.Events;
using ReportService.Data;
using ReportService.Models;
using ReportService.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ContactService.Models;

namespace ReportService.Consumers
{
    public class ReportRequestConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReportRequestConsumer> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _phoneBookServiceBaseUrl;

        public ReportRequestConsumer(IServiceScopeFactory scopeFactory, IConfiguration configuration, ILogger<ReportRequestConsumer> logger)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _logger = logger;
            _httpClient = new HttpClient();
            _phoneBookServiceBaseUrl = _configuration["PhoneBookService:BaseUrl"] ?? "http://localhost:5001"; // PhoneBookService adresinizi yapılandırmadan alabilirsiniz
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = "report-service-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_configuration["Kafka:Topic"]); // Topic adını yapılandırmadan alıyoruz

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    if (result?.Message?.Value != null)
                    {
                        var reportRequest = JsonConvert.DeserializeObject<ReportRequestCreatedEvent>(result.Message.Value);
                        _logger.LogInformation($"Rapor talebi alındı. Rapor ID: {reportRequest.ReportId}, Konum: {reportRequest.Location}");

                        using var scope = _scopeFactory.CreateScope();
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                        var report = await dbContext.Reports.FindAsync(reportRequest.ReportId);
                        if (report != null && report.Status == ReportStatus.Preparing)
                        {
                            try
                            {
                                // 1. PhoneBookService'ten ilgili verileri çek
                                var personsResponse = await _httpClient.GetAsync($"{_phoneBookServiceBaseUrl}/api/person"); // Tüm kişileri varsayalım, filtreleme gerekebilir
                                personsResponse.EnsureSuccessStatusCode();
                                var personsJson = await personsResponse.Content.ReadAsStringAsync();
                                var persons = JsonConvert.DeserializeObject<List<ContactService.Models.Person>>(personsJson); // ContactService.Models.Person tipini PhoneBookService'teki modelinize göre ayarlayın

                                var locationPersons = persons?.Where(p => p.ContactInfos?.Any(c => c.Type == ContactType.Location && c.Content.ToLower() == reportRequest.Location.ToLower()) == true).ToList();
                                var personCount = locationPersons?.Count ?? 0;
                                var phoneNumberCount = locationPersons?.Sum(p => p.ContactInfos?.Count(c => c.Type == ContactType.Phone) ?? 0) ?? 0;

                                // 2. Raporu güncelle
                                report.Location = reportRequest.Location;
                                report.PersonCount = personCount;
                                report.PhoneNumberCount = phoneNumberCount;
                                report.Status = ReportStatus.Completed;
                                // Eğer dosya oluşturuyorsanız, burada oluşturup FilePath'i güncelleyebilirsiniz

                                await dbContext.SaveChangesAsync(stoppingToken);
                                _logger.LogInformation($"Rapor oluşturuldu ve kaydedildi. Rapor ID: {report.Id}, Konum: {report.Location}, Kişi Sayısı: {personCount}, Telefon Sayısı: {phoneNumberCount}");
                            }
                            catch (HttpRequestException ex)
                            {
                                _logger.LogError($"PhoneBookService'e istek hatası: {ex.Message}");
                                report.Status = ReportStatus.Preparing; // Hata durumunda durumu değiştirebilirsiniz
                                await dbContext.SaveChangesAsync(stoppingToken);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"Rapor işleme hatası: {ex.Message}");
                                report.Status = ReportStatus.Preparing; // Hata durumunda durumu değiştirebilirsiniz
                                await dbContext.SaveChangesAsync(stoppingToken);
                            }
                        }
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"Kafka tüketim hatası: {ex.Error?.Reason}");
                }
                await Task.Delay(1000, stoppingToken); // Gerekirse tüketim aralığını ayarlayın
            }
            consumer.Close();
        }
    }
}