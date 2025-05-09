#define ASSERTS_ON
using System;
using System.Collections.Generic;
using System.Diagnostics;

public abstract class MatchableCondition : LoadableCondition
{
	private IList<IMatcher> matchers;

	private int simulatedExistsID;

	public IList<IMatcher> Matchers
	{
		get
		{
			return matchers;
		}
	}

	protected MatchableCondition()
	{
	}

	protected MatchableCondition(uint id, uint count, string loadToken, IList<string> relevantTypes, IList<IMatcher> matchers, IList<uint> prerequisiteConditions, int SimulatedExistsID = -1)
	{
		Initialize(id, count, loadToken, relevantTypes, prerequisiteConditions);
		Initialize(matchers, SimulatedExistsID);
	}

	protected void Parse(Dictionary<string, object> loadedData, string loadToken, IList<string> relevantTypes, IList<IMatcher> matchers, int SimulatedExistsID = -1)
	{
		Parse(loadedData, loadToken, relevantTypes);
		Initialize(matchers, SimulatedExistsID);
	}

	private void Initialize(IList<IMatcher> matchers, int SimulatedExistsID = -1)
	{
		this.matchers = matchers;
		simulatedExistsID = SimulatedExistsID;
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		foreach (IMatcher matcher in matchers)
		{
			foreach (string key in matcher.Keys)
			{
				if (matcher.IsRequired(key))
				{
					dictionary[key] = matcher.GetTarget(key);
				}
			}
		}
		return dictionary;
	}

	[Conditional("DEBUG")]
	private void VerifyMatchable()
	{
		int num = 0;
		foreach (IMatcher matcher in matchers)
		{
			foreach (string key in matcher.Keys)
			{
				if (matcher.IsRequired(key))
				{
					num++;
				}
			}
		}
		TFUtils.Assert(num > 0, "No conditions found to match for Condition(" + ToString() + ")");
	}

	public override void Evaluate(ConditionState state, Game game, ITrigger trigger)
	{
		if (!IsTypeApplicable(trigger))
		{
			if (state.SelfExam == ConditionResult.UNINITIALIZED)
			{
				state.SelfExam = ConditionResult.UNDECIDED;
			}
			return;
		}
		uint num = 1u;
		foreach (IMatcher matcher in matchers)
		{
			num *= matcher.MatchAmount(game, trigger.Data);
		}
		state.Count += num;
		uint num2 = Count;
		int num3 = 0;
		if (simulatedExistsID > 0)
		{
			num2++;
			if (game.simulation.FindSimulated(simulatedExistsID) != null)
			{
				num3++;
			}
		}
		if (state.Count + num3 >= num2)
		{
			state.SelfExam = ConditionResult.PASS;
		}
		else
		{
			state.SelfExam = ConditionResult.UNDECIDED;
		}
	}

	public override string ToString()
	{
		string text = string.Empty;
		foreach (IMatcher matcher in matchers)
		{
			text = string.Concat(text, matcher, ",");
		}
		return "MatchableCondition:(, matchers=[" + text + "], " + base.ToString() + ")";
	}

	public override string Description(Game game)
	{
		throw new NotImplementedException();
	}
}
