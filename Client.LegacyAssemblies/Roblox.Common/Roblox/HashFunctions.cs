using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Roblox;

public static class HashFunctions
{
	public static string HashToString(byte[] rawHash)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < 16; i++)
		{
			string twoChars = rawHash[i].ToString("x");
			if (twoChars.Length == 1)
			{
				stringBuilder.AppendFormat("0{0}", twoChars);
			}
			else
			{
				stringBuilder.AppendFormat(twoChars);
			}
		}
		return stringBuilder.ToString();
	}

	public static byte[] ComputeHash(Stream buffer)
	{
		long pos = buffer.Position;
		buffer.Seek(0L, SeekOrigin.Begin);
		byte[] result;
		using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
		{
			result = md5.ComputeHash(buffer);
		}
		buffer.Seek(pos, SeekOrigin.Begin);
		return result;
	}

	public static byte[] ComputeHash(byte[] data)
	{
		using MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
		return md5.ComputeHash(data);
	}

	public static string ComputeHashString(byte[] data)
	{
		return HashToString(ComputeHash(data));
	}

	public static string ComputeHashString(Stream buffer)
	{
		return HashToString(ComputeHash(buffer));
	}
}
