#nullable enable

using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading.Tasks;
using Supermodel.DataAnnotations.Validations;
using Supermodel.Persistence.Entities;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models.Base;
using Supermodel.Presentation.WebMonk.Models.Mvc;
using Supermodel.ReflectionMapper;

namespace Supermodel.Presentation.WebMonk.Bootstrap4.Models
{
    public static partial class Bs4
    {
        public abstract class MvcModelForEntity<TEntity> : MvcModelForEntityCore, IMvcModelForEntity where TEntity : class, IEntity, new()
        {
            #region Validation
            //The default implementation just grabs domain model validation but this can be overriden
            public virtual async Task<ValidationResultList> ValidateAsync(ValidationContext validationContext)
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
                var entity = (TEntity?)UnitOfWorkContext.CustomValues[$"Item_{Id}"];
                if (entity == null) throw new NoNullAllowedException($"UnitOfWorkContext.CustomValues[\"Item_{Id}\"] == null");

                var entityCopyForValidation = await entity.MapToAsync((TEntity)CreateBlankEntityWithMyId());
                entityCopyForValidation = await this.MapToAsync(entityCopyForValidation);
                return entityCopyForValidation;
            }
            public virtual IEntity CreateBlankEntityWithMyId()
            {
                return new TEntity() { Id = Id };
            }
            #endregion
        }
    }
}
