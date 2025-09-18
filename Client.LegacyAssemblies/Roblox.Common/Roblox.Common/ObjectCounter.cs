using System;
using System.Diagnostics;

namespace Roblox.Common;

public class ObjectCounter<T>
{
	private class Count
	{
		private static readonly string perfCategory = "Roblox.Common.ObjectCounter";

		private readonly PerformanceCounter perfItemCount;

		private readonly PerformanceCounter perfCollectRate;

		public Count()
		{
			if (!PerformanceCounterCategory.Exists(perfCategory))
			{
				CounterCreationDataCollection counterCreationDataCollection = new CounterCreationDataCollection
				{
					new CounterCreationData("Count", string.Empty, PerformanceCounterType.NumberOfItems32),
					new CounterCreationData("Collect Rate", string.Empty, PerformanceCounterType.RateOfCountsPerSecond32)
				};
				PerformanceCounterCategory.Create(perfCategory, string.Empty, PerformanceCounterCategoryType.MultiInstance, counterCreationDataCollection);
			}
			string typeName = typeof(T).Name;
			Console.WriteLine($"ObjectCounter name: {typeName}");
			perfItemCount = new PerformanceCounter(perfCategory, "Count", readOnly: false);
			perfItemCount.RawValue = 0L;
			perfCollectRate = new PerformanceCounter(perfCategory, "Collect Rate", readOnly: false);
		}

		internal void Increment()
		{
			perfItemCount.Increment();
		}

		internal void Decrement()
		{
			perfItemCount.Decrement();
			perfCollectRate.Increment();
		}
	}

	private static readonly Count count = new Count();

	public ObjectCounter()
	{
		count.Increment();
	}

	~ObjectCounter()
	{
		count.Decrement();
	}
}
