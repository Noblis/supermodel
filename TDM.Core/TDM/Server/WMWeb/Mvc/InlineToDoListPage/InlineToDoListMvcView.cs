#nullable enable

using System.Linq;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.Presentation.WebMonk.Bootstrap4.Views;
using Supermodel.Presentation.WebMonk.Views.Interfaces;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WMDomain.Supermodel.Persistence;
using WMWeb.Mvc.InlineToDoItemPage;
using WMWeb.Mvc.ToDoListPage;

namespace WMWeb.Mvc.InlineToDoListPage
{
    public class InlineToDoListMvcView : EnhancedCRUDMvcView<ToDoListMvcModel, ToDoListSearchMvcModel, DataContext>
    {
        public override ListMode ListMode => ListMode.Simple;
        protected override string ListPageTitle => "My ToDo Lists";
        protected override string FilterTitle => "Search ToDos";
        //protected override string? DetailPageTitle => "ToDo Item";

        protected override IGenerateHtml RenderChildren(ToDoListMvcModel model)
        {
            return new Bs4.CRUDMultiColumnChildrenEditableList(model.ToDoItems.OrderBy(x => x.Completed.ValueBool).ThenBy(x => x.DueOn.DateTimeValue).ThenBy(x => x.Priority.SelectedEnum).ToList(), typeof(DataContext), typeof(InlineToDoItemMvcController), model.Id, "To Do Items");
        }

    }
    
    //public class InlineToDoListMvcView : EnhancedCRUDMvcViewBase<ToDoListMvcModel, ToDoListSearchMvcModel>
    //{
    //    public override IGenerateHtml RenderSearch(ToDoListSearchMvcModel model)
    //    {
    //        throw new InvalidOperationException();
    //    }

    //    public override IGenerateHtml RenderList(ListWithCriteria<ToDoListMvcModel, ToDoListSearchMvcModel> models, int totalCount)
    //    {
    //        return ApplyToDefaultLayout(new Tags
    //        { 
    //            new Bs4.CRUDSearchForm(models.Criteria, "Search ToDos", resetButton:true),
    //            new H2 { new Txt("My ToDo Lists") },
    //            new Bs4.Pagination(totalCount),
    //            new Bs4.CRUDList(models),
    //            new Bs4.Pagination(totalCount),
    //        });
    //    }

    //    public override IGenerateHtml RenderDetail(ToDoListMvcModel model)
    //    {
    //        return ApplyToDefaultLayout(new Tags
    //        { 
    //            new Bs4.CRUDEdit(model, model.Name.Value),
    //            new Bs4.CRUDMultiColumnChildrenEditableList(model.ToDoItems.OrderBy(x => x.Completed.ValueBool).ThenBy(x => x.DueOn.DateTimeValue).ThenBy(x => x.Priority.SelectedEnum).ToList(), typeof(DataContext), typeof(InlineToDoItemMvcController), model.Id, "To Do Items")
    //        });
    //    }
    //}
}
