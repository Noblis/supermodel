﻿#nullable enable

using System;
using System.IO;
using Supermodel.ReflectionMapper;

namespace Supermodel.Tooling.SolutionMaker.Cmd
{
    public class SolutionMakerParams : ISolutionMakerParams
    {
        #region Methods
        public static SolutionMakerParams ReadFromConsole()
        {
            var solutionMakerParams = new SolutionMakerParams();
            
            while(true)
            {
                solutionMakerParams.SolutionName = ReadSolutionName();
                solutionMakerParams.SolutionDirectory = ReadSolutionDirectory();
                solutionMakerParams.WebFramework = ReadWebFramework();
                solutionMakerParams.MobileApi = ReadMobileApi();
                solutionMakerParams.Database = ReadDataSource();
                
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Solution Name: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(solutionMakerParams.SolutionName);
                
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Solution Directory: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(solutionMakerParams.SolutionDirectory);
                
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Web Framework: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(solutionMakerParams.WebFramework.GetDescription());

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Mobile API: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(solutionMakerParams.MobileApi.GetDescription());

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Database: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(solutionMakerParams.Database.GetDescription());

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Is this correct? (y/n): ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                var input = Console.ReadLine();
                if (input != null)
                {
                    input = input.Trim().ToLower();
                    if (input == "y") return solutionMakerParams;
                }

                Console.WriteLine();
            }
        }

        public string CalculateFullPath() => SolutionMaker.CombineAndAdjustPaths(SolutionDirectory, SolutionName);
        #endregion

        #region Private Helper Methods
        private static DatabaseEnum ReadDataSource()
        {
            while(true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Select Database (1 - Sqlite, 2 - SQL Server): ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                var input = Console.ReadLine();
                if (input != null)
                {
                    input = input.Trim();
                    if (input == "1") return DatabaseEnum.Sqlite;
                    if (input == "2") return DatabaseEnum.SqlServer;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"'{input}' is not a valid option for a Database. Please re-enter.");
            }
        }

        private static MobileApiEnum ReadMobileApi()
        {
            while(true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Select Mobile API (1 - Xamarin.Forms, 2 - Platform's Native API): ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                var input = Console.ReadLine();
                if (input != null)
                {
                    input = input.Trim();
                    if (input == "1") return MobileApiEnum.XamarinForms;
                    if (input == "2") return MobileApiEnum.Native;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"'{input}' is not a valid option for a Mobile API. Please re-enter.");
            }
        }

        private static WebFrameworkEnum ReadWebFramework()
        {
            while(true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Select Web Framework (1 - WebMonk, 2 - MVC): ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                var input = Console.ReadLine();
                if (input != null)
                {
                    input = input.Trim();
                    if (input == "1") return WebFrameworkEnum.WebMonk;
                    if (input == "2") return WebFrameworkEnum.Mvc;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"'{input}' is not a valid option for a Web Framework. Please re-enter.");
            }
        }
        
        private static string ReadSolutionDirectory()
        {
            while(true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Solution Directory: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                var input = Console.ReadLine();
                if (input != null && IsValidDirectory(input)) return Path.GetFullPath(input);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"'{input}' is not a valid Solution Directory. Please re-enter.");
            }
        }
        private static bool IsValidDirectory(string dir)
        {
            try
            {
                Path.GetFullPath(dir);
                return Path.IsPathRooted(dir);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string ReadSolutionName()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Solution Name: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                var input = Console.ReadLine();
                if (input != null && IsValidSolutionName(input)) return input;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"'{input}' is not a valid Solution Name. Please re-enter.");
            }
        }
        private static bool IsValidSolutionName(string name)
        {
            //var match = Regex.Match(name, "^[a-zA-Z_$][a-zA-Z_$0-9]*$");
            //return match.Success;
            return Microsoft.CodeAnalysis.CSharp.SyntaxFacts.IsValidIdentifier(name);
        }
        #endregion
        
        #region Properties
        public string SolutionName { get; set; } = "";
        public string SolutionDirectory { get; set; } = "";

        public WebFrameworkEnum WebFramework { get; set; }
        public MobileApiEnum MobileApi { get; set; }

        public DatabaseEnum Database { get; set; }
        #endregion
    }
}
