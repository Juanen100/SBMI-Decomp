using System.Collections.Generic;

public class CallbackQueue
{
	protected class CallbackEntry
	{
		public TFServer.JsonResponseHandler handler;

		public Dictionary<string, object> data;

		public object customData;

		public CallbackEntry(TFServer.JsonResponseHandler handler, Dictionary<string, object> data, object userData)
		{
		}
	}

	protected class DelayedCallbackEntry : CallbackEntry
	{
		public float scheduledTime;

		public DelayedCallbackEntry(TFServer.JsonResponseHandler handler, Dictionary<string, object> data, float scheduledTime, object userData)
			: base(null, null, null)
		{
		}
	}

	protected List<CallbackEntry> callbackEntries;

	protected List<DelayedCallbackEntry> delayedCallbackEntries;

	public TFServer.JsonResponseHandler AsyncCallback(TFServer.JsonResponseHandler handler)
	{
		return null;
	}

	public void QueueCallback(TFServer.JsonResponseHandler handler, Dictionary<string, object> data, object userData)
	{
	}

	private bool CallbackReady(DelayedCallbackEntry entry)
	{
		return false;
	}

	public void ProcessQueue()
	{
	}

	public void QueueCallback(TFServer.JsonResponseHandler handler, Dictionary<string, object> data, float delay, object userData)
	{
	}
}
