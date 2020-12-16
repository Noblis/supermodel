#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Supermodel.Presentation.Cmd.ConsoleOutput;

namespace Supermodel.Presentation.Cmd.Models.Base
{
    public abstract class SingleSelectCmdModel : UIComponentBase
    {
        #region Option nested class
        public class Option
        {
            public Option(string value, string label, bool isDisabled = false)
            {
                Value = value;
                Label = label;
                IsDisabled = isDisabled;
            }
            public string Value { get; }
            public string Label { get; }
            public bool IsDisabled { get; }
        }
        public List<Option> Options { get; protected set; } = new List<Option>();
        #endregion

        #region Static Dropdown and Radio helpers
        public virtual object CommonDropdownEditorTemplate(SingleSelectCmdModel singleSelect)
        {
            var selectListItemList = new List<ConsoleExt.SelectListItem> { ConsoleExt.SelectListItem.Empty };
            foreach (var option in singleSelect.Options)
            {
                var isSelectedOption = singleSelect.SelectedValue != null && string.CompareOrdinal(singleSelect.SelectedValue, option.Value) == 0;
                if (isSelectedOption || !option.IsDisabled)
                {
                    var item = new ConsoleExt.SelectListItem(option.Value, !option.IsDisabled ? option.Label : option.Label + DisabledSuffix);
                    selectListItemList.Add(item);
                }
            }
            
            while(true)
            {
                SelectedValue = ConsoleExt.EditDropdownList(singleSelect.SelectedValue ?? "", selectListItemList);
                return this;
            }
        }
        #endregion

        #region IUIComponentWithValue implemetation
        public override string ComponentValue 
        {
            get => SelectedValue ?? "";
            set => SelectedValue = value;
        }
        #endregion

        #region IDisplayTemplate implemetation
        public override void Display(int screenOrderFrom = Int32.MinValue, int screenOrderTo = Int32.MaxValue)
        {
            Console.Write(SelectedLabel ?? "");
        }
        #endregion

        #region ToString override
        public override string ToString()
        {
            return SelectedValue ?? "";
        }
        #endregion

        #region Properties
        public string? SelectedValue { get; set; }
        public string? SelectedLabel
        {
            get
            {
                var selectedOption = Options.FirstOrDefault(x => x.Value == SelectedValue);
                return selectedOption?.Label;
            }
        }
        public bool IsEmpty => string.IsNullOrEmpty(SelectedValue);
        public string DisabledSuffix { get; set; } = " [DISABLED]";
        #endregion
    }
}
