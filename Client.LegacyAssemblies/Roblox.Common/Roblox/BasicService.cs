using System;
using System.Collections;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace Roblox;

public class BasicService : ServiceBase
{
	public void Process(string[] args, Action statsTask)
	{
		if (args.Length != 0)
		{
			try
			{
				switch (args[0].Substring(1).ToLower())
				{
				case "console":
				{
					ConsoleKey exitKey = ConsoleKey.Escape;
					ConsoleKey garbageCollectionKey = ConsoleKey.G;
					Console.WriteLine("Starting {0}...", GetType());
					OnStart(args);
					Console.WriteLine("Service started. Press any key to {0}.", (statsTask == null) ? "exit" : "get stats");
					Console.WriteLine("Press {0} to force a full Garbage Collection cycle", garbageCollectionKey);
					Console.WriteLine("Press {0} to exit process", exitKey);
					while (true)
					{
						ConsoleKey key = Console.ReadKey(intercept: true).Key;
						if (key == exitKey)
						{
							break;
						}
						if (key == garbageCollectionKey)
						{
							Console.Write("Initiating GC cycle ....");
							GC.Collect(3, GCCollectionMode.Forced);
							Console.WriteLine("done");
						}
						else
						{
							if (statsTask == null)
							{
								break;
							}
							statsTask();
						}
					}
					Console.WriteLine("Stopping Service.");
					OnStop();
					break;
				}
				case "install":
				{
					Console.WriteLine("Installing...");
					AssemblyInstaller installer2 = new AssemblyInstaller(Assembly.GetEntryAssembly(), new string[0]);
					installer2.UseNewContext = true;
					IDictionary savedState = new Hashtable();
					installer2.Install(savedState);
					installer2.Commit(savedState);
					Console.WriteLine("Service Installed");
					break;
				}
				case "uninstall":
				{
					Console.WriteLine("Uninstalling...");
					AssemblyInstaller installer = new AssemblyInstaller(Assembly.GetEntryAssembly(), new string[0]);
					installer.UseNewContext = true;
					IDictionary savedState2 = new Hashtable();
					installer.Uninstall(savedState2);
					Console.WriteLine("Service Uninstalled");
					break;
				}
				default:
					throw new ApplicationException("Bad argument " + args[0]);
				}
				return;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				return;
			}
		}
		ServiceBase.Run(this);
	}
}
