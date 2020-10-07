#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;
using WebMonk.Context;
using WebMonk.Extensions;
using WebMonk.Filters;
using WebMonk.HttpRequestHandlers.Controllers;
using WebMonk.Results;

namespace WebMonkTester.StudentPage
{
    public class StudentMvcController : MvcController
    {
        public LocalRedirectResult GetRedirectToDetail()
        {
            //return RedirectToLocal("/Student/Detail/1?a=1");
            HttpContext.Current.TempData["SomeStuff"] = "Ignore";
            return RedirectToAction<StudentMvcController>(x => x.GetDetail(1));
        }
        
        //[Authorize]
        //[NonAction]
        public HtmlResult GetList1()
        {
            //var model1 = new StudentMvcModel { FirstName ="Ilya", LastName = "Basin", GPA = 3.1 };
            //var model2 = new StudentMvcModel { FirstName ="Mariya", LastName = "Basin", GPA = 3.2 };
            //var model3 = new StudentMvcModel { FirstName ="Andrew", LastName = "Basin", GPA = 3.3 };
            //var model4 = new StudentMvcModel { FirstName ="Natalie", LastName = "Basin", GPA = 3.4 };
            //var models = new List<StudentMvcModel> { model1, model2, model3, model4 };

            var models = new List<StudentMvcModel>();
            return new StudentMvcView().RenderList(models).ToHtmlResult();
        }

        public Task<HtmlResult> GetList2Async()
        {
            //var model1 = new StudentMvcModel { FirstName ="Ilya", LastName = "Basin", GPA = 3.1 };
            //var model2 = new StudentMvcModel { FirstName ="Mariya", LastName = "Basin", GPA = 3.2 };
            //var model3 = new StudentMvcModel { FirstName ="Andrew", LastName = "Basin", GPA = 3.3 };
            //var model4 = new StudentMvcModel { FirstName ="Natalie", LastName = "Basin", GPA = 3.4 };
            //var models = new List<StudentMvcModel> { model1, model2, model3, model4 };

            var models = new List<StudentMvcModel>();
            return Task.FromResult(new StudentMvcView().RenderList(models).ToHtmlResult());
        }

        [Authorize("Admin", "Power User")]
        public HtmlResult GetDetail(int id)
        {
            HttpContext.Current.Session["Test"] = "TST46";
            HttpContext.Current.TempData["Test"] = "TST48";
            var model = new StudentMvcModel(); // { FirstName ="Ilya", LastName = "Basin", GPA = 3.1 };
            return new StudentMvcView().RenderDetail(model).ToHtmlResult();
        }
        //public HtmlResult GetDetail()
        //{
        //    HttpContext.Current.Session["Test"] = "TST46";
        //    HttpContext.Current.TempData["Test"] = "TST48";
        //    var model = new StudentMvcModel { FirstName = "Ilya", LastName = "Basin", GPA = 3.1 };
        //    return new StudentMvcView().RenderDetail(model).ToHtmlResult();
        //}

        [Authorize("Admin", "Power User")]
        //public HtmlResult PostDetail(int id, StudentMvcModel student)
        public async Task<HtmlResult> PostDetailAsync(int id, StudentMvcModel student)
        {
            // ReSharper disable UnusedVariable
            var x = HttpContext.Current.Session["Test"];
            var y = HttpContext.Current.TempData["Test"];
            // ReSharper restore UnusedVariable

            //var anotherStudent = new StudentMvcModel();
            //await TryUpdateModelAsync(anotherStudent);

            // ReSharper disable once UnusedVariable
            var valueProviders = await HttpContext.Current.ValueProviderManager.GetValueProvidersListAsync();

            //var gpaList = valueProviders.GetValueOrDefault<List<double>?>("GPA");
            //var gpaArray = valueProviders.GetValueOrDefault<double[]?>("GPA");

            //var firstNameList = valueProviders.GetValueOrDefault<List<string>?>("FirstName");
            //var firstNameArray = valueProviders.GetValueOrDefault<string[]?>("FirstName");
            
            // ReSharper disable once UnusedVariable
            var errors = HttpContext.Current.ValidationResultList;

            return new StudentMvcView().RenderDetail(student).ToHtmlResult();
        }
    }
}
