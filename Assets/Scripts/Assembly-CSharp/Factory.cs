#define ASSERTS_ON
using System.Collections.Generic;

public class Factory<Key, Base>
{
	private Dictionary<Key, Ctor<Base>> ctors = new Dictionary<Key, Ctor<Base>>();

	public void Register(Key key, Ctor<Base> ctor)
	{
		ctors.Add(key, ctor);
	}

	public Base Create(Key key)
	{
		TFUtils.Assert(ctors.ContainsKey(key), string.Format("Missing Factory Item: {0}", key));
		return ctors[key].Create();
	}

	public Base Create(Key key, Identity id)
	{
		TFUtils.Assert(ctors.ContainsKey(key), string.Format("Missing Factory Item: {0}", key));
		return ctors[key].Create(id);
	}

	public void Reset()
	{
		ctors.Clear();
	}
}
