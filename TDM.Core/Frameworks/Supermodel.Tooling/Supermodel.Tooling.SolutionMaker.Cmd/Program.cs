using System;

namespace Supermodel.Tooling.SolutionMaker.Cmd
{
    class Program
    {
        static void Main()
        {
            //Un-comment and run this once to refresh the solution zip
            //SolutionMaker.CreateSnapshot(@"..\..\..\..\..\..\..\XXYXX.Core\XXYXX", @"..\..\..\");
            //Console.WriteLine("All done!");
            //return;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Supermodel.Core Solution Maker. Version Beta 2.0");
            Console.WriteLine("Please Enter Solution Parameters");
            
            var solutionMakerParams = SolutionMakerParams.ReadFromConsole();
            SolutionMaker.CreateSupermodelShell(solutionMakerParams);
        }
    }
}
