using System;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SanShain.Bilichat.Extensions
{
    public static class DistributedCacheExtensions
    {
        public async static Task<T> GetJsonObjectAsync<T>(this IDistributedCache cache,string key, CancellationToken token = default(CancellationToken))
        {
            var result = await cache.GetStringAsync(key,token);
            if (result == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(result,
                new JsonSerializerSettings() { Error = (s, e) => { e.ErrorContext.Handled = true; } });
        }

        public async static Task SetJsonObjectAsync<T>(this IDistributedCache cache,string key,T @object, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            await cache.SetStringAsync(key,JsonConvert.SerializeObject(@object),options,token);
        }
    }
}
