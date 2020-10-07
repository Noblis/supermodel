#nullable enable

using System;

namespace Supermodel.DataAnnotations.Validations.Attributes
{
    public class RestUrlAttribute : Attribute
    {
        public RestUrlAttribute(string url)
        {
            Url = url;
        }
        
        public string Url { get; }
    }
}
