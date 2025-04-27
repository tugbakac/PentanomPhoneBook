using System.Text.Json.Serialization;

namespace ContactService.Models
{
    public class Person
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }

        [JsonIgnore] // Döngüyü önlüyor
        public ICollection<ContactInfo> ContactInfos { get; set; } = new List<ContactInfo>();
    }
}
