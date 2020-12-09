#nullable enable

using System;
using Supermodel.Presentation.Cmd.ConsoleOutput;

namespace Supermodel.Presentation.Cmd.Models
{
    public static class CmdScaffoldingSettings
    {
        public static FBColors? EditLabel { get; set; } = new FBColors(ConsoleColor.Green, ConsoleColor.Black);
        public static FBColors? DisplayLabel { get; set; } = new FBColors(ConsoleColor.Green, ConsoleColor.Black);

        public static FBColors? EditValue { get; set; } = new FBColors(ConsoleColor.White, ConsoleColor.Black);
        public static FBColors? DisplayValue { get; set; } = new FBColors(ConsoleColor.White, ConsoleColor.Black);

        public static StringWithColor RequiredMarker { get; set; } = new StringWithColor("*", ConsoleColor.Red, ConsoleColor.Black);
    }
}
