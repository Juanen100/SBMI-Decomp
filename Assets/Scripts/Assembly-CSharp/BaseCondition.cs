using System.Collections.Generic;

public abstract class BaseCondition : ICondition
{
	private uint id;

	private uint count;

	private ICollection<string> relevantTypes;

	private IList<uint> prerequisiteConditions;

	public uint Id
	{
		get
		{
			return 0u;
		}
	}

	public uint Count
	{
		get
		{
			return 0u;
		}
	}

	public IList<uint> PrerequisiteConditions
	{
		get
		{
			return null;
		}
	}

	public ICollection<string> RelevantTypes
	{
		get
		{
			return null;
		}
	}

	public virtual bool IsExpensiveToCalculate
	{
		get
		{
			return false;
		}
	}

	public abstract string Description(Game game);

	protected void Initialize(uint id, uint count, ICollection<string> relevantTypes, IList<uint> prerequisiteConditions)
	{
	}

	public virtual uint FindNextId()
	{
		return 0u;
	}

	public virtual uint FindNextId(uint floor)
	{
		return 0u;
	}

	public virtual void FillSubstates(ref List<ConditionState> substates)
	{
	}

	public abstract void Evaluate(ConditionState state, Game game, ITrigger trigger);

	protected bool IsTypeApplicable(ITrigger trigger)
	{
		return false;
	}

	public override string ToString()
	{
		return null;
	}
}
