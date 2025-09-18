using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;

namespace Roblox.Configuration;

public class Provider : SettingsProvider
{
	private SettingsProvider alternateSettings;

	private SelfDisposingTimer timer;

	private ApplicationSettingsBase applicationSettings;

	private int databaseHash;

	private string connectionString;

	private string applicationName;

	private string groupName;

	public override string ApplicationName
	{
		get
		{
			return applicationName;
		}
		set
		{
			applicationName = value;
		}
	}

	public override string Description => "Provides settings with a database, auto-update, etc.";

	public static string GetDefaultConnectionString()
	{
		return (ConfigurationManager.GetSection("robloxConfigurationProvider") as ProviderConfigSection).GroupConfigs["*"].ConnectionString;
	}

	public override void Initialize(string name, NameValueCollection config)
	{
		if (string.IsNullOrEmpty(name))
		{
			name = GetType().FullName;
		}
		base.Initialize(name, config);
	}

	private void InitializeConfig(SettingsContext context)
	{
		if (!string.IsNullOrEmpty(connectionString) && alternateSettings != null)
		{
			return;
		}
		try
		{
			groupName = (string)context["GroupName"];
		}
		catch (Exception inner)
		{
			throw new ConfigurationErrorsException("Invalid or missing GroupName", inner);
		}
		GroupConfigElement groupConfiguration = GetGroupConfiguration(ConfigurationManager.GetSection("robloxConfigurationProvider") as ProviderConfigSection, groupName);
		if (groupConfiguration != null && !string.IsNullOrEmpty(groupConfiguration.ConnectionString))
		{
			connectionString = groupConfiguration.ConnectionString;
			Utilities.Log(EventLogEntryType.Information, "Group {0} source is {1}", groupName, connectionString);
			if (groupConfiguration.UpdateInterval != TimeSpan.Zero)
			{
				Utilities.Log(EventLogEntryType.Information, "Polling {0} for updates every {1}", groupName, groupConfiguration.UpdateInterval);
				timer = new SelfDisposingTimer(CheckSettings, groupConfiguration.UpdateInterval, groupConfiguration.UpdateInterval);
			}
		}
		else
		{
			Utilities.Log(EventLogEntryType.Information, "Group {0} is using file-based configuration", groupName);
			alternateSettings = new LocalFileSettingsProvider();
			alternateSettings.Initialize(null, null);
		}
	}

	internal static GroupConfigElement GetGroupConfiguration(ProviderConfigSection section, string groupName)
	{
		if (section != null)
		{
			GroupConfigElement element = section.GroupConfigs[groupName];
			if (element == null)
			{
				return section.GroupConfigs["*"];
			}
			return element;
		}
		return null;
	}

	public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
	{
		InitializeConfig(context);
		if (alternateSettings != null)
		{
			return alternateSettings.GetPropertyValues(context, collection);
		}
		Dictionary<string, SettingsPropertyValue> properties = new Dictionary<string, SettingsPropertyValue>();
		SettingsPropertyValueCollection settingsCollection = new SettingsPropertyValueCollection();
		foreach (object item in collection)
		{
			if (item is SettingsProperty setting2)
			{
				SettingsPropertyValue value3 = new SettingsPropertyValue(setting2);
				settingsCollection.Add(value3);
				properties[setting2.Name] = value3;
			}
		}
		if (string.IsNullOrEmpty(this.connectionString))
		{
			return settingsCollection;
		}
		PopulateDatabase(collection);
		using (ConfigurationDataClassesDataContext db = new ConfigurationDataClassesDataContext(this.connectionString))
		{
			IOrderedQueryable<Setting> settingQuery = from p in db.Settings
				where p.GroupName == groupName
				orderby p.ID
				select p;
			foreach (Setting setting in settingQuery)
			{
				if (!properties.TryGetValue(setting.Name, out var value2))
				{
					Utilities.Log(EventLogEntryType.Warning, "Unknown setting from database: {0}:{1}", groupName, setting.Name);
				}
				else
				{
					value2.SerializedValue = setting.Value;
				}
			}
			IOrderedQueryable<ConnectionString> connectionStringQuery = from p in db.ConnectionStrings
				where p.GroupName == groupName
				orderby p.ID
				select p;
			foreach (ConnectionString connectionString in connectionStringQuery)
			{
				if (!properties.TryGetValue(connectionString.Name, out var value))
				{
					Utilities.Log(EventLogEntryType.Warning, "Unknown connectionString from database: {0}:{1}", groupName, connectionString.Name);
				}
				else
				{
					value.SerializedValue = connectionString.Value;
				}
			}
			databaseHash = GetHash(settingQuery, connectionStringQuery);
		}
		if (timer != null)
		{
			timer.Unpause();
		}
		return settingsCollection;
	}

	private void PopulateDatabase(SettingsPropertyCollection collection)
	{
		using ConfigurationDataClassesDataContext db = new ConfigurationDataClassesDataContext(connectionString);
		if ((from row in db.PopulatedGroups
			where row.GroupName == groupName
			select true).Count() == 0)
		{
			Utilities.Log(EventLogEntryType.Information, "populating Configuration database with group {0}", groupName);
			Utilities.MergeSettings(ConfigurationManager.GetSection("applicationSettings/" + groupName) as ClientSettingsSection, ConfigurationManager.ConnectionStrings, groupName, connectionString, collection);
			db.PopulatedGroups.InsertOnSubmit(new PopulatedGroup
			{
				GroupName = groupName
			});
			db.SubmitChanges();
		}
	}

	private static int GetHash(IEnumerable<Setting> orderedSettings, IEnumerable<ConnectionString> orderedConnectionStrings)
	{
		int hash = 0;
		foreach (Setting setting in orderedSettings)
		{
			hash ^= setting.Name.GetHashCode();
			hash ^= setting.Value.GetHashCode();
		}
		foreach (ConnectionString connectionString in orderedConnectionStrings)
		{
			hash ^= connectionString.Name.GetHashCode();
			hash ^= connectionString.Value.GetHashCode();
		}
		return hash;
	}

	public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
	{
		if (alternateSettings != null)
		{
			alternateSettings.SetPropertyValues(context, collection);
			return;
		}
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new ConfigurationErrorsException("Database connection string is empty");
		}
		if (!(ConfigurationManager.GetSection("robloxConfigurationProvider") is ProviderConfigSection))
		{
			Utilities.Log(EventLogEntryType.Warning, "Missing robloxConfigurationProvider section group in the configuration");
			return;
		}
		using ConfigurationDataClassesDataContext db = new ConfigurationDataClassesDataContext(connectionString);
		foreach (object item in collection)
		{
			if (item is SettingsPropertyValue value)
			{
				if (value.Property.SerializeAs != 0)
				{
					Utilities.Log(EventLogEntryType.Warning, "Property {0}.{1} cannot be saveed because it serializes as {2}", groupName, value.Name, value.Property.SerializeAs);
				}
				else if (IsConnectionString(value))
				{
					SaveConnectionString(db.ConnectionStrings, value);
				}
				else
				{
					SaveProperty(db.Settings, value);
				}
			}
		}
		db.SubmitChanges();
		databaseHash = GetHash(db);
	}

	private void SaveProperty(Table<Setting> settings, SettingsPropertyValue prop)
	{
		Setting setting = settings.Where((Setting c) => c.GroupName == groupName && c.Name == prop.Name).SingleOrDefault();
		if (setting != null)
		{
			setting.Value = (string)prop.SerializedValue;
			setting.Type = prop.Property.PropertyType.FullName;
			setting.LastModified = Utilities.GetTimestamp();
		}
		else if (!prop.UsingDefaultValue)
		{
			Setting entity = new Setting
			{
				GroupName = groupName,
				Name = prop.Name,
				Type = prop.Property.PropertyType.FullName,
				Value = (string)prop.SerializedValue,
				LastModified = Utilities.GetTimestamp()
			};
			settings.InsertOnSubmit(entity);
		}
	}

	private void SaveConnectionString(Table<ConnectionString> connectionStrings, SettingsPropertyValue prop)
	{
		ConnectionString connectionString = connectionStrings.Where((ConnectionString c) => c.GroupName == groupName && c.Name == prop.Name).SingleOrDefault();
		if (connectionString != null)
		{
			connectionString.Value = (string)prop.SerializedValue;
			connectionString.LastModified = Utilities.GetTimestamp();
		}
		else if (!prop.UsingDefaultValue)
		{
			ConnectionString entity = new ConnectionString
			{
				GroupName = groupName,
				Name = prop.Name,
				Value = (string)prop.SerializedValue,
				LastModified = Utilities.GetTimestamp()
			};
			connectionStrings.InsertOnSubmit(entity);
		}
	}

	private int GetHash(ConfigurationDataClassesDataContext db)
	{
		return GetHash(from p in db.Settings
			where p.GroupName == groupName
			orderby p.ID
			select p, from p in db.ConnectionStrings
			where p.GroupName == groupName
			orderby p.ID
			select p);
	}

	private bool IsConnectionString(SettingsPropertyValue prop)
	{
		return ConfigurationManager.ConnectionStrings[groupName + "." + prop.Name] != null;
	}

	public static void RegisterSettings(SettingsLoadedEventArgs e, ApplicationSettingsBase applicationSettings)
	{
		if (e.Provider is Provider provider && provider.applicationSettings != applicationSettings)
		{
			if (provider.applicationSettings != null)
			{
				throw new InvalidOperationException("RegisterSettings changing applicationSettings");
			}
			provider.applicationSettings = applicationSettings;
		}
	}

	private void CheckSettings()
	{
		try
		{
			if (applicationSettings == null)
			{
				Utilities.Log(EventLogEntryType.Warning, "You haven't called RegisterSettings in {0}.OnSettingsLoaded. Cannot reload settings in the event of a change", groupName);
				return;
			}
			int hash;
			using (ConfigurationDataClassesDataContext db = new ConfigurationDataClassesDataContext(connectionString))
			{
				hash = GetHash(db);
			}
			if (hash != databaseHash)
			{
				timer.Pause();
				Utilities.Log(EventLogEntryType.Information, "Configuration {0} has changed. Reloading settings", groupName);
				applicationSettings.Reload();
			}
		}
		catch (Exception ex)
		{
			Utilities.Log(EventLogEntryType.Error, ex.ToString());
		}
	}
}
