using System;
using System.Configuration;

namespace Roblox.Configuration;

public class GroupConfigElement : ConfigurationElement
{
	private static readonly ConfigurationPropertyCollection properties;

	private static readonly ConfigurationProperty groupNameProperty;

	private static readonly ConfigurationProperty connectionStringProperty;

	private static readonly ConfigurationProperty updateIntervalProperty;

	public string GroupName
	{
		get
		{
			return (string)base[groupNameProperty];
		}
		set
		{
			base[groupNameProperty] = value;
		}
	}

	public string ConnectionString
	{
		get
		{
			return (string)base[connectionStringProperty];
		}
		set
		{
			base[connectionStringProperty] = value;
		}
	}

	public TimeSpan UpdateInterval
	{
		get
		{
			return (TimeSpan)base[updateIntervalProperty];
		}
		set
		{
			base[updateIntervalProperty] = value;
		}
	}

	protected override ConfigurationPropertyCollection Properties => properties;

	static GroupConfigElement()
	{
		properties = new ConfigurationPropertyCollection();
		groupNameProperty = new ConfigurationProperty("groupName", typeof(string), null, ConfigurationPropertyOptions.IsRequired);
		properties.Add(groupNameProperty);
		connectionStringProperty = new ConfigurationProperty("connectionString", typeof(string), null, ConfigurationPropertyOptions.None);
		properties.Add(connectionStringProperty);
		updateIntervalProperty = new ConfigurationProperty("updateInterval", typeof(TimeSpan), TimeSpan.Zero, ConfigurationPropertyOptions.None);
		properties.Add(updateIntervalProperty);
	}
}
