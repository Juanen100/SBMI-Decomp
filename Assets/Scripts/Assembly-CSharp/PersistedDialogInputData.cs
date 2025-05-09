#define ASSERTS_ON
using System.Collections.Generic;

public abstract class PersistedDialogInputData : DialogInputData
{
	public PersistedDialogInputData(uint sequenceId, string type, string soundImmediate, string soundBeat)
		: base(sequenceId, type, soundImmediate, soundBeat)
	{
	}

	public static PersistedDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		string text = TFUtils.LoadString(dict, "type");
		switch (text)
		{
		case "character":
			return CharacterDialogInputData.FromPersistenceDict(dict);
		case "quest_start":
			return QuestStartDialogInputData.FromPersistenceDict(dict);
		case "quest_complete":
			return QuestCompleteDialogInputData.FromPersistenceDict(dict);
		case "quest_line_start":
			return QuestLineStartDialogInputData.FromPersistenceDict(dict);
		case "quest_line_complete":
			return QuestLineCompleteDialogInputData.FromPersistenceDict(dict);
		case "booty_quest_complete":
			return BootyQuestCompleteDialogInputData.FromPersistenceDict(dict);
		case "level_up":
			return LevelUpDialogInputData.FromPersistenceDict(dict);
		case "found_movie":
			return FoundMovieDialogInputData.FromPersistenceDict(dict);
		case "found_item":
			return FoundItemDialogInputData.FromPersistenceDict(dict);
		case "explanation":
			return ExplanationDialogInputData.FromPersistenceDict(dict);
		case "movein":
			return MoveInDialogInputData.FromPersistenceDict(dict);
		case "found_treasure":
			return TreasureDialogInputData.FromPersistenceDict(dict);
		case "spongy_games":
			return SpongyGamesDialogInputData.FromPersistenceDict(dict);
		case "daily_bonus":
			return DailyBonusDialogInputData.FromPersistenceDict(dict);
		default:
			TFUtils.Assert(false, "Unexpected dialog type:  " + text);
			return null;
		}
	}

	public abstract Dictionary<string, object> ToPersistenceDict();

	protected virtual void BuildPersistenceDict(ref Dictionary<string, object> dict, string dialogType)
	{
		dict["type"] = dialogType;
	}
}
