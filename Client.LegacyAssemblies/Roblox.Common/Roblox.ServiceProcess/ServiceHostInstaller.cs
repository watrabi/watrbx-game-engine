using System;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Roblox.ServiceProcess;

public abstract class ServiceHostInstaller : Installer
{
	public abstract string ServiceName { get; }

	public abstract string DisplayName { get; }

	public abstract string Description { get; }

	public ServiceHostInstaller()
	{
		ServiceProcessInstaller processInstaller = new ServiceProcessInstaller
		{
			Account = ServiceAccount.LocalSystem
		};
		ServiceInstaller serviceInstaller = new ServiceInstaller();
		serviceInstaller.ServiceName = ServiceName;
		serviceInstaller.DisplayName = DisplayName;
		serviceInstaller.Description = Description;
		serviceInstaller.StartType = ServiceStartMode.Automatic;
		serviceInstaller.Committed += service_Committed;
		base.Installers.Add(processInstaller);
		base.Installers.Add(serviceInstaller);
	}

	private void service_Committed(object sender, InstallEventArgs e)
	{
		using ServiceController controller = new ServiceController(ServiceName);
		controller.Start();
		controller.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10.0));
	}
}
