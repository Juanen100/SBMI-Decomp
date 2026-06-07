using System.Collections.Generic;

public class ButtonTapAction : PersistedTriggerableAction
{
	public const string BUTTON_TAP = "bt";

	public string m_sID;

	public TriggerableMixin Triggerable
	{
		get
		{
			return null;
		}
	}

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public ButtonTapAction(string sID)
		: base(null, null)
	{
	}

	public new static ButtonTapAction FromDict(Dictionary<string, object> data)
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

	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return null;
	}
}
