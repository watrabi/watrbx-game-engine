using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using Roblox.Common.Mime;
using Roblox.Common.Properties;

namespace Roblox.Common;

public class Email
{
	private static string defaultSMTPServer;

	private const string linebreak = "\r\n";

	static Email()
	{
		defaultSMTPServer = Settings.Default.SMTPServer;
		EventLog.WriteEntry("Web Server", $"Roblox.Common.Email.SMTPServer is {defaultSMTPServer}", EventLogEntryType.Information);
	}

	public static void SendEmail(string to, string from, string subject, string body)
	{
		SendEmail(to, from, subject, body, defaultSMTPServer);
	}

	public static void SendEmail(string to, string from, string subject, string body, string smtpServer)
	{
		if (to.Trim().Length == 0)
		{
			return;
		}
		SmtpClient smtpClient = new SmtpClient(smtpServer);
		using MailMessage message = new MailMessage(from, to, subject, body);
		smtpClient.Send(message);
	}

	public static void SendMimeEmail(string to, string from, string subject, string plaintextbody, string htmlbody)
	{
		ContentType mimeType = new ContentType("text/html");
		AlternateView alternate = AlternateView.CreateAlternateViewFromString(htmlbody, mimeType);
		SmtpClient client = new SmtpClient(defaultSMTPServer);
		client.Credentials = CredentialCache.DefaultNetworkCredentials;
		using MailMessage message = new MailMessage(from, to, subject, plaintextbody);
		message.AlternateViews.Add(alternate);
		client.Send(message);
	}

	public static void SendMimeEmail(string to, string from, string subject, string plaintextbody, string htmlbody, string smtpServer)
	{
		if (to.Trim().Length == 0)
		{
			return;
		}
		ContentType mimeType = new ContentType("text/html");
		AlternateView alternate = AlternateView.CreateAlternateViewFromString(htmlbody, mimeType);
		SmtpClient client = new SmtpClient(smtpServer);
		client.Credentials = CredentialCache.DefaultNetworkCredentials;
		using MailMessage message = new MailMessage(from, to, subject, plaintextbody);
		message.AlternateViews.Add(alternate);
		client.Send(message);
	}

	public static MailMessageEx ParseEmail(string rawEmail)
	{
		MimeReader reader = new MimeReader(Regex.Split(rawEmail, "\r\n"));
		MimeEntity mime = reader.CreateMimeEntity();
		return mime.ToMailMessageEx();
	}
}
