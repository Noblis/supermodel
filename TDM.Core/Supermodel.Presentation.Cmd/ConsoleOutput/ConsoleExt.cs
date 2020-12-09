#nullable enable

using System;
using System.Collections.Generic;

namespace Supermodel.Presentation.Cmd.ConsoleOutput
{
    public static class ConsoleExt
    {
        #region Edit Boolean
        //public static bool EditBool(bool value, , FBColors? errorColors = null)
        #endregion

        #region Edit Floating Point
        public static decimal? EditFloat(decimal? value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString() ?? "";
            while(true)
            {
                var input = EditLine(valueStr, IsValidUnsignedIntegerChar);
                if (string.IsNullOrWhiteSpace(input)) return null;
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
                if (string.IsNullOrWhiteSpace(input)) return null;
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
                if (string.IsNullOrWhiteSpace(input)) return null;
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
                if (string.IsNullOrWhiteSpace(input)) return null;
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
                if (string.IsNullOrWhiteSpace(input)) return null;
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
                if (string.IsNullOrWhiteSpace(input)) return null;
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
                if (string.IsNullOrWhiteSpace(input)) return null;
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
                if (string.IsNullOrWhiteSpace(input)) return null;
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
                if (string.IsNullOrWhiteSpace(input)) return null;
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
                if (string.IsNullOrWhiteSpace(input)) return null;
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
                if (string.IsNullOrWhiteSpace(input)) return null;
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
        
        #region Edit String
        public static string EditString(string value)
        {
            return EditLine(value, x => char.IsLetterOrDigit(x) || char.IsSymbol(x) || char.IsPunctuation(x) || char.IsSeparator(x));
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
        #endregion

        #region Private Helpers
        private static void PrintErrorMessage(FBColors? errorColors = null)
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
