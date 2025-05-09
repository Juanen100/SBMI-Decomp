using System.Collections.Generic;

public class MicroEventMatcher : Matcher
{
	public const string DEFINITION_ID = "micro_event_id";

	public static MicroEventMatcher FromDict(Dictionary<string, object> dict)
	{
		MicroEventMatcher microEventMatcher = new MicroEventMatcher();
		microEventMatcher.RegisterProperty("micro_event_id", dict);
		return microEventMatcher;
	}

	public override string DescribeSubject(Game game)
	{
		if (game == null)
		{
			return "did " + GetTarget("micro_event_id");
		}
		uint nDID = uint.Parse(GetTarget("micro_event_id"));
		return game.microEventManager.GetMicroEventData((int)nDID).m_sName;
	}
}
