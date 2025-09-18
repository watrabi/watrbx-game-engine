using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Roblox.Common.Mime;

public class QuotedPrintableEncoding
{
	private static string HexDecoderEvaluator(Match m)
	{
		return ((char)Convert.ToInt32(m.Groups[2].Value, 16)).ToString();
	}

	private static string HexDecoder(string line)
	{
		if (line == null)
		{
			throw new ArgumentNullException();
		}
		return new Regex("(\\=([0-9A-F][0-9A-F]))", RegexOptions.IgnoreCase).Replace(line, HexDecoderEvaluator);
	}

	public static string DecodeFile(string filepath)
	{
		if (filepath == null)
		{
			throw new ArgumentNullException();
		}
		FileInfo fileInfo = new FileInfo(filepath);
		if (!fileInfo.Exists)
		{
			throw new FileNotFoundException();
		}
		StreamReader streamReader = fileInfo.OpenText();
		try
		{
			string result = string.Empty;
			string encoded;
			while ((encoded = streamReader.ReadLine()) != null)
			{
				result += Decode(encoded);
			}
			return result;
		}
		finally
		{
			streamReader.Close();
		}
	}

	public static string Decode(string encoded)
	{
		if (encoded == null)
		{
			throw new ArgumentNullException();
		}
		using StringWriter stringWriter = new StringWriter();
		using (StringReader stringReader = new StringReader(encoded))
		{
			string line;
			while ((line = stringReader.ReadLine()) != null)
			{
				if (line.EndsWith("="))
				{
					stringWriter.Write(HexDecoder(line.Substring(0, line.Length - 1)));
				}
				else
				{
					stringWriter.WriteLine(HexDecoder(line));
				}
				stringWriter.Flush();
			}
		}
		return stringWriter.ToString();
	}
}
