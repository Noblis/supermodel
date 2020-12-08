#nullable enable

using System;
using System.Linq.Expressions;
using System.Reflection;
using Supermodel.DataAnnotations.Exceptions;
using Supermodel.DataAnnotations.Expressions;
using Supermodel.DataAnnotations.Validations.Attributes;
using Supermodel.Presentation.Cmd.ConsoleOutput;
using Supermodel.Presentation.Cmd.Models.Interfaces;

namespace Supermodel.Presentation.Cmd.Rendering
{
    public static class CmdRender
    {
        #region Helper Class
        public static class Helper
        {
            #region Parsing Expressions for Routing
            // ReSharper disable once UnusedParameter.Local
            public static string GetPropertyName<TModel, TValue>(TModel model, Expression<Func<TModel, TValue>> expression)
            {
                if (expression.Body.NodeType == ExpressionType.Convert)
                {
                    var body = (MemberExpression)((UnaryExpression)expression.Body).Operand;
                    var propertyName = body.Member is PropertyInfo ? body.Member.Name : "";
                    if (body.Expression.NodeType == ExpressionType.Parameter) return propertyName;
                    return $"{GetExpressionName(body.Expression)}.{propertyName}";
                }
                else if (expression.Body.NodeType == ExpressionType.MemberAccess)
                {
                    var body = (MemberExpression)expression.Body;
                    var propertyName = body.Member is PropertyInfo ? body.Member.Name : "";
                    if (body.Expression.NodeType == ExpressionType.Parameter) return propertyName;
                    return $"{GetExpressionName(body.Expression)}.{propertyName}";
                }
                else if (expression.Body.NodeType == ExpressionType.Call)
                {
                    var body = (MethodCallExpression)expression.Body;
                    if (body.Method.Name != "get_Item") throw new ArgumentException("Expression must describe a property or an indexer", nameof(expression));
                    return GetExpressionName(body);
                }
                else if (expression.Body.NodeType == ExpressionType.ArrayIndex)
                {
                    var body = (BinaryExpression)expression.Body;
                    return GetExpressionName(body);
                }
                else if (expression.Body.NodeType == ExpressionType.Parameter)
                {
                    var body = (ParameterExpression)expression.Body;
                    return GetExpressionName(body);
                }                
                else
                {
                    throw new ArgumentException("Expression must describe a property or an indexer", nameof(expression));
                }
            }
            private static string GetExpressionName(Expression expression)
            {
                if (expression.NodeType == ExpressionType.Parameter) return "";

                if (expression.NodeType == ExpressionType.MemberAccess)
                {
                    var memberExpression = (MemberExpression)expression;
                    return $"{GetExpressionName(memberExpression.Expression)}{memberExpression.Member.Name}";
                }

                if (expression.NodeType == ExpressionType.Call)
                {
                    var methodCallExpression = (MethodCallExpression)expression;
                    var indexExpression = methodCallExpression.Arguments[0];
                    var indexExpressionResult = Expression.Lambda(indexExpression).Compile().DynamicInvoke();
                    return $"{GetExpressionName(methodCallExpression.Object!)}[{indexExpressionResult}]";
                }

                if (expression.NodeType == ExpressionType.ArrayIndex)
                {
                    var binaryExpression = (BinaryExpression)expression;
                    var indexExpression = binaryExpression.Right;
                    var indexExpressionResult = Expression.Lambda(indexExpression).Compile().DynamicInvoke();
                    return $"{GetExpressionName(binaryExpression.Left)}[{indexExpressionResult}]";
                }

                throw new Exception("Invalid Expression '" + expression + "'");
            }
            #endregion
        }
        #endregion

        #region Render Label Methods
        public static ICmdOutput LabelFor<TModel, TValue>(TModel model, Expression<Func<TModel, TValue>> propertyExpression, string? label = null, FBColors? colors = null)
        {
            var propertyName = Helper.GetPropertyName(model, propertyExpression);
            return Label(model, propertyName, label, colors);
        }
        public static ICmdOutput Label<TModel>(TModel model, string expression, string? label = null, FBColors? colors = null)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            label ??= model.GetType().GetDisplayNameForProperty(expression);
            return new StringWithColor(label, colors);
        }
        #endregion

        #region Render Validation Methods
        //public static ICmdOutput ValidationSummary(object? ulAttributes = null, object? liAttributes = null)
        //{
        //    if (HttpContext.Current.ValidationResultList.IsValid) return new Tags();

        //    var ulTag = new Ul(ulAttributes)
        //    { 
        //        new CodeBlock(() => 
        //        { 
        //            var errorMessages = HttpContext.Current.ValidationResultList.Select(x => x.ErrorMessage);
        //            var tags = new Tags();
        //            foreach (var errorMessage in errorMessages)
        //            {
        //                tags.Add(new Li(liAttributes) { new Txt(errorMessage) });
        //            }
        //            return tags;
        //        })
        //    };
        //    return ulTag;
        //}
        //public static ICmdOutput ValidationMessageFor<TModel, TValue>(TModel model, Expression<Func<TModel, TValue>> propertyExpression, object? attributes = null, bool returnDiv = false)
        //{
        //    var propertyName = Helper.GetPropertyName(model, propertyExpression);
        //    return ValidationMessage(model, propertyName, attributes, returnDiv);
        //}
        //public static ICmdOutput ValidationMessage<TModel>(TModel model, string expression, object? attributes = null, bool returnDiv = false)
        //{
        //    if (HttpContext.Current.ValidationResultList.IsValid) return new Tags();
            
        //    var prefix = HttpContext.Current.PrefixManager.CurrentPrefix;

        //    if (model == null) throw new ArgumentNullException(nameof(model));

        //    if (!string.IsNullOrEmpty(prefix) && !string.IsNullOrEmpty(expression)) prefix = $"{prefix}.";
        //    var id = $"{prefix}{expression}".ToHtmlId();
        //    var name = id.ToHtmlName();

        //    var errors = HttpContext.Current.ValidationResultList.GetAllErrorsFor(name);
        //    if (!errors.Any()) return new Tags();

        //    if (returnDiv)
        //    {
        //        var divTag = new Span(new { data_valmsg_for=id }) { new Txt( errors.First()) };
        //        divTag.AddOrUpdateAttr(attributes);
        //        return divTag;
        //    }
        //    else
        //    {
        //        var spanTag = new Span(new { data_valmsg_for=id }) { new Txt( errors.First()) };
        //        spanTag.AddOrUpdateAttr(attributes);
        //        return spanTag;
        //    }
        //}
        #endregion

        #region Render Editor Methods
        //public static ICmdOutput EditorForModel<TModel>(TModel model, object? attributes = null)
        //{
        //    return Editor(model, "", attributes);
        //}
        //public static ICmdOutput EditorFor<TModel, TValue>(TModel model, Expression<Func<TModel, TValue>> propertyExpression, object? attributes = null)
        //{
        //    var propertyName = Helper.GetPropertyName(model, propertyExpression);
        //    return Editor(model, propertyName, attributes);
        //}
        //public static ICmdOutput Editor<TModel>(TModel model, string expression, object? attributes = null)
        //{
        //    if (model == null) throw new ArgumentNullException(nameof(model));

        //    var prefix = HttpContext.Current.PrefixManager.CurrentPrefix;

        //    if (!string.IsNullOrEmpty(prefix) && !string.IsNullOrEmpty(expression)) prefix = $"{prefix}.";
        //    var id = $"{prefix}{expression}".ToHtmlId();
        //    var name = id.ToHtmlName();

        //    var (propertyInfo, propertyType, propertyValue) = model.GetPropertyInfoPropertyTypeAndValueByFullName(expression);
        //    if (typeof(IEditorTemplate).IsAssignableFrom(propertyType))
        //    {
        //        using(HttpContext.Current.PrefixManager.NewPrefix(expression, model))
        //        {
        //            propertyValue ??= Activator.CreateInstance(propertyType);
        //            if (propertyValue is IEditorTemplate template) return template.EditorTemplate();
        //            else throw new WebMonkException("This should never happen: propertyValue is not IEditorTemplate");
        //        }
        //    }

        //    //IGenerateHtml
        //    if(typeof(IGenerateHtml).IsAssignableFrom(propertyType))
        //    {
        //        return (IGenerateHtml)(propertyValue ?? new Tags());
        //    }

        //    Tag editorTag;
        //    Tag? hiddenInputTag = null;

        //    //strings
        //    if (typeof(string).IsAssignableFrom(propertyType))
        //    {
        //        var attr = propertyInfo?.GetCustomAttribute<DataTypeAttribute>(true);
        //        if (attr == null)
        //        {
        //            editorTag = new Input(new { type="text", id, name, value=Helper.AdjustIfInvalid(propertyValue?.ToString(), expression) });
        //        }
        //        else
        //        {
        //            switch(attr.DataType)
        //            {
        //                case DataType.Password: editorTag = new Input(new { type="password", id, name, value=Helper.AdjustIfInvalid(propertyValue?.ToString(), expression)}); break;
        //                case DataType.EmailAddress: editorTag = new Input(new { type="email", id, name, value=Helper.AdjustIfInvalid(propertyValue?.ToString(), expression)}); break;
        //                case DataType.MultilineText: editorTag = new Textarea(new { id, name}){ new Txt(Helper.AdjustIfInvalid(propertyValue?.ToString(), expression))}; break;
        //                case DataType.PhoneNumber: editorTag = new Input(new { type="tel", id, name, value=Helper.AdjustIfInvalid(propertyValue?.ToString(), expression)}); break;
        //                default: editorTag = new Input(new { type="text", id, name, value=Helper.AdjustIfInvalid(propertyValue?.ToString(), expression)}); break;
        //            }
        //        }
        //    }
            
        //    //integer types
        //    else if (typeof(int).IsAssignableFrom(propertyType) ||
        //             typeof(int?).IsAssignableFrom(propertyType) ||
        //             typeof(uint).IsAssignableFrom(propertyType) ||
        //             typeof(uint?).IsAssignableFrom(propertyType) ||
        //             typeof(long).IsAssignableFrom(propertyType) ||
        //             typeof(long?).IsAssignableFrom(propertyType) ||
        //             typeof(ulong).IsAssignableFrom(propertyType) ||
        //             typeof(ulong?).IsAssignableFrom(propertyType) ||
        //             typeof(short).IsAssignableFrom(propertyType) ||
        //             typeof(short?).IsAssignableFrom(propertyType) ||
        //             typeof(ushort).IsAssignableFrom(propertyType) ||
        //             typeof(ushort?).IsAssignableFrom(propertyType) ||
        //             typeof(byte).IsAssignableFrom(propertyType) ||
        //             typeof(byte?).IsAssignableFrom(propertyType) ||
        //             typeof(sbyte).IsAssignableFrom(propertyType) ||
        //             typeof(sbyte?).IsAssignableFrom(propertyType))
        //    {
        //        editorTag = new Input(new { type="number", id, name, value=Helper.AdjustIfInvalid(propertyValue?.ToString(), expression)});
        //    }
            
        //    //floating point types
        //    else if (typeof(double).IsAssignableFrom(propertyType) ||
        //             typeof(double?).IsAssignableFrom(propertyType) ||
        //             typeof(float).IsAssignableFrom(propertyType) ||
        //             typeof(float?).IsAssignableFrom(propertyType) ||
        //             typeof(decimal).IsAssignableFrom(propertyType) ||
        //             typeof(decimal?).IsAssignableFrom(propertyType))
        //    {
        //        editorTag = new Input(new { type="number", step="any", id, name, value=Helper.AdjustIfInvalid(propertyValue?.ToString(), expression)});
        //    }

        //    //booleans (special case)
        //    else if (typeof(bool).IsAssignableFrom(propertyType) ||
        //             typeof(bool?).IsAssignableFrom(propertyType))
        //    {
        //        //this preserves the attempted values for special case of bool
        //        bool @checked;
        //        if (!HttpContext.Current.ValidationResultList.IsValid)
        //        {
        //            var valueProviders = HttpContext.Current.ValueProviderManager.GetCachedValueProvidersList() ?? throw new WebMonkException("This should never happen: valueProviders == null");
        //            var propertyValueResult = valueProviders.GetValueOrDefault<bool?>(name);
        //            @checked = propertyValueResult.GetNewValue<bool?>() == true;
        //        }
        //        else
        //        {
        //            @checked = (bool?)propertyValue == true;
        //        }

        //        editorTag = new Input(new { type="checkbox", id, name, value="true" } );
        //        if (@checked) editorTag.Attributes.Add("checked", "on");

        //        hiddenInputTag = new Input(new { type="hidden", name, value="false" });
        //    }

        //    //DateTime
        //    else if (typeof(DateTime).IsAssignableFrom(propertyType) ||
        //             typeof(DateTime?).IsAssignableFrom(propertyType))
        //    {
        //        editorTag = new Input(new { type="datetime-local", id, name, value=Helper.AdjustIfInvalid(((DateTime?)propertyValue)?.ToString("yyyy-MM-ddTHH:mm"), expression)});
        //    }

        //    //enums
        //    else if (typeof(Enum).IsAssignableFrom(propertyType) ||
        //             Nullable.GetUnderlyingType(propertyType)?.IsEnum == true)
        //    {
        //        editorTag = new Input(new { type="text", id, name, value=Helper.AdjustIfInvalid(propertyValue?.ToString(), expression)});
        //    }

        //    //byte array
        //    else if (typeof(byte[]).IsAssignableFrom(propertyType))
        //    {
        //        editorTag = new Input(new { type="file", id, name});
        //    }

        //    //Guid
        //    else if (typeof(Guid).IsAssignableFrom(propertyType))
        //    {
        //        editorTag = new Input(new { type="text", id, name, value=Helper.AdjustIfInvalid(propertyValue?.ToString(), expression)});
        //    }
            
        //    //catch-all for primitive types
        //    else if (propertyType.IsPrimitive)
        //    {
        //        editorTag = new Input(new { type="text", id, name, value=Helper.AdjustIfInvalid(propertyValue?.ToString(), expression)});
        //    }
            
        //    //catch-all for complex types
        //    else
        //    {
        //        return new Tags();
        //    }

        //    editorTag.AddOrUpdateAttr(attributes);

        //    if (hiddenInputTag == null) return editorTag;
        //    else return new Tags { editorTag, hiddenInputTag };
        //}
        #endregion
        
        #region Render Display Methods
        public static ICmdOutput DisplayForModel<TModel>(TModel model, FBColors? colors = null)
        {
            return Display(model, "", colors);
        }
        public static ICmdOutput DisplayFor<TModel, TValue>(TModel model, Expression<Func<TModel, TValue>> propertyExpression, FBColors? colors = null)
        {
            var propertyName = Helper.GetPropertyName(model, propertyExpression);
            return Display(model, propertyName, colors);
        }
        public static ICmdOutput Display<TModel>(TModel model, string expression, FBColors? colors = null)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var (_, propertyType, propertyValue) = model.GetPropertyInfoPropertyTypeAndValueByFullName(expression);
            if (typeof(ICmdDisplayTemplate).IsAssignableFrom(propertyType))
            {
                propertyValue ??= Activator.CreateInstance(propertyType);

                if (propertyValue is ICmdDisplayTemplate template) return template.DisplayTemplate();
                else throw new SupermodelException("This should never happen: propertyValue is not IEditorTemplate");
            }

            //IGenerateHtml
            if(typeof(ICmdOutput).IsAssignableFrom(propertyType))
            {
                return (ICmdOutput)(propertyValue ?? StringWithColor.Empty);
            }

            //strings
            if (typeof(string).IsAssignableFrom(propertyType))
            {
                return new StringWithColor(propertyValue?.ToString() ?? "", colors);
            }
            
            //integer types
            else if (typeof(int).IsAssignableFrom(propertyType) ||
                     typeof(int?).IsAssignableFrom(propertyType) ||
                     typeof(uint).IsAssignableFrom(propertyType) ||
                     typeof(uint?).IsAssignableFrom(propertyType) ||
                     typeof(long).IsAssignableFrom(propertyType) ||
                     typeof(long?).IsAssignableFrom(propertyType) ||
                     typeof(ulong).IsAssignableFrom(propertyType) ||
                     typeof(ulong?).IsAssignableFrom(propertyType) ||
                     typeof(short).IsAssignableFrom(propertyType) ||
                     typeof(short?).IsAssignableFrom(propertyType) ||
                     typeof(ushort).IsAssignableFrom(propertyType) ||
                     typeof(ushort?).IsAssignableFrom(propertyType) ||
                     typeof(byte).IsAssignableFrom(propertyType) ||
                     typeof(byte?).IsAssignableFrom(propertyType) ||
                     typeof(sbyte).IsAssignableFrom(propertyType) ||
                     typeof(sbyte?).IsAssignableFrom(propertyType))
            {
                return new StringWithColor(propertyValue?.ToString() ?? "", colors);
            }
            
            //floating point types
            else if (typeof(double).IsAssignableFrom(propertyType) ||
                     typeof(double?).IsAssignableFrom(propertyType) ||
                     typeof(float).IsAssignableFrom(propertyType) ||
                     typeof(float?).IsAssignableFrom(propertyType) ||
                     typeof(decimal).IsAssignableFrom(propertyType) ||
                     typeof(decimal?).IsAssignableFrom(propertyType))
            {
                return new StringWithColor(propertyValue?.ToString() ?? "", colors);
            }

            //booleans
            else if (typeof(bool).IsAssignableFrom(propertyType) ||
                     typeof(bool?).IsAssignableFrom(propertyType))
            {
                return new StringWithColor((bool?)propertyValue == true ? "Yes" : "No" , colors);
            }

            //DateTime
            else if (typeof(DateTime).IsAssignableFrom(propertyType) ||
                     typeof(DateTime?).IsAssignableFrom(propertyType))
            {
                return new StringWithColor(propertyValue?.ToString() ?? "", colors);
            }

            //enums
            else if (typeof(Enum).IsAssignableFrom(propertyType) ||
                     Nullable.GetUnderlyingType(propertyType)?.IsEnum == true)
            {
                return new StringWithColor(propertyValue?.ToString() ?? "", colors);
            }

            //Guid
            else if (typeof(Guid).IsAssignableFrom(propertyType))
            {
                return new StringWithColor(propertyValue?.ToString() ?? "", colors);
            }

            //catch-all for primitive types
            else if (propertyType.IsPrimitive)
            {
                return new StringWithColor(propertyValue?.ToString() ?? "", colors);
            }

            //catch-all for complex types
            else
            {
                return StringWithColor.Empty;
            }
        }
        #endregion
    }
}
