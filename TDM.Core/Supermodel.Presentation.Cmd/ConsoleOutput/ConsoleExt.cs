#nullable enable

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Supermodel.DataAnnotations.Expressions;
using Supermodel.Presentation.Cmd.Models;
using Supermodel.Presentation.Cmd.Rendering;

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
        public static DateTime? EditDateTime(DateTime? value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors);
                    continue;
                }

                if (DateTime.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors);
                    valueStr = input;
                }
            }
        }
        #endregion

        #region Edit Boolean
        public static bool? EditBool(bool? value, FBColors? errorColors = null)
        {
            var valueStr = value == null ? "" : value.Value ? "Y" : "N";
            while(true)
            {
                var input = EditLine(valueStr, x => x == 'Y' || x == 'y' || x == 'N' || x == 'n');
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors);
                    continue;
                }
                
                var trimmedInput = input.Trim().ToLower();
                if (trimmedInput == "y") 
                {
                    return true;
                }
                else if (trimmedInput == "n") 
                {
                    return false;
                }
                else
                {
                    PrintErrorMessage(errorColors);
                    valueStr = input;
                }
            }
        }
        #endregion

        #region Edit Floating Point
        public static decimal? EditFloat(decimal? value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors);
                    continue;
                }

                if (decimal.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors);
                    valueStr = input;
                }
            }
        }
        public static double? EditFloat(double? value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors);
                    continue;
                }

                if (double.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors);
                    valueStr = input;
                }
            }
        }
        public static float? EditFloat(float? value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors);
                    continue;
                }

                if (float.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors);
                    valueStr = input;
                }
            }
        }
        #endregion
        
        #region Edit Integer
        public static ulong? EditInteger(ulong? value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors);
                    continue;
                }

                if (ulong.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors);
                    valueStr = input;
                }
            }
        }
        public static uint? EditInteger(uint? value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors);
                    continue;
                }

                if (uint.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors);
                    valueStr = input;
                }
            }
        }
        public static ushort? EditInteger(ushort? value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors);
                    continue;
                }

                if (ushort.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors);
                    valueStr = input;
                }
            }
        }
        public static byte? EditInteger(byte? value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors);
                    continue;
                }

                if (byte.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors);
                    valueStr = input;
                }
            }
        }
        
        public static long? EditInteger(long? value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors);
                    continue;
                }

                if (long.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors);
                    valueStr = input;
                }
            }
        }
        public static int? EditInteger(int? value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors);
                    continue;
                }

                if (int.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors);
                    valueStr = input;
                }
            }
        }
        public static short? EditInteger(short? value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors);
                    continue;
                }

                if (short.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors);
                    valueStr = input;
                }
            }
        }
        public static sbyte? EditInteger(sbyte? value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return null;
                    PrintRequiredFieldMessage(errorColors);
                    continue;
                }

                if (sbyte.TryParse(input, out var result))
                {
                    return result;
                }
                else
                {
                    PrintErrorMessage(errorColors);
                    valueStr = input;
                }
            }
        }
        #endregion
        
        #region Edit String & Password
        public static string EditString(string value, FBColors? errorColors = null)
        {
            while(true)
            {
                var input = EditLine(value, x => char.IsLetterOrDigit(x) || char.IsSymbol(x) || char.IsPunctuation(x) || char.IsSeparator(x));
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return "";
                    PrintRequiredFieldMessage(errorColors);
                    continue;
                }
                return input;
            }
        }
        public static string ReadPasswordLine(FBColors? errorColors = null)
        {
            while(true)
            {
                var input = ReadPasswordLine(x => char.IsLetterOrDigit(x) || char.IsSymbol(x) || char.IsPunctuation(x) || char.IsSeparator(x));
                if (string.IsNullOrWhiteSpace(input)) 
                {
                    if (!CmdContext.IsPropertyRequired) return "";
                    PrintRequiredFieldMessage(errorColors);
                    continue;
                }
                return input;
            }
        }
        #endregion

        #region Generic Edit Line
        public static string EditLine(string value, Func<char, bool> isValidCharFunc)
        {
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
        #endregion

        #region Generic Dropdown List
        public static string EditDropdownListForModel(string? model, IEnumerable<SelectListItem> options, FBColors? valueColors = null, FBColors? arrowColors = null)
        {
            return EditDropdownList(model, "", options, valueColors, arrowColors);
        }
        public static string EditDropdownListFor<TModel, TValue>(TModel model, Expression<Func<TModel, TValue>> propertyExpression, IEnumerable<SelectListItem> options, FBColors? valueColors = null, FBColors? arrowColors = null)
        {
            var propertyName = CmdRender.Helper.GetPropertyName(model, propertyExpression);
            return EditDropdownList(model, propertyName, options, valueColors, arrowColors);
        }
        public static string EditDropdownList<TModel>(TModel model, string expression, IEnumerable<SelectListItem> options, FBColors? valueColors = null, FBColors? arrowColors = null)
        {
            if (model == null && !string.IsNullOrEmpty(expression)) throw new ArgumentNullException(nameof(model));

            object? propertyValue;
            if (model == null) 
            {
                if (!string.IsNullOrEmpty(expression)) throw new ArgumentNullException(nameof(model));
                propertyValue = "";
            }
            else 
            {
                (_, _, propertyValue) = model.GetPropertyInfoPropertyTypeAndValueByFullName(expression);
            }
            
            var cursorLeft = Console.CursorLeft;
            var cursorTop = Console.CursorTop;
            var currentIdx = -1;

            foreach (var selectOption in options)
            {
                if (propertyValue?.ToString() == selectOption.Value) 
                {
                    valueColors?.SetColors();
                    Console.Write(selectOption.Label);

                    arrowColors?.SetColors();
                    Console.Write('▼');
                }
            }

            while(true)
            {
                var info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Enter)
                {

                }
                if (info.Key == ConsoleKey.Escape)
                {

                }
                if (info.Key == ConsoleKey.DownArrow)
                {

                }
                if (info.Key == ConsoleKey.UpArrow)
                {

                }
            }
        }

        #endregion

        #region Private Helpers
        private static void PrintErrorMessage(FBColors? errorColors = null)
        {
            var currentColors = FBColors.FromCurrent();
            errorColors?.SetColors();

            if (CmdContext.PropertyDisplayName != null) Console.Write($"The {CmdContext.PropertyDisplayName} is required. Try again:");
            else Console.Write($"This field is required. Try again:");

            currentColors.SetColors();
        }
        private static void PrintRequiredFieldMessage(FBColors? errorColors = null)
        {
            var currentColors = FBColors.FromCurrent();
            errorColors?.SetColors();
            Console.Write("Invalid entry. Try again: ");
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
