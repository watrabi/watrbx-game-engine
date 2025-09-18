using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace Roblox.Grid.Rcc;

[GeneratedCode("wsdl", "4.8.3928.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class GetExpirationCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public double Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (double)results[0];
		}
	}

	internal GetExpirationCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
