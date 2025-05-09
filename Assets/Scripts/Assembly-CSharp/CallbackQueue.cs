using System;
using System.Collections.Generic;
using UnityEngine;

public class CallbackQueue
{
	protected class CallbackEntry
	{
		public TFServer.JsonResponseHandler handler;

		public Dictionary<string, object> data;

		public object customData;

		public CallbackEntry(TFServer.JsonResponseHandler handler, Dictionary<string, object> data, object userData)
		{
			this.handler = handler;
			this.data = data;
			customData = userData;
		}
	}

	protected class DelayedCallbackEntry : CallbackEntry
	{
		public float scheduledTime;

		public DelayedCallbackEntry(TFServer.JsonResponseHandler handler, Dictionary<string, object> data, float scheduledTime, object userData)
			: base(handler, data, userData)
		{
			this.scheduledTime = scheduledTime;
		}
	}

	protected List<CallbackEntry> callbackEntries = new List<CallbackEntry>();

	protected List<DelayedCallbackEntry> delayedCallbackEntries = new List<DelayedCallbackEntry>();

	public TFServer.JsonResponseHandler AsyncCallback(TFServer.JsonResponseHandler handler)
	{
		return delegate(Dictionary<string, object> response, object userData)
		{
			QueueCallback(handler, response, userData);
		};
	}

	public void QueueCallback(TFServer.JsonResponseHandler handler, Dictionary<string, object> data, object userData)
	{
		CallbackEntry item = new CallbackEntry(handler, data, userData);
		lock (callbackEntries)
		{
			callbackEntries.Add(item);
		}
	}

	private bool CallbackReady(DelayedCallbackEntry entry)
	{
		return entry.scheduledTime <= Time.time;
	}

	public void ProcessQueue()
	{
		List<CallbackEntry> list;
		lock (callbackEntries)
		{
			list = new List<CallbackEntry>(callbackEntries);
			callbackEntries.Clear();
		}
		lock (delayedCallbackEntries)
		{
			List<DelayedCallbackEntry> list2 = delayedCallbackEntries.FindAll(CallbackReady);
			foreach (DelayedCallbackEntry item in list2)
			{
				list.Add(item);
				delayedCallbackEntries.Remove(item);
			}
		}
		foreach (CallbackEntry item2 in list)
		{
			try
			{
				item2.handler(item2.data, item2.customData);
			}
			catch (Exception ex)
			{
				TFUtils.ErrorLog("Callback processing failure: " + ex);
				TFUtils.ErrorLog(string.Concat("Failed to process callback for ", item2.handler, " with data ", item2.data));
			}
		}
	}

	public void QueueCallback(TFServer.JsonResponseHandler handler, Dictionary<string, object> data, float delay, object userData)
	{
		DelayedCallbackEntry item = new DelayedCallbackEntry(handler, data, Time.time + delay, userData);
		lock (delayedCallbackEntries)
		{
			delayedCallbackEntries.Add(item);
		}
	}
}
