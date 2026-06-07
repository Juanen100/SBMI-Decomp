using System.Collections.Generic;

public class QuestBookendInfo
{
	public class ChunkConditions
	{
		public LoadableCondition Condition;

		public string Name;

		public string Icon;

		public ChunkConditions(LoadableCondition condition, string name, string icon)
		{
		}
	}

	public List<ChunkConditions> Chunks;

	public uint? DialogSequenceId;

	public float Postpone;

	private const string DIALOG_SEQUENCE_ID = "dialog_sequence_id";

	private const string POSTPONE = "postpone";

	private const string ARRAY = "array";

	private const string CONDITIONS = "conditions";

	public static QuestBookendInfo FromDict(Dictionary<string, object> data, bool chunkQuest, bool autoQuest)
	{
		return null;
	}

	public Dictionary<string, object> ToDict()
	{
		return null;
	}
}
