#nullable enable

using System;
using Supermodel.Presentation.Cmd.ConsoleOutput;

namespace Supermodel.Presentation.Cmd.Models
{
    public static class CmdScaffoldingSettings
    {
        public static FBColors? Label { get; set; } = new FBColors(ConsoleColor.Green, ConsoleColor.Black);
        public static FBColors? Value { get; set; } = new FBColors(ConsoleColor.White, ConsoleColor.Black);
        public static FBColors? RequiredMarker { get; set; } = new FBColors(ConsoleColor.Red, ConsoleColor.Black);
    }
}
