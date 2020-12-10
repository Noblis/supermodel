#nullable enable

using Supermodel.DataAnnotations.Validations;

namespace Supermodel.Presentation.Cmd.Models
{
    public static class CmdContext
    {
        public static ValidationResultList ValidationResultList { get; set; } = new ValidationResultList();
    }
}
