#nullable enable

using Supermodel.Presentation.WebMonk.Bootstrap4.Models;

namespace WMWeb.Mvc.ToDoListPage
{
    public class ToDoListSearchMvcModel : Bs4.MvcModel
    {
        public Bs4.AutocompleteTextBoxMvcModel SearchTerm { get; set; } = new Bs4.AutocompleteTextBoxMvcModel("ToDoListAutocompleteApi");
    }

}
