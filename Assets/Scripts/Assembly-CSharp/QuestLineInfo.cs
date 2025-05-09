using System.Collections.Generic;

public class QuestLineInfo
{
	private const string NAME = "name";

	private const string ICON = "icon";

	private const string HAS_PROGRESS = "has_progress";

	private string name;

	private string icon;

	private bool hasProgress;

	public string Name
	{
		get
		{
			return name;
		}
	}

	public string Icon
	{
		get
		{
			return icon;
		}
	}

	public bool HasProgress
	{
		get
		{
			return hasProgress;
		}
	}

	public static QuestLineInfo FromDict(Dictionary<string, object> data)
	{
		QuestLineInfo questLineInfo = new QuestLineInfo();
		questLineInfo.name = (string)data["name"];
		questLineInfo.icon = (string)data["icon"];
		questLineInfo.hasProgress = !data.ContainsKey("has_progress") || TFUtils.LoadBool(data, "has_progress");
		return questLineInfo;
	}

	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["name"] = name;
		dictionary["icon"] = icon;
		dictionary["has_progress"] = hasProgress;
		return dictionary;
	}
}
