using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Roblox.Grid.Rcc.ServerInterface;

[Serializable]
[GeneratedCode("wsdl", "4.8.3928.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://roblox.com/")]
public class ScriptExecution : INotifyPropertyChanged
{
	private string nameField;

	private string scriptField;

	private LuaValue[] argumentsField;

	public string name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
			RaisePropertyChanged("name");
		}
	}

	public string script
	{
		get
		{
			return scriptField;
		}
		set
		{
			scriptField = value;
			RaisePropertyChanged("script");
		}
	}

	public LuaValue[] arguments
	{
		get
		{
			return argumentsField;
		}
		set
		{
			argumentsField = value;
			RaisePropertyChanged("arguments");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
