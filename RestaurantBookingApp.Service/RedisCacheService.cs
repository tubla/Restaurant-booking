using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace RestaurantBookingApp.Service
{
    public class RedisCacheService : IRedisCacheService
    {
        private static IDatabase? _cache;

        public RedisCacheService(IConfiguration configuration)
        {
            if (_cache == null)
            {

                var lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                {
                    var redisCacheConnectionString = KeyVaultSecretReader.GetConnectionString(configuration, "RedisKeyVault");
                    return ConnectionMultiplexer.Connect(redisCacheConnectionString);
                });

                _cache = lazyConnection.Value.GetDatabase();
            }
        }

        public void CacheData(string key, string value)
        {
            var cachedData = _cache?.StringGet(key);
            if (cachedData.HasValue && !string.IsNullOrEmpty(cachedData.Value))
            {
                return;
            }
            _cache?.StringSet(key, value);

        }

        public string GetData(string key)
        {
            var cachedData = _cache?.StringGet(key);
            if (cachedData.HasValue)
            {
                return cachedData.ToString()!;
            }
            return string.Empty;
        }

        public T? GetDeserializedData<T>(string key)
        {
            var cachedData = _cache?.StringGet(key);
            if (cachedData.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(cachedData.ToString());
            }
            return default(T);
        }


        public void DeleteKey(string key)
        {
            _cache?.KeyDelete(key);
        }

        public bool HasKey(string key)
        {
            var cachedData = _cache?.StringGet(key);
            return cachedData.HasValue;
        }


    }
}
