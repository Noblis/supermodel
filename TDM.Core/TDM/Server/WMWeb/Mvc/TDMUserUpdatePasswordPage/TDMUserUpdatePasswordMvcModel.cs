#nullable enable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Supermodel.DataAnnotations.Attributes;
using Supermodel.DataAnnotations.Validations;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.ReflectionMapper;
using WMDomain.Entities;

namespace WMWeb.Mvc.TDMUserUpdatePasswordPage
{
    public class TDMUserUpdatePasswordMvcModel : Bs4.MvcModelForEntity<TDMUser>
    {
        #region Overrides
        public override string Label => "N/A";
        public override Task<T> MapToCustomAsync<T>(T other)
        {
            #pragma warning disable RECS0017 // Possible compare of value type with 'null'
            if (other == null) throw new ArgumentNullException(nameof(other));
            #pragma warning restore RECS0017 // Possible compare of value type with 'null'
            var user = (TDMUser)(object)other;
            user.Password = NewPassword.Value;
            return base.MapToCustomAsync(other);
        }
        public override Task<ValidationResultList> ValidateAsync(ValidationContext validationContext)
        {
            //we don't want the domain-level validation (which is called by the base method) to be called here becaue we do not fill the object completely
            return Task.FromResult(new ValidationResultList());
        }
        #endregion

        #region Properties
        [Required, NotRMapped] public Bs4.PasswordTextBoxMvcModel OldPassword { get; set; } = new Bs4.PasswordTextBoxMvcModel { PlaceholderBehavior = Bs4.PasswordTextBoxMvcModel.PlaceholderBehaviorEnum.ForceNoPlaceholder };
        [Required, NotRMapped, MustEqualTo(nameof(ConfirmPassword), ErrorMessage = "Passwords do not Match")] public Bs4.PasswordTextBoxMvcModel NewPassword { get; set; } = new Bs4.PasswordTextBoxMvcModel { PlaceholderBehavior = Bs4.PasswordTextBoxMvcModel.PlaceholderBehaviorEnum.ForceNoPlaceholder };
        [Required, NotRMapped, MustEqualTo(nameof(NewPassword), ErrorMessage = "Passwords do not Match")] public Bs4.PasswordTextBoxMvcModel ConfirmPassword { get; set; } = new Bs4.PasswordTextBoxMvcModel { PlaceholderBehavior = Bs4.PasswordTextBoxMvcModel.PlaceholderBehaviorEnum.ForceNoPlaceholder };
        #endregion
    }
}
