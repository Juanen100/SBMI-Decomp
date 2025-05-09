using System.Collections.Generic;

public class TriggerableMixin
{
	public delegate void AddDataCallback(ref Dictionary<string, object> data);

	public ITrigger BuildTrigger(string type, AddDataCallback addMoreDataCallback, Identity target = null, Identity dropID = null)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		addMoreDataCallback(ref data);
		return new Trigger(type, data, TFUtils.EpochTime(), target, dropID);
	}
}
