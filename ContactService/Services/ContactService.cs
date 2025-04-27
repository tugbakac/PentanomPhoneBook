using ContactService.Data;
using ContactService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.Services
{
    public class ContactService : IContactService
    {
        private readonly PhoneDBContext _context;

        public ContactService(PhoneDBContext context)
        {
            _context = context;
        }

        public async Task<List<Person>> GetAllPersonsAsync()
        {
            return await _context.Persons.Include(p => p.ContactInfos).ToListAsync();
        }

        public async Task<Person> GetPersonByIdAsync(Guid id)
        {
            return await _context.Persons.Include(p => p.ContactInfos)
                                         .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Person> CreatePersonAsync(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePersonAsync(Guid id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
                return false;

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ContactInfo> AddContactInfoAsync(Guid personId, ContactInfo contactInfo)
        {
            var person = await _context.Persons.FindAsync(personId);
            if (person == null)
                return null;

            contactInfo.PersonId = personId;
            _context.ContactInfos.Add(contactInfo);
            await _context.SaveChangesAsync();
            return contactInfo;
        }

        public async Task<bool> DeleteContactInfoAsync(Guid contactInfoId)
        {
            var contactInfo = await _context.ContactInfos.FindAsync(contactInfoId);
            if (contactInfo == null)
                return false;

            _context.ContactInfos.Remove(contactInfo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
