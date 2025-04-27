namespace ContactService.Models
{
    public enum ContactType
    {
        Phone,
        Email,
        Location
    }

    public class ContactInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public ContactType Type { get; set; }
        public string Content { get; set; }
        public Guid PersonId { get; set; }
        public Person Person { get; set; }
    }
}
