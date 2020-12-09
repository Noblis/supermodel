#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Supermodel.Presentation.Cmd.ConsoleOutput
{
    public class StringBuilderWithColor : ICmdOutput
    {
        #region Constructors
        public StringBuilderWithColor()
        {
            Content = new StringBuilder();
            ColorChanges = new List<ColorChange>();
        }
        public StringBuilderWithColor(StringWithColor str)
        {
            Content = new StringBuilder(str.Content);
            ColorChanges = str.ColorChanges.ToList();
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return Content.ToString();
        }
        #endregion

        #region Methods
        public void Append(string str)
        {
            Content.Append(str);
        }
        public void AppendLine(string str)
        {
            Content.AppendLine(str);
        }
        public void Append(StringWithColor str)
        {
            Content.Append(str.Content);
            AppendColorChanges(str.ColorChanges);
        }
        public void AppendLine(StringWithColor str)
        {
            Content.AppendLine(str.Content);
            AppendColorChanges(str.ColorChanges);
        }
        protected void AppendColorChanges(ImmutableArray<ColorChange> colorChanges)
        {
            var offset = Content.Length;
            foreach (var colorChange in colorChanges) 
            {
                if (ColorChanges.Count == 0 || ColorChanges.Last().Colors != colorChange.Colors) ColorChanges.Add(colorChange.CloneWithOffset(offset));
            }
        }
        #endregion

        #region IConsoleOutput
        public virtual void WriteLineToConsole()
        {
            WriteToConsole(true);
        }
        public virtual void WriteToConsole()
        {
            WriteToConsole(false);
        }
        protected virtual void WriteToConsole(bool writeLine)
        {
            var content = Content.ToString();
            var currentColorChange = new ColorChange(0, null, null);
            
            foreach (var colorChange in ColorChanges)
            {
                var strPortion = content[currentColorChange.Index..colorChange.Index];
                currentColorChange.Colors.SetColors();
                Console.Write(strPortion);

                currentColorChange = colorChange;
            }
            
            var strEndPortion = content[currentColorChange.Index..];
            currentColorChange.Colors.SetColors();
            Console.Write(strEndPortion);

            if (writeLine) Console.WriteLine();
        }
        #endregion
        
        #region Properties
        public StringBuilder Content { get; }
        public List<ColorChange> ColorChanges { get; }
        #endregion
    }
}