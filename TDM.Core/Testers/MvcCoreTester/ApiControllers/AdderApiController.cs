using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Supermodel.Presentation.Mvc.Controllers.Api;

namespace MvcCoreTester.ApiControllers
{
    public class Input
    {
        public int Op1 { get; set; }
        public int Op2 { get; set; }
    }

    public class Output
    {
        public int PlusResult { get; set; }
        [JsonIgnore] public int MultiplyResult { get; set; }
    }
    
    [Authorize] 
    [Authorize(Roles = "Adder")]
    public class AdderApiController : CommandApiController<Input, Output>
    {
        protected override Task<Output> ExecuteAsync(Input input)
        {
            return Task.FromResult(new Output { PlusResult = input.Op1 + input.Op2, MultiplyResult = input.Op1 * input.Op2 } );
        }
    }
}