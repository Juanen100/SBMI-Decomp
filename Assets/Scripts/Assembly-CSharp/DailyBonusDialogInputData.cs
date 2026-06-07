using System.Collections.Generic;

public class DailyBonusDialogInputData : PersistedDialogInputData
{
	public const string DIALOG_TYPE = "daily_bonus";

	private int currentDay;

	private bool alreadyCollected;

	private SoaringArray<SBMISoaring.SBMIDailyBonusDay> dailyBonusData;

	public SoaringArray<SBMISoaring.SBMIDailyBonusDay> DailyBonusData
	{
		get
		{
			return null;
		}
	}

	public int CurrentDay
	{
		get
		{
			return 0;
		}
	}

	public bool AlreadyCollected
	{
		get
		{
			return false;
		}
	}

	public DailyBonusDialogInputData()
		: base(0u, null, null, null)
	{
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		return null;
	}

	public new static DailyBonusDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		return null;
	}
}
