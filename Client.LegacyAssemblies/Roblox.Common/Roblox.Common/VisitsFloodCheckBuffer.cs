using System;
using System.Collections.Generic;

namespace Roblox.Common;

public class VisitsFloodCheckBuffer : IDisposable
{
	private readonly ObjectRegistry<int, IDictionary<long, DateTime>> userLogs;

	private static readonly TimeSpan floodCheckInterval = TimeSpan.FromHours(1.0);

	public static readonly VisitsFloodCheckBuffer Singleton = new VisitsFloodCheckBuffer();

	private VisitsFloodCheckBuffer()
	{
		userLogs = new ObjectRegistry<int, IDictionary<long, DateTime>>(new ObjectRegistry<int, IDictionary<long, DateTime>>.Configuration("VisitsFloodCheckBuffer")
		{
			Lease = floodCheckInterval,
			Getter = (int k) => new Dictionary<long, DateTime>(),
			PurgeFrequency = TimeSpan.FromMinutes(5.0)
		});
	}

	public bool PassesFloodCheck(int userId, long placeId)
	{
		if (!userLogs.TryGetValue(userId, out var log))
		{
			return true;
		}
		IDictionary<long, DateTime> sync = log;
		lock (sync)
		{
			if (log == null)
			{
				return true;
			}
			DateTime minValue;
			return !log.TryGetValue(placeId, out minValue) || minValue == DateTime.MinValue || DateTime.Now.Subtract(minValue) > floodCheckInterval;
		}
	}

	public bool RegisterVisit(int userId, long placeId)
	{
		DateTime now = DateTime.Now;
		IDictionary<long, DateTime> dict = userLogs.Get(userId);
		IDictionary<long, DateTime> sync = dict;
		KeyValuePair<long, DateTime> item = new KeyValuePair<long, DateTime>(placeId, now);
		lock (sync)
		{
			if (dict.TryGetValue(placeId, out var minValue))
			{
				if (!(now.Subtract(minValue) > floodCheckInterval))
				{
					dict.Remove(placeId);
					dict.Add(new KeyValuePair<long, DateTime>(placeId, DateTime.MinValue));
					return false;
				}
				dict.Remove(placeId);
			}
			dict.Add(item);
			return true;
		}
	}

	public void Dispose()
	{
		userLogs.Dispose();
	}
}
