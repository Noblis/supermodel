#nullable enable

using System;
using System.Threading.Tasks;
using EFCoreTester.DataContexts;
using EFCoreTester.Entities;
using Supermodel.Persistence.DataContext;
using Supermodel.Persistence.EFCore;
using Supermodel.Persistence.Entities.ValueTypes;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;

namespace EFCoreTester
{
    class Program
    {
        static async Task Main()
        {
            await Run<SqlServerDbContext>();
            //await Run<InMemoryDbContext>();
            //await Run<SqliteDbContext>();
        }

        static async Task Run<TDataContext>() where TDataContext : class, IDataContext, new()
        {
            await using (new UnitOfWork<TDataContext>())
            {
                await EFCoreUnitOfWorkContext.Database.EnsureDeletedAsync();
                await EFCoreUnitOfWorkContext.Database.EnsureCreatedAsync();
                await UnitOfWorkContext.SeedDataAsync();
            }
            
            await using (new UnitOfWork<TDataContext>())
            {
                var authors = new[]
                {
                    new Author { Name = "Lev Tolstoy", Address = new USAddress { Street = "2565 Pennington Place"} },
                    new Author { Name = "Mark Twain" }
                };
                foreach (var author in authors) author.Add();

                var books = new[]
                {
                    new Book { Title = "War and peace", Author = authors[0], Price = 30, Content = new BinaryFile { FileName = "WarAndPeace.pdf", BinaryContent = new byte[] { 0x10, 0x11, 0x12 } } },
                    new Book { Title = "Adventures of Huckleberry Finn", Author = authors[1], Price = 20, Content = new BinaryFile { FileName = "AdventuresOfHuckFinn.pdf", BinaryContent = new byte[] { 0x20, 0x21, 0x22 } } }
                };
                foreach (var book in books) book.Add();

                var users = new[]
                {
                    new User { Username = "ilya.basin@noblis.com", Password = "1234" }
                };
                foreach (var user in users) user.Add();

                //EFCoreUnitOfWorkContext.ValidateOnSaveEnabled = false;
            }

            await using (new UnitOfWork<TDataContext>(ReadOnly.Yes))
            {
                await using(new UnitOfWorkIfNoAmbientContext<TDataContext>(MustBeWritable.No))
                {
                    EFCoreUnitOfWorkContext.LoadReadOnlyEntitiesAsNoTracking = false;
                    
                    foreach (var book in await RepoFactory.Create<Book>().GetAllAsync())
                    {
                        // ReSharper disable once UnusedVariable
                        var address = book.Author.Address;
                        Console.WriteLine($"{book.Title} by {book.Author!.Name}. Cost: ${book.Price}");
                    }
                }
            }
            //await using (new UnitOfWork<TDataContext>())
            //{
            //    var repo = (BookRepo)RepoFactory.Create<Book>();
            //    repo.Delete(1);
            //}
        }
    }
}
