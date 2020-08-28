using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyAnimeList.Data;

namespace UdemyAnimeList.Web.Intrastructure.Services
{
    public interface IConfigurationCache
    {
        Task<T> Get<T>(string key);
    }

    public class ConfigurationCache : IConfigurationCache
    {
        private IMemoryCache _memoryCache;
        private readonly ApplicationDbContext _context;

        private MemoryCacheEntryOptions CacheTime 
            => new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(15));

        public ConfigurationCache(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<T> Get<T>(string key)
        {
            if (!_memoryCache.TryGetValue(key, out T value))
            {
                var entry = await _context.Configuration.FindAsync(key);
                if (entry?.Value == null)
                {
                    return default;
                }

                value = (T) Convert.ChangeType(entry.Value, typeof(T));
                _memoryCache.Set(key, value, CacheTime);
            }

            return value;
        }
    }
}
