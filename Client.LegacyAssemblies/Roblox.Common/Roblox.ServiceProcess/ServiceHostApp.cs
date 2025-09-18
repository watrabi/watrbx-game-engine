using System;
using System.Collections;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceModel;
using System.ServiceProcess;

namespace Roblox.ServiceProcess;

public class ServiceHostApp<TServiceClass> : ServiceBasePublic where TServiceClass : class
{
	private ServiceHost serviceHost;

	private TServiceClass singleton;

	private ServiceBasePublic[] otherSingletons;

	public event EventHandler HostClosed;

	public event EventHandler HostClosing;

	public event EventHandler HostFaulted;

	public event EventHandler HostOpened;

	public event EventHandler HostOpening;

	public ServiceHostApp(TServiceClass singleton)
	{
		this.singleton = singleton;
	}

	public ServiceHostApp(TServiceClass singleton, ServiceBasePublic[] others)
		: this(singleton)
	{
		otherSingletons = others;
	}

	public ServiceHostApp()
	{
	}

	protected override void OnStart(string[] args)
	{
		CloseServiceHost();
		if (singleton != null)
		{
			serviceHost = new ServiceHost(singleton);
		}
		else
		{
			serviceHost = new ServiceHost(typeof(TServiceClass));
		}
		if (serviceHost.SingletonInstance is IArgumentService)
		{
			(serviceHost.SingletonInstance as IArgumentService).ProcessArgs(args);
		}
		serviceHost.Closed += serviceHost_Closed;
		serviceHost.Closing += serviceHost_Closing;
		serviceHost.Faulted += serviceHost_Faulted;
		serviceHost.Opened += serviceHost_Opened;
		serviceHost.Opening += serviceHost_Opening;
		serviceHost.Open();
		if (otherSingletons != null)
		{
			ServiceBasePublic[] array = otherSingletons;
			foreach (ServiceBasePublic sbp in array)
			{
				sbp.OnStartPublic(args);
			}
		}
	}

	private void CloseServiceHost()
	{
		if (serviceHost != null)
		{
			if (serviceHost.State != CommunicationState.Closed)
			{
				serviceHost.Close();
			}
			serviceHost.Closed -= serviceHost_Closed;
			serviceHost.Closing -= serviceHost_Closing;
			serviceHost.Faulted -= serviceHost_Faulted;
			serviceHost.Opened -= serviceHost_Opened;
			serviceHost.Opening -= serviceHost_Opening;
			serviceHost = null;
		}
	}

	private void serviceHost_Closed(object sender, EventArgs e)
	{
		if (this.HostClosed != null)
		{
			this.HostClosed(sender, e);
		}
	}

	private void serviceHost_Closing(object sender, EventArgs e)
	{
		if (this.HostClosing != null)
		{
			this.HostClosing(sender, e);
		}
	}

	private void serviceHost_Faulted(object sender, EventArgs e)
	{
		if (this.HostFaulted != null)
		{
			this.HostFaulted(sender, e);
		}
	}

	private void serviceHost_Opened(object sender, EventArgs e)
	{
		if (this.HostOpened != null)
		{
			this.HostOpened(sender, e);
		}
	}

	private void serviceHost_Opening(object sender, EventArgs e)
	{
		if (this.HostOpening != null)
		{
			this.HostOpening(sender, e);
		}
	}

	protected override void OnStop()
	{
		CloseServiceHost();
		if (otherSingletons != null)
		{
			ServiceBasePublic[] array = otherSingletons;
			foreach (ServiceBasePublic sbp in array)
			{
				sbp.OnStopPublic();
			}
		}
	}

	public void Process(string[] args)
	{
		Process(args, null);
	}

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
					ConsoleKey closeSocketsKey = ConsoleKey.Q;
					ConsoleKey exitKey = ConsoleKey.Escape;
					ConsoleKey garbageCollectionKey = ConsoleKey.G;
					Console.WriteLine("Starting {0}...", typeof(TServiceClass));
					OnStart(args);
					Console.WriteLine("Service started. Press any key to {0}.", (statsTask == null) ? "exit" : "get stats");
					Console.WriteLine("Press {0} to force a full Garbage Collection cycle", garbageCollectionKey);
					Console.WriteLine("Press {0} to close sockets or {1} to exit process", closeSocketsKey, exitKey);
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
						else if (key == closeSocketsKey)
						{
							Console.Write("Closing sockets....");
							CloseServiceHost();
							Console.WriteLine(" done");
							Console.WriteLine("Press {0} to exit process", exitKey);
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
					IDictionary savedState2 = new Hashtable();
					installer2.Install(savedState2);
					installer2.Commit(savedState2);
					Console.WriteLine("Service Installed");
					break;
				}
				case "uninstall":
				{
					Console.WriteLine("Uninstalling...");
					AssemblyInstaller installer = new AssemblyInstaller(Assembly.GetEntryAssembly(), new string[0]);
					installer.UseNewContext = true;
					IDictionary savedState = new Hashtable();
					installer.Uninstall(savedState);
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
		if (otherSingletons != null)
		{
			ServiceBase[] hosts = new ServiceBase[otherSingletons.Length + 1];
			int pos = 0;
			hosts[pos++] = this;
			ServiceBasePublic[] array = otherSingletons;
			foreach (ServiceBase sb in array)
			{
				hosts[pos++] = sb;
			}
			ServiceBase.Run(hosts);
		}
		else
		{
			ServiceBase.Run(this);
		}
	}
}
