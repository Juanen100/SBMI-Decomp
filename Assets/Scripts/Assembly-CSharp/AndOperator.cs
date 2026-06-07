public class AndOperator : IOperator
{
	public const string DESCRIPTION = "AND";

	public override string ToString()
	{
		return null;
	}

	public ConditionResult Operate(ConditionResult left, ConditionResult right)
	{
		return default(ConditionResult);
	}
}
