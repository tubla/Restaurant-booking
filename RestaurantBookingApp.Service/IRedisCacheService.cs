namespace RestaurantBookingApp.Service
{
    public interface IRedisCacheService
    {
        void CacheData(string key, string value);
        string GetData(string key);
        T? GetDeserializedData<T>(string key);

        void DeleteKey(string key);

        bool HasKey(string key);
    }
}