using System;
using System.IO;
using MvcCoreTester.Models;
using Supermodel.Mobile.CodeGen;

namespace ModelGeneratorTester
{
    class Program
    {
        static void Main()
        {
            var modelGenerator = new ModelGen(new[] { typeof(School).Assembly });
            var sb = modelGenerator.GenerateModels();
            File.WriteAllText(@"..\..\..\" + "Supermodel.Mobile.ModelsForRuntime.cs", sb.ToString());
            File.WriteAllText(@"..\..\..\..\XFormsTester\XFormsTester\Supermodel\Runtime\Supermodel.Mobile.ModelsForRuntime.cs", sb.ToString());

            Console.WriteLine("All done!");
        }
    }
}
