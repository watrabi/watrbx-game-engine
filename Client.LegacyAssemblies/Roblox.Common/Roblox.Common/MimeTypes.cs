using Microsoft.Win32;

namespace Roblox.Common;

public class MimeTypes
{
	public static string GetExtensionFromMime(string mimeType)
	{
		try
		{
			RegistryKey regKey = Registry.ClassesRoot.OpenSubKey("Mime\\Database\\Content Type\\" + mimeType, writable: false);
			if (regKey == null)
			{
				return null;
			}
			string ext = regKey.GetValue("Extension") as string;
			if (string.IsNullOrEmpty(ext))
			{
				return string.Empty;
			}
			return ext;
		}
		catch
		{
			return string.Empty;
		}
	}

	public static string GetMimeFromExtension(string ext)
	{
		RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(ext, writable: false);
		if (regKey == null)
		{
			return null;
		}
		return regKey.GetValue("Content Type") as string;
	}
}
