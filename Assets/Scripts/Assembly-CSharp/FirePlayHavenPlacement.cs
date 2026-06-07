using System.Collections.Generic;

public class FirePlayHavenPlacement : SessionActionDefinition
{
	public const string TYPE = "call_playhaven";

	private const string PLACEMENT_FIELD = "placement";

	private string placement;

	public static FirePlayHavenPlacement Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
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

	public override void PreActivate(Game game, SessionActionTracker tracker)
	{
	}

	public override string ToString()
	{
		return null;
	}
}
