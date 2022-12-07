#nullable enable

using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using WMDomain.Entities;
using WMDomain.Supermodel.Persistence;
using WMWeb.Api.ToDoListAutocompleteApi;

namespace WMWeb.Mvc.ToDoListPage
{
    public class ToDoListSearchMvcModel : Bs4.MvcModel
    {
        public Bs4.AutocompleteTextBoxMvcModel<ToDoList, ToDoListAutocompleteApiController, DataContext> SearchTerm { get; set; } = new ();
    }

}
