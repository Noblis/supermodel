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
using Newtonsoft.Json;
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
    public abstract class ApiController : ControllerBase
    {
        #region Constructors
        protected ApiController()
        {
            var myType = GetType();
            ControllerPart = myType.GetApiControllerName();

            // ReSharper disable once VirtualMemberCallInConstructor
            ActionMethodsParts = GetActionMethodsParts(myType);
        }
        #endregion
        
        #region IHttpRequestHandler implementation
        public override int Priority => 400;
        public override bool SaveSessionState => false;

        public override async Task<IHttpRequestHandler.HttpRequestHandlerResult> TryExecuteHttpRequestAsync(CancellationToken cancellationToken)
        {
            var localParts = HttpContext.Current.RouteManager.LocalPathParts;
            if (localParts.Length < 2) return IHttpRequestHandler.HttpRequestHandlerResult.False;
            if (!localParts[0].Equals("api", StringComparison.InvariantCultureIgnoreCase)) return IHttpRequestHandler.HttpRequestHandlerResult.False;
            if (!ControllerPart.Equals(localParts[1], StringComparison.InvariantCultureIgnoreCase)) return IHttpRequestHandler.HttpRequestHandlerResult.False;

            var overridenHttpMethod = HttpContext.Current.RouteManager.OverridenHttpMethod;

            string? action;
            Dictionary<string, object> routeData;
            var controller = localParts[1];
            if (localParts.Length >= 3)
            {
                if (long.TryParse(localParts[2], out _)) 
                {
                    // /api/student/1
                    action = null;
                    routeData = new Dictionary<string, object> { {"__controller__", controller}, { "id", localParts[2]} };
                }
                else 
                {
                    // /api/student/all or /api/student/detail/1
                    action = localParts[2];
                    if (localParts.Length >= 4) routeData = new Dictionary<string, object> { {"__controller__", controller}, { "__action__", action }, { "id", localParts[3]} };
                    else routeData = new Dictionary<string, object> { {"__controller__", controller}, { "__action__", action } };
                }
            }
            else //localParts.Length cannot be less than 2, we checked for that earlier
            {
                action = null;
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
            
            var parametersList = new List<object?>();
            
            var parametersInfos = actionMethodInfo.GetParameters();
            foreach (var parameterInfo in parametersInfos)
            {
                var parametersType = parameterInfo.ParameterType;
                var modelBinder = HttpContext.Current.StaticModelBinderManager.GetStaticModelBinder();
                
                var parameterValue = Type.Missing;
                
                //if class or a struct, we don't need an extra prefix 
                if (parameterInfo.ParameterType.IsComplexType())
                {
                    //first try to bind to all value providers (query string)
                    using(HttpContext.Current.PrefixManager.NewPrefix(parameterInfo.Name, null))
                    {
                        parameterValue = await modelBinder.BindExistingModelAsync(parametersType, parametersType, parameterValue, valueProviders).ConfigureAwait(false);
                    }

                    //Then we bind to body as json
                    var messageBodyValueProvider = valueProviders.GetFirstOrDefaultValueProviderOfType<MessageBodyValueProvider>() ?? throw new WebMonkException("Unable to find MessageBodyValueProvider");
                    var bodyResult = messageBodyValueProvider.GetValueOrDefault(""); //get the entire body
                    if (!bodyResult.ValueMissing && bodyResult.NewValue != null)
                    {
                        var body = bodyResult.GetNewValue<string>();
                        if (!string.IsNullOrEmpty(body)) parameterValue = JsonConvert.DeserializeObject(body, parameterInfo.ParameterType);
                    }
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
        public virtual Task<bool> TryUpdateModelAsync(object model, List<IValueProvider>? valueProviders = null)
        {
            return HttpContext.Current.StaticModelBinderManager.TryUpdateApiModelAsync(model, valueProviders);
        }

        protected virtual LocalRedirectResult RedirectToAction<T>(Expression<Action<T>> action, NameValueCollection? queryString)  where T : ApiController
        {
            return RedirectToAction(action, queryString?.ToQueryStringDictionary());
        }
        protected virtual LocalRedirectResult RedirectToAction<T>(Expression<Action<T>> action, QueryStringDict? queryStringDict = null) where T : ApiController
        {
            return new LocalRedirectResult(Render.Helper.UrlToApiAction(action, queryStringDict));
        }        

        protected virtual LocalRedirectResult RedirectToAction(string action, NameValueCollection? queryString)
        {
            return RedirectToAction(action, queryString?.ToQueryStringDictionary());
        }
        protected virtual LocalRedirectResult RedirectToAction(string action, QueryStringDict? queryStringDict = null)
        {
            return RedirectToAction(action, "", queryStringDict);
        }
        
        protected virtual LocalRedirectResult RedirectToAction(string action, string id, NameValueCollection? queryString)
        {
            return RedirectToAction(action, id, queryString?.ToQueryStringDictionary());
        }
        protected virtual LocalRedirectResult RedirectToAction(string action, string id, QueryStringDict? queryStringDict = null)
        {
            var controller = GetType().GetApiControllerName();
            return RedirectToAction(controller, action, id, queryStringDict);
        }
        
        protected virtual LocalRedirectResult RedirectToAction(string controller, string action, string id, NameValueCollection? queryString)
        {
            return RedirectToAction(controller, action, id, queryString?.ToQueryStringDictionary());
        }
        protected virtual LocalRedirectResult RedirectToAction(string controller, string action, string id, QueryStringDict? queryStringDict = null)
        {
            return new LocalRedirectResult(Render.Helper.UrlToApiAction(controller, action, id, queryStringDict));
        }
        #endregion

        #region Properties
        protected internal string ControllerPart { get; set; }
        protected internal ImmutableList<MethodInfo> ActionMethodsParts { get; set; }
        #endregion
    }
}