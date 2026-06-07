using System.Collections.Generic;

public class InitializeAction : PersistedActionBuffer.PersistedAction
{
	public const string INITIALIZE = "i";

	public InitializeAction()
		: base(null, null)
	{
	}

	public new static InitializeAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public override void Process(Game game)
	{
	}

	public override void Apply(Game game, ulong utcNow)
	{
	}
}
