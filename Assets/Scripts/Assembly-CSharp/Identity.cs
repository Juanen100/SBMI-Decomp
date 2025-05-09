using System;
using System.Collections.Generic;

public class Identity
{
	public class Equality : IEqualityComparer<Identity>
	{
		public bool Equals(Identity lhs, Identity rhs)
		{
			return lhs.Equals(rhs);
		}

		public int GetHashCode(Identity lhs)
		{
			return lhs.GetHashCode();
		}
	}

	private string value;

	public Identity()
	{
		value = Guid.NewGuid().ToString();
	}

	public Identity(string value)
	{
		this.value = value;
	}

	public Identity(Reader reader)
	{
		Unserialize(reader);
	}

	public void Unserialize(Reader reader)
	{
		reader.Read(out value);
	}

	public void Serialize(Writer writer)
	{
		writer.Write(value);
	}

	public string Describe()
	{
		return value;
	}

	public static Identity Null()
	{
		return new Identity(Guid.Empty.ToString());
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		Identity identity = obj as Identity;
		if (identity == null)
		{
			return false;
		}
		return value.Equals(identity.value);
	}

	public override int GetHashCode()
	{
		return value.GetHashCode();
	}

	public override string ToString()
	{
		return "Identity(guid=" + value + ")";
	}
}
