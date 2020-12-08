using System;
using System.Collections.Generic;

#nullable enable

namespace Supermodel.Presentation.Cmd.ConsoleOutput
{
    public static class ConsoleExt
    {
        public static int EditInteger(int value)
        {
            var str = EditLine(value, x => char.IsLetterOrDigit(x) || char.IsSymbol(x) || char.IsPunctuation(x) || char.IsSeparator(x));
        }
        
        public static string EditLine(string value)
        {
            return EditLine(value, x => char.IsLetterOrDigit(x) || char.IsSymbol(x) || char.IsPunctuation(x) || char.IsSeparator(x));
        }

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
    }
}
