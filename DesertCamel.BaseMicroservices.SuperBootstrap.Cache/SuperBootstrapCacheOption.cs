using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertCamel.BaseMicroservices.SuperBootstrap.Cache
{
    public class SuperBootstrapCacheOption
    {
        public class RedisOption
        {
            public string ConnectionString { get; set; }

            public string ServiceName { get; set; }
        }

        public class ProviderOption
        {
            public const string REDIS = "Redis";
            public const string MEMCACHED = "Memcached";
        }
    }
}
