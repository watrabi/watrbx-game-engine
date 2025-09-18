using System.Collections.Generic;
using Microsoft.Ccr.Core;

namespace Roblox;

public class AsyncDictionary<TKey, TValue> : IAsyncDictionary<TKey, TValue> where TValue : new()
{
	private readonly Interleaver interleaver = new Interleaver();

	private readonly IDictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

	public void Add(TKey key, TValue value, Port<EmptyValue> result)
	{
		interleaver.DoExclusive(delegate
		{
			dictionary.Add(key, value);
			result.Post(EmptyValue.SharedInstance);
		});
	}

	public void GetOrCreate(TKey key, Port<TValue> result)
	{
		interleaver.DoExclusive(delegate
		{
			if (!dictionary.TryGetValue(key, out var value))
			{
				value = new TValue();
				dictionary.Add(key, value);
			}
			result.Post(value);
		});
	}

	public void ContainsKey(TKey key, Port<bool> result)
	{
		interleaver.DoConcurrent(delegate
		{
			result.Post(dictionary.ContainsKey(key));
		});
	}

	public void Remove(TKey key, Port<bool> result)
	{
		interleaver.DoExclusive(delegate
		{
			result.Post(dictionary.Remove(key));
		});
	}

	public void TryGetValue(TKey key, PortSet<TValue, EmptyValue> result)
	{
		interleaver.DoConcurrent(delegate
		{
			if (dictionary.TryGetValue(key, out var value))
			{
				result.Post(value);
			}
			else
			{
				result.Post(EmptyValue.SharedInstance);
			}
		});
	}
}
