using System;

namespace Roblox.Common;

public class Result<T>
{
	private Exception _Exception;

	private T _Value;

	public Exception Exception => _Exception;

	public T Value
	{
		get
		{
			if (_Exception != null)
			{
				throw _Exception;
			}
			return _Value;
		}
	}

	public Result(T value)
	{
		_Value = value;
	}

	public Result(Exception exception)
	{
		_Exception = exception;
	}

	public void Test(Action<T> successHandler, Action<Exception> failureHandler)
	{
		try
		{
			if (_Exception != null)
			{
				throw _Exception;
			}
			successHandler?.Invoke(_Value);
		}
		catch (Exception ex)
		{
			failureHandler?.Invoke(ex);
		}
	}

	public void Test(Action<T> successHandler, Action<Exception> failureHandler, Action cleanupHandler)
	{
		try
		{
			if (_Exception != null)
			{
				throw _Exception;
			}
			successHandler?.Invoke(_Value);
		}
		catch (Exception ex)
		{
			failureHandler?.Invoke(ex);
		}
		finally
		{
			cleanupHandler?.Invoke();
		}
	}
}
