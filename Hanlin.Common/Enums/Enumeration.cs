using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hanlin.Common.Enums
{
    public interface IEnumeration
    {
        string DisplayName { get; }
        object Value { get; }
    }

    /// <summary>
    /// Enumeration classes adapted from http://lostechies.com/jimmybogard/2008/08/12/enumeration-classes/
    /// </summary>
    /// 
    public class Enumeration<TValue> : Enumeration, IEnumeration, IComparable where TValue : IComparable<TValue>, IEquatable<TValue>
    {
        private readonly TValue _value;

        public Enumeration()
        {

        }

        protected Enumeration(TValue value, string displayName)
        {
            _value = value;
            DisplayName = displayName;
        }

        public override string ToString()
        {
            return DisplayName;
        }

        object IEnumeration.Value { get { return _value; } }

        public TValue Value
        {
            get { return _value; }
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration<TValue>;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = _value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(Enumeration<TValue> obj1, Enumeration<TValue> obj2)
        {
            if ((object)obj1 == null || ((object)obj2) == null)
                return Object.Equals(obj1, obj2);

            return obj1.Equals(obj2);
        }

        public static bool operator !=(Enumeration<TValue> obj1, Enumeration<TValue> obj2)
        {
            return !(obj1 == obj2);
        }

        public int CompareTo(object other)
        {
            return Value.CompareTo(((Enumeration<TValue>)other).Value);
        }
    }

    public abstract class Enumeration
    {
        public string DisplayName { get; protected set; }

        public static IEnumerable<T> GetEnumerations<T>() where T : Enumeration, new()
        {
            var enums = GetEnumerations(typeof (T));
            return enums.Cast<T>();
        }

        public static IEnumerable GetEnumerations(Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var info in fields)
            {
                var instance = Activator.CreateInstance(type);
                var locatedValue = info.GetValue(instance);

                if (locatedValue != null)
                {
                    yield return locatedValue;
                }
            }
        }

        protected static T ParseOrNull<T>(Func<T, bool> predicate) where T : Enumeration, new()
        {
            return GetEnumerations<T>().FirstOrDefault(predicate);
        }

        protected static T Parse<T, TValueToParse>(TValueToParse value, string description, Func<T, bool> predicate) where T : Enumeration, new()
        {
            var matchingItem = GetEnumerations<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                string message;

                if (value == null)
                    message = string.Format("Cannot parse null value");
                else
                    message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
                throw new ApplicationException(message);
            }

            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName, T defaultValue) where T : Enumeration, new()
        {
            return string.IsNullOrEmpty(displayName) || !Enumeration.IsNameDefined<T>(displayName)
                       ? defaultValue
                       : Enumeration.FromDisplayName<T>(displayName);
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
            return matchingItem;
        }

        public static T FromDisplayNameOrNull<T>(string displayName) where T : Enumeration, new()
        {
            if (displayName != null)
            {
                return ParseOrNull<T>(item => item.DisplayName == displayName);
            }
            return null;
        }

        public static T FromValue<T, TValue>(TValue value) 
            where T : Enumeration<TValue>, new() 
            where TValue : IComparable<TValue>, IEquatable<TValue>
        {
            var matchingItem = Parse<T, TValue>(value, "value", item => item.Value.Equals(value));
            return matchingItem;
        }

        public static T FromValueOrNull<T, TValue>(TValue value) 
            where T : Enumeration<TValue>, new()
            where TValue : IComparable<TValue>, IEquatable<TValue>

        {
            if (!Equals(value, null))
            {
                return ParseOrNull<T>(item => item.Value.Equals(value));
            }
            return null;
        }

        public static bool IsNameDefined<T>(string name) where T : Enumeration, new()
        {
            bool isDefined;
            try
            {
                FromDisplayName<T>(name);
                isDefined = true;
            }
            catch (ApplicationException)
            {
                isDefined = false;
            }
            return isDefined;
        }

        public static bool IsValueDefined<T, TValue>(TValue value) 
            where T : Enumeration<TValue>, new()
            where TValue : IComparable<TValue>, IEquatable<TValue>

        {
            bool isDefined;
            try
            {
                FromValue<T, TValue>(value);
                isDefined = true;
            }
            catch (ApplicationException)
            {
                isDefined = false;
            }
            return isDefined;
        }
    }
}
