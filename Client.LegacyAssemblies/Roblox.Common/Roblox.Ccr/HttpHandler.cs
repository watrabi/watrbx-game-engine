using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Web;
using Microsoft.Ccr.Core;
using Roblox.Common;

namespace Roblox.Ccr;

public abstract class HttpHandler : IHttpAsyncHandler, IHttpHandler
{
	private static long _PendingAsyncCalls;

	public abstract bool IsReusable { get; }

	static HttpHandler()
	{
		Thread thread = new Thread(MonitorPerformance);
		thread.IsBackground = true;
		thread.Name = "Performance Monitor: Ccr.HttpHandler";
		thread.Start();
	}

	protected abstract IEnumerator<ITask> Execute(HttpContext context);

	protected virtual bool SynchronousExecute(HttpContext context)
	{
		return false;
	}

	private IEnumerator<ITask> ExecuteAndComplete(HttpContext context, FastAsyncResult result)
	{
		IEnumerator<ITask> enu;
		try
		{
			enu = Execute(context);
		}
		catch (Exception completed2)
		{
			result.SetCompleted(completed2);
			yield break;
		}
		using (enu)
		{
			while (true)
			{
				try
				{
					if (!enu.MoveNext())
					{
						result.SetCompleted();
						break;
					}
				}
				catch (Exception completed)
				{
					result.SetCompleted(completed);
					break;
				}
				yield return enu.Current;
			}
		}
	}

	private static void MonitorPerformance()
	{
		try
		{
			string categoryName = "Roblox Ccr.HttpHandler";
			if (!PerformanceCounterCategory.Exists(categoryName))
			{
				CounterCreationDataCollection counterCreationDataCollection = new CounterCreationDataCollection();
				counterCreationDataCollection.Add(new CounterCreationData("Pending Async Calls", string.Empty, PerformanceCounterType.NumberOfItems64));
				PerformanceCounterCategory.Create(categoryName, string.Empty, PerformanceCounterCategoryType.SingleInstance, counterCreationDataCollection);
			}
			PerformanceCounter perfPendingAsyncCalls = new PerformanceCounter(categoryName, "Pending Async Calls", readOnly: false);
			while (true)
			{
				perfPendingAsyncCalls.RawValue = _PendingAsyncCalls;
				Thread.Sleep(500);
			}
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex)
		{
			ExceptionHandler.LogException(ex);
		}
	}

	public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
	{
		if (SynchronousExecute(context))
		{
			return new SynchronousCompletionAsyncResult(cb, extraData);
		}
		Interlocked.Increment(ref _PendingAsyncCalls);
		FastAsyncResult asyncResult = new FastAsyncResult(cb, extraData);
		CcrService.Singleton.SpawnIterator(context, asyncResult, ExecuteAndComplete);
		return asyncResult;
	}

	public void EndProcessRequest(IAsyncResult result)
	{
		if (result is FastAsyncResult fastResult)
		{
			Exception error = fastResult.Error;
			fastResult.Dispose();
			Interlocked.Decrement(ref _PendingAsyncCalls);
			if (error != null)
			{
				throw new ApplicationException("Roblox.Ccr.HttpHandler Error", error);
			}
		}
	}

	public void ProcessRequest(HttpContext context)
	{
		throw new NotImplementedException();
	}
}
