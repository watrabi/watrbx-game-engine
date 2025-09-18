using System.Configuration;

namespace Roblox.Configuration;

public class ProviderConfigSection : ConfigurationSection
{
	private static readonly ConfigurationPropertyCollection _Properties;

	private static readonly ConfigurationProperty isDatabaseWritableProperty;

	private static readonly ConfigurationProperty groupConfigs;

	private static readonly ConfigurationProperty endpointAddressConfigs;

	public bool IsDatabaseReadonly
	{
		get
		{
			return !(bool)base[isDatabaseWritableProperty];
		}
		set
		{
			base[isDatabaseWritableProperty] = !value;
		}
	}

	public GroupConfigElements GroupConfigs => (GroupConfigElements)base[groupConfigs];

	public EndpointAddressElements EndpointAddressConfigs => (EndpointAddressElements)base[endpointAddressConfigs];

	protected override ConfigurationPropertyCollection Properties => _Properties;

	static ProviderConfigSection()
	{
		_Properties = new ConfigurationPropertyCollection();
		isDatabaseWritableProperty = new ConfigurationProperty("isDatabaseWritable", typeof(bool), false, ConfigurationPropertyOptions.None);
		_Properties.Add(isDatabaseWritableProperty);
		groupConfigs = new ConfigurationProperty("groups", typeof(GroupConfigElements), null, ConfigurationPropertyOptions.IsRequired);
		_Properties.Add(groupConfigs);
		endpointAddressConfigs = new ConfigurationProperty("endpointAddresses", typeof(EndpointAddressElements), null, ConfigurationPropertyOptions.None);
		_Properties.Add(endpointAddressConfigs);
	}
}
