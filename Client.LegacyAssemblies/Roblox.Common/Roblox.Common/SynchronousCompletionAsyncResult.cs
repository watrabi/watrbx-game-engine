using System;
using System.Threading;

namespace Roblox.Common;

public class SynchronousCompletionAsyncResult : IAsyncResult
{
	private Exception _Error;

	private readonly AsyncCallback _Callback;

	private bool _IsCompleted = true;

	private readonly object _State;

	public WaitHandle AsyncWaitHandle
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public object AsyncState => _State;

	public bool CompletedSynchronously => true;

	public bool IsCompleted => _IsCompleted;

	public Exception Error => _Error;

	public SynchronousCompletionAsyncResult(AsyncCallback callback, object state)
	{
		_Callback = callback;
		_State = state;
	}

	public SynchronousCompletionAsyncResult(AsyncCallback callback, object state, bool setComplete)
	{
		_Callback = callback;
		_State = state;
		if (setComplete)
		{
			SetCompleted();
		}
	}

	public SynchronousCompletionAsyncResult(AsyncCallback callback, object state, Exception error)
	{
		_Callback = callback;
		_State = state;
		SetCompleted(error);
	}

	public void CheckResult()
	{
		if (_Error != null)
		{
			throw _Error;
		}
	}

	public void SetCompleted()
	{
		_IsCompleted = true;
		_Callback?.Invoke(this);
	}

	public void SetCompleted(Exception error)
	{
		_Error = error;
		SetCompleted();
	}
}
public class SynchronousCompletionAsyncResult<T> : SynchronousCompletionAsyncResult, IResult<T>
{
	private T result;

	[Obsolete("This property can throw, which is bad design. Use GetResult() and SetCompleted() instead")]
	public T Token => GetResult();

	public SynchronousCompletionAsyncResult(T token, AsyncCallback callback, object state)
		: base(callback, state)
	{
		result = token;
		SetCompleted();
	}

	public T GetResult()
	{
		if (base.Error != null)
		{
			throw base.Error;
		}
		return result;
	}
}
