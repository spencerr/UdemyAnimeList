using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UdemyAnimeList.Services.Storage
{
    public interface IBucketStorage
    {
        Task<bool> Put(IFormFile file, string key);
        Task<bool> Remove(string key);
    }
}
