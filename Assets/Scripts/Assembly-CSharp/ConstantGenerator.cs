public class ConstantGenerator : ResultGenerator
{
	private string val;

	public ConstantGenerator(string val)
	{
		this.val = val;
	}

	public string GetResult()
	{
		return val;
	}

	public string GetExpectedValue()
	{
		return val;
	}

	public string GetLowestValue()
	{
		return val;
	}
}
