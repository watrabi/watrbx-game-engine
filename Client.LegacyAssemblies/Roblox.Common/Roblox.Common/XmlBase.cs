using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Roblox.Common;

public abstract class XmlBase
{
	protected abstract XmlWriterSettings XmlWriterSettings { get; }

	private bool IsLegalCharacter(int character)
	{
		if (character != 9 && character != 10 && character != 13 && (character < 32 || character > 55295) && (character < 57344 || character > 65533))
		{
			if (character >= 65536)
			{
				return character <= 1114111;
			}
			return false;
		}
		return true;
	}

	public string Sanitize(string xmlString)
	{
		if (string.IsNullOrEmpty(xmlString))
		{
			return xmlString;
		}
		StringBuilder builder = new StringBuilder(xmlString.Length);
		foreach (char c in xmlString)
		{
			if (IsLegalCharacter(c))
			{
				builder.Append(c);
			}
		}
		return builder.ToString();
	}

	public virtual byte[] Write(Action<XmlWriter> write)
	{
		using MemoryStream ms = new MemoryStream();
		using XmlWriter writer = XmlWriter.Create(ms, XmlWriterSettings);
		write(writer);
		writer.Flush();
		ms.Seek(0L, SeekOrigin.Begin);
		return ms.ToArray();
	}
}
