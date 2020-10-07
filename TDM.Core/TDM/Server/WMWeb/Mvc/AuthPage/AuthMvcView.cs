#nullable enable

using Supermodel.Presentation.WebMonk.Bootstrap4.Views;
using WebMonk.Rendering.Views;
using WMWeb.Mvc.Layouts;

namespace WMWeb.Mvc.AuthPage
{
    public class AuthMvcView : SimpleAuthMvcView 
    {
        protected override IMvcLayout GetLayout() => _layout;
        private static readonly IMvcLayout _layout = new EmptyModalMvcLayout();
    }
}
