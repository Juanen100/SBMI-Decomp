using System.Collections.Generic;

public class ConditionTree : LoadableCondition
{
	public const string LOAD_TOKEN = "tree";

	private const string DESCRIPTION = "ConditionTree";

	private const int LEFT = 0;

	private const int RIGHT = 1;

	private IOperator op;

	private ICondition left;

	private ICondition right;

	public IOperator Operator
	{
		get
		{
			return null;
		}
	}

	public ICondition Left
	{
		get
		{
			return null;
		}
	}

	public ICondition Right
	{
		get
		{
			return null;
		}
	}

	private ConditionTree()
	{
	}

	public ConditionTree(uint id, ICondition left, IOperator op, ICondition right)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public static ConditionTree FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public static ConditionTree Create(ICondition first, IOperator op, ICondition last)
	{
		return null;
	}

	protected void Parse(Dictionary<string, object> loadedData, string loadToken, ICondition left, IOperator op, ICondition right)
	{
	}

	private void Initialize(uint id, string loadToken, ICondition left, IOperator op, ICondition right)
	{
	}

	private void InitializePartial(ICondition left, IOperator op, ICondition right)
	{
	}

	public override string Description(Game game)
	{
		return null;
	}

	public override void FillSubstates(ref List<ConditionState> substates)
	{
	}

	public override uint FindNextId(uint floor)
	{
		return 0u;
	}

	public override void Evaluate(ConditionState state, Game game, ITrigger trigger)
	{
	}

	private static List<string> BubbleTypes(ICondition left, ICondition right)
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}
}
