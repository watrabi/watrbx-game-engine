using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace Roblox.Common.Mail.Pop3;

public class Pop3MailClient : IDisposable
{
	protected string popServer;

	protected int port;

	private bool useSSL;

	private bool isAutoReconnect;

	private bool isTimeoutReconnect;

	protected int readTimeout = -1;

	protected string username;

	protected string password;

	protected Pop3ConnectionStateEnum pop3ConnectionState = Pop3ConnectionStateEnum.Disconnected;

	private TcpClient serverTcpConnection;

	private Stream pop3Stream;

	protected StreamReader pop3StreamReader;

	protected string CRLF = "\r\n";

	protected bool isTraceRawEmail;

	protected StringBuilder RawEmailSB;

	public bool isDebug;

	public string PopServer => popServer;

	public int Port => port;

	public bool UseSSL => useSSL;

	public bool IsAutoReconnect
	{
		get
		{
			return isAutoReconnect;
		}
		set
		{
			isAutoReconnect = value;
		}
	}

	public int ReadTimeout
	{
		get
		{
			return readTimeout;
		}
		set
		{
			readTimeout = value;
			if (pop3Stream != null && pop3Stream.CanTimeout)
			{
				pop3Stream.ReadTimeout = readTimeout;
			}
		}
	}

	public string Username => username;

	public string Password => password;

	public Pop3ConnectionStateEnum Pop3ConnectionState => pop3ConnectionState;

	public event WarningHandler Warning;

	public event TraceHandler Trace;

	protected void CallWarning(string methodName, string response, string warningText, params object[] warningParameters)
	{
		warningText = string.Format(warningText, warningParameters);
		if (this.Warning != null)
		{
			this.Warning(methodName + ": " + warningText, response);
		}
		CallTrace("!! {0}", warningText);
	}

	protected void CallTrace(string text, params object[] parameters)
	{
		if (this.Trace != null)
		{
			this.Trace(DateTime.Now.ToString("hh:mm:ss ") + popServer + " " + string.Format(text, parameters));
		}
	}

	protected void TraceFrom(string text, params object[] parameters)
	{
		if (this.Trace != null)
		{
			CallTrace("   " + string.Format(text, parameters));
		}
	}

	protected void setPop3ConnectionState(Pop3ConnectionStateEnum State)
	{
		pop3ConnectionState = State;
		CallTrace("   Pop3MailClient Connection State {0} reached", State);
	}

	protected void EnsureState(Pop3ConnectionStateEnum requiredState)
	{
		if (pop3ConnectionState != requiredState)
		{
			throw new Pop3Exception("GetMailboxStats only accepted during connection state: " + requiredState.ToString() + "\n The connection to server " + popServer + " is in state " + pop3ConnectionState);
		}
	}

	public Pop3MailClient(string PopServer, int Port, bool useSSL, string Username, string Password)
	{
		popServer = PopServer;
		port = Port;
		this.useSSL = useSSL;
		username = Username;
		password = Password;
	}

	public void Connect()
	{
		if (pop3ConnectionState != Pop3ConnectionStateEnum.Disconnected && pop3ConnectionState != Pop3ConnectionStateEnum.Closed && !isTimeoutReconnect)
		{
			CallWarning("connect", "", "Connect command received, but connection state is: " + pop3ConnectionState);
			return;
		}
		try
		{
			CallTrace("   Connect at port {0}", port);
			serverTcpConnection = new TcpClient(popServer, port);
		}
		catch (Exception ex5)
		{
			throw new Pop3Exception("Connection to server " + popServer + ", port " + port + " failed.\nRuntime Error: " + ex5.ToString());
		}
		if (useSSL)
		{
			try
			{
				CallTrace("   Get SSL connection");
				pop3Stream = new SslStream(serverTcpConnection.GetStream(), leaveInnerStreamOpen: false);
				pop3Stream.ReadTimeout = readTimeout;
			}
			catch (Exception ex4)
			{
				throw new Pop3Exception("Server " + popServer + " found, but cannot get SSL data stream.\nRuntime Error: " + ex4.ToString());
			}
			try
			{
				CallTrace("   Get SSL authentication");
				((SslStream)pop3Stream).AuthenticateAsClient(popServer);
			}
			catch (Exception ex3)
			{
				throw new Pop3Exception("Server " + popServer + " found, but problem with SSL Authentication.\nRuntime Error: " + ex3.ToString());
			}
		}
		else
		{
			try
			{
				CallTrace("   Get connection without SSL");
				pop3Stream = serverTcpConnection.GetStream();
				pop3Stream.ReadTimeout = readTimeout;
			}
			catch (Exception ex2)
			{
				throw new Pop3Exception("Server " + popServer + " found, but cannot get data stream (without SSL).\nRuntime Error: " + ex2.ToString());
			}
		}
		try
		{
			pop3StreamReader = new StreamReader(pop3Stream, Encoding.ASCII);
		}
		catch (Exception ex)
		{
			if (useSSL)
			{
				throw new Pop3Exception("Server " + popServer + " found, but cannot read from SSL stream.\nRuntime Error: " + ex.ToString());
			}
			throw new Pop3Exception("Server " + popServer + " found, but cannot read from stream (without SSL).\nRuntime Error: " + ex.ToString());
		}
		if (!readSingleLine(out var response))
		{
			throw new Pop3Exception("Server " + popServer + " not ready to start AUTHORIZATION.\nMessage: " + response);
		}
		setPop3ConnectionState(Pop3ConnectionStateEnum.Authorization);
		if (!executeCommand("USER " + username, out response))
		{
			throw new Pop3Exception("Server " + popServer + " doesn't accept username '" + username + "'.\nMessage: " + response);
		}
		if (!executeCommand("PASS " + password, out response))
		{
			throw new Pop3Exception("Server " + popServer + " doesn't accept password '" + password + "' for user '" + username + "'.\nMessage: " + response);
		}
		setPop3ConnectionState(Pop3ConnectionStateEnum.Connected);
	}

	public void Disconnect()
	{
		if (pop3ConnectionState == Pop3ConnectionStateEnum.Disconnected || pop3ConnectionState == Pop3ConnectionStateEnum.Closed)
		{
			CallWarning("disconnect", "", "Disconnect received, but was already disconnected.");
			return;
		}
		try
		{
			if (executeCommand("QUIT", out var response))
			{
				setPop3ConnectionState(Pop3ConnectionStateEnum.Closed);
				return;
			}
			CallWarning("Disconnect", response, "negative response from server while closing connection: " + response);
			setPop3ConnectionState(Pop3ConnectionStateEnum.Disconnected);
		}
		finally
		{
			if (pop3Stream != null)
			{
				pop3Stream.Close();
			}
			pop3StreamReader.Close();
		}
	}

	public bool DeleteEmail(int msg_number)
	{
		EnsureState(Pop3ConnectionStateEnum.Connected);
		if (!executeCommand("DELE " + msg_number, out var response))
		{
			CallWarning("DeleteEmail", response, "negative response for email (Id: {0}) delete request", msg_number);
			return false;
		}
		return true;
	}

	public bool GetEmailIdList(out List<int> EmailIds)
	{
		EnsureState(Pop3ConnectionStateEnum.Connected);
		EmailIds = new List<int>();
		if (!executeCommand("LIST", out var response))
		{
			CallWarning("GetEmailIdList", response, "negative response for email list request");
			return false;
		}
		while (readMultiLine(out response))
		{
			if (int.TryParse(response.Split(' ')[0], out var EmailId))
			{
				EmailIds.Add(EmailId);
			}
			else
			{
				CallWarning("GetEmailIdList", response, "first characters should be integer (EmailId)");
			}
		}
		TraceFrom("{0} email ids received", EmailIds.Count);
		return true;
	}

	public int GetEmailSize(int msg_number)
	{
		EnsureState(Pop3ConnectionStateEnum.Connected);
		executeCommand("LIST " + msg_number, out var response);
		int EmailSize = 0;
		string[] responseSplit = response.Split(' ');
		if (responseSplit.Length < 2 || !int.TryParse(responseSplit[2], out EmailSize))
		{
			CallWarning("GetEmailSize", response, "'+OK int int' format expected (EmailId, EmailSize)");
		}
		return EmailSize;
	}

	public bool GetUniqueEmailIdList(out List<EmailUid> EmailIds)
	{
		EnsureState(Pop3ConnectionStateEnum.Connected);
		EmailIds = new List<EmailUid>();
		if (!executeCommand("UIDL ", out var response))
		{
			CallWarning("GetUniqueEmailIdList", response, "negative response for email list request");
			return false;
		}
		while (readMultiLine(out response))
		{
			string[] responseSplit = response.Split(' ');
			int EmailId;
			if (responseSplit.Length < 2)
			{
				CallWarning("GetUniqueEmailIdList", response, "response not in format 'int string'");
			}
			else if (!int.TryParse(responseSplit[0], out EmailId))
			{
				CallWarning("GetUniqueEmailIdList", response, "first charaters should be integer (Unique EmailId)");
			}
			else
			{
				EmailIds.Add(new EmailUid(EmailId, responseSplit[1]));
			}
		}
		TraceFrom("{0} unique email ids received", EmailIds.Count);
		return true;
	}

	public bool GetUniqueEmailIdList(out SortedList<string, int> EmailIds)
	{
		EnsureState(Pop3ConnectionStateEnum.Connected);
		EmailIds = new SortedList<string, int>();
		if (!executeCommand("UIDL", out var response))
		{
			CallWarning("GetUniqueEmailIdList", response, "negative response for email list request");
			return false;
		}
		while (readMultiLine(out response))
		{
			string[] responseSplit = response.Split(' ');
			int EmailId;
			if (responseSplit.Length < 2)
			{
				CallWarning("GetUniqueEmailIdList", response, "response not in format 'int string'");
			}
			else if (!int.TryParse(responseSplit[0], out EmailId))
			{
				CallWarning("GetUniqueEmailIdList", response, "first charaters should be integer (Unique EmailId)");
			}
			else
			{
				EmailIds.Add(responseSplit[1], EmailId);
			}
		}
		TraceFrom("{0} unique email ids received", EmailIds.Count);
		return true;
	}

	public int GetUniqueEmailId(EmailUid msg_number)
	{
		EnsureState(Pop3ConnectionStateEnum.Connected);
		executeCommand("LIST " + msg_number, out var response);
		int EmailSize = 0;
		string[] responseSplit = response.Split(' ');
		if (responseSplit.Length < 2 || !int.TryParse(responseSplit[2], out EmailSize))
		{
			CallWarning("GetEmailSize", response, "'+OK int int' format expected (EmailId, EmailSize)");
		}
		return EmailSize;
	}

	public bool NOOP()
	{
		EnsureState(Pop3ConnectionStateEnum.Connected);
		if (!executeCommand("NOOP", out var response))
		{
			CallWarning("NOOP", response, "negative response for NOOP request");
			return false;
		}
		return true;
	}

	public bool GetRawEmail(int MessageNo, out string EmailText)
	{
		if (!SendRetrCommand(MessageNo))
		{
			EmailText = null;
			return false;
		}
		int LineCounter = 0;
		if (RawEmailSB == null)
		{
			RawEmailSB = new StringBuilder(100000);
		}
		else
		{
			RawEmailSB.Length = 0;
		}
		isTraceRawEmail = true;
		string response;
		while (readMultiLine(out response))
		{
			LineCounter++;
		}
		EmailText = RawEmailSB.ToString();
		TraceFrom("email with {0} lines,  {1} chars received", LineCounter.ToString(), EmailText.Length);
		return true;
	}

	public bool UndeleteAllEmails()
	{
		EnsureState(Pop3ConnectionStateEnum.Connected);
		string response;
		return executeCommand("RSET", out response);
	}

	public bool GetMailboxStats(out int NumberOfMails, out int MailboxSize)
	{
		EnsureState(Pop3ConnectionStateEnum.Connected);
		NumberOfMails = 0;
		MailboxSize = 0;
		if (executeCommand("STAT", out var response))
		{
			string[] responseParts = response.Split(' ');
			if (responseParts.Length < 2)
			{
				throw new Pop3Exception("Server " + popServer + " sends illegally formatted response.\nExpected format: +OK int int\nReceived response: " + response);
			}
			NumberOfMails = int.Parse(responseParts[1]);
			MailboxSize = int.Parse(responseParts[2]);
			return true;
		}
		return false;
	}

	protected bool SendRetrCommand(int MessageNo)
	{
		EnsureState(Pop3ConnectionStateEnum.Connected);
		if (!executeCommand("RETR " + MessageNo, out var response))
		{
			CallWarning("GetRawEmail", response, "negative response for email (ID: {0}) request", MessageNo);
			return false;
		}
		return true;
	}

	private bool executeCommand(string command, out string response)
	{
		byte[] commandBytes = Encoding.ASCII.GetBytes((command + CRLF).ToCharArray());
		CallTrace("Tx '{0}'", command);
		bool isSupressThrow = false;
		try
		{
			pop3Stream.Write(commandBytes, 0, commandBytes.Length);
			if (isDebug)
			{
				isDebug = false;
				throw new IOException("Test", new SocketException(10053));
			}
		}
		catch (IOException ex2)
		{
			if (!executeReconnect(ex2, command, commandBytes))
			{
				throw;
			}
		}
		pop3Stream.Flush();
		response = null;
		try
		{
			response = pop3StreamReader.ReadLine();
		}
		catch (IOException ex)
		{
			if (!executeReconnect(ex, command, commandBytes))
			{
				throw;
			}
			response = pop3StreamReader.ReadLine();
		}
		if (response == null)
		{
			throw new Pop3Exception("Server " + popServer + " has not responded, timeout has occured.");
		}
		CallTrace("Rx '{0}'", response);
		if (response.Length > 0)
		{
			return response[0] == '+';
		}
		return false;
	}

	private bool executeReconnect(IOException ex, string command, byte[] commandBytes)
	{
		if (ex.InnerException != null && ex.InnerException is SocketException)
		{
			SocketException innerEx = (SocketException)ex.InnerException;
			if (innerEx.ErrorCode == 10053)
			{
				CallWarning("ExecuteCommand", "", "probably timeout occured");
				if (isAutoReconnect)
				{
					isTimeoutReconnect = true;
					try
					{
						CallTrace("   try to auto reconnect");
						Connect();
						CallTrace("   reconnect successful, try to resend command");
						CallTrace("Tx '{0}'", command);
						pop3Stream.Write(commandBytes, 0, commandBytes.Length);
						pop3Stream.Flush();
						return true;
					}
					finally
					{
						isTimeoutReconnect = false;
					}
				}
			}
		}
		return false;
	}

	protected bool readSingleLine(out string response)
	{
		response = null;
		try
		{
			response = pop3StreamReader.ReadLine();
		}
		catch (Exception ex)
		{
			string s = ex.Message;
		}
		if (response == null)
		{
			throw new Pop3Exception("Server " + popServer + " has not responded, timeout has occured.");
		}
		CallTrace("Rx '{0}'", response);
		if (response.Length > 0)
		{
			return response[0] == '+';
		}
		return false;
	}

	protected bool readMultiLine(out string response)
	{
		response = null;
		response = pop3StreamReader.ReadLine();
		if (response == null)
		{
			throw new Pop3Exception("Server " + popServer + " has not responded, probably timeout has occured.");
		}
		if (isTraceRawEmail)
		{
			RawEmailSB.Append(response + CRLF);
		}
		if (response.Length > 0 && response[0] == '.')
		{
			if (response == ".")
			{
				return false;
			}
			response = response.Substring(1, response.Length - 1);
		}
		return true;
	}

	public void Dispose()
	{
		Disconnect();
	}
}
