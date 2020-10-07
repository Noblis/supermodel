#nullable enable

using System;
using System.ComponentModel;
using Supermodel.DataAnnotations.Validations.Attributes;

namespace Supermodel.ReflectionMapper
{
    public static class ObjectAttributeReaderExtensions
    {
        public static string GetDescription(this object? value)
        {
            if (value == null) return "";
            
            //Tries to find a DescriptionAttribute for a potential friendly name for the enum
            var type = value.GetType();

            var valueToString = value.ToString();

            if (type.IsEnum)
            {
                var memberInfo = type.GetMember(valueToString);
                if (memberInfo.Length > 0)
                {
                    var attr = Attribute.GetCustomAttribute(memberInfo[0], typeof(DescriptionAttribute), true);
                    if (attr != null) return ((DescriptionAttribute)attr).Description;
                }
                //If we have no description attribute, just return the ToString() or ToString().InsertSpacesBetweenWords() for enum
                return value.ToString().InsertSpacesBetweenWords();
            }

            return valueToString;
        }
        public static int GetScreenOrder(this object value)
        {
            //Tries to find a ScreenOrderAttribute for a potential friendly name for the enum
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            if (memberInfo.Length > 0)
            {
                var attr = Attribute.GetCustomAttribute(memberInfo[0], typeof(ScreenOrderAttribute), true);
                if (attr != null) return ((ScreenOrderAttribute)attr).Order;
            }
            //If we have no order, default is 100
            return 100;
        }
        public static bool IsDisabled(this object value)
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            if (memberInfo.Length > 0)
            {
                var attr = Attribute.GetCustomAttribute(memberInfo[0], typeof(DisabledAttribute), true);
                if (attr != null) return true;
            }
            //If we have no disabled attribute, we assume active
            return false;
        }
    }
}
