using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using Microsoft.Ccr.Core;

namespace Roblox.Common;

public static class Extensions
{
	public enum QueryOperators
	{
		AND,
		OR,
		NOT,
		NEAR
	}

	private static readonly char quote = '"';

	private static readonly char space = ' ';

	private static readonly char[] spaceDelimiter = new char[1] { space };

	public static IEnumerable<T> AsEnumerable<T>(this T item)
	{
		yield return item;
	}

	private static IEnumerator<ITask> StreamToArrayIterator(Stream stream, PortSet<byte[], Exception> result)
	{
		if (stream is MemoryStream str2)
		{
			result.Post(str2.ToArray());
			yield break;
		}
		byte[] buff = new byte[1024];
		MemoryStream memoryStream;
		MemoryStream str = (memoryStream = new MemoryStream());
		using (memoryStream)
		{
			PortSet<int, Exception> readPort = new PortSet<int, Exception>();
			Exception ex;
			while (true)
			{
				AsyncHelper.Call(stream.BeginRead, buff, 0, buff.Length, stream.EndRead, readPort, TimeSpan.FromMinutes(1.0), null);
				yield return (ITask)readPort;
				ex = readPort;
				if (ex != null)
				{
					break;
				}
				PortSet<int, Exception> index = readPort;
				if ((int)index == 0)
				{
					result.Post(str.ToArray());
					yield break;
				}
				str.Write(buff, 0, index);
			}
			result.Post(ex);
		}
	}

	private static IEnumerator<ITask> StreamToArrayAndDisposeIterator(Stream stream, PortSet<byte[], Exception> result)
	{
		using (stream)
		{
			if (stream is MemoryStream memStream2)
			{
				result.Post(memStream2.ToArray());
				yield break;
			}
			byte[] buff = new byte[1024];
			MemoryStream memStream = new MemoryStream();
			using (memStream)
			{
				PortSet<int, Exception> readPort = new PortSet<int, Exception>();
				while (true)
				{
					AsyncHelper.Call(stream.BeginRead, buff, 0, buff.Length, stream.EndRead, readPort, TimeSpan.FromMinutes(1.0), null);
					yield return (Choice)readPort;
					Exception ex = readPort;
					if (ex != null)
					{
						result.Post(ex);
						yield break;
					}
					int index = readPort;
					if (index == 0)
					{
						break;
					}
					memStream.Write(buff, 0, index);
				}
				result.Post(memStream.ToArray());
			}
		}
	}

	private static IEnumerator<ITask> StreamToStringIterator(Stream stream, PortSet<string, Exception> result)
	{
		ASCIIEncoding encoding = new ASCIIEncoding();
		if (stream is MemoryStream memStream2)
		{
			result.Post(encoding.GetString(memStream2.ToArray()));
			yield break;
		}
		byte[] buf = new byte[1024];
		MemoryStream memStream = new MemoryStream();
		using (memStream)
		{
			PortSet<int, Exception> readPort = new PortSet<int, Exception>();
			while (true)
			{
				AsyncHelper.Call(stream.BeginRead, buf, 0, buf.Length, stream.EndRead, readPort, TimeSpan.FromMinutes(1.0), null);
				yield return (Choice)readPort;
				Exception ex = readPort;
				if (ex != null)
				{
					result.Post(ex);
					yield break;
				}
				int index = readPort;
				if (index == 0)
				{
					break;
				}
				memStream.Write(buf, 0, index);
			}
			result.Post(encoding.GetString(memStream.ToArray()));
		}
	}

	private static IEnumerator<ITask> StreamToStringAndDisposeIterator(Stream stream, PortSet<string, Exception> result)
	{
		using (stream)
		{
			ASCIIEncoding encoding = new ASCIIEncoding();
			if (stream is MemoryStream memStream2)
			{
				result.Post(encoding.GetString(memStream2.ToArray()));
				yield break;
			}
			byte[] buff = new byte[1024];
			MemoryStream memStream = new MemoryStream();
			using (memStream)
			{
				PortSet<int, Exception> readPort = new PortSet<int, Exception>();
				while (true)
				{
					AsyncHelper.Call(stream.BeginRead, buff, 0, buff.Length, stream.EndRead, readPort, TimeSpan.FromMinutes(1.0), null);
					yield return (Choice)readPort;
					Exception ex = readPort;
					if (ex != null)
					{
						result.Post(ex);
						yield break;
					}
					int index = readPort;
					if (index == 0)
					{
						break;
					}
					memStream.Write(buff, 0, index);
				}
				result.Post(encoding.GetString(memStream.ToArray()));
			}
		}
	}

	public static void Exists(this FileInfo fileInfo, PortSet<bool, Exception> result)
	{
		FileHelper.ExecuteTask(File.Exists, fileInfo.FullName, result);
	}

	public static void ToArray(this Stream stream, PortSet<byte[], Exception> result)
	{
		CcrService.Singleton.SpawnIterator(stream, result, StreamToArrayIterator);
	}

	public static void ToArrayAndDispose(this Stream stream, PortSet<byte[], Exception> result)
	{
		CcrService.Singleton.SpawnIterator(stream, result, StreamToArrayAndDisposeIterator);
	}

	public static void ToString(this Stream stream, PortSet<string, Exception> result)
	{
		CcrService.Singleton.SpawnIterator(stream, result, StreamToStringIterator);
	}

	public static void ToStringAndDispose(this Stream stream, PortSet<string, Exception> result)
	{
		CcrService.Singleton.SpawnIterator(stream, result, StreamToStringIterator);
	}

	public static void WriteAsync(this Stream stream, byte[] data, Action<Result<CompletionSignal>> resultHandler)
	{
		Request<CompletionSignal>.HandleResponse((Func<byte[], int, int, AsyncCallback, object, IAsyncResult>)stream.BeginWrite, data, 0, data.Length, (Action<IAsyncResult>)stream.EndWrite, resultHandler);
	}

	public static void WriteAndDisposeAsync(this Stream stream, byte[] data, Action<Result<CompletionSignal>> resultHandler)
	{
		Request<CompletionSignal>.HandleResponse(stream.BeginWrite, data, 0, data.Length, delegate(IAsyncResult ar)
		{
			try
			{
				stream.EndWrite(ar);
				return CompletionSignal.Instance;
			}
			finally
			{
				stream?.Dispose();
			}
		}, resultHandler);
	}

	public static string Convert(this string value, Encoding originalEncoding, Encoding newEncoding)
	{
		byte[] valueBytes = value.GetBytes(originalEncoding);
		byte[] newBytes = Encoding.Convert(originalEncoding, newEncoding, valueBytes);
		return newEncoding.GetString(newBytes);
	}

	public static byte[] GetBytes(this string value)
	{
		return value.GetBytes(new ASCIIEncoding());
	}

	public static byte[] GetBytes(this string value, Encoding encoding)
	{
		return encoding.GetBytes(value);
	}

	public static bool IsEven(this int value)
	{
		return value % 2 == 0;
	}

	public static bool IsOdd(this int value)
	{
		return !value.IsEven();
	}

	public static int[] IndexesOf(this string s, char value)
	{
		List<int> indexes = new List<int>();
		for (int i = s.IndexOf(value); i > -1; i = s.IndexOf(value, i + 1))
		{
			indexes.Add(i);
		}
		return indexes.ToArray();
	}

	public static string ParsedSubString(this string text, string startToken, string endToken, bool startWithLastOccurrences)
	{
		return text.ParsedSubString(startToken, endToken, includeStartToken: false, includeEndToken: false, startWithLastOccurrences);
	}

	public static string ParsedSubString(this string text, string startToken, string endToken)
	{
		return text.ParsedSubString(startToken, endToken, includeStartToken: false, includeEndToken: false, startWithLastOccurrences: false);
	}

	public static string ParsedSubString(this string text, string startToken, string endToken, bool includeStartToken, bool includeEndToken, bool startWithLastOccurrences)
	{
		int startIndex = ((!startWithLastOccurrences) ? text.ToLower().IndexOf(startToken.ToLower()) : text.ToLower().LastIndexOf(startToken.ToLower()));
		startIndex = ((startIndex >= 0) ? (startIndex + ((!includeStartToken) ? startToken.Length : 0)) : 0);
		text = text.Substring(startIndex);
		int length = text.ToLower().IndexOf(endToken.ToLower());
		length = ((length >= 0) ? (length + (includeEndToken ? endToken.Length : 0)) : text.Length);
		return text.Substring(0, length);
	}

	public static void GetRequestStreamAsync(this WebRequest webRequest, Action<Result<Stream>> resultHandler)
	{
		Request<Stream>.HandleResponse(webRequest.BeginGetRequestStream, webRequest.EndGetRequestStream, resultHandler);
	}

	public static IAsyncResult BeginGetResponseStream(this WebResponse webResponse, AsyncCallback callback, object state)
	{
		FastAsyncResult<Stream> result = new FastAsyncResult<Stream>(callback, state);
		RobloxThreadPool.QueueUserWorkItem(delegate
		{
			try
			{
				result.SetCompleted(webResponse.GetResponseStream());
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception completed)
			{
				result.SetCompleted(completed);
			}
		});
		return result;
	}

	public static Stream EndGetResponseStream(this WebResponse webResponse, IAsyncResult asyncResult)
	{
		using FastAsyncResult<Stream> result = asyncResult as FastAsyncResult<Stream>;
		return result.GetResult();
	}

	public static void GetResponseStreamAsync(this WebResponse webResponse, Action<Result<Stream>> resultHandler)
	{
		Request<Stream>.HandleResponse(webResponse.BeginGetResponseStream, webResponse.EndGetResponseStream, resultHandler);
	}

	public static IAsyncResult BeginLoad(this XmlDocument xmlDocument, Stream data, AsyncCallback callback, object state)
	{
		FastAsyncResult result = new FastAsyncResult(callback, state);
		RobloxThreadPool.QueueUserWorkItem(delegate
		{
			try
			{
				xmlDocument.Load(data);
				result.SetCompleted();
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception completed)
			{
				result.SetCompleted(completed);
			}
		});
		return result;
	}

	public static void EndLoad(this XmlDocument _, IAsyncResult asyncResult)
	{
		using FastAsyncResult result = asyncResult as FastAsyncResult;
		if (result.Error != null)
		{
			throw result.Error;
		}
	}

	public static void LoadAsync(this XmlDocument xmlDocument, Stream data, Action<Result<CompletionSignal>> resultHandler)
	{
		Request<CompletionSignal>.HandleResponse(xmlDocument.BeginLoad, data, delegate(IAsyncResult ar)
		{
			xmlDocument.EndLoad(ar);
			return CompletionSignal.Instance;
		}, resultHandler);
	}

	public static string ToQuery(this string query)
	{
		return query.ToQuery(QueryOperators.AND);
	}

	public static string ToQuery(this string query, QueryOperators defaultOperator)
	{
		string op = $" {defaultOperator.ToString()} ";
		string q = "";
		if (!string.IsNullOrEmpty(query.Trim()))
		{
			string searchTerm = "";
			char qout = '"';
			char ws = ' ';
			List<string> searchTerms = new List<string>();
			bool isQuot = false;
			query = Regex.Replace(query.Trim(), "\\s{2,}", ws.ToString());
			char[] bytes = query.ToCharArray();
			for (int i = 0; i < query.Length; i++)
			{
				char @char = bytes[i];
				searchTerm += @char;
				if (@char.Equals(qout))
				{
					isQuot = !isQuot;
				}
				if (isQuot)
				{
					continue;
				}
				if (@char.Equals(qout))
				{
					AddSearchTerm(searchTerm, ref searchTerms, defaultOperator);
					searchTerm = "";
				}
				if (@char.Equals(ws))
				{
					if (!string.IsNullOrEmpty(searchTerm.Trim()))
					{
						AddSearchTerm(searchTerm, ref searchTerms, defaultOperator);
					}
					searchTerm = "";
				}
			}
			if (searchTerm != "" && (!IsQueryOperator(searchTerm) || searchTerm.Equals(")")))
			{
				AddSearchTerm(searchTerm, ref searchTerms, defaultOperator);
			}
			if (!IsQueryOperator(searchTerms[0]))
			{
				q = searchTerms[0];
			}
			for (int j = 1; j < searchTerms.Count; j++)
			{
				if (!IsQueryOperator(searchTerms[j]) && !IsQueryOperator(searchTerms[j - 1]) && !searchTerms[j].Trim().Equals(")"))
				{
					q += op;
				}
				if (searchTerms[j].ToUpper() == QueryOperators.NOT.ToString() && !IsQueryOperator(searchTerms[j - 1]))
				{
					q += op;
				}
				q = q + " " + searchTerms[j];
			}
			q = Regex.Replace(q.Trim(), "\\s{2,}", ws.ToString());
		}
		return q;
	}

	private static bool IsQueryOperator(string s)
	{
		s = s.Trim().ToLower();
		string[] names = Enum.GetNames(typeof(QueryOperators));
		foreach (string name in names)
		{
			if (s == name.ToLower())
			{
				return true;
			}
		}
		if (!(s == "("))
		{
			return s == ")";
		}
		return true;
	}

	private static string AddQuotesToSearchTerm(string searchTerm)
	{
		char qout = '"';
		char openBkt = '(';
		char closeBkt = ')';
		searchTerm = searchTerm.Trim();
		if (!IsQueryOperator(searchTerm) && searchTerm[0] == qout)
		{
			if (searchTerm[searchTerm.Length - 1] != qout)
			{
				searchTerm += qout;
			}
			else
			{
				searchTerm = ((searchTerm[0] != openBkt) ? (qout + searchTerm) : (openBkt + AddQuotesToSearchTerm(searchTerm.Substring(1))));
				if (searchTerm[searchTerm.Length - 1] == closeBkt)
				{
					searchTerm = AddQuotesToSearchTerm(searchTerm.Substring(0, searchTerm.Length - 1)) + closeBkt;
				}
				else if (searchTerm[searchTerm.Length - 1] != qout)
				{
					searchTerm += qout;
				}
			}
		}
		return searchTerm;
	}

	private static void AddSearchTerm(string searchTerm, ref List<string> searchTerms, QueryOperators defaultOperator)
	{
		int idx = searchTerm.IndexOf(quote);
		searchTerm = Regex.Replace(searchTerm, "^(\\&{1,}|\\+{1,})", " and ");
		searchTerm = Regex.Replace(searchTerm, "^(\\|{1,})", " or ");
		searchTerm = Regex.Replace(searchTerm, "^(\\~{1,})", " near ");
		searchTerm = Regex.Replace(searchTerm, "^(\\-{1,}|\\!{1,})", $" {defaultOperator.ToString()} not ");
		searchTerm = Regex.Replace(searchTerm.Trim(), "\\s+", space.ToString());
		int length = searchTerm.Length;
		if (searchTerm.Contains(quote.ToString()))
		{
			length = idx;
		}
		if (searchTerm.Substring(0, length).Contains(space.ToString()))
		{
			string[] array = searchTerm.Substring(0, length).Split(spaceDelimiter, StringSplitOptions.RemoveEmptyEntries);
			foreach (string subTerm in array)
			{
				searchTerms.Add(AddQuotesToSearchTerm(subTerm));
			}
			if (length < searchTerm.Length)
			{
				searchTerms.Add(AddQuotesToSearchTerm(searchTerm.Substring(length)));
			}
		}
		else
		{
			searchTerms.Add(AddQuotesToSearchTerm(searchTerm));
		}
	}

	public static string ToDescription(this Enum value)
	{
		DescriptionAttribute[] array = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
		if (array != null && array.Length != 0)
		{
			return array[0].Description;
		}
		return value.ToString();
	}

	public static bool UnorderedEqual<T>(this ICollection<T> list, ICollection<T> listToCompare)
	{
		if (list.Count != listToCompare.Count)
		{
			return false;
		}
		Dictionary<T, int> d = new Dictionary<T, int>();
		foreach (T key in list)
		{
			if (d.TryGetValue(key, out var num2))
			{
				d[key] = num2 + 1;
			}
			else
			{
				d.Add(key, 1);
			}
		}
		foreach (T key2 in listToCompare)
		{
			if (!d.TryGetValue(key2, out var num))
			{
				return false;
			}
			if (num == 0)
			{
				return false;
			}
			d[key2] = num - 1;
		}
		foreach (int value in d.Values)
		{
			if (value != 0)
			{
				return false;
			}
		}
		return true;
	}

	public static string ToJSON<TSource>(this IEnumerable<TSource> source) where TSource : class
	{
		string json = "[";
		foreach (TSource obj in source)
		{
			json = json + obj.ToJSON() + ",";
		}
		json = json.TrimEnd(',');
		return json + "]";
	}

	public static string ToJSON<T>(this T obj) where T : class
	{
		DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
		using MemoryStream stream = new MemoryStream();
		dataContractJsonSerializer.WriteObject(stream, obj);
		return Encoding.Default.GetString(stream.ToArray());
	}

	public static T FromJSON<T>(this string json) where T : class
	{
		using MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(json));
		return new DataContractJsonSerializer(typeof(T)).ReadObject(stream) as T;
	}
}
