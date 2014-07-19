using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace Hanlin.Common.AWS
{
    public class S3CompatibleService : IS3Service, IDisposable
    {
        public string ServiceUrl { get; private set; }
        public IAmazonS3 S3 { get; private set; }

        public string ServiceName { get; set; }
        public string BucketName { get; set; }

        public S3CompatibleService(string endpoint, string accessKey, string secretKey, string bucket)
        {
            ServiceUrl = endpoint;
            ServiceName = "Hicloud";
            BucketName = bucket;

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

        public void Put(string key, string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                Put(key, stream);
            }
        }

        public string Put(string key, byte[] bytes)
        {
            return Put(key, new MemoryStream(bytes)); // It's not necessary to dispose the MemoryStream as it is backed by an array.
        }

        public string Put(string key, Stream inputStream)
        {
            VerifyKey(key);

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

            var response = S3.PutObject(request);

            inputStream.Position = 0; // Rewind the stream as the stream could be used again after the method returns.

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return "https://s3-ap-southeast-1.amazonaws.com/" + BucketName + "/" + key;
            }


            return string.Empty;
        }

        public void Get(string key, Stream outputStream)
        {
            VerifyKey(key);

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

        private static void VerifyKey(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("key is required");
            if (key[0] == '/') throw new ArgumentException("Key should not start with slash: " + key);
        }

        public IEnumerable<string> ListBuckets()
        {
            return S3.ListBuckets().Buckets.Select(bucket => bucket.BucketName).ToList();
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