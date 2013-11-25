using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanlin.Common.Enums
{
    public static class EnumHelper
    {
        public static IDictionary<string, int> EnumToDictionary(Type enumType)
        {
            var levels = new Dictionary<string, int>();
            Array levelValues = Enum.GetValues(enumType);

            foreach (var value in levelValues)
            {
                levels.Add(Enum.GetName(enumType, value), (int)value);
            }
            return levels;
        }

        public static TEnum Parse<TEnum>(string nameOrValue) where TEnum : struct
        {
            TEnum parsed;
            if (!Enum.TryParse<TEnum>(nameOrValue, out parsed))
                throw EnumExceptionHelper.InvalidEnumName<TEnum>(nameOrValue);

            if (!Enum.IsDefined(parsed.GetType(), parsed))
                throw EnumExceptionHelper.NotEnumMember<TEnum>(parsed);

            return parsed;
        }
    }
}
