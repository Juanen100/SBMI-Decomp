#define ASSERTS_ON
public class OrOperator : IOperator
{
	public const string DESCRIPTION = "OR";

	public override string ToString()
	{
		return "OR";
	}

	public ConditionResult Operate(ConditionResult left, ConditionResult right)
	{
		TFUtils.Assert(left != ConditionResult.UNINITIALIZED, "Can't operate on an unitialized left operand!");
		TFUtils.Assert(right != ConditionResult.UNINITIALIZED, "Can't operate on an unitialized right operand!");
		if (left == ConditionResult.PASS || right == ConditionResult.PASS)
		{
			return ConditionResult.PASS;
		}
		if (left == ConditionResult.FAIL && right == ConditionResult.FAIL)
		{
			return ConditionResult.FAIL;
		}
		return ConditionResult.UNDECIDED;
	}
}
