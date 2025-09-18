using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace Roblox.Grid.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://roblox.com/", ConfigurationName = "Roblox.Grid.Client.RccService")]
public interface RccService
{
	[OperationContract(Action = "http://roblox.com/GetStatus", ReplyAction = "http://roblox.com/GetStatusResponse")]
	GetStatusResponse GetStatus(GetStatusRequest request);

	[OperationContract(AsyncPattern = true, Action = "http://roblox.com/GetStatus", ReplyAction = "http://roblox.com/GetStatusResponse")]
	IAsyncResult BeginGetStatus(GetStatusRequest request, AsyncCallback callback, object asyncState);

	GetStatusResponse EndGetStatus(IAsyncResult result);
}
