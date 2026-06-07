public class OrOperator : IOperator
{
	public const string DESCRIPTION = "OR";

	public override string ToString()
	{
		return null;
	}

	public ConditionResult Operate(ConditionResult left, ConditionResult right)
	{
		return default(ConditionResult);
	}
}
