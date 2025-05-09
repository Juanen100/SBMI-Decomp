#define ASSERTS_ON
public static class OperatorFactory
{
	public static IOperator StringToOperator(string opString)
	{
		switch (opString)
		{
		case "OR":
			return new OrOperator();
		case "AND":
			return new AndOperator();
		case "NOT":
			return new NotOperator();
		default:
			TFUtils.Assert(false, "This ConditionTree uses an unknown operator: " + opString);
			return null;
		}
	}
}
