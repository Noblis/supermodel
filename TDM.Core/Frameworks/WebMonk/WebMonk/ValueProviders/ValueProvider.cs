﻿#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebMonk.Context;
using WebMonk.Exceptions;
using WebMonk.Extensions;

namespace WebMonk.ValueProviders
{
    public abstract class ValueProvider : IValueProvider
    {
        #region Methods
        public virtual Task<IValueProvider> InitAsync(Dictionary<string, object> dict)
        {
            Values = dict.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);
            return Task.FromResult((IValueProvider)this);
        }
        public virtual Task<IValueProvider> InitAsync(NameValueCollection nvc)
        {
            Values = nvc.ToValueProviderDictionary().ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);
            return Task.FromResult((IValueProvider)this);
        }
        
        public List<string>? GetIndexesWithValue(string key)
        {
            var indexes = new List<string>();
            
            //find all values that look like $"^{key}[match]..."
            foreach (var keyValuePair in Values)
            {
                var regex = new Regex(@$"^{Regex.Escape(key)}\s*\[(.*?)\]");
                var matches = regex.Matches(keyValuePair.Key);
                if (matches.Count > 0)
                {
                    //if multiple matches we grab the fir
                    var match = matches[0];
                    indexes.Add(match.Groups[1].Value.Trim()); //extract the content of square brackets
                    if (matches.Count > 1) throw new WebMonkException($"GetIndexesWithValues(): More than one Regex matches for key {key}");
                }
            }
            if (indexes.Count == 0) return null;
            return indexes;
        }

        #nullable disable
        public virtual IValueProvider.Result GetValueOrDefault<T>(string key)
        {
            return GetValueOrDefault(key, typeof(T));
        }
        #nullable enable
        public virtual IValueProvider.Result GetValueOrDefault(string key, Type type)
        {
            var result = GetValueOrDefault(key);
            try
            {
                return ParseToType(result, type, key);
            }
            catch (Exception)
            {
                throw new WebMonkInvalidFormatException(result, type, key, GetType());
            }
        }
        public virtual IValueProvider.Result GetValueOrDefault(string key)
        {
            if (Values.ContainsKey(key)) 
            {
                var value = Values[key];
                if (HttpContext.Current.BlockDangerousValueProviderValues && IsDangerousValue(value)) throw new HttpRequestValidationException("Attempt to pass a dangerous value is blocked");
                return new IValueProvider.Result(GetType(), value);
            }
            else
            {
                return new IValueProvider.Result(GetType(), null, true);
            }
        }

        public virtual IValueProvider.Result ParseToType(IValueProvider.Result result, Type type, string key)
        {
            //if value is null, it means we could not find it in value provider
            if (result.NewValue == null) return result;

            //Handle binary data (file upload). if value is byte[], we only return it as a byte[]
            if (result.NewValue is byte[])
            {
                if (typeof(byte[]).IsAssignableFrom(type)) 
                {
                    //if file name is not there or empty, we say the file is null (otherwise, it will say the file is a byte[0])
                    if (string.IsNullOrEmpty(GetValueOrDefault($"{key}{IValueProvider.FileNameSuffix}").GetNewValue<string>())) return new IValueProvider.Result(GetType(), null);
                    else return result;
                }
                else 
                {
                    throw new WebMonkException("Asking for non byte array for a byte array value");
                }
            }

            //if user asked for an array
            if (type.IsArray)
            {
                var innerType = type.GetElementType();
                if (result.NewValue is IList<string> valueList)
                {
                    var array = Array.CreateInstance(innerType!, valueList.Count);
                    for(var i = 0; i < valueList.Count; i++) array.SetValue(ParseToType(new IValueProvider.Result(GetType(), valueList[i]), innerType!, key), i);
                    return new IValueProvider.Result(GetType(), array);
                }
                else
                {
                    var array = Array.CreateInstance(innerType!, 1);
                    array.SetValue(ParseToType(result, innerType!, key), 0);
                    return new IValueProvider.Result(GetType(), array);
                }
            }

            //if user asked for a list, this would be a case of multiple HTML fields with identical name
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var innerType = type.GenericTypeArguments[0];
                var specificType = typeof(List<>).MakeGenericType(innerType);
                
                var list = (IList)Activator.CreateInstance(specificType);
                if (result.NewValue is IList<string> valueList)
                {
                    foreach (var val in valueList) list.Add(ParseToType(new IValueProvider.Result(GetType(), val), innerType, key).NewValue);
                }
                else
                {
                    list.Add(ParseToType(result, innerType, key).NewValue);
                }
                return new IValueProvider.Result(GetType(), list);
            }

            //special handing for list of booleans. If value is list but we ask for a boolean
            if (result.NewValue is IList<string> stringList)
            {
                if (typeof(bool).IsAssignableFrom(type) || typeof(bool?).IsAssignableFrom(type))
                {
                    var @checked = false;
                    foreach (var val in stringList) @checked |= Boolean.Parse(val);
                    return new IValueProvider.Result(GetType(), @checked);
                }
            }

            //the only other type we support is string
            if (result.NewValue is string strValue)
            {
                //handle blank string for nullable value types
                if (strValue.Trim() == "" && Nullable.GetUnderlyingType(type) != null) return new IValueProvider.Result(GetType(), null);

                //strings
                if (typeof(string).IsAssignableFrom(type)) return new IValueProvider.Result(GetType(), strValue); 

                //integer types
                if (typeof(int).IsAssignableFrom(type) || typeof(int?).IsAssignableFrom(type)) return new IValueProvider.Result(GetType(), int.Parse(strValue)); 
                if (typeof(uint).IsAssignableFrom(type) || typeof(uint?).IsAssignableFrom(type)) return new IValueProvider.Result(GetType(), uint.Parse(strValue)); 
                if (typeof(long).IsAssignableFrom(type) || typeof(long?).IsAssignableFrom(type)) return new IValueProvider.Result(GetType(), long.Parse(strValue));
                if (typeof(ulong).IsAssignableFrom(type) || typeof(ulong?).IsAssignableFrom(type)) return new IValueProvider.Result(GetType(), ulong.Parse(strValue));
                if (typeof(short).IsAssignableFrom(type) || typeof(short?).IsAssignableFrom(type)) return new IValueProvider.Result(GetType(), Int16.Parse(strValue));
                if (typeof(ushort).IsAssignableFrom(type) || typeof(ushort?).IsAssignableFrom(type)) return new IValueProvider.Result(GetType(), ushort.Parse(strValue));
                if (typeof(byte).IsAssignableFrom(type) || typeof(byte?).IsAssignableFrom(type)) return new IValueProvider.Result(GetType(), byte.Parse(strValue));
                if (typeof(sbyte).IsAssignableFrom(type) || typeof(sbyte?).IsAssignableFrom(type)) return new IValueProvider.Result(GetType(), sbyte.Parse(strValue));
            
                //floating point types
                if (typeof(double).IsAssignableFrom(type) || typeof(double?).IsAssignableFrom(type)) return new IValueProvider.Result(GetType(), double.Parse(strValue)); 
                if (typeof(float).IsAssignableFrom(type) || typeof(float?).IsAssignableFrom(type)) return new IValueProvider.Result(GetType(), float.Parse(strValue)); 
                if (typeof(decimal).IsAssignableFrom(type) || typeof(decimal?).IsAssignableFrom(type)) return new IValueProvider.Result(GetType(), decimal.Parse(strValue)); 

                //boolean
                if (typeof(bool).IsAssignableFrom(type) || typeof(bool?).IsAssignableFrom(type)) return new IValueProvider.Result(GetType(), ParseBool(strValue)); 

                //datetime 
                if (typeof(DateTime).IsAssignableFrom(type) || typeof(DateTime?).IsAssignableFrom(type)) return new IValueProvider.Result(GetType(), DateTime.Parse(strValue)); 

                //enums
                if (typeof(Enum).IsAssignableFrom(type) || Nullable.GetUnderlyingType(type)?.IsEnum == true) return new IValueProvider.Result(GetType(), ParseEnum(type, strValue));

                //guid
                if (typeof(Guid).IsAssignableFrom(type) || typeof(Guid?).IsAssignableFrom(type)) return new IValueProvider.Result(GetType(), Guid.Parse(strValue));

                //unsupported type
                throw new WebMonkException($"ParseToType(): cannot parse string into {type.Name}");                    
            }

            throw new ArgumentException($"Unable to parse {result.NewValue.GetType().Name} into {type.Name}");
        }
        #endregion

        #region Dangerous Value Detection
        protected virtual bool IsDangerousValue(object? value)
        { 
            if (value == null) return false;

            if (value is byte[]) return false;

            if (value is string valueString) return IsDangerousString(valueString, out _);

            if (value is IList<string> valueList)
            {
                foreach (var valueListString in valueList)
                {
                    if (IsDangerousString(valueListString, out _)) return true;
                }
                return false;
            }

            //unsupported type
            throw new WebMonkException($"IsDangerousValue(): Unable to process value of type {value.GetType()}");                    
        }

        protected virtual bool IsDangerousString(string? s, out int matchIndex)
        {
            matchIndex = 0;
            var startIndex = 0;

            s = RemoveNullCharacters(s);
            if (s == null) return false;

            while (true)
            {
                var index = s.IndexOfAny(StartingChars, startIndex);
                if (index >= 0 && index != s.Length - 1)
                {
                    matchIndex = index;
                    switch (s[index])
                    {
                        case '&':
                        { 
                            if (s[index + 1] != '#') return false;
                            return true;
                        }
                        case '<':
                        { 
                            if (IsAtoZ(s[index + 1]) || s[index + 1] == '!' || (s[index + 1] == '/' || s[index + 1] == '?')) return true;
                            else return false;
                        }
                    }
                    startIndex = index + 1;
                }
                else
                { 
                    return false;
                }
            }
        }
        private static bool IsAtoZ(char c)
        {
            if (c >= 'a' && c <= 'z') return true;
            return c >= 'A' && c <= 'Z';
        }
        private static string? RemoveNullCharacters(string? s)
        {
            if (s == null) return null;
            return s.IndexOf(Char.MinValue) > -1 ? s.Replace("\0", String.Empty) : s;
        }
        private static char[] StartingChars { get; } =  { '<', '&' };
        #endregion

        #region Protected Helpers
        protected object ParseEnum(Type type, string strValue)
        {
            var underlyingEnumType = Nullable.GetUnderlyingType(type);
            if (underlyingEnumType != null && underlyingEnumType.IsEnum) return Enum.Parse(underlyingEnumType, strValue, true);
            
            return Enum.Parse(type, strValue, true);
        }
        protected bool ParseBool(string str)
        {
            var boolStrings = str.Split(",");
            var value = false;
            foreach (var boolString in boolStrings)
            {
                value |= bool.Parse(boolString);
            }
            return value;
        }
        #endregion
        
        #region Properties
        public ImmutableDictionary<string, object> Values { get; protected set;} = ImmutableDictionary<string, object>.Empty;
        #endregion
    }
}
