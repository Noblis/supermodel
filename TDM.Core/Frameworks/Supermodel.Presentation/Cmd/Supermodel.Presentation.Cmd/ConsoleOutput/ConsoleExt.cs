#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Supermodel.Presentation.Cmd.Models;

namespace Supermodel.Presentation.Cmd.ConsoleOutput
{
    public static class ConsoleExt
    {
        #region EmbeddedTypes
        public class SelectListItem
        {
            #region Constructors
            public SelectListItem(string value, string label)
            {
                Value = value;
                Label = label;
            }
            public SelectListItem(string value)
            {
                Value = value;
                Label = value;
            }
            #endregion

            #region Properties
            public string Value { get; }
            public string Label { get; }
            #endregion

            #region Static constants
            public static SelectListItem Empty { get; } = new SelectListItem("", "");
            #endregion
        }
        #endregion
        
        #region Date
        public static DateTime? EditDateTime(DateTime? value, FBColors? errorColors = null, FBColors? promptColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors, promptColors);
                    continue;
                }

                if (DateTime.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors, promptColors);
                    valueStr = input;
                }
            }
        }
        #endregion

        #region Edit Boolean
        public static bool EditBool(bool? value, FBColors? arrowColors = null, FBColors? errorColors = null, FBColors? promptColors = null)
        {
            var valueStr = value == true ? "Yes" : "No";
            var result = EditDropdownList(valueStr, BoolOptions, CmdScaffoldingSettings.DropdownArrow);
            return result == "Yes";
        }
        private static SelectListItem[] BoolOptions { get; } = new []{ new SelectListItem("Yes"), new SelectListItem("No")};
        #endregion

        #region Edit Floating Point
        public static decimal? EditFloat(decimal? value, FBColors? errorColors = null, FBColors? promptColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors, promptColors);
                    continue;
                }

                if (decimal.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors, promptColors);
                    valueStr = input;
                }
            }
        }
        public static double? EditFloat(double? value, FBColors? errorColors = null, FBColors? promptColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors, promptColors);
                    continue;
                }

                if (double.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors, promptColors);
                    valueStr = input;
                }
            }
        }
        public static float? EditFloat(float? value, FBColors? errorColors = null, FBColors? promptColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors, promptColors);
                    continue;
                }

                if (float.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors, promptColors);
                    valueStr = input;
                }
            }
        }
        #endregion
        
        #region Edit Integer
        public static ulong? EditInteger(ulong? value, FBColors? errorColors = null, FBColors? promptColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors, promptColors);
                    continue;
                }

                if (ulong.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors, promptColors);
                    valueStr = input;
                }
            }
        }
        public static uint? EditInteger(uint? value, FBColors? errorColors = null, FBColors? promptColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors, promptColors);
                    continue;
                }

                if (uint.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors, promptColors);
                    valueStr = input;
                }
            }
        }
        public static ushort? EditInteger(ushort? value, FBColors? errorColors = null, FBColors? promptColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors, promptColors);
                    continue;
                }

                if (ushort.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors, promptColors);
                    valueStr = input;
                }
            }
        }
        public static byte? EditInteger(byte? value, FBColors? errorColors = null, FBColors? promptColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors, promptColors);
                    continue;
                }

                if (byte.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors, promptColors);
                    valueStr = input;
                }
            }
        }
        
        public static long? EditInteger(long? value, FBColors? errorColors = null, FBColors? promptColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors, promptColors);
                    continue;
                }

                if (long.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors, promptColors);
                    valueStr = input;
                }
            }
        }
        public static int? EditInteger(int? value, FBColors? errorColors = null, FBColors? promptColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors, promptColors);
                    continue;
                }

                if (int.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors, promptColors);
                    valueStr = input;
                }
            }
        }
        public static short? EditInteger(short? value, FBColors? errorColors = null, FBColors? promptColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors, promptColors);
                    continue;
                }

                if (short.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors, promptColors);
                    valueStr = input;
                }
            }
        }
        public static sbyte? EditInteger(sbyte? value, FBColors? errorColors = null, FBColors? promptColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors, promptColors);
                    continue;
                }

                if (sbyte.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors, promptColors);
                    valueStr = input;
                }
            }
        }
        #endregion
        
        #region Edit String & Password
        public static string EditString(string value, FBColors? errorColors = null, FBColors? promptColors = null)
        {
            while(true)
            {
                var input = EditLine(value, x => char.IsLetterOrDigit(x) || char.IsSymbol(x) || char.IsPunctuation(x) || char.IsSeparator(x));
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return "";
                    PrintRequiredFieldMessage(errorColors, promptColors);
                    continue;
                }
                return input;
            }
        }
        public static string ReadPassword(FBColors? errorColors = null, FBColors? promptColors = null )
        {
            while(true)
            {
                var input = ReadPasswordLine(x => char.IsLetterOrDigit(x) || char.IsSymbol(x) || char.IsPunctuation(x) || char.IsSeparator(x));
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return "";
                    PrintRequiredFieldMessage(errorColors, promptColors);
                    continue;
                }
                return input;
            }
        }
        #endregion

        #region Low Level Edit Line
        public static string EditLine(string value, Func<char, bool> isValidCharFunc)
        {
            if (value.Contains('\n')) throw new ArgumentException("Cannot contain new line", nameof(value));
            
            var cursorLeft = Console.CursorLeft;
            var cursorTop = Console.CursorTop;

            Console.Write(value);
            var chars = new List<char>();
            if (!string.IsNullOrEmpty(value)) chars.AddRange(value.ToCharArray());

            while (true)
            {
                var info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Backspace && (Console.CursorLeft > cursorLeft || Console.CursorTop != cursorTop))
                {
                    chars.RemoveAt(chars.Count - 1);
                    
                    var newCursorLeft = Console.CursorLeft - 1;
                    var newCursorTop = Console.CursorTop;
                    if (Console.CursorLeft == 0 && Console.CursorTop > cursorTop)
                    {
                        newCursorTop = Console.CursorTop-1;
                        newCursorLeft = Console.WindowWidth-1;
                    }
                    Console.CursorTop = newCursorTop;
                    Console.CursorLeft = newCursorLeft;
                    Console.Write(' ');
                    Console.CursorTop = newCursorTop;
                    Console.CursorLeft = newCursorLeft;
                }
                else if (info.Key == ConsoleKey.Enter) 
                { 
                    Console.WriteLine(); 
                    break; 
                }
                else if (info.Key == ConsoleKey.Escape)
                {
                    if (CmdContext.CtrlEscEnabled && info.Modifiers.HasFlag(ConsoleModifiers.Shift)) throw new ShiftEscException();
                    
                    Console.CursorTop = cursorTop;
                    Console.CursorLeft = cursorLeft;
                    Console.Write("".PadRight(chars.Count));

                    Console.CursorTop = cursorTop;
                    Console.CursorLeft = cursorLeft;
                    Console.WriteLine(value);

                    return value;
                }
                else if (isValidCharFunc.Invoke(info.KeyChar))
                {
                    Console.Write(info.KeyChar);
                    chars.Add(info.KeyChar);
                }
            }
            return new string(chars.ToArray ());
        }
        public static string ReadPasswordLine(Func<char, bool> isValidCharFunc)
        {
            var value = "";

            var cursorLeft = Console.CursorLeft;
            var cursorTop = Console.CursorTop;

            Console.Write(value);
            var chars = new List<char>();
            if (!string.IsNullOrEmpty(value)) chars.AddRange(value.ToCharArray());

            while (true)
            {
                var info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Backspace && (Console.CursorLeft > cursorLeft || Console.CursorTop != cursorTop))
                {
                    chars.RemoveAt(chars.Count - 1);
                    
                    var newCursorLeft = Console.CursorLeft - 1;
                    var newCursorTop = Console.CursorTop;
                    if (Console.CursorLeft == 0 && Console.CursorTop > cursorTop)
                    {
                        newCursorTop = Console.CursorTop-1;
                        newCursorLeft = Console.WindowWidth-1;
                    }
                    Console.CursorTop = newCursorTop;
                    Console.CursorLeft = newCursorLeft;
                    Console.Write(' ');
                    Console.CursorTop = newCursorTop;
                    Console.CursorLeft = newCursorLeft;
                }
                else if (info.Key == ConsoleKey.Enter) 
                { 
                    Console.Write(Environment.NewLine); 
                    break; 
                }
                else if (info.Key == ConsoleKey.Escape)
                {
                    if (CmdContext.CtrlEscEnabled && info.Modifiers.HasFlag(ConsoleModifiers.Shift)) throw new ShiftEscException();
                    
                    Console.CursorTop = cursorTop;
                    Console.CursorLeft = cursorLeft;
                    Console.Write("".PadRight(chars.Count));

                    Console.CursorTop = cursorTop;
                    Console.CursorLeft = cursorLeft;
                    Console.WriteLine(value);

                    return value;
                }
                else if (isValidCharFunc.Invoke(info.KeyChar))
                {
                    Console.Write('*');
                    chars.Add(info.KeyChar);
                }
            }
            return new string(chars.ToArray ());
        }
        public static string EditLineAllCaps(string value, Func<char, bool> isValidCharFunc)
        {
            if (value.Contains('\n')) throw new ArgumentException("Cannot contain new line", nameof(value));
            
            value = value.ToUpper();
            
            var cursorLeft = Console.CursorLeft;
            var cursorTop = Console.CursorTop;

            Console.Write(value);
            var chars = new List<char>();
            if (!string.IsNullOrEmpty(value)) chars.AddRange(value.ToCharArray());

            while (true)
            {
                var info = Console.ReadKey(true);
                var infoKeyChar = char.ToUpper(info.KeyChar);

                if (info.Key == ConsoleKey.Backspace && (Console.CursorLeft > cursorLeft || Console.CursorTop != cursorTop))
                {
                    chars.RemoveAt(chars.Count - 1);
                    
                    var newCursorLeft = Console.CursorLeft - 1;
                    var newCursorTop = Console.CursorTop;
                    if (Console.CursorLeft == 0 && Console.CursorTop > cursorTop)
                    {
                        newCursorTop = Console.CursorTop-1;
                        newCursorLeft = Console.WindowWidth-1;
                    }
                    Console.CursorTop = newCursorTop;
                    Console.CursorLeft = newCursorLeft;
                    Console.Write(' ');
                    Console.CursorTop = newCursorTop;
                    Console.CursorLeft = newCursorLeft;
                }
                else if (info.Key == ConsoleKey.Enter) 
                { 
                    Console.WriteLine(); 
                    break; 
                }
                else if (info.Key == ConsoleKey.Escape)
                {
                    if (CmdContext.CtrlEscEnabled && info.Modifiers.HasFlag(ConsoleModifiers.Shift)) throw new ShiftEscException();
                    
                    Console.CursorTop = cursorTop;
                    Console.CursorLeft = cursorLeft;
                    Console.Write("".PadRight(chars.Count));

                    Console.CursorTop = cursorTop;
                    Console.CursorLeft = cursorLeft;
                    Console.WriteLine(value);

                    return value;
                }
                else if (isValidCharFunc.Invoke(infoKeyChar))
                {
                    Console.Write(infoKeyChar);
                    chars.Add(infoKeyChar);
                }
            }
            return new string(chars.ToArray ());
        }
        #endregion

        #region Low Level Dropdown List
        public static string EditDropdownList(string value, IEnumerable<SelectListItem> options, FBColors? arrowColors = null)
        {
            var savedCursorVisible = Console.CursorVisible;
            Console.CursorVisible = false;
            
            var optionsArray = options.ToArray();
            var selectedOption = optionsArray.Single(x => x.Value == value);
            bool originalValue = true;
            
            var cursorLeft = Console.CursorLeft;
            var cursorTop = Console.CursorTop;
            var maxLenPlus2 = optionsArray.Select(x => x.Label.Length).Max() + 2;

            var currentColors = FBColors.FromCurrent();

            while(true)
            {
                PrintOption(selectedOption!, maxLenPlus2, cursorLeft, cursorTop, currentColors, arrowColors);

                var info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Enter) 
                {
                    Console.WriteLine(); 
                    Console.CursorVisible = savedCursorVisible;
                    return selectedOption!.Value;
                }
                if (info.Key == ConsoleKey.Escape) 
                {
                    if (CmdContext.CtrlEscEnabled && info.Modifiers.HasFlag(ConsoleModifiers.Shift)) throw new ShiftEscException();
                    
                    selectedOption = optionsArray.Single(x => x.Value == value);
                    PrintOption(selectedOption, maxLenPlus2, cursorLeft, cursorTop, currentColors, arrowColors);
                    Console.WriteLine(); 
                    Console.CursorVisible = savedCursorVisible;
                    return selectedOption!.Value;
                }
                if (info.Key == ConsoleKey.DownArrow)
                {
                    if (originalValue)
                    {
                        var newSelectedOption = optionsArray.First();
                        if (newSelectedOption.Value == selectedOption!.Value) newSelectedOption = optionsArray.ElementAtOrDefault(1);
                        selectedOption = newSelectedOption;
                    }
                    else
                    {
                        var remaining = optionsArray.SkipWhile(x => x.Value != selectedOption!.Value).ToArray();
                        var newSelectedOption = remaining.ElementAtOrDefault(1);
                        selectedOption = newSelectedOption ?? optionsArray.First();
                    }
                    originalValue = false;
                }
                if (info.Key == ConsoleKey.UpArrow)
                {
                    if (originalValue)
                    {
                        var newSelectedOption = optionsArray.Last();
                        if (newSelectedOption.Value == selectedOption!.Value) newSelectedOption = optionsArray.Reverse().ElementAtOrDefault(1);
                        selectedOption = newSelectedOption ?? optionsArray.First();
                    }
                    else
                    {
                        var remaining = optionsArray.TakeWhile(x => x.Value != selectedOption!.Value).Reverse().ToArray();
                        var newSelectedOption = remaining.FirstOrDefault();
                        selectedOption = newSelectedOption ?? optionsArray.Last();
                    }
                    originalValue = false;
                }
            }
        }
        #endregion

        #region Private Helpers
        private static void PrintOption(SelectListItem selectedOption, int maxLenPlus2, int cursorLeft, int cursorTop, FBColors? valueColors, FBColors? arrowColors)
        {
            //Erase the old value
            Console.CursorLeft = cursorLeft;
            Console.CursorTop = cursorTop;
            Console.Write("".PadRight(maxLenPlus2));
                
            //Print the new value
            Console.CursorLeft = cursorLeft;
            Console.CursorTop = cursorTop;
            valueColors?.SetColors();
            Console.Write(selectedOption.Label);
            arrowColors?.SetColors();
            Console.Write('▼');
        }
        private static void PrintRequiredFieldMessage(FBColors? errorColors, FBColors? tryAgainColors)
        {
            var currentColors = FBColors.FromCurrent();
            errorColors?.SetColors();

            if (CmdContext.PropertyDisplayName != null) Console.Write($"The {CmdContext.PropertyDisplayName} is required. ");
            else Console.Write("This field is required. ");

            tryAgainColors?.SetColors();
            Console.Write("Try again: ");

            currentColors.SetColors();
        }
        private static void PrintErrorMessage(FBColors? errorColors, FBColors? tryAgainColors)
        {
            var currentColors = FBColors.FromCurrent();
            
            errorColors?.SetColors();
            Console.Write("Invalid entry. ");
            tryAgainColors?.SetColors();
            Console.Write("Try again: ");

            currentColors.SetColors();
        }
        private static bool IsValidFloatingPoint(char x)
        {
            return char.IsDigit(x) || x == '-' || x == '+' || x == '.' || x == 'e' || x == 'E';
        }
        private static bool IsValidUnsignedIntegerChar(char x)
        {
            return char.IsDigit(x);
        }
        private static bool IsValidIntegerChar(char x)
        {
            return char.IsDigit(x) || x == '-';
        }
        #endregion
    }
}
