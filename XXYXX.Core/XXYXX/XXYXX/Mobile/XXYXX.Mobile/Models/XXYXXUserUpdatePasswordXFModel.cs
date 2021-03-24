#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Supermodel.ApiClient.Models;
using Supermodel.DataAnnotations.Attributes;
using Supermodel.DataAnnotations.Validations;
using Supermodel.Mobile.Runtime.Common.XForms.UIComponents;
using Supermodel.Mobile.Runtime.Common.XForms.ViewModels;
using Supermodel.ReflectionMapper;

namespace XXYXX.Mobile.Models
{
    public class XXYXXUserUpdatePasswordXFModel : XFModelForModel<XXYXXUserUpdatePassword>
    {    
        #region Validation
        public override async Task<ValidationResultList> ValidateAsync(ValidationContext validationContext)
        {
            var vr = await base.ValidateAsync(validationContext) ?? new ValidationResultList();
            //We only check if anything is filled in
            if (!string.IsNullOrEmpty(OldPassword.Text) || !string.IsNullOrEmpty(NewPassword.Text) || !string.IsNullOrEmpty(ConfirmPassword.Text))
            {
                if (NewPassword.Text != ConfirmPassword.Text) vr.AddValidationResult(this, "Passwords Must Equal", x => x.NewPassword, x=> x.ConfirmPassword);
                if (string.IsNullOrEmpty(OldPassword.Text)) vr.AddValidationResult(this, "The Old Password field is required", x => x.OldPassword);
                if (string.IsNullOrEmpty(NewPassword.Text)) vr.AddValidationResult(this, "The New Password field is required", x => x.NewPassword);
                if (string.IsNullOrEmpty(ConfirmPassword.Text)) vr.AddValidationResult(this, "The Confirm Password field is required", x => x.ConfirmPassword);
            }
            return vr;
        }
        #endregion

        #region Properties
        [ForceRequiredLabel] public TextBoxXFModel OldPassword { get; set; } = new TextBoxXFModel { TextEntry = { IsPassword = true } };
        [ForceRequiredLabel] public TextBoxXFModel NewPassword { get; set; } = new TextBoxXFModel { TextEntry = { IsPassword = true } };
        [ForceRequiredLabel, NotRMapped] public TextBoxXFModel ConfirmPassword { get; set; } = new TextBoxXFModel{ TextEntry = { IsPassword = true } };
        #endregion
    }
}