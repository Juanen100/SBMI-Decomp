using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : PersistedSimulatedAction
{
	private struct ResidentInfo
	{
		public string id;

		public int did;

		public int? hungerId;

		public int? prevHungerId;

		public ulong? wishExpiresAt;

		public ulong hungryAt;

		public ulong? fullnessLength;

		public ResidentInfo(string id, int did, int? hungerId, int? prevHungerId, ulong? wishExpiresAt, ulong hungryAt, ulong? fullnessLength)
		{
			this.id = id;
			this.did = did;
			this.hungerId = hungerId;
			this.prevHungerId = prevHungerId;
			this.wishExpiresAt = wishExpiresAt;
			this.hungryAt = hungryAt;
			this.fullnessLength = fullnessLength;
		}
	}

	public const string MOVE = "m";

	public int? x;

	public int? y;

	public bool? flip;

	private List<ResidentInfo> residentInfos;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public MoveAction(Simulated simulated, List<Simulated> residents)
		: this(simulated.Id, (int)simulated.Position.x, (int)simulated.Position.y, simulated.Flip, residents)
	{
	}

	public MoveAction(Identity id, int? x, int? y, bool? flip, List<Simulated> residents)
		: base("m", id, typeof(MoveAction).ToString())
	{
		this.x = x;
		this.y = y;
		this.flip = flip;
		if (residents != null)
		{
			InitializeResidents(residents);
		}
	}

	private MoveAction(Identity id, int? x, int? y, bool? flip, List<ResidentInfo> residentInfos)
		: this(id, x, y, flip, (List<Simulated>)null)
	{
		this.residentInfos = residentInfos;
	}

	public new static MoveAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		int? num = TFUtils.LoadNullableInt(data, "x");
		int? num2 = TFUtils.LoadNullableInt(data, "y");
		bool? flag = TFUtils.LoadNullableBool(data, "flip");
		List<object> list = (List<object>)data["residents"];
		List<ResidentInfo> list2 = null;
		if (list != null)
		{
			list2 = new List<ResidentInfo>();
			int num3 = 0;
			foreach (Dictionary<string, object> item in list)
			{
				string id2 = (string)item["id"];
				int did = TFUtils.LoadInt(item, "did");
				int? hungerId = TFUtils.TryLoadInt(item, "wish_product_id");
				int? prevHungerId = TFUtils.TryLoadInt(item, "prev_wish_product_id");
				ulong? wishExpiresAt = TFUtils.TryLoadUlong(item, "wish_expires_at");
				ulong hungryAt = TFUtils.LoadUlong(item, "hungry_at");
				ulong? fullnessLength = TFUtils.TryLoadUlong(item, "fullness_length");
				list2.Add(new ResidentInfo(id2, did, hungerId, prevHungerId, wishExpiresAt, hungryAt, fullnessLength));
				num3++;
			}
		}
		return new MoveAction(id, num, num2, flag, list2);
	}

	private void InitializeResidents(List<Simulated> residents)
	{
		residentInfos = new List<ResidentInfo>();
		int num = 0;
		foreach (Simulated resident in residents)
		{
			ResidentEntity entity = resident.GetEntity<ResidentEntity>();
			residentInfos.Add(new ResidentInfo(resident.Id.Describe(), resident.entity.DefinitionId, entity.HungerResourceId, entity.PreviousResourceId, entity.WishExpiresAt, entity.HungryAt, entity.FullnessLength));
			num++;
		}
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["x"] = x;
		dictionary["y"] = y;
		dictionary["flip"] = flip;
		List<object> list = null;
		if (residentInfos != null)
		{
			list = new List<object>();
			foreach (ResidentInfo residentInfo in residentInfos)
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2["id"] = residentInfo.id;
				dictionary2["did"] = residentInfo.did;
				dictionary2["wish_product_id"] = residentInfo.hungerId;
				dictionary2["wish_expires_at"] = residentInfo.wishExpiresAt;
				dictionary2["hungry_at"] = residentInfo.hungryAt;
				list.Add(dictionary2);
			}
		}
		dictionary["residents"] = list;
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(target);
		if (simulated != null)
		{
			int? num = x;
			if (num.HasValue)
			{
				int? num2 = y;
				if (num2.HasValue)
				{
					int? num3 = x;
					float num4 = num3.Value;
					int? num5 = y;
					simulated.Warp(new Vector2(num4, num5.Value), game.simulation);
					simulated.Flip = flip.HasValue && flip.Value;
					simulated.simFlags |= Simulated.SimulatedFlags.FIRST_ANIMATE;
					Simulated.Building.AdjustWorkerPosition(simulated, game.simulation);
					goto IL_0323;
				}
			}
			BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
			List<KeyValuePair<int, Identity>> list = new List<KeyValuePair<int, Identity>>();
			if (residentInfos != null)
			{
				foreach (ResidentInfo residentInfo in residentInfos)
				{
					Identity identity = new Identity(residentInfo.id);
					list.Add(new KeyValuePair<int, Identity>(residentInfo.did, identity));
					Simulated simulated2 = game.simulation.FindSimulated(identity);
					if (simulated2 != null)
					{
						game.simulation.RemoveSimulated(simulated2);
						SwarmManager.Instance.RemoveResident(simulated2.GetEntity<ResidentEntity>(), simulated);
					}
				}
			}
			game.inventory.AddItem(entity, list);
			simulated.SetFootprint(game.simulation, false);
			game.simulation.RemoveSimulated(simulated);
		}
		else
		{
			int? num6 = x;
			if (num6.HasValue)
			{
				int? num7 = y;
				if (num7.HasValue)
				{
					List<KeyValuePair<int, Identity>> outAssociatedEntities;
					Entity entity2 = game.inventory.RemoveEntity(target, out outAssociatedEntities);
					if (entity2 != null)
					{
						BuildingEntity decorator = entity2.GetDecorator<BuildingEntity>();
						if (decorator.HasDecorator<PeriodicProductionDecorator>())
						{
							PeriodicProductionDecorator decorator2 = decorator.GetDecorator<PeriodicProductionDecorator>();
							decorator2.ProductReadyTime = GetTime() + decorator2.RentProductionTime;
						}
						Simulation simulation = game.simulation;
						int? num8 = x;
						float num9 = num8.Value;
						int? num10 = y;
						Simulated.Building.Load(decorator, simulation, new Vector2(num9, num10.Value), flip.HasValue && flip.Value, utcNow);
						if (residentInfos != null && residentInfos.Count > 0)
						{
							foreach (ResidentInfo residentInfo2 in residentInfos)
							{
								ResidentEntity decorator3 = game.entities.GetEntity(new Identity(residentInfo2.id)).GetDecorator<ResidentEntity>();
								if (decorator3.Disabled)
								{
									residentInfos.Remove(residentInfo2);
								}
								else
								{
									Simulated.Resident.Load(decorator3, target, residentInfo2.wishExpiresAt, residentInfo2.hungerId, residentInfo2.prevHungerId, residentInfo2.hungryAt, null, null, game.simulation, utcNow);
								}
							}
						}
					}
				}
			}
		}
		goto IL_0323;
		IL_0323:
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		string targetString = target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find(match);
		if (dictionary == null)
		{
			base.Confirm(gameState);
			return;
		}
		int? num = TFUtils.LoadNullableInt(dictionary, "x");
		int? num2 = TFUtils.LoadNullableInt(dictionary, "y");
		dictionary["x"] = x;
		dictionary["y"] = y;
		dictionary["flip"] = flip;
		int did = TFUtils.LoadInt(dictionary, "did");
		if (residentInfos != null)
		{
			if (!num.HasValue && !num2.HasValue && x.HasValue && y.HasValue)
			{
				Blueprint blueprint = EntityManager.GetBlueprint("building", did);
				if (blueprint.Invariable["time.production"] != null)
				{
					dictionary["rent_ready_time"] = GetTime() + (ulong)blueprint.Invariable["time.production"];
				}
				foreach (ResidentInfo residentInfo in residentInfos)
				{
					Dictionary<string, object> unitGameState = ResidentEntity.GetUnitGameState(gameState, new Identity(residentInfo.id));
					if (unitGameState == null)
					{
						Simulated.Building.AddResidentToGameState(gameState, residentInfo.id, residentInfo.did, target.Describe(), residentInfo.hungryAt);
						continue;
					}
					ResidentEntity.UpdateHungerTimeInUnitState(unitGameState, residentInfo.hungryAt);
					ResidentEntity.SetActiveStatusInUnitState(unitGameState, true);
				}
			}
			else if (num.HasValue && num2.HasValue && !x.HasValue && !y.HasValue)
			{
				foreach (ResidentInfo residentInfo2 in residentInfos)
				{
					Dictionary<string, object> unitGameState2 = ResidentEntity.GetUnitGameState(gameState, new Identity(residentInfo2.id));
					if (unitGameState2 != null)
					{
						ResidentEntity.SetActiveStatusInUnitState(unitGameState2, false);
					}
				}
			}
		}
		base.Confirm(gameState);
	}
}
