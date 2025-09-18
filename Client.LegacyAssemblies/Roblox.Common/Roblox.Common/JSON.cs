using System.Text;
using System.Web.Script.Serialization;

namespace Roblox.Common;

public class JSON
{
	public static string WriteError(string message)
	{
		return $"{{\"Error\" : \"{message}\"}}";
	}

	public static string WriteProperty(string propertyName, string propertyValue)
	{
		return $"{{\"{propertyName}\" : \"{propertyValue}\"}}";
	}

	public static string AddListToJSONObject(string jsonString, string propertyName, string propertyValue)
	{
		if (!jsonString.Contains("{") && !jsonString.Contains("}") && !jsonString.Contains("[") && !jsonString.Contains("]"))
		{
			jsonString = "{" + jsonString + "}";
		}
		bool isArray = false;
		if (jsonString.StartsWith("[") && jsonString.EndsWith("]"))
		{
			isArray = true;
			jsonString = jsonString.Remove(0, 1);
			jsonString = jsonString.Remove(jsonString.Length - 1);
		}
		string property = $"\"{propertyName}\" : {propertyValue}";
		if (!isArray)
		{
			jsonString = jsonString.Remove(jsonString.Length - 1);
			if (jsonString.Length != 1)
			{
				jsonString += ",";
			}
			jsonString = jsonString + property + "}";
		}
		else
		{
			if (jsonString.Length != 0)
			{
				jsonString += ",";
			}
			jsonString += property;
		}
		if (isArray)
		{
			jsonString = "[" + jsonString + "]";
		}
		return jsonString;
	}

	public static string AddObjectToJSONObject(string jsonString, string propertyName, string propertyValue)
	{
		if (!jsonString.Contains("{") && !jsonString.Contains("}") && !jsonString.Contains("[") && !jsonString.Contains("]"))
		{
			jsonString = "{" + jsonString + "}";
		}
		bool isArray = false;
		if (jsonString.StartsWith("[") && jsonString.EndsWith("]"))
		{
			isArray = true;
			jsonString = jsonString.Remove(0, 1);
			jsonString = jsonString.Remove(jsonString.Length - 1);
		}
		string property = $"\"{propertyName}\" : {propertyValue}";
		if (!isArray)
		{
			jsonString = jsonString.Remove(jsonString.Length - 1);
			if (jsonString.Length != 1)
			{
				jsonString += ",";
			}
			jsonString = jsonString + property + "}";
		}
		else
		{
			if (jsonString.Length != 0)
			{
				jsonString += ",";
			}
			jsonString += property;
		}
		if (isArray)
		{
			jsonString = "[" + jsonString + "]";
		}
		return jsonString;
	}

	public static string AddPropertyToJSONObject(string jsonString, string propertyName, string propertyValue)
	{
		if (!jsonString.Contains("{") && !jsonString.Contains("}") && !jsonString.Contains("[") && !jsonString.Contains("]"))
		{
			jsonString = "{" + jsonString + "}";
		}
		bool isArray = false;
		if (jsonString.StartsWith("[") && jsonString.EndsWith("]"))
		{
			isArray = true;
			jsonString = jsonString.Remove(0, 1);
			jsonString = jsonString.Remove(jsonString.Length - 1);
		}
		string property = $"\"{propertyName}\" : \"{propertyValue}\"";
		if (!isArray)
		{
			jsonString = jsonString.Remove(jsonString.Length - 1);
			if (jsonString.Length != 1)
			{
				jsonString += ",";
			}
			jsonString = jsonString + property + "}";
		}
		else
		{
			if (jsonString.Length != 0)
			{
				jsonString += ",";
			}
			jsonString += property;
		}
		if (isArray)
		{
			jsonString = "[" + jsonString + "]";
		}
		return jsonString;
	}
}
public abstract class Json
{
	protected string SerializedData;

	public virtual string Serialize()
	{
		if (SerializedData == null)
		{
			string value = new JavaScriptSerializer().Serialize(this);
			SerializedData = value.Convert(Encoding.Unicode, Encoding.UTF8);
		}
		return SerializedData;
	}
}
