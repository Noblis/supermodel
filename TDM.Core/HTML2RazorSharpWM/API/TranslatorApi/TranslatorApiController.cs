#nullable enable

using System.Threading.Tasks;
using Supermodel.Presentation.WebMonk.Controllers.Api;

namespace HTML2RazorSharpWM.API.TranslatorApi
{
    public class TranslatorApiController : CommandApiController<TranslatorInput, TranslatorOutput>
    {
        #region Methods
        #pragma warning disable 1998
        protected override async Task<TranslatorOutput> ExecuteAsync(TranslatorInput input)
        #pragma warning restore 1998
        {
            return new TranslatorOutput
            {
                RazorSharp = $"This is the output from {input.Html}",
            };
        }
        #endregion
    }
}