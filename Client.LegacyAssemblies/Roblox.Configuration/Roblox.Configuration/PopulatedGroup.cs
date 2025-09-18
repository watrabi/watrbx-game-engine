using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace Roblox.Configuration;

[Table(Name = "dbo.PopulatedGroups")]
public class PopulatedGroup : INotifyPropertyChanging, INotifyPropertyChanged
{
	private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(string.Empty);

	private int _ID;

	private string _GroupName;

	[Column(Storage = "_ID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
	public int ID
	{
		get
		{
			return _ID;
		}
		set
		{
			if (_ID != value)
			{
				SendPropertyChanging();
				_ID = value;
				SendPropertyChanged("ID");
			}
		}
	}

	[Column(Storage = "_GroupName", DbType = "NVarChar(MAX) NOT NULL", CanBeNull = false)]
	public string GroupName
	{
		get
		{
			return _GroupName;
		}
		set
		{
			if (_GroupName != value)
			{
				SendPropertyChanging();
				_GroupName = value;
				SendPropertyChanged("GroupName");
			}
		}
	}

	public event PropertyChangingEventHandler PropertyChanging;

	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual void SendPropertyChanging()
	{
		this.PropertyChanging?.Invoke(this, emptyChangingEventArgs);
	}

	protected virtual void SendPropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
