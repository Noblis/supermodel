#nullable enable

using System;
using System.Threading.Tasks;
using ApiClientCmd.Supermodel.Auth;
using ApiClientCmd.Supermodel.Persistence;
using Supermodel.ApiClient.Models;
using Supermodel.Mobile.Runtime.Common.DataContext.Core;
using Supermodel.Mobile.Runtime.Common.Repository;
using Supermodel.Mobile.Runtime.Common.UnitOfWork;

namespace ApiClientCmd
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();

            await using (new UnitOfWork<TDMSqliteDataContext>())
            {
                //var x = await RepoFactory.Create<ToDoList>().GetAllAsync();
                
                var toDoList = new ToDoList();
                toDoList.Add();

                toDoList.ToDoItems.Add(new ToDoItem { Name = "Item1" });
                toDoList.ToDoItems.Add(new ToDoItem { Name = "Item2" });
            }

            //var authHeaderGenerator = new BasicAuthHeaderGenerator("ilya.basin@noblis.org", "0");
            var authHeaderGenerator = new TDMSecureAuthHeaderGenerator("ilya.basin@noblis.org", "0", Array.Empty<byte>());

            // ReSharper disable once NotAccessedVariable
            DelayedModels<ToDoList> delayedTodoLists;
            await using (new UnitOfWork<TDMWebApiDataContext>())
            {
                UnitOfWorkContext.AuthHeader = authHeaderGenerator.CreateAuthHeader();
                
                var repo = RepoFactory.Create<ToDoList>();
                
                var todoLists = await repo.GetAllAsync();

                //todoLists[2].Delete(); //this causes things to fail

                todoLists[0].ToDoItems.Add(new ToDoItem { Name = "New One" });
                todoLists[1].ToDoItems.Add(new ToDoItem { Name = "Another New One" });

                //DelayedModels<ToDoList> delayedTodoLists;
                // ReSharper disable once UnusedVariable
                repo.DelayedGetAll(out delayedTodoLists);
                
                
                //// ReSharper disable once UnusedVariable
                //var count = await repo.GetCountAllAsync();

                //var studentSearch = new StudentSearch { Term = "Ilya" };
                //// ReSharper disable once UnusedVariable
                //var filteredStudents = await repo.GetWhereAsync(studentSearch);
                //// ReSharper disable once UnusedVariable
                //var filteredCount = await repo.GetCountWhereAsync(studentSearch);

                //var students = await repo.GetAllAsync();
                //foreach(var student in students) 
                //{
                //    student.Delete();
                //}

                //var input = new Input { Op1 = 1,  Op2 = 2 };
                //// ReSharper disable once UnusedVariable
                //var output = await UnitOfWorkContext<TestWebApiDataContext>.CurrentDataContext.AdderApiAsync(input);
            }
        }
    }
}