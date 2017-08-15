using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Hanlin.Common.Configuration
{
    public static class ConnectionStringSettingsExtensions
    {
        private const char KeyValueSeparator = '=';
        private const char EntrySeparator = ';';

        public static IDictionary<string, string> GetConnectionProperties(this ConnectionStringSettings connSettings)
        {
            if (connSettings == null)
            {
                throw new ArgumentNullException(nameof(connSettings));
            }

            return ParseConnectionString(connSettings.ConnectionString);
        }

        private static IDictionary<string, string> ParseConnectionString(string connStr)
        {
            if (connStr.Contains(KeyValueSeparator) || connStr.Contains(EntrySeparator))
            {
                return connStr.Split(new[] { EntrySeparator }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(entry => entry.Split(new[] { KeyValueSeparator }, StringSplitOptions.RemoveEmptyEntries))
                    .ToDictionary(pair => pair[0], pair => pair.Length > 1 ? pair[1] : null);
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }
    }
}