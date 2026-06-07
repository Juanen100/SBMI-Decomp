public class DumbCondition : BaseCondition
{
	public const string TRIGGER_TYPE = "DumbCondition";

	public static readonly Trigger PASS_TRIGGER;

	public static readonly Trigger FAIL_TRIGGER;

	private const string STATUS = "status";

	public DumbCondition(uint id)
	{
	}

	public override string Description(Game game)
	{
		return null;
	}

	public override void Evaluate(ConditionState state, Game game, ITrigger trigger)
	{
	}
}
