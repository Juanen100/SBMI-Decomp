using System.Collections.Generic;

public class LevelUpDialogInputData : PersistedDialogInputData
{
	public const string DIALOG_TYPE = "level_up";

	private int newLevel;

	private List<Reward> rewards;

	public int NewLevel
	{
		get
		{
			return 0;
		}
	}

	public List<Reward> Rewards
	{
		get
		{
			return null;
		}
	}

	public LevelUpDialogInputData(int newLevel, List<Reward> rewards)
		: base(0u, null, null, null)
	{
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		return null;
	}

	public new static LevelUpDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		return null;
	}
}
