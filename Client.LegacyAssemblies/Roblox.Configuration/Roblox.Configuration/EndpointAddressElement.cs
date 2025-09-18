using System.Configuration;

namespace Roblox.Configuration;

public class EndpointAddressElement : ConfigurationElement
{
	private static readonly ConfigurationPropertyCollection properties;

	private static readonly ConfigurationProperty nameProperty;

	private static readonly ConfigurationProperty connectionStringProperty;

	public string EndpointConfigurationName
	{
		get
		{
			return (string)base[nameProperty];
		}
		set
		{
			base[nameProperty] = value;
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

	protected override ConfigurationPropertyCollection Properties => properties;

	static EndpointAddressElement()
	{
		properties = new ConfigurationPropertyCollection();
		nameProperty = new ConfigurationProperty("endpointConfigurationName", typeof(string), null, ConfigurationPropertyOptions.IsRequired);
		properties.Add(nameProperty);
		connectionStringProperty = new ConfigurationProperty("connectionString", typeof(string), null, ConfigurationPropertyOptions.IsRequired);
		properties.Add(connectionStringProperty);
	}
}
