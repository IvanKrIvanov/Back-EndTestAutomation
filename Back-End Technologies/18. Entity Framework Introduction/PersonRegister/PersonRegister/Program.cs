using Microsoft.EntityFrameworkCore;
using PersonRegister.Data;
using PersonRegister.Data.Models;

namespace PersonRegister
{
    public class Startup
    {
        static async Task Main(string[] args)
        {
            var contextFactory = new PersonRegisterDbContextFactory();
            var dbContext = contextFactory.CreateDbContext(args);

            await dbContext.Database.MigrateAsync();
            
            /*var person = new Person()
            {
                FirstName = "Petyr",
                LastName = "Ivanov",
                City = "Plovdiv",
                Age = 35,
            };
            dbContext.Persons.Add(person);
            await dbContext.SaveChangesAsync();*/

            var person = dbContext.Persons.Where(p => p.FirstName == "Ivan").ToList();
            var person2 = dbContext.Persons.Where(p => p.Age == 20).Select(p => p.City).ToList();
            var person3 = dbContext.Persons.OrderByDescending(p => p.FirstName).ToList();
            var statement = dbContext.Persons.Any(p => p.FirstName == "Jivko");
            var statement2 = dbContext.Persons.All(p => p.FirstName == "Ivan");
            var person4 = dbContext.Persons.Skip(1).ToList();

        }
    }
}