#nullable enable

using System.ComponentModel.DataAnnotations;
using Supermodel.Persistence.Entities;
using Supermodel.Presentation.Mvc.Bootstrap4.Models;
using Supermodel.Presentation.Mvc.Models.Api;

namespace MvcCoreTester.Models
{
    public class SchoolApiModel : ApiModelForEntity<School>
    {
        #region Properties
        [Required] public string Name { get; set; } = "";
        #endregion
    }
    
    public class SchoolMvcModel : Bs4.MvcModelForEntity<School>
    {
        #region Overrides
        public override string Label => Name.Value;
        #endregion 

        #region Properties
        [Required] public Bs4.TextBoxMvcModel Name { get; set; } = new Bs4.TextBoxMvcModel();
        #endregion
    }

    public class School : Entity
    {
        #region Properties
        [Required] public string Name { get; set; } = "";

        //public virtual List<Student> Students { get; set; } = new List<Student>();
        #endregion
    }
}
