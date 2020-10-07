#nullable enable

using System;

namespace Supermodel.Persistence.Entities
{
    public interface IM2M
    {
        #region Methods
        IEntity GetConnectionToOther(Type otherType);
        void SetConnectionToOther(IEntity other);
        #endregion
    }
}