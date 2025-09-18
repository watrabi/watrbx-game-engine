using Microsoft.Ccr.Core;

namespace Roblox;

public interface IAsyncDictionary<TKey, TValue>
{
	void Add(TKey key, TValue value, Port<EmptyValue> result);

	void ContainsKey(TKey key, Port<bool> result);

	void Remove(TKey key, Port<bool> result);

	void TryGetValue(TKey key, PortSet<TValue, EmptyValue> result);

	void GetOrCreate(TKey key, Port<TValue> result);
}
