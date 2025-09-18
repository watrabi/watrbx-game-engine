using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Roblox.Grid.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
[DataContract(Namespace = "http://roblox.com/")]
public class GetStatusResponseBody
{
	[DataMember(EmitDefaultValue = false, Order = 0)]
	public Status GetStatusResult;

	public GetStatusResponseBody()
	{
	}

	public GetStatusResponseBody(Status GetStatusResult)
	{
		this.GetStatusResult = GetStatusResult;
	}
}
