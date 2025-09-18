using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.Ccr.Core;
using Roblox.Common.Properties;

namespace Roblox.Ccr;

public class DispatcherMonitor : IDisposable
{
	private readonly Dispatcher dispatcher;

	private readonly ICollection<Thread> threads = new HashSet<Thread>();

	private Thread thread;

	public DispatcherMonitor(Dispatcher dispatcher)
	{
		this.dispatcher = dispatcher;
		GatherWorkerThreads();
		dispatcher.UnhandledException += dispatcher_UnhandledException;
		thread = new Thread(Monitor);
		thread.IsBackground = true;
		thread.Name = "DispatcherMonitor: " + dispatcher.Name;
		thread.Start();
	}

	private void dispatcher_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		ExceptionHandler.LogException(new ApplicationException($"{dispatcher.Name} had an unhandled exception", e.ExceptionObject as Exception));
	}

	private void Monitor()
	{
		int count = 0;
		TimeSpan sleep = TimeSpan.Zero;
		while (true)
		{
			try
			{
				int pendingTaskCount = dispatcher.PendingTaskCount;
				int trigger = Settings.Default.CcrServiceBacklogTrigger;
				if (trigger <= 0)
				{
					sleep = TimeSpan.FromSeconds(10.0);
				}
				else if (pendingTaskCount > trigger)
				{
					count++;
					if (count >= 4)
					{
						ReportBacklog(pendingTaskCount);
						sleep = TimeSpan.FromMinutes(1.0);
					}
				}
				else
				{
					count = 0;
				}
			}
			catch (ThreadAbortException)
			{
				break;
			}
			catch (Exception ex)
			{
				ExceptionHandler.LogException(ex);
			}
			if (sleep != TimeSpan.Zero)
			{
				Thread.Sleep(sleep);
				sleep = TimeSpan.Zero;
				continue;
			}
			TimeSpan wait = Settings.Default.CcrServiceBacklogTriggerInterval;
			if (wait != TimeSpan.Zero)
			{
				Thread.Sleep((int)(wait.TotalMilliseconds / 4.0));
			}
			else
			{
				Thread.Sleep(TimeSpan.FromSeconds(10.0));
			}
		}
	}

	private void GatherWorkerThreads()
	{
		int count = 2 * dispatcher.WorkerThreadCount;
		for (int i = 0; i < 2 * dispatcher.WorkerThreadCount; i++)
		{
			dispatcher.DispatcherQueues[0].Enqueue(Arbiter.FromHandler(delegate
			{
				Thread.Sleep(200);
				lock (threads)
				{
					threads.Add(Thread.CurrentThread);
				}
				Interlocked.Decrement(ref count);
			}));
		}
		while (count > 0)
		{
			Thread.Sleep(10);
		}
	}

	private IEnumerable<StackTrace> GetWorkerStacks()
	{
		foreach (Thread t in threads)
		{
			t.Suspend();
			StackTrace trace;
			try
			{
				trace = new StackTrace(t, needFileInfo: true);
			}
			catch (Exception)
			{
				continue;
			}
			finally
			{
				t.Resume();
			}
			yield return trace;
		}
	}

	private void ReportBacklog(int pendingTasks)
	{
		string message = $"CcrService detected a backlog of {pendingTasks}. These are the currently running tasks:\r\n\r\n";
		foreach (StackTrace trace in GetWorkerStacks())
		{
			message = message + trace.ToString() + "\r\n\r\n";
		}
		ExceptionHandler.LogException(message, EventLogEntryType.Warning, 4061);
	}

	public void Dispose()
	{
		if (dispatcher != null)
		{
			dispatcher.Dispose();
		}
		threads.Clear();
	}
}
