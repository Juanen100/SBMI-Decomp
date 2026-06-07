using System.Collections.Generic;

public class UpdateVariableAction<T> : PersistedTriggerableAction
{
	public const string UPDATE_VARIABLE = "uv";

	private string m_sVarName;

	private T m_pVariable;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public UpdateVariableAction(string sVarName, T pVariable)
		: base(null, null)
	{
	}

	public new static UpdateVariableAction<T> FromDict(Dictionary<string, object> pData)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Apply(Game game, ulong utcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}
}
