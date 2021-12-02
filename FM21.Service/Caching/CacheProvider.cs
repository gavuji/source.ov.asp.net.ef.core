using FM21.Core;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FM21.Service.Caching
{
    /// <summary>
    /// <see cref="https://github.com/sahan91/InMemoryCacheNetCore/"/>
    /// </summary>
    public class CacheProvider : ICacheProvider
    {
        private readonly int cacheSeconds;
        private readonly IMemoryCache _cache;

        public CacheProvider(IMemoryCache cache)
        {
            _cache = cache;
            cacheSeconds = ApplicationConstants.CacheDurationInSecond;
        }

        public T GetFromCache<T>(string key) where T : class
        {
            var cachedResponse = _cache.Get(key);
            return cachedResponse as T;
        }

        public void SetCache<T>(string key, T value) where T : class
        {
            SetCache(key, value, DateTimeOffset.Now.AddSeconds(cacheSeconds));
        }

        public void SetCache<T>(string key, T value, DateTimeOffset duration) where T : class
        {
            _cache.Set(key, value, duration);
        }

        public void ClearCache(string key)
        {
            _cache.Remove(key);
        }

        public static byte[] SerializeToByteArray(object obj)
        {
            if (obj == null)
            {
                return new byte[] { };
            }
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T Deserialize<T>(byte[] byteArray) where T : new()
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(byteArray, 0, byteArray.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = (T)binForm.Deserialize(memStream);
                return obj;
            }
        }
    }
}