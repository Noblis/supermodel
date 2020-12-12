#nullable enable

using System;
using Supermodel.Presentation.Cmd.ConsoleOutput;

namespace Supermodel.Presentation.Cmd.Models
{
    public static class CmdScaffoldingSettings
    {
        public static ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
        
        public static FBColors? Prompt1 { get; set; } = new FBColors(ConsoleColor.Cyan, BackgroundColor);
        public static FBColors? Prompt2 { get; set; } = new FBColors(ConsoleColor.Magenta, BackgroundColor);

        public static FBColors? ListTitle { get; set; } = new FBColors(ConsoleColor.Cyan, BackgroundColor);
        public static FBColors? ListTitleUnderline { get; set; } = new FBColors(ConsoleColor.Cyan, BackgroundColor);
        public static FBColors? ListId { get; set; } = new FBColors(ConsoleColor.Cyan, BackgroundColor);
        public static FBColors? DefaultListLabel { get; set; } = new FBColors(ConsoleColor.Yellow, BackgroundColor);
        
        public static FBColors? DetailTitle { get; set; } = new FBColors(ConsoleColor.Cyan, BackgroundColor);
        
        public static FBColors? EditLabel { get; set; } = new FBColors(ConsoleColor.Green, BackgroundColor);
        public static FBColors? DisplayLabel { get; set; } = new FBColors(ConsoleColor.Green, BackgroundColor);

        public static FBColors? EditValue { get; set; } = new FBColors(ConsoleColor.White, BackgroundColor);
        public static FBColors? DropdownArrow { get; set; } = new FBColors(ConsoleColor.Blue, BackgroundColor);
        public static FBColors? DisplayValue { get; set; } = new FBColors(ConsoleColor.White, BackgroundColor);

        public static FBColors? InvalidValueMessage { get; set; } = new FBColors(ConsoleColor.Red, BackgroundColor);
        public static FBColors? ValidationErrorMessage { get; set; } = new FBColors(ConsoleColor.Red, BackgroundColor);
        public static FBColors? InvalidValueDisplayLabel { get; set; } = new FBColors(ConsoleColor.Magenta, BackgroundColor);
        public static FBColors? PleaseFixValidationErrors { get; set; } = new FBColors(ConsoleColor.Cyan, BackgroundColor);

        public static StringWithColor RequiredMarker { get; set; } = new StringWithColor("*", ConsoleColor.Red, BackgroundColor);
    }
}
