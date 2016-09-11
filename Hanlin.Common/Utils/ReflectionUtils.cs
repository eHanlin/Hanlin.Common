using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hanlin.Common.Utils
{
    public class ReflectionUtils
    {
        private static string ToFirstLowerOrUpperCase(string name, bool isLower)
        {
            if (name.Length > 0)
            {
                var firstChar = (isLower ? char.ToLower(name[0]) : char.ToUpper(name[0]));

                return name.Length > 1 ? firstChar + name.Substring(1) : firstChar.ToString();
            }
            return name;
        }

        private static string GetFieldName(string field, bool? isUpperCase, bool? isLowerCase)
        {
            return isUpperCase.HasValue && (bool)isUpperCase ? ToFirstLowerOrUpperCase(field, false) :
                isLowerCase.HasValue && (bool)isLowerCase ? ToFirstLowerOrUpperCase(field, true) : field;
        }

        public static T GetPropertyValue<T>(object instance, string field, bool isUpperCase = false, bool isLowerCase = false)
        {
            var fieldName = GetFieldName(field, isUpperCase, isLowerCase);
            var instanceType = instance.GetType();
            var propInfo = instanceType.GetProperty(fieldName);
            return (T)propInfo.GetValue(instance);
        }

        public static T GetFieldValue<T>(object instance, string field, bool isUpperCase = false, bool isLowerCase = false)
        {
            var fieldName = GetFieldName(field, isUpperCase, isLowerCase);
            var instanceType = instance.GetType();
            var fieldInfo = instanceType.GetField(fieldName);
            return (T)fieldInfo.GetValue(instance);
        }

        public static IEnumerable<string> GetPropertyNames(object instance, bool isUpperCase = false, bool isLowerCase = false)
        {
            return instance.GetType().GetProperties().Select(s => GetFieldName(s.Name, isUpperCase, isLowerCase));
        }

        public static IEnumerable<string> GetFieldNames(object instance, bool isUpperCase = false, bool isLowerCase = false)
        {
            return instance.GetType().GetFields().Select(s => GetFieldName(s.Name, isUpperCase, isLowerCase));
        }
    }
}