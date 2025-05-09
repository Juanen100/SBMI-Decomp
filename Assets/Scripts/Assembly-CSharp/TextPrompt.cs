using System;
using System.Collections.Generic;

public class TextPrompt : SessionActionDefinition
{
	public enum Anchor
	{
		Top = 0,
		Center = 1,
		Bottom = 2
	}

	public const string TYPE = "text_prompt";

	private const string POSITION = "anchor";

	private const string TEXT = "text";

	private TextPromptSpawn spawnTemplate = new TextPromptSpawn();

	private Anchor position;

	private string text;

	public Anchor Position
	{
		get
		{
			return position;
		}
	}

	public static TextPrompt Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		TextPrompt textPrompt = new TextPrompt();
		textPrompt.Parse(data, id, startConditions, originatedFromQuest);
		return textPrompt;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		this.text = Language.Get(TFUtils.LoadString(data, "text"));
		string text = TFUtils.LoadString(data, "anchor");
		bool flag = false;
		foreach (int value in Enum.GetValues(typeof(Anchor)))
		{
			if (text.ToLower() == ((Anchor)value).ToString().ToLower())
			{
				position = (Anchor)value;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			position = Anchor.Bottom;
			string[] names = Enum.GetNames(typeof(Anchor));
			string arg = string.Join(", ", names).ToLower();
			TFUtils.ErrorLog(string.Format("Error parsing TextPrompt SessionAction. Could not parse value({0}) for key({1}).\nValid types are: {2}", text, "anchor", arg));
		}
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["anchor"] = position;
		dictionary["text"] = text;
		return dictionary;
	}

	public void Handle(Session session, SessionActionTracker action, SBGUIScreen containingScreen)
	{
		if (action.Status == SessionActionTracker.StatusCode.REQUESTED)
		{
			containingScreen.UsedInSessionAction = true;
			spawnTemplate.Spawn(session.TheGame, action, containingScreen, text, position);
		}
	}

	public override string ToString()
	{
		return string.Concat(base.ToString(), "TextPrompt:(position=", position, "text=", text, ")");
	}
}
