﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Namotion.Reflection;
using Supermodel.DataAnnotations.Validations;
using WebMonk.Context;
using WebMonk.Exceptions;
using WebMonk.Extensions;
using WebMonk.Misc;
using WebMonk.ModeBinding;
using WebMonk.Rendering.Views;
using WebMonk.Results;
using WebMonk.ValueProviders;

namespace WebMonk.HttpRequestHandlers.Controllers
{
    public abstract class MvcController : ControllerBase
    {
        #region Constructors
        protected MvcController()
        {
            var myType = GetType();
            ControllerPart = myType.GetMvcControllerName();

            // ReSharper disable once VirtualMemberCallInConstructor
            ActionMethodsParts = GetActionMethodsParts(myType);
        }
        #endregion
        
        #region IHttpRequestHandler implementation
        public override int Priority => 300;
        public override bool SaveSessionState => true;

        public override async Task<IHttpRequestHandler.HttpRequestHandlerResult> TryExecuteHttpRequestAsync(CancellationToken cancellationToken)
        {
            var localParts = HttpContext.Current.RouteManager.LocalPathParts;
            if (localParts.Length < 1) return IHttpRequestHandler.HttpRequestHandlerResult.False;
            if (!ControllerPart.Equals(localParts[0], StringComparison.InvariantCultureIgnoreCase)) return IHttpRequestHandler.HttpRequestHandlerResult.False;

            var overridenHttpMethod = HttpContext.Current.RouteManager.OverridenHttpMethod;
            
            string? action;
            Dictionary<string, object> routeData;
            var controller = localParts[0];
            if (localParts.Length >= 2)
            {
                if (long.TryParse(localParts[1], out _)) 
                {
                    // /student/1
                    action = null;
                    var id = localParts[1];
                    routeData = new Dictionary<string, object> { { "__controller__", controller }, {"id", id } };
                }
                else 
                {
                    // /student/list or /student/detail/1
                    action = localParts[1];
                    if (localParts.Length >= 3) routeData = new Dictionary<string, object> { {"__controller__", controller}, { "__action__", action }, {"id", localParts[2]} };
                    else routeData = new Dictionary<string, object> { {"__controller__", controller}, { "__action__", action } };
                }
            }
            else //localParts.Length cannot be less than 1, we checked for that earlier
            {
                action =null;
                routeData = new Dictionary<string, object> { {"__controller__", controller} };
            }

            var actionMethodInfos = ActionMethodsParts.Where(x => $"{overridenHttpMethod}{action}".Equals(x.Name, StringComparison.InvariantCultureIgnoreCase)).ToArray();
            if (actionMethodInfos.Length > 0) return await RunActionsAsync(actionMethodInfos, routeData, cancellationToken).ConfigureAwait(false);

            var asyncActionMethodInfos = ActionMethodsParts.Where(x => $"{overridenHttpMethod}{action}Async".Equals(x.Name, StringComparison.InvariantCultureIgnoreCase)).ToArray();        
            if (asyncActionMethodInfos.Length > 0) return await RunAsyncActionsAsync(asyncActionMethodInfos, routeData, cancellationToken).ConfigureAwait(false);

            return IHttpRequestHandler.HttpRequestHandlerResult.False;
        }
        #endregion

        #region Overrides
        protected override async Task<(bool, object?[])> TryBindAndValidateParametersAsync(MethodInfo actionMethodInfo)
        {
            var valueProviders = await HttpContext.Current.ValueProviderManager.GetValueProvidersListAsync().ConfigureAwait(false);
            var modelBinder = HttpContext.Current.StaticModelBinderManager.GetStaticModelBinder();

            var parametersList = new List<object?>();
            
            var parametersInfos = actionMethodInfo.GetParameters();
            foreach (var parameterInfo in parametersInfos)
            {
                var parametersType = parameterInfo.ParameterType;
                
                object? parameterValue;
                
                //if class or a struct, we don't need an extra prefix 
                if (parameterInfo.ParameterType.IsComplexType())
                {
                    parameterValue = await modelBinder.BindNewModelAsync(parametersType, parametersType, valueProviders).ConfigureAwait(false);
                }
                else
                {
                    using(HttpContext.Current.PrefixManager.NewPrefix(parameterInfo.Name, null))
                    {
                        try
                        {
                            parameterValue = await modelBinder.BindNewModelAsync(parametersType, parametersType, valueProviders).ConfigureAwait(false);
                        }
                        catch (WebMonkInvalidFormatException)
                        {
                            return (false, Array.Empty<object?>());
                        }
                    }
                }
                
                if (parameterValue == null)
                { 
                    if (parameterInfo.ToContextualParameter().Nullability == Nullability.NotNullable) return (false, parametersList.ToArray());
                }
                else if (parameterValue == Type.Missing)
                {
                    if (!parameterInfo.IsOptional) return(false, Array.Empty<object?>());
                }
                else
                {
                    var nVrl = new ValidationResultList(); //nullability validation result list
                    if (ValidateNullability && !NullabilityHelper.TryValidateObjectNullability(parameterValue, nVrl))
                    {
                        HttpContext.Current.ValidationResultList.AddValidationResultList(nVrl);
                    }

                    var vrl = new ValidationResultList();
                    if (!await AsyncValidator.TryValidateObjectAsync(parameterValue, new ValidationContext(parameterValue), vrl).ConfigureAwait(false))
                    {
                        HttpContext.Current.ValidationResultList.AddValidationResultList(vrl);
                    }
                }

                parametersList.Add(parameterValue);
            }
            return (true, parametersList.ToArray());
        }        
        #endregion

        #region Methods
        public virtual Task<bool> TryUpdateModelAsync<TModel>(TModel model, string additionalPrefix, List<IValueProvider>? valueProviders = null, bool ignoreRootObjectIModelBinder = false) where TModel : class
        {
            return HttpContext.Current.StaticModelBinderManager.TryUpdateMvcModelAsync(model, additionalPrefix, valueProviders, ignoreRootObjectIModelBinder);
        }
        public virtual Task<bool> TryUpdateModelAsync<TModel>(TModel model, List<IValueProvider>? valueProviders = null, bool ignoreRootObjectIModelBinder = false) where TModel : class
        {
            return HttpContext.Current.StaticModelBinderManager.TryUpdateMvcModelAsync(model, valueProviders, ignoreRootObjectIModelBinder);
        }
        
        protected virtual LocalRedirectResult RedirectToAction<T>(Expression<Action<T>> action, NameValueCollection? queryString)  where T : MvcController
        {
            return RedirectToAction(action, queryString?.ToQueryStringDictionary());
        }
        protected virtual LocalRedirectResult RedirectToAction<T>(Expression<Action<T>> action, QueryStringDict? queryStringDict = null)  where T : MvcController
        {
            return new LocalRedirectResult(Render.Helper.UrlToMvcAction(action, queryStringDict));
        }

        protected virtual LocalRedirectResult RedirectToAction(string action)
        {
            return RedirectToAction(action, (QueryStringDict?)null);
        }
        
        protected virtual LocalRedirectResult RedirectToAction(string action, NameValueCollection queryString)
        {
            return RedirectToAction(action, queryString.ToQueryStringDictionary());
        }
        protected virtual LocalRedirectResult RedirectToAction(string action, QueryStringDict? queryStringDict)
        {
            return RedirectToActionStrId(action, "", queryStringDict);
        }

        protected virtual LocalRedirectResult RedirectToAction(string action, long? id, NameValueCollection queryString)
        {
            return RedirectToActionStrId(action, id?.ToString(), queryString);
        }
        protected virtual LocalRedirectResult RedirectToActionStrId(string action, string? id, NameValueCollection queryString)
        {
            return RedirectToActionStrId(action, id, queryString.ToQueryStringDictionary());
        }
        
        protected virtual LocalRedirectResult RedirectToAction(string action, long? id, QueryStringDict? queryStringDict = null)
        {
            return RedirectToActionStrId(action, id?.ToString(), queryStringDict);
        }
        protected virtual LocalRedirectResult RedirectToActionStrId(string action, string? id, QueryStringDict? queryStringDict = null)
        {
            var controller = GetType().GetMvcControllerName();
            return RedirectToActionStrId(controller, action, id, queryStringDict);
        }
        
        protected virtual LocalRedirectResult RedirectToAction(string controller, string action, long? id, NameValueCollection queryString)
        {
            return RedirectToActionStrId(controller, action, id?.ToString(), queryString);
        }
        protected virtual LocalRedirectResult RedirectToActionStrId(string controller, string action, string? id, NameValueCollection queryString)
        {
            return RedirectToActionStrId(controller, action, id, queryString.ToQueryStringDictionary());
        }

        protected virtual LocalRedirectResult RedirectToAction(string controller, string action, long? id = null, QueryStringDict? queryStringDict = null)
        {
            return RedirectToActionStrId(controller, action, id?.ToString(), queryStringDict);
        }
        protected virtual LocalRedirectResult RedirectToActionStrId(string controller, string action, string? id = null, QueryStringDict? queryStringDict = null)
        {
            return new LocalRedirectResult(Render.Helper.UrlToMvcAction(controller, action, id, queryStringDict));
        }
        #endregion

        #region Properties
        protected internal string ControllerPart { get; set; }
        protected internal ImmutableList<MethodInfo> ActionMethodsParts { get; set; }
        #endregion
    }
}
