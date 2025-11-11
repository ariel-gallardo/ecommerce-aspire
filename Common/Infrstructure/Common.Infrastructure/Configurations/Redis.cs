using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json.Serialization;

namespace Common.Infrastructure.Configurations
{
    public class Redis
    {
        public string DataMount { get; set; }
        public int SecondsToWaitDependency { get; set; }
        public int ExpirationMinutesCache { get; set; }
        public int RetryTimesDependency { get; set; }
        public string Configuration { get; set; }
        public string InstanceName { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public DistributedCacheEntryOptions DistributedCacheEntryOptions => new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(ExpirationMinutesCache)
        };
    }
}
