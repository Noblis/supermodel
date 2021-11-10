using Supermodel.Mobile.Runtime.Common.DataContext.Core;
using Supermodel.Mobile.Runtime.Common.Repository;
using Supermodel.Mobile.Runtime.Common.UnitOfWork;
using Supermodel.ApiClient.Models;
using System;
using System.Threading.Tasks;
using XFormsTester.Supermodel.Persistence;

namespace WebServicesCmdClientTester
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();

            //var authHeaderGenerator = new BasicAuthHeaderGenerator("ilya.basin@gmail.org", "0");
            var authHeaderGenerator = new SecureAuthHeaderGenerator("ilya.basin@gmail.org", "0", Array.Empty<byte>());

            await using (new UnitOfWork<TestWebApiDataContext>())
            {
                UnitOfWorkContext.AuthHeader = authHeaderGenerator.CreateAuthHeader();
                
                var repo = RepoFactory.Create<Student>();
                // ReSharper disable once UnusedVariable
                var count = await repo.GetCountAllAsync();

                var studentSearch = new StudentSearch { Term = "Ilya" };
                // ReSharper disable once UnusedVariable
                var filteredStudents = await repo.GetWhereAsync(studentSearch);
                // ReSharper disable once UnusedVariable
                var filteredCount = await repo.GetCountWhereAsync(studentSearch);

                var students = await repo.GetAllAsync();
                foreach(var student in students) 
                {
                    student.Delete();
                }

                var input = new Input { Op1 = 1,  Op2 = 2 };
                // ReSharper disable once UnusedVariable
                var output = await UnitOfWorkContext<TestWebApiDataContext>.CurrentDataContext.AdderApiAsync(input);
            }

            // ReSharper disable once NotAccessedVariable
            DelayedModels<Student> delayedStudents;
            await using (new UnitOfWork<TestWebApiDataContext>())
            {
                UnitOfWorkContext.AuthHeader = authHeaderGenerator.CreateAuthHeader();

                var repo = RepoFactory.Create<Student>();
                
                var ilya = new Student
                {
                    FirstName = "Ilya",
                    LastName = "Basin",
                    Age = 44,
                    SocialSecurity = "123",
                    Gender = GenderEnum.Male,
                    School = new School { Id = 1, Name = "MIT" },
                    DateOfBirthday = DateTime.Parse("12/18/1974"),
                    SecurityClearance = true
                };
                ilya.Add();
                
                var andrew = new Student
                {
                    FirstName = "Andrew",
                    LastName = "Basin",
                    Age = 15,
                    SocialSecurity = "4567",
                    Gender = GenderEnum.Male,
                    School = new School { Id = 1, Name = "MIT" },
                    DateOfBirthday = DateTime.Parse("03/27/2004"),
                    SecurityClearance = true
                };
                andrew.Add();

                repo.DelayedGetAll(out delayedStudents);
            }
        }
    }
}
