#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Domain.Entities;
using Supermodel.DataAnnotations.Attributes;
using Supermodel.DataAnnotations.Validations;
using Supermodel.Presentation.Mvc.Bootstrap4.Models;
using Supermodel.Presentation.Mvc.Models.Api;
using Supermodel.ReflectionMapper;

namespace WebMVC.Models
{
    public class XXYXXUserUpdatePasswordApiModel : ApiModelForEntity<XXYXXUser>
    {
        #region Overrides
        public override Task<T> MapToCustomAsync<T>(T other)
        {
            var user = CastToEntity(other);
            user.Password = NewPassword;
            return base.MapToCustomAsync(other);
        }
        public override Task<ValidationResultList> ValidateAsync(ValidationContext validationContext)
        {
            //we don't want to the domain-level validation to be called here (which called by the base method) because we don't fill that object completely
            var vr = new ValidationResultList();
            return Task.FromResult(vr);
        }
        #endregion
        
        #region Properties
        [Required, NotRMapped] public string OldPassword { get; set; } = "";
        [Required, NotRMapped] public string NewPassword { get; set; } = "";
        #endregion
    }    
    
    public class XXYXXUserUpdatePasswordMvcModel : Bs4.MvcModelForEntity<XXYXXUser>
    {
        #region Overrides
        public override string Label => "N/A";
        public override Task<T> MapToCustomAsync<T>(T other)
        {
            var user = CastToEntity(other);
            user.Password = NewPassword.Value;
            return base.MapToCustomAsync(other);
        }
        public override Task<ValidationResultList> ValidateAsync(ValidationContext validationContext)
        {
            //we don't want to the domain-level validation to be called here (which called by the base method) because we don't fill that object completely
            var vr = new ValidationResultList();
            return Task.FromResult(vr);
        }
        #endregion

        #region Properties
        [Required, NotRMapped] 
        public Bs4.PasswordTextBoxMvcModel OldPassword { get; set; } = new Bs4.PasswordTextBoxMvcModel { PlaceholderBehavior = Bs4.PasswordTextBoxMvcModel.PlaceholderBehaviorEnum.ForceNoPlaceholder };

        [Required, NotRMapped, MustEqualTo(nameof(ConfirmPassword), ErrorMessage = "Passwords do not match")]
        public Bs4.PasswordTextBoxMvcModel NewPassword { get; set; } = new Bs4.PasswordTextBoxMvcModel { PlaceholderBehavior = Bs4.PasswordTextBoxMvcModel.PlaceholderBehaviorEnum.ForceNoPlaceholder };

        [Required, NotRMapped, MustEqualTo(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        public Bs4.PasswordTextBoxMvcModel ConfirmPassword { get; set; } = new Bs4.PasswordTextBoxMvcModel { PlaceholderBehavior = Bs4.PasswordTextBoxMvcModel.PlaceholderBehaviorEnum.ForceNoPlaceholder };
        #endregion
    }
}
