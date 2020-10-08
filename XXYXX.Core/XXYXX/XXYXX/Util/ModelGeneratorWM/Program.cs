#nullable enable

using Supermodel.Mobile.CodeGen;
using System;
using System.IO;

namespace ModelGeneratorWM
{
    class Program
    {
        static void Main()
        {
            var modelGenerator = new ModelGen(new[] { typeof(WebWM.Mvc.XXYXXUserUpdatePasswordPage.XXYXXUserUpdatePasswordMvcController).Assembly });
            var sb = modelGenerator.GenerateModels();
            var code = sb.ToString();

            File.WriteAllText(@"..\..\..\..\..\Server\BatchApiClientWM\Supermodel\ModelsForRuntime\Supermodel.Mobile.ModelsForRuntime.cs", code);
            File.WriteAllText(@"..\..\..\..\..\Mobile\XXYXX.Mobile\Supermodel\ModelsForRuntime\Supermodel.Mobile.ModelsForRuntime.cs", code);

            Console.WriteLine("All done!");
        }
    }
}
