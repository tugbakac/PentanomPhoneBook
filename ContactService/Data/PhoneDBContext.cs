using ContactService.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactService.Data
{
    public class PhoneDBContext:DbContext
    {
        public PhoneDBContext(DbContextOptions<PhoneDBContext> options) : base(options) { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactInfo>()
                .HasOne(ci => ci.Person)
                .WithMany(p => p.ContactInfos)
                .HasForeignKey(ci => ci.PersonId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete ekledik
        }
    }
}
