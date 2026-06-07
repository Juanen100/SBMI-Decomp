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
		}

		public override List<Simulated> FindCandidatesToQuery(Game game)
		{
			return null;
		}

		public override List<Simulated> FindCandidates(Game game)
		{
			return null;
		}

		public override int GetNumInventoryItems(Game game)
		{
			return 0;
		}
	}

	private class TypeFinder : QuerySimulatedFinder
	{
		private int definitionId;

		private bool m_bOnlyCompleteBuildings;

		public TypeFinder(int definitionId, bool bOnlyCompleteBuildings)
		{
		}

		public override List<Simulated> FindCandidatesToQuery(Game game)
		{
			return null;
		}

		public override List<Simulated> FindCandidates(Game game)
		{
			return null;
		}

		public override int GetNumInventoryItems(Game game)
		{
			return 0;
		}
	}

	private class TaskFinder
	{
		private int definitionId;

		public TaskFinder(int definitionId)
		{
		}

		public TaskData FindTaskToQuery(Game game)
		{
			return null;
		}
	}

	private class ResourceFinder
	{
		private int definitionId;

		public ResourceFinder(int definitionId)
		{
		}

		public Resource FindResourceToQuery(Game game)
		{
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
		return null;
	}

	public override uint MatchAmount(Game game, Dictionary<string, object> data)
	{
		return 0u;
	}

	public override string DescribeSubject(Game game)
	{
		return null;
	}

	private uint MatchCount(MatchableProperty idProperty, Dictionary<string, object> candidateWrapper, Game game)
	{
		return 0u;
	}

	private uint MatchTaskCount(MatchableProperty idProperty, Dictionary<string, object> candidateWrapper, Game game)
	{
		return 0u;
	}

	private uint MatchCostume(MatchableProperty idProperty, Dictionary<string, object> candidateWrapper, Game game)
	{
		return 0u;
	}

	private uint MatchCraftReward(MatchableProperty craftRewardProperty, Dictionary<string, object> candidateWrapper, Game game)
	{
		return 0u;
	}
}
