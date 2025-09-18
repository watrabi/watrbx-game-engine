using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Ccr.Core;

namespace Roblox.Common;

public class FileHelper : IDisposable
{
	private static readonly int _DefaultPoolSize;

	private static readonly DispatcherQueue _DispatcherQueue;

	private static readonly Port<FileHelper> _Pool;

	private FileHelper()
	{
	}

	static FileHelper()
	{
		_DefaultPoolSize = 25;
		_DispatcherQueue = new PatchedDispatcherQueue("Roblox FileHelper", new Dispatcher(0, ThreadPriority.Normal, DispatcherOptions.UseBackgroundThreads, "Roblox FileHelper"));
		_Pool = new Port<FileHelper>();
		Thread thread = new Thread(MonitorPerformance);
		thread.IsBackground = true;
		thread.Name = "Performance Monitor: FileHelper";
		thread.Start();
		for (int i = 0; i < _DefaultPoolSize; i++)
		{
			AddToPool();
		}
	}

	public static void ExecuteTask<TResult>(Func<TResult> func, PortSet<TResult, Exception> result)
	{
		Port<FileHelper> port = new Port<FileHelper>();
		Get(port);
		CcrService.Singleton.Activate<Receiver<FileHelper>>(Arbiter.Receive(persist: false, port, delegate(FileHelper fh)
		{
			try
			{
				result.Post(func());
			}
			catch (Exception item)
			{
				result.Post(item);
			}
			finally
			{
				fh.Dispose();
			}
		}));
	}

	public static void ExecuteTask<Arg0, TResult>(Func<Arg0, TResult> func, Arg0 arg0, PortSet<TResult, Exception> result)
	{
		Port<FileHelper> port = new Port<FileHelper>();
		Get(port);
		CcrService.Singleton.Activate<Receiver<FileHelper>>(Arbiter.Receive(persist: false, port, delegate(FileHelper fh)
		{
			try
			{
				result.Post(func(arg0));
			}
			catch (Exception item)
			{
				result.Post(item);
			}
			finally
			{
				fh.Dispose();
			}
		}));
	}

	public static void AddToPool()
	{
		_Pool.Post(new FileHelper());
	}

	private static void Get(Port<FileHelper> result)
	{
		Arbiter.Activate(_DispatcherQueue, Arbiter.Receive(persist: false, _Pool, delegate(FileHelper h)
		{
			result.Post(h);
		}));
	}

	private static void MonitorPerformance()
	{
		try
		{
			string categoryName = "Roblox FileHelper";
			if (!PerformanceCounterCategory.Exists(categoryName))
			{
				CounterCreationDataCollection collection = new CounterCreationDataCollection();
				collection.Add(new CounterCreationData("Dispatcher Queue Count", string.Empty, PerformanceCounterType.NumberOfItems32));
				collection.Add(new CounterCreationData("Dispatcher Queue Current Scheduling Rate", string.Empty, PerformanceCounterType.RateOfCountsPerSecond64));
				collection.Add(new CounterCreationData("Dispatcher Queue Scheduled Task Count", string.Empty, PerformanceCounterType.NumberOfItems64));
				collection.Add(new CounterCreationData("Dispatcher Pending Task Count", string.Empty, PerformanceCounterType.NumberOfItems32));
				collection.Add(new CounterCreationData("Dispatcher Processed Task Count", string.Empty, PerformanceCounterType.NumberOfItems64));
				collection.Add(new CounterCreationData("Dispatcher Worker Thread Count", string.Empty, PerformanceCounterType.NumberOfItems32));
				PerformanceCounterCategory.Create(categoryName, string.Empty, PerformanceCounterCategoryType.SingleInstance, collection);
			}
			PerformanceCounter perfDispatcherQueueCount = new PerformanceCounter(categoryName, "Dispatcher Queue Count", readOnly: false);
			PerformanceCounter perfDispatcherQueueCurrentSchedulingRate = new PerformanceCounter(categoryName, "Dispatcher Queue Current Scheduling Rate", readOnly: false);
			PerformanceCounter perfDispatcherQueueScheduledTaskCount = new PerformanceCounter(categoryName, "Dispatcher Queue Scheduled Task Count", readOnly: false);
			PerformanceCounter perfDispatcherPendingTaskCount = new PerformanceCounter(categoryName, "Dispatcher Pending Task Count", readOnly: false);
			PerformanceCounter perfDispatcherProcessedTaskCount = new PerformanceCounter(categoryName, "Dispatcher Processed Task Count", readOnly: false);
			PerformanceCounter perfDispatcherWorkerThreadCount = new PerformanceCounter(categoryName, "Dispatcher Worker Thread Count", readOnly: false);
			long num = _DispatcherQueue.ScheduledTaskCount;
			while (true)
			{
				perfDispatcherQueueCount.RawValue = _DispatcherQueue.Count;
				long scheduledTaskCount = _DispatcherQueue.ScheduledTaskCount;
				perfDispatcherQueueCurrentSchedulingRate.IncrementBy(scheduledTaskCount - num);
				num = (perfDispatcherQueueScheduledTaskCount.RawValue = scheduledTaskCount);
				perfDispatcherPendingTaskCount.RawValue = _DispatcherQueue.Dispatcher.PendingTaskCount;
				perfDispatcherProcessedTaskCount.RawValue = _DispatcherQueue.Dispatcher.ProcessedTaskCount;
				perfDispatcherWorkerThreadCount.RawValue = _DispatcherQueue.Dispatcher.WorkerThreadCount;
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

	public void Dispose()
	{
		AddToPool();
	}
}
