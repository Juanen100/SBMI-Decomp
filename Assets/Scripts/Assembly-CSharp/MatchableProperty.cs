using System.Collections.Generic;

public class MatchableProperty
{
	public delegate uint MatchFn(MatchableProperty property, Dictionary<string, object> triggerData, Game game);

	private bool isRequired;

	private string key;

	private object target;

	private MatchFn matchFn;

	public bool IsRequired
	{
		get
		{
			return isRequired;
		}
	}

	public string Key
	{
		get
		{
			return key;
		}
	}

	public object Target
	{
		get
		{
			return target;
		}
	}

	public MatchableProperty(bool isRequired, string key, object target, MatchFn matchFn)
	{
		this.isRequired = isRequired;
		this.key = key;
		this.target = target;
		this.matchFn = matchFn;
	}

	public uint Evaluate(Dictionary<string, object> triggerData, Game game)
	{
		return matchFn(this, triggerData, game);
	}

	public override string ToString()
	{
		return string.Concat("{", key, " (isRequired=", isRequired, ", target=", Target, ")}");
	}
}
