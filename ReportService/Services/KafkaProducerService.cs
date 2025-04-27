using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Events;
using System;
using System.Threading.Tasks;

namespace ContactService.Services
{
    public class KafkaProducerService
    {
        private readonly ProducerConfig _config;
        private readonly ILogger<KafkaProducerService> _logger;
        private readonly string _reportRequestTopic = "report-request-created";

        public KafkaProducerService(IConfiguration configuration, ILogger<KafkaProducerService> logger)
        {
            _config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };
            _logger = logger;
        }

        public async Task ProduceReportRequestCreatedEvent(Guid reportId, string location)
        {
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                try
                {
                    var reportRequestEvent = new ReportRequestCreatedEvent
                    {
                        ReportId = reportId,
                        Location = location,
                        RequestedAt = DateTime.UtcNow
                    };
                    var message = new Message<Null, string>
                    {
                        Value = JsonConvert.SerializeObject(reportRequestEvent)
                    };

                    var deliveryResult = await producer.ProduceAsync(_reportRequestTopic, message);
                    _logger.LogInformation($"'report-request-created' topic'ine mesaj gönderildi. Partition: {deliveryResult.Partition}, Offset: {deliveryResult.Offset}");
                }
                catch (ProduceException<Null, string> e)
                {
                    _logger.LogError($"Mesaj gönderme hatası: {e.Error.Reason}");
                }
            }
        }
    }
}