#nullable enable

using System;
using Supermodel.ReflectionMapper;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Supermodel.DataAnnotations.Validations;
using Supermodel.Persistence.Entities;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.Mvc.Bootstrap4.Models.Base;
using Supermodel.Presentation.Mvc.Models.Mvc;

namespace Supermodel.Presentation.Mvc.Bootstrap4.Models
{
    public static partial class Bs4
    {
        public abstract class MvcModelForEntity<TEntity> : MvcModelForEntityCore, IMvcModelForEntity where TEntity : class, IEntity, new()
        {
            #region Validation
            //The default implementation just grabs domain model validation but this can be overriden
            public virtual async Task<ValidationResultList> ValidateAsync(ValidationContext validationContext)
            {
                var tempEntityForValidation = await CreateTempValidationEntityAsync();
                var vr = new ValidationResultList();
                await AsyncValidator.TryValidateObjectAsync(tempEntityForValidation, new ValidationContext(tempEntityForValidation), vr);
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
                var entity = IsNewModel() ? 
                    (TEntity)CreateEntityWithMyId() : 
                    await RepoFactory.Create<TEntity>().GetByIdAsync(Id);

                var entityCopyForValidation = await entity.MapToAsync((TEntity)CreateEntityWithMyId());
                entityCopyForValidation = await this.MapToAsync(entityCopyForValidation);
                return entityCopyForValidation;
            }
            public virtual IEntity CreateEntityWithMyId()
            {
                return new TEntity { Id = Id };
            }
            #endregion
        }
    }
}
