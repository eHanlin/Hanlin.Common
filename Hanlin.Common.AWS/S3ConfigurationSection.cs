using System.Configuration;

namespace Hanlin.Common.AWS
{
    public class S3ConfigurationSection : ConfigurationSection
    {
        private const string EndpointAttribute = "endpoint";

        [ConfigurationProperty(EndpointAttribute)]
        public string Endpoint
        {
            get { return (string)this[EndpointAttribute]; }
            set { this[EndpointAttribute] = value; }
        }

        private const string AccessKeyAttribute = "accessKey";

        [ConfigurationProperty(AccessKeyAttribute, IsRequired = true)]
        public string AccessKey
        {
            get { return (string)this[AccessKeyAttribute]; }
            set { this[AccessKeyAttribute] = value; }
        }
        
        private const string SecretKeyAttribute = "secretKey";

        [ConfigurationProperty(SecretKeyAttribute, IsRequired = true)]
        public string SecretKey
        {
            get { return (string)this[SecretKeyAttribute]; }
            set { this[SecretKeyAttribute] = value; }
        }

        private const string DefaultBucketAttribute = "defaultBucket";

        [ConfigurationProperty(DefaultBucketAttribute)]
        public string DefaultBucket
        {
            get { return (string)this[DefaultBucketAttribute]; }
            set { this[DefaultBucketAttribute] = value; }
        }
    }
}