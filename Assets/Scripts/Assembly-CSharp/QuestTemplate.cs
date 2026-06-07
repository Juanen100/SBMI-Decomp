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
			return 0u;
		}
	}

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

	public Dictionary<string, object> TemplateData
	{
		get
		{
			return null;
		}
	}

	public static QuestTemplate FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	private void AddRandomTemplate(uint id, string name, string icon, Dictionary<string, object> data)
	{
	}
}
