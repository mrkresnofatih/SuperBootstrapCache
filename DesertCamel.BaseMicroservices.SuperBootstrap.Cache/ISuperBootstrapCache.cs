using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DesertCamel.BaseMicroservices.SuperBootstrap.Cache.SuperBootstrapCacheOption;

namespace DesertCamel.BaseMicroservices.SuperBootstrap.Cache
{
    public interface ISuperBootstrapCache
    {
        Task<FuncResponse<bool>> Store(StoreRequest storeRequest);

        Task<FuncResponse<T>> Fetch<T>(FetchRequest fetchRequest);

        Task<FuncResponse<bool>> Delete(DeleteRequest deleteRequest);
    }

    public class SuperBootstrapCache : ISuperBootstrapCache
    {
        private readonly RedisOption _redisOption;
        private readonly IDatabase _redisDatabase;

        public SuperBootstrapCache(RedisOption redisOption, IDatabase redisDatabase)
        {
            _redisOption = redisOption;
            _redisDatabase = redisDatabase;
        }

        public async Task<FuncResponse<bool>> Delete(DeleteRequest deleteRequest)
        {
            try
            {
                var key = _GetKey(deleteRequest.Key);
                await _redisDatabase.KeyDeleteAsync(key);

                return new FuncResponse<bool>
                {
                    Data = true
                };
            }
            catch(Exception e)
            {
                return new FuncResponse<bool>
                {
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<FuncResponse<T>> Fetch<T>(FetchRequest fetchRequest)
        {
            try
            {
                var key = _GetKey(fetchRequest.Key);
                var value = await _redisDatabase.StringGetAsync(key);
                if (value.HasValue)
                {
                    var parsed = JsonConvert.DeserializeObject<T>(value);
                    return new FuncResponse<T>
                    {
                        Data = parsed
                    };
                }

                throw new Exception($"Cache with key: {key} is not found");
            }
            catch(Exception e)
            {
                return new FuncResponse<T>
                {
                    ErrorMessage = e.Message,
                };
            }
        }

        public async Task<FuncResponse<bool>> Store(StoreRequest storeRequest)
        {
            try
            {
                var key = _GetKey(storeRequest.Key);
                var jsonValue = JsonConvert.SerializeObject(storeRequest.Value);
                await _redisDatabase.StringSetAsync(key, jsonValue, TimeSpan.FromSeconds(storeRequest.TimeToLive));

                return new FuncResponse<bool>
                {
                    Data = true
                };
            }
            catch(Exception e)
            {
                return new FuncResponse<bool>
                {
                    ErrorMessage = e.Message
                };
            }
        }

        private string _GetKey(string key)
        {
            return String.Format("{0}-{1}", _redisOption.ServiceName, key);
        }
    }

    public class StoreRequest
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public int TimeToLive { get; set; }
    }

    public class FetchRequest
    {
        public string Key { get; set; }
    }

    public class DeleteRequest
    {
        public string Key { get; set; }
    }
}
