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
public class Job : INotifyPropertyChanged
{
	private string idField;

	private double expirationInSecondsField;

	private int categoryField;

	private double coresField;

	public string id
	{
		get
		{
			return idField;
		}
		set
		{
			idField = value;
			RaisePropertyChanged("id");
		}
	}

	public double expirationInSeconds
	{
		get
		{
			return expirationInSecondsField;
		}
		set
		{
			expirationInSecondsField = value;
			RaisePropertyChanged("expirationInSeconds");
		}
	}

	public int category
	{
		get
		{
			return categoryField;
		}
		set
		{
			categoryField = value;
			RaisePropertyChanged("category");
		}
	}

	public double cores
	{
		get
		{
			return coresField;
		}
		set
		{
			coresField = value;
			RaisePropertyChanged("cores");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
