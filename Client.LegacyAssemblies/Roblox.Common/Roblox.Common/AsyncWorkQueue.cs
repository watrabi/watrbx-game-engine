using System;
using Microsoft.Ccr.Core;

namespace Roblox.Common;

public class AsyncWorkQueue<T>
{
	internal class WorkItem
	{
		private Action _CompletionTask;

		private T _Item;

		private SuccessFailurePort _Result;

		internal Action CompletionTask => _CompletionTask;

		internal T Item => _Item;

		internal SuccessFailurePort Result => _Result;

		internal WorkItem(T item)
		{
			_Item = item;
		}

		internal WorkItem(T item, Action completionTask)
		{
			_CompletionTask = completionTask;
			_Item = item;
		}

		internal WorkItem(T item, SuccessFailurePort result)
		{
			_Item = item;
			_Result = result;
		}
	}

	public delegate void AsyncItemHandler(T item, SuccessFailurePort result);

	private DispatcherQueue _DispatcherQueue;

	private AsyncItemHandler _ItemHandler;

	private Port<WorkItem> _QueuedItems = new Port<WorkItem>();

	public AsyncWorkQueue(DispatcherQueue dispatcherQueue, AsyncItemHandler itemHandler)
	{
		if (itemHandler == null)
		{
			throw new ApplicationException("AsyncWorkQueue initialization failed.  Valid AsyncItemHandler required.");
		}
		_DispatcherQueue = dispatcherQueue;
		_ItemHandler = itemHandler;
		Receiver<WorkItem> receiver = Arbiter.Receive(persist: true, _QueuedItems, delegate(WorkItem workItem)
		{
			DoWork(workItem);
		});
		Arbiter.Activate(_DispatcherQueue, receiver);
	}

	private void DoCompletionTask(SuccessFailurePort itemHandlerResult, Action completionTask)
	{
		Choice choice = Arbiter.Choice(itemHandlerResult, delegate
		{
			completionTask();
		}, delegate(Exception failure)
		{
			ExceptionHandler.LogException(failure);
		});
		Arbiter.Activate(_DispatcherQueue, choice);
	}

	private void DoWork(WorkItem workItem)
	{
		SuccessFailurePort result = ((workItem.Result == null) ? new SuccessFailurePort() : workItem.Result);
		_ItemHandler(workItem.Item, result);
		if (workItem.CompletionTask != null)
		{
			DoCompletionTask(result, workItem.CompletionTask);
		}
	}

	public void EnqueueWorkItem(T item)
	{
		_QueuedItems.Post(new WorkItem(item));
	}

	public void EnqueueWorkItem(T item, Action completionTask)
	{
		_QueuedItems.Post(new WorkItem(item, completionTask));
	}

	public void EnqueueWorkItem(T item, SuccessFailurePort result)
	{
		_QueuedItems.Post(new WorkItem(item, result));
	}
}
