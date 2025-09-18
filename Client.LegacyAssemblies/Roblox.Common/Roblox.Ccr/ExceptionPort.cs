using System;
using Microsoft.Ccr.Core;

namespace Roblox.Ccr;

public class ExceptionPort<T> : PortSet<T, Exception>
{
	public static implicit operator T(ExceptionPort<T> port)
	{
		Exception ex = (Exception)port.P1.Test();
		if (ex != null)
		{
			throw ex;
		}
		return (T)port.P0.Test();
	}

	public void Check()
	{
		Exception ex = (Exception)base.P1.Test();
		if (ex != null)
		{
			throw ex;
		}
	}
}
