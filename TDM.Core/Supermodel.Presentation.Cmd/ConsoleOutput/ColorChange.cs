using System;

namespace Supermodel.Presentation.Cmd.ConsoleOutput
{
    public readonly struct ColorChange
    {
        #region Constructors
        public ColorChange(int index, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)
        {
            if (index < 0) throw new ArgumentException("index < 0");            
            
            Index = index;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }
        #endregion

        #region Methods
        public void SetColors()
        {
            if (ForegroundColor != null) Console.ForegroundColor = ForegroundColor.Value;
            if (BackgroundColor != null) Console.ForegroundColor = BackgroundColor.Value;
        }
        #endregion

        #region Properties
        public int Index { get; }
        public ConsoleColor? ForegroundColor { get; }
        public ConsoleColor? BackgroundColor { get; }
        #endregion
    }
}