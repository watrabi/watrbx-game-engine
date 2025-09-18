using System;
using System.ServiceModel;
using System.Xml;
using Roblox.Grid.Rcc;

namespace Roblox.Grid.Common;

public class GridServiceUtils
{
	private static readonly BasicHttpBinding binding;

	static GridServiceUtils()
	{
		binding = new BasicHttpBinding();
		binding.MaxReceivedMessageSize = 2147483647L;
		binding.SendTimeout = TimeSpan.FromMinutes(5.0);
		binding.ReceiveTimeout = TimeSpan.FromMinutes(5.0);
		binding.ReaderQuotas = new XmlDictionaryReaderQuotas();
		binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
	}

	public static RCCServiceSoap GetService(string Address)
	{
		return GetService(Address, 64989);
	}

	public static RCCServiceSoap GetService(string Address, int port)
	{
		if (Address == null)
		{
			return null;
		}
		return new RCCServiceSoap
		{
			Url = "http://" + Address + ":" + port
		};
	}
}
