using EuroBankAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System.Text.Json;

namespace EuroBankAPI.Models
{
    public class ResponseCacheService:ICacheService
    {
        private readonly StackExchange.Redis.IDatabase _database;
        public ResponseCacheService(StackExchange.Redis.IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response == null) return;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var serialisedResponse = JsonSerializer.Serialize(response, options);

            await _database.StringSetAsync(cacheKey, serialisedResponse, timeToLive);
        }


        public async Task<string> GetCachedResponse(string cacheKey)
        {
            var cachedResponse = await _database.StringGetAsync(cacheKey);

            if (cachedResponse.IsNullOrEmpty) return null;

            return cachedResponse;
        }
    }
}
