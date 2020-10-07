#nullable enable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;

namespace WebMonkTester.StudentPage
{
    public class AddressMvcModel : Bs4.MvcModel //: IEditorTemplate
    {
        #region IEditorTemplate implementation 
        //public override IGenerateHtml EditorTemplate(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue, object? attributes = null)
        //{
        //    return new Tags
        //    {
        //        new Div(new { @class="form-group"})
        //        {
        //            Render.LabelFor(this, x => x.Street),
        //            Render.EditorFor(this, x => x.Street, new { @class="form-control" }),
        //            Render.ValidationMessageFor(this, x => x.Street, new { @class="invalid-feedback d-block" }),
        //        },
        //        new Div(new { @class="form-group"})
        //        {
        //            Render.LabelFor(this, x => x.City),
        //            Render.EditorFor(this, x => x.City, new { @class="form-control" }),
        //            Render.ValidationMessageFor(this, x => x.City, new { @class="invalid-feedback d-block" }),
        //        },
        //        new Div(new { @class="form-group"})
        //        {
        //            Render.LabelFor(this, x => x.State),
        //            Render.EditorFor(this, x => x.State, new { @class="form-control" }),
        //            Render.ValidationMessageFor(this, x => x.City, new { @class="invalid-feedback d-block" }),
        //        },
        //        new Div(new { @class="form-group"})
        //        {
        //            Render.LabelFor(this, x => x.Zip),
        //            Render.EditorFor(this, x => x.Zip, new { @class="form-control" }),
        //            Render.ValidationMessageFor(this, x => x.Zip, new { @class="invalid-feedback d-block" }),
        //        },
        //    };
        //}
        #endregion

        #region Properties
        [DisplayName("Za Street")] public Bs4.TextBoxMvcModel Street{ get; set; } = new Bs4.TextBoxMvcModel();
        public  Bs4.TextBoxMvcModel  City { get; set; } = new Bs4.TextBoxMvcModel();
        public  Bs4.TextBoxMvcModel  State { get; set; } = new Bs4.TextBoxMvcModel();
        [Required] public  Bs4.TextBoxMvcModel  Zip { get; set; } = new Bs4.TextBoxMvcModel();
        #endregion
    }
}
