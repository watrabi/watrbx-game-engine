using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;

namespace Roblox;

public class ObjectRegistry<TKey, TValue> : IDisposable, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
{
	private class Reference
	{
		protected readonly WeakReference weakReference;

		public virtual TValue Target => (TValue)weakReference.Target;

		public virtual bool IsAlive => weakReference.IsAlive;

		public Reference(TValue value)
		{
			weakReference = new WeakReference(value);
		}

		internal virtual void Renew(TValue value)
		{
			weakReference.Target = value;
		}
	}

	private class LeasedReference : Reference
	{
		private TValue strongReference;

		private DateTime expiration;

		private readonly TimeSpan lease;

		private readonly Random r = new Random();

		public override TValue Target
		{
			get
			{
				RenewLease();
				if (strongReference == null)
				{
					return (TValue)weakReference.Target;
				}
				return strongReference;
			}
		}

		public override bool IsAlive
		{
			get
			{
				if (strongReference == null)
				{
					return base.IsAlive;
				}
				return true;
			}
		}

		public LeasedReference(TValue value, TimeSpan Lease)
			: base(value)
		{
			strongReference = value;
			lease = Lease;
			expiration = DateTime.Now + Lease;
			ScheduleExpirationCheck();
		}

		private void ScheduleExpirationCheck()
		{
			TimeSpan span = TimeSpan.FromMilliseconds((1.0 + 0.2 * r.NextDouble()) * lease.TotalMilliseconds + 20.0);
			CcrService.Singleton.Delay(span, CheckExpiration);
		}

		private void CheckExpiration()
		{
			lock (weakReference)
			{
				if (expiration - DateTime.Now <= TimeSpan.Zero)
				{
					strongReference = default(TValue);
				}
				else
				{
					ScheduleExpirationCheck();
				}
			}
		}

		private void RenewLease()
		{
			object target = weakReference.Target;
			if (target == null)
			{
				return;
			}
			DateTime t = DateTime.Now - lease;
			lock (weakReference)
			{
				if (t > expiration)
				{
					expiration = t;
				}
				if (strongReference == null)
				{
					strongReference = (TValue)target;
					ScheduleExpirationCheck();
				}
			}
		}

		internal override void Renew(TValue value)
		{
			base.Renew(value);
			strongReference = value;
			RenewLease();
		}
	}

	public delegate TValue Getter(TKey key);

	public class Configuration
	{
		public TimeSpan Lease = TimeSpan.Zero;

		public Getter Getter;

		public TimeSpan PurgeFrequency = TimeSpan.FromSeconds(15.0);

		public readonly string Name;

		public Configuration(string name)
		{
			Name = name;
		}
	}

	public readonly TimeSpan Lease;

	public readonly bool HasLease;

	private int count;

	private readonly IDictionary<TKey, Reference>[] dictionaries;

	private readonly Getter getter;

	private readonly Timer timer;

	private static readonly string perfCategory;

	private readonly PerformanceCounter perfItemCount;

	private readonly PerformanceCounter perfItemTotalPurged;

	private readonly PerformanceCounter perfPurgeRate;

	public int Count => count;

	private IDictionary<TKey, Reference> GetDictionary(TKey key)
	{
		checked
		{
			return dictionaries[(int)(IntPtr)(long)unchecked((ulong)key.GetHashCode() % (ulong)dictionaries.Length)];
		}
	}

	static ObjectRegistry()
	{
		perfCategory = "Roblox.Common.ObjectRegistry.2";
		if (!PerformanceCounterCategory.Exists(perfCategory))
		{
			CounterCreationDataCollection collection = new CounterCreationDataCollection
			{
				new CounterCreationData("Count", "", PerformanceCounterType.NumberOfItems32),
				new CounterCreationData("Total Purged", "", PerformanceCounterType.NumberOfItems64),
				new CounterCreationData("Purge Rate", "", PerformanceCounterType.RateOfCountsPerSecond32)
			};
			PerformanceCounterCategory.Create(perfCategory, "", PerformanceCounterCategoryType.MultiInstance, collection);
		}
	}

	public ObjectRegistry(Configuration configuration)
	{
		Dictionary<TKey, Reference>[] references = new Dictionary<TKey, Reference>[1024];
		IDictionary<TKey, Reference>[] array = references;
		dictionaries = array;
		for (int i = 0; i < dictionaries.Length; i++)
		{
			dictionaries[i] = new Dictionary<TKey, Reference>();
		}
		perfItemCount = new PerformanceCounter(perfCategory, "Count", configuration.Name, readOnly: false);
		perfItemCount.RawValue = 0L;
		perfItemTotalPurged = new PerformanceCounter(perfCategory, "Total Purged", configuration.Name, readOnly: false);
		perfItemTotalPurged.RawValue = 0L;
		perfPurgeRate = new PerformanceCounter(perfCategory, "Purge Rate", configuration.Name, readOnly: false);
		perfPurgeRate.RawValue = 0L;
		getter = configuration.Getter;
		Lease = configuration.Lease;
		HasLease = Lease > TimeSpan.Zero;
		timer = new Timer(delegate
		{
			Purge();
		}, null, configuration.PurgeFrequency, configuration.PurgeFrequency);
	}

	private void IncrementPerfCountersForPurge(int magnitude)
	{
		perfPurgeRate?.IncrementBy(magnitude);
		perfItemCount?.IncrementBy(-1 * magnitude);
		perfItemTotalPurged?.IncrementBy(magnitude);
	}

	public void Add(TKey key, TValue value)
	{
		perfItemCount?.Increment();
		IDictionary<TKey, Reference> dict = GetDictionary(key);
		lock (dict)
		{
			dict.Add(key, HasLease ? new LeasedReference(value, Lease) : new Reference(value));
		}
		Interlocked.Increment(ref count);
	}

	public TValue Get(TKey key)
	{
		TValue val;
		if (getter == null)
		{
			TryGetValue(key, out val);
		}
		else
		{
			IDictionary<TKey, Reference> dict = GetDictionary(key);
			lock (dict)
			{
				if (!dict.TryGetValue(key, out var reference))
				{
					val = getter(key);
					if (val != null)
					{
						Add(key, val);
					}
				}
				else
				{
					val = reference.Target;
					if (val == null)
					{
						val = getter(key);
						reference.Renew(val);
					}
				}
			}
		}
		return val;
	}

	public ReadOnlyCollection<TValue> GetValues()
	{
		List<TValue> list = new List<TValue>();
		IDictionary<TKey, Reference>[] array = dictionaries;
		foreach (IDictionary<TKey, Reference> dict in array)
		{
			lock (dict)
			{
				foreach (Reference reference in dict.Values)
				{
					TValue target = reference.Target;
					if (target != null)
					{
						list.Add(target);
					}
				}
			}
		}
		return new ReadOnlyCollection<TValue>(list);
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		IDictionary<TKey, Reference> dict = GetDictionary(key);
		lock (dict)
		{
			if (!dict.TryGetValue(key, out var reference))
			{
				value = default(TValue);
				return false;
			}
			value = reference.Target;
			if (value == null)
			{
				IncrementPerfCountersForPurge(1);
				Interlocked.Decrement(ref count);
				dict.Remove(key);
				return false;
			}
		}
		return true;
	}

	public void Remove(TKey key)
	{
		perfItemCount?.Decrement();
		Interlocked.Decrement(ref count);
		IDictionary<TKey, Reference> dict = GetDictionary(key);
		lock (dict)
		{
			dict.Remove(key);
		}
	}

	public void Purge()
	{
		IDictionary<TKey, Reference>[] array = dictionaries;
		foreach (IDictionary<TKey, Reference> dict in array)
		{
			lock (dict)
			{
				ICollection<KeyValuePair<TKey, Reference>> coll = null;
				foreach (KeyValuePair<TKey, Reference> reference in dict)
				{
					if (!reference.Value.IsAlive)
					{
						if (coll == null)
						{
							coll = new List<KeyValuePair<TKey, Reference>>();
						}
						coll.Add(reference);
						Interlocked.Decrement(ref count);
					}
				}
				if (coll == null)
				{
					continue;
				}
				IncrementPerfCountersForPurge(coll.Count);
				foreach (KeyValuePair<TKey, Reference> i in coll)
				{
					dict.Remove(i);
				}
			}
		}
	}

	public void Dispose()
	{
		timer?.Dispose();
		perfItemCount?.Dispose();
		perfItemTotalPurged?.Dispose();
		perfPurgeRate?.Dispose();
	}

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		IDictionary<TKey, Reference>[] array = dictionaries;
		foreach (IDictionary<TKey, Reference> dict in array)
		{
			lock (dict)
			{
				foreach (KeyValuePair<TKey, Reference> reference in dict)
				{
					TValue target = reference.Value.Target;
					if (target != null)
					{
						yield return new KeyValuePair<TKey, TValue>(reference.Key, target);
					}
				}
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		IDictionary<TKey, Reference>[] array = dictionaries;
		foreach (IDictionary<TKey, Reference> dict in array)
		{
			lock (dict)
			{
				foreach (KeyValuePair<TKey, Reference> reference in dict)
				{
					TValue target = reference.Value.Target;
					if (target != null)
					{
						yield return new KeyValuePair<TKey, TValue>(reference.Key, target);
					}
				}
			}
		}
	}
}
