using System.Collections.Generic;

public class DisableFleeAction : PersistedTriggerableAction
{
	public const string DISABLE_FLEE = "df";

	public int did;

	public string id;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public DisableFleeAction(int did, string id)
		: base("df", Identity.Null())
	{
		this.did = did;
		this.id = id;
	}

	public new static DisableFleeAction FromDict(Dictionary<string, object> data)
	{
		int num = TFUtils.LoadInt(data, "did");
		string text = TFUtils.LoadString(data, "id");
		return new DisableFleeAction(num, text);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["did"] = did;
		dictionary["id"] = id;
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = null;
		if (!string.IsNullOrEmpty(id))
		{
			simulated = game.simulation.FindSimulated(new Identity(id));
		}
		if (simulated != null)
		{
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			entity.DisableFlee = true;
		}
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> wandererGameState = ResidentEntity.GetWandererGameState(gameState, new Identity(id));
		if (wandererGameState != null)
		{
			wandererGameState["disable_flee"] = true;
		}
		base.Confirm(gameState);
	}

	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}
}
