#define ASSERTS_ON
using System.Collections.Generic;

public class ConditionState
{
	public ConditionResult SelfExam;

	public uint Count;

	private ICondition definition;

	private List<ConditionState> substates;

	public List<ConditionState> SubStates
	{
		get
		{
			return substates;
		}
	}

	public ConditionState(ICondition definition)
	{
		this.definition = definition;
		substates = new List<ConditionState>(2);
		this.definition.FillSubstates(ref substates);
	}

	public T GetDefinition<T>()
	{
		return (T)definition;
	}

	public void Hydrate(ConditionalProgress progress, Game game, List<uint> metIDs = null)
	{
		if (metIDs == null)
		{
			metIDs = new List<uint>();
			GetMetIds(ref metIDs);
		}
		foreach (ConditionState substate in substates)
		{
			substate.Hydrate(progress, game, metIDs);
		}
		Count = (uint)progress.MetIds.FindAll((uint id) => id == definition.Id).Count;
		if (Count >= definition.Count)
		{
			SelfExam = ConditionResult.PASS;
		}
		if (SelfExam != ConditionResult.UNINITIALIZED)
		{
			return;
		}
		int count = definition.PrerequisiteConditions.Count;
		if (count > 0)
		{
			for (int num = 0; num < count; num++)
			{
				if (!metIDs.Contains(definition.PrerequisiteConditions[num]))
				{
					SelfExam = ConditionResult.UNDECIDED;
					return;
				}
			}
		}
		definition.Evaluate(this, game, Trigger.Null);
	}

	public ConditionalProgress Dehydrate()
	{
		List<uint> completedIds = new List<uint>();
		GetMetIds(ref completedIds);
		return new ConditionalProgress(completedIds);
	}

	public static ConditionalProgress DehydrateChunks(List<ConditionState> list)
	{
		List<uint> completedIds = new List<uint>();
		foreach (ConditionState item in list)
		{
			item.GetMetIds(ref completedIds);
		}
		return new ConditionalProgress(completedIds);
	}

	protected virtual void GetMetIds(ref List<uint> completedIds)
	{
		foreach (ConditionState substate in substates)
		{
			substate.GetMetIds(ref completedIds);
		}
		if (SelfExam == ConditionResult.PASS || SelfExam == ConditionResult.UNDECIDED)
		{
			for (int i = 0; i < Count; i++)
			{
				completedIds.Add(definition.Id);
			}
		}
	}

	public virtual ConditionResult Examine()
	{
		return SelfExam;
	}

	public bool Recalculate(Game game, ITrigger trigger, List<uint> metIDs = null)
	{
		if (metIDs == null)
		{
			metIDs = new List<uint>();
			GetMetIds(ref metIDs);
		}
		int count = definition.PrerequisiteConditions.Count;
		if (count > 0)
		{
			for (int i = 0; i < count; i++)
			{
				if (!metIDs.Contains(definition.PrerequisiteConditions[i]))
				{
					return false;
				}
			}
		}
		TFUtils.Assert(trigger != null, "Given trigger cannot be null. Use Trigger.Null instead.");
		if (SelfExam != ConditionResult.UNINITIALIZED && SelfExam != ConditionResult.UNDECIDED)
		{
			return false;
		}
		bool flag = false;
		foreach (ConditionState substate in substates)
		{
			flag = flag || substate.Recalculate(game, trigger, metIDs);
		}
		bool flag2 = SelfExam == ConditionResult.UNINITIALIZED;
		uint count2 = Count;
		definition.Evaluate(this, game, trigger);
		TFUtils.Assert(SelfExam != ConditionResult.UNINITIALIZED, "A ConditionState's Definition returned UNINITIALIZED as a result of the Evaulate call. Fix that definition so it doesn't do that.");
		count2 = Count - count2;
		if (SelfExam == ConditionResult.UNDECIDED && !flag2 && !flag && count2 == 0)
		{
			return false;
		}
		return true;
	}

	public List<ConditionDescription> Describe(Game game)
	{
		List<ConditionDescription> list = new List<ConditionDescription>();
		list.Add(DescribeMe(game));
		return list;
	}

	protected virtual ConditionDescription DescribeMe(Game game)
	{
		return new ConditionDescription
		{
			Id = definition.Id,
			OccuranceCount = Count,
			OccurancesRequired = definition.Count,
			IsPassed = (Examine() == ConditionResult.PASS),
			Description = definition.Description(game)
		};
	}

	public override string ToString()
	{
		return string.Concat("ConditionState:(SelfExam=", SelfExam, ", Definition=", definition, ", Count=", Count, ", Substates=", substates, ")");
	}
}
