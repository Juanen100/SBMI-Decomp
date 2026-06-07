using System;
using System.Collections.Generic;
using UnityEngine;

public class SBGUIAutoQuestStatusDialog : SBGUIScrollableDialog
{
	public const int STEP_GAP = 0;

	private int? questIconSize;

	private float markerXOffset;

	private Vector2 scrollSize;

	private SBGUIPulseButton okayButton;

	private SBGUIPulseButton allDoneButton;

	private SBGUIAtlasImage window;

	private SBGUIElement stepsMarker;

	private int numChunksLeft;

	public override void SetParent(SBGUIElement element)
	{
	}

	public void CreateScrollRegionUI(SBGUIStandardScreen screen, List<QuestBookendInfo.ChunkConditions> chunks, List<ConditionDescription> steps, Action makeButtonHandler, string forcedStepPrefabName = null)
	{
	}

	public void SetupDialogInfo(string sDialogHeading, string sDialogBody, string sPortrait, List<Reward> pRewards, List<ConditionDescription> steps, QuestDefinition pQuestDef)
	{
	}
}
