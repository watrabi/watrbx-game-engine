using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Roblox.Grid.Rcc;

[Serializable]
[GeneratedCode("wsdl", "4.8.3928.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://roblox.com/")]
public class Status : INotifyPropertyChanged
{
	private string versionField;

	private int environmentCountField;

	public string version
	{
		get
		{
			return versionField;
		}
		set
		{
			versionField = value;
			RaisePropertyChanged("version");
		}
	}

	public int environmentCount
	{
		get
		{
			return environmentCountField;
		}
		set
		{
			environmentCountField = value;
			RaisePropertyChanged("environmentCount");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
