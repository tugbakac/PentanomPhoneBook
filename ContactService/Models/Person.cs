namespace ContactService.Models
{

        public class Person
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Company { get; set; }
            public ICollection<ContactInfo> ContactInfos { get; set; }
        }
}
