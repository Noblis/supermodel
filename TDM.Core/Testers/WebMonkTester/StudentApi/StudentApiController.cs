#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;
using WebMonk.Context;
using WebMonk.Filters;
using WebMonk.HttpRequestHandlers.Controllers;
using WebMonk.Results;

namespace WebMonkTester.StudentApi
{
    [Authorize]
    public class StudentApiController : ApiController
    {
        public Task<ActionResult> GetAllAsync()
        {
            var model1 = new StudentApiModel {  FirstName ="Ilya", LastName = "Basin", GPA = 3.1 };
            var model2 = new StudentApiModel {  FirstName ="Mariya", LastName = "Basin", GPA = 3.2 };
            var model3 = new StudentApiModel {  FirstName ="Andrew", LastName = "Basin", GPA = 3.3 };
            var model4 = new StudentApiModel {  FirstName ="Natalie", LastName = "Basin", GPA = 3.4 };
            var models = new List<StudentApiModel> { model1, model2, model3, model4 };
            return Task.FromResult((ActionResult)new JsonApiResult(models));
        }
        public Task<ActionResult> PostAsync(int id, StudentApiModel student)
        {
            var anotherStudent = new StudentApiModel();
            TryUpdateModelAsync(anotherStudent);

            // ReSharper disable once UnusedVariable
            var vrl = HttpContext.Current.ValidationResultList;
            
            var model1 = new StudentApiModel {  FirstName ="Ilya", LastName = "Basin", GPA = 3.1 };
            var model2 = new StudentApiModel {  FirstName ="Mariya", LastName = "Basin", GPA = 3.2 };
            var model3 = new StudentApiModel {  FirstName ="Andrew", LastName = "Basin", GPA = 3.3 };
            var model4 = new StudentApiModel {  FirstName ="Natalie", LastName = "Basin", GPA = 3.4 };
            var models = new List<StudentApiModel> { model1, model2, model3, model4 };
            return Task.FromResult((ActionResult)new JsonApiResult(models));
        }
    }
}