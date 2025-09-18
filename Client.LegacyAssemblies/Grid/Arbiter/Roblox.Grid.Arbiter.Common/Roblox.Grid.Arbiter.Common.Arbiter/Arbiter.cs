using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace Roblox.Grid.Arbiter.Common.Arbiter;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://roblox.com/", ConfigurationName = "Roblox.Grid.Arbiter.Common.Arbiter.Arbiter")]
public interface Arbiter
{
	[OperationContract(Action = "http://roblox.com/Arbiter/GetStatsEx", ReplyAction = "http://roblox.com/Arbiter/GetStatsExResponse")]
	string GetStatsEx(bool clearExceptions);

	[OperationContract(AsyncPattern = true, Action = "http://roblox.com/Arbiter/GetStatsEx", ReplyAction = "http://roblox.com/Arbiter/GetStatsExResponse")]
	IAsyncResult BeginGetStatsEx(bool clearExceptions, AsyncCallback callback, object asyncState);

	string EndGetStatsEx(IAsyncResult result);

	[OperationContract(Action = "http://roblox.com/Arbiter/GetStats", ReplyAction = "http://roblox.com/Arbiter/GetStatsResponse")]
	string GetStats();

	[OperationContract(AsyncPattern = true, Action = "http://roblox.com/Arbiter/GetStats", ReplyAction = "http://roblox.com/Arbiter/GetStatsResponse")]
	IAsyncResult BeginGetStats(AsyncCallback callback, object asyncState);

	string EndGetStats(IAsyncResult result);

	[OperationContract(Action = "http://roblox.com/Arbiter/SetMultiProcess", ReplyAction = "http://roblox.com/Arbiter/SetMultiProcessResponse")]
	void SetMultiProcess(bool value);

	[OperationContract(AsyncPattern = true, Action = "http://roblox.com/Arbiter/SetMultiProcess", ReplyAction = "http://roblox.com/Arbiter/SetMultiProcessResponse")]
	IAsyncResult BeginSetMultiProcess(bool value, AsyncCallback callback, object asyncState);

	void EndSetMultiProcess(IAsyncResult result);

	[OperationContract(Action = "http://roblox.com/Arbiter/SetRecycleProcess", ReplyAction = "http://roblox.com/Arbiter/SetRecycleProcessResponse")]
	void SetRecycleProcess(bool value);

	[OperationContract(AsyncPattern = true, Action = "http://roblox.com/Arbiter/SetRecycleProcess", ReplyAction = "http://roblox.com/Arbiter/SetRecycleProcessResponse")]
	IAsyncResult BeginSetRecycleProcess(bool value, AsyncCallback callback, object asyncState);

	void EndSetRecycleProcess(IAsyncResult result);

	[OperationContract(Action = "http://roblox.com/Arbiter/SetThreadConfigScript", ReplyAction = "http://roblox.com/Arbiter/SetThreadConfigScriptResponse")]
	void SetThreadConfigScript(string script, bool broadcast);

	[OperationContract(AsyncPattern = true, Action = "http://roblox.com/Arbiter/SetThreadConfigScript", ReplyAction = "http://roblox.com/Arbiter/SetThreadConfigScriptResponse")]
	IAsyncResult BeginSetThreadConfigScript(string script, bool broadcast, AsyncCallback callback, object asyncState);

	void EndSetThreadConfigScript(IAsyncResult result);

	[OperationContract(Action = "http://roblox.com/Arbiter/SetRecycleQueueSize", ReplyAction = "http://roblox.com/Arbiter/SetRecycleQueueSizeResponse")]
	void SetRecycleQueueSize(int value);

	[OperationContract(AsyncPattern = true, Action = "http://roblox.com/Arbiter/SetRecycleQueueSize", ReplyAction = "http://roblox.com/Arbiter/SetRecycleQueueSizeResponse")]
	IAsyncResult BeginSetRecycleQueueSize(int value, AsyncCallback callback, object asyncState);

	void EndSetRecycleQueueSize(IAsyncResult result);
}
