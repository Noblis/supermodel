#nullable enable

using Supermodel.Presentation.Cmd.ConsoleOutput;

namespace Supermodel.Presentation.Cmd.Models.Interfaces
{
    interface ICmdEditorTemplate
    {
        ICmdOutput EditorTemplate(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue);    
    }
}
