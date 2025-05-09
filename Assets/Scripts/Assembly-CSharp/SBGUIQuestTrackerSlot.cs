using UnityEngine;

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
		Vector2 screenPosition = GetScreenPosition();
		if (screenPosition.y < upperBound)
		{
			base.transform.Find("questbackground").GetComponent<Renderer>().enabled = false;
			base.GetComponent<Renderer>().enabled = false;
			return QuestTrackerState.AboveBounds;
		}
		if (screenPosition.y > lowerBound)
		{
			base.GetComponent<Renderer>().enabled = false;
			base.transform.Find("questbackground").GetComponent<Renderer>().enabled = false;
			return QuestTrackerState.BelowBounds;
		}
		if (base.transform.parent.gameObject.active)
		{
			base.GetComponent<Renderer>().enabled = true;
			base.transform.Find("questbackground").GetComponent<Renderer>().enabled = true;
		}
		return QuestTrackerState.InBounds;
	}
}
