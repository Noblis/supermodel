#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Supermodel.DataAnnotations;

namespace WebMonk.RazorSharp.HtmlTags.BaseTags
{
    public class CodeBlock : IGenerateHtml
    {
        #region Constructors
        public CodeBlock(Func<Tags> code)
        {
            Tags = code();
        }
        #endregion

        #region IGenerateHtml implemntation
        public StringBuilderWithIndents ToHtml(StringBuilderWithIndents? sb = null)
        {
            return Tags.ToHtml(sb);
        }
        public IGenerateHtml FillBodySectionWith(IGenerateHtml replacement)
        {
            return Tags.FillBodySectionWith(replacement);
        }
        public IGenerateHtml FillSectionWith(string sectionId, IGenerateHtml replacement)
        {
            return Tags.FillSectionWith(sectionId, replacement);
        }
        public bool TryFillSectionWith(string sectionId, IGenerateHtml replacement)
        {
            return Tags.TryFillSectionWith(sectionId, replacement);
        }

        public IGenerateHtml DisableAllControls()
        {
            Tags.DisableAllControls();
            return this;
        }
        public IGenerateHtml DisableAllControlsIf(bool condition)
        {
            Tags.DisableAllControlsIf(condition);
            return this;
        }

        public IEnumerable<Tag> GetTagsInOrder()
        {
            return Tags.GetTagsInOrder();
        }
        public virtual List<Tag> NormalizeAndFlatten()
        {
            return Tags.NormalizeAndFlatten();
        }
        #endregion

        #region Linq-like methods
        public virtual int CountWhere(Func<Tag, bool> predicate)
        {
            return GetTagsInOrder().Count(predicate);
        }
        public virtual IEnumerable<Tag> Where(Func<Tag, bool> predicate)
        {
            return GetTagsInOrder().Where(predicate);
        }

        public virtual Tag SingleWhere(Func<Tag, bool> predicate)
        {
            return GetTagsInOrder().Single(predicate);
        }
        public virtual Tag? SingleOrDefaultWhere(Func<Tag, bool> predicate)
        {
            return GetTagsInOrder().SingleOrDefault(predicate);
        }

        public virtual Tag FirstWhere(Func<Tag, bool> predicate)
        {
            return GetTagsInOrder().First(predicate);
        }
        public virtual Tag? FirstOrDefaultWhere(Func<Tag, bool> predicate)
        {
            return GetTagsInOrder().FirstOrDefault(predicate);
        }

        public virtual bool AnyWhere(Func<Tag, bool> predicate)
        {
            return GetTagsInOrder().Any(predicate);
        }
        public virtual bool AllWhere(Func<Tag, bool> predicate)
        {
            return GetTagsInOrder().All(predicate);
        }

        public virtual int RemoveWhere(Func<Tag, bool> predicate)
        {
            return Tags.RemoveWhere(predicate);
        }
        public virtual int RemoveFirstWhere(Func<Tag, bool> predicate)
        {
            return Tags.RemoveFirstWhere(predicate);
        }

        public virtual int InsertBeforeWhere(Func<Tag, bool> predicate, IGenerateHtml tags)
        {
            return Tags.InsertBeforeWhere(predicate, tags);
        }
        public virtual int InsertAfterWhere(Func<Tag, bool> predicate, IGenerateHtml tags)
        {
            return Tags.InsertAfterWhere(predicate, tags);
        }

        public virtual int InsertBeforeFirstWhere(Func<Tag, bool> predicate, IGenerateHtml tags)
        {
            return Tags.InsertBeforeFirstWhere(predicate, tags);
        }
        public virtual int InsertAfterFirstWhere(Func<Tag, bool> predicate, IGenerateHtml tags)
        {
            return Tags.InsertAfterFirstWhere(predicate, tags);
        }
        #endregion

        #region Properties
        public Tags Tags { get; set; }
        #endregion
    }
}
