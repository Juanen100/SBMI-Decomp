#define ASSERTS_ON
using System.Collections.Generic;

public class DumbCondition : BaseCondition
{
	public const string TRIGGER_TYPE = "DumbCondition";

	private const string STATUS = "status";

	public static readonly Trigger PASS_TRIGGER = new Trigger("DumbCondition", new Dictionary<string, object> { 
	{
		"status",
		ConditionResult.PASS
	} }, 0uL);

	public static readonly Trigger FAIL_TRIGGER = new Trigger("DumbCondition", new Dictionary<string, object> { 
	{
		"status",
		ConditionResult.FAIL
	} }, 0uL);

	public DumbCondition(uint id)
	{
		Initialize(id, 1u, new List<string> { "DumbCondition" }, new List<uint>());
	}

	public override string Description(Game game)
	{
		return "Dumb condition";
	}

	public override void Evaluate(ConditionState state, Game game, ITrigger trigger)
	{
		if (IsTypeApplicable(trigger))
		{
			if (!trigger.Data.ContainsKey("status"))
			{
				TFUtils.Assert(trigger.Data.ContainsKey("status"), "Found a DumbCondition Trigger, but the Data was not formatted as expected (could not find the 'status' field). Data=" + TFUtils.DebugDictToString(trigger.Data));
			}
			state.SelfExam = (ConditionResult)(int)trigger.Data["status"];
		}
		else
		{
			state.SelfExam = ConditionResult.UNDECIDED;
		}
	}
}
