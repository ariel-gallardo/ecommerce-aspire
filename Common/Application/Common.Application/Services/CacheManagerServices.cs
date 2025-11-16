using Common.Domain.Exceptions;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Common.Application.Services
{
    public class CacheManagerServices : ICacheManagerServices
    {
        private readonly IDistributedCache _cache;
        private readonly AppSettings _appSettings;
        private readonly DistributedCacheEntryOptions _cacheOptions;
        private readonly int _delayMilliseconds;
        private readonly int _retryTimes;
        private CancellationToken? _cancellationToken;
        public CacheManagerServices(IDistributedCache cache, IOptions<AppSettings> options)
        {
            _cache = cache;
            _appSettings = options.Value;
            _cacheOptions = _appSettings.Redis.DistributedCacheEntryOptions;
            _delayMilliseconds = _appSettings.Redis.SecondsToWaitDependency * 1000;
            _retryTimes = _appSettings.Redis.RetryTimesDependency-1;
        }
        public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var data = await _cache.GetStringAsync($"{key}", _cancellationToken ?? cancellationToken);
            if (!string.IsNullOrWhiteSpace(data))
            {
                data = Regex.Unescape(data);
                return JsonSerializer.Deserialize<T>(data);
            }
            else return default(T);
        }

        public async Task SaveUnlimitedAsync(string key, object data, CancellationToken cancellationToken = default)
        => await _cache.SetStringAsync($"{key}", JsonSerializer.Serialize(data), new DistributedCacheEntryOptions { }, _cancellationToken ?? cancellationToken);
        public async Task SaveAsync(string key, object data, CancellationToken cancellationToken = default)
        => await _cache.SetStringAsync($"{key}", JsonSerializer.Serialize(data), _cacheOptions, _cancellationToken ?? cancellationToken);

        public async Task SaveAsync(string key, object data, DistributedCacheEntryOptions opt = null, CancellationToken cancellationToken = default)
        => await _cache.SetStringAsync($"{key}", JsonSerializer.Serialize(data), opt ?? _cacheOptions, _cancellationToken ?? cancellationToken);

        public async Task WaitAsync(List<string> dependencies, CancellationToken cancellationToken = default)
        {
            int times = 0;
            List<string> foundKeys = new List<string>();
            if (dependencies.Any())
            while (true)
            {
                var allReady = (await Task.WhenAll(dependencies.Select(async d =>
                {
                    var value = await _cache.GetStringAsync($"{d}", _cancellationToken ?? cancellationToken);
                    if (!string.IsNullOrWhiteSpace(value)) value = Regex.Unescape(value);
                    if (!string.IsNullOrWhiteSpace(value)) lock(foundKeys) foundKeys.Add(d);
                    return !string.IsNullOrWhiteSpace(value) && JsonSerializer.Deserialize<bool>(value);
                }))).All(v => v);
                if (allReady) 
                        break;
                times++;
                if (times == _retryTimes) throw new CacheNotFoundException(string.Join(',',dependencies.Except(foundKeys)), times + 1);
                await Task.Delay(_delayMilliseconds, cancellationToken);
            }
        }

        public async Task WaitAsync(List<string> dependencies, int retryTimes, int secondsDelay, CancellationToken cancellationToken = default)
        {
            int times = 0;
            List<string> foundKeys = new List<string>();
            if (dependencies.Any())
                while (true)
                {
                    var allReady = (await Task.WhenAll(dependencies.Select(async d =>
                    {
                        var value = await _cache.GetStringAsync($"{d}", _cancellationToken ?? cancellationToken);
                        if (!string.IsNullOrWhiteSpace(value)) value = Regex.Unescape(value);
                        if (!string.IsNullOrWhiteSpace(value)) lock (foundKeys) foundKeys.Add(d);
                        return !string.IsNullOrWhiteSpace(value) && JsonSerializer.Deserialize<bool>(value);
                    }))).All(v => v);
                    if (allReady) 
                        break;
                    times++;
                    if (times == retryTimes) throw new CacheNotFoundException(string.Join(',', dependencies.Except(foundKeys)), times + 1);
                    await Task.Delay(secondsDelay * 1000, cancellationToken);
                }
        }

        public async Task WaitAsync(string dependency, CancellationToken cancellationToken = default)
        {
            int times = 0;
            if (!string.IsNullOrWhiteSpace(dependency))
            while (true)
            {
                var value = await _cache.GetStringAsync($"{dependency}", _cancellationToken ?? cancellationToken);
                if (!string.IsNullOrWhiteSpace(value)) value = Regex.Unescape(value);
                if (!string.IsNullOrWhiteSpace(value) && JsonSerializer.Deserialize<bool>(value)) 
                        break;
                times++; if (times == _retryTimes) throw new CacheNotFoundException(dependency, times + 1);
                await Task.Delay(_delayMilliseconds, cancellationToken);
            }
        }

        public async Task WaitAsync(string dependency, int retryTimes, int secondsDelay, CancellationToken cancellationToken = default)
        {
            int times = 0;
            if (!string.IsNullOrWhiteSpace(dependency))
                while (true)
                {
                    var value = await _cache.GetStringAsync($"{dependency}", _cancellationToken ?? cancellationToken);
                    if (!string.IsNullOrWhiteSpace(value)) value = Regex.Unescape(value);
                    if (!string.IsNullOrWhiteSpace(value) && JsonSerializer.Deserialize<bool>(value)) 
                        break;
                    times++; if (times == retryTimes) throw new CacheNotFoundException(dependency, times + 1);
                    await Task.Delay(secondsDelay * 1000, cancellationToken);
                }
        }

        public void SetCancellationToken(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _cancellationToken.Value.ThrowIfCancellationRequested();
        }

        public async Task RemoveAsync(params string[] keys)
        {
            var entities = Enumerable.Range(0, keys.Length - 1).Select(async i =>
            {
                try
                {
                    await _cache.RemoveAsync(keys[i], _cancellationToken.Value);
                }
                catch (Exception e)
                {

                }
            });
            if(entities.Any()) await Task.WhenAll(entities);
        }
    }
}
