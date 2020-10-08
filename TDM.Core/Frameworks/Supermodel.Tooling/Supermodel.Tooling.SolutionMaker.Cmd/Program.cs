using System;
using System.IO;

namespace Supermodel.Tooling.SolutionMaker.Cmd
{
    class Program
    {
        static void Main()
        {
            //Un-comment and run this once to refresh the solution zip
            //SolutionMaker.CreateSnapshot(@"..\..\..\..\..\..\..\XXYXX.Core\XXYXX", @"..\..\..\");
            //Console.WriteLine($"{SolutionMaker.ZipFileName} created successfully!");
            //return;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Supermodel.Core Solution Maker. Version Beta 2.0");
            
            //var solutionMakerParams = SolutionMakerParams.ReadFromConsole();
            //Console.WriteLine("Please Enter Solution Parameters");

            //Comment this out for production, this is to speed up development and incremental testing
            var solutionMakerParams = new SolutionMakerParams
            {
                SolutionName = "XYX",
                SolutionDirectory = @"C:\Users\ilyabasin\Documents\Projects",
                WebFramework = WebFrameworkEnum.WebMonk,
                MobileApi = MobileApiEnum.Native,
                Database = DatabaseEnum.Sqlite
            };
            
            var path = solutionMakerParams.CalculateFullPath();
            if (Directory.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("Directory ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(path);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(" already exists.\nWould you like to replace it? (y/n)");
                
                var input = Console.ReadLine();
                if (input == null) return;
                input = input.Trim().ToLower();
                if (input != "y") return;
                Directory.Delete(Path.Combine(solutionMakerParams.SolutionDirectory, solutionMakerParams.SolutionName), true);
            }

            SolutionMaker.CreateSupermodelShell(solutionMakerParams);
            
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }
}
