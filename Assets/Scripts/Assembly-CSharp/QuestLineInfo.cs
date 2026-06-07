using System.Collections.Generic;

public class QuestLineInfo
{
	private string name;

	private string icon;

	private bool hasProgress;

	private const string NAME = "name";

	private const string ICON = "icon";

	private const string HAS_PROGRESS = "has_progress";

	public string Name
	{
		get
		{
			return null;
		}
	}

	public string Icon
	{
		get
		{
			return null;
		}
	}

	public bool HasProgress
	{
		get
		{
			return false;
		}
	}

	public static QuestLineInfo FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public Dictionary<string, object> ToDict()
	{
		return null;
	}
}
