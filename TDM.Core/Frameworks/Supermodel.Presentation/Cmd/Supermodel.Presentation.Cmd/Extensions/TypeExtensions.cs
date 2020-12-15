#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Supermodel.DataAnnotations.Attributes;
using Supermodel.Persistence.Entities;

namespace Supermodel.Presentation.Cmd.Extensions
{
    public static class TypeExtensions
    {
        #region Methods
        public static bool IsEntityType(this Type me)
        {
            return typeof (IEntity).IsAssignableFrom(me);
        }
        #endregion
    }
}