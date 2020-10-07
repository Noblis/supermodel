﻿#nullable enable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Supermodel.DataAnnotations.Validations;
using Supermodel.Persistence.Entities;
using Supermodel.ReflectionMapper;

namespace Supermodel.Presentation.WebMonk.Models.Api
{
    public abstract class ApiModelForEntity<TEntity> : ApiModelForAnyEntity where TEntity : class, IEntity, new()
    {
        #region Validation
        //The default implementation just grabs domain model validation but this can be overriden
        public override async Task<ValidationResultList> ValidateAsync(ValidationContext validationContext)
        {
            var tempEntityForValidation = await CreateTempValidationEntityAsync().ConfigureAwait(false);
            var vr = new ValidationResultList();
            await AsyncValidator.TryValidateObjectAsync(tempEntityForValidation, new ValidationContext(tempEntityForValidation), vr).ConfigureAwait(false); 
            return vr;
        }
        #endregion

        #region Private Helper Methods
        protected TEntity CastToEntity(object? obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return (TEntity)obj;
        }
        protected virtual async Task<TEntity> CreateTempValidationEntityAsync()
        {
            var entity = (TEntity)CreateEntity();
            entity = await this.MapToAsync(entity).ConfigureAwait(false);
            return entity;
        }
        public virtual IEntity CreateEntity()
        {
            return new TEntity() { Id = Id };
        }
        #endregion
    }
}
