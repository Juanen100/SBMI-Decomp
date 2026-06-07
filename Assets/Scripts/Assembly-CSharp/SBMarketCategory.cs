using System.Collections.Generic;

public class SBMarketCategory : SBTabCategory
{
	private string name;

	private string type;

	private string texture;

	private string deltaDNAName;

	private int[] dids;

	private string label;

	private int microEventDID;

	private bool microEventOnly;

	public string Name
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string DeltaDNAName
	{
		get
		{
			return null;
		}
	}

	public string Type
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string Texture
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public int MicroEventDID
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public bool MicroEventOnly
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public string Label
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public int[] Dids
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public SBMarketCategory(Dictionary<string, object> cat)
	{
	}

	public override string ToString()
	{
		return null;
	}
}
