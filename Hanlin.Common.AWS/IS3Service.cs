using System.Collections.Generic;
using System.IO;

namespace Hanlin.Common.AWS
{
    public interface IS3Service
    {
        string ServiceName { get; }
        string BucketName { get; set; }

        IEnumerable<string> ListBuckets();

        string Put(string key, byte[] bytes);
        string Put(string key, Stream inputStream);
        void Put(string key, string path);

        void Get(string key, Stream outputStream);

        bool Exists(string key);

        void Delete(string key);
        void Delete(string[] keys);

        string BuildUrl(string key);
    }
}