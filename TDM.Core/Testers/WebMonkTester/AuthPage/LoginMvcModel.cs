#nullable enable

using System.ComponentModel.DataAnnotations;

namespace WebMonkTester.AuthPage
{
    public class LoginMvcModel
    {
        [Required] public string Username { get; set; } = "";
        [Required, DataType(DataType.Password)] public string Password { get; set; } = "";
    }
}
