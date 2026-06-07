using System;
using System.Collections.Generic;

public class SBGUIQuestDialog : SBGUIModalDialog
{
	public const int STEP_GAP = 4;

	private double residentPosX;

	private double residentPosY;

	private int? prefabIconSize;

	protected override void Awake()
	{
	}

	public override void SetParent(SBGUIElement element)
	{
	}

	public void SetupQuestDialogInfo(string name, string icon, List<ConditionDescription> steps, bool hasCount, List<QuestBookendInfo.ChunkConditions> chunks, Action findButtonHandler)
	{
	}

	public void SetupQuestDialogInfo(string name, string icon, List<ConditionDescription> steps, bool hasCount)
	{
	}

	public void SetupQuestDialogInfo(string name, string icon)
	{
	}
}
