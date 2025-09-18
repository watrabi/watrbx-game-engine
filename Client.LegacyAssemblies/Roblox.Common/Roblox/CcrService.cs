using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Ccr.Core;
using Roblox.Ccr;

namespace Roblox;

public class CcrService : CcrServiceBase, IDisposable
{
	private DispatcherMonitor _Monitor;

	public static readonly CcrService Singleton = new CcrService(monitor: false);

	public new DispatcherQueue TaskQueue => base.TaskQueue;

	private CcrService(bool monitor)
		: base(new PatchedDispatcherQueue("Roblox CcrService", new Dispatcher(0, ThreadPriority.Normal, DispatcherOptions.UseBackgroundThreads, "Roblox CcrService")))
	{
		if (monitor)
		{
			_Monitor = new DispatcherMonitor(TaskQueue.Dispatcher);
		}
		Thread performanceMonitor = new Thread(MonitorPerformance);
		performanceMonitor.IsBackground = true;
		performanceMonitor.Name = "Performance Monitor: CcrService";
		performanceMonitor.Start();
	}

	private void MonitorPerformance()
	{
		try
		{
			string performanceCategory = "Roblox CcrService";
			if (!PerformanceCounterCategory.Exists(performanceCategory))
			{
				CounterCreationDataCollection collection = new CounterCreationDataCollection();
				collection.Add(new CounterCreationData("TaskQueue Count", string.Empty, PerformanceCounterType.NumberOfItems32));
				collection.Add(new CounterCreationData("TaskQueue CurrentSchedulingRate", string.Empty, PerformanceCounterType.RateOfCountsPerSecond64));
				collection.Add(new CounterCreationData("TaskQueue ScheduledTaskCount", string.Empty, PerformanceCounterType.NumberOfItems64));
				collection.Add(new CounterCreationData("Dispatcher PendingTaskCount", string.Empty, PerformanceCounterType.NumberOfItems32));
				collection.Add(new CounterCreationData("Dispatcher ProcessedTaskCount", string.Empty, PerformanceCounterType.NumberOfItems64));
				collection.Add(new CounterCreationData("Dispatcher WorkerThreadCount", string.Empty, PerformanceCounterType.NumberOfItems32));
				PerformanceCounterCategory.Create(performanceCategory, string.Empty, PerformanceCounterCategoryType.SingleInstance, collection);
			}
			PerformanceCounter perfQueueCount = new PerformanceCounter(performanceCategory, "TaskQueue Count", readOnly: false);
			PerformanceCounter perfCurrentSchedulingRate = new PerformanceCounter(performanceCategory, "TaskQueue CurrentSchedulingRate", readOnly: false);
			PerformanceCounter perfScheduledTaskCount = new PerformanceCounter(performanceCategory, "TaskQueue ScheduledTaskCount", readOnly: false);
			PerformanceCounter perfPendingTaskCount = new PerformanceCounter(performanceCategory, "Dispatcher PendingTaskCount", readOnly: false);
			PerformanceCounter perfProcessdTaskCount = new PerformanceCounter(performanceCategory, "Dispatcher ProcessedTaskCount", readOnly: false);
			PerformanceCounter perfWorkerThreadCount = new PerformanceCounter(performanceCategory, "Dispatcher WorkerThreadCount", readOnly: false);
			long scheduledTaskCount = TaskQueue.ScheduledTaskCount;
			while (true)
			{
				perfQueueCount.RawValue = TaskQueue.Count;
				long count = TaskQueue.ScheduledTaskCount;
				perfCurrentSchedulingRate.IncrementBy(count - scheduledTaskCount);
				scheduledTaskCount = (perfScheduledTaskCount.RawValue = count);
				perfPendingTaskCount.RawValue = TaskQueue.Dispatcher.PendingTaskCount;
				perfProcessdTaskCount.RawValue = TaskQueue.Dispatcher.ProcessedTaskCount;
				perfWorkerThreadCount.RawValue = TaskQueue.Dispatcher.WorkerThreadCount;
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

	public bool BlockUntilCompletion(ITask task, TimeSpan timeout)
	{
		EventWaitHandle handle = new EventWaitHandle(initialState: false, EventResetMode.ManualReset);
		try
		{
			Port<EmptyValue> donePort = new Port<EmptyValue>();
			Arbiter.ExecuteToCompletion(TaskQueue, task, donePort);
			Activate<Receiver<EmptyValue>>(Arbiter.Receive(persist: false, donePort, delegate
			{
				handle.Set();
			}));
			return handle.WaitOne(timeout);
		}
		finally
		{
			if (handle != null)
			{
				((IDisposable)handle).Dispose();
			}
		}
	}

	public SuccessFailurePort Choice(Action<SuccessResult> successHandler, Action<Exception> failureHandler)
	{
		SuccessFailurePort result = new SuccessFailurePort();
		Choice(result, successHandler, failureHandler);
		return result;
	}

	public PortSet<T0, T1> Choice<T0, T1>(Action<T0> handler0, Action<T1> handler1)
	{
		PortSet<T0, T1> result = new PortSet<T0, T1>();
		Choice(result, handler0, handler1);
		return result;
	}

	public void Choice<T0, T1>(PortSet<T0, T1> resultPortSet, Action<T0> handler0, Action<T1> handler1)
	{
		Choice choice = Arbiter.Choice(resultPortSet, delegate(T0 result0)
		{
			try
			{
				handler0(result0);
			}
			catch (Exception ex2)
			{
				ExceptionHandler.LogException(ex2);
			}
		}, delegate(T1 result1)
		{
			try
			{
				handler1(result1);
			}
			catch (Exception ex)
			{
				ExceptionHandler.LogException(ex);
			}
		});
		Singleton.Activate<Choice>(choice);
	}

	public void Choice<T>(PortSet<T, Exception> resultPortSet, Action<T> successHandler)
	{
		Choice(resultPortSet, successHandler, ExceptionHandler.LogException);
	}

	public void Delay(TimeSpan timeSpan, Handler handler)
	{
		Port<DateTime> timeoutPort = TimeoutPort(timeSpan);
		Receiver<DateTime> receiver = Arbiter.Receive(persist: false, timeoutPort, delegate
		{
			handler();
		});
		Activate<Receiver<DateTime>>(receiver);
	}

	public void DelayInterator(TimeSpan timeSpan, IteratorHandler handler)
	{
		Port<DateTime> timeoutPort = TimeoutPort(timeSpan);
		Receiver<DateTime> receiver = Arbiter.Receive(persist: false, timeoutPort, delegate
		{
			SpawnIterator(handler);
		});
		Activate<Receiver<DateTime>>(receiver);
	}

	public ITask ExecuteToCompletion(IteratorHandler handler)
	{
		return Arbiter.ExecuteToCompletion(TaskQueue, new IterativeTask<bool>(t0: true, (bool notUsed) => handler()));
	}

	public ITask NestIterator(IteratorHandler handler)
	{
		return Arbiter.ExecuteToCompletion(base.TaskQueue, Arbiter.FromIteratorHandler(handler));
	}

	public Port<T> Receive<T>(bool persist, Action<T> handler)
	{
		Port<T> result = new Port<T>();
		Receive(persist, result, handler);
		return result;
	}

	public void Receive<T>(bool persist, Port<T> result, Action<T> handler)
	{
		Receiver<T> receiver = Arbiter.Receive(persist, result, delegate(T t)
		{
			try
			{
				handler(t);
			}
			catch (Exception ex)
			{
				ExceptionHandler.LogException(ex);
			}
		});
		Activate<Receiver<T>>(receiver);
	}

	public new void Spawn(Handler handler)
	{
		Task<bool> task = new Task<bool>(t0: true, delegate
		{
			handler();
		});
		TaskQueue.Enqueue(task);
	}

	public new void SpawnIterator(IteratorHandler handler)
	{
		IterativeTask<bool> iterativeTask = new IterativeTask<bool>(t0: true, (bool notUsed) => handler());
		TaskQueue.Enqueue(iterativeTask);
	}

	public new void SpawnIterator<T0>(T0 t0, IteratorHandler<T0> handler)
	{
		IterativeTask<bool> iterativeTask = new IterativeTask<bool>(t0: true, (bool notUsed) => handler(t0));
		TaskQueue.Enqueue(iterativeTask);
	}

	public new void SpawnIterator<T0, T1>(T0 t0, T1 t1, IteratorHandler<T0, T1> handler)
	{
		IterativeTask<bool> iterativeTask = new IterativeTask<bool>(t0: true, (bool notUsed) => handler(t0, t1));
		TaskQueue.Enqueue(iterativeTask);
	}

	public new void SpawnIterator<T0, T1, T2>(T0 t0, T1 t1, T2 t2, IteratorHandler<T0, T1, T2> handler)
	{
		IterativeTask<bool> iterativeTask = new IterativeTask<bool>(t0: true, (bool notUsed) => handler(t0, t1, t2));
		TaskQueue.Enqueue(iterativeTask);
	}

	public new Port<DateTime> TimeoutPort(TimeSpan ts)
	{
		return base.TimeoutPort(ts);
	}

	public ITask Wait(TimeSpan ts)
	{
		return base.TimeoutPort(ts).Receive();
	}

	public void Dispose()
	{
		if (_Monitor != null)
		{
			_Monitor.Dispose();
		}
		TaskQueue.Dispose();
		base.TaskQueue.Dispose();
	}
}
