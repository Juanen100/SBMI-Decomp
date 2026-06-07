public class SBGUIQuestTrackerSlot : SBGUIAtlasButton
{
	public enum QuestTrackerState
	{
		InBounds = 0,
		AboveBounds = 1,
		BelowBounds = 2
	}

	public QuestTrackerState OnUpdate(float upperBound, float lowerBound)
	{
		return default(QuestTrackerState);
	}
}
