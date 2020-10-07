#nullable enable

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;

namespace WebMonkTester.StudentPage
{
    public class StudentMvcModel : Bs4.MvcModel  //ISelfModelBinder
    {
        //#region IModelBinder implementation
        //public async Task<object?> BindMeAsync(object rootObject, List<IValueProvider> valueProviders)
        //{
        //    await HttpContext.Current.StaticModelBinderManager.TryUpdateMvcModelAsync(this, valueProviders, true);
        //    return this;
        //}
        //#endregion
        
        #region Properties
        [Required] public Bs4.TextBoxMvcModel FirstName { get; set; } = new Bs4.TextBoxMvcModel();
        [ScaffoldColumn(false)] public Bs4.TextBoxMvcModel LastName { get; set; } = new Bs4.TextBoxMvcModel();
        [DisplayName("Grade Point Average"), Required] public Bs4.TextBoxMvcModel GPA { get; set; } = new Bs4.TextBoxMvcModel { Type = "number" };
        public Bs4.TextBoxMvcModel AnnualIncome { get; set; } = new Bs4.TextBoxMvcModel();
        [DataType(DataType.MultilineText)] public string Notes { get; set; } = "";

        public bool MinorityStudent { get; set; } = true;
        
        [Required] public byte[] Picture { get; set; } = new byte[0];
        [ScaffoldColumn(false)] public string PictureFileName { get; set; } = "";

        public AddressMvcModel Address { get; set; } = new AddressMvcModel 
        { 
            Street = new Bs4.TextBoxMvcModel { Value = "2565 Pennington Place", Type = "text" },
            City = new Bs4.TextBoxMvcModel { Value = "Vienna", Type = "text" },
            State = new Bs4.TextBoxMvcModel { Value = "VA", Type = "text" },
            Zip = new Bs4.TextBoxMvcModel { Value = "22181", Type = "text" },
        };

        public List<string> Classes { get; set; } = new List<string> { "Math", "Science", "English" };
        public string[] ClassesArr { get; set; } = new string[] { "Math", "Science", "English" };
        public List<HobbyMvcModel> Hobbies { get; set; } = new List<HobbyMvcModel> { new HobbyMvcModel { Name = "Model Airplanes" }, { new HobbyMvcModel { Name = "Stamp Collecting" } } };
        public Dictionary<string, string> Dict { get; set; }  = new Dictionary<string, string> { { "A", "Alpha" }, { "B", "Beta"} };
        #endregion
    }
}
