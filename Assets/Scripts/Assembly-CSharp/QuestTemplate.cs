using System.Collections.Generic;

public class QuestTemplate
{
	private const string DID = "did";

	private const string NAME = "name";

	private const string ICON = "icon";

	private uint did;

	private string name;

	private string icon;

	private Dictionary<string, object> templateData;

	public uint Did
	{
		get
		{
			return did;
		}
	}

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

	public Dictionary<string, object> TemplateData
	{
		get
		{
			return templateData;
		}
	}

	public static QuestTemplate FromDict(Dictionary<string, object> data)
	{
		QuestTemplate questTemplate = new QuestTemplate();
		uint id = TFUtils.LoadUint(data, "did");
		string text = TFUtils.LoadString(data, "name");
		string text2 = TFUtils.LoadString(data, "icon");
		questTemplate.AddRandomTemplate(id, text, text2, data);
		TFUtils.DebugLog("Loaded Random Quest:" + questTemplate.name);
		return questTemplate;
	}

	private void AddRandomTemplate(uint id, string name, string icon, Dictionary<string, object> data)
	{
		did = id;
		this.name = name;
		this.icon = icon;
		templateData = data;
	}
}
