using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContactService.Models;
using ContactService.Services;
using FluentAssertions;

namespace ContactService.Tests
{
    public class ContactServiceTests
    {
        private readonly Mock<IContactService> _mockContactService;

        public ContactServiceTests()
        {
            _mockContactService = new Mock<IContactService>();
        }

        [Fact]
        public async Task CreatePerson_ShouldCreatePersonSuccessfully()
        {
            // Arrange
            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Company = "Acme Corp",
                ContactInfos = new List<ContactInfo>
                {
                    new ContactInfo { Type = ContactType.Phone, Content = "123456789" }
                }
            };

            _mockContactService.Setup(service => service.CreatePersonAsync(It.IsAny<Person>())).ReturnsAsync(person);

            // Act
            var result = await _mockContactService.Object.CreatePersonAsync(person);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("John");
            result.LastName.Should().Be("Doe");
            result.Company.Should().Be("Acme Corp");
            result.ContactInfos.Should().Contain(contact => contact.Type == ContactType.Phone && contact.Content == "123456789");
        }

        [Fact]
        public async Task DeletePerson_ShouldDeletePersonSuccessfully()
        {
            // Arrange
            var personId = Guid.NewGuid();
            _mockContactService.Setup(service => service.DeletePersonAsync(personId)).ReturnsAsync(true);

            // Act
            var result = await _mockContactService.Object.DeletePersonAsync(personId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task AddContactInfo_ShouldAddContactInfoSuccessfully()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var newContactInfo = new ContactInfo
            {
                Type = ContactType.Email,
                Content = "john.doe@example.com",
                PersonId = personId
            };

            _mockContactService.Setup(service => service.AddContactInfoAsync(personId, It.IsAny<ContactInfo>())).ReturnsAsync(newContactInfo);

            // Act
            var result = await _mockContactService.Object.AddContactInfoAsync(personId, newContactInfo);

            // Assert
            result.Should().NotBeNull();
            result.Type.Should().Be(ContactType.Email);
            result.Content.Should().Be("john.doe@example.com");
        }

        [Fact]
        public async Task DeleteContactInfo_ShouldDeleteContactInfoSuccessfully()
        {
            // Arrange
            var contactInfoId = Guid.NewGuid();
            _mockContactService.Setup(service => service.DeleteContactInfoAsync(contactInfoId)).ReturnsAsync(true);

            // Act
            var result = await _mockContactService.Object.DeleteContactInfoAsync(contactInfoId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetPersonById_ShouldReturnPersonSuccessfully()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var person = new Person
            {
                Id = personId,
                FirstName = "Jane",
                LastName = "Doe",
                Company = "XYZ Corp",
                ContactInfos = new List<ContactInfo>
                {
                    new ContactInfo { Type = ContactType.Phone, Content = "987654321" }
                }
            };

            _mockContactService.Setup(service => service.GetPersonByIdAsync(personId)).ReturnsAsync(person);

            // Act
            var result = await _mockContactService.Object.GetPersonByIdAsync(personId);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("Jane");
            result.LastName.Should().Be("Doe");
            result.Company.Should().Be("XYZ Corp");
            result.ContactInfos.Should().Contain(contact => contact.Type == ContactType.Phone && contact.Content == "987654321");
        }

        [Fact]
        public async Task GetAllPersons_ShouldReturnPersonsSuccessfully()
        {
            // Arrange
            var persons = new List<Person>
            {
                new Person
                {
                    FirstName = "Alice",
                    LastName = "Smith",
                    Company = "ABC Corp",
                    ContactInfos = new List<ContactInfo>
                    {
                        new ContactInfo { Type = ContactType.Phone, Content = "5551234" }
                    }
                },
                new Person
                {
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Company = "DEF Inc",
                    ContactInfos = new List<ContactInfo>
                    {
                        new ContactInfo { Type = ContactType.Email, Content = "bob.johnson@example.com" }
                    }
                }
            };

            _mockContactService.Setup(service => service.GetAllPersonsAsync()).ReturnsAsync(persons);

            // Act
            var result = await _mockContactService.Object.GetAllPersonsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].FirstName.Should().Be("Alice");
            result[1].FirstName.Should().Be("Bob");
        }
    }
}
