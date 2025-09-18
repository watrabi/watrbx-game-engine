using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Roblox.Grid.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Status", Namespace = "http://roblox.com/")]
public class Status : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string versionField;

	private int environmentCountField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember(EmitDefaultValue = false)]
	public string version
	{
		get
		{
			return versionField;
		}
		set
		{
			versionField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 1)]
	public int environmentCount
	{
		get
		{
			return environmentCountField;
		}
		set
		{
			environmentCountField = value;
		}
	}
}
