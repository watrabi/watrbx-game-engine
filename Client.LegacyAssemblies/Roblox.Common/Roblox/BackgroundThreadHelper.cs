using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Roblox;

public class BackgroundThreadHelper
{
	public delegate void F();

	public class Handle : IDisposable
	{
		private EventWaitHandle eventWaitHandle;

		internal Handle(EventWaitHandle eventWaitHandle)
		{
			this.eventWaitHandle = eventWaitHandle;
			lock (waitHandles)
			{
				waitHandles.Add(eventWaitHandle);
			}
		}

		public void Set()
		{
			eventWaitHandle.Set();
		}

		public void Dispose()
		{
			lock (waitHandles)
			{
				waitHandles.Remove(eventWaitHandle);
			}
		}
	}

	private static bool done;

	private static readonly EventWaitHandle doneHandle;

	private static readonly List<EventWaitHandle> waitHandles;

	public static bool IsDone => done;

	static BackgroundThreadHelper()
	{
		done = false;
		doneHandle = new EventWaitHandle(initialState: false, EventResetMode.ManualReset);
		waitHandles = new List<EventWaitHandle>();
		EventLog.WriteEntry("Web Server", "BackgroundThreadHelper Startup", EventLogEntryType.Information, 8732);
		AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
	}

	private static void CurrentDomain_DomainUnload(object sender, EventArgs e)
	{
		EventLog.WriteEntry("Web Server", "BackgroundThreadHelper DomainUnload", EventLogEntryType.Information, 8732);
		EventWaitHandle[] newWaitHandles;
		lock (waitHandles)
		{
			newWaitHandles = new EventWaitHandle[waitHandles.Count];
			waitHandles.CopyTo(newWaitHandles);
		}
		EventLog.WriteEntry("Web Server", "BackgroundThreadHelper Setting doneHandle", EventLogEntryType.Information, 8732);
		done = true;
		doneHandle.Set();
		EventLog.WriteEntry("Web Server", "BackgroundThreadHelper doneHandle Set", EventLogEntryType.Information, 8732);
		for (int i = 0; i < newWaitHandles.Length; i++)
		{
			newWaitHandles[i].Set();
		}
		EventLog.WriteEntry("Web Server", "BackgroundThreadHelper waitHandles Set", EventLogEntryType.Information, 8732);
	}

	public static bool Wait(TimeSpan span)
	{
		return doneHandle.WaitOne(span, exitContext: false);
	}

	public static bool Wait(TimeSpan span, bool exitContext)
	{
		return doneHandle.WaitOne(span, exitContext);
	}

	public static Thread RunInBackground(TimeSpan sleepTime, F f)
	{
		Thread thread = new Thread((ThreadStart)delegate
		{
			while (true)
			{
				try
				{
					Thread.Sleep(sleepTime);
					f();
				}
				catch (ThreadInterruptedException)
				{
					break;
				}
				catch (ThreadAbortException)
				{
					break;
				}
				catch (Exception ex3)
				{
					ExceptionHandler.LogException(ex3);
				}
			}
		})
		{
			IsBackground = true
		};
		thread.Start();
		return thread;
	}

	public static Handle SetOnProcessExit(EventWaitHandle eventWaitHandle)
	{
		return new Handle(eventWaitHandle);
	}
}
