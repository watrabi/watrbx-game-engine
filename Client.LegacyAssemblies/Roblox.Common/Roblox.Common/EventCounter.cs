using System;
using System.Threading;

namespace Roblox.Common;

public class EventCounter : IDisposable
{
	private class Bucket
	{
		private long eventCount;

		public long EventCount => eventCount;

		public Bucket(long value)
		{
			eventCount = value;
		}

		public void LogEvent()
		{
			Interlocked.Increment(ref eventCount);
		}
	}

	private class BucketAggregation
	{
		private Bucket current;

		private BucketAggregation next;

		public readonly TimeSpan Span;

		public BucketAggregation(TimeSpan span)
		{
			Span = span;
		}

		public void AddBucket(Bucket bucket)
		{
			if (current == null)
			{
				current = bucket;
				return;
			}
			if (next == null)
			{
				next = new BucketAggregation(Span.Add(Span));
			}
			next.AddBucket(new Bucket(current.EventCount + bucket.EventCount));
			current = null;
		}

		public long GetEventCount(TimeSpan span)
		{
			if (current != null)
			{
				if (span == Span)
				{
					return current.EventCount;
				}
				if (span > Span && next != null)
				{
					return current.EventCount + next.GetEventCount(span.Subtract(Span));
				}
				return current.EventCount * span.Ticks / span.Ticks;
			}
			if (next != null)
			{
				return next.GetEventCount(span);
			}
			return 0L;
		}

		public long GetTotalEventCount()
		{
			long eventCount = 0L;
			if (current != null)
			{
				eventCount += current.EventCount;
			}
			if (next != null)
			{
				eventCount += next.GetTotalEventCount();
			}
			return eventCount;
		}
	}

	private Bucket currentBucket;

	private readonly BucketAggregation head = new BucketAggregation(SmallestInterval);

	private readonly Timer timer;

	public static readonly TimeSpan SmallestInterval = TimeSpan.FromSeconds(2.0);

	public EventCounter()
	{
		currentBucket = new Bucket(0L);
		timer = new Timer(delegate
		{
			head.AddBucket(currentBucket);
			currentBucket = new Bucket(0L);
		}, null, SmallestInterval, SmallestInterval);
	}

	public long GetEventCount(TimeSpan sample)
	{
		if (sample == TimeSpan.MaxValue)
		{
			return GetTotalEventCount();
		}
		return head.GetEventCount(sample);
	}

	public double GetEventsPerSecond(TimeSpan sample)
	{
		long eventCount = GetEventCount(sample);
		if (sample == TimeSpan.Zero)
		{
			return 0.0;
		}
		return (double)eventCount / sample.TotalSeconds;
	}

	public long GetTotalEventCount()
	{
		return head.GetTotalEventCount();
	}

	public void LogEvent()
	{
		currentBucket.LogEvent();
	}

	public void Dispose()
	{
		timer.Dispose();
	}
}
