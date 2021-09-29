﻿#nullable enable

using System.ComponentModel.DataAnnotations;

namespace Supermodel.DataAnnotations.Attributes
{
    public class MustEqualToAttribute : ComparisonAttribute
    {
        #region Constructors
        public MustEqualToAttribute(string match, string errorMessage = "Field is Invalid") : base(match, errorMessage) {}
        #endregion

        #region Overrides
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var compResult = GetComparisonResult(value, validationContext);
            if (compResult == 0) return ValidationResult.Success!;
            return new ValidationResult(ErrorMessage, new [] { validationContext.MemberName! });
        }
        #endregion
    }
}