#nullable enable

namespace Supermodel.Presentation.Cmd.Models.Interfaces
{
    public interface ICmdReader
    {
        public void Read(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue);
    }
}
