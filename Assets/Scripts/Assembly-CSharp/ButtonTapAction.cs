using System.Collections.Generic;

public class ButtonTapAction : PersistedTriggerableAction
{
	public const string BUTTON_TAP = "bt";

	public string m_sID;

	public TriggerableMixin Triggerable
	{
		get
		{
			return triggerable;
		}
	}

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public ButtonTapAction(string sID)
		: base("bt", Identity.Null())
	{
		m_sID = sID;
	}

	public new static ButtonTapAction FromDict(Dictionary<string, object> data)
	{
		string sID = TFUtils.LoadString(data, "button_id");
		return new ButtonTapAction(sID);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["button_id"] = m_sID;
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		base.Confirm(gameState);
	}

	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		data.Add("button_id", m_sID);
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}
}
