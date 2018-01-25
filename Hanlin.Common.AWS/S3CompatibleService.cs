using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using log4net;

namespace Hanlin.Common.AWS
{
    public class S3CompatibleService : IS3Service, IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string ServiceUrl { get; private set; }
        public IAmazonS3 S3 { get; private set; }

        public string ServiceName { get; set; }
        public string BucketName { get; set; }

        public string BucketUrl { get; private set; }

        public S3CompatibleService(string endpoint, string accessKey, string secretKey, string bucket)
        {
            ServiceUrl = endpoint;
            ServiceName = "Hicloud";

            BucketName = bucket;
            BucketUrl = string.Format("https://{0}.s3.amazonaws.com/", BucketName);

            if (ServiceUrl != null)
            {
                S3 = Amazon.AWSClientFactory.CreateAmazonS3Client(accessKey, secretKey, new AmazonS3Config
                {
                    ServiceURL = ServiceUrl
                });
            }
            else
            {
                S3 = Amazon.AWSClientFactory.CreateAmazonS3Client(accessKey, secretKey, RegionEndpoint.APSoutheast1);
            }
        }

        public string BuildUrl(string key)
        {
            if (key.StartsWith("http")) return key;

            return BucketUrl + key;
        }

        public void Put(string key, string path, string contentType = null)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                Put(key, stream, contentType);
            }
        }

        public string Put(string key, byte[] bytes, string contentType = null)
        {
            return Put(key, new MemoryStream(bytes), contentType); // It's not necessary to dispose the MemoryStream as it is backed by an array.
        }

        public string Put(string key, Stream inputStream, string contentType = null)
        {
            VerifyKey(key);

            key = ConvertKey(key);

            // Setting stream position is not necessary for AWS API 
            // but we are bing explicit to demostrate that the position does matter depending on the underlying API.
            // For example, GridFs API does require position to be at zero!

            inputStream.Position = 0;

            var request = new PutObjectRequest
            {
                BucketName = BucketName,
                InputStream = inputStream,
                Key = key,
                CannedACL = S3CannedACL.PublicRead,
                AutoCloseStream = false,
            };

            if (!string.IsNullOrEmpty(contentType)) request.ContentType = contentType;

            var response = S3.PutObject(request);


            inputStream.Position = 0; // Rewind the stream as the stream could be used again after the method returns.

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return BucketUrl + key;
            }


            return string.Empty;
        }

        private string ConvertKey(string key)
        {
            if (key.StartsWith(BucketUrl))
            {
                key = key.Replace(BucketUrl, string.Empty);
            }

            return key;
        }

        public void Get(string key, Stream outputStream)
        {
            VerifyKey(key);
            key = ConvertKey(key);

            var request = new GetObjectRequest
            {
                BucketName = BucketName,
                Key = key
            };

            using (var response = S3.GetObject(request))
            {
                response.ResponseStream.CopyTo(outputStream);
            }

            outputStream.Position = 0; // Rewind the stream as the stream could be used again after the method returns.
        }

        public bool Exists(string key)
        {
            VerifyKey(key);
            key = ConvertKey(key);

            var request = new GetObjectMetadataRequest
            {
                BucketName = BucketName,
                Key = key,
            };

            var exists = false;

            try
            {
                S3.GetObjectMetadata(request);
                exists = true;
            }
            catch (AmazonS3Exception e)
            {
                if (e.ErrorCode != "NotFound")
                {
                    throw;
                }
            }

            return exists;
        }

        public void Delete(string key)
        {
            Delete(new [] { key });
        }

        public void Delete(string[] keys)
        {
            var request = new DeleteObjectsRequest
            {
                BucketName = BucketName,
                Quiet = true,
                Objects = keys.Select(k => new KeyVersion { Key = ConvertKey(k) }).ToList()
            };

            try
            {
                S3.DeleteObjects(request);
            }
            catch (DeleteObjectsException doe)
            {
                // From http://docs.aws.amazon.com/sdkfornet/latest/apidocs/items/TS3DeleteObjectsRequest_NET4_5.html
                var errorResponse = doe.Response;
                foreach (var deleteError in errorResponse.DeleteErrors)
                {
                    Log.Error("Error deleting item " + deleteError.Key);
                    Log.Error(" Code - " + deleteError.Code);
                    Log.Error(" Message - " + deleteError.Message);
                }

                throw;
            }
        }

        private static void VerifyKey(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("key is required");
            if (key[0] == '/') throw new ArgumentException("Key should not start with slash: " + key);
        }

        public IEnumerable<string> ListBuckets()
        {
            return Enumerable.ToList<string>(S3.ListBuckets().Buckets.Select(bucket => bucket.BucketName));
        }

/*        public IEnumerable<string> ListBucket(string bucketName)
        {
            var s3Obj = S3.GetObject(new GetObjectRequest {BucketName = bucketName});
        }*/

        public void Dispose()
        {
            if (S3 != null)
            {
                S3.Dispose();
            }
        }
    }
}