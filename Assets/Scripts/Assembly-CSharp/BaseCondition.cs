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
			return id;
		}
	}

	public uint Count
	{
		get
		{
			return count;
		}
	}

	public IList<uint> PrerequisiteConditions
	{
		get
		{
			return prerequisiteConditions;
		}
	}

	public ICollection<string> RelevantTypes
	{
		get
		{
			return relevantTypes;
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
		this.id = id;
		this.count = count;
		this.relevantTypes = relevantTypes;
		this.prerequisiteConditions = prerequisiteConditions;
	}

	public virtual uint FindNextId()
	{
		return FindNextId(id);
	}

	public virtual uint FindNextId(uint floor)
	{
		return (floor <= id) ? (id + 1) : floor;
	}

	public virtual void FillSubstates(ref List<ConditionState> substates)
	{
	}

	public abstract void Evaluate(ConditionState state, Game game, ITrigger trigger);

	protected bool IsTypeApplicable(ITrigger trigger)
	{
		return relevantTypes == null || relevantTypes.Contains(trigger.Type);
	}

	public override string ToString()
	{
		string text = string.Empty;
		if (relevantTypes != null)
		{
			text += ", relevantTypes=[";
			foreach (string relevantType in relevantTypes)
			{
				text = text + relevantType + ",";
			}
			text += "]";
		}
		return "BaseCondition:(id=" + id + ", count=" + count + text + ")";
	}
}
