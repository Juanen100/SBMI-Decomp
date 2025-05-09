#define ASSERTS_ON
using System.Collections.Generic;

public class SimulatedQuerier : Matcher
{
	private abstract class QuerySimulatedFinder
	{
		public abstract List<Simulated> FindCandidatesToQuery(Game game);

		public abstract List<Simulated> FindCandidates(Game game);

		public abstract int GetNumInventoryItems(Game game);
	}

	private class InstanceFinder : QuerySimulatedFinder
	{
		private Identity id;

		private bool m_bOnlyCompleteBuildings;

		public InstanceFinder(Identity id, bool bOnlyCompleteBuildings)
		{
			this.id = id;
			m_bOnlyCompleteBuildings = bOnlyCompleteBuildings;
		}

		public override List<Simulated> FindCandidatesToQuery(Game game)
		{
			List<Simulated> list = new List<Simulated>();
			Simulated simulated = game.simulation.FindSimulated(id);
			if (m_bOnlyCompleteBuildings && simulated.HasEntity<BuildingEntity>())
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				ActivatableDecorator decorator = entity.GetDecorator<ActivatableDecorator>();
				if (decorator.Activated == 0L)
				{
					list.Remove(simulated);
				}
				else if (simulated != null && simulated.SimulatedQueryable)
				{
					list.Add(simulated);
				}
			}
			return list;
		}

		public override List<Simulated> FindCandidates(Game game)
		{
			List<Simulated> list = new List<Simulated>();
			Simulated simulated = game.simulation.FindSimulated(id);
			if (m_bOnlyCompleteBuildings && simulated.HasEntity<BuildingEntity>())
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				ActivatableDecorator decorator = entity.GetDecorator<ActivatableDecorator>();
				if (decorator.Activated == 0L)
				{
					list.Remove(simulated);
				}
				else if (simulated != null && simulated.SimulatedQueryable)
				{
					list.Add(simulated);
				}
			}
			return list;
		}

		public override int GetNumInventoryItems(Game game)
		{
			return game.inventory.GetNumItems(id);
		}
	}

	private class TypeFinder : QuerySimulatedFinder
	{
		private int definitionId;

		private bool m_bOnlyCompleteBuildings;

		public TypeFinder(int definitionId, bool bOnlyCompleteBuildings)
		{
			this.definitionId = definitionId;
			m_bOnlyCompleteBuildings = bOnlyCompleteBuildings;
		}

		public override List<Simulated> FindCandidatesToQuery(Game game)
		{
			List<Simulated> list = game.simulation.FindAllSimulateds(definitionId);
			if (m_bOnlyCompleteBuildings)
			{
				int num = list.Count;
				for (int i = 0; i < num; i++)
				{
					if (list[i].HasEntity<BuildingEntity>())
					{
						BuildingEntity entity = list[i].GetEntity<BuildingEntity>();
						ActivatableDecorator decorator = entity.GetDecorator<ActivatableDecorator>();
						if (decorator.Activated == 0L)
						{
							list.RemoveAt(i);
							i--;
							num--;
						}
					}
				}
			}
			return list;
		}

		public override List<Simulated> FindCandidates(Game game)
		{
			List<Simulated> list = game.simulation.FindAllSimulateds(definitionId);
			list = list.FindAll((Simulated candidate) => candidate.SimulatedQueryable);
			if (m_bOnlyCompleteBuildings)
			{
				int count = list.Count;
				for (int num = 0; num < count; num++)
				{
					if (list[num].HasEntity<BuildingEntity>())
					{
						BuildingEntity entity = list[num].GetEntity<BuildingEntity>();
						ActivatableDecorator decorator = entity.GetDecorator<ActivatableDecorator>();
						if (decorator.Activated == 0L)
						{
							list.RemoveAt(num);
						}
					}
				}
			}
			return list;
		}

		public override int GetNumInventoryItems(Game game)
		{
			return game.inventory.GetNumItems(definitionId);
		}
	}

	private class TaskFinder
	{
		private int definitionId;

		public TaskFinder(int definitionId)
		{
			this.definitionId = definitionId;
		}

		public TaskData FindTaskToQuery(Game game)
		{
			return game.taskManager.GetTaskData(definitionId);
		}
	}

	private class ResourceFinder
	{
		private int definitionId;

		public ResourceFinder(int definitionId)
		{
			this.definitionId = definitionId;
		}

		public Resource FindResourceToQuery(Game game)
		{
			if (game.resourceManager.Resources.ContainsKey(definitionId))
			{
				return game.resourceManager.Resources[definitionId];
			}
			return null;
		}
	}

	public const string INSTANCE_ID = "simulated_guid";

	public const string DEFINITION_ID = "simulated_id";

	public const string INCLUDE_INVENTORY = "include_inventory";

	public const string COSTUME_ID = "costume_id";

	public const string TASK_ID = "task_id";

	public const string INSTANCE_COUNT = "instance_count";

	public const string TASK_COUNT = "task_count";

	public const string CRAFT_REWARD = "craft_reward";

	public const string COLLECT_READY = "ready_to_collect";

	public const string RESOURCE_ID = "resource_id";

	public const string BUILDING_COMPLETE = "complete_buildings_only";

	private const string SIMULATED_CANDIDATE = "simulated_candidate";

	private QuerySimulatedFinder simFinder;

	private TaskFinder taskFinder;

	private ResourceFinder resourceFinder;

	private ResourceMatcher resourceSubMatcher;

	private bool collectReady;

	private bool m_bIncludeInventory;

	private bool buildingComplete;

	public static SimulatedQuerier FromDict(Dictionary<string, object> dict)
	{
		SimulatedQuerier simulatedQuerier = new SimulatedQuerier();
		simulatedQuerier.simFinder = null;
		simulatedQuerier.taskFinder = null;
		simulatedQuerier.m_bIncludeInventory = false;
		if (dict.ContainsKey("include_inventory"))
		{
			simulatedQuerier.m_bIncludeInventory = TFUtils.LoadInt(dict, "include_inventory") == 1;
		}
		if (dict.ContainsKey("complete_buildings_only"))
		{
			simulatedQuerier.buildingComplete = TFUtils.LoadBool(dict, "complete_buildings_only");
		}
		else
		{
			simulatedQuerier.buildingComplete = false;
		}
		if (dict.ContainsKey("simulated_guid"))
		{
			simulatedQuerier.simFinder = new InstanceFinder(new Identity(TFUtils.LoadString(dict, "simulated_guid")), simulatedQuerier.buildingComplete);
		}
		else if (dict.ContainsKey("simulated_id"))
		{
			simulatedQuerier.simFinder = new TypeFinder(TFUtils.LoadInt(dict, "simulated_id"), simulatedQuerier.buildingComplete);
		}
		else if (dict.ContainsKey("task_id"))
		{
			simulatedQuerier.taskFinder = new TaskFinder(TFUtils.LoadInt(dict, "task_id"));
		}
		else if (dict.ContainsKey("resource_id"))
		{
			simulatedQuerier.resourceFinder = new ResourceFinder(TFUtils.LoadInt(dict, "resource_id"));
		}
		else
		{
			TFUtils.ErrorLog(string.Format("Not enough information to find a simulated to query. Requires either '{0}' or '{1}' or '{2}'\nData={3}", "simulated_guid", "simulated_id", "task_id", TFUtils.DebugDictToString(dict)));
		}
		if (dict.ContainsKey("instance_count"))
		{
			simulatedQuerier.RegisterProperty("instance_count", dict, simulatedQuerier.MatchCount);
		}
		if (dict.ContainsKey("task_count"))
		{
			simulatedQuerier.RegisterProperty("task_count", dict, simulatedQuerier.MatchTaskCount);
		}
		else if (simulatedQuerier.taskFinder != null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("task_count", 1);
			simulatedQuerier.RegisterProperty("task_count", dictionary, simulatedQuerier.MatchTaskCount);
		}
		if (dict.ContainsKey("ready_to_collect"))
		{
			simulatedQuerier.collectReady = TFUtils.LoadInt(dict, "ready_to_collect") == 1;
		}
		else
		{
			simulatedQuerier.collectReady = false;
		}
		if (dict.ContainsKey("craft_reward"))
		{
			simulatedQuerier.RegisterProperty("craft_reward", dict, simulatedQuerier.MatchCraftReward);
			simulatedQuerier.resourceSubMatcher = ResourceMatcher.FromDict(TFUtils.LoadDict(dict, "craft_reward"));
		}
		if (dict.ContainsKey("costume_id"))
		{
			simulatedQuerier.RegisterProperty("costume_id", dict, simulatedQuerier.MatchCostume);
		}
		return simulatedQuerier;
	}

	public override uint MatchAmount(Game game, Dictionary<string, object> data)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (taskFinder != null)
		{
			dictionary["task_count"] = 0;
			TaskData taskData = taskFinder.FindTaskToQuery(game);
			if (taskData != null)
			{
				Task activeTask = game.taskManager.GetActiveTask(taskData.m_nDID);
				if (activeTask != null)
				{
					if (collectReady)
					{
						if (activeTask.GetTimeLeft() == 0)
						{
							dictionary["task_count"] = 1;
						}
					}
					else
					{
						dictionary["task_count"] = 1;
					}
				}
			}
			return base.MatchAmount(game, dictionary);
		}
		if (resourceFinder != null)
		{
			dictionary["instance_count"] = 0;
			Resource resource = resourceFinder.FindResourceToQuery(game);
			if (resource != null)
			{
				dictionary["instance_count"] = resource.Amount;
			}
			return base.MatchAmount(game, dictionary);
		}
		List<Simulated> list = simFinder.FindCandidatesToQuery(game);
		dictionary["instance_count"] = list.Count;
		int num = 0;
		foreach (Simulated item in list)
		{
			num += game.taskManager.GetActiveTasksForSimulated(item.entity.DefinitionId, null).Count;
		}
		dictionary["task_count"] = num;
		uint num2 = 0u;
		foreach (Simulated item2 in list)
		{
			dictionary["simulated_candidate"] = item2;
			num2 = base.MatchAmount(game, dictionary);
			if (num2 != 0)
			{
				break;
			}
		}
		if (num2 == 0)
		{
			int num3 = list.Count;
			if (num3 <= 0)
			{
				num3 = simFinder.FindCandidates(game).Count;
			}
			if (m_bIncludeInventory)
			{
				num3 += simFinder.GetNumInventoryItems(game);
			}
			dictionary["simulated_candidate"] = null;
			dictionary["instance_count"] = num3;
			num2 = base.MatchAmount(game, dictionary);
		}
		return num2;
	}

	public override string DescribeSubject(Game game)
	{
		return string.Empty;
	}

	private uint MatchCount(MatchableProperty idProperty, Dictionary<string, object> candidateWrapper, Game game)
	{
		int amount = (int)candidateWrapper["instance_count"];
		return CompareOperandRangesToAmount(idProperty.Target, amount);
	}

	private uint MatchTaskCount(MatchableProperty idProperty, Dictionary<string, object> candidateWrapper, Game game)
	{
		int amount = (int)candidateWrapper["task_count"];
		return CompareOperandRangesToAmount(idProperty.Target, amount);
	}

	private uint MatchCostume(MatchableProperty idProperty, Dictionary<string, object> candidateWrapper, Game game)
	{
		Simulated simulated = (Simulated)candidateWrapper["simulated_candidate"];
		if (simulated == null)
		{
			return 0u;
		}
		if (!simulated.HasEntity<ResidentEntity>())
		{
			return 0u;
		}
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		if (entity.CostumeDID.HasValue)
		{
			return CompareOperandRangesToAmount(idProperty.Target, entity.CostumeDID.Value);
		}
		return 0u;
	}

	private uint MatchCraftReward(MatchableProperty craftRewardProperty, Dictionary<string, object> candidateWrapper, Game game)
	{
		Simulated simulated = (Simulated)candidateWrapper["simulated_candidate"];
		if (simulated == null)
		{
			return 0u;
		}
		TFUtils.Assert((simulated.entity.AllTypes & EntityType.BUILDING) != 0, "Was expecting to be examining a building entity! Instead found: " + simulated.entity.AllTypes);
		BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
		if (entity.CraftRewards == null)
		{
			return 0u;
		}
		Dictionary<string, object> data = new Dictionary<string, object>();
		entity.CraftRewards.AddDataToTrigger(ref data);
		return resourceSubMatcher.MatchAmount(game, data);
	}
}
