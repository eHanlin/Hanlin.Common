using System.Collections.Generic;
using System.IO;
using Amazon.S3;

namespace Hanlin.Common.AWS
{
    public interface IS3Service
    {
        string ServiceName { get; }
        string BucketName { get; set; }

        IEnumerable<string> ListBuckets();

        string Put(string key, byte[] bytes, string contentType = null, S3CannedACL cannedAcl = null);
        string Put(string key, Stream inputStream, string contentType = null, S3CannedACL cannedAcl = null);
        void Put(string key, string path, string contentType = null, S3CannedACL cannedAcl = null);

        void Get(string key, Stream outputStream);

        bool Exists(string key);

        void Delete(string key);
        void Delete(string[] keys);

        string BuildUrl(string key);
    }
}