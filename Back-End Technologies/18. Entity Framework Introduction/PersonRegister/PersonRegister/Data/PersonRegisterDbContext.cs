using Microsoft.EntityFrameworkCore;
using PersonRegister.Data.Models;

namespace PersonRegister.Data
{
    public class PersonRegisterDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public PersonRegisterDbContext(DbContextOptions options)
        : base (options) 
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>();
        }
    }
}
