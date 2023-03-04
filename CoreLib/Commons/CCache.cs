using System;
using Microsoft.Extensions.Caching.Memory;

namespace CoreLib.Commons
{
    // Summary:
    //     Represents for cache of application.
    public static class CCache<T>
    {
        // Summary:
        //     Gets or create cache
        //
        // Returns:
        //     Object with type T
        public static T GetOrCreateCache(IMemoryCache cache, string key, Func<T, T> getData, T objSeach)
        {

            try
            {
                // Check key is null.
                if (string.IsNullOrEmpty(key)) return objSeach;

                // Check whether the cache exists or not.
                if (cache.TryGetValue(key.ToUpper(), out T t))
                {
                    return t;
                }
                else
                {
                    // Get data from objSearch. (getData is a List<>)
                    T v = getData(objSeach);

                    if (v != null)
                    {
                        // Create new cache.
                        T objT = cache.GetOrCreate<T>(key.ToUpper(), cacheEntry =>
                        {
                            // Add option for cache.
                            cacheEntry.AbsoluteExpiration = DateTime.Now.AddMinutes((24 - DateTime.Now.Hour) * 60);
                            return v;
                        });

                        return objT;
                    }

                    return v;
                }
            }
            catch (Exception e)
            {
                e.ToString();

                return objSeach;
            }
        }
    }
}
