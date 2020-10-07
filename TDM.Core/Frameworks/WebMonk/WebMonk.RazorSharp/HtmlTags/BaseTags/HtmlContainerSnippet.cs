#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;

namespace WebMonk.RazorSharp.HtmlTags.BaseTags
{
    public abstract class HtmlContainerSnippet : HtmlStack, IGenerateAndContainHtml
    {
        #region IList<IGenerateHtml> implementation by wrapping InnerContent
        public IEnumerator<IGenerateHtml> GetEnumerator()
        {
            return InnerContent.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return InnerContent.GetEnumerator();
        }
        public void Add(IGenerateHtml item)
        {
            InnerContent.Add(item);
        }
        public void Clear()
        {
            InnerContent.Clear();
        }
        public bool Contains(IGenerateHtml item)
        {
            return InnerContent.Contains(item);
        }
        public void CopyTo(IGenerateHtml[] array, int arrayIndex)
        {
            InnerContent.CopyTo(array, arrayIndex);
        }
        public bool Remove(IGenerateHtml item)
        {
            return InnerContent.Remove(item);
        }
        public int IndexOf(IGenerateHtml item)
        {
            return InnerContent.IndexOf(item);
        }
        public void Insert(int index, IGenerateHtml item)
        {
            InnerContent.Insert(index, item);
        }
        public void RemoveAt(int index)
        {
            InnerContent.RemoveAt(index);
        }
        public IGenerateHtml this[int index]
        {
            get => InnerContent[index];
            set => InnerContent[index] = value;
        }
        public bool IsReadOnly { get; set; }
        #endregion

        #region Properties
        public IGenerateAndContainHtml InnerContent 
        { 
            get => _innerContent ?? throw new SystemException($"{GetType().Name} HtmlContainerSnippet must assign InnerContent in its constructor"); 
            set => _innerContent = value;
        }
        protected IGenerateAndContainHtml? _innerContent;

        #endregion
    }
}
