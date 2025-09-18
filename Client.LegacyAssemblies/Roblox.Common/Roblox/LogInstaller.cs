using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

namespace Roblox;

[RunInstaller(true)]
public class LogInstaller : EventLogInstaller
{
	public const string LogName = "Roblox";

	public const string SourceName = "Web Server";

	public LogInstaller()
	{
		base.Source = "Web Server";
		base.Log = "Roblox";
	}

	public override void Install(IDictionary stateSaver)
	{
		base.Install(stateSaver);
		EventLog log = new EventLog("Roblox");
		log.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 5);
		log.MaximumKilobytes = 16000L;
	}
}
