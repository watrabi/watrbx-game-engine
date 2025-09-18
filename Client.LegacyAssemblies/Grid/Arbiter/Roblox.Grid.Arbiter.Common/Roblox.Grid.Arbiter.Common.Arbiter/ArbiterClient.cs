using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;

namespace Roblox.Grid.Arbiter.Common.Arbiter;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ArbiterClient : ClientBase<Arbiter>, Arbiter
{
	private BeginOperationDelegate onBeginGetStatsExDelegate;

	private EndOperationDelegate onEndGetStatsExDelegate;

	private SendOrPostCallback onGetStatsExCompletedDelegate;

	private BeginOperationDelegate onBeginGetStatsDelegate;

	private EndOperationDelegate onEndGetStatsDelegate;

	private SendOrPostCallback onGetStatsCompletedDelegate;

	private BeginOperationDelegate onBeginSetMultiProcessDelegate;

	private EndOperationDelegate onEndSetMultiProcessDelegate;

	private SendOrPostCallback onSetMultiProcessCompletedDelegate;

	private BeginOperationDelegate onBeginSetRecycleProcessDelegate;

	private EndOperationDelegate onEndSetRecycleProcessDelegate;

	private SendOrPostCallback onSetRecycleProcessCompletedDelegate;

	private BeginOperationDelegate onBeginSetThreadConfigScriptDelegate;

	private EndOperationDelegate onEndSetThreadConfigScriptDelegate;

	private SendOrPostCallback onSetThreadConfigScriptCompletedDelegate;

	private BeginOperationDelegate onBeginSetRecycleQueueSizeDelegate;

	private EndOperationDelegate onEndSetRecycleQueueSizeDelegate;

	private SendOrPostCallback onSetRecycleQueueSizeCompletedDelegate;

	public event EventHandler<GetStatsExCompletedEventArgs> GetStatsExCompleted;

	public event EventHandler<GetStatsCompletedEventArgs> GetStatsCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetMultiProcessCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetRecycleProcessCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetThreadConfigScriptCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetRecycleQueueSizeCompleted;

	public ArbiterClient()
	{
	}

	public ArbiterClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ArbiterClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ArbiterClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ArbiterClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public string GetStatsEx(bool clearExceptions)
	{
		return base.Channel.GetStatsEx(clearExceptions);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetStatsEx(bool clearExceptions, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetStatsEx(clearExceptions, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string EndGetStatsEx(IAsyncResult result)
	{
		return base.Channel.EndGetStatsEx(result);
	}

	private IAsyncResult OnBeginGetStatsEx(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool clearExceptions = (bool)inValues[0];
		return BeginGetStatsEx(clearExceptions, callback, asyncState);
	}

	private object[] OnEndGetStatsEx(IAsyncResult result)
	{
		string retVal = EndGetStatsEx(result);
		return new object[1] { retVal };
	}

	private void OnGetStatsExCompleted(object state)
	{
		if (this.GetStatsExCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetStatsExCompleted(this, new GetStatsExCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetStatsExAsync(bool clearExceptions)
	{
		GetStatsExAsync(clearExceptions, null);
	}

	public void GetStatsExAsync(bool clearExceptions, object userState)
	{
		if (onBeginGetStatsExDelegate == null)
		{
			onBeginGetStatsExDelegate = OnBeginGetStatsEx;
		}
		if (onEndGetStatsExDelegate == null)
		{
			onEndGetStatsExDelegate = OnEndGetStatsEx;
		}
		if (onGetStatsExCompletedDelegate == null)
		{
			onGetStatsExCompletedDelegate = OnGetStatsExCompleted;
		}
		InvokeAsync(onBeginGetStatsExDelegate, new object[1] { clearExceptions }, onEndGetStatsExDelegate, onGetStatsExCompletedDelegate, userState);
	}

	public string GetStats()
	{
		return base.Channel.GetStats();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetStats(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetStats(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string EndGetStats(IAsyncResult result)
	{
		return base.Channel.EndGetStats(result);
	}

	private IAsyncResult OnBeginGetStats(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetStats(callback, asyncState);
	}

	private object[] OnEndGetStats(IAsyncResult result)
	{
		string retVal = EndGetStats(result);
		return new object[1] { retVal };
	}

	private void OnGetStatsCompleted(object state)
	{
		if (this.GetStatsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetStatsCompleted(this, new GetStatsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetStatsAsync()
	{
		GetStatsAsync(null);
	}

	public void GetStatsAsync(object userState)
	{
		if (onBeginGetStatsDelegate == null)
		{
			onBeginGetStatsDelegate = OnBeginGetStats;
		}
		if (onEndGetStatsDelegate == null)
		{
			onEndGetStatsDelegate = OnEndGetStats;
		}
		if (onGetStatsCompletedDelegate == null)
		{
			onGetStatsCompletedDelegate = OnGetStatsCompleted;
		}
		InvokeAsync(onBeginGetStatsDelegate, null, onEndGetStatsDelegate, onGetStatsCompletedDelegate, userState);
	}

	public void SetMultiProcess(bool value)
	{
		base.Channel.SetMultiProcess(value);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetMultiProcess(bool value, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetMultiProcess(value, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetMultiProcess(IAsyncResult result)
	{
		base.Channel.EndSetMultiProcess(result);
	}

	private IAsyncResult OnBeginSetMultiProcess(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool value = (bool)inValues[0];
		return BeginSetMultiProcess(value, callback, asyncState);
	}

	private object[] OnEndSetMultiProcess(IAsyncResult result)
	{
		EndSetMultiProcess(result);
		return null;
	}

	private void OnSetMultiProcessCompleted(object state)
	{
		if (this.SetMultiProcessCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetMultiProcessCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetMultiProcessAsync(bool value)
	{
		SetMultiProcessAsync(value, null);
	}

	public void SetMultiProcessAsync(bool value, object userState)
	{
		if (onBeginSetMultiProcessDelegate == null)
		{
			onBeginSetMultiProcessDelegate = OnBeginSetMultiProcess;
		}
		if (onEndSetMultiProcessDelegate == null)
		{
			onEndSetMultiProcessDelegate = OnEndSetMultiProcess;
		}
		if (onSetMultiProcessCompletedDelegate == null)
		{
			onSetMultiProcessCompletedDelegate = OnSetMultiProcessCompleted;
		}
		InvokeAsync(onBeginSetMultiProcessDelegate, new object[1] { value }, onEndSetMultiProcessDelegate, onSetMultiProcessCompletedDelegate, userState);
	}

	public void SetRecycleProcess(bool value)
	{
		base.Channel.SetRecycleProcess(value);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetRecycleProcess(bool value, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetRecycleProcess(value, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetRecycleProcess(IAsyncResult result)
	{
		base.Channel.EndSetRecycleProcess(result);
	}

	private IAsyncResult OnBeginSetRecycleProcess(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool value = (bool)inValues[0];
		return BeginSetRecycleProcess(value, callback, asyncState);
	}

	private object[] OnEndSetRecycleProcess(IAsyncResult result)
	{
		EndSetRecycleProcess(result);
		return null;
	}

	private void OnSetRecycleProcessCompleted(object state)
	{
		if (this.SetRecycleProcessCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetRecycleProcessCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetRecycleProcessAsync(bool value)
	{
		SetRecycleProcessAsync(value, null);
	}

	public void SetRecycleProcessAsync(bool value, object userState)
	{
		if (onBeginSetRecycleProcessDelegate == null)
		{
			onBeginSetRecycleProcessDelegate = OnBeginSetRecycleProcess;
		}
		if (onEndSetRecycleProcessDelegate == null)
		{
			onEndSetRecycleProcessDelegate = OnEndSetRecycleProcess;
		}
		if (onSetRecycleProcessCompletedDelegate == null)
		{
			onSetRecycleProcessCompletedDelegate = OnSetRecycleProcessCompleted;
		}
		InvokeAsync(onBeginSetRecycleProcessDelegate, new object[1] { value }, onEndSetRecycleProcessDelegate, onSetRecycleProcessCompletedDelegate, userState);
	}

	public void SetThreadConfigScript(string script, bool broadcast)
	{
		base.Channel.SetThreadConfigScript(script, broadcast);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetThreadConfigScript(string script, bool broadcast, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetThreadConfigScript(script, broadcast, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetThreadConfigScript(IAsyncResult result)
	{
		base.Channel.EndSetThreadConfigScript(result);
	}

	private IAsyncResult OnBeginSetThreadConfigScript(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string script = (string)inValues[0];
		bool broadcast = (bool)inValues[1];
		return BeginSetThreadConfigScript(script, broadcast, callback, asyncState);
	}

	private object[] OnEndSetThreadConfigScript(IAsyncResult result)
	{
		EndSetThreadConfigScript(result);
		return null;
	}

	private void OnSetThreadConfigScriptCompleted(object state)
	{
		if (this.SetThreadConfigScriptCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetThreadConfigScriptCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetThreadConfigScriptAsync(string script, bool broadcast)
	{
		SetThreadConfigScriptAsync(script, broadcast, null);
	}

	public void SetThreadConfigScriptAsync(string script, bool broadcast, object userState)
	{
		if (onBeginSetThreadConfigScriptDelegate == null)
		{
			onBeginSetThreadConfigScriptDelegate = OnBeginSetThreadConfigScript;
		}
		if (onEndSetThreadConfigScriptDelegate == null)
		{
			onEndSetThreadConfigScriptDelegate = OnEndSetThreadConfigScript;
		}
		if (onSetThreadConfigScriptCompletedDelegate == null)
		{
			onSetThreadConfigScriptCompletedDelegate = OnSetThreadConfigScriptCompleted;
		}
		InvokeAsync(onBeginSetThreadConfigScriptDelegate, new object[2] { script, broadcast }, onEndSetThreadConfigScriptDelegate, onSetThreadConfigScriptCompletedDelegate, userState);
	}

	public void SetRecycleQueueSize(int value)
	{
		base.Channel.SetRecycleQueueSize(value);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetRecycleQueueSize(int value, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetRecycleQueueSize(value, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetRecycleQueueSize(IAsyncResult result)
	{
		base.Channel.EndSetRecycleQueueSize(result);
	}

	private IAsyncResult OnBeginSetRecycleQueueSize(object[] inValues, AsyncCallback callback, object asyncState)
	{
		int value = (int)inValues[0];
		return BeginSetRecycleQueueSize(value, callback, asyncState);
	}

	private object[] OnEndSetRecycleQueueSize(IAsyncResult result)
	{
		EndSetRecycleQueueSize(result);
		return null;
	}

	private void OnSetRecycleQueueSizeCompleted(object state)
	{
		if (this.SetRecycleQueueSizeCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetRecycleQueueSizeCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetRecycleQueueSizeAsync(int value)
	{
		SetRecycleQueueSizeAsync(value, null);
	}

	public void SetRecycleQueueSizeAsync(int value, object userState)
	{
		if (onBeginSetRecycleQueueSizeDelegate == null)
		{
			onBeginSetRecycleQueueSizeDelegate = OnBeginSetRecycleQueueSize;
		}
		if (onEndSetRecycleQueueSizeDelegate == null)
		{
			onEndSetRecycleQueueSizeDelegate = OnEndSetRecycleQueueSize;
		}
		if (onSetRecycleQueueSizeCompletedDelegate == null)
		{
			onSetRecycleQueueSizeCompletedDelegate = OnSetRecycleQueueSizeCompleted;
		}
		InvokeAsync(onBeginSetRecycleQueueSizeDelegate, new object[1] { value }, onEndSetRecycleQueueSizeDelegate, onSetRecycleQueueSizeCompletedDelegate, userState);
	}
}
