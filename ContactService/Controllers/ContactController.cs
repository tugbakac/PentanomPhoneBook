using ContactService.Models;
using ContactService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ContactService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        // Tüm kişileri listele
        [HttpGet]
        public async Task<IActionResult> GetAllPersons()
        {
            var persons = await _contactService.GetAllPersonsAsync();
            return Ok(persons);
        }

        // Yeni kişi oluştur
        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] Person person)
        {
            var createdPerson = await _contactService.CreatePersonAsync(person);
            return CreatedAtAction(nameof(GetPersonById), new { id = createdPerson.Id }, createdPerson);
        }

        // Id'ye göre kişi getirme
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonById(Guid id)
        {
            var person = await _contactService.GetPersonByIdAsync(id);
            if (person == null)
                return NotFound();

            return Ok(person);
        }

        // Kişi silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            var result = await _contactService.DeletePersonAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // İletişim bilgisi ekleme
        [HttpPost("{id}/contactinfo")]
        public async Task<IActionResult> AddContactInfo(Guid id, [FromBody] ContactInfo contactInfo)
        {
            var addedContactInfo = await _contactService.AddContactInfoAsync(id, contactInfo);
            if (addedContactInfo == null)
                return NotFound();

            return Ok(addedContactInfo);
        }

        // İletişim bilgisi silme
        [HttpDelete("contactinfo/{contactInfoId}")]
        public async Task<IActionResult> DeleteContactInfo(Guid contactInfoId)
        {
            var result = await _contactService.DeleteContactInfoAsync(contactInfoId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
