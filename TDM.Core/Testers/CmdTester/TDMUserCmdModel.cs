#nullable enable

using Supermodel.DataAnnotations.Validations.Attributes;
using Supermodel.Presentation.Cmd.Models;
using WMDomain.Entities;

namespace CmdTester
{
    public class TDMUserCmdModel : CmdModelForEntity<TDMUser>
    {
        #region Overrides
        protected override string LabelInternal => $"{FirstName} {LastName}";
        #endregion

        #region Properties
        [Email] public string Username { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        #endregion
    }
}