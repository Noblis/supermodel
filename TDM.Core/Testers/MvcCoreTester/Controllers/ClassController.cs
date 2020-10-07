using MvcCoreTester.DataContexts;
using MvcCoreTester.Models;
using Supermodel.Presentation.Mvc.Controllers.Mvc;

namespace MvcCoreTester.Controllers
{
    public class ClassController : ChildCRUDController<Class, ClassMvcModel, Student, StudentController, DataContext>
    {
    }
}
