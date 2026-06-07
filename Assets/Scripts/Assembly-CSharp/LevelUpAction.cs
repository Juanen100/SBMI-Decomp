using System.Collections.Generic;

public class LevelUpAction : PersistedTriggerableAction
{
	public const string LEVEL_UP = "lu";

	private const string WALLTIME_START_PREVIOUS_LEVEL = "wts_begin";

	private const string PLAYTIME_TO_LEVEL = "time_to";

	private Reward reward;

	private Dictionary<string, object> buildingLabels;

	private ulong buildCompleteTime;

	private int level;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public LevelUpAction(int level, Reward reward, ulong buildCompleteTime)
		: base(null, null)
	{
	}

	public new static LevelUpAction FromDict(Dictionary<string, object> data)
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
