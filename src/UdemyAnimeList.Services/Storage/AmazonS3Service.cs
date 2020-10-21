using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UdemyAnimeList.Services.Storage
{
    public class AmazonS3Configuration 
    {
        public string BucketName { get; set; }
        public string CdnUrl { get; set; }
    }

    public class AmazonS3Service : IBucketStorage
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly AmazonS3Configuration _amazonS3Config;

        public AmazonS3Service(IAmazonS3 amazonS3, IOptions<AmazonS3Configuration> amazonS3Config)
        {
            _amazonS3 = amazonS3;
            _amazonS3Config = amazonS3Config.Value;
        }

        public async Task<bool> Put(IFormFile file, string key)
        {

            var bytes = new byte[file.Length];
            file.OpenReadStream().Read(bytes, 0, (int)file.Length);

            using var stream = new MemoryStream(bytes);
            var request = new PutObjectRequest
            {
                BucketName = _amazonS3Config.BucketName,
                Key = key,
                InputStream = stream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead
            };

            var response = await _amazonS3.PutObjectAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> Remove(string key)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _amazonS3Config.BucketName,
                Key = key
            };

            var response = await _amazonS3.DeleteObjectAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
