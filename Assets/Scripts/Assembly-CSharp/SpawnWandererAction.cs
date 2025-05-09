using System.Collections.Generic;

public class SpawnWandererAction : PersistedTriggerableAction
{
	public const string SPAWN_WANDERER = "sw";

	public int did;

	public string id;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public SpawnWandererAction(int did, string id)
		: base("sw", Identity.Null())
	{
		this.did = did;
		this.id = id;
	}

	public new static SpawnWandererAction FromDict(Dictionary<string, object> data)
	{
		int num = TFUtils.LoadInt(data, "did");
		string text = TFUtils.LoadString(data, "id");
		return new SpawnWandererAction(num, text);
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
		simulated = game.simulation.FindSimulated(did);
		if (simulated == null)
		{
			ResidentEntity decorator = game.simulation.EntityManager.Create(EntityType.WANDERER, did, null, true).GetDecorator<ResidentEntity>();
			simulated = Simulated.Wanderer.Load(decorator, utcNow, decorator.DisableFlee, game.simulation, utcNow);
		}
		else
		{
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			entity.HideExpiresAt = TFUtils.EpochTime();
			simulated.EnterInitialState(EntityManager.WandererActions["spawn"], game.simulation);
		}
		id = simulated.Id.Describe();
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> wandererGameState = ResidentEntity.GetWandererGameState(gameState, did);
		if (wandererGameState == null)
		{
			Simulated.Wanderer.AddWandererToGameState(gameState, id, did);
		}
		else
		{
			wandererGameState["hide_expires_at"] = TFUtils.EpochTime();
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
