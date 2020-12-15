﻿#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Supermodel.DataAnnotations.Validations.Attributes;
using Supermodel.Presentation.Cmd.Models;
using Supermodel.ReflectionMapper;
using WMDomain.Entities;

namespace CmdTester
{
    public class TDMUserCmdModel : CmdModelForEntity<TDMUser>
    {
        #region Overrides
        protected override string LabelInternal => $"{FirstName} {LastName}";
        public override Task<T> MapToCustomAsync<T>(T other)
        {
            var user = CastToEntity(other);
            if (!string.IsNullOrEmpty(NewPassword)) user.Password = NewPassword;
            return base.MapToCustomAsync(other);
        }
        #endregion

        #region Properties
        [Required] public TextBoxCmdModel FirstName { get; set; } = new TextBoxCmdModel();
        [Required] public TextBoxCmdModel LastName { get; set; } = new TextBoxCmdModel();
        [Email, Required] public TextBoxCmdModel Username { get; set; } = new TextBoxCmdModel();

        [ForceRequiredLabel, NotRMapped, MustEqualTo(nameof(ConfirmPassword), ErrorMessage = "Passwords do not match")]
        public string NewPassword { get; set; } = "";

        [ForceRequiredLabel, NotRMapped, MustEqualTo(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = "";
        #endregion
    }
}