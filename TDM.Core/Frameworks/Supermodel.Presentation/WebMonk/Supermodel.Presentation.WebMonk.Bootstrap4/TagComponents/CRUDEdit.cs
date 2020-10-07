#nullable enable

using Supermodel.Presentation.WebMonk.Models;
using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Views;

// ReSharper disable once CheckNamespace
namespace Supermodel.Presentation.WebMonk.Bootstrap4.Models
{
    public static partial class Bs4
    {
        public class CRUDEdit : HtmlSnippet
        {
            #region Constructors
            public CRUDEdit(IViewModelForEntity model, string pageTitle, bool readOnly = false, bool skipBackButton = false) :
                this(model, new Txt(pageTitle), readOnly, skipBackButton)
            { }

            public CRUDEdit(IViewModelForEntity model, IGenerateHtml? pageTitle = null, bool readOnly = false, bool skipBackButton = false)
            {
                AppendAndPush(new CRUDEditContainer(model, pageTitle, readOnly, skipBackButton));
                Append(Render.EditorForModel(model).DisableAllControlsIf(readOnly));
                Pop<CRUDEditContainer>();
            }
            #endregion
        }
    }
}
