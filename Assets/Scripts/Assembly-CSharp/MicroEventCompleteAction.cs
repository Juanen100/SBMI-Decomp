using System.Collections.Generic;

public class MicroEventCompleteAction : PersistedTriggerableAction
{
	public const string MICRO_EVENT_COMPLETE = "mca";

	private MicroEvent m_pMicroEvent;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public MicroEventCompleteAction(MicroEvent pMicroEvent)
		: base(null, null)
	{
	}

	public new static MicroEventCompleteAction FromDict(Dictionary<string, object> pData)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Apply(Game pGame, ulong ulUtcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> pGameState)
	{
	}

	protected virtual void AddMoreDataToTrigger(ref Dictionary<string, object> pData)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> pData)
	{
		return null;
	}
}
