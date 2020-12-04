#nullable enable

using System.Threading.Tasks;
using WebMonk.Extensions;
using WebMonk.HttpRequestHandlers.Controllers;
using WebMonk.Results;

namespace HTML2RazorSharpWM.MainPage
{
    public class MainMvcController : MvcController
    {
        public Task<ActionResult> GetIndexAsync()
        {
            return Task.FromResult<ActionResult>(new MainMvcView().RenderIndex().ToHtmlResult());
            //return Task.FromResult<ActionResult>(new ProspectSignUpMvcView().RenderProspectSignUp(new ProspectSignUpMvcModel()).ToHtmlResult());

        }
    }
}