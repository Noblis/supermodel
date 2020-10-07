#nullable enable

using System;
using System.Collections.Generic;
using Supermodel.DataAnnotations.Exceptions;
using Supermodel.Persistence.DataContext;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.Presentation.WebMonk.Models.Mvc;
using Supermodel.Presentation.WebMonk.Views;
using Supermodel.Presentation.WebMonk.Views.Interfaces;
using WebMonk.RazorSharp.HtmlTags.BaseTags;

namespace Supermodel.Presentation.WebMonk.Bootstrap4.Views
{
    public abstract class CRUDMvcView<TMvcModel, TDataContext> : CRUDMvcView<TMvcModel, TMvcModel, TDataContext> 
        where TMvcModel : class, IMvcModelForEntity, new() 
        where TDataContext : class, IDataContext, new()
    { }

    
    public abstract class CRUDMvcView<TDetailMvcModel, TListMvcModel, TDataContext> : CRUDMvcViewBase<TDetailMvcModel, TListMvcModel>
        where TDetailMvcModel : class, IMvcModelForEntity, new()
        where TListMvcModel : class, IMvcModelForEntity, new()
        where TDataContext : class, IDataContext, new()
    {
        #region Views Methods
        public override IGenerateHtml RenderList(List<TListMvcModel> models)
        {
            switch (ListMode)
            {
                case ListMode.NoList:
                {
                    throw new InvalidOperationException("List is not valid for NoList ListModel");
                }
                case ListMode.Simple:
                {
                    if (ListPageTitle != null) return ApplyToDefaultLayout(new Bs4.CRUDList(models, ListPageTitle, ListSkipAddNew || ReadOnly, ListSkipDelete || ReadOnly, ReadOnly));
                    else return ApplyToDefaultLayout(new Bs4.CRUDList(models, (IGenerateHtml?)null, ListSkipAddNew || ReadOnly, ListSkipDelete || ReadOnly, ReadOnly));
                }
                case ListMode.MultiColumn:
                {
                    if (ListPageTitle != null) return ApplyToDefaultLayout(new Bs4.CRUDMultiColumnList(models, ListPageTitle, ListSkipAddNew || ReadOnly, ListSkipDelete || ReadOnly, ReadOnly));
                    else return ApplyToDefaultLayout(new Bs4.CRUDMultiColumnList(models, (IGenerateHtml?)null, ListSkipAddNew || ReadOnly, ListSkipDelete || ReadOnly, ReadOnly));
                }
                case ListMode.MultiColumnNoActions:
                {
                    if (ListPageTitle != null) return ApplyToDefaultLayout(new Bs4.CRUDMultiColumnListNoActions(models, ListPageTitle));
                    else return ApplyToDefaultLayout(new Bs4.CRUDMultiColumnListNoActions(models));
                }
                case ListMode.EditableMultiColumn:
                {
                    if (ReadOnly) throw new InvalidOperationException("ReadOnly is not compatible with EditableMultiColumn ListModel");
                    
                    if (ListPageTitle != null) return ApplyToDefaultLayout(new Bs4.CRUDMultiColumnEditableList(models, typeof(TDataContext), ListPageTitle, ListSkipAddNew, ListSkipDelete));
                    else return ApplyToDefaultLayout(new Bs4.CRUDMultiColumnEditableList(models, typeof(TDataContext), (IGenerateHtml?)null, ListSkipAddNew, ListSkipDelete));
                }
                default:
                {
                    throw new SupermodelException($"Unknown ListMode: {ListMode}");
                }
            }
        }
        public override IGenerateHtml RenderDetail(TDetailMvcModel model)
        {
            if (ListMode == ListMode.EditableMultiColumn || ListMode == ListMode.MultiColumnNoActions) 
            {
                throw new InvalidOperationException("Detail is not valid for EditableMultiColumn or MultiColumnNoActions ListModels");
            }
            
            var detailPageTitle = DetailPageTitle;
            if (ShowDefaultDetailPageTitle)
            {
                if (model.IsNewModel()) detailPageTitle = "Create New";
                else detailPageTitle = model.Label;
            }
            
            var accordionPanels = GetAccordionPanels(model);
            
            IGenerateHtml editTags;
            if (detailPageTitle != null)
            {
                if (accordionPanels == null) editTags = new Bs4.CRUDEdit(model, detailPageTitle, ReadOnly, ListMode == ListMode.NoList);
                else editTags = new Bs4.CRUDEditInAccordion(model, typeof(TDetailMvcModel).Name, accordionPanels, detailPageTitle, ReadOnly, ListMode == ListMode.NoList);
            }
            else
            {
                if (accordionPanels == null) editTags = new Bs4.CRUDEdit(model, (IGenerateHtml?)null, ReadOnly, ListMode == ListMode.NoList);
                else editTags = new Bs4.CRUDEditInAccordion(model, typeof(TDetailMvcModel).Name, accordionPanels, (IGenerateHtml?)null, ReadOnly, ListMode == ListMode.NoList);
            }
            
            var childrenTags = model.IsNewModel()? null : RenderChildren(model);

            if (childrenTags == null) return ApplyToDefaultLayout(editTags);
            else return ApplyToDefaultLayout(new Tags{ editTags, childrenTags });
        }
        #endregion

        #region Overrides
        protected virtual string? ListPageTitle { get; } = null;

        protected virtual bool ShowDefaultDetailPageTitle { get; } = true;
        protected virtual string? DetailPageTitle { get; } = null;
        
        protected virtual bool ListSkipDelete { get; } = false;
        protected virtual bool ListSkipAddNew { get; } = false;

        protected virtual bool ReadOnly { get; } = false;

        //override this to get accordion
        protected virtual List<Bs4.AccordionPanel>? GetAccordionPanels(TDetailMvcModel model)
        {
            return null; 
        }

        protected virtual IGenerateHtml? RenderChildren(TDetailMvcModel model) => null;
        #endregion
    }
}
