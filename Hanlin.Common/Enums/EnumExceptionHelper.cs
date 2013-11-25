using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanlin.Common.Extensions;

namespace Hanlin.Common.Enums
{
    public class EnumExceptionHelper
    {
        public static Exception InvalidEnumName<TEnum>(string source) where TEnum : struct
        {
            string msg = "Invalid enum name: {0}. Valid names are {1}.";
            return new ArgumentException(string.Format(msg, source, string.Join(", ", Enum.GetNames(typeof(TEnum)))));
        }

        public static Exception NotEnumMember<TEnum>(object source) where TEnum : struct
        {
            string msg = "Invalid enum value: {0}. Valid values are {1}.";
            return new ArgumentException(string.Format(msg, source, string.Join(", ", Enum.GetValues(typeof(TEnum)))));
        }
    }
}
