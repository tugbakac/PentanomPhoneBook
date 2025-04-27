using ReportService.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportService.Services
{
    public interface IReportService
    {
        Task<Report> CreateReportAsync(string location);
        Task<List<Report>> GetAllReportsAsync();
        Task<Report> GetReportByIdAsync(Guid id);
    }
}
