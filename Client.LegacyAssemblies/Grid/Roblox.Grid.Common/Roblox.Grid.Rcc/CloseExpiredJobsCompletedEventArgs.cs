using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace Roblox.Grid.Rcc;

[GeneratedCode("wsdl", "4.8.3928.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class CloseExpiredJobsCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public int Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (int)results[0];
		}
	}

	internal CloseExpiredJobsCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
