using System.ComponentModel.DataAnnotations;

#nullable enable

namespace WebMonkTester.StudentApi
{
    public class StudentApiModel
    {
        [Required] public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public double GPA { get; set; }
    }
}
