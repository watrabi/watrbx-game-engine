using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;

namespace Roblox.Grid.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class RccServiceClient : ClientBase<RccService>, RccService
{
	private BeginOperationDelegate onBeginGetStatusDelegate;

	private EndOperationDelegate onEndGetStatusDelegate;

	private SendOrPostCallback onGetStatusCompletedDelegate;

	public event EventHandler<GetStatusCompletedEventArgs> GetStatusCompleted;

	public RccServiceClient()
	{
	}

	public RccServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public RccServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public RccServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public RccServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	GetStatusResponse RccService.GetStatus(GetStatusRequest request)
	{
		return base.Channel.GetStatus(request);
	}

	public Status GetStatus()
	{
		GetStatusRequest inValue = new GetStatusRequest();
		inValue.Body = new GetStatusRequestBody();
		return ((RccService)this).GetStatus(inValue).Body.GetStatusResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	IAsyncResult RccService.BeginGetStatus(GetStatusRequest request, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetStatus(request, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetStatus(AsyncCallback callback, object asyncState)
	{
		GetStatusRequest inValue = new GetStatusRequest();
		inValue.Body = new GetStatusRequestBody();
		return ((RccService)this).BeginGetStatus(inValue, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	GetStatusResponse RccService.EndGetStatus(IAsyncResult result)
	{
		return base.Channel.EndGetStatus(result);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Status EndGetStatus(IAsyncResult result)
	{
		return ((RccService)this).EndGetStatus(result).Body.GetStatusResult;
	}

	private IAsyncResult OnBeginGetStatus(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetStatus(callback, asyncState);
	}

	private object[] OnEndGetStatus(IAsyncResult result)
	{
		Status retVal = EndGetStatus(result);
		return new object[1] { retVal };
	}

	private void OnGetStatusCompleted(object state)
	{
		if (this.GetStatusCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetStatusCompleted(this, new GetStatusCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetStatusAsync()
	{
		GetStatusAsync(null);
	}

	public void GetStatusAsync(object userState)
	{
		if (onBeginGetStatusDelegate == null)
		{
			onBeginGetStatusDelegate = OnBeginGetStatus;
		}
		if (onEndGetStatusDelegate == null)
		{
			onEndGetStatusDelegate = OnEndGetStatus;
		}
		if (onGetStatusCompletedDelegate == null)
		{
			onGetStatusCompletedDelegate = OnGetStatusCompleted;
		}
		InvokeAsync(onBeginGetStatusDelegate, null, onEndGetStatusDelegate, onGetStatusCompletedDelegate, userState);
	}
}
