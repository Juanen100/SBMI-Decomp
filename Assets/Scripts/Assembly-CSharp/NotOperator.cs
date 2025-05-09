#define ASSERTS_ON
public class NotOperator : IOperator
{
	public const string DESCRIPTION = "NOT";

	public override string ToString()
	{
		return "NOT";
	}

	public ConditionResult Operate(ConditionResult left, ConditionResult right)
	{
		TFUtils.Assert(left != ConditionResult.UNINITIALIZED, "Can't operate on an unitialized left operand!");
		TFUtils.Assert(right == ConditionResult.UNINITIALIZED, "Right operand should not have a value!");
		switch (left)
		{
		case ConditionResult.UNDECIDED:
			return ConditionResult.UNDECIDED;
		case ConditionResult.PASS:
			return ConditionResult.FAIL;
		default:
			return ConditionResult.PASS;
		}
	}
}
