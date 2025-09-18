using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.Ccr.Core;
using Roblox.Common;

namespace Roblox.Ccr;

public abstract class HttpModule : IHttpModule
{
	protected abstract IEnumerator<ITask> Execute(HttpApplication httpApplication);

	private IAsyncResult BeginPreRequestHandlerExecute(object source, EventArgs e, AsyncCallback callback, object state)
	{
		HttpApplication t = (HttpApplication)source;
		FastAsyncResult result = new FastAsyncResult(callback, state);
		CcrService.Singleton.SpawnIterator(t, result, ExecuteAndComplete);
		return result;
	}

	private void EndPreRequestHandlerExecute(IAsyncResult result)
	{
		if (result is FastAsyncResult fastResult)
		{
			Exception error = fastResult.Error;
			fastResult.Dispose();
			if (error != null)
			{
				throw new ApplicationException("Roblox.Ccr.HttpModule Error", error);
			}
		}
	}

	private IEnumerator<ITask> ExecuteAndComplete(HttpApplication httpApplication, FastAsyncResult result)
	{
		IEnumerator<ITask> en;
		try
		{
			en = Execute(httpApplication);
		}
		catch (Exception completed2)
		{
			result.SetCompleted(completed2);
			yield break;
		}
		using (en)
		{
			while (true)
			{
				try
				{
					if (!en.MoveNext())
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
				yield return en.Current;
			}
		}
	}

	public void Dispose()
	{
	}

	public void Init(HttpApplication application)
	{
		application.AddOnPreRequestHandlerExecuteAsync(BeginPreRequestHandlerExecute, EndPreRequestHandlerExecute);
	}
}
