#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MvcCoreTester.ApiControllers;
using Supermodel.DataAnnotations.Validations;
using Supermodel.DataAnnotations.Validations.Attributes;
using Supermodel.Persistence.Entities;
using Supermodel.Persistence.Entities.ValueTypes;
using Supermodel.Presentation.Mvc.Bootstrap4.Models;
using Supermodel.Presentation.Mvc.Models;
using Supermodel.Presentation.Mvc.Models.Api;
using Supermodel.ReflectionMapper;

namespace MvcCoreTester.Models
{
    public enum GenderEnum { Male, Female, [Disabled, ScreenOrder(0)] Unknown }
    
    public class StudentApiModel : ApiModelForEntity<Student>
    {
        #region Properties
        [Required] public string FirstName { get; set; } = "";
        [Required] public string LastName { get; set; } = "";
        public string SocialSecurity { get; set; } = "";
        public int? Age { get; set; }
        public BinaryFileApiModel? Image { get; set; }
        [Required] public GenderEnum? Gender { get; set; } = GenderEnum.Unknown;
        [Required] public SchoolApiModel? School { get; set; }
        public string? Notes { get; set; }
        public bool? SecurityClearance { get; set; }
        [Required] public DateTime? DateOfBirthday { get; set; }

        //public ViewModelList<ClassApiModel, Class> Classes { get; set; } = new ViewModelList<ClassApiModel, Class>();
        #endregion
    }    
    public class StudentSearchApiModel : SearchApiModel
    {
        public string Term { get; set; } = "";
    }
    
    public class StudentSearchMvcModel : Bs4.MvcModel//, IAsyncValidatableObject 
    {
        //#region Validation
        //public Task<ValidationResultList> ValidateAsync(ValidationContext validationContext)
        //{
        //    var vrl = new ValidationResultList();
        //    if (MinAge.IntValue.HasValue && MaxAge.IntValue.HasValue && MinAge.IntValue.Value > MaxAge.IntValue.Value)
        //    {
        //        vrl.AddValidationResult(this, "Min Age must be smaller than Max Age", x => x.MinAge, x=> x.MaxAge);
        //    }
        //    return Task.FromResult(vrl);
        //}
        //#endregion

        #region Properties
        [NotRMapped] public List<Bs4.AccordionPanel> Panels { get; } = new List<Bs4.AccordionPanel>
        {
            new Bs4.AccordionPanel("Filter", "Filter", 100, 100, false),
        };

        //[Email] public Super.Bs4.TextBoxMvcModel Email { get; set; } = new Super.Bs4.TextBoxMvcModel();
        public Bs4.AutocompleteTextBoxMvcModel Name { get; set; } = new Bs4.AutocompleteTextBoxMvcModel(typeof(StudentAutocompleteApiController));
        public Bs4.AutocompleteTextBoxMvcModel Name2 { get; set; } = new Bs4.AutocompleteTextBoxMvcModel(typeof(Student2AutocompleteApiController));
        public Bs4.TextBoxMvcModel FirstName { get; set; } = new Bs4.TextBoxMvcModel();
        public Bs4.TextBoxMvcModel LastName { get; set; } = new Bs4.TextBoxMvcModel();
        public Bs4.DropdownMvcModelUsingEnum<GenderEnum> Gender { get; set; } = new Bs4.DropdownMvcModelUsingEnum<GenderEnum>();
        //public Bs4.RadioSelectMvcModelUsingEnum<GenderEnum> Gender { get; set; } = new Bs4.RadioSelectMvcModelUsingEnum<GenderEnum>();
        public Bs4.DropdownMvcModelUsing<SchoolMvcModel> School { get; set; } = new Bs4.DropdownMvcModelUsing<SchoolMvcModel>();
        public Bs4.CheckboxMvcModel ClearanceRequired { get; set; } = new Bs4.CheckboxMvcModel();
        [MustBeLessOrEqualThan(nameof(MaxAge))] public Bs4.TextBoxMvcModel MinAge { get; set; } = new Bs4.TextBoxMvcModel { Type = "number" };
        [MustBeGreaterOrEqualThan(nameof(MinAge))] public Bs4.TextBoxMvcModel MaxAge { get; set; } = new Bs4.TextBoxMvcModel { Type = "number" };
        #endregion
    }

    public class StudentDetailMvcModel : Bs4.MvcModelForEntity<Student>
    {
        #region Overrides
        public override string Label => $"{FirstName} {LastName}";
        #endregion

        #region Properties
        [NotRMapped] public List<Bs4.AccordionPanel> Panels { get; set; } = new List<Bs4.AccordionPanel>
        {
            new Bs4.AccordionPanel("General", "General", 100, 100, true),
            new Bs4.AccordionPanel("Security", "Security", 200, 200, false)
        };
        
        [Required, ListColumn(OrderBy = nameof(FirstName))] public Bs4.TextBoxMvcModel FirstName { get; set; } = new Bs4.TextBoxMvcModel();
        [Required, ListColumn(OrderBy = nameof(LastName)), DisplayName("Surname")] public Bs4.TextBoxMvcModel LastName { get; set; } = new Bs4.TextBoxMvcModel();
        public Bs4.PasswordTextBoxMvcModel SocialSecurity { get; set; } = new Bs4.PasswordTextBoxMvcModel();
        [Required, ListColumn(OrderBy = nameof(Age))] public Bs4.TextBoxMvcModel Age { get; set; } = new Bs4.TextBoxMvcModel();
        [RMapTo(".Image"), NotRMappedTo] public Bs4.ImageMvcModel Picture { get; set; } = new Bs4.ImageMvcModel { HtmlAttributesAsObj = new { width = 200 } };
        [HideLabel(KeepLabelSpace = true)] public Bs4.BinaryFileMvcModel Image { get; set; } = new Bs4.BinaryFileMvcModel();
        //[Required, RMapTo(".Gender")] public Super.Bs4.DropdownMvcModelUsingEnum<GenderEnum> Gender2 { get; set; } = new Super.Bs4.DropdownMvcModelUsingEnum<GenderEnum>();
        [Required, ListColumn(OrderBy = nameof(Gender))] public Bs4.RadioSelectMvcModelUsingEnum<GenderEnum> Gender { get; set; } = new Bs4.RadioSelectMvcModelUsingEnum<GenderEnum>();
        [Required, ListColumn(OrderBy = nameof(School))] public Bs4.DropdownMvcModelUsing<SchoolMvcModel> School { get; set; } = new Bs4.DropdownMvcModelUsing<SchoolMvcModel>();
        //public Bs4.RadioSelectMvcModelUsing<SchoolMvcModel> School { get; set; } = new Bs4.RadioSelectMvcModelUsing<SchoolMvcModel>();
        public Bs4.TextAreaMvcModel Notes { get; set; } = new Bs4.TextAreaMvcModel();
        [Required, ListColumn(OrderBy = nameof(DateOfBirthday))] public Bs4.DateMvcModel DateOfBirthday { get; set; } = new Bs4.DateMvcModel();
        //[Required, ListColumn(OrderBy = nameof(DateOfBirthday))] public Bs4.DateTimeMvcModel DateOfBirthday { get; set; } = new Bs4.DateTimeMvcModel();
        [ScreenOrder(200)] public Bs4.CheckboxMvcModel SecurityClearance { get; set; } = new Bs4.CheckboxMvcModel();

        public virtual ListViewModel<ClassMvcModel, Class> Classes { get; set; } = new ListViewModel<ClassMvcModel, Class>();
        #endregion
    }

    public class StudentListMvcModel : Bs4.MvcModelForEntity<Student>
    {
        #region Overrides
        public override string Label => $"{FirstName} {LastName}";
        #endregion

        #region Properties
        [ListColumn(OrderBy = nameof(FirstName))] public Bs4.TextBoxMvcModel FirstName { get; set; } = new Bs4.TextBoxMvcModel();
        [ListColumn(Header = "Surname", OrderBy = "LastName,FirstName")] public Bs4.TextBoxMvcModel LastName { get; set; } = new Bs4.TextBoxMvcModel();
        [ListColumn(OrderBy = nameof(Age))] public Bs4.TextBoxMvcModel Age { get; set; } = new Bs4.TextBoxMvcModel();
        [ListColumn(OrderBy = nameof(Gender))] public Bs4.RadioSelectMvcModelUsingEnum<GenderEnum> Gender { get; set; } = new Bs4.RadioSelectMvcModelUsingEnum<GenderEnum>();
        [ListColumn(OrderBy = nameof(School))] public Bs4.RadioSelectMvcModelUsing<SchoolMvcModel> School { get; set; } = new Bs4.RadioSelectMvcModelUsing<SchoolMvcModel>();
        [ListColumn] public Bs4.TextAreaMvcModel Notes { get; set; } = new Bs4.TextAreaMvcModel();
        [ListColumn(OrderBy = nameof(DateOfBirthday))] public Bs4.DateTimeMvcModel DateOfBirthday { get; set; } = new Bs4.DateTimeMvcModel();
        [ListColumn(OrderBy = nameof(SecurityClearance))] public Bs4.CheckboxMvcModel SecurityClearance { get; set; } = new Bs4.CheckboxMvcModel();
        #endregion
    }

    public class Student : Entity
    {
        #region Overrides
        protected override void DeleteInternal()
        {
            if (Classes.Any()) foreach (var @class in Classes) @class.Delete();
            base.DeleteInternal();
        }
        #endregion
        
        #region Validation
        public override async Task<ValidationResultList> ValidateAsync(ValidationContext validationContext)
        {
            var vrl = await base.ValidateAsync(validationContext);
            //if (FirstName == "Ilya") vrl.AddValidationResult(this, "First Name cannot be Ilya", x => x.FirstName);
            if (Gender == GenderEnum.Unknown) vrl.AddValidationResult(this, "Unknown is not allowed", x => x.Gender);
            return vrl;
        }
        #endregion

        #region Properties
        [Required] public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string SocialSecurity { get; set; } = "";
        public int? Age { get; set; }
        public virtual BinaryFile Image { get; set; } = new BinaryFile();
        [Required] public GenderEnum? Gender { get; set; } = GenderEnum.Unknown;
        [Required] public virtual School? School { get; set; }
        public string? Notes { get; set; }
        public bool? SecurityClearance { get; set; }
        // ReSharper disable once InconsistentNaming
        [Required] public DateTime? DateOfBirthday { get; set; }

        public virtual List<Class> Classes { get; set; } = new List<Class>();
        #endregion
    }
}
