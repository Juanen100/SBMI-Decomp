using System.Collections.Generic;

public class Identity
{
	public class Equality : IEqualityComparer<Identity>
	{
		public bool Equals(Identity lhs, Identity rhs)
		{
			return false;
		}

		public int GetHashCode(Identity lhs)
		{
			return 0;
		}
	}

	private string value;

	public Identity()
	{
	}

	public Identity(string value)
	{
	}

	public Identity(Reader reader)
	{
	}

	public void Unserialize(Reader reader)
	{
	}

	public void Serialize(Writer writer)
	{
	}

	public string Describe()
	{
		return null;
	}

	public static Identity Null()
	{
		return null;
	}

	public override bool Equals(object obj)
	{
		return false;
	}

	public override int GetHashCode()
	{
		return 0;
	}

	public override string ToString()
	{
		return null;
	}
}
