using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyAnimeList.Data.Config;

namespace UdemyAnimeList.Web.Intrastructure.Services
{
    public interface IAzureFileStorage
    {
        BlobServiceClient ServiceClient { get; }
        BlobContainerClient Container { get; }
    }

    public class AzureFileStorage : IAzureFileStorage
    {
        private readonly AzureBlobSettings _azureBlobProperties;

        public AzureFileStorage(IOptions<AzureBlobSettings> azureBlobProperties)
        {
            _azureBlobProperties = azureBlobProperties.Value;

            ServiceClient = new BlobServiceClient(_azureBlobProperties.ConnectionString);
            Container = ServiceClient.GetBlobContainerClient(_azureBlobProperties.Container);
        }

        public BlobServiceClient ServiceClient { get; }

        public BlobContainerClient Container { get; }

    }
}
