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
			return newLevel;
		}
	}

	public List<Reward> Rewards
	{
		get
		{
			return rewards;
		}
	}

	public LevelUpDialogInputData(int newLevel, List<Reward> rewards)
		: base(uint.MaxValue, "level_up", "Dialog_LevelUp", null)
	{
		this.newLevel = newLevel;
		this.rewards = rewards;
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["type"] = "level_up";
		dictionary["level"] = newLevel;
		List<object> list = new List<object>();
		foreach (Reward reward in rewards)
		{
			list.Add(Reward.RewardToDict(reward));
		}
		dictionary["rewards"] = list;
		return dictionary;
	}

	public new static LevelUpDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		int num = TFUtils.LoadInt(dict, "level");
		List<object> list = (List<object>)dict["rewards"];
		List<Reward> list2 = new List<Reward>();
		foreach (object item in list)
		{
			list2.Add(Reward.FromObject(item));
		}
		return new LevelUpDialogInputData(num, list2);
	}
}
