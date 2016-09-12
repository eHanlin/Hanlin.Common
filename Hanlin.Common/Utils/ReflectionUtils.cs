﻿using System;
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

        private static string GetFieldName(string field, bool? isFirstUpperCase, bool? isFirstLowerCase)
        {
            return isFirstUpperCase.HasValue && (bool)isFirstUpperCase ? ToFirstLowerOrUpperCase(field, false) :
                isFirstLowerCase.HasValue && (bool)isFirstLowerCase ? ToFirstLowerOrUpperCase(field, true) : field;
        }

        public static T GetPropertyValue<T>(object instance, string field, bool isFirstUpperCase = false, bool isFirstLowerCase = false)
        {
            var fieldName = GetFieldName(field, isFirstUpperCase, isFirstLowerCase);
            var instanceType = instance.GetType();
            var propInfo = instanceType.GetProperty(fieldName);
            return (T)propInfo.GetValue(instance);
        }

        public static T GetFieldValue<T>(object instance, string field, bool isFirstUpperCase = false, bool isFirstLowerCase = false)
        {
            var fieldName = GetFieldName(field, isFirstUpperCase, isFirstLowerCase);
            var instanceType = instance.GetType();
            var fieldInfo = instanceType.GetField(fieldName);
            return (T)fieldInfo.GetValue(instance);
        }

        public static IEnumerable<string> GetPropertyNames(object instance, bool isFirstUpperCase = false, bool isFirstLowerCase = false)
        {
            return instance.GetType().GetProperties().Select(s => GetFieldName(s.Name, isFirstUpperCase, isFirstLowerCase));
        }

        public static IEnumerable<string> GetFieldNames(object instance, bool isFirstUpperCase = false, bool isFirstLowerCase = false)
        {
            return instance.GetType().GetFields().Select(s => GetFieldName(s.Name, isFirstUpperCase, isFirstLowerCase));
        }
    }
}