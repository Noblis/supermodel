#nullable enable

using Supermodel.Persistence.Entities;
using Supermodel.Persistence.Repository;

namespace Domain.Supermodel.Persistence
{
    public class CustomRepoFactory : IRepoFactory
    {
        public IDataRepo<TEntity>? CreateRepo<TEntity>() where TEntity : class, IEntity, new()
        {
            //Use the following pattern to register custom repos
            //if (typeof(TEntity) == typeof(Student)) return (IDataRepo<TEntity>)new StudentRepo();

            return null;
        }
    }
}
