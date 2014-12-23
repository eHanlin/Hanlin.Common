using System.Configuration;

namespace Hanlin.Common.Configuration
{
    public class DatabaseConfigurationSection : ConfigurationSection
    {
        private const string UriAttribute = "uri";
        private const string DatabaseAttribute = "database";

        [ConfigurationProperty(UriAttribute, IsRequired = true)]
        public string Uri
        {
            get { return (string)this[UriAttribute]; }
            set { this[UriAttribute] = value; }
        }

        [ConfigurationProperty(DatabaseAttribute)]
        public string Database
        {
            get { return (string)this[DatabaseAttribute]; }
            set { this[DatabaseAttribute] = value; }
        }
    }
}