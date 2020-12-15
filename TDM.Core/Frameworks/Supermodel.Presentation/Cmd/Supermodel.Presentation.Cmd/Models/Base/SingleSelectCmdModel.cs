﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Supermodel.DataAnnotations.Misc;
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
        public virtual IGenerateHtml CommonDropdownEditorTemplate(SingleSelectMvcModel singleSelect, AttributesDict? attributesDict)
        {
            attributesDict ??= new AttributesDict();
            attributesDict.AddOrAppendCssClass("form-control");

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
            var result = Render.DropdownListForModel(singleSelect.SelectedValue, selectListItemList, attributesDict);
            return result;
        }
        public virtual IGenerateHtml CommonRadioEditorTemplate(SingleSelectMvcModel singleSelect, AttributesDict? divAttributesDict, AttributesDict? inputAttributesDict, AttributesDict? labelAttributesDict)
        {
            var prefix = HttpContext.Current.PrefixManager.CurrentPrefix;
            var id = prefix.ToHtmlId();
            var name = prefix.ToHtmlName();

            var result = new HtmlStack();

            //Set up class attributes
            divAttributesDict ??= new AttributesDict();
            divAttributesDict.AddOrAppendCssClass("form-check");

            inputAttributesDict ??= new AttributesDict();
            inputAttributesDict.AddOrAppendCssClass("form-check-input");

            labelAttributesDict ??= new AttributesDict();
            labelAttributesDict.AddOrAppendCssClass("form-check-label");

            foreach (var option in singleSelect.Options)
            {
                result.AppendAndPush(new Div(divAttributesDict));
                var isSelectedOption = singleSelect.SelectedValue != null && string.CompareOrdinal(singleSelect.SelectedValue, option.Value) == 0;
                if (isSelectedOption || !option.IsDisabled)
                {
                    result.Append(Render.RadioButtonForModel(singleSelect.SelectedValue, option.Value, inputAttributesDict));
                    result.Append(new Label(new { @for = name }) { new Txt(!option.IsDisabled ? option.Label : option.Label + DisabledSuffix) }).AddOrUpdateAttr(labelAttributesDict);
                }
                result.Pop<Div>();
            }
            result.Append(new Input(new { id, name, type="hidden", value="" }));

            return result;
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
