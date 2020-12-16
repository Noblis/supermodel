#nullable enable

using System;
using Supermodel.Presentation.Cmd.Models.Base;

namespace Supermodel.Presentation.Cmd.Models
{
    public class DropdownCmdModelUsing<TMvcModel> : SingleSelectMvcModelUsing<TMvcModel> where TMvcModel : CmdModelForEntityCore
    {
        #region IEditorTemplate implementation
        public override object Edit(int screenOrderFrom = Int32.MinValue, int screenOrderTo = Int32.MaxValue)
        {
            return CommonDropdownEditorTemplate(this);
        }
        #endregion
    }
}
