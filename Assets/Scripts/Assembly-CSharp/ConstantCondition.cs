using System.Collections.Generic;

public class ConstantCondition : LoadableCondition
{
	public const string LOAD_TOKEN = "constant";

	private bool val;

	public bool Value
	{
		get
		{
			return val;
		}
	}

	private ConstantCondition()
	{
	}

	public ConstantCondition(uint id, bool val)
	{
		Initialize(id, 1u, "constant", new List<string>(), new List<uint>());
		this.val = val;
	}

	public static ConstantCondition FromDict(Dictionary<string, object> dict)
	{
		bool flag = ((TFUtils.LoadInt(dict, "value") != 0) ? true : false);
		ConstantCondition constantCondition = new ConstantCondition();
		constantCondition.Parse(dict, "constant", new List<string>(), flag);
		return constantCondition;
	}

	public override string Description(Game game)
	{
		return Value.ToString();
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["value"] = (val ? 1 : 0);
		return dictionary;
	}

	protected void Parse(Dictionary<string, object> loadedData, string loadToken, ICollection<string> relevantTypes, bool val)
	{
		Parse(loadedData, loadToken, relevantTypes);
		this.val = val;
	}

	public override void Evaluate(ConditionState state, Game game, ITrigger trigger)
	{
		state.SelfExam = (val ? ConditionResult.PASS : ConditionResult.FAIL);
	}
}
