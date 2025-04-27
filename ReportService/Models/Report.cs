using ReportService.Models.Enums;
using System.ComponentModel.DataAnnotations;

public class Report
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    public ReportStatus Status { get; set; } = ReportStatus.Preparing;

    public string Location { get; set; } // Konum bilgisi
    public int PersonCount { get; set; }    // Kişi sayısı
    public int PhoneNumberCount { get; set; } // Telefon numarası sayısı
    public string FilePath { get; set; } // Rapor dosyasının kaydedildiği yer (isteğe bağlı)
}