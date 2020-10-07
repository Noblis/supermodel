#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Supermodel.Persistence
{
    public static class DataSeeder
    {
        public static Task SeedDataAsync()
        {
            var users = new[]
            {
                new TDMUser { FirstName="Ilya", LastName="Basin", Username="ilya.basin@noblis.org", Password="0" },
            };
            foreach (var user in users) user.Add();

            var firstUser = users[0];

            firstUser.ToDoLists.Add(new ToDoList
            {
                Name = "List #1",
                ToDoItems = new List<ToDoItem>
                {
                    new ToDoItem { Name = "Item 1"},
                    new ToDoItem { Name = "Item 2"},
                }
            });

            firstUser.ToDoLists.Add(new ToDoList
            {
                Name = "Groceries",
                ToDoItems = new List<ToDoItem>
                {
                    new ToDoItem { Name = "Bread"},
                    new ToDoItem { Name = "Eggs"},
                    new ToDoItem { Name = "Milk"},
                }
            });

            firstUser.ToDoLists.Add(new ToDoList
            {
                Name = "Supplies",
                ToDoItems = new List<ToDoItem>
                {
                    new ToDoItem { Name = "Pens"},
                    new ToDoItem { Name = "Pencils"},
                    new ToDoItem { Name = "Staplers"},
                }
            });

            return Task.CompletedTask;
        }
    }
}
