using System;
using System.Threading;
using Microsoft.Ccr.Core;

namespace Roblox.Common;

public class RefreshAhead<T> : IDisposable
{
	private DateTime _LastRefresh = DateTime.MinValue;

	private readonly Timer _RefreshTimer;

	private T _Value;

	public TimeSpan IntervalSinceRefresh => DateTime.Now.Subtract(_LastRefresh);

	public T Value => _Value;

	private RefreshAhead(T initialValue, TimeSpan refreshInterval, Func<T> refreshDelegate)
	{
		RefreshAhead<T> refreshAhead = this;
		int refreshIntervalInMilliseconds = (int)refreshInterval.TotalMilliseconds;
		_Value = initialValue;
		_LastRefresh = DateTime.Now;
		_RefreshTimer = new Timer(delegate
		{
			refreshAhead.Refresh(refreshDelegate);
		}, null, refreshIntervalInMilliseconds, refreshIntervalInMilliseconds);
	}

	private RefreshAhead(T initialValue, TimeSpan refreshInterval, Action<PortSet<T, Exception>> refreshDelegate)
	{
		RefreshAhead<T> refreshAhead = this;
		int refreshIntervalInMilliseconds = (int)refreshInterval.TotalMilliseconds;
		_Value = initialValue;
		_LastRefresh = DateTime.Now;
		_RefreshTimer = new Timer(delegate
		{
			refreshAhead.Refresh(refreshDelegate);
		}, null, refreshIntervalInMilliseconds, refreshIntervalInMilliseconds);
	}

	public RefreshAhead(TimeSpan refreshInterval, Func<T> refreshDelegate)
	{
		RefreshAhead<T> refreshAhead = this;
		int refreshIntervalInMilliseconds = (int)refreshInterval.TotalMilliseconds;
		_RefreshTimer = new Timer(delegate
		{
			refreshAhead.Refresh(refreshDelegate);
		}, null, refreshIntervalInMilliseconds, refreshIntervalInMilliseconds);
	}

	private void Refresh(Func<T> refreshDelegate)
	{
		try
		{
			_Value = refreshDelegate();
			_LastRefresh = DateTime.Now;
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex)
		{
			ExceptionHandler.LogException(ex);
		}
	}

	private void Refresh(Action<PortSet<T, Exception>> refreshDelegate)
	{
		try
		{
			PortSet<T, Exception> valueResult = new PortSet<T, Exception>();
			refreshDelegate(valueResult);
			CcrService.Singleton.Choice(valueResult, delegate(T value)
			{
				_Value = value;
				_LastRefresh = DateTime.Now;
			});
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex)
		{
			ExceptionHandler.LogException(ex);
		}
	}

	public static RefreshAhead<T> ConstructAndPopulate(TimeSpan refreshInterval, Func<T> refreshDelegate)
	{
		T value = refreshDelegate();
		return new RefreshAhead<T>(value, refreshInterval, refreshDelegate);
	}

	public static void ConstructAndPopulate(TimeSpan refreshInterval, Action<PortSet<T, Exception>> refreshDelegate, PortSet<RefreshAhead<T>, Exception> result)
	{
		PortSet<T, Exception> valueResult = new PortSet<T, Exception>();
		refreshDelegate(valueResult);
		CcrService.Singleton.Choice(valueResult, delegate(T value)
		{
			RefreshAhead<T> item = new RefreshAhead<T>(value, refreshInterval, refreshDelegate);
			result.Post(item);
		}, delegate(Exception ex)
		{
			result.Post(ex);
		});
	}

	public void Dispose()
	{
		if (_RefreshTimer != null)
		{
			_RefreshTimer.Dispose();
		}
	}
}
