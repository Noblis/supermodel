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
            
            //Console.WriteLine("Please Enter Solution Parameters");
            //var solutionMakerParams = SolutionMakerParams.ReadFromConsole();

            //Comment this out for production, this is to speed up development and incremental testing
            var solutionMakerParams = new SolutionMakerParams
            {
                SolutionName = "XYX",
                SolutionDirectory = @"C:\Users\ilyabasin\Documents\Projects",
                WebFramework = WebFrameworkEnum.WebMonk,
                MobileApi = MobileApiEnum.XamarinForms,
                Database = DatabaseEnum.Sqlite
            };
            //End of comment out for production
            
            var path = solutionMakerParams.CalculateFullPath();
            if (Directory.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("Directory ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(path);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(" already exists.\nWould you like to replace it? (y/n): ");
                
                var input = Console.ReadLine();
                if (input == null) return;
                input = input.Trim().ToLower();
                if (input != "y") return;
                Directory.Delete(Path.Combine(solutionMakerParams.SolutionDirectory, solutionMakerParams.SolutionName), true);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Generating solution {solutionMakerParams.SolutionName}. Please standby...");
            SolutionMaker.CreateSupermodelShell(solutionMakerParams);
            
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine($"Solution {solutionMakerParams.SolutionName} generated successfully");
        }
    }
}
