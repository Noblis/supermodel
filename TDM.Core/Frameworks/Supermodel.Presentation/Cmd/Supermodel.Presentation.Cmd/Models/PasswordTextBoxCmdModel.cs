#nullable enable

using System;
using System.Threading.Tasks;
using Supermodel.Presentation.Cmd.ConsoleOutput;
using Supermodel.Presentation.Cmd.Rendering;
using Supermodel.ReflectionMapper;

namespace Supermodel.Presentation.Cmd.Models
{
    public class PasswordTextBoxCmdModel : TextBoxCmdModel
    {
        #region Embedded Types
        public enum PlaceholderBehaviorEnum { Default, ForceNoPlaceholder, ForceDotDotDotPlaceholder }
        #endregion
        
        #region Constructors
        public PasswordTextBoxCmdModel()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            InitFor<string>();
        }
        #endregion

        #region IRMapperCustom implemtation
        public override Task MapFromCustomAsync<T>(T other)
        {
            if (typeof(T) != typeof(string)) throw new ArgumentException("other must be of string type", nameof(other));
            InitFor<string>();

            Value = "";
            return Task.CompletedTask;
        }
        // ReSharper disable once RedundantAssignment
        public override Task<T> MapToCustomAsync<T>(T other)
        {
            if (typeof(T) != typeof(string)) throw new ArgumentException("other must be of string type", nameof(other));

            other = (T)(object)Value;
            return Task.FromResult(other);
        }
        #endregion
        
        #region IEditor implemtation
        public override object Edit(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue)
        {
            if (Type == typeof(string)) { Value = ConsoleExt.ReadPassword(); return this; }

            throw new Exception($"TextBoxCmdModel.Edit: Unknown type {Type?.GetTypeFriendlyDescription()}");
        }
        #endregion

        #region IDisplayer implemetation
        public override void Display(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue)
        {
            CmdRender.DisplayForModel("*******");
        }
        #endregion

        #region Properties
        public PlaceholderBehaviorEnum PlaceholderBehavior { get; set; } = PlaceholderBehaviorEnum.Default;
        #endregion
    }
}
