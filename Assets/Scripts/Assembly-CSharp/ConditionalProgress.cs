using System.Collections.Generic;

public class ConditionalProgress
{
	private List<uint> metIds;

	public List<uint> MetIds
	{
		get
		{
			return metIds;
		}
	}

	public ConditionalProgress()
		: this(new List<uint>())
	{
	}

	public ConditionalProgress(List<uint> metIds)
	{
		this.metIds = metIds;
	}

	public override string ToString()
	{
		string text = "[ConditionalProgress (metIds=";
		foreach (uint metId in metIds)
		{
			text = text + metId + ", ";
		}
		return text + ")]";
	}
}
