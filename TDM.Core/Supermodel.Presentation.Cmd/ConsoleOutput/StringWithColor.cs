#nullable enable

using System;
using System.Collections.Immutable;

namespace Supermodel.Presentation.Cmd.ConsoleOutput
{
    public class StringWithColor : IConsoleOutput
    {
        #region Constructors
        public StringWithColor(string content, ConsoleColor foregroundColor, ConsoleColor? backgroundColor = null)
        {
            Content = content;
            ColorChanges = new [] { new ColorChange(0, foregroundColor, backgroundColor) }.ToImmutableArray();
        }
        public StringWithColor(string content, params ColorChange[] colorChanges)
        {
            Content = content;
            ColorChanges = colorChanges.ToImmutableArray();
        }
        #endregion

        #region Operator Overloading
        public static StringWithColor operator +(StringWithColor a, StringWithColor b)
        {
            var content = a.Content + b.Content;
            
            var colorChanges = new ColorChange[a.ColorChanges.Length + b.ColorChanges.Length];
            var idx = 0;
            foreach (var colorChange in a.ColorChanges) colorChanges[idx++] = colorChange;
            var offset = a.Content.Length;
            foreach (var colorChange in b.ColorChanges) colorChanges[idx++] = colorChange.CloneWithOffset(offset);
            
            return new StringWithColor(content, colorChanges);
        }
        public static StringWithColor operator +(string a, StringWithColor b)
        {
            var content = a + b.Content;
            
            var colorChanges = new ColorChange[b.ColorChanges.Length];
            var idx = 0;
            var offset = a.Length;
            foreach (var colorChange in b.ColorChanges) colorChanges[idx++] = colorChange.CloneWithOffset(offset);
            
            return new StringWithColor(content, colorChanges);
        }
        public static StringWithColor operator +(StringWithColor a, string b)
        {
            var content = a.Content + b;
            
            var colorChanges = new ColorChange[a.ColorChanges.Length];
            var idx = 0;
            foreach (var colorChange in a.ColorChanges) colorChanges[idx++] = colorChange;
            
            return new StringWithColor(content, colorChanges);
        }
        #endregion
        
        #region IConsoleOutput
        public virtual void WriteToConsole(bool writeLine = true)
        {
            var currentColorChange = new ColorChange(0, null, null);
            
            foreach (var colorChange in ColorChanges)
            {
                var strPortion = Content[currentColorChange.Index..colorChange.Index];
                currentColorChange.SetColors();
                Console.Write(strPortion);

                currentColorChange = colorChange;
            }
            
            var strEndPortion = Content[currentColorChange.Index..];
            currentColorChange.SetColors();
            Console.Write(strEndPortion);

            if (writeLine) Console.WriteLine();
        }
        #endregion

        #region Properties
        public string Content { get; }
        public ImmutableArray<ColorChange> ColorChanges { get; }
        #endregion
    }
}
