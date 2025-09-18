using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;

namespace Roblox.Common.Mime;

public class MimeReader
{
	public const string Crlf = "\r\n";

	private static readonly char[] HeaderWhitespaceChars = new char[2] { ' ', '\t' };

	private Queue<string> _lines;

	private MimeEntity _entity;

	private MimeReader()
	{
		_entity = new MimeEntity();
	}

	private MimeReader(MimeEntity entity, Queue<string> lines)
		: this()
	{
		if (entity == null)
		{
			throw new ArgumentNullException("entity");
		}
		if (lines == null)
		{
			throw new ArgumentNullException("lines");
		}
		_lines = lines;
		_entity = new MimeEntity(entity);
	}

	public MimeReader(string[] lines)
		: this()
	{
		if (lines == null)
		{
			throw new ArgumentNullException("lines");
		}
		_lines = new Queue<string>(lines);
	}

	private int ParseHeaders()
	{
		string name = string.Empty;
		string line = string.Empty;
		while (_lines.Count > 0 && !string.IsNullOrEmpty(_lines.Peek()))
		{
			line = _lines.Dequeue();
			if (line.StartsWith(" ") || line.StartsWith(Convert.ToString('\t')))
			{
				_entity.Headers[name] = _entity.Headers[name] + line;
				continue;
			}
			int idx = line.IndexOf(':');
			if (idx >= 0)
			{
				string key = line.Substring(0, idx);
				string value = line.Substring(idx + 1).Trim(HeaderWhitespaceChars);
				_entity.Headers.Add(key.ToLower(), value);
				name = key;
			}
		}
		if (_lines.Count > 0)
		{
			_lines.Dequeue();
		}
		return _entity.Headers.Count;
	}

	private void ProcessHeaders()
	{
		string[] allKeys = _entity.Headers.AllKeys;
		foreach (string headerName in allKeys)
		{
			switch (headerName)
			{
			case "content-description":
				_entity.ContentDescription = _entity.Headers[headerName];
				break;
			case "content-disposition":
				_entity.ContentDisposition = new ContentDisposition(_entity.Headers[headerName]);
				break;
			case "content-id":
				_entity.ContentId = _entity.Headers[headerName];
				break;
			case "content-transfer-encoding":
				_entity.TransferEncoding = _entity.Headers[headerName];
				_entity.ContentTransferEncoding = GetTransferEncoding(_entity.Headers[headerName]);
				break;
			case "content-type":
				_entity.SetContentType(GetContentType(_entity.Headers[headerName]));
				break;
			case "mime-version":
				_entity.MimeVersion = _entity.Headers[headerName];
				break;
			}
		}
	}

	public MimeEntity CreateMimeEntity()
	{
		ParseHeaders();
		ProcessHeaders();
		ParseBody();
		SetDecodedContentStream();
		return _entity;
	}

	private void SetDecodedContentStream()
	{
		switch (_entity.ContentTransferEncoding)
		{
		case TransferEncoding.QuotedPrintable:
			_entity.Content = new MemoryStream(GetBytes(QuotedPrintableEncoding.Decode(_entity.EncodedMessage.ToString())), writable: false);
			break;
		case TransferEncoding.Base64:
			_entity.Content = new MemoryStream(Convert.FromBase64String(_entity.EncodedMessage.ToString()), writable: false);
			break;
		default:
			_entity.Content = new MemoryStream(GetBytes(_entity.EncodedMessage.ToString()), writable: false);
			break;
		}
	}

	private byte[] GetBytes(string content)
	{
		using MemoryStream stream = new MemoryStream();
		using (StreamWriter streamWriter = new StreamWriter(stream))
		{
			streamWriter.Write(content);
		}
		return stream.ToArray();
	}

	private void ParseBody()
	{
		if (_entity.HasBoundry)
		{
			while (_lines.Count > 0 && !string.Equals(_lines.Peek(), _entity.EndBoundry) && (_entity.Parent == null || !string.Equals(_entity.Parent.StartBoundry, _lines.Peek())))
			{
				if (string.Equals(_lines.Peek(), _entity.StartBoundry))
				{
					AddChildEntity(_entity, _lines);
					continue;
				}
				if (string.Equals(_entity.ContentType.MediaType, MediaTypes.MessageRfc822, StringComparison.InvariantCultureIgnoreCase) && string.Equals(_entity.ContentDisposition.DispositionType, "attachment", StringComparison.InvariantCultureIgnoreCase))
				{
					AddChildEntity(_entity, _lines);
					break;
				}
				_entity.EncodedMessage.Append(_lines.Dequeue() + "\r\n");
			}
		}
		else
		{
			while (_lines.Count > 0)
			{
				_entity.EncodedMessage.Append(_lines.Dequeue() + "\r\n");
			}
		}
	}

	private void AddChildEntity(MimeEntity entity, Queue<string> lines)
	{
		MimeReader reader = new MimeReader(entity, lines);
		entity.Children.Add(reader.CreateMimeEntity());
	}

	public static ContentType GetContentType(string contentType)
	{
		if (string.IsNullOrEmpty(contentType))
		{
			contentType = "text/plain; charset=us-ascii";
		}
		return new ContentType(contentType);
	}

	public static string GetMediaType(string mediaType)
	{
		if (string.IsNullOrEmpty(mediaType))
		{
			return MediaTypes.TextPlain;
		}
		return mediaType.Trim();
	}

	public static string GetMediaMainType(string mediaType)
	{
		int idx = mediaType.IndexOf('/');
		if (idx < 0)
		{
			return mediaType;
		}
		return mediaType.Substring(0, idx);
	}

	public static string GetMediaSubType(string mediaType)
	{
		int idx = mediaType.IndexOf('/');
		if (idx < 0)
		{
			if (mediaType.Equals("text"))
			{
				return "plain";
			}
			return string.Empty;
		}
		if (mediaType.Length > idx)
		{
			return mediaType.Substring(idx + 1);
		}
		if (GetMediaMainType(mediaType).Equals("text"))
		{
			return "plain";
		}
		return string.Empty;
	}

	public static TransferEncoding GetTransferEncoding(string transferEncoding)
	{
		switch (transferEncoding.Trim().ToLowerInvariant())
		{
		case "7bit":
		case "8bit":
			return TransferEncoding.SevenBit;
		case "quoted-printable":
			return TransferEncoding.QuotedPrintable;
		case "base64":
			return TransferEncoding.Base64;
		default:
			return TransferEncoding.Unknown;
		}
	}
}
