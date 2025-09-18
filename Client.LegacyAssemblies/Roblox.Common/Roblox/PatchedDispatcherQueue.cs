using System.Reflection;
using Microsoft.Ccr.Core;

namespace Roblox;

public class PatchedDispatcherQueue : DispatcherQueue
{
	private static FieldInfo _Next = typeof(TaskCommon).GetField("_next", BindingFlags.Instance | BindingFlags.NonPublic);

	private static FieldInfo _Previous = typeof(TaskCommon).GetField("_previous", BindingFlags.Instance | BindingFlags.NonPublic);

	public PatchedDispatcherQueue(string name, Dispatcher dispatcher)
		: base(name, dispatcher)
	{
	}

	public override bool TryDequeue(out ITask task)
	{
		bool result = base.TryDequeue(out task);
		if (result && task is TaskCommon taskCommon)
		{
			_Next.SetValue(taskCommon, null);
			_Previous.SetValue(taskCommon, null);
		}
		return result;
	}
}
