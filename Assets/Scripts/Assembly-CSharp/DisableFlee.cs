using System.Collections.Generic;

public class DisableFlee : SessionActionDefinition
{
	public const string TYPE = "disable_flee";

	public const string WANDERER_ID = "id";

	private int? wandererID;

	public static DisableFlee Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		DisableFlee disableFlee = new DisableFlee();
		disableFlee.Parse(data, id, startConditions, originatedFromQuest);
		return disableFlee;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		wandererID = TFUtils.TryLoadInt(data, "id");
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		int? num = wandererID;
		dictionary["id"] = (num.HasValue ? wandererID : new int?(-1));
		return dictionary;
	}

	public void Handle(Session session, SessionActionTracker action)
	{
		action.MarkStarted();
		int? num = wandererID;
		if (num.HasValue)
		{
			int? num2 = wandererID;
			if (!num2.HasValue || num2.Value >= 0)
			{
				Simulated simulated = session.TheGame.simulation.FindSimulated(wandererID.Value);
				ResidentEntity residentEntity = null;
				if (simulated == null)
				{
					action.MarkFailed();
					return;
				}
				residentEntity = simulated.GetEntity<ResidentEntity>();
				residentEntity.DisableFlee = true;
				session.TheGame.simulation.ModifyGameState(new DisableFleeAction(wandererID.Value, simulated.Id.Describe()));
				action.MarkSucceeded();
				return;
			}
		}
		action.MarkFailed();
	}
}
