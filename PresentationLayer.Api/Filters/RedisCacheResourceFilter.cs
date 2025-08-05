using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace PresentationLayer.Api.Filters
{
    public class RedisCacheResourceFilter : IResourceFilter
    {
        private readonly IDistributedCache _cache;
        private readonly int _duration;

        public RedisCacheResourceFilter(IDistributedCache cache, int duration)
        {
            _cache = cache;
            _duration = duration;
        }
        
        public async void OnResourceExecuting(ResourceExecutingContext context)
        {
            var cacheKey = context.HttpContext.Request.Path.ToString(); //     /api/products
            var cachedResponse = await _cache.GetStringAsync(cacheKey);
            if (cachedResponse != null)
            {
                context.Result = new ContentResult         // ShortCut
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                };
            }
        }

        public async void OnResourceExecuted(ResourceExecutedContext context)
        {
            if(context.Result is ObjectResult result && result.Value != null)
            {
                var cacheKey = context.HttpContext.Request.Path.ToString(); //     /api/products
                // serialization
                var serializedResponse = JsonSerializer.Serialize(result.Value);

                // set response in cache
                await _cache.SetStringAsync(cacheKey, serializedResponse, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_duration)
                });
            }
        }
    }
}
