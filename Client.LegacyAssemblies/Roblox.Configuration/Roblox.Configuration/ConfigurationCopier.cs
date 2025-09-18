using System;
using System.IO;

namespace Roblox.Configuration;

public class ConfigurationCopier
{
	public enum Result
	{
		UpdatedFiles,
		NoUpdatesNeeded
	}

	public static Result CopyConfiguration(string source, DirectoryInfo destination)
	{
		Result result = Result.NoUpdatesNeeded;
		if (string.IsNullOrEmpty(source))
		{
			throw new InvalidOperationException("SharedConfigSource isn't specified.");
		}
		FileInfo[] files = new DirectoryInfo(source).GetFiles("*.config");
		foreach (FileInfo file in files)
		{
			FileInfo destinationFile = new FileInfo(destination.FullName + file.Name);
			if (!destinationFile.Exists)
			{
				file.CopyTo(destinationFile.FullName, overwrite: true);
				result = Result.UpdatedFiles;
			}
			else if (destinationFile.LastWriteTimeUtc != file.LastWriteTimeUtc)
			{
				file.CopyTo(destinationFile.FullName, overwrite: true);
				result = Result.UpdatedFiles;
			}
		}
		return result;
	}
}
