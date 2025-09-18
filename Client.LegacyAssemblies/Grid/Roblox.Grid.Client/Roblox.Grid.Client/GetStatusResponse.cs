using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace Roblox.Grid.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[MessageContract(IsWrapped = false)]
public class GetStatusResponse
{
	[MessageBodyMember(Name = "GetStatusResponse", Namespace = "http://roblox.com/", Order = 0)]
	public GetStatusResponseBody Body;

	public GetStatusResponse()
	{
	}

	public GetStatusResponse(GetStatusResponseBody Body)
	{
		this.Body = Body;
	}
}
