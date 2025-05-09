using System;
using System.Collections.Generic;
using UnityEngine;

public class CompleteBuildingAction : PersistedSimulatedAction
{
	private class ResidentInfo
	{
		private const string ID = "id";

		private const string DID = "did";

		private const string HUNGRY_AT = "hungry";

		public string id;

		public int did;

		public ulong hungryAt;

		public ResidentInfo()
		{
		}

		public ResidentInfo(Simulated livingResident)
		{
			id = livingResident.Id.Describe();
			did = livingResident.entity.DefinitionId;
			hungryAt = livingResident.GetEntity<ResidentEntity>().HungryAt;
		}

		public static ResidentInfo FromDict(Dictionary<string, object> data)
		{
			ResidentInfo residentInfo = new ResidentInfo();
			residentInfo.id = TFUtils.LoadString(data, "id");
			residentInfo.did = TFUtils.LoadInt(data, "did");
			residentInfo.hungryAt = TFUtils.LoadUlong(data, "hungry");
			return residentInfo;
		}

		public Dictionary<string, object> ToDict()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["id"] = id;
			dictionary["did"] = did;
			dictionary["hungry"] = hungryAt;
			return dictionary;
		}
	}

	public const string COMPLETE_BUILDING = "cb";

	public const int NO_HUNGER = -1;

	public ulong completeTime;

	private List<ResidentInfo> residents;

	public ulong productReady;

	public Reward reward;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	private CompleteBuildingAction(Identity target, ulong completeTime, List<ResidentInfo> residents, Reward reward)
		: this(target, completeTime, typeof(CompleteBuildingAction).ToString())
	{
		this.residents = residents;
		this.reward = reward;
	}

	public CompleteBuildingAction(Simulated simulated, List<Simulated> residents, Reward reward)
		: this(simulated.Id, TFUtils.EpochTime(), typeof(CompleteBuildingAction).ToString())
	{
		if (simulated.HasEntity<PeriodicProductionDecorator>())
		{
			PeriodicProductionDecorator entity = simulated.GetEntity<PeriodicProductionDecorator>();
			productReady = completeTime + entity.RentProductionTime;
		}
		if (residents == null)
		{
			this.residents = null;
		}
		else
		{
			this.residents = new List<ResidentInfo>();
			foreach (Simulated resident in residents)
			{
				this.residents.Add(new ResidentInfo(resident));
			}
		}
		this.reward = reward;
	}

	private CompleteBuildingAction(Identity target, ulong completeTime, string triggerType)
		: base("cb", target, triggerType)
	{
		this.completeTime = completeTime;
	}

	public new static CompleteBuildingAction FromDict(Dictionary<string, object> data)
	{
		Identity identity = new Identity((string)data["target"]);
		ulong num = TFUtils.LoadUlong(data, "completeTime");
		Dictionary<string, object> dictionary = TFUtils.TryLoadDict(data, "residents");
		List<ResidentInfo> list = new List<ResidentInfo>();
		if (dictionary != null)
		{
			foreach (Dictionary<string, object> value in dictionary.Values)
			{
				list.Add(ResidentInfo.FromDict(value));
			}
		}
		if (data.ContainsKey("residentId"))
		{
			ResidentInfo residentInfo = new ResidentInfo();
			residentInfo.did = TFUtils.LoadInt(data, "residentDid");
			residentInfo.id = (string)data["residentId"];
			residentInfo.hungryAt = TFUtils.LoadUlong(data, "residentHungryTime");
			list.Add(residentInfo);
		}
		Reward reward = ((!data.ContainsKey("reward")) ? null : Reward.FromObject(data["reward"]));
		CompleteBuildingAction completeBuildingAction = new CompleteBuildingAction(identity, num, list, reward);
		completeBuildingAction.DropTargetDataFromDict(data);
		return completeBuildingAction;
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["completeTime"] = completeTime;
		if (residents == null)
		{
			dictionary["residents"] = null;
		}
		else
		{
			Dictionary<string, Dictionary<string, object>> dictionary2 = new Dictionary<string, Dictionary<string, object>>();
			int num = 0;
			foreach (ResidentInfo resident in residents)
			{
				dictionary2.Add(num++.ToString(), resident.ToDict());
			}
			dictionary["residents"] = dictionary2;
		}
		if (reward != null)
		{
			dictionary["reward"] = reward.ToDict();
		}
		DropTargetDataToDict(dictionary);
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
		ActivatableDecorator entity = simulated.GetEntity<ActivatableDecorator>();
		simulated.ClearPendingCommands();
		game.simulation.Router.CancelMatching(Command.TYPE.COMPLETE, simulated.Id, simulated.Id);
		entity.Activated = utcNow;
		simulated.entity.PatchReferences(game);
		if (simulated.HasEntity<PeriodicProductionDecorator>())
		{
			PeriodicProductionDecorator entity2 = simulated.GetEntity<PeriodicProductionDecorator>();
			productReady = GetTime() + entity2.RentProductionTime;
			TFUtils.DebugLog("setting product.ready to " + productReady + ". That is " + (productReady - utcNow) + " seconds from now.");
			entity2.ProductReadyTime = productReady;
			if (productReady <= utcNow)
			{
				simulated.EnterInitialState(EntityManager.BuildingActions["produced"], game.simulation);
			}
			else
			{
				simulated.EnterInitialState(EntityManager.BuildingActions["producing"], game.simulation);
				simulated.AddPendingCommand(new Simulated.PendingCommand
				{
					c = CompleteCommand.Create(simulated.Id, simulated.Id),
					delay = productReady - utcNow
				});
			}
		}
		else
		{
			simulated.EnterInitialState(EntityManager.BuildingActions["reflecting"], game.simulation);
		}
		foreach (ResidentInfo resident in residents)
		{
			ResidentEntity decorator = game.entities.Create(EntityType.RESIDENT, resident.did, new Identity(resident.id), true).GetDecorator<ResidentEntity>();
			if (decorator.Disabled)
			{
				residents.Remove(resident);
				continue;
			}
			ulong nextHungerTime = 0uL;
			Simulated.Resident.Load(decorator, target, null, null, null, nextHungerTime, null, null, game.simulation, utcNow);
		}
		if (reward != null)
		{
			game.ApplyReward(reward, GetTime());
		}
		AddPickup(game.simulation);
		simulated.FirstAnimate(game.simulation);
		simulated.RemoveScaffolding(game.simulation);
		simulated.RemoveFence(game.simulation);
		Vector3 vector = new Vector3((simulated.Box.xmax + simulated.Box.xmin) * 0.5f, (simulated.Box.ymax + simulated.Box.ymin) * 0.5f, 0f);
		simulated.DisplayController.Position = simulated.DisplayOffsetWorld + vector - simulated.TextureOriginWorld;
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		string targetString = target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		Dictionary<string, object> data = (Dictionary<string, object>)list.Find(match);
		ActivatableDecorator.Serialize(ref data, completeTime);
		if (data.ContainsKey("build_finish_time"))
		{
			data.Remove("build_finish_time");
		}
		data["rent_ready_time"] = ((productReady != 0L) ? ((object)productReady) : null);
		foreach (ResidentInfo resident in residents)
		{
			Simulated.Building.AddResidentToGameState(gameState, resident.id, resident.did, target.Describe(), resident.hungryAt);
		}
		if (reward != null)
		{
			RewardManager.ApplyToGameState(reward, GetTime(), gameState);
		}
		AddPickupToGameState(gameState);
		base.Confirm(gameState);
	}
}
