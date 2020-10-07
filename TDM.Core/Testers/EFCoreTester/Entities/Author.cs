#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;
using Supermodel.Persistence.DataContext;
using Supermodel.Persistence.Entities;
using Supermodel.Persistence.Entities.ValueTypes;

namespace EFCoreTester.Entities
{
    public class Author : Entity
    {
        protected override void DeleteInternal()
        {
            foreach (var book in Books!.ToArray())
            {
                book.Delete();
            }
            base.DeleteInternal();
        }

        // ReSharper disable once RedundantOverriddenMember
        public override Task BeforeSaveAsync(OperationEnum operation)
        {
            return base.BeforeSaveAsync(operation);
        }

        public string Name { get; set; } = "";
        public virtual USAddress Address { get; set; } = default!; 
        public virtual List<Book> Books { get; set; } = new List<Book>();
    }
}