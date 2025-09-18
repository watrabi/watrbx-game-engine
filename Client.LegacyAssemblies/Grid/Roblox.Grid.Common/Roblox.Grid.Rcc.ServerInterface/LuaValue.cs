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
public class LuaValue : INotifyPropertyChanged
{
	private LuaType typeField;

	private string valueField;

	private LuaValue[] tableField;

	public LuaType type
	{
		get
		{
			return typeField;
		}
		set
		{
			typeField = value;
			RaisePropertyChanged("type");
		}
	}

	public string value
	{
		get
		{
			return valueField;
		}
		set
		{
			valueField = value;
			RaisePropertyChanged("value");
		}
	}

	public LuaValue[] table
	{
		get
		{
			return tableField;
		}
		set
		{
			tableField = value;
			RaisePropertyChanged("table");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
