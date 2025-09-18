using System.Web;

namespace Roblox.Common;

public class TextTransforms
{
	public static string transformString(string stringToTransform)
	{
		return performCarriageReturnSubstitution(HttpContext.Current.Server.HtmlEncode(stringToTransform));
	}

	public static string performCarriageReturnSubstitution(string text)
	{
		return text.Replace("\n", "<br />");
	}
}
