using System.Collections.Generic;

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
			this.id = null;
			this.did = 0;
			this.hungerId = null;
			this.prevHungerId = null;
			this.wishExpiresAt = null;
			this.hungryAt = 0uL;
			this.fullnessLength = null;
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
			return false;
		}
	}

	public MoveAction(Simulated simulated, List<Simulated> residents)
		: base(null, null, null)
	{
	}

	public MoveAction(Identity id, int? x, int? y, bool? flip, List<Simulated> residents)
		: base(null, null, null)
	{
	}

	private MoveAction(Identity id, int? x, int? y, bool? flip, List<ResidentInfo> residentInfos)
		: base(null, null, null)
	{
	}

	public new static MoveAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	private void InitializeResidents(List<Simulated> residents)
	{
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
