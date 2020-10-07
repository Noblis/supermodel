#nullable enable

using Supermodel.Presentation.WebMonk.Bootstrap4.Views;
using Supermodel.Presentation.WebMonk.Views.Interfaces;
using WMDomain.Supermodel.Persistence;

namespace WMWeb.Mvc.TDMUserUpdatePasswordPage
{
    public class TDMUserUpdatePasswordMvcView : CRUDMvcView<TDMUserUpdatePasswordMvcModel, DataContext>
    {
        public override ListMode ListMode { get; } = ListMode.NoList;

        protected override bool ShowDefaultDetailPageTitle => false;
        protected override string? DetailPageTitle { get; } = "Update Password";
    }
    
    //public class TDMUserUpdatePasswordMvcView : CRUDMvcViewBase<TDMUserUpdatePasswordMvcModel>
    //{
    //    public override IGenerateHtml RenderList(List<TDMUserUpdatePasswordMvcModel> models)
    //    {
    //        throw new InvalidOperationException();
    //    }

    //    public override IGenerateHtml RenderDetail(TDMUserUpdatePasswordMvcModel model)
    //    {
    //        return ApplyToDefaultLayout(new Bs4.CRUDEdit(model, "Update Password", false, true));
    //    }
    //}
}
