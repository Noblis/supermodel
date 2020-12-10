#nullable enable

using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Supermodel.Presentation.Cmd.Controllers;

namespace CmdTester
{
    //public class TDMUserCmdController : CRUDCmdController<TDMUser, TDMUserCmdModel, DataContext> { }

    class Program
    {
        static async Task Main()
        {
            var controller = new CRUDCmdController<TDMUser, TDMUserCmdModel, DataContext>();
            await controller.ShowListAsync();
            
            //var ilya = new StringWithColor("Ilya", ConsoleColor.Red);
            //var basin = new StringWithColor("Basin", ConsoleColor.Blue);

            //var ilyaBasin = ilya + " " + basin;
            //ilyaBasin.WriteToConsole();

            //Console.Write("Edit text: ");
            //var text = ConsoleExt.ReadPasswordLine();
            //Console.WriteLine(text);

            //var ilya = new Student();
            //CmdContext.ValidationResultList.Clear();
            //CmdContext.ValidationResultList.AddValidationResult(ilya, "Bad First Name", x => x.FirstName);
            //CmdContext.ValidationResultList.AddValidationResult(ilya, "Bad DOB", x => x.DateOfBirth);

            //CmdRender.DisplayForModel(ilya);
            //Console.WriteLine();
            //ilya = CmdRender.EditForModel(ilya);
            //Console.WriteLine();
            //CmdRender.DisplayForModel(ilya);

            //var optionsList = new List<ConsoleExt.SelectListItem> 
            //{ 
            //    new ConsoleExt.SelectListItem("A", "Letter A"),
            //    new ConsoleExt.SelectListItem("B", "Letter B"),
            //    new ConsoleExt.SelectListItem("C", "Letter C"),
            //    new ConsoleExt.SelectListItem("D", "Letter D"),

            //};
            //var x = ConsoleExt.EditDropdownList("A", optionsList, new FBColors(ConsoleColor.Blue));
            //Console.WriteLine(x);
        }
    }
}
