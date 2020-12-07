using Supermodel.Presentation.Cmd.ConsoleOutput;

#nullable enable

namespace Supermodel.Presentation.Cmd.Models.Interfaces
{
    interface ICmdDisplayTemplate
    {
        IConsoleOutput DisplayTemplate<TModel>(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue);    
    }
}
