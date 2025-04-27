using ReportService.Services; 
using Moq;
using Xunit;
using ReportService.Models;
using System;
using ContactService.Models;
using ContactService.Services;

namespace ReportService.Test
{
    public class ReportServiceTest
    {
        private readonly Mock<IContactService> _mockContactService;
        private readonly ReportService.Services.ReportService _reportService;

        public ReportServiceTest()
        {
            _mockContactService = new Mock<IContactService>();
            _reportService = new ReportService.Services.ReportService(_mockContactService.Object); // Burada doğru sınıfı kullandığınızdan emin olun.
        }

        [Fact]
        public async Task CreateReportAsync_ShouldReturnReport()
        {
            // Arrange
            var location = "Istanbul";
            var personList = new List<Person>
            {
                new Person
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    ContactInfos = new List<ContactInfo>
                    {
                        new ContactInfo { Type = ContactType.Location, Content = "Istanbul" },
                        new ContactInfo { Type = ContactType.Phone, Content = "123456789" }
                    }
                }
            };

            _mockContactService.Setup(service => service.GetAllPersonsAsync()).ReturnsAsync(personList);

            // Act
            var result = await _reportService.CreateReportAsync(location);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Istanbul", result.Location);
            Assert.Equal(1, result.PersonCount);
            Assert.Equal(1, result.PhoneNumberCount);
        }
    }
}
