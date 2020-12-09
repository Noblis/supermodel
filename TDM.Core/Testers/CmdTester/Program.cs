#nullable enable

using System;
using Supermodel.Presentation.Cmd.ConsoleOutput;
using Supermodel.Presentation.Cmd.Models;
using Supermodel.Presentation.Cmd.Rendering;

namespace CmdTester
{
    class Program
    {
        public class Student : CmdModel
        {
            public string FirstName { get; set; } = "Ilya";
            public string LastName { get; set; } = "Basin";
            public DateTime DateOfBirth { get; set; } = DateTime.Parse("12/18/1974");
            public int NumberOfKids { get; set; } = 2;
            public double GPA { get; set; } = 3.8;
        }
        
        static void Main()
        {
            var ilya = new StringWithColor("Ilya", ConsoleColor.Red);
            var basin = new StringWithColor("Basin", ConsoleColor.Blue);

            var ilyaBasin = ilya + " " + basin;
            ilyaBasin.WriteToConsole();

            //Console.Write("Edit text: ");
            //var text = ConsoleExt.ReadPasswordLine();
            //Console.WriteLine(text);

            //var ilya = new Student();
            //ValidationContext.ValidationResultList.Clear();
            //ValidationContext.ValidationResultList.AddValidationResult(ilya, "Bad First Name", x => x.FirstName);
            //ValidationContext.ValidationResultList.AddValidationResult(ilya, "Bad DOB", x => x.DateOfBirth);
            
            //CmdRender.DisplayForModel(ilya);
            //Console.WriteLine();
            //ilya = CmdRender.EditForModel(ilya);
            //Console.WriteLine();
            //CmdRender.DisplayForModel(ilya);
        }
    }
}
