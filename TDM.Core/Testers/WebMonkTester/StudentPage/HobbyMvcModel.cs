#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Supermodel.DataAnnotations.Validations;

namespace WebMonkTester.StudentPage
{
    public class HobbyMvcModel : IAsyncValidatableObject
    {
        #region 
        public Task<ValidationResultList> ValidateAsync(ValidationContext validationContext)
        {
            var vrl = new ValidationResultList();
            if (string.IsNullOrEmpty(Name)) vrl.AddValidationResult(this, "Name is a required field for a hobby", x => x.Name);
            return Task.FromResult(vrl);
        }
        #endregion
        
        #region Properties
        public string Name { get; set; } = "";
        #endregion

    }
}
