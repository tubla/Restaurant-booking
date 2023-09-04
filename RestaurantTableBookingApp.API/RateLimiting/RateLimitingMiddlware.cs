using Newtonsoft.Json;
using RestaurantBookingApp.Core;
using RestaurantBookingApp.Service;
using System.Net;

namespace RestaurantTableBookingApp.API.RateLimiting
{
    public class RateLimitingMiddlware
    {
        private readonly RequestDelegate _next;

        public RateLimitingMiddlware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IRedisCacheService redisCacheService)
        {
            var endPoint = context.GetEndpoint();
            var decorator = endPoint?.Metadata.GetMetadata<LimitRequests>();
            if (decorator is null)
            {
                await _next(context);
                return;
            }

            var key = GenerateClientKey(context);
            var clientStatistics = redisCacheService.GetDeserializedData<ClientStatistics>(key)!;
            if (clientStatistics != null &&
                DateTime.UtcNow < clientStatistics.LastSuccessfulResponseTime.AddSeconds(decorator.TimeWindow) &&
                clientStatistics.NumberOfRequestsCompletedSuccessfully > decorator.MaxRequests)
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests; //429
                return;
            }

            redisCacheService.CacheData(key, JsonConvert.SerializeObject(new ClientStatistics
            {
                LastSuccessfulResponseTime = DateTime.Now,
                NumberOfRequestsCompletedSuccessfully = decorator.MaxRequests

            }));

            await _next(context);
        }

        private static string GenerateClientKey(HttpContext context)
        {
            return $"{context.Request.Path}_{context.Connection.RemoteIpAddress}";
        }
    }
}
