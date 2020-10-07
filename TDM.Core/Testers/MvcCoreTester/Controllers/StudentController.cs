using System.Linq;
using Microsoft.AspNetCore.Authorization;
using MvcCoreTester.DataContexts;
using MvcCoreTester.Models;
using Supermodel.Presentation.Mvc.Controllers.Mvc;

namespace MvcCoreTester.Controllers
{
    [Authorize(Roles = "Adder")]
    //public class StudentController : EnhancedCRUDController<Student, StudentDetailMvcModel, StudentSearchMvcModel, DataContext>
    public class StudentController : EnhancedCRUDController<Student, StudentDetailMvcModel, StudentListMvcModel, StudentSearchMvcModel, DataContext>
    //public class StudentController : CRUDController<Student, StudentDetailMvcModel, DataContext>
    {
        //protected override async Task<ActionResult> AfterCreateAsync(long id, Student entityItem, StudentDetailMvcModel mvcModelItem)
        //{
        //    TempData.Super().NextPageModalMessage = "Created";
        //    //return base.AfterCreateAsync(id, entityItem, mvcModelItem);
        //    await UnitOfWorkContext.FinalSaveChangesAsync();
        //    return StayOnDetailScreen(entityItem.Id);
        //}
        //protected override Task<ActionResult> AfterUpdateAsync(long id, Student entityItem, StudentDetailMvcModel mvcModelItem)
        //{
        //    TempData.Super().NextPageModalMessage = "Updated";
        //    return base.AfterCreateAsync(id, entityItem, mvcModelItem);
        //}

        //protected override Task<ActionResult> AfterDeleteAsync(long id, Student entityItem)
        //{
        //    TempData.Super().NextPageModalMessage = "Deleted";
        //    return base.AfterDeleteAsync(id, entityItem);
        //}

        protected override IQueryable<Student> ApplySearchBy(IQueryable<Student> items, StudentSearchMvcModel searchBy)
        {
            if (!string.IsNullOrEmpty(searchBy.FirstName.Value)) items = items.Where(x => x.FirstName.ToLower().Contains(searchBy.FirstName.Value.ToLower()));
            if (!string.IsNullOrEmpty(searchBy.LastName.Value)) items = items.Where(x => x.LastName.ToLower().Contains(searchBy.LastName.Value.ToLower()));
            if (searchBy.Gender.SelectedEnum.HasValue) items = items.Where(x => x.Gender == searchBy.Gender.SelectedEnum);
            if (searchBy.School.SelectedId.HasValue) items = items.Where(x => x.School.Id == searchBy.School.SelectedId);
            if (searchBy.ClearanceRequired.ValueBool) items = items.Where(x => x.SecurityClearance.HasValue && x.SecurityClearance.Value);
            if (!string.IsNullOrEmpty(searchBy.MinAge.Value)) items = items.Where(x => x.Age >= int.Parse(searchBy.MinAge.Value));
            if (!string.IsNullOrEmpty(searchBy.MaxAge.Value)) items = items.Where(x => x.Age <= int.Parse(searchBy.MaxAge.Value));
            return items;
        }

        protected override IOrderedQueryable<Student> ApplySortBy(IQueryable<Student> items, string sortBy)
        {
            if (sortBy?.ToLower() == "school") return items.OrderBy(x => x.School.Name);
            if (sortBy?.ToLower() == "-school") return items.OrderByDescending(x => x.School.Name);
            return base.ApplySortBy(items, sortBy);
        }

        protected override bool IsGoingToList()
        {
            return true;
        }
    }
}