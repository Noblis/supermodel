﻿#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Supermodel.DataAnnotations;
using Supermodel.DataAnnotations.Exceptions;
using Supermodel.ReflectionMapper;

namespace Supermodel.Mobile.CodeGen
{
    public class ModelGen
    {
        #region Constructors
        public ModelGen(Assembly webAssembly, string nameSpace = "Supermodel.ApiClient.Models") : this(new List<Assembly> { webAssembly }, nameSpace) { }
        public ModelGen(IEnumerable<Assembly> assemblies, string nameSpace = "Supermodel.ApiClient.Models")
        {
            Assemblies = assemblies;
            NameSpace = nameSpace;
        }
        #endregion

        #region Methods
        public StringBuilderWithIndents GenerateModelsAndSaveToDisk(string filename = @"..\..\" + "Supermodel.Mobile.ModelsAndRuntime.cs", StringBuilderWithIndents? sb = null)
        {
            if (sb == null) sb = new StringBuilderWithIndents();
            sb = GenerateModels(sb);
            File.WriteAllText(filename, sb.ToString());
            return sb;
        }
        public StringBuilderWithIndents GenerateModels(StringBuilderWithIndents? sb = null)
        {
            _globalCustomTypesDefined.Clear();

            if (sb == null) sb = new StringBuilderWithIndents();
            var customTypesDefined = new List<Type>();

            sb.AppendLine("//   DO NOT MAKE CHANGES TO THIS FILE. THEY MIGHT GET OVERWRITTEN!!!");
            sb.AppendLine("//   Auto-generated by Supermodel.Mobile on " + DateTime.Now);
            sb.AppendLine("//");
            sb.AppendLine("");
            sb.AppendLine("// ReSharper disable RedundantUsingDirective");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using Supermodel.Mobile.Runtime.Common.DataContext.WebApi;");
            sb.AppendLine("using Supermodel.Mobile.Runtime.Common.Models;");
            sb.AppendLine("using System.ComponentModel;");
            sb.AppendLine("using Supermodel.DataAnnotations.Validations;");
            sb.AppendLine("// ReSharper restore RedundantUsingDirective");
            sb.AppendLine("");
            sb.AppendLine("// ReSharper disable once CheckNamespace");
            sb.AppendLineFormat("namespace {0}", NameSpace);
            sb.AppendLineIndentPlus("{");

            var first = true;
            foreach (var controllerType in FindAllSupermodelControllers())
            {
                if (!controllerType.Name.EndsWith("ApiController")) throw new SupermodelException("Controller " + controllerType.Name + ": names by convention must end with 'ApiController'");

                var modelTypes = GetModelTypesForController(controllerType, out var controllerKind);
                if (controllerKind == ControllerKindEnum.Command && modelTypes.Count != 2) throw new Exception("controllerKind == ControllerKindEnum.Command && modelTypes.Count != 2");
                if (controllerKind == ControllerKindEnum.CRUD && modelTypes.Count != 2) throw new Exception("controllerKind == ControllerKindEnum.CRUD && modelTypes.Count != 2");
                if (controllerKind == ControllerKindEnum.EnhancedCRUD && modelTypes.Count != 3) throw new Exception("controllerKind == ControllerKindEnum.EnhancedCRUD && modelTypes.Count != 3");
                if (controllerKind == ControllerKindEnum.WMCommand && modelTypes.Count != 2) throw new Exception("controllerKind == ControllerKindEnum.WMCommand && modelTypes.Count != 2");
                if (controllerKind == ControllerKindEnum.WMCRUD && modelTypes.Count != 2) throw new Exception("controllerKind == ControllerKindEnum.WMCRUD && modelTypes.Count != 2");
                if (controllerKind == ControllerKindEnum.WMEnhancedCRUD && modelTypes.Count != 3) throw new Exception("controllerKind == ControllerKindEnum.WMEnhancedCRUD && modelTypes.Count != 3");

                if (first) first = false;
                else sb.AppendLine("");

                sb.AppendLineFormat("#region {0}", controllerType.Name);

                if (controllerKind == ControllerKindEnum.Command || controllerKind == ControllerKindEnum.WMCommand)
                {
                    //if (!modelTypes[0].Name.EndsWith("ApiModel")) throw new SupermodelSystemErrorException("Model " + modelTypes[0].Name + ": names by convention must end with 'ApiModel'");
                    //if (!modelTypes[1].Name.EndsWith("ApiModel")) throw new SupermodelSystemErrorException("Model " + modelTypes[0].Name + ": names by convention must end with 'ApiModel'");

                    var commandName = controllerType.Name.Replace("Controller", "");
                    var inputType = modelTypes[0].Name.Replace("ApiModel", "");
                    var outputType = modelTypes[1].Name.Replace("ApiModel", "");

                    sb.AppendLine($"//Extension method for {commandName} command");
                    sb.AppendLine($"public static class {commandName}CommandExt");
                    sb.AppendLineIndentPlus("{");
                    sb.AppendLine($"public static async Task<{outputType}> {commandName}Async(this WebApiDataContext me, {inputType} input)");
                    sb.AppendLineIndentPlus("{");
                    sb.AppendLine($"return await me.ExecutePostAsync<{inputType}, {outputType}>(\"{commandName}\", input);");
                    sb.AppendLineIndentMinus("}");
                    sb.AppendLineIndentMinus("}");

                    if (!_globalCustomTypesDefined.Contains(modelTypes[0]))
                    {
                        _globalCustomTypesDefined.Add(modelTypes[0]);
                        sb.AppendLine("// ReSharper disable once PartialTypeWithSinglePart");
                        sb.AppendLineFormat("public partial class {0}", modelTypes[0].Name.Replace("ApiModel", ""));
                        sb = GeneratePropertiesForModel(modelTypes[0], customTypesDefined, false, sb);
                    }
                    else
                    {
                        sb.AppendLineFormat("//Class {0} is already defined elsewhere", modelTypes[0].Name.Replace("ApiModel", ""));
                    }

                    if (!_globalCustomTypesDefined.Contains(modelTypes[1]))
                    {
                        _globalCustomTypesDefined.Add(modelTypes[1]);
                        sb.AppendLine("// ReSharper disable once PartialTypeWithSinglePart");
                        sb.AppendLineFormat("public partial class {0}", modelTypes[1].Name.Replace("ApiModel", ""));
                        sb = GeneratePropertiesForModel(modelTypes[1], customTypesDefined, false, sb);
                    }
                    else
                    {
                        sb.AppendLineFormat("//Class {0} is already defined elsewhere", modelTypes[1].Name.Replace("ApiModel", ""));
                    }
                }

                if (controllerKind == ControllerKindEnum.CRUD || controllerKind == ControllerKindEnum.EnhancedCRUD ||
                    controllerKind == ControllerKindEnum.WMCRUD || controllerKind == ControllerKindEnum.WMEnhancedCRUD)
                {
                    if (modelTypes[0] == modelTypes[1]) //Detail and List are the same model
                    {
                        if (!modelTypes[0].Name.EndsWith("ApiModel")) throw new SupermodelException("Model " + modelTypes[0].Name + ": names by convention must end with 'ApiModel'");
                    }
                    else
                    {
                        if (!modelTypes[0].Name.EndsWith("DetailApiModel")) throw new SupermodelException("Detail Model " + modelTypes[0].Name + ": names by convention must end with 'DetailApiModel'");
                    }

                    string restUrlAttribute;
                    if (controllerKind == ControllerKindEnum.CRUD || controllerKind == ControllerKindEnum.EnhancedCRUD) restUrlAttribute = $"[RestUrl(\"{controllerType.Name.Replace("Controller", "")}\")]";
                    else restUrlAttribute = $"[RestUrl(\"{controllerType.Name.Replace("ApiController", "")}\")]";

                    if (!_globalCustomTypesDefined.Contains(modelTypes[0]))
                    {
                        _globalCustomTypesDefined.Add(modelTypes[0]);
                        sb.AppendLine(restUrlAttribute);
                        sb.AppendLine("// ReSharper disable once PartialTypeWithSinglePart");
                        sb.AppendLineFormat("public partial class {0} : Model", modelTypes[0].Name.Replace("DetailApiModel", "").Replace("ApiModel", ""));
                        sb = GeneratePropertiesForModel(modelTypes[0], customTypesDefined, false, sb);
                    }
                    else
                    {
                        sb.AppendLineFormat("//Class {0} is already defined elsewhere", modelTypes[0].Name.Replace("DetailApiModel", "").Replace("ApiModel", ""));
                    }

                    //if List and Detail types are different, do a separate one here
                    if (modelTypes[0] != modelTypes[1])
                    {
                        sb.AppendLine("");
                        sb.AppendLine(restUrlAttribute);
                        if (!modelTypes[1].Name.EndsWith("ListApiModel")) throw new SupermodelException("List Model " + modelTypes[0].Name + ": names by convention must end with 'ListApiModel'");

                        if (!_globalCustomTypesDefined.Contains(modelTypes[1]))
                        {
                            _globalCustomTypesDefined.Add(modelTypes[1]);
                            sb.AppendLine("// ReSharper disable once PartialTypeWithSinglePart");
                            sb.AppendLineFormat("public partial class {0} : Model", modelTypes[1].Name.Replace("ApiModel", ""));
                            sb = GeneratePropertiesForModel(modelTypes[1], customTypesDefined, false, sb);
                        }
                        else
                        {
                            sb.AppendLineFormat("//Class {0} is already defined elsewhere", modelTypes[1].Name.Replace("ApiModel", ""));
                        }
                    }

                    //if we are dealing with an enhanced controller, do the search model
                    if (controllerKind == ControllerKindEnum.EnhancedCRUD || controllerKind == ControllerKindEnum.WMEnhancedCRUD)
                    {
                        sb.AppendLine("");
                        if (!modelTypes[2].Name.EndsWith("SearchApiModel")) throw new SupermodelException("Search Model " + modelTypes[0].Name + ": names by convention must end with 'SearchApiModel'");

                        if (!_globalCustomTypesDefined.Contains(modelTypes[2]))
                        {
                            _globalCustomTypesDefined.Add(modelTypes[2]);
                            sb.AppendLine("// ReSharper disable once PartialTypeWithSinglePart");
                            sb.AppendLineFormat("public partial class {0}", modelTypes[2].Name.Replace("ApiModel", ""));
                            sb = GeneratePropertiesForModel(modelTypes[2], customTypesDefined, true, sb);
                        }
                        else
                        {
                            sb.AppendLineFormat("//Class {0} is already defined elsewhere", modelTypes[2].Name.Replace("ApiModel", ""));
                        }
                    }
                }
                sb.AppendLine("#endregion");
            }

            //Add types marked by an [IncludeInApiClient] attribute
            foreach (var assembly in Assemblies)
            {
                var types = assembly.GetTypes().Where(x => Attribute.IsDefined(x, typeof(IncludeInApiClientAttribute))).ToList();
                foreach (var type in types)
                {
                    if (!_globalCustomTypesDefined.Contains(type))
                    {
                        customTypesDefined.Add(type);
                        _globalCustomTypesDefined.Add(type);
                    }
                }
            }

            sb.AppendLine("");
            sb.AppendLine("#region Types models depend on and types that were specifically marked with [IncludeInApiClient]");
            sb = GenerateCustomTypes(customTypesDefined, sb);
            sb.AppendLine("#endregion");

            sb.AppendLineIndentMinus("}");
            return sb;
        }
        #endregion

        #region Private Helper Methods
        private StringBuilderWithIndents GeneratePropertiesForModel(Type modelType, List<Type> customTypesDefined, bool generateIdProp, StringBuilderWithIndents sb)
        {
            sb.AppendLineIndentPlus("{");
            sb.AppendLine("#region Properties");
            foreach (var property in GetPublicProperties(modelType))
            {
                //skip if property is marked with [JsonIgnore]
                if (property.GetCustomAttributes().SingleOrDefault(x => x is JsonIgnoreAttribute) != null) continue;

                var customModelAttributeAttribute = (MobileModelAttributeAttribute?)property.GetCustomAttributes().SingleOrDefault(x => x is MobileModelAttributeAttribute);
                if (customModelAttributeAttribute != null) sb.Append(customModelAttributeAttribute.AttrContent + " ");

                if (property.PropertyType.IsPrimitive)
                {
                    if (generateIdProp || property.Name != "Id")
                    {
                        sb.AppendFormat("public {0} {1} ", property.PropertyType.Name, property.Name);
                        sb.AppendLine("{ get; set; }");
                    }
                }
                else if (property.PropertyType != typeof(string) && property.PropertyType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                {
                    var genericArgType = property.PropertyType.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)).GetGenericArguments()[0];
                    if (!_globalCustomTypesDefined.Contains(genericArgType))
                    {
                        customTypesDefined.Add(genericArgType);
                        _globalCustomTypesDefined.Add(genericArgType);
                    }
                    var genericParam = genericArgType.Name.Replace("ApiModel", "");
                    sb.AppendFormat("public List<{0}> {1} ", genericParam, property.Name);
                    sb.AppendLineFormat("{{ get; set; }} = new List<{0}>();", genericParam);
                }
                else if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                {
                    var underlyingPropType = Nullable.GetUnderlyingType(property.PropertyType);
                    if (underlyingPropType == null) throw new Exception("underlyingPropType == null: this should never happen");
                    sb.AppendFormat("public {0}? {1} ", underlyingPropType.Name, property.Name);
                    if (underlyingPropType.IsEnum)
                    {
                        if (!_globalCustomTypesDefined.Contains(underlyingPropType))
                        {
                            customTypesDefined.Add(underlyingPropType);
                            _globalCustomTypesDefined.Add(underlyingPropType);
                        }
                    }
                    sb.AppendLine("{ get; set; }");

                }
                else if (property.PropertyType == typeof(string) || property.PropertyType == typeof(DateTime) || property.PropertyType.Name == "BinaryFileApiModel")
                {
                    var typeName = property.PropertyType.Name.Replace("ApiModel", "");
                    sb.AppendFormat("public {0} {1} ", typeName, property.Name);

                    if (property.PropertyType.Name == "BinaryFileApiModel" || property.PropertyType == typeof(DateTime)) sb.AppendLineFormat("{{ get; set; }} = new {0}();", typeName);
                    else sb.AppendLine("{ get; set; }");
                }
                else if (property.PropertyType.IsEnum || property.PropertyType.IsClass || property.PropertyType.IsValueType)
                {
                    sb.AppendFormat("public {0} {1} ", property.PropertyType.Name.Replace("ApiModel", ""), property.Name);
                    if (!_globalCustomTypesDefined.Contains(property.PropertyType))
                    {
                        customTypesDefined.Add(property.PropertyType);
                        _globalCustomTypesDefined.Add(property.PropertyType);
                    }
                    sb.AppendLine("{ get; set; }");
                }
                else
                {
                    throw new SupermodelException("Unexpected property type '" + property.PropertyType.Name + "'");
                }
            }
            sb.AppendLine("#endregion");
            sb.AppendLineIndentMinus("}");
            return sb;
        }
        private StringBuilderWithIndents GenerateCustomTypes(List<Type> customTypesDefined, StringBuilderWithIndents sb)
        {
            var additionalCustomTypes = new List<Type>();
            var firstCustomType = true;
            foreach (var customType in customTypesDefined)
            {
                if (firstCustomType) firstCustomType = false;
                else sb.AppendLine("");

                if (customType.IsEnum)
                {
                    sb.AppendLineFormat("public enum {0}", customType.Name);
                    sb.AppendLineIndentPlus("{");
                    var first = true;
                    foreach (var enumValue in Enum.GetValues(customType))
                    {
                        var enumValueLabel = enumValue!.ToString();
                        
                        var descriptionAttribute = "";
                        var descriptionStr = enumValue.GetDescription();
                        if (descriptionStr != enumValueLabel) descriptionAttribute = $"[Description(\"{descriptionStr}\")] ";

                        var disabledAttribute = "";
                        var isDisabled = enumValue.IsDisabled();
                        if (isDisabled) disabledAttribute = "[Disabled] ";

                        if (first)
                        {
                            sb.AppendFormat("{0}{1}{2} = {3}", descriptionAttribute, disabledAttribute, enumValueLabel!, Convert.ChangeType(enumValue, Enum.GetUnderlyingType(customType)));
                            first = false;
                        }
                        else
                        {
                            sb.AppendLine(",");
                            sb.AppendFormat("{0}{1}{2} = {3}", descriptionAttribute, disabledAttribute, enumValueLabel!, Convert.ChangeType(enumValue, Enum.GetUnderlyingType(customType)));
                        }
                    }
                    sb.AppendLine("");
                    sb.AppendLineIndentMinus("}");
                }
                else if (customType.IsClass || customType.IsValueType)
                {
                    sb.AppendLine("// ReSharper disable once PartialTypeWithSinglePart");
                    sb.AppendLineFormat("public partial class {0}", customType.Name.Replace("ApiModel", ""));
                    sb = GeneratePropertiesForModel(customType, additionalCustomTypes, true, sb);
                }
                else
                {
                    throw new SupermodelException("Unexpected Custom Type '" + customType.Name + "'");
                }
            }

            if (additionalCustomTypes.Any()) GenerateCustomTypes(additionalCustomTypes, sb);
            return sb;
        }
        private List<PropertyInfo> GetPublicProperties(Type modelType)
        {
            return modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanRead && x.CanWrite).ToList();
        }
        private List<Type> GetModelTypesForController(Type controllerType, out ControllerKindEnum controllerKind)
        {
            var modelTypes = new List<Type>();

            var crudControllerBaseType = GetBaseGenericType(controllerType, _crudApiControllerType, _wmCrudApiControllerType);
            if (crudControllerBaseType != null)
            {
                var crudGenericArguments = crudControllerBaseType.GetGenericArguments();
                modelTypes.Add(crudGenericArguments[1]);
                modelTypes.Add(crudGenericArguments[2]);

                //if this is an enhanced CRUD controller, grab the third argument (SearchType)
                var controllerEnhancedBaseType = GetBaseGenericType(controllerType, _enhancedCrudApiControllerType, _wmEnhancedCrudApiControllerType);
                if (controllerEnhancedBaseType != null)
                {
                    if (controllerEnhancedBaseType.GetGenericTypeDefinition() == _enhancedCrudApiControllerType) controllerKind = ControllerKindEnum.EnhancedCRUD;
                    else if (controllerEnhancedBaseType.GetGenericTypeDefinition() == _wmEnhancedCrudApiControllerType) controllerKind = ControllerKindEnum.WMEnhancedCRUD;
                    else throw new SupermodelException("unknown controllerEnhancedBaseType");
                    
                    modelTypes.Add(controllerEnhancedBaseType.GetGenericArguments()[3]);
                }
                else
                {
                    if (crudControllerBaseType.GetGenericTypeDefinition() == _crudApiControllerType) controllerKind = ControllerKindEnum.CRUD;
                    else if (crudControllerBaseType.GetGenericTypeDefinition() == _wmCrudApiControllerType) controllerKind = ControllerKindEnum.WMCRUD;
                    else throw new SupermodelException("unknown crudControllerBaseType");
                }
            }
            else
            {
                var commandControllerBaseType = GetBaseGenericType(controllerType, _commandApiControllerType, _wmCommandApiControllerType);
                if (commandControllerBaseType == null) throw new SupermodelException("commandControllerBaseType == null");
                var genericArguments = commandControllerBaseType.GetGenericArguments();
                modelTypes.Add(genericArguments[0]);
                modelTypes.Add(genericArguments[1]);

                if (commandControllerBaseType.GetGenericTypeDefinition() == _commandApiControllerType) controllerKind = ControllerKindEnum.Command;
                else if (commandControllerBaseType.GetGenericTypeDefinition() == _wmCommandApiControllerType) controllerKind = ControllerKindEnum.WMCommand;
                else throw new SupermodelException("unknown commandControllerBaseType");
            }

            return modelTypes;
        }
        private List<Type> FindAllSupermodelControllers()
        {
            var result = new List<Type>();

            foreach (var assembly in Assemblies)
            {
                var assemblyResult = assembly.GetTypes().Where(t => !t.IsAbstract && IsSubclassOf(t, _crudApiControllerType)).ToList();

                result.AddRange(assemblyResult);
            }
            foreach (var assembly in Assemblies)
            {
                var assemblyResult = assembly.GetTypes().Where(t => !t.IsAbstract && IsSubclassOf(t, _wmCrudApiControllerType)).ToList();

                result.AddRange(assemblyResult);
            }

            //we do it twice to insure that all Command controllers are after the CRUD controllers
            foreach (var assembly in Assemblies)
            {
                var assemblyResult = assembly.GetTypes().Where(t => !t.IsAbstract && IsSubclassOf(t, _commandApiControllerType)).ToList();

                result.AddRange(assemblyResult);
            }
            foreach (var assembly in Assemblies)
            {
                var assemblyResult = assembly.GetTypes().Where(t => !t.IsAbstract && IsSubclassOf(t, _wmCommandApiControllerType)).ToList();

                result.AddRange(assemblyResult);
            }
            return result;
        }
        private bool IsSubclassOf(Type me, Type allegedBaseType)
        {
            Type? toCheck = me;
            while (toCheck != null && toCheck != typeof(object))
            {
                var curType = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (allegedBaseType == curType) return true;
                toCheck = toCheck.BaseType;
            }
            return false;
        }
        private Type? GetBaseGenericType(Type me, Type allegedBaseGenericType1, Type allegedBaseGenericType2)
        {
            Type? toCheck = me;
            while (toCheck != null && toCheck != typeof(object))
            {
                var curType = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (curType == allegedBaseGenericType1 || curType == allegedBaseGenericType2) return toCheck;
                toCheck = toCheck.BaseType;
            }
            return null;
        }
        #endregion

        #region Proprties & Attributes
        public IEnumerable<Assembly> Assemblies { get; }
        public string NameSpace { get; }

        private readonly HashSet<Type> _globalCustomTypesDefined = new HashSet<Type>();
        
        //private readonly Type _ApiControllerBaseType = typeof(ApiControllerBase);
        private readonly Type _enhancedCrudApiControllerType = typeof(Presentation.Mvc.Controllers.Api.EnhancedCRUDApiController<,,,,>);
        private readonly Type _crudApiControllerType = typeof(Presentation.Mvc.Controllers.Api.CRUDApiController<,,,>);
        private readonly Type _commandApiControllerType = typeof(Presentation.Mvc.Controllers.Api.CommandApiController<,>);

        private readonly Type _wmEnhancedCrudApiControllerType = typeof(Presentation.WebMonk.Controllers.Api.EnhancedCRUDApiController<,,,,>);
        private readonly Type _wmCrudApiControllerType = typeof(Presentation.WebMonk.Controllers.Api.CRUDApiController<,,,>);
        private readonly Type _wmCommandApiControllerType = typeof(Presentation.WebMonk.Controllers.Api.CommandApiController<,>);
        #endregion
    }
}
