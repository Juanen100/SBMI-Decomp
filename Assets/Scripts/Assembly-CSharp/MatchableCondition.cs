using System.Collections.Generic;

public abstract class MatchableCondition : LoadableCondition
{
	private IList<IMatcher> matchers;

	private int simulatedExistsID;

	public IList<IMatcher> Matchers
	{
		get
		{
			return null;
		}
	}

	protected MatchableCondition()
	{
	}

	protected MatchableCondition(uint id, uint count, string loadToken, IList<string> relevantTypes, IList<IMatcher> matchers, IList<uint> prerequisiteConditions, int SimulatedExistsID = -1)
	{
	}

	protected void Parse(Dictionary<string, object> loadedData, string loadToken, IList<string> relevantTypes, IList<IMatcher> matchers, int SimulatedExistsID = -1)
	{
	}

	private void Initialize(IList<IMatcher> matchers, int SimulatedExistsID = -1)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	private void VerifyMatchable()
	{
	}

	public override void Evaluate(ConditionState state, Game game, ITrigger trigger)
	{
	}

	public override string ToString()
	{
		return null;
	}

	public override string Description(Game game)
	{
		return null;
	}
}
