using System;
using System.Collections.Generic;

#nullable enable

namespace Supermodel.Presentation.Cmd.ConsoleOutput
{
    public static class ConsoleExt
    {
        public static int EditDouble(int value)
        {
            var input = EditLine(value.ToString(), x => char.IsDigit(x) || x == '-');
            return int.Parse(input);
        }

        #region Edit Integer
        public static ulong EditInteger(ulong value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString();
            while(true)
            {
                var input = EditLine(valueStr, char.IsDigit);
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
        public static uint EditInteger(uint value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString();
            while(true)
            {
                var input = EditLine(valueStr, char.IsDigit);
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
        public static ushort EditInteger(ushort value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString();
            while(true)
            {
                var input = EditLine(valueStr, char.IsDigit);
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
        public static ushort EditInteger(byte value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString();
            while(true)
            {
                var input = EditLine(valueStr, char.IsDigit);
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
        
        public static long EditInteger(long value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString();
            while(true)
            {
                var input = EditLine(valueStr, x => char.IsDigit(x) || x == '-');
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
        public static int EditInteger(int value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString();
            while(true)
            {
                var input = EditLine(valueStr, x => char.IsDigit(x) || x == '-');
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
        public static short EditInteger(short value, FBColors? errorColors = null)
        {
            var valueStr = value.ToString();
            while(true)
            {
                var input = EditLine(valueStr, x => char.IsDigit(x) || x == '-');
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
        #endregion
    }
}
