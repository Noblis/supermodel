#nullable enable

using System.ComponentModel.DataAnnotations;
using Supermodel.DataAnnotations.Attributes;
using Supermodel.Persistence.Entities;
using Supermodel.Presentation.Mvc.Bootstrap4.Models;
using Supermodel.Presentation.Mvc.Models.Api;

namespace MvcCoreTester.Models
{
    public class ClassApiModel : ApiModelForEntity<Class>
    {
        #region Properties
        [Required] public string Name { get; set; } = "";
        [Required] public int? Credits { get; set; }
        #endregion
    }

    public class ClassMvcModel : Bs4.ChildMvcModelForEntity<Class, Student>
    {
        #region Overrides
        public override string Label => Name.Value;
        public override Student? GetParentEntity(Class entity)
        {
            return (entity).Student;
        }
        public override void SetParentEntity(Class entity, Student? parent)
        {
            entity.Student = parent;
        }
        #endregion

        #region Properties
        [ListColumn, Required] public Bs4.TextBoxMvcModel Name { get; set; } = new Bs4.TextBoxMvcModel();
        [ListColumn, Required] public  Bs4.TextBoxMvcModel Credits { get; set; } = new Bs4.TextBoxMvcModel();
        #endregion

    }
    
    public class Class : Entity
    {
        #region Properties
        [Required] public string Name { get; set; } = "";
        [Required] public int? Credits { get; set; }

        public virtual Student? Student { get; set; }
        #endregion
    }
}
