using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace grenius_api.Application.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache, CancellationToken cancellationToken, string id,
            T data,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(120);
            options.SlidingExpiration = unusedExpireTime;

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(id, jsonData, options, cancellationToken);
        }

        public static async Task<T?> GetRecordAsync<T>(this IDistributedCache cache,
            CancellationToken cancellationToken,
            string id)
        {
            var jsonData = await cache.GetStringAsync(id, cancellationToken);
            if (jsonData == null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
