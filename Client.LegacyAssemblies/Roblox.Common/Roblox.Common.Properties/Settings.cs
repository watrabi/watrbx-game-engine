using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Roblox.Configuration;

namespace Roblox.Common.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.2.0.0")]
[SettingsProvider(typeof(Provider))]
public sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("localhost")]
	public string SMTPServer => (string)this["SMTPServer"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("http://localhost:3966/RobloxWebSite")]
	public string ApplicationURL => (string)this["ApplicationURL"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("25")]
	public int dbHelper_DefaultPoolSize => (int)this["dbHelper_DefaultPoolSize"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool ReplicateLocalCache => (bool)this["ReplicateLocalCache"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("00:00:00.05")]
	public string CacheReplicatorWaitTime => (string)this["CacheReplicatorWaitTime"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("224.168.100.2")]
	public string CacheReplicatorAddress => (string)this["CacheReplicatorAddress"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("11000")]
	public int CacheReplicatorPort => (int)this["CacheReplicatorPort"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("60000")]
	public int CacheReplicatorBatchSize => (int)this["CacheReplicatorBatchSize"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("00:00:00.1")]
	public string CacheReplicatorSendtimeout => (string)this["CacheReplicatorSendtimeout"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("192.168.0")]
	public string CacheReplicatorNicPrefix => (string)this["CacheReplicatorNicPrefix"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("100")]
	public uint PgmWindowSizeinMB => (uint)this["PgmWindowSizeinMB"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("00:00:10")]
	public TimeSpan PgmWindowSizeTimeSpan => (TimeSpan)this["PgmWindowSizeTimeSpan"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int CcrServiceBacklogTrigger => (int)this["CcrServiceBacklogTrigger"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("00:00:03")]
	public TimeSpan CcrServiceBacklogTriggerInterval => (TimeSpan)this["CcrServiceBacklogTriggerInterval"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool PreventDirectAppServerAccess => (bool)this["PreventDirectAppServerAccess"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("300")]
	public int ClientPresenceUpdateIntervalInSeconds => (int)this["ClientPresenceUpdateIntervalInSeconds"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("!0x5f3759df")]
	public string AccessKey => (string)this["AccessKey"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[SpecialSetting(SpecialSetting.ConnectionString)]
	[DefaultSettingValue("Data Source=192.168.100.80;Initial Catalog=RobloxBilling;User ID=Roblox;Password=To0Big2F@il")]
	public string BillingConnectionString => (string)this["BillingConnectionString"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("01:00:00")]
	public string CacheSlidingExpiration => (string)this["CacheSlidingExpiration"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public string FacebookUploadEnabled => (string)this["FacebookUploadEnabled"];

	protected override void OnSettingsLoaded(object sender, SettingsLoadedEventArgs e)
	{
		base.OnSettingsLoaded(sender, e);
		Provider.RegisterSettings(e, this);
	}
}
