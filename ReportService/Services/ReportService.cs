using ReportService.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactService.Models;
using ContactService.Services;

namespace ReportService.Services
{
    public class ReportService : IReportService
    {
        private readonly IContactService _contactService;

        public ReportService(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<Report> CreateReportAsync(string location)
        {
            var persons = await _contactService.GetAllPersonsAsync();
            var filteredPersons = persons.Where(p => p.ContactInfos.Any(c => c.Type == ContactType.Location && c.Content.Contains(location))).ToList();

            var report = new Report
            {
                Id = Guid.NewGuid(),
                RequestedAt = DateTime.UtcNow,
                Location = location,
                PersonCount = filteredPersons.Count,
                PhoneNumberCount = filteredPersons.Sum(p => p.ContactInfos.Count(c => c.Type == ContactType.Phone)),
                Status = ReportStatus.Preparing
            };

            // Raporu veritabanına kaydedebiliriz (örneğin, DbContext ile)

            return report;
        }

        public async Task<List<Report>> GetAllReportsAsync()
        {
            // Veritabanından tüm raporları alabiliriz. Şu an için boş bir liste döneceğiz.
            return new List<Report>();
        }

        public async Task<Report> GetReportByIdAsync(Guid id)
        {
            // Veritabanından raporu ID ile alabiliriz. Şu an için null dönecek.
            return null;
        }
    }
}
