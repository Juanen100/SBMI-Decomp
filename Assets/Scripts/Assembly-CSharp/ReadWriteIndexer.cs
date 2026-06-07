using System.Collections.Generic;

public class ReadWriteIndexer
{
	private Dictionary<string, object> properties;

	public object Item
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public ReadWriteIndexer(Dictionary<string, object> properties)
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

	public void Remove(string property)
	{
	}
}
