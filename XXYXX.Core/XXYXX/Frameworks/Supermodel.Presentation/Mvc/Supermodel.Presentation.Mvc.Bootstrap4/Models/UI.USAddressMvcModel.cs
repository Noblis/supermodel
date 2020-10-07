#nullable enable

using System.ComponentModel.DataAnnotations;

namespace Supermodel.Presentation.Mvc.Bootstrap4.Models
{
    public class USAddressMvcModel : Bs4.ValueObjectMvcModel
    {
        #region Properties
        public Bs4.TextBoxMvcModel Street { get; set; } = new Bs4.TextBoxMvcModel();
        public Bs4.TextBoxMvcModel City { get; set; } = new Bs4.TextBoxMvcModel();
        public Bs4.TextBoxMvcModel State { get; set; } = new Bs4.TextBoxMvcModel();
        public Bs4.TextBoxMvcModel Zip { get; set; } = new Bs4.TextBoxMvcModel();
        #endregion
    }

    public class USAddressRequiredMvcModel : Bs4.ValueObjectMvcModel
    {
        #region Properties
        [Required] public Bs4.TextBoxMvcModel Street { get; set; } = new Bs4.TextBoxMvcModel();
        [Required] public Bs4.TextBoxMvcModel City { get; set; } = new Bs4.TextBoxMvcModel();
        [Required] public Bs4.TextBoxMvcModel State { get; set; } = new Bs4.TextBoxMvcModel();
        [Required] public Bs4.TextBoxMvcModel Zip { get; set; } = new Bs4.TextBoxMvcModel();
        #endregion
    }
}
