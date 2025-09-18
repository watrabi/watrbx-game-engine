using System;
using System.Threading;

namespace Roblox;

public static class RobloxThreadPool
{
	private static int _QueueLength;

	public static int QueueLength => _QueueLength;

	public static void GetAvailableThreads(out int workerThreads, out int completionPortThreads)
	{
		ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
	}

	public static bool QueueUserWorkItem(Action callback)
	{
		Interlocked.Increment(ref _QueueLength);
		return ThreadPool.QueueUserWorkItem(delegate
		{
			try
			{
				callback();
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				ExceptionHandler.LogException(ex2);
			}
			finally
			{
				Interlocked.Decrement(ref _QueueLength);
			}
		});
	}
}
