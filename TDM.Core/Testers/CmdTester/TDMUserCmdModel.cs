﻿#nullable enable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Supermodel.DataAnnotations.Attributes;
using Supermodel.Presentation.Cmd.Models;
using Supermodel.ReflectionMapper;
using WMDomain.Entities;

namespace CmdTester
{
    public enum GenderEnum { Male, Female }
    
    public class TDMUserCmdModel : CmdModelForEntity<TDMUser>
    {
        #region Overrides
        protected override string LabelInternal => $"{FirstName} {LastName}";
        public override Task<T> MapToCustomAsync<T>(T other)
        {
            var user = CastToEntity(other);
            if (!string.IsNullOrEmpty(NewPassword.Value)) user.Password = NewPassword.Value;
            return base.MapToCustomAsync(other);
        }
        #endregion

        #region Properties
        [Required] public TextBoxCmdModel FirstName { get; set; } = new TextBoxCmdModel();
        [Required] public TextBoxCmdModel LastName { get; set; } = new TextBoxCmdModel();
        [Email, Required] public TextBoxCmdModel Username { get; set; } = new TextBoxCmdModel();
        
        [Required, NotRMapped] public DateCmdModel DOB { get; set; } = new DateCmdModel { DateTimeValue = DateTime.Today };
        [NotRMapped] public CheckboxCmdModel Admin {get; set; } = new CheckboxCmdModel();
        [Required, NotRMapped] public DropdownCmdModelUsingEnum<GenderEnum> Sex { get; set; } = new DropdownCmdModelUsingEnum<GenderEnum>();

        [SkipForDisplay, ForceRequiredLabel, NotRMapped, MustEqualTo(nameof(ConfirmPassword), ErrorMessage = "Passwords do not match")]
        public PasswordTextBoxCmdModel NewPassword { get; set; } = new PasswordTextBoxCmdModel();

        [SkipForDisplay, ForceRequiredLabel, NotRMapped, MustEqualTo(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        public PasswordTextBoxCmdModel ConfirmPassword { get; set; } = new PasswordTextBoxCmdModel();
        #endregion
    }
}