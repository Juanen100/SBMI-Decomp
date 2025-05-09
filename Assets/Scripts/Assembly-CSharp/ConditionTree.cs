#define ASSERTS_ON
using System;
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
			return op;
		}
	}

	public ICondition Left
	{
		get
		{
			return left;
		}
	}

	public ICondition Right
	{
		get
		{
			return right;
		}
	}

	private ConditionTree()
	{
	}

	public ConditionTree(uint id, ICondition left, IOperator op, ICondition right)
	{
		Initialize(id, "tree", left, op, right);
	}

	public override Dictionary<string, object> ToDict()
	{
		LoadableCondition loadableCondition = left as LoadableCondition;
		LoadableCondition loadableCondition2 = right as LoadableCondition;
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["operator"] = op.ToString();
		dictionary["loperand"] = ((loadableCondition == null) ? null : loadableCondition.ToDict());
		dictionary["roperand"] = ((loadableCondition2 == null) ? null : loadableCondition2.ToDict());
		return dictionary;
	}

	public static ConditionTree FromDict(Dictionary<string, object> data)
	{
		TFUtils.Assert(data.Count != 0, "This ConditionTree has no conditions to evaluate.");
		string opString = (string)data["operator"];
		IOperator obj = OperatorFactory.StringToOperator(opString);
		ICondition condition = null;
		ICondition condition2 = null;
		if (data.ContainsKey("loperand"))
		{
			condition = ConditionFactory.FromDict((Dictionary<string, object>)data["loperand"]);
		}
		if (data.ContainsKey("roperand"))
		{
			condition2 = ConditionFactory.FromDict((Dictionary<string, object>)data["roperand"]);
		}
		ConditionTree conditionTree = new ConditionTree();
		conditionTree.Parse(data, "tree", condition, obj, condition2);
		return conditionTree;
	}

	public static ConditionTree Create(ICondition first, IOperator op, ICondition last)
	{
		return new ConditionTree(first.FindNextId(last.FindNextId()), first, op, last);
	}

	protected void Parse(Dictionary<string, object> loadedData, string loadToken, ICondition left, IOperator op, ICondition right)
	{
		Parse(loadedData, loadToken, BubbleTypes(left, right));
		InitializePartial(left, op, right);
	}

	private void Initialize(uint id, string loadToken, ICondition left, IOperator op, ICondition right)
	{
		Initialize(id, 1u, loadToken, BubbleTypes(left, right), new List<uint>());
		InitializePartial(left, op, right);
	}

	private void InitializePartial(ICondition left, IOperator op, ICondition right)
	{
		this.left = left;
		this.right = right;
		this.op = op;
	}

	public override string Description(Game game)
	{
		return left.Description(game) + " " + op.ToString().ToLower() + Environment.NewLine + right.Description(game);
	}

	public override void FillSubstates(ref List<ConditionState> substates)
	{
		substates.Insert(0, new ConditionState(left));
		substates.Insert(1, new ConditionState(right));
	}

	public override uint FindNextId(uint floor)
	{
		return right.FindNextId(left.FindNextId(base.FindNextId(floor)));
	}

	public override void Evaluate(ConditionState state, Game game, ITrigger trigger)
	{
		ConditionState conditionState = state.SubStates[0];
		ConditionState conditionState2 = state.SubStates[1];
		state.SelfExam = op.Operate(conditionState.Examine(), conditionState2.Examine());
	}

	private static List<string> BubbleTypes(ICondition left, ICondition right)
	{
		if (left.RelevantTypes == null && right.RelevantTypes == null)
		{
			return null;
		}
		List<string> list = new List<string>();
		if (left.RelevantTypes != null)
		{
			list.AddRange(left.RelevantTypes);
		}
		if (right.RelevantTypes != null)
		{
			list.AddRange(right.RelevantTypes);
		}
		return list;
	}

	public override string ToString()
	{
		return string.Concat(base.ToString(), "ConditionTree:( Operator=", op, ", Left=", left.ToString(), ", Right=", right.ToString(), ")");
	}
}
