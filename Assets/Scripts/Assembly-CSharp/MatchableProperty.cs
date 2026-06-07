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
			return false;
		}
	}

	public string Key
	{
		get
		{
			return null;
		}
	}

	public object Target
	{
		get
		{
			return null;
		}
	}

	public MatchableProperty(bool isRequired, string key, object target, MatchFn matchFn)
	{
	}

	public uint Evaluate(Dictionary<string, object> triggerData, Game game)
	{
		return 0u;
	}

	public override string ToString()
	{
		return null;
	}
}
