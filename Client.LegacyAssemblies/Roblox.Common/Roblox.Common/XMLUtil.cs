using System.Collections.Generic;

namespace Roblox.Common;

public static class XMLUtil
{
	public static string GenerateXMLTable(ICollection<KeyValuePair<object, object>> entries)
	{
		string text = "<Value><Table>";
		foreach (KeyValuePair<object, object> entry in entries)
		{
			text += $"<Entry><Key>{entry.Key.ToString()}</Key><Value>{entry.Value.ToString()}</Value></Entry>";
		}
		return text + "</Table></Value>";
	}

	public static string GenerateXMLBool(bool value)
	{
		return $"<List><Value>{value.ToString()}</Value></List>";
	}
}
