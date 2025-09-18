using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Web.Services.Protocols;
using Microsoft.Ccr.Core;
using Roblox.Ccr;

namespace Roblox.Common;

public class AsyncHelper
{
	private struct AsyncLookupItem<T0, T1>
	{
		public int Index;

		public T0 Key;

		public PortSet<T1, Exception> Result;

		public AsyncLookupItem(int index, T0 key, PortSet<T1, Exception> result)
		{
			Index = index;
			Key = key;
			Result = result;
		}
	}

	public delegate void DoLookup<T0, T1>(T0 key, PortSet<T1, Exception> result);

	private static readonly string performanceCategory;

	private static readonly PerformanceCounter perfTotalAsyncCalls;

	private static readonly PerformanceCounter timeoutCounts;

	private static readonly PerformanceCounter riskyTimeoutCounts;

	private static readonly PerformanceCounter perfPendingAsyncCallsCount;

	static AsyncHelper()
	{
		performanceCategory = "Roblox.AsyncHelper";
		if (!PerformanceCounterCategory.Exists(performanceCategory))
		{
			CounterCreationDataCollection collection = new CounterCreationDataCollection
			{
				new CounterCreationData("Pending Async Calls", string.Empty, PerformanceCounterType.NumberOfItems64),
				new CounterCreationData("Total Async Calls", string.Empty, PerformanceCounterType.NumberOfItems64),
				new CounterCreationData("Timeouts", string.Empty, PerformanceCounterType.NumberOfItems64),
				new CounterCreationData("Risky Timeouts", string.Empty, PerformanceCounterType.NumberOfItems64)
			};
			PerformanceCounterCategory.Create(performanceCategory, string.Empty, PerformanceCounterCategoryType.SingleInstance, collection);
		}
		perfTotalAsyncCalls = new PerformanceCounter(performanceCategory, "Total Async Calls", readOnly: false);
		perfTotalAsyncCalls.RawValue = 0L;
		timeoutCounts = new PerformanceCounter(performanceCategory, "Timeouts", readOnly: false);
		timeoutCounts.RawValue = 0L;
		riskyTimeoutCounts = new PerformanceCounter(performanceCategory, "Risky Timeouts", readOnly: false);
		riskyTimeoutCounts.RawValue = 0L;
		perfPendingAsyncCallsCount = new PerformanceCounter(performanceCategory, "Pending Async Calls", readOnly: false);
		perfPendingAsyncCallsCount.RawValue = 0L;
	}

	private static AsyncLookupItem<T0, T1>[] GetAsyncLookupItems<T0, T1>(ICollection<T0> lookupKeys, DoLookup<T0, T1> asyncLookup)
	{
		int index = 0;
		AsyncLookupItem<T0, T1>[] asyncLookupItems = new AsyncLookupItem<T0, T1>[lookupKeys.Count];
		foreach (T0 lookupKey in lookupKeys)
		{
			AsyncLookupItem<T0, T1> asyncLookupItem = new AsyncLookupItem<T0, T1>(index, lookupKey, new PortSet<T1, Exception>());
			asyncLookup(asyncLookupItem.Key, asyncLookupItem.Result);
			asyncLookupItems[index] = asyncLookupItem;
			index++;
		}
		return asyncLookupItems;
	}

	private static IEnumerator<ITask> GetCollectionIterator<T0, T1>(ICollection<T0> keys, DoLookup<T0, T1> itemGetter, PortSet<ICollection<T1>, Exception> result)
	{
		AsyncLookupItem<T0, T1>[] lookupItems = GetAsyncLookupItems(keys, itemGetter);
		using IEnumerator<ITask> enumerarator = HandleAsyncLookupItems(lookupItems, result);
		while (enumerarator.MoveNext())
		{
			yield return enumerarator.Current;
		}
	}

	private static IEnumerator<ITask> HandleAsyncLookupItems<T0, T1>(AsyncLookupItem<T0, T1>[] asyncLookupItems, PortSet<ICollection<T1>, Exception> result)
	{
		int countDown = asyncLookupItems.Length;
		if (countDown == 0)
		{
			result.Post(new List<T1>());
			yield break;
		}
		T1[] items = new T1[asyncLookupItems.Length];
		for (int i = 0; i < asyncLookupItems.Length; i++)
		{
			AsyncLookupItem<T0, T1> asyncLookupItem = asyncLookupItems[i];
			yield return (Choice)asyncLookupItem.Result;
			Exception ex = asyncLookupItem.Result.Test<Exception>();
			if (ex != null)
			{
				result.Post(ex);
				break;
			}
			items[asyncLookupItem.Index] = asyncLookupItem.Result;
			if (Interlocked.Decrement(ref countDown) == 0)
			{
				result.Post(items);
				break;
			}
		}
	}

	public static Choice Call<TResult>(Func<AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, PortSet<TResult, Exception> result, TimeSpan timeout, Action finalizer)
	{
		Choice choice = result;
		try
		{
			IAsyncResult asyncResult = begin(delegate(IAsyncResult ar)
			{
				perfPendingAsyncCallsCount.Decrement();
				PortSet<TResult, Exception> portSet2 = Interlocked.Exchange(ref result, null);
				if (portSet2 != null)
				{
					asyncResult = null;
				}
				try
				{
					TResult item = end(ar);
					portSet2?.Post(item);
				}
				catch (Exception item2)
				{
					portSet2?.Post(item2);
				}
				finally
				{
					FinalizeOnce(ref finalizer);
				}
			}, null);
			perfPendingAsyncCallsCount.Increment();
			perfTotalAsyncCalls.Increment();
			if (timeout < TimeSpan.MaxValue)
			{
				CcrService.Singleton.Activate<Receiver<DateTime>>(Arbiter.Receive(persist: false, CcrService.Singleton.TimeoutPort(timeout), delegate(DateTime time)
				{
					PortSet<TResult, Exception> portSet = Interlocked.Exchange(ref result, null);
					if (portSet != null)
					{
						portSet.Post(new TimeoutException(string.Format("AsyncHelper: timeout of {1} before {0}", end, time)));
						timeoutCounts.Increment();
						if (!(asyncResult is WebClientAsyncResult webClientAsyncResult))
						{
							riskyTimeoutCounts.Increment();
						}
						else
						{
							webClientAsyncResult.Abort();
						}
					}
					FinalizeOnce(ref finalizer);
				}));
			}
		}
		catch (Exception ex)
		{
			FinalizeOnce(ref finalizer);
			Interlocked.Exchange(ref result, null)?.Post(ex);
		}
		return choice;
	}

	private static void FinalizeOnce(ref Action finalizer)
	{
		Interlocked.Exchange(ref finalizer, null)?.Invoke();
	}

	public static TResult BlockingCall<TResult>(Func<AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, TimeSpan timeout, Action finalizer)
	{
		EventWaitHandle wait = new EventWaitHandle(initialState: false, EventResetMode.ManualReset);
		try
		{
			ExceptionPort<TResult> result = new ExceptionPort<TResult>();
			Call(begin, end, result, timeout, finalizer);
			CcrService.Singleton.Activate<Choice>(Arbiter.Choice(result, delegate(TResult t)
			{
				result.Post(t);
				wait.Set();
			}, delegate(Exception e)
			{
				result.Post(e);
				wait.Set();
			}));
			wait.WaitOne();
			return result;
		}
		finally
		{
			if (wait != null)
			{
				((IDisposable)wait).Dispose();
			}
		}
	}

	public static void Call<TResult, Arg0>(Func<Arg0, AsyncCallback, object, IAsyncResult> begin, Arg0 arg0, Func<IAsyncResult, TResult> end, PortSet<TResult, Exception> result, TimeSpan timeout, Action finalizer)
	{
		Call((AsyncCallback a, object o) => begin(arg0, a, o), end, result, timeout, finalizer);
	}

	public static Choice Call<TResult, Arg0, Arg1>(Func<Arg0, Arg1, AsyncCallback, object, IAsyncResult> begin, Arg0 arg0, Arg1 arg1, Func<IAsyncResult, TResult> end, PortSet<TResult, Exception> result, TimeSpan timeout, Action finalizer)
	{
		return Call((AsyncCallback a, object o) => begin(arg0, arg1, a, o), end, result, timeout, finalizer);
	}

	public static void Call<TResult, Arg0, Arg1, Arg2>(Func<Arg0, Arg1, Arg2, AsyncCallback, object, IAsyncResult> begin, Arg0 arg0, Arg1 arg1, Arg2 arg2, Func<IAsyncResult, TResult> end, PortSet<TResult, Exception> result, TimeSpan timeout, Action finalizer)
	{
		Call((AsyncCallback a, object o) => begin(arg0, arg1, arg2, a, o), end, result, timeout, finalizer);
	}

	public static void Call<TResult, Arg0, Arg1, Arg2, Arg3>(Func<Arg0, Arg1, Arg2, Arg3, AsyncCallback, object, IAsyncResult> begin, Arg0 arg0, Arg1 arg1, Arg2 arg2, Arg3 arg3, Func<IAsyncResult, TResult> end, PortSet<TResult, Exception> result, TimeSpan timeout, Action finalizer)
	{
		Call((AsyncCallback a, object o) => begin(arg0, arg1, arg2, arg3, a, o), end, result, timeout, finalizer);
	}

	public static void Call<TResult, Arg0, Arg1, Arg2, Arg3, Arg4>(Func<Arg0, Arg1, Arg2, Arg3, Arg4, AsyncCallback, object, IAsyncResult> begin, Arg0 arg0, Arg1 arg1, Arg2 arg2, Arg3 arg3, Arg4 arg4, Func<IAsyncResult, TResult> end, PortSet<TResult, Exception> result, TimeSpan timeout, Action finalizer)
	{
		Call((AsyncCallback a, object o) => begin(arg0, arg1, arg2, arg3, arg4, a, o), end, result, timeout, finalizer);
	}

	public static void Call(Func<AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end, SuccessFailurePort result, TimeSpan timeout, Action finalizer)
	{
		Call((AsyncCallback a, object o) => begin(a, o), delegate(IAsyncResult a)
		{
			end(a);
			return SuccessResult.Instance;
		}, result, timeout, finalizer);
	}

	public static void Call<Arg0>(Func<Arg0, AsyncCallback, object, IAsyncResult> begin, Arg0 arg0, Action<IAsyncResult> end, SuccessFailurePort result, TimeSpan timeout, Action finalizer)
	{
		Call((AsyncCallback a, object o) => begin(arg0, a, o), delegate(IAsyncResult a)
		{
			end(a);
			return SuccessResult.Instance;
		}, result, timeout, finalizer);
	}

	public static void Call<Arg0, Arg1>(Func<Arg0, Arg1, AsyncCallback, object, IAsyncResult> begin, Arg0 arg0, Arg1 arg1, Action<IAsyncResult> end, SuccessFailurePort result, TimeSpan timeout, Action finalizer)
	{
		Call((AsyncCallback a, object o) => begin(arg0, arg1, a, o), delegate(IAsyncResult a)
		{
			end(a);
			return SuccessResult.Instance;
		}, result, timeout, finalizer);
	}

	public static void Call<Arg0, Arg1, Arg2>(Func<Arg0, Arg1, Arg2, AsyncCallback, object, IAsyncResult> begin, Arg0 arg0, Arg1 arg1, Arg2 arg2, Action<IAsyncResult> end, SuccessFailurePort result, TimeSpan timeout, Action finalizer)
	{
		Call((AsyncCallback a, object o) => begin(arg0, arg1, arg2, a, o), delegate(IAsyncResult a)
		{
			end(a);
			return SuccessResult.Instance;
		}, result, timeout, finalizer);
	}

	public static void Call<Arg0, Arg1, Arg2, Arg3>(Func<Arg0, Arg1, Arg2, Arg3, AsyncCallback, object, IAsyncResult> begin, Arg0 arg0, Arg1 arg1, Arg2 arg2, Arg3 arg3, Action<IAsyncResult> end, SuccessFailurePort result, TimeSpan timeout, Action finalizer)
	{
		Call((AsyncCallback a, object o) => begin(arg0, arg1, arg2, arg3, a, o), delegate(IAsyncResult a)
		{
			end(a);
			return SuccessResult.Instance;
		}, result, timeout, finalizer);
	}

	public static void Call<Arg0, Arg1, Arg2, Arg3, Arg4>(Func<Arg0, Arg1, Arg2, Arg3, Arg4, AsyncCallback, object, IAsyncResult> begin, Arg0 arg0, Arg1 arg1, Arg2 arg2, Arg3 arg3, Arg4 arg4, Action<IAsyncResult> end, SuccessFailurePort result, TimeSpan timeout, Action finalizer)
	{
		Call((AsyncCallback a, object o) => begin(arg0, arg1, arg2, arg3, arg4, a, o), delegate(IAsyncResult a)
		{
			end(a);
			return SuccessResult.Instance;
		}, result, timeout, finalizer);
	}

	public static void GetCollection<T0, T1>(ICollection<T0> keys, DoLookup<T0, T1> itemGetter, PortSet<ICollection<T1>, Exception> result)
	{
		CcrService.Singleton.SpawnIterator(keys, itemGetter, result, GetCollectionIterator);
	}
}
