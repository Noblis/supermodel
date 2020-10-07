﻿#nullable enable

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Supermodel.DataAnnotations.Validations;
using Supermodel.ReflectionMapper;

namespace Supermodel.Presentation.Mvc.Bootstrap4.Models
{
    public static partial class Bs4
    {
        public abstract class ValueObjectMvcModel : MvcModel, IRMapperCustom, IValidatableObject
        {
            #region IRMapperCustom implemetation
            public virtual Task MapFromCustomAsync<T>(T other)
            {
                return this.MapFromCustomBaseAsync(other);
            }
            public virtual Task<T> MapToCustomAsync<T>(T other)
            {
                return this.MapToCustomBaseAsync(other);
            }
            #endregion

            #region IValidatableObject implemetation
            public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                return new ValidationResultList();
            }
            #endregion
        }
    }
}
