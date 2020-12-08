#nullable enable

namespace Supermodel.Presentation.Cmd.ConsoleOutput
{
    public interface ICmdOutput
    {
        void WriteToConsole(bool writeLine);
    }
}
