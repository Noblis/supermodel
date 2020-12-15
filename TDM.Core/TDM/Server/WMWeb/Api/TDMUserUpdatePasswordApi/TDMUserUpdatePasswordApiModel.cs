#nullable enable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Supermodel.DataAnnotations.Attributes;
using Supermodel.DataAnnotations.Validations;
using Supermodel.Presentation.WebMonk.Models.Api;
using Supermodel.ReflectionMapper;
using WMDomain.Entities;

namespace WMWeb.Api.TDMUserUpdatePasswordApi
{
    public class TDMUserUpdatePasswordApiModel : ApiModelForEntity<TDMUser>
    {
        #region Overrides
        public override Task<T> MapToCustomAsync<T>(T other)
        {
            #pragma warning disable RECS0017 // Possible compare of value type with 'null'
            if (other == null) throw new ArgumentNullException(nameof(other));
            #pragma warning restore RECS0017 // Possible compare of value type with 'null'
            var user = (TDMUser)(object)other;
            user.Password = NewPassword;
            return base.MapToCustomAsync(other);
        }
        public override Task<ValidationResultList> ValidateAsync(ValidationContext validationContext)
        {
            //we don't want the domain-level validation (which is called by the base method) to be called here because we do not fill the object completely
            return Task.FromResult(new ValidationResultList());
        }
        #endregion

        #region Properties
        [Required, NotRMapped] public string OldPassword { get; set; } = "";
        [Required, NotRMapped, MustEqualTo(nameof(ConfirmPassword), ErrorMessage = "Passwords do not Match")] public string NewPassword { get; set; } = "";
        [Required, NotRMapped, MustEqualTo(nameof(NewPassword), ErrorMessage = "Passwords do not Match")] public string ConfirmPassword { get; set; } = "";
        #endregion
    }
}
