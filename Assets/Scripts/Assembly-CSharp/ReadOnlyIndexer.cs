using System.Collections.Generic;

public class ReadOnlyIndexer
{
	private Dictionary<string, object> properties;

	public object Item
	{
		get
		{
			return null;
		}
	}

	public ReadOnlyIndexer(Dictionary<string, object> properties)
	{
	}

	public bool ContainsKey(string property)
	{
		return false;
	}

	public bool TryGetValue(string property, out object value)
	{
		value = null;
		return false;
	}
}
