using System;

namespace Roblox.Common.Mail.Pop3;

public class Pop3Exception : ApplicationException
{
	public Pop3Exception()
	{
	}

	public Pop3Exception(string ErrorMessage)
		: base(ErrorMessage)
	{
	}
}
