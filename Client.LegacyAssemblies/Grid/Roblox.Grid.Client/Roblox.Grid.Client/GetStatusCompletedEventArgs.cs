using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace Roblox.Grid.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class GetStatusCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public Status Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (Status)results[0];
		}
	}

	public GetStatusCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
