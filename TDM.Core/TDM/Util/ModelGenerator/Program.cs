#nullable enable

using System;
using System.IO;
using Supermodel.Mobile.CodeGen;

namespace ModelGenerator
{
    class Program
    {
        static void Main()
        {
            var modelGenerator = new ModelGen(new[] { typeof(Web.Controllers.ToDoItemController).Assembly });
            //var modelGenerator = new ModelGen(new[] { typeof(WMWeb.Mvc.ToDoItemPage.ToDoItemMvcController).Assembly });
            var sb = modelGenerator.GenerateModels();
            var code = sb.ToString();
            File.WriteAllText(@"..\..\..\Supermodel.Mobile.ModelsForRuntime.cs", code);
            File.WriteAllText(@"..\..\..\..\..\Mobile\TDM.Mobile.Online\Supermodel\Runtime\Supermodel.Mobile.ModelsForRuntime.cs", code);
            File.WriteAllText(@"..\..\..\..\..\Mobile\TDM.Mobile.Offline\Supermodel\Runtime\Supermodel.Mobile.ModelsForRuntime.cs", code);
            File.WriteAllText(@"..\..\..\..\..\Server\ApiClientCmd\Supermodel\Runtime\Supermodel.Mobile.ModelsForRuntime.cs", code);

            Console.WriteLine("All done!");
        }
    }
}
