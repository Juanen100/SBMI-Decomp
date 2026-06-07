public class NotOperator : IOperator
{
	public const string DESCRIPTION = "NOT";

	public override string ToString()
	{
		return null;
	}

	public ConditionResult Operate(ConditionResult left, ConditionResult right)
	{
		return default(ConditionResult);
	}
}
