#nullable enable

namespace Supermodel.Presentation.Cmd.Models.Interfaces
{
    public interface ICmdEditor : ICmdDisplayer, ICmdReader
    {
        void Edit(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue);    
    }
}
