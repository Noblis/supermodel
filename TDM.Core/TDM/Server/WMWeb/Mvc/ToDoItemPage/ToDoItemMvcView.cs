#nullable enable

using Supermodel.Presentation.WebMonk.Bootstrap4.Views;

namespace WMWeb.Mvc.ToDoItemPage
{
    public class ToDoItemMvcView : ChildCRUDMvcView<ToDoItemMvcModel> { }
    
    //public class ToDoItemMvcView : ChildCRUDMvcViewBase<ToDoItemMvcModel>
    //{
    //    public override IGenerateHtml RenderDetail(ToDoItemMvcModel model)
    //    {
    //        return ApplyToDefaultLayout(new Bs4.CRUDEdit(model));
    //    }
    //}
}
