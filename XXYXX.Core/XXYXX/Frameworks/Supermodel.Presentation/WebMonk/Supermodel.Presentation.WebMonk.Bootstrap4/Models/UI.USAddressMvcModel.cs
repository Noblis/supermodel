#nullable enable

using System.ComponentModel.DataAnnotations;

namespace Supermodel.Presentation.WebMonk.Bootstrap4.Models
{
    public static partial class Bs4
    {
        public class USAddressMvcModel : ValueObjectMvcModel
        {
            #region Properties
            public TextBoxMvcModel Street { get; set; } = new TextBoxMvcModel();
            public TextBoxMvcModel City { get; set; } = new TextBoxMvcModel();
            public TextBoxMvcModel State { get; set; } = new TextBoxMvcModel();
            public TextBoxMvcModel Zip { get; set; } = new TextBoxMvcModel();
            #endregion
        }

        public class USAddressRequiredMvcModel : ValueObjectMvcModel
        {
            #region Properties
            [Required] public TextBoxMvcModel Street { get; set; } = new TextBoxMvcModel();
            [Required] public TextBoxMvcModel City { get; set; } = new TextBoxMvcModel();
            [Required] public TextBoxMvcModel State { get; set; } = new TextBoxMvcModel();
            [Required] public TextBoxMvcModel Zip { get; set; } = new TextBoxMvcModel();
            #endregion
        }
    }
}
