using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace Roblox.Configuration;

public static class Utilities
{
	public static DateTime GetTimestamp()
	{
		return DateTime.UtcNow.AddHours(-7.0);
	}

	internal static void MergeSettings(ClientSettingsSection settings, ConnectionStringSettingsCollection connectionStrings, string groupName, string connectionString, SettingsPropertyCollection collection)
	{
		using ConfigurationDataClassesDataContext dataAccess = new ConfigurationDataClassesDataContext(connectionString);
		if (settings == null)
		{
			Log(EventLogEntryType.Information, "CopySettings: Did not find config for group {0}", groupName);
		}
		else
		{
			foreach (object obj in settings.Settings)
			{
				SettingElement setting = obj as SettingElement;
				if (setting == null)
				{
					continue;
				}
				if (setting.SerializeAs != 0)
				{
					Log(EventLogEntryType.Warning, "Property {0}.{1} cannot be saved because it serializes as {2}", groupName, setting.Name, setting.SerializeAs);
					continue;
				}
				Type type = (from p in collection.OfType<SettingsProperty>()
					where p.Name == setting.Name
					select p.PropertyType).SingleOrDefault();
				if (type == null)
				{
					Log(EventLogEntryType.Warning, "Property {0}.{1} is not a valid entry", groupName, setting.Name);
				}
				else if (dataAccess.Settings.Where((Setting c) => c.GroupName == groupName && c.Name == setting.Name).SingleOrDefault() == null)
				{
					Setting entity2 = new Setting
					{
						GroupName = groupName,
						Name = setting.Name,
						Type = type.ToString(),
						Value = setting.Value.ValueXml.InnerText,
						LastModified = GetTimestamp()
					};
					dataAccess.Settings.InsertOnSubmit(entity2);
				}
			}
		}
		foreach (object connectionString2 in connectionStrings)
		{
			if (connectionString2 is ConnectionStringSettings cs && cs.Name.StartsWith(groupName))
			{
				string name = cs.Name.Substring(groupName.Length + 1);
				ConnectionString entity = dataAccess.ConnectionStrings.Where((ConnectionString c) => c.GroupName == groupName && c.Name == name).SingleOrDefault();
				if (entity != null)
				{
					entity.Value = cs.ConnectionString;
					entity.LastModified = GetTimestamp();
					continue;
				}
				entity = new ConnectionString
				{
					GroupName = groupName,
					Name = name,
					Value = cs.ConnectionString,
					LastModified = GetTimestamp()
				};
				dataAccess.ConnectionStrings.InsertOnSubmit(entity);
			}
		}
		dataAccess.SubmitChanges();
	}

	internal static void Log(EventLogEntryType type, string format, params object[] args)
	{
		string message = string.Format(format, args);
		Console.WriteLine("EventLog - " + type.ToString() + " > " + message);
		EventLog.WriteEntry("Roblox.Configuration", message, type, 4983);
	}
}
