using System.Collections.Generic;

public class CompleteBuildingAction : PersistedSimulatedAction
{
	private class ResidentInfo
	{
		public string id;

		public int did;

		public ulong hungryAt;

		private const string ID = "id";

		private const string DID = "did";

		private const string HUNGRY_AT = "hungry";

		public ResidentInfo()
		{
		}

		public ResidentInfo(Simulated livingResident)
		{
		}

		public static ResidentInfo FromDict(Dictionary<string, object> data)
		{
			return null;
		}

		public Dictionary<string, object> ToDict()
		{
			return null;
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
			return false;
		}
	}

	private CompleteBuildingAction(Identity target, ulong completeTime, List<ResidentInfo> residents, Reward reward)
		: base(null, null, null)
	{
	}

	public CompleteBuildingAction(Simulated simulated, List<Simulated> residents, Reward reward)
		: base(null, null, null)
	{
	}

	private CompleteBuildingAction(Identity target, ulong completeTime, string triggerType)
		: base(null, null, null)
	{
	}

	public new static CompleteBuildingAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Apply(Game game, ulong utcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}
}
