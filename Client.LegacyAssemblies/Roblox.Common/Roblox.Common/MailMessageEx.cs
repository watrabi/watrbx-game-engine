using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using Roblox.Common.Mime;

namespace Roblox.Common;

public class MailMessageEx : MailMessage
{
	public const string EmailRegexPattern = "(['\"]{1,}.+['\"]{1,}\\s+)?<?[\\w\\.\\-]+@[^\\.][\\w\\.\\-]+\\.[a-z]{2,}>?";

	private long _octets;

	private int _messageNumber;

	private static readonly char[] AddressDelimiters = new char[2] { ',', ';' };

	private List<MailMessageEx> _children;

	public long Octets
	{
		get
		{
			return _octets;
		}
		set
		{
			_octets = value;
		}
	}

	public int MessageNumber
	{
		get
		{
			return _messageNumber;
		}
		internal set
		{
			_messageNumber = value;
		}
	}

	public List<MailMessageEx> Children => _children;

	public string PlainTextBody
	{
		get
		{
			if (ContentType.MediaType == MediaTypes.TextPlain)
			{
				return base.Body;
			}
			foreach (AlternateView view in base.AlternateViews)
			{
				if (view.ContentType.MediaType == MediaTypes.TextPlain)
				{
					StreamReader rdr = new StreamReader(view.ContentStream);
					return rdr.ReadToEnd();
				}
			}
			return "";
		}
	}

	public DateTime DeliveryDate
	{
		get
		{
			string date = GetHeader("date");
			if (string.IsNullOrEmpty(date))
			{
				return DateTime.MinValue;
			}
			return Convert.ToDateTime(date);
		}
	}

	public MailAddress ReturnAddress
	{
		get
		{
			string replyTo = GetHeader("reply-to");
			if (string.IsNullOrEmpty(replyTo))
			{
				return null;
			}
			return CreateMailAddress(replyTo);
		}
	}

	public string Routing => GetHeader("received");

	public string MessageId => GetHeader("message-id");

	public string ReplyToMessageId => GetHeader("in-reply-to", stripBrackts: true);

	public string MimeVersion => GetHeader("mime-version");

	public string ContentId => GetHeader("content-id");

	public string ContentDescription => GetHeader("content-description");

	public ContentDisposition ContentDisposition
	{
		get
		{
			string contentDisposition = GetHeader("content-disposition");
			if (string.IsNullOrEmpty(contentDisposition))
			{
				return null;
			}
			return new ContentDisposition(contentDisposition);
		}
	}

	public ContentType ContentType
	{
		get
		{
			string contentType = GetHeader("content-type");
			if (string.IsNullOrEmpty(contentType))
			{
				return null;
			}
			return MimeReader.GetContentType(contentType);
		}
	}

	public MailMessageEx()
	{
		_children = new List<MailMessageEx>();
	}

	private string GetHeader(string header)
	{
		return GetHeader(header, stripBrackts: false);
	}

	private string GetHeader(string header, bool stripBrackts)
	{
		if (stripBrackts)
		{
			return MimeEntity.TrimBrackets(base.Headers[header]);
		}
		return base.Headers[header];
	}

	public static MailMessageEx CreateMailMessageFromEntity(MimeEntity entity)
	{
		MailMessageEx message = new MailMessageEx();
		string[] allKeys = entity.Headers.AllKeys;
		foreach (string key in allKeys)
		{
			string value = entity.Headers[key];
			if (value.Equals(string.Empty))
			{
				value = " ";
			}
			message.Headers.Add(key.ToLowerInvariant(), value);
			switch (key.ToLowerInvariant())
			{
			case "bcc":
				PopulateAddressList(value, message.Bcc);
				break;
			case "cc":
				PopulateAddressList(value, message.CC);
				break;
			case "from":
				message.From = CreateMailAddress(value);
				break;
			case "reply-to":
				message.ReplyTo = CreateMailAddress(value);
				break;
			case "subject":
				message.Subject = value;
				break;
			case "to":
				PopulateAddressList(value, message.To);
				break;
			}
		}
		return message;
	}

	public static MailAddress CreateMailAddress(string address)
	{
		try
		{
			return new MailAddress(address.Trim('\t'));
		}
		catch (FormatException e)
		{
			throw new ApplicationException("Unable to create mail address from provided string: " + address, e);
		}
	}

	public static void PopulateAddressList(string addressList, MailAddressCollection recipients)
	{
		foreach (MailAddress address in GetMailAddresses(addressList))
		{
			recipients.Add(address);
		}
	}

	public static IEnumerable<MailAddress> GetMailAddresses(string addressList)
	{
		Regex email = new Regex("(['\"]{1,}.+['\"]{1,}\\s+)?<?[\\w\\.\\-]+@[^\\.][\\w\\.\\-]+\\.[a-z]{2,}>?");
		foreach (Match match in email.Matches(addressList))
		{
			yield return CreateMailAddress(match.Value);
		}
	}
}
