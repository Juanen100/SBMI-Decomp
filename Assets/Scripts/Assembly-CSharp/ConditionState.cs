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
			return null;
		}
	}

	public ConditionState(ICondition definition)
	{
	}

	public T GetDefinition<T>()
	{
		return default(T);
	}

	public void Hydrate(ConditionalProgress progress, Game game, List<uint> metIDs = null)
	{
	}

	public ConditionalProgress Dehydrate()
	{
		return null;
	}

	public static ConditionalProgress DehydrateChunks(List<ConditionState> list)
	{
		return null;
	}

	protected virtual void GetMetIds(ref List<uint> completedIds)
	{
	}

	public virtual ConditionResult Examine()
	{
		return default(ConditionResult);
	}

	public bool Recalculate(Game game, ITrigger trigger, List<uint> metIDs = null)
	{
		return false;
	}

	public List<ConditionDescription> Describe(Game game)
	{
		return null;
	}

	protected virtual ConditionDescription DescribeMe(Game game)
	{
		return default(ConditionDescription);
	}

	public override string ToString()
	{
		return null;
	}
}
