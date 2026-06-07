using System.Collections.Generic;

public class TriggerableMixin
{
	public delegate void AddDataCallback(ref Dictionary<string, object> data);

	public ITrigger BuildTrigger(string type, AddDataCallback addMoreDataCallback, Identity target = null, Identity dropID = null)
	{
		return null;
	}
}
