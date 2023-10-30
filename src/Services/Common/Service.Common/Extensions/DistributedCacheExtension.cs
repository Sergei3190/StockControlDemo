using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;

using System.Text;

namespace Service.Common.Extensions;

// расширение для Redis
public static class DistributedCacheExtension
{
	public static async Task<T?> GetAsync<T>(this IDistributedCache cache, string key)
	{
		ArgumentNullException.ThrowIfNull(cache, nameof(cache));

		var model = await cache.GetAsync(key).ConfigureAwait(false);

		return FromByteArray<T>(model!);
	}

	public static T? Get<T>(this IDistributedCache cache, string key)
	{
		ArgumentNullException.ThrowIfNull(cache, nameof(cache));

		var model = cache.Get(key);

		return FromByteArray<T>(model!);
	}

	public static async Task SetDataAsync<T>(this IDistributedCache cache, string key, T data,
		DistributedCacheEntryOptions options)
	{
		ArgumentNullException.ThrowIfNull(cache, nameof(cache));

		var bytes = ToByteArray(data);

		await cache.SetAsync(key, bytes!, options).ConfigureAwait(false);
	}

	public static void SetData<T>(this IDistributedCache cache, string key, T? data,
		DistributedCacheEntryOptions options)
	{
		ArgumentNullException.ThrowIfNull(cache, nameof(cache));

		var bytes = ToByteArray(data);

		cache.Set(key, bytes!, options);
	}

	public static bool TryGet<T>(this IDistributedCache cache, string key, out T data)
	{
		ArgumentNullException.ThrowIfNull(cache, nameof(cache));

		var value = cache.Get(key);

		if (value is null)
		{
			data = default!;
			return false;
		}
		
		data = FromByteArray<T>(value)!;
		return true;
	}

	private static byte[]? ToByteArray<T>(T data)
	{
		if (data is null)
			return null;

		var str = JsonConvert.SerializeObject(data);
		return Encoding.UTF8.GetBytes(str);
	}

	private static T? FromByteArray<T>(byte[] data)
	{
		if (data is null)
			return default;

		var json = Encoding.UTF8.GetString(data);
		return JsonConvert.DeserializeObject<T>(json);
	}
}
