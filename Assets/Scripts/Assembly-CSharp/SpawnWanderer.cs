using System.Collections.Generic;

public class SpawnWanderer : SessionActionDefinition
{
	public const string TYPE = "spawn_wanderer";

	public const string WANDERER_ID = "id";

	private int? wandererID;

	public static SpawnWanderer Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		SpawnWanderer spawnWanderer = new SpawnWanderer();
		spawnWanderer.Parse(data, id, startConditions, originatedFromQuest);
		return spawnWanderer;
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
					residentEntity = session.TheGame.simulation.EntityManager.Create(EntityType.WANDERER, wandererID.Value, null, true).GetDecorator<ResidentEntity>();
					simulated = Simulated.Wanderer.Load(residentEntity, TFUtils.EpochTime(), residentEntity.DisableFlee, session.TheGame.simulation, TFUtils.EpochTime());
				}
				if (residentEntity == null)
				{
					residentEntity = simulated.GetEntity<ResidentEntity>();
					residentEntity.HideExpiresAt = TFUtils.EpochTime();
					simulated.EnterInitialState(EntityManager.WandererActions["spawn"], session.TheGame.simulation);
				}
				session.TheGame.simulation.ModifyGameState(new SpawnWandererAction(wandererID.Value, simulated.Id.Describe()));
				action.MarkSucceeded();
				return;
			}
		}
		action.MarkFailed();
	}
}
