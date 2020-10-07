#nullable enable

using EFCoreTester.Entities;
using Supermodel.Persistence.Entities;
using Supermodel.Persistence.Repository;

namespace EFCoreTester.Repos
{
    public class CustomRepoFactory : IRepoFactory
    {
        public IDataRepo<TEntity>? CreateRepo<TEntity>() where TEntity : class, IEntity, new()
        {
            if (typeof(TEntity) == typeof(Book)) return (IDataRepo<TEntity>)new BookRepo();
            return null;
        }
    }
}
