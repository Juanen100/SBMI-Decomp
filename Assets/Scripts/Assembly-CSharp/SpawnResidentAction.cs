using System.Collections.Generic;

public class SpawnResidentAction : PersistedTriggerableAction
{
	public const string SPAWN_RESDIENT = "sr";

	public int residentDID;

	public string residentID;

	public int buildingDID;

	public string buildingID;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public SpawnResidentAction(int residentDID, string residentID, int buildingDID, string buildingID)
		: base("sr", Identity.Null())
	{
		this.residentDID = residentDID;
		this.residentID = residentID;
		this.buildingDID = buildingDID;
		this.buildingID = buildingID;
	}

	public new static SpawnResidentAction FromDict(Dictionary<string, object> data)
	{
		int num = TFUtils.LoadInt(data, "resident_did");
		string text = TFUtils.LoadString(data, "resident_id");
		int num2 = TFUtils.LoadInt(data, "building_did");
		string text2 = TFUtils.LoadString(data, "building_id");
		return new SpawnResidentAction(num, text, num2, text2);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["resident_did"] = residentDID;
		dictionary["resident_id"] = residentID;
		dictionary["building_did"] = buildingDID;
		dictionary["building_id"] = buildingID;
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(new Identity(buildingID));
		if (simulated != null)
		{
			Simulated simulated2 = null;
			simulated2 = game.simulation.FindSimulated(residentDID);
			if (simulated2 == null)
			{
				ResidentEntity decorator = game.simulation.EntityManager.Create(EntityType.RESIDENT, residentDID, null, true).GetDecorator<ResidentEntity>();
				decorator.HungerResourceId = null;
				decorator.PreviousResourceId = null;
				decorator.WishExpiresAt = null;
				decorator.HungryAt = TFUtils.EpochTime();
				decorator.MatchBonus = null;
				simulated2 = Simulated.Resident.Load(decorator, simulated.Id, decorator.WishExpiresAt, decorator.HungerResourceId, decorator.PreviousResourceId, decorator.HungryAt, null, decorator.MatchBonus, game.simulation, TFUtils.EpochTime());
			}
			residentID = simulated2.Id.Describe();
			base.Apply(game, utcNow);
		}
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> unitGameState = ResidentEntity.GetUnitGameState(gameState, residentDID);
		if (unitGameState == null)
		{
			Simulated.Building.AddResidentToGameState(gameState, residentID, residentDID, buildingID, TFUtils.EpochTime());
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
