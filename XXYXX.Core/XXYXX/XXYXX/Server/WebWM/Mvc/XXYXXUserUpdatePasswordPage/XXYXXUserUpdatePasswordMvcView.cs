#nullable enable

using Domain.Supermodel.Persistence;
using Supermodel.Presentation.WebMonk.Bootstrap4.Views;
using Supermodel.Presentation.WebMonk.Views.Interfaces;

namespace WebWM.Mvc.XXYXXUserUpdatePasswordPage
{
    public class XXYXXUserUpdatePasswordMvcView : CRUDMvcView<XXYXXUserUpdatePasswordMvcModel, DataContext>
    {
        public override ListMode ListMode { get; } = ListMode.NoList;

        protected override bool ShowDefaultDetailPageTitle => false;
        protected override string? DetailPageTitle { get; } = "Update Password";
    }
}
