using System.Collections.Generic;

public class MovieInfo
{
	private int identity;

	private string name;

	private string collectName;

	private string description;

	private string moviefile;

	private string texture;

	public int Did
	{
		get
		{
			return identity;
		}
	}

	public string Name
	{
		get
		{
			return name;
		}
	}

	public string CollectName
	{
		get
		{
			return collectName;
		}
	}

	public string Description
	{
		get
		{
			return description;
		}
	}

	public string MovieFile
	{
		get
		{
			return moviefile;
		}
	}

	public string MovieInfoTexture
	{
		get
		{
			return texture;
		}
	}

	public MovieInfo(Dictionary<string, object> dict)
	{
		identity = TFUtils.LoadInt(dict, "did");
		name = (string)dict["name"];
		collectName = (string)dict["collect_name"];
		description = (string)dict["description"];
		moviefile = (string)dict["movie"];
		texture = (string)dict["texture"];
	}
}
