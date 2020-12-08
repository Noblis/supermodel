#nullable enable

using System;
using Supermodel.Presentation.Cmd.ConsoleOutput;

namespace CmdTester
{
    class Program
    {
        static void Main()
        {
            //var ilya = new StringWithColor("Ilya", ConsoleColor.Red);
            //var basin = new StringWithColor("Basin", ConsoleColor.Blue);

            //var ilyaBasin = ilya + " " + basin;
            //ilyaBasin.WriteToConsole();

            Console.Write("Edit text: ");
            var text = ConsoleExt.EditString("Ilya Basin");
            Console.WriteLine(text);
        }
    }
}
