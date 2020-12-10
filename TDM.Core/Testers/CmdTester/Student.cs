#nullable enable

using System;
using System.ComponentModel.DataAnnotations;
using Supermodel.DataAnnotations.Validations.Attributes;
using Supermodel.Presentation.Cmd.Models;

namespace CmdTester
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
}
