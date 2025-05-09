using System;
using System.Collections.Generic;

public class ReadWriteIndexer
{
	private Dictionary<string, object> properties;

	public object this[string property]
	{
		get
		{
			return properties[property];
		}
		set
		{
			properties[property] = value;
		}
	}

	public ReadWriteIndexer(Dictionary<string, object> properties)
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

	public void Remove(string property)
	{
		if (!properties.ContainsKey(property))
		{
			throw new InvalidOperationException("No key: " + property);
		}
		properties.Remove(property);
	}
}
