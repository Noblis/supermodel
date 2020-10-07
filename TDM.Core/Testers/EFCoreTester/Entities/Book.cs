#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Supermodel.Persistence.DataContext;
using Supermodel.Persistence.Entities;
using Supermodel.Persistence.Entities.ValueTypes;

namespace EFCoreTester.Entities
{
    public class Book : Entity
    {
        // ReSharper disable once RedundantOverriddenMember
        public override Task BeforeSaveAsync(OperationEnum operation)
        {
            return base.BeforeSaveAsync(operation);
        }

        //public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    return base.Validate(validationContext);
        //}

        [Required] public string Title { get; set; } = default!; //"";
        public double Price { get; set; }
        public long AuthorId { get; set; }
        public virtual Author Author { get; set; } = default!; //new Author();
        public virtual BinaryFile Content { get; set; } = default!; //new BinaryFile();
    }

    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            //builder.ToTable("Knigi");
        }
    }
}