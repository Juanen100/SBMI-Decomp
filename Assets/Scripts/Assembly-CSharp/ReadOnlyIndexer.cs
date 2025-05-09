using System.Collections.Generic;

public class ReadOnlyIndexer
{
	private Dictionary<string, object> properties;

	public object this[string property]
	{
		get
		{
			return properties[property];
		}
	}

	public ReadOnlyIndexer(Dictionary<string, object> properties)
	{
		this.properties = properties;
	}

	public bool ContainsKey(string property)
	{
		return properties.ContainsKey(property);
	}

	public bool TryGetValue(string property, out object value)
	{
		value = null;
		properties.TryGetValue(property, out value);
		return value != null;
	}
}
