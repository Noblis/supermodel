#nullable enable

using System;
using System.Diagnostics;
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
            if (Debugger.IsAttached || !await EFCoreUnitOfWorkContext.Database.CanConnectAsync())
            {
                Console.Write("Recreating the database... ");
                await using (new UnitOfWork<DataContext>())
                {
                    await EFCoreUnitOfWorkContext.Database.EnsureDeletedAsync();
                    await EFCoreUnitOfWorkContext.Database.EnsureCreatedAsync();
                    await UnitOfWorkContext.SeedDataAsync();
                }
                Console.WriteLine("Done!");
            }
            
            var controller = new CRUDCmdController<TDMUser, TDMUserCmdModel, DataContext>("User");
            await controller.ListAsync();
            Console.WriteLine();
            await controller.ViewDetailAsync(1);
            Console.WriteLine();
            //await controller.AddDetailAsync();
            //Console.WriteLine();
            //await controller.ListAsync();
            //Console.WriteLine();
            await controller.EditDetailAsync(1);
            Console.WriteLine();
            await controller.ListAsync();
            Console.WriteLine();
            await controller.DeleteDetailAsync(1);
            Console.WriteLine();
            await controller.ListAsync();


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
            //var x = ConsoleExt.EditDropdownList("A", optionsList, new FBColors(ConsoleColor.Cyan));
            //Console.WriteLine(x);
        }
    }
}
