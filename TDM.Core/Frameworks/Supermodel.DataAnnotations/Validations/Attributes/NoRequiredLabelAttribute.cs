#nullable enable

using System;

namespace Supermodel.DataAnnotations.Validations.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class NoRequiredLabelAttribute : Attribute { }
}