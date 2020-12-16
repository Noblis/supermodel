﻿#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Supermodel.DataAnnotations.Attributes;
using Supermodel.DataAnnotations.Expressions;
using Supermodel.ReflectionMapper;
using WebMonk.Context;
using WebMonk.Exceptions;
using WebMonk.Extensions;
using WebMonk.ModeBinding;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Templates;
using WebMonk.Rendering.Views;
using WebMonk.ValueProviders;

namespace Supermodel.Presentation.WebMonk.Bootstrap4.Models.Base
{
    public abstract class MultiSelectMvcModel : IComparable, ISelfModelBinder, IEditorTemplate, IDisplayTemplate, IHiddenTemplate
    {
        #region Nested Option class
        public class Option
        {
            public Option(string value, string label, bool isDisabled, bool selected = false)
            {
                Value = value;
                Label = label;
                IsDisabled = isDisabled;
                Selected = selected;
            }
            public string Value { get; private set; }
            public string Label { get; private set; }
            public bool IsDisabled { get; private set; }
            public bool Selected { get; set; }
            public bool IsShown => Selected || !IsDisabled;
        }
        #endregion 

        #region Methods
        protected virtual string GetFullLabel(Option option)
        {
            return option.IsDisabled ? $"{option.Label}{DisabledSuffix}" : option.Label;
        }
        #endregion
        
        #region IComparable implemtation
        public int CompareTo(object? obj)
        {
            var other = (MultiSelectMvcModel?)obj;
            if (other == null) return 1;
            if (Options.Count != other.Options.Count) return 1;

            foreach (var option in Options)
            {
                if (other.Options.Find(x => x.Value == option.Value && x.Label == option.Label && x.Selected == option.Selected) == null) return 1;
            }
            return 0;
        }
        #endregion

        #region IEditorTemplate implementation
        public abstract IGenerateHtml EditorTemplate(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue, object? attributes = null);
        #endregion

        #region IDisplayTemplate implementation
        public virtual IGenerateHtml DisplayTemplate(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue, object? attributes = null)
        {
            return EditorTemplate(screenOrderFrom, screenOrderTo, attributes).DisableAllControls();
        }
        #endregion

        #region IHiddenTemplate implementation
        public virtual IGenerateHtml HiddenTemplate(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue, object? attributes = null)
        {
            var html = new HtmlStack();
            
            foreach (var option in Options)
            {
                if (option.Selected) html.Append(Render.HiddenForModel(option.Value, attributes));
            }
            html.Append(Render.HiddenForModel("", attributes));

            return html;
        }
        #endregion

        #region ISelfModelBinder implementation
        public Task<object?> BindMeAsync(Type rootType, List<IValueProvider> valueProviders)
        {
            var prefix = HttpContext.Current.PrefixManager.CurrentPrefix;

            if (string.IsNullOrEmpty(prefix)) throw new WebMonkException("prefix is not set");
            var name = prefix.ToHtmlName();

            var attemptedValues = valueProviders.GetValueOrDefault<List<string>>(name).GetNewValue<List<string>>();
            attemptedValues.Remove(""); //remove blank option. it must be always present

            if (attemptedValues.Count < 1)
            {
                if (prefix.StartsWith($"{Config.InlinePrefix}.", StringComparison.OrdinalIgnoreCase)) prefix = prefix.Substring(Config.InlinePrefix.Length + 1);
                    
                var propertyInfo = rootType.GetPropertyByFullName(prefix);
                if (propertyInfo.GetAttribute<RequiredAttribute>() != null)
                {
                    var label = rootType.GetDisplayNameForProperty(prefix);
                    HttpContext.Current.ValidationResultList.Add(new ValidationResult($"The {label} field is required", new[] { name }));
                }
            }

            Options.ForEach(x => x.Selected = false);
            foreach (var selectedValue in attemptedValues)
            {
                var selectedOption = Options.Find(x => x.Value == selectedValue);
                if (selectedOption != null) selectedOption.Selected = true;
            }
            return Task.FromResult((object?)this);
        }
        #endregion

        #region Properties
        public List<Option> Options { get; protected set; } = new List<Option>();
        public string DisabledSuffix { get; set; } = " [DISABLED]";
        #endregion
    }
}
