using System.Collections.Generic;

public class HideWandererAction : PersistedSimulatedAction
{
	public const string HIDE_WANDERER = "hw";

	public ulong hideExpiresAt;

	public int dId;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public HideWandererAction(Identity id, int did, ulong hideExpireAt)
		: base("hw", id, typeof(HideWandererAction).ToString())
	{
		dId = did;
		hideExpiresAt = hideExpireAt;
	}

	public HideWandererAction(Simulated simulated, ulong hideExpireAt)
		: base("hw", simulated.Id, typeof(HideWandererAction).ToString())
	{
		Entity entity = simulated.entity;
		dId = entity.DefinitionId;
		hideExpiresAt = hideExpireAt;
	}

	public new static HideWandererAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		int did = TFUtils.LoadInt(data, "did");
		ulong hideExpireAt = TFUtils.LoadUlong(data, "hideExpiresAt");
		return new HideWandererAction(id, did, hideExpireAt);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["did"] = dId;
		dictionary["hideExpiresAt"] = hideExpiresAt;
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		simulated.ClearPendingCommands();
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		entity.HideExpiresAt = hideExpiresAt;
		simulated.EnterInitialState(EntityManager.WandererActions["hidden"], game.simulation);
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> wandererGameState = ResidentEntity.GetWandererGameState(gameState, target);
		if (wandererGameState != null)
		{
			wandererGameState["hide_expires_at"] = hideExpiresAt;
		}
		base.Confirm(gameState);
	}
}
