using ContactService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactService.Services
{
    public interface IContactService
    {
        Task<List<Person>> GetAllPersonsAsync();
        Task<Person> GetPersonByIdAsync(Guid id);
        Task<Person> CreatePersonAsync(Person person);
        Task<bool> DeletePersonAsync(Guid id);
        Task<ContactInfo> AddContactInfoAsync(Guid personId, ContactInfo contactInfo);
        Task<bool> DeleteContactInfoAsync(Guid contactInfoId);
    }
}
