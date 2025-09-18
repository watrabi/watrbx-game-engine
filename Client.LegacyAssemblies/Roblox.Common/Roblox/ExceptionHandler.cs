using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using Roblox.Common;

namespace Roblox;

public class ExceptionHandler
{
	public class PresentableException : ApplicationException
	{
		private string _presentationErrorCode;

		private string _presentationErrorMessage;

		public string PresentationErrorCode => _presentationErrorCode;

		public string PresentationErrorMessage => _presentationErrorMessage;

		public PresentableException(string presentationErrorMessage, string errorMessage)
			: this(string.Empty, presentationErrorMessage, errorMessage)
		{
		}

		public PresentableException(string presentationErrorCode, string presentationErrorMessage, string errorMessage)
			: this(presentationErrorCode, presentationErrorMessage, errorMessage, null)
		{
		}

		public PresentableException(string presentationErrorMessage, string errorMessage, Exception innerException)
			: this(string.Empty, presentationErrorMessage, errorMessage, innerException)
		{
		}

		public PresentableException(string presentationErrorCode, string presentationErrorMessage, string errorMessage, Exception innerException)
			: base(errorMessage, innerException)
		{
			_presentationErrorMessage = presentationErrorMessage;
			_presentationErrorCode = presentationErrorCode;
		}
	}

	public enum PresentableSQLErrors
	{
		[Description("Search Query is malformed, please check the search terms and try your search again.")]
		SearchQueryMalformed = 7630
	}

	private static Dictionary<int, PresentableSQLErrors> PresentableSQLErrorsList;

	private static List<IExceptionHandlerListener> listeners;

	static ExceptionHandler()
	{
		listeners = new List<IExceptionHandlerListener>();
		if (PresentableSQLErrorsList != null)
		{
			return;
		}
		PresentableSQLErrorsList = new Dictionary<int, PresentableSQLErrors>();
		foreach (PresentableSQLErrors err in Converters.EnumToList<PresentableSQLErrors>())
		{
			PresentableSQLErrorsList.Add((int)err, err);
		}
	}

	public static void addListener(IExceptionHandlerListener listener)
	{
		lock (listeners)
		{
			listeners.Add(listener);
		}
	}

	public static void removeListener(IExceptionHandlerListener listener)
	{
		lock (listeners)
		{
			listeners.Remove(listener);
		}
	}

	public static string GetInnerText(Exception ex)
	{
		if (ex.InnerException != null)
		{
			return GetInnerText(ex.InnerException);
		}
		return ex.ToString();
	}

	public static string GetText(Exception ex)
	{
		if (ex.InnerException != null)
		{
			return ex.ToString() + "\n\nCaused by:\n" + GetText(ex.InnerException);
		}
		return ex.ToString();
	}

	public static void LogException(string errorMessage)
	{
		LogException(new ApplicationException(errorMessage));
	}

	public static void LogException(Exception ex)
	{
		LogException(ex, EventLogEntryType.Error);
	}

	public static void LogException(string errorMessage, EventLogEntryType type)
	{
		LogException(new ApplicationException(errorMessage), type);
	}

	public static void LogException(Exception ex, EventLogEntryType type)
	{
		LogException(ex, type, 1);
	}

	public static void LogException(string errorMessage, string sourceName)
	{
		LogException(new ApplicationException(errorMessage), sourceName);
	}

	public static void LogException(string errorMessage, EventLogEntryType type, string sourceName)
	{
		LogException(new ApplicationException(errorMessage), type, sourceName);
	}

	public static void LogException(Exception ex, string sourceName)
	{
		LogException(ex, EventLogEntryType.Error, 1, sourceName);
	}

	public static void LogException(Exception ex, EventLogEntryType type, string sourceName)
	{
		LogException(ex, type, 1, sourceName);
	}

	public static void LogException(string errorMessage, EventLogEntryType type, int eventID)
	{
		LogException(new ApplicationException(errorMessage), type, eventID);
	}

	public static void LogException(Exception ex, EventLogEntryType type, int eventID)
	{
		LogException(ex, type, eventID, "Roblox");
	}

	public static void LogException(string errorMessage, EventLogEntryType type, int eventID, string sourceName)
	{
		LogException(new ApplicationException(errorMessage), type, eventID, sourceName);
	}

	public static void LogException(Exception ex, EventLogEntryType type, int eventID, string sourceName)
	{
		short category = 0;
		string s;
		if (HttpContext.Current == null)
		{
			s = GetText(ex);
		}
		else
		{
			s = ex.Message + "\n\n";
			if (HttpContext.Current.Request != null)
			{
				s = s + "Url: " + HttpContext.Current.Request.Url?.ToString() + "\n";
				if (HttpContext.Current.Request.UrlReferrer != null)
				{
					s = s + "Referrer: " + HttpContext.Current.Request.UrlReferrer?.ToString() + "\n";
				}
				else if (HttpContext.Current.User != null)
				{
					if (HttpContext.Current.User.Identity != null)
					{
						s = s + "Identity: \"" + HttpContext.Current.User.Identity.Name + "\"\n";
						category = 2;
					}
					else
					{
						s += "Identity: null\n";
						category = 1;
					}
				}
				s += "\n";
			}
			s += GetText(ex);
		}
		if (ex is NotLoggedException)
		{
			return;
		}
		EventLog.WriteEntry(sourceName, s, type, eventID, category);
		foreach (IExceptionHandlerListener listener in listeners)
		{
			listener.exceptionLogged();
		}
	}

	public static void HandleSQLException(SqlException eSQL)
	{
		if (PresentableSQLErrorsList.TryGetValue(eSQL.Number, out var result))
		{
			PresentableException pe = new PresentableException(result.ToString(), result.ToDescription(), eSQL.Message, eSQL);
			LogException(pe, EventLogEntryType.Warning);
			throw pe;
		}
		throw eSQL;
	}
}
