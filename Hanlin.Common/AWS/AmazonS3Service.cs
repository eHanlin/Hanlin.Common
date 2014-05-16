namespace Hanlin.Common.AWS
{
    public class AmazonS3Service : S3CompatibleService
    {
        public AmazonS3Service(string accessKey, string secretKey, string bucket) : base(null, accessKey, secretKey, bucket)
        {
            ServiceName = "AmazonS3";
        }
    }
}
