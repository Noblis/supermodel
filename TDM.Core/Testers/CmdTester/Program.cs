#nullable enable

using System.Threading.Tasks;
using Supermodel.Persistence.EFCore;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.Cmd.Controllers;
using WMDomain.Entities;
using WMDomain.Supermodel.Persistence;

namespace CmdTester
{
    //public class TDMUserCmdController : CRUDCmdController<TDMUser, TDMUserCmdModel, DataContext> { }

    class Program
    {
        static async Task Main()
        {
            //This removes and recreates the database
            await using (new UnitOfWork<DataContext>())
            {
                await EFCoreUnitOfWorkContext.Database.EnsureDeletedAsync();
                await EFCoreUnitOfWorkContext.Database.EnsureCreatedAsync();
                await UnitOfWorkContext.SeedDataAsync();
            }
            
            await new CRUDCmdController<TDMUser, TDMUserCmdModel, DataContext>("Users").ShowListAsync();
            
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
