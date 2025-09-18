using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace Roblox.Grid.Rcc;

[GeneratedCode("wsdl", "4.8.3928.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[WebServiceBinding(Name = "RCCServiceSoap", Namespace = "http://roblox.com/")]
public class RCCServiceSoap : SoapHttpClientProtocol
{
	private SendOrPostCallback HelloWorldOperationCompleted;

	private SendOrPostCallback GetVersionOperationCompleted;

	private SendOrPostCallback GetStatusOperationCompleted;

	private SendOrPostCallback OpenJobOperationCompleted;

	private SendOrPostCallback OpenJobExOperationCompleted;

	private SendOrPostCallback RenewLeaseOperationCompleted;

	private SendOrPostCallback ExecuteOperationCompleted;

	private SendOrPostCallback ExecuteExOperationCompleted;

	private SendOrPostCallback CloseJobOperationCompleted;

	private SendOrPostCallback BatchJobOperationCompleted;

	private SendOrPostCallback BatchJobExOperationCompleted;

	private SendOrPostCallback GetExpirationOperationCompleted;

	private SendOrPostCallback GetAllJobsOperationCompleted;

	private SendOrPostCallback GetAllJobsExOperationCompleted;

	private SendOrPostCallback CloseExpiredJobsOperationCompleted;

	private SendOrPostCallback CloseAllJobsOperationCompleted;

	private SendOrPostCallback DiagOperationCompleted;

	private SendOrPostCallback DiagExOperationCompleted;

	public event HelloWorldCompletedEventHandler HelloWorldCompleted;

	public event GetVersionCompletedEventHandler GetVersionCompleted;

	public event GetStatusCompletedEventHandler GetStatusCompleted;

	public event OpenJobCompletedEventHandler OpenJobCompleted;

	public event OpenJobExCompletedEventHandler OpenJobExCompleted;

	public event RenewLeaseCompletedEventHandler RenewLeaseCompleted;

	public event ExecuteCompletedEventHandler ExecuteCompleted;

	public event ExecuteExCompletedEventHandler ExecuteExCompleted;

	public event CloseJobCompletedEventHandler CloseJobCompleted;

	public event BatchJobCompletedEventHandler BatchJobCompleted;

	public event BatchJobExCompletedEventHandler BatchJobExCompleted;

	public event GetExpirationCompletedEventHandler GetExpirationCompleted;

	public event GetAllJobsCompletedEventHandler GetAllJobsCompleted;

	public event GetAllJobsExCompletedEventHandler GetAllJobsExCompleted;

	public event CloseExpiredJobsCompletedEventHandler CloseExpiredJobsCompleted;

	public event CloseAllJobsCompletedEventHandler CloseAllJobsCompleted;

	public event DiagCompletedEventHandler DiagCompleted;

	public event DiagExCompletedEventHandler DiagExCompleted;

	[SoapDocumentMethod("http://roblox.com/HelloWorld", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string HelloWorld()
	{
		return (string)Invoke("HelloWorld", new object[0])[0];
	}

	public IAsyncResult BeginHelloWorld(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("HelloWorld", new object[0], callback, asyncState);
	}

	public string EndHelloWorld(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void HelloWorldAsync()
	{
		HelloWorldAsync(null);
	}

	public void HelloWorldAsync(object userState)
	{
		if (HelloWorldOperationCompleted == null)
		{
			HelloWorldOperationCompleted = OnHelloWorldOperationCompleted;
		}
		InvokeAsync("HelloWorld", new object[0], HelloWorldOperationCompleted, userState);
	}

	private void OnHelloWorldOperationCompleted(object arg)
	{
		if (this.HelloWorldCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/GetVersion", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string GetVersion()
	{
		return (string)Invoke("GetVersion", new object[0])[0];
	}

	public IAsyncResult BeginGetVersion(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetVersion", new object[0], callback, asyncState);
	}

	public string EndGetVersion(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void GetVersionAsync()
	{
		GetVersionAsync(null);
	}

	public void GetVersionAsync(object userState)
	{
		if (GetVersionOperationCompleted == null)
		{
			GetVersionOperationCompleted = OnGetVersionOperationCompleted;
		}
		InvokeAsync("GetVersion", new object[0], GetVersionOperationCompleted, userState);
	}

	private void OnGetVersionOperationCompleted(object arg)
	{
		if (this.GetVersionCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.GetVersionCompleted(this, new GetVersionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/GetStatus", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public Status GetStatus()
	{
		return (Status)Invoke("GetStatus", new object[0])[0];
	}

	public IAsyncResult BeginGetStatus(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetStatus", new object[0], callback, asyncState);
	}

	public Status EndGetStatus(IAsyncResult asyncResult)
	{
		return (Status)EndInvoke(asyncResult)[0];
	}

	public void GetStatusAsync()
	{
		GetStatusAsync(null);
	}

	public void GetStatusAsync(object userState)
	{
		if (GetStatusOperationCompleted == null)
		{
			GetStatusOperationCompleted = OnGetStatusOperationCompleted;
		}
		InvokeAsync("GetStatus", new object[0], GetStatusOperationCompleted, userState);
	}

	private void OnGetStatusOperationCompleted(object arg)
	{
		if (this.GetStatusCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.GetStatusCompleted(this, new GetStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/OpenJob", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement("OpenJobResult")]
	public LuaValue[] OpenJob(Job job, ScriptExecution script)
	{
		return (LuaValue[])Invoke("OpenJob", new object[2] { job, script })[0];
	}

	public IAsyncResult BeginOpenJob(Job job, ScriptExecution script, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("OpenJob", new object[2] { job, script }, callback, asyncState);
	}

	public LuaValue[] EndOpenJob(IAsyncResult asyncResult)
	{
		return (LuaValue[])EndInvoke(asyncResult)[0];
	}

	public void OpenJobAsync(Job job, ScriptExecution script)
	{
		OpenJobAsync(job, script, null);
	}

	public void OpenJobAsync(Job job, ScriptExecution script, object userState)
	{
		if (OpenJobOperationCompleted == null)
		{
			OpenJobOperationCompleted = OnOpenJobOperationCompleted;
		}
		InvokeAsync("OpenJob", new object[2] { job, script }, OpenJobOperationCompleted, userState);
	}

	private void OnOpenJobOperationCompleted(object arg)
	{
		if (this.OpenJobCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.OpenJobCompleted(this, new OpenJobCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/OpenJobEx", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public LuaValue[] OpenJobEx(Job job, ScriptExecution script)
	{
		return (LuaValue[])Invoke("OpenJobEx", new object[2] { job, script })[0];
	}

	public IAsyncResult BeginOpenJobEx(Job job, ScriptExecution script, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("OpenJobEx", new object[2] { job, script }, callback, asyncState);
	}

	public LuaValue[] EndOpenJobEx(IAsyncResult asyncResult)
	{
		return (LuaValue[])EndInvoke(asyncResult)[0];
	}

	public void OpenJobExAsync(Job job, ScriptExecution script)
	{
		OpenJobExAsync(job, script, null);
	}

	public void OpenJobExAsync(Job job, ScriptExecution script, object userState)
	{
		if (OpenJobExOperationCompleted == null)
		{
			OpenJobExOperationCompleted = OnOpenJobExOperationCompleted;
		}
		InvokeAsync("OpenJobEx", new object[2] { job, script }, OpenJobExOperationCompleted, userState);
	}

	private void OnOpenJobExOperationCompleted(object arg)
	{
		if (this.OpenJobExCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.OpenJobExCompleted(this, new OpenJobExCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/RenewLease", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public double RenewLease(string jobID, double expirationInSeconds)
	{
		return (double)Invoke("RenewLease", new object[2] { jobID, expirationInSeconds })[0];
	}

	public IAsyncResult BeginRenewLease(string jobID, double expirationInSeconds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("RenewLease", new object[2] { jobID, expirationInSeconds }, callback, asyncState);
	}

	public double EndRenewLease(IAsyncResult asyncResult)
	{
		return (double)EndInvoke(asyncResult)[0];
	}

	public void RenewLeaseAsync(string jobID, double expirationInSeconds)
	{
		RenewLeaseAsync(jobID, expirationInSeconds, null);
	}

	public void RenewLeaseAsync(string jobID, double expirationInSeconds, object userState)
	{
		if (RenewLeaseOperationCompleted == null)
		{
			RenewLeaseOperationCompleted = OnRenewLeaseOperationCompleted;
		}
		InvokeAsync("RenewLease", new object[2] { jobID, expirationInSeconds }, RenewLeaseOperationCompleted, userState);
	}

	private void OnRenewLeaseOperationCompleted(object arg)
	{
		if (this.RenewLeaseCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.RenewLeaseCompleted(this, new RenewLeaseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/Execute", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement("ExecuteResult", IsNullable = true)]
	public LuaValue[] Execute(string jobID, ScriptExecution script)
	{
		return (LuaValue[])Invoke("Execute", new object[2] { jobID, script })[0];
	}

	public IAsyncResult BeginExecute(string jobID, ScriptExecution script, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("Execute", new object[2] { jobID, script }, callback, asyncState);
	}

	public LuaValue[] EndExecute(IAsyncResult asyncResult)
	{
		return (LuaValue[])EndInvoke(asyncResult)[0];
	}

	public void ExecuteAsync(string jobID, ScriptExecution script)
	{
		ExecuteAsync(jobID, script, null);
	}

	public void ExecuteAsync(string jobID, ScriptExecution script, object userState)
	{
		if (ExecuteOperationCompleted == null)
		{
			ExecuteOperationCompleted = OnExecuteOperationCompleted;
		}
		InvokeAsync("Execute", new object[2] { jobID, script }, ExecuteOperationCompleted, userState);
	}

	private void OnExecuteOperationCompleted(object arg)
	{
		if (this.ExecuteCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.ExecuteCompleted(this, new ExecuteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/ExecuteEx", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public LuaValue[] ExecuteEx(string jobID, ScriptExecution script)
	{
		return (LuaValue[])Invoke("ExecuteEx", new object[2] { jobID, script })[0];
	}

	public IAsyncResult BeginExecuteEx(string jobID, ScriptExecution script, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("ExecuteEx", new object[2] { jobID, script }, callback, asyncState);
	}

	public LuaValue[] EndExecuteEx(IAsyncResult asyncResult)
	{
		return (LuaValue[])EndInvoke(asyncResult)[0];
	}

	public void ExecuteExAsync(string jobID, ScriptExecution script)
	{
		ExecuteExAsync(jobID, script, null);
	}

	public void ExecuteExAsync(string jobID, ScriptExecution script, object userState)
	{
		if (ExecuteExOperationCompleted == null)
		{
			ExecuteExOperationCompleted = OnExecuteExOperationCompleted;
		}
		InvokeAsync("ExecuteEx", new object[2] { jobID, script }, ExecuteExOperationCompleted, userState);
	}

	private void OnExecuteExOperationCompleted(object arg)
	{
		if (this.ExecuteExCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.ExecuteExCompleted(this, new ExecuteExCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/CloseJob", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CloseJob(string jobID)
	{
		Invoke("CloseJob", new object[1] { jobID });
	}

	public IAsyncResult BeginCloseJob(string jobID, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CloseJob", new object[1] { jobID }, callback, asyncState);
	}

	public void EndCloseJob(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CloseJobAsync(string jobID)
	{
		CloseJobAsync(jobID, null);
	}

	public void CloseJobAsync(string jobID, object userState)
	{
		if (CloseJobOperationCompleted == null)
		{
			CloseJobOperationCompleted = OnCloseJobOperationCompleted;
		}
		InvokeAsync("CloseJob", new object[1] { jobID }, CloseJobOperationCompleted, userState);
	}

	private void OnCloseJobOperationCompleted(object arg)
	{
		if (this.CloseJobCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.CloseJobCompleted(this, new AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/BatchJob", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement("BatchJobResult", IsNullable = true)]
	public LuaValue[] BatchJob(Job job, ScriptExecution script)
	{
		return (LuaValue[])Invoke("BatchJob", new object[2] { job, script })[0];
	}

	public IAsyncResult BeginBatchJob(Job job, ScriptExecution script, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("BatchJob", new object[2] { job, script }, callback, asyncState);
	}

	public LuaValue[] EndBatchJob(IAsyncResult asyncResult)
	{
		return (LuaValue[])EndInvoke(asyncResult)[0];
	}

	public void BatchJobAsync(Job job, ScriptExecution script)
	{
		BatchJobAsync(job, script, null);
	}

	public void BatchJobAsync(Job job, ScriptExecution script, object userState)
	{
		if (BatchJobOperationCompleted == null)
		{
			BatchJobOperationCompleted = OnBatchJobOperationCompleted;
		}
		InvokeAsync("BatchJob", new object[2] { job, script }, BatchJobOperationCompleted, userState);
	}

	private void OnBatchJobOperationCompleted(object arg)
	{
		if (this.BatchJobCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.BatchJobCompleted(this, new BatchJobCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/BatchJobEx", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public LuaValue[] BatchJobEx(Job job, ScriptExecution script)
	{
		return (LuaValue[])Invoke("BatchJobEx", new object[2] { job, script })[0];
	}

	public IAsyncResult BeginBatchJobEx(Job job, ScriptExecution script, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("BatchJobEx", new object[2] { job, script }, callback, asyncState);
	}

	public LuaValue[] EndBatchJobEx(IAsyncResult asyncResult)
	{
		return (LuaValue[])EndInvoke(asyncResult)[0];
	}

	public void BatchJobExAsync(Job job, ScriptExecution script)
	{
		BatchJobExAsync(job, script, null);
	}

	public void BatchJobExAsync(Job job, ScriptExecution script, object userState)
	{
		if (BatchJobExOperationCompleted == null)
		{
			BatchJobExOperationCompleted = OnBatchJobExOperationCompleted;
		}
		InvokeAsync("BatchJobEx", new object[2] { job, script }, BatchJobExOperationCompleted, userState);
	}

	private void OnBatchJobExOperationCompleted(object arg)
	{
		if (this.BatchJobExCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.BatchJobExCompleted(this, new BatchJobExCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/GetExpiration", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public double GetExpiration(string jobID)
	{
		return (double)Invoke("GetExpiration", new object[1] { jobID })[0];
	}

	public IAsyncResult BeginGetExpiration(string jobID, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetExpiration", new object[1] { jobID }, callback, asyncState);
	}

	public double EndGetExpiration(IAsyncResult asyncResult)
	{
		return (double)EndInvoke(asyncResult)[0];
	}

	public void GetExpirationAsync(string jobID)
	{
		GetExpirationAsync(jobID, null);
	}

	public void GetExpirationAsync(string jobID, object userState)
	{
		if (GetExpirationOperationCompleted == null)
		{
			GetExpirationOperationCompleted = OnGetExpirationOperationCompleted;
		}
		InvokeAsync("GetExpiration", new object[1] { jobID }, GetExpirationOperationCompleted, userState);
	}

	private void OnGetExpirationOperationCompleted(object arg)
	{
		if (this.GetExpirationCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.GetExpirationCompleted(this, new GetExpirationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/GetAllJobs", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement("GetAllJobsResult", IsNullable = true)]
	public Job[] GetAllJobs()
	{
		return (Job[])Invoke("GetAllJobs", new object[0])[0];
	}

	public IAsyncResult BeginGetAllJobs(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllJobs", new object[0], callback, asyncState);
	}

	public Job[] EndGetAllJobs(IAsyncResult asyncResult)
	{
		return (Job[])EndInvoke(asyncResult)[0];
	}

	public void GetAllJobsAsync()
	{
		GetAllJobsAsync(null);
	}

	public void GetAllJobsAsync(object userState)
	{
		if (GetAllJobsOperationCompleted == null)
		{
			GetAllJobsOperationCompleted = OnGetAllJobsOperationCompleted;
		}
		InvokeAsync("GetAllJobs", new object[0], GetAllJobsOperationCompleted, userState);
	}

	private void OnGetAllJobsOperationCompleted(object arg)
	{
		if (this.GetAllJobsCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.GetAllJobsCompleted(this, new GetAllJobsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/GetAllJobsEx", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public Job[] GetAllJobsEx()
	{
		return (Job[])Invoke("GetAllJobsEx", new object[0])[0];
	}

	public IAsyncResult BeginGetAllJobsEx(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllJobsEx", new object[0], callback, asyncState);
	}

	public Job[] EndGetAllJobsEx(IAsyncResult asyncResult)
	{
		return (Job[])EndInvoke(asyncResult)[0];
	}

	public void GetAllJobsExAsync()
	{
		GetAllJobsExAsync(null);
	}

	public void GetAllJobsExAsync(object userState)
	{
		if (GetAllJobsExOperationCompleted == null)
		{
			GetAllJobsExOperationCompleted = OnGetAllJobsExOperationCompleted;
		}
		InvokeAsync("GetAllJobsEx", new object[0], GetAllJobsExOperationCompleted, userState);
	}

	private void OnGetAllJobsExOperationCompleted(object arg)
	{
		if (this.GetAllJobsExCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.GetAllJobsExCompleted(this, new GetAllJobsExCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/CloseExpiredJobs", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int CloseExpiredJobs()
	{
		return (int)Invoke("CloseExpiredJobs", new object[0])[0];
	}

	public IAsyncResult BeginCloseExpiredJobs(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CloseExpiredJobs", new object[0], callback, asyncState);
	}

	public int EndCloseExpiredJobs(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void CloseExpiredJobsAsync()
	{
		CloseExpiredJobsAsync(null);
	}

	public void CloseExpiredJobsAsync(object userState)
	{
		if (CloseExpiredJobsOperationCompleted == null)
		{
			CloseExpiredJobsOperationCompleted = OnCloseExpiredJobsOperationCompleted;
		}
		InvokeAsync("CloseExpiredJobs", new object[0], CloseExpiredJobsOperationCompleted, userState);
	}

	private void OnCloseExpiredJobsOperationCompleted(object arg)
	{
		if (this.CloseExpiredJobsCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.CloseExpiredJobsCompleted(this, new CloseExpiredJobsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/CloseAllJobs", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int CloseAllJobs()
	{
		return (int)Invoke("CloseAllJobs", new object[0])[0];
	}

	public IAsyncResult BeginCloseAllJobs(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CloseAllJobs", new object[0], callback, asyncState);
	}

	public int EndCloseAllJobs(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void CloseAllJobsAsync()
	{
		CloseAllJobsAsync(null);
	}

	public void CloseAllJobsAsync(object userState)
	{
		if (CloseAllJobsOperationCompleted == null)
		{
			CloseAllJobsOperationCompleted = OnCloseAllJobsOperationCompleted;
		}
		InvokeAsync("CloseAllJobs", new object[0], CloseAllJobsOperationCompleted, userState);
	}

	private void OnCloseAllJobsOperationCompleted(object arg)
	{
		if (this.CloseAllJobsCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.CloseAllJobsCompleted(this, new CloseAllJobsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/Diag", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement("DiagResult", IsNullable = true)]
	public LuaValue[] Diag(int type, string jobID)
	{
		return (LuaValue[])Invoke("Diag", new object[2] { type, jobID })[0];
	}

	public IAsyncResult BeginDiag(int type, string jobID, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("Diag", new object[2] { type, jobID }, callback, asyncState);
	}

	public LuaValue[] EndDiag(IAsyncResult asyncResult)
	{
		return (LuaValue[])EndInvoke(asyncResult)[0];
	}

	public void DiagAsync(int type, string jobID)
	{
		DiagAsync(type, jobID, null);
	}

	public void DiagAsync(int type, string jobID, object userState)
	{
		if (DiagOperationCompleted == null)
		{
			DiagOperationCompleted = OnDiagOperationCompleted;
		}
		InvokeAsync("Diag", new object[2] { type, jobID }, DiagOperationCompleted, userState);
	}

	private void OnDiagOperationCompleted(object arg)
	{
		if (this.DiagCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.DiagCompleted(this, new DiagCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	[SoapDocumentMethod("http://roblox.com/DiagEx", RequestNamespace = "http://roblox.com/", ResponseNamespace = "http://roblox.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public LuaValue[] DiagEx(int type, string jobID)
	{
		return (LuaValue[])Invoke("DiagEx", new object[2] { type, jobID })[0];
	}

	public IAsyncResult BeginDiagEx(int type, string jobID, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DiagEx", new object[2] { type, jobID }, callback, asyncState);
	}

	public LuaValue[] EndDiagEx(IAsyncResult asyncResult)
	{
		return (LuaValue[])EndInvoke(asyncResult)[0];
	}

	public void DiagExAsync(int type, string jobID)
	{
		DiagExAsync(type, jobID, null);
	}

	public void DiagExAsync(int type, string jobID, object userState)
	{
		if (DiagExOperationCompleted == null)
		{
			DiagExOperationCompleted = OnDiagExOperationCompleted;
		}
		InvokeAsync("DiagEx", new object[2] { type, jobID }, DiagExOperationCompleted, userState);
	}

	private void OnDiagExOperationCompleted(object arg)
	{
		if (this.DiagExCompleted != null)
		{
			InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
			this.DiagExCompleted(this, new DiagExCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
		}
	}

	public new void CancelAsync(object userState)
	{
		base.CancelAsync(userState);
	}
}
