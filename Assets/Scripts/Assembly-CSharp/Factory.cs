using System.Collections.Generic;

public class Factory<Key, Base>
{
	private Dictionary<Key, Ctor<Base>> ctors;

	public void Register(Key key, Ctor<Base> ctor)
	{
	}

	public Base Create(Key key)
	{
		return default(Base);
	}

	public Base Create(Key key, Identity id)
	{
		return default(Base);
	}

	public void Reset()
	{
	}
}
