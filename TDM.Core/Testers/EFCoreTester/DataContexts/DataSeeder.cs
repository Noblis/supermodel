using System.Threading.Tasks;
using EFCoreTester.Entities;
using Supermodel.Persistence.Entities.ValueTypes;

namespace EFCoreTester.DataContexts
{
    public static class DataSeeder
    {
        //public static void SeedData(ModelBuilder modelBuilder){ }
        public static Task SeedDataAsync()
        {
            var addresses = new[]
            {
                new USAddress
                {
                    Street = "2565 Pennington Place",
                    City = "Vienna",
                    State = "VA",
                    Zip = "22181"
                },
                new USAddress
                {
                    Street = "3431 W. Colette Ct",
                    City = "Mequon",
                    State = "WI",
                    Zip = "53092"
                }
            };

            var authors = new[]
            {
                new Author { Id = 1, Name = "Lev Tolstoy", Address = addresses[0] },
                new Author { Id = 2, Name = "Mark Twain", Address = addresses[1] }
            };
            foreach (var author in authors) author.Add();

            var books = new[]
            {
                new Book
                {
                    Id = 1,
                    Title = "War and peace",
                    Author = authors[0],
                    Price = 30,
                    Content = new BinaryFile
                    {
                        FileName = "WarAndPeace.pdf",
                        BinaryContent = new byte[] { 0x10, 0x11, 0x12 }
                    }
                },
                new Book
                {
                    Id = 2,
                    Title = "Adventures of Huckleberry Finn",
                    Author = authors[1],
                    Price = 20,
                    Content = new BinaryFile
                    {
                        FileName = "AdventuresOfHuckFinn.pdf",
                        BinaryContent = new byte[] { 0x20, 0x21, 0x22 }
                    }
                }
            };
            foreach (var book in books) book.Add();

            var user = new User { Id = 1, Username = "ilya.basin@gmail.com", Password = "1234" };
            user.Add();

            return Task.CompletedTask;
        }
    }
}
