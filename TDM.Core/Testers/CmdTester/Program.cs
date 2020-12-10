#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Supermodel.DataAnnotations.Validations.Attributes;
using Supermodel.Presentation.Cmd.ConsoleOutput;
using Supermodel.Presentation.Cmd.Models;

namespace CmdTester
{
    class Program
    {
        public class Student : CmdModel
        {
            [Required/*, DisplayName("↕")*/] public string FirstName { get; set; } = "Ilya";
            public string LastName { get; set; } = "Basin";
            public DateTime DateOfBirth { get; set; } = DateTime.Parse("12/18/1974");
            public int NumberOfKids { get; set; } = 2;
            public double GPA { get; set; } = 3.8;
            [ForceRequiredLabel] public bool? Jewish { get; set; } = true;
        }
        
        static void Main()
        {
            //var ilya = new StringWithColor("Ilya", ConsoleColor.Red);
            //var basin = new StringWithColor("Basin", ConsoleColor.Blue);

            //var ilyaBasin = ilya + " " + basin;
            //ilyaBasin.WriteToConsole();

            //Console.Write("Edit text: ");
            //var text = ConsoleExt.ReadPasswordLine();
            //Console.WriteLine(text);

            //var ilya = new Student();
            //CmdContext.ValidationResultList.Clear();
            //CmdContext.ValidationResultList.AddValidationResult(ilya, "Bad First Name", x => x.FirstName);
            //CmdContext.ValidationResultList.AddValidationResult(ilya, "Bad DOB", x => x.DateOfBirth);

            //CmdRender.DisplayForModel(ilya);
            //Console.WriteLine();
            //ilya = CmdRender.EditForModel(ilya);
            //Console.WriteLine();
            //CmdRender.DisplayForModel(ilya);

            var optionsList = new List<ConsoleExt.SelectListItem> 
            { 
                new ConsoleExt.SelectListItem("A", "Letter A"),
                new ConsoleExt.SelectListItem("B", "Letter B"),
                new ConsoleExt.SelectListItem("C", "Letter C"),
                new ConsoleExt.SelectListItem("D", "Letter D"),

            };
            var x = ConsoleExt.EditDropdownListForModel("A", optionsList, new FBColors(ConsoleColor.Blue));
            Console.WriteLine(x);
        }
    }
}
