using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace Roblox.Grid.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class GetStatusRequest
{
	[MessageBodyMember(Name = "GetStatus", Namespace = "http://roblox.com/", Order = 0)]
	public GetStatusRequestBody Body;

	public GetStatusRequest()
	{
	}

	public GetStatusRequest(GetStatusRequestBody Body)
	{
		this.Body = Body;
	}
}
