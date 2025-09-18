using System;
using Microsoft.Ccr.Core;

namespace Roblox;

public class ParallelDictionary<TKey, TValue> : IAsyncDictionary<TKey, TValue> where TValue : new()
{
	private readonly AsyncDictionary<TKey, TValue>[] dictionaries = new AsyncDictionary<TKey, TValue>[64];

	public ParallelDictionary()
	{
		for (int i = 0; i < dictionaries.Length; i++)
		{
			dictionaries[i] = new AsyncDictionary<TKey, TValue>();
		}
	}

	private AsyncDictionary<TKey, TValue> GetDictionary(TKey key)
	{
		uint hashCode = (uint)key.GetHashCode();
		checked
		{
			return dictionaries[(int)(IntPtr)(long)unchecked((ulong)hashCode % (ulong)dictionaries.Length)];
		}
	}

	public void Add(TKey key, TValue value, Port<EmptyValue> result)
	{
		GetDictionary(key).Add(key, value, result);
	}

	public void ContainsKey(TKey key, Port<bool> result)
	{
		GetDictionary(key).ContainsKey(key, result);
	}

	public void Remove(TKey key, Port<bool> result)
	{
		GetDictionary(key).Remove(key, result);
	}

	public void TryGetValue(TKey key, PortSet<TValue, EmptyValue> result)
	{
		GetDictionary(key).TryGetValue(key, result);
	}

	public void GetOrCreate(TKey key, Port<TValue> result)
	{
		GetDictionary(key).GetOrCreate(key, result);
	}
}
