using System.Collections.Generic;

public interface ICondition
{
	uint Id { get; }

	uint Count { get; }

	ICollection<string> RelevantTypes { get; }

	IList<uint> PrerequisiteConditions { get; }

	bool IsExpensiveToCalculate { get; }

	string Description(Game game);

	uint FindNextId();

	uint FindNextId(uint floor);

	void FillSubstates(ref List<ConditionState> substates);

	void Evaluate(ConditionState state, Game game, ITrigger trigger);

	new string ToString();
}
