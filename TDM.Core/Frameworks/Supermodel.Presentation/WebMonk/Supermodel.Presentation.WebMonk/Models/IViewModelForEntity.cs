﻿#nullable enable

using Supermodel.DataAnnotations.Validations;
using Supermodel.Persistence.Entities;
using Supermodel.ReflectionMapper;

namespace Supermodel.Presentation.WebMonk.Models
{
    //we need this as a separate interface for ApiModelForAnyEntity
    public interface IViewModelForAnyEntity : IAsyncValidatableObject, IRMapperCustom
    {
        long Id { get; set; }
        bool IsNewModel();
    }
    
    public interface IViewModelForEntity : IViewModelForAnyEntity
    {
        IEntity CreateEntityWithMyId();
    }
}
