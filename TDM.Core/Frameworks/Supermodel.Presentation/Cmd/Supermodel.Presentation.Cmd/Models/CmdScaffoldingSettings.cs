#nullable enable

using System;
using Supermodel.Presentation.Cmd.ConsoleOutput;

namespace Supermodel.Presentation.Cmd.Models
{
    public static class CmdScaffoldingSettings
    {
        public static ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
        
        public static FBColors? Prompt { get; set; } = new FBColors(ConsoleColor.Cyan, BackgroundColor);
        public static FBColors? CommandValue { get; set; } = new FBColors(ConsoleColor.Magenta, BackgroundColor);
        public static FBColors? InvalidCommand { get; set; } = new FBColors(ConsoleColor.Magenta, BackgroundColor);
        public static FBColors? Title { get; set; } = new FBColors(ConsoleColor.Cyan, BackgroundColor);

        public static FBColors? ListEntityId { get; set; } = new FBColors(ConsoleColor.Cyan, BackgroundColor);
        public static FBColors? DefaultListLabel { get; set; } = new FBColors(ConsoleColor.Yellow, BackgroundColor);
        
        public static FBColors? EditLabel { get; set; } = new FBColors(ConsoleColor.White, BackgroundColor);
        public static FBColors? EditValue { get; set; } = new FBColors(ConsoleColor.Yellow, BackgroundColor);
        public static FBColors? DropdownArrow { get; set; } = new FBColors(ConsoleColor.Cyan, BackgroundColor);
        
        public static FBColors? DisplayLabel { get; set; } = new FBColors(ConsoleColor.White, BackgroundColor);
        public static FBColors? DisplayValue { get; set; } = new FBColors(ConsoleColor.Yellow, BackgroundColor);

        public static FBColors? InvalidValueMessage { get; set; } = new FBColors(ConsoleColor.Magenta, BackgroundColor);
        public static FBColors? ValidationErrorMessage { get; set; } = new FBColors(ConsoleColor.Magenta, BackgroundColor);
        public static FBColors? InvalidValueDisplayLabel { get; set; } = new FBColors(ConsoleColor.Magenta, BackgroundColor);

        public static StringWithColor RequiredMarker { get; set; } = new StringWithColor("*", ConsoleColor.Magenta, BackgroundColor);
    }
}
