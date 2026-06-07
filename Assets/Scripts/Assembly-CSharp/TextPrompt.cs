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

	private TextPromptSpawn spawnTemplate;

	private Anchor position;

	private string text;

	public Anchor Position
	{
		get
		{
			return default(Anchor);
		}
	}

	public static TextPrompt Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		return null;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public void Handle(Session session, SessionActionTracker action, SBGUIScreen containingScreen)
	{
	}

	public override string ToString()
	{
		return null;
	}
}
