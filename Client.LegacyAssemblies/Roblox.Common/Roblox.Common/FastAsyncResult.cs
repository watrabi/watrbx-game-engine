using System;
using System.Threading;

namespace Roblox.Common;

public class FastAsyncResult : IAsyncResult, IDisposable
{
	private Exception _Error;

	private readonly AsyncCallback _Callback;

	private bool _IsCompleted = true;

	private readonly object _State;

	private ManualResetEvent _WaitHandle;

	public WaitHandle AsyncWaitHandle => CreateWaitHandle();

	public object AsyncState => _State;

	public bool CompletedSynchronously => false;

	public bool IsCompleted => _IsCompleted;

	public Exception Error => _Error;

	public FastAsyncResult(AsyncCallback callback, object state)
	{
		_Callback = callback;
		_State = state;
	}

	private WaitHandle CreateWaitHandle()
	{
		if (_WaitHandle != null)
		{
			return _WaitHandle;
		}
		ManualResetEvent resetEvt = new ManualResetEvent(initialState: false);
		if (Interlocked.CompareExchange(ref _WaitHandle, resetEvt, null) != null)
		{
			resetEvt.Close();
		}
		if (_IsCompleted)
		{
			_WaitHandle.Set();
		}
		return _WaitHandle;
	}

	public void Dispose()
	{
		_WaitHandle?.Close();
	}

	public void SetCompleted()
	{
		_IsCompleted = true;
		Thread.MemoryBarrier();
		_WaitHandle?.Set();
		_Callback?.Invoke(this);
	}

	public void SetCompleted(Exception error)
	{
		_Error = error;
		SetCompleted();
	}

	public void SetFailed(Exception error)
	{
		_Error = error;
	}
}
public class FastAsyncResult<T> : FastAsyncResult, IResult<T>
{
	private T result;

	[Obsolete("This property can throw, which is bad design. Use GetResult() and SetCompleted() instead")]
	public T Token
	{
		get
		{
			return GetResult();
		}
		set
		{
			result = value;
		}
	}

	public FastAsyncResult(AsyncCallback callback, object state)
		: base(callback, state)
	{
	}

	public T GetResult()
	{
		if (base.Error != null)
		{
			throw base.Error;
		}
		return result;
	}

	public void SetCompleted(T result)
	{
		this.result = result;
		SetCompleted();
	}
}
