using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcCoreTester.Models;
using Supermodel.ReflectionMapper;

namespace MvcCoreTester.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var entity = new Student();
            var mvcModel = await new StudentDetailMvcModel().MapFromAsync(entity);
            ModelState.AddModelError("FirstName", "Bad First Name");
            ModelState.AddModelError("LastName", "Bad Last Name");
            ModelState.AddModelError("SocialSecurity", "Bad SS# Name");
            // ReSharper disable once Mvc.ViewNotResolved
            return View(mvcModel);
        }
    }
}
