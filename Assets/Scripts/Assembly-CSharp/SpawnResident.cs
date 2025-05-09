using System.Collections.Generic;

public class SpawnResident : SessionActionDefinition
{
	public const string TYPE = "spawn_resident";

	public const string RESIDENT_ID = "resident_id";

	public const string BUILDING_ID = "building_id";

	private int? residentID;

	private int? buildingID;

	public static SpawnResident Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		SpawnResident spawnResident = new SpawnResident();
		spawnResident.Parse(data, id, startConditions, originatedFromQuest);
		return spawnResident;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		residentID = TFUtils.TryLoadInt(data, "resident_id");
		buildingID = TFUtils.TryLoadInt(data, "building_id");
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		int? num = residentID;
		dictionary["resident_id"] = (num.HasValue ? residentID : new int?(-1));
		int? num2 = buildingID;
		dictionary["building_id"] = (num2.HasValue ? buildingID : new int?(-1));
		return dictionary;
	}

	public void Handle(Session session, SessionActionTracker action)
	{
		action.MarkStarted();
		int? num = residentID;
		if (num.HasValue)
		{
			int? num2 = residentID;
			if (!num2.HasValue || num2.Value >= 0)
			{
				int? num3 = buildingID;
				if (num3.HasValue)
				{
					int? num4 = buildingID;
					if (!num4.HasValue || num4.Value >= 0)
					{
						Simulated simulated = session.TheGame.simulation.FindSimulated(buildingID.Value);
						if (simulated == null)
						{
							action.MarkFailed();
							return;
						}
						Simulated simulated2 = session.TheGame.simulation.FindSimulated(residentID.Value);
						ResidentEntity residentEntity = null;
						if (simulated2 == null)
						{
							residentEntity = session.TheGame.simulation.EntityManager.Create(EntityType.RESIDENT, residentID.Value, null, true).GetDecorator<ResidentEntity>();
							if (residentEntity.Disabled)
							{
								action.MarkFailed();
								return;
							}
							residentEntity.HungerResourceId = null;
							residentEntity.PreviousResourceId = null;
							residentEntity.WishExpiresAt = null;
							residentEntity.HungryAt = TFUtils.EpochTime();
							residentEntity.MatchBonus = null;
							simulated2 = Simulated.Resident.Load(residentEntity, simulated.Id, residentEntity.WishExpiresAt, residentEntity.HungerResourceId, residentEntity.PreviousResourceId, residentEntity.HungryAt, null, residentEntity.MatchBonus, session.TheGame.simulation, TFUtils.EpochTime());
							session.TheGame.simulation.ModifyGameState(new SpawnResidentAction(residentID.Value, simulated2.Id.Describe(), buildingID.Value, simulated.Id.Describe()));
							action.MarkSucceeded();
						}
						else
						{
							action.MarkFailed();
						}
						return;
					}
				}
			}
		}
		action.MarkFailed();
	}
}
