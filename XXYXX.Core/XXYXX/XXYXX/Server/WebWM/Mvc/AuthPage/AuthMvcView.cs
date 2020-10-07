#nullable enable

using Supermodel.Presentation.WebMonk.Bootstrap4.Views;
using WebMonk.Rendering.Views;
using WebWM.Mvc.Layouts;

namespace WebWM.Mvc.AuthPage
{
    public class AuthMvcView : SimpleAuthMvcView 
    {
        protected override IMvcLayout GetLayout() => _layout;
        private static readonly IMvcLayout _layout = new EmptyModalMvcLayout();
    }
}