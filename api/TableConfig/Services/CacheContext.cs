/*
* ==============================================================================
*
* FileName: CacheContext.cs
* Created: 2026/1/8 9:41:30
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TableConfig.Services
{
    public class CacheContext
    {
        public static readonly string GroupToken = "Token";

        private readonly IMemoryCache _cache;

        private static readonly ConcurrentDictionary<string, DateTime> _keys = new();

        public CacheContext(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Set(string group,string key, object value, TimeSpan expiresIn, bool absolute)
        {
            var _key = GetKey(group,key);

            AddKey(_key);

            _cache.Set(_key, value, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absolute ? expiresIn : null,
                SlidingExpiration = absolute ? null : expiresIn,
                PostEvictionCallbacks = {
                        new PostEvictionCallbackRegistration
                        {
                            EvictionCallback = (k, v, r, s) =>
                            {
                                _keys.TryRemove(k.ToString(), out _);
                            }
                        }
                    }
            });
        }

        public T Get<T>(string group, string key)
        {
            var _key = GetKey(group, key);
            return _cache.Get<T>(_key);
        }

        public void Remove(string group, string key)
        {
            var _key = GetKey(group, key);
            _cache.Remove(_key);

            RemoveKey(_key);
        }

        public List<string> GetKeys()
        {
            return _keys.Keys.ToList();
        }
        public List<string> GetKeys(string group)
        {
            return GetKeys().Where(m=>m.StartsWith(group+":")).ToList();
        }
        public IEnumerable<T> GetValues<T>(string group)
        {
            var keys = GetKeys(group);

            foreach (var item in keys)
            {
                yield return _cache.Get<T>(item);
            }
        }

        private string GetKey(string group, string key)
        {
            return $"{group}:{key}";
        }

        private void AddKey( string key)
        {
            _keys[key] = DateTime.Now;
        }
        private void RemoveKey(string key)
        {
            _keys.TryRemove(key, out _);
        }
    }


}
