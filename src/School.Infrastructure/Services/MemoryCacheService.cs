using Microsoft.Extensions.Caching.Memory;
using School.Application.Interfaces.Services;
using System.Collections.Concurrent;

namespace School.Infrastructure.Services;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private static readonly ConcurrentDictionary<string, byte> _cacheKeys = new();

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Task<T?> GetAsync<T>(string key)
    {
        _memoryCache.TryGetValue(key, out T? value);
        return Task.FromResult(value);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5)
        };

        _memoryCache.Set(key, value, cacheOptions);
        _cacheKeys.TryAdd(key, 0);

        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _memoryCache.Remove(key);
        _cacheKeys.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task RemoveByPrefixAsync(string prefix)
    {
        var keysToRemove = _cacheKeys.Keys
            .Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .ToList();

        foreach (var key in keysToRemove)
        {
            _memoryCache.Remove(key);
            _cacheKeys.TryRemove(key, out _);
        }

        return Task.CompletedTask;
    }
}