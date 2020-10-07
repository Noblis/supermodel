#nullable enable

using EFCoreTester.Entities;
using Supermodel.Persistence.EFCore;

namespace EFCoreTester.Repos
{
    public class BookRepo : EFCoreSimpleDataRepo<Book>
    {
        public virtual void Delete(long id)
        {
            var entity = new Book { Id = id };
            DbSet.Attach(entity);
            DbSet.Remove(entity);
        }
    }
}
