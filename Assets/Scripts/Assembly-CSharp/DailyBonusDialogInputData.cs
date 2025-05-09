using System.Collections.Generic;

public class DailyBonusDialogInputData : PersistedDialogInputData
{
	public const string DIALOG_TYPE = "daily_bonus";

	private int currentDay = -1;

	private bool alreadyCollected;

	private SoaringArray<SBMISoaring.SBMIDailyBonusDay> dailyBonusData;

	public SoaringArray<SBMISoaring.SBMIDailyBonusDay> DailyBonusData
	{
		get
		{
			return dailyBonusData;
		}
	}

	public int CurrentDay
	{
		get
		{
			return currentDay;
		}
	}

	public bool AlreadyCollected
	{
		get
		{
			return alreadyCollected;
		}
	}

	public DailyBonusDialogInputData()
		: base(uint.MaxValue, "daily_bonus", null, null)
	{
		dailyBonusData = SBMISoaring.GetCachedDailyBonus(ref currentDay, ref alreadyCollected);
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dict = new Dictionary<string, object>();
		BuildPersistenceDict(ref dict, "daily_bonus");
		dict["current_day"] = currentDay;
		dict["already_collected"] = alreadyCollected;
		dict["dailyBonus_data"] = dailyBonusData;
		return dict;
	}

	public new static DailyBonusDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		return new DailyBonusDialogInputData();
	}
}
