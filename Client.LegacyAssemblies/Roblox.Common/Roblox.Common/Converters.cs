using System;
using System.Collections.Generic;

namespace Roblox.Common;

public static class Converters
{
	public static string ConvertIntegersToCSV(int[] integers)
	{
		return string.Join(",", Array.ConvertAll(integers, ConvertIntegerToString));
	}

	private static string ConvertIntegerToString(int integer)
	{
		return integer.ToString();
	}

	public static int[] ConvertCSVToIntegers(string[] strings)
	{
		return Array.ConvertAll(strings, ConvertStringToInteger);
	}

	private static int ConvertStringToInteger(string s)
	{
		return int.Parse(s);
	}

	public static List<T> EnumToList<T>()
	{
		Type typeFromHandle = typeof(T);
		if (typeFromHandle.BaseType != typeof(Enum))
		{
			throw new ArgumentException("T must be of type System.Enum");
		}
		Array values = Enum.GetValues(typeFromHandle);
		List<T> outList = new List<T>(values.Length);
		foreach (object value in values)
		{
			outList.Add((T)Enum.Parse(typeFromHandle, ((int)value).ToString()));
		}
		return outList;
	}
}
