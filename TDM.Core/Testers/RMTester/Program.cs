#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;
using Supermodel.DataAnnotations.Validations;
using Supermodel.ReflectionMapper;

namespace RMTester
{    
    class StudentMvcModel
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public double? GPA { get; set; }
        public AddressMvcModel Address { get; set; } = new AddressMvcModel();
        [RMapTo(".Address.Zip")] public string StudentZip { get; set; } = "";
        public List<GradeMvcModel> Grades {get; set;} = new List<GradeMvcModel>();

    }

    class AddressMvcModel //: IRMapperCustom
    {
        public string Street { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string Zip { get; set; } = "";
    }

    class GradeMvcModel //: IRMapperCustom
    {
        public string Subject { get; set; } = "";
        public double Score { get; set; } = 0;
    }

    class Student
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public double? GPA { get; set; }
        public Address Address { get; set; } = new Address();
        public List<Grade> Grades {get; set;} = new List<Grade>();
    }

    class Address
    {
        public string Street { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string Zip { get; set; } = "";
    }

    class Grade
    {
        public string Subject { get; set; } = "";
        public double Score { get; set; }
    }
    
    class Program
    {
        static async Task Main()
        {
            //byte[] key = { 0xA6, 0x46, 0x10, 0xF1, 0xEA, 0x16, 0x51, 0xA0, 0xB2, 0x41, 0x27, 0x5C, 0x23, 0x9C, 0xF0, 0xDD };
            //var code = EncryptorAgent.Lock(key, "Abacaba48", out var iv);
            //var decode = EncryptorAgent.Unlock(key, code, iv);
            //var hash = decode.GetMD5Hash();
            //return;

            await Task.Delay(0);
            var student = new Student
            {
                FirstName = "Ilya",
                LastName = "Basin",
                GPA = null,
                Address = new Address
                {
                    Street = "2565 Pennington Place",
                    City = "Vienna",
                    State = "VA",
                    Zip = "22181"
                },
                Grades = new List<Grade> 
                { 
                    new Grade{ Subject = "Math", Score = 4.0},
                    new Grade{ Subject = "Biology", Score = 3.0},
                    new Grade{ Subject = "PE", Score = 2.0},
                }
            };
            var students = new List<Student> { student };
            var studentMvcModels = new List<StudentMvcModel>();

            await studentMvcModels.MapFromAsync(students);
            students = new List<Student>();
            await studentMvcModels.MapToAsync(students);


            var studentMvcModel = await new StudentMvcModel().MapFromAsync(student);
            var student2 = new Student();
            await studentMvcModel.MapToAsync(student2);
            
            var student3 = new Student();
            await studentMvcModel.MapFromAsync(student3);

            // ReSharper disable once UnusedVariable
            var isSame = student.IsEqualToObject(student2);
            student2.Address.Zip = "";
            // ReSharper disable once UnusedVariable
            var isSame2 = student.IsEqualToObject(student2);

            var vrl = new ValidationResultList();
            vrl.AddValidationResult(studentMvcModel, "Error", x => x.FirstName, x => x.LastName);
        }
    }
}
