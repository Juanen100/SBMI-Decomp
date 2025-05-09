using System.Collections.Generic;

public class CompleteDialogCondition : MatchableCondition
{
	public CompleteDialogCondition(uint id, uint targetDialogId)
		: base(id, 1u, "NOT LOADABLE", new List<string> { "dialogtrigger_character", "dialogtrigger_quest_start", "dialogtrigger_quest_complete", "dialogtrigger_booty_quest_complete", "dialogtrigger_level_up" }, new List<IMatcher>
		{
			new DialogMatcher(targetDialogId)
		}, new List<uint>())
	{
	}
}
