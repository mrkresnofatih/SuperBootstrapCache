using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DesertCamel.BaseMicroservices.SuperBootstrap.Cache.SuperBootstrapCacheOption;

namespace DesertCamel.BaseMicroservices.SuperBootstrap.Cache
{
    public static class Extensions
    {
        public static void AddBootstrapCache(this IServiceCollection services, IConfiguration configuration)
        {
            var selectedProvider = configuration.GetSection("SuperBootstrap:Cache:Selected").Value;
            switch (selectedProvider)
            {
                case SuperBootstrapCacheOption.ProviderOption.REDIS:
                    var options = configuration.GetSection("SuperBootstrap:Cache:Options:Redis");
                    var redisOption = new RedisOption();
                    options.Bind(redisOption);

                    var redis = ConnectionMultiplexer.Connect(redisOption.ConnectionString);
                    
                    services.AddSingleton(redisOption);
                    services.AddSingleton(redis.GetDatabase());
                    services.AddSingleton<ISuperBootstrapCache, SuperBootstrapCache>();
                    break;
                case SuperBootstrapCacheOption.ProviderOption.MEMCACHED:
                default:
                    throw new Exception("Not Implemented Yet!");
            }
        }
    }
}
