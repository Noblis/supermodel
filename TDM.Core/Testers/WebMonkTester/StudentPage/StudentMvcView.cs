#nullable enable

using System.Collections.Generic;
using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Views;

namespace WebMonkTester.StudentPage
{
    public class StudentMvcView : MvcView
    {
        public IGenerateHtml RenderList(List<StudentMvcModel> students)
        {
            var html = new Tags
            {
                new H1 { new Txt("List of Students") },
                new Br(),
                new Table(new { @class="table table-striped"})
                {
                    new Thead 
                    {
                        new Tr
                        {
                            new Th { new Txt("First Name") },
                            new Th { new Txt("Last Name") },
                            new Th { new Txt("GPA") },
                        },
                    },
                    new Tbody
                    {
                        new CodeBlock(() => 
                        { 
                            var tags = new Tags();
                            foreach (var student in students)
                            {
                                var tr = new Tr();
                                tr.Add(new Td { new Txt(student.FirstName.Value)});
                                tr.Add(new Td { new Txt(student.LastName.Value)});
                                tr.Add(new Td { new Txt(student.GPA.Value)});
                                tags.Add(tr);
                            }
                            return tags;
                        }),
                    },
                },
            };

            return ApplyToDefaultLayout(html);
        }
        public IGenerateHtml RenderDetail(StudentMvcModel student)
        {
            var html = new Tags
            {
                Render.ValidationSummary(new { @class="invalid-feedback d-block" }),
                new Form(new { method = "POST", enctype="multipart/form-data" })
                {
                    Render.EditorForModel(student),

                    new Div(new { @class="form-group"})
                    {
                        Render.LabelFor(student, x => x.Dict["A"]),
                        Render.TextBoxFor(student, x => x.Dict["A"], new { @class="form-control" }),
                        Render.ValidationMessageFor(student, x => x.Dict["A"], new { @class="invalid-feedback d-block" }),
                    },

                    new Div(new { @class="form-group"})
                    {
                        Render.LabelFor(student, x => x.Classes[0]),
                        Render.TextAreaFor(student, x => x.Classes[0], new { @class="form-control" }),
                        Render.ValidationMessageFor(student, x => x.Classes[0], new { @class="invalid-feedback d-block" }),
                    },

                    new Div(new { @class="form-group row"})
                    {
                        new Div(new { @class="col-sm-2" })
                        {
                            Render.LabelFor(student, x => x.MinorityStudent),
                        },
                        new Div(new { @class="col-sm-10" })
                        {
                            Render.CheckBoxFor(student, x => x.MinorityStudent, new { @class="form-check-input" }),
                            Render.ValidationMessageFor(student, x => x.MinorityStudent, new { @class="invalid-feedback d-block" }),
                        },
                    },

                    new Div(new { @class="form-group row"})
                    {
                        new Div(new { @class="col-sm-2" })
                        {
                            Render.LabelFor(student, x => x.Picture),
                        },
                        new Div(new { @class="col-sm-10" })
                        {
                            Render.FilePickerFor(student, x => x.Picture, new { @class="form-control" }),
                            Render.ValidationMessageFor(student, x => x.Picture, new { @class="invalid-feedback d-block" }),
                        },
                    },

                    new Div(new { @class="form-group row"})
                    {
                        new Div(new { @class="col-sm-2" })
                        {
                            Render.LabelFor(student, x => x.LastName),
                        },
                        new Div(new { @class="col-sm-10" })
                        {
                            Render.DropdownListFor(student, x => x.LastName, new []{ SelectListItem.Empty, new SelectListItem("Basin", "Basin", "1"), new SelectListItem("Satanovsky", "Satanovsky", "2") }, new { @class="form-control" }),
                            Render.ValidationMessageFor(student, x => x.LastName, new { @class="invalid-feedback d-block" }),
                        },
                    },

                    new Button(new { type = "submit", @class="btn btn-primary" }){ new Txt("Submit") },
                }
                //new Form(new { method = "POST", enctype="multipart/form-data" })
                //{
                //    new Div(new { @class="form-group"})
                //    {
                //        Render.LabelFor(student, x => x.MinorityStudent),
                //        Render.EditorFor(student, x => x.MinorityStudent, new { @class="form-control" }),
                //        Render.ValidationMessageFor(student, x => x.MinorityStudent, new { @class="invalid-feedback d-block" })
                //        //RenderDisplayFor(student, x => x.Address.Street, new { @class="form-control" }),
                //        //RenderHiddenFor(student, x => x.Address.Street ),
                //    },

                //    new Div(new { @class="form-group"})
                //    {
                //        Render.LabelFor(student, x => x.Address),
                //        Render.EditorFor(student, x => x.Address, new { @class="form-control" }),
                //        Render.ValidationMessageFor(student, x => x.Address, new { @class="invalid-feedback d-block" })
                //        //RenderDisplayFor(student, x => x.Address.Street, new { @class="form-control" }),
                //        //RenderHiddenFor(student, x => x.Address.Street ),
                //    },

                //    new Div(new { @class="form-group"})
                //    {
                //        Render.LabelFor(student, x => x.Dict["A"]),
                //        Render.EditorFor(student, x => x.Dict["A"], new { @class="form-control" }),
                //        Render.ValidationMessageFor(student, x => x.Dict["A"], new { @class="invalid-feedback d-block" }),
                //    },

                //    new Div(new { @class="form-group"})
                //    {
                //        Render.LabelFor(student, x => x.Classes[0]),
                //        Render.EditorFor(student, x => x.Classes[0], new { @class="form-control" }),
                //        Render.ValidationMessageFor(student, x => x.Classes[0], new { @class="invalid-feedback d-block" }),
                //    },

                //    new Div(new { @class="form-group"})
                //    {
                //        Render.LabelFor(student, x => x.FirstName),
                //        Render.EditorFor(student, x => x.FirstName, new { @class="form-control", id = (string?)null }),
                //        Render.ValidationMessageFor(student, x => x.FirstName, new { @class="invalid-feedback d-block" }),
                //        //RenderDisplayFor(student, x => x.FirstName, new { @class="form-control" }),
                //        //RenderHiddenFor(student, x => x.FirstName ),
                //    },
                //    new Div(new { @class="form-group"})
                //    {
                //        Render.LabelFor(student, x => x.LastName),
                //        Render.EditorFor(student, x => x.LastName, new { @class="form-control" }),
                //        Render.ValidationMessageFor(student, x => x.LastName, new { @class="invalid-feedback d-block" }),
                //        //RenderDisplayFor(student, x => x.LastName, new { @class="form-control" }),
                //        //RenderHiddenFor(student, x => x.LastName ),
                //    },
                //    new Div(new { @class="form-group"})
                //    {
                //        Render.LabelFor(student, x => x.GPA),
                //        Render.EditorFor(student, x => x.GPA, new { @class="form-control", id = (string?)null }),
                //        Render.ValidationMessageFor(student, x => x.GPA, new { @class="invalid-feedback d-block" }),
                //        //RenderDisplayFor(student, x => x.GPA, new { @class="form-control" }),
                //        //RenderHiddenFor(student, x => x.GPA ),
                //    },
                //    new Div(new { @class="form-group"})
                //    {
                //        Render.LabelFor(student, x => x.AnnualIncome),
                //        Render.EditorFor(student, x => x.AnnualIncome, new { @class="form-control", id = (string?)null }),
                //        Render.ValidationMessageFor(student, x => x.AnnualIncome, new { @class="invalid-feedback d-block" }),
                //        //RenderDisplayFor(student, x => x.GPA, new { @class="form-control" }),
                //        //RenderHiddenFor(student, x => x.GPA ),
                //    },
                //    new Div(new { @class="form-group"})
                //    {
                //        Render.LabelFor(student, x => x.Notes),
                //        Render.EditorFor(student, x => x.Notes, new { @class="form-control" }),
                //        Render.ValidationMessageFor(student, x => x.Notes, new { @class="invalid-feedback d-block" }),
                //        //RenderDisplayFor(student, x => x.Notes, new { @class="form-control" }),
                //        //RenderHiddenFor(student, x => x.Notes ),
                //    },
                //    new Div(new { @class="form-group"})
                //    {
                //        Render.LabelFor(student, x => x.Picture),
                //        Render.EditorFor(student, x => x.Picture, new { @class="form-control" }),
                //        Render.ValidationMessageFor(student, x => x.Picture, new { @class="invalid-feedback d-block" }),
                //        //RenderDisplayFor(student, x => x.Notes, new { @class="form-control" }),
                //        //RenderHiddenFor(student, x => x.Notes ),
                //    },
                //    new CodeBlock(() =>
                //    {
                //        var tags = new Tags();
                //        for (var i =0; i < student.Hobbies.Count; i++)
                //        {
                //            var j = i;
                //            tags.Add(new Div(new { @class="form-group"})
                //            {
                //                Render.LabelFor(student, x => x.Hobbies[j].Name),
                //                Render.EditorFor(student, x => x.Hobbies[j].Name, new { @class="form-control" }),
                //                Render.ValidationMessageFor(student, x => x.Hobbies[j].Name, new { @class="invalid-feedback d-block" }),
                //            });
                //        }
                //        return tags;
                //    }),
                //    new CodeBlock(() => 
                //    { 
                //        var tags = new Tags();
                //        for (var i =0; i < student.ClassesArr.Length; i++)
                //        {
                //            var j = i;
                //            tags.Add(new Div(new { @class="form-group"})
                //            {
                //                Render.LabelFor(student, x => x.ClassesArr[j]),
                //                Render.EditorFor(student, x => x.ClassesArr[j], new { @class="form-control" }),
                //                Render.ValidationMessageFor(student, x => x.ClassesArr[j], new { @class="invalid-feedback d-block" }),
                //            });
                //        }
                //        return tags;
                //    }),


                //    new Button(new { type = "submit", @class="btn btn-primary" }){ new Txt("Submit") },

                //    Render.ActionLink<StudentMvcController>("Link <Text>", x => x.GetDetail(5), new { @class="btn btn-success" }),
                //    Render.ActionLink<StudentMvcController>(new Tags() { new Hr(), new B { new Txt("Html link") }, new Hr() }, x => x.GetDetail(5), new { @class="btn btn-success" }),
                //}
            };

            return ApplyToDefaultLayout(html);//.FillSectionWith("@AnotherSection", otherTag);
        }
    }
}
