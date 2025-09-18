using System;
using Microsoft.Ccr.Core;

namespace Roblox;

public class Interleaver
{
	private readonly Port<Action> exclusive = new Port<Action>();

	private readonly Port<Action> concurrent = new Port<Action>();

	public void DoExclusive(Action action)
	{
		exclusive.Post(action);
	}

	public void DoConcurrent(Action action)
	{
		concurrent.Post(action);
	}

	public Interleaver()
	{
		CcrService.Singleton.Activate<Interleave>(Arbiter.Interleave(new TeardownReceiverGroup(), new ExclusiveReceiverGroup(Arbiter.Receive(persist: true, exclusive, delegate(Action action)
		{
			action();
		})), new ConcurrentReceiverGroup(Arbiter.Receive(persist: true, concurrent, delegate(Action action)
		{
			action();
		}))));
	}
}
