using System;
using System.IO;

namespace Supermodel.Tooling.SolutionMaker.Cmd
{
    class Program
    {
        static void Main()
        {
            try
            {
                //Un-comment and run this once to refresh the solution zip
                //SolutionMaker.CreateSnapshot(@"..\..\..\..\..\..\..\XXYXX.Core\XXYXX", @"..\..\..\");
                //Console.WriteLine($"{SolutionMaker.ZipFileName} created successfully!");
                //return;

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Supermodel.Core Solution Maker");
                Console.WriteLine("Version: Beta 2.0");

                Console.WriteLine("Please Enter Solution Parameters");
                var solutionMakerParams = SolutionMakerParams.ReadFromConsole();

                //Comment this out for production, this is to speed up development and incremental testing
                //var solutionMakerParams = new SolutionMakerParams
                //{
                //    SolutionName = "XYX",
                //    SolutionDirectory = @"C:\Users\ilyabasin\Documents\Projects",
                //    WebFramework = WebFrameworkEnum.Mvc,
                //    MobileApi = MobileApiEnum.Native,
                //    Database = DatabaseEnum.SqlServer
                //};
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
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Deleting {path}...");
                    Directory.Delete(Path.Combine(solutionMakerParams.SolutionDirectory, solutionMakerParams.SolutionName), true);
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Generating new solution {solutionMakerParams.SolutionName}...");
                SolutionMaker.CreateSupermodelShell(solutionMakerParams);
                
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Solution {solutionMakerParams.SolutionName} generated successfully!");
               
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(ex);
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }
    }
}
