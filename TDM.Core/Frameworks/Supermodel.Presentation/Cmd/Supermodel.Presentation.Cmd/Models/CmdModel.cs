﻿#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Supermodel.DataAnnotations.Validations.Attributes;
using Supermodel.Presentation.Cmd.ConsoleOutput;
using Supermodel.Presentation.Cmd.Models.Interfaces;
using Supermodel.Presentation.Cmd.Rendering;
using Supermodel.ReflectionMapper;

namespace Supermodel.Presentation.Cmd.Models
{
    public class CmdModel : ICmdEditor, ICmdDisplayer
    {
        #region ICmdEditor
        public virtual void Display(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue)
        {
            foreach (var propertyInfo in GetDetailPropertyInfosInOrder(screenOrderFrom, screenOrderTo))
            {
                var required = propertyInfo.HasAttribute<RequiredAttribute>();

                //Label
                var hideLabelAttribute = propertyInfo.GetAttribute<HideLabelAttribute>();
                if (hideLabelAttribute == null)
                {
                    if (CmdContext.ValidationResultList.GetAllErrorsFor(propertyInfo.Name).Any())
                    {
                        CmdRender.ShowLabel(this, propertyInfo.Name, null, CmdScaffoldingSettings.DisplayLabel);
                    }
                    else
                    {
                        CmdRender.ShowLabel(this, propertyInfo.Name, null, CmdScaffoldingSettings.InvalidValueDisplayLabel);
                    }

                    if (!propertyInfo.HasAttribute<NoRequiredLabelAttribute>())
                    {
                        var currentColors = FBColors.FromCurrent();
                        if (required || propertyInfo.HasAttribute<ForceRequiredLabelAttribute>()) 
                        {
                            CmdScaffoldingSettings.RequiredMarker.WriteToConsole();
                        }
                        currentColors.SetColors();
                    }
                    Console.Write(": ");
                }

                //Value
                using(CmdContext.NewRequiredScope(required, GetType().GetDisplayNameForProperty(propertyInfo.Name)))
                {
                    CmdRender.Display(this, propertyInfo.Name, CmdScaffoldingSettings.DisplayValue);
                }

                //Validation Error
                if (CmdContext.ValidationResultList.GetAllErrorsFor(propertyInfo.Name).Any())
                {
                    CmdScaffoldingSettings.ValidationErrorMessage?.SetColors();
                    Console.Write(" - ");
                    CmdRender.ShowValidationMessage(this, propertyInfo.Name, CmdScaffoldingSettings.ValidationErrorMessage);
                }

                //New Line
                Console.WriteLine();
            }
        }
        public virtual object Edit(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue)
        {
            foreach (var propertyInfo in GetDetailPropertyInfosInOrder(screenOrderFrom, screenOrderTo))
            {
                var required = propertyInfo.HasAttribute<RequiredAttribute>();
                
                //Label
                var hideLabelAttribute = propertyInfo.GetAttribute<HideLabelAttribute>();
                if (hideLabelAttribute == null)
                {
                    if (CmdContext.ValidationResultList.GetAllErrorsFor(propertyInfo.Name).Any())
                    {
                        CmdRender.ShowLabel(this, propertyInfo.Name, null, CmdScaffoldingSettings.DisplayLabel);
                    }
                    else
                    {
                        CmdRender.ShowLabel(this, propertyInfo.Name, null, CmdScaffoldingSettings.InvalidValueDisplayLabel);
                    }
                    
                    if (!propertyInfo.HasAttribute<NoRequiredLabelAttribute>())
                    {
                        var currentColors = FBColors.FromCurrent();
                        if (required || propertyInfo.HasAttribute<ForceRequiredLabelAttribute>()) 
                        {
                            CmdScaffoldingSettings.RequiredMarker.WriteToConsole();
                        }
                        currentColors.SetColors();
                    }
                    Console.Write(": ");
                }
                
                //Value
                using(CmdContext.NewRequiredScope(required, GetType().GetDisplayNameForProperty(propertyInfo.Name)))
                {
                    if (!propertyInfo.HasAttribute<DisplayOnlyAttribute>())
                    {
                        var newPropertyValue = CmdRender.Edit(this, propertyInfo.Name, CmdScaffoldingSettings.EditValue, CmdScaffoldingSettings.InvalidValueMessage);
                        this.PropertySet(propertyInfo.Name, newPropertyValue);
                    }
                    else
                    {
                        CmdRender.Display(this, propertyInfo.Name, CmdScaffoldingSettings.DisplayValue);
                    }
                }
            }
            return this;
        }
        #endregion

        #region Protected Helper Methods
        protected virtual IEnumerable<PropertyInfo> GetDetailPropertyInfosInOrder(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue)
        {
            var result = GetType().GetProperties()
                .Where(x => x.GetCustomAttribute<ScaffoldColumnAttribute>() == null || x.GetCustomAttribute<ScaffoldColumnAttribute>()!.Scaffold)
                .Where(x => (x.GetCustomAttribute<ScreenOrderAttribute>() != null ? x.GetCustomAttribute<ScreenOrderAttribute>()!.Order : 100) >= screenOrderFrom)
                .Where(x => (x.GetCustomAttribute<ScreenOrderAttribute>() != null ? x.GetCustomAttribute<ScreenOrderAttribute>()!.Order : 100) <= screenOrderTo)
                .OrderBy(x => x.GetCustomAttribute<ScreenOrderAttribute>() != null ? x.GetCustomAttribute<ScreenOrderAttribute>()!.Order : 100);
        
            //By default we do not scaffold enumerations (except for strings of course and unless they implement ISupermodelEditorTemplate)
            return result.Where(x => !(x.PropertyType != typeof(string) && 
                                       !typeof(ICmdEditor).IsAssignableFrom(x.PropertyType) && 
                                       typeof(IEnumerable).IsAssignableFrom(x.PropertyType)));
        }
        #endregion
    }
}
