using System.Text;
using System.Xml;

namespace Roblox.Common;

public class Xml : XmlBase
{
	private static XmlWriterSettings _XmlWriterSettings = new XmlWriterSettings
	{
		Encoding = Encoding.UTF8
	};

	public static Xml Singleton = new Xml();

	protected override XmlWriterSettings XmlWriterSettings => _XmlWriterSettings;

	private Xml()
	{
	}
}
