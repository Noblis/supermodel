using System.Threading.Tasks;
using MvcCoreTester.Models;

namespace MvcCoreTester.DataContexts
{
    public static class DataSeeder
    {
        public static Task SeedDataAsync()
        { 
            var schools = new[]
            {
                new School { Id = 1, Name = "MIT" },
                new School { Id = 2, Name = "CalTech" }
            };
            foreach (var school in schools) school.Add();

            return Task.CompletedTask;
        }
    }
}
