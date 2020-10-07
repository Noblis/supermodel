#nullable enable

using System;

namespace Supermodel.DataAnnotations.Validations.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HideLabelAttribute : Attribute 
    { 
        public bool KeepLabelSpace { get; set; }
    }
}