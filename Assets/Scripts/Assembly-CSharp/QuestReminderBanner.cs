using System;
using UnityEngine;

public class QuestReminderBanner : ClickableUiPointer
{
	private const string PREFAB_NAME = "Prefabs/GUI/Widgets/QuestReminder_Banner";

	private SBGUIPulseButton bannerSubElement;

	private JumpPattern periodicSquisher;

	public void Spawn(Game game, SessionActionTracker parentAction, SBGUIElement parentElement, SBGUIScreen containingScreen, Action clickHandler, string barTexture, string circleTexture)
	{
	}

	protected void Initialize(Game game, SessionActionTracker action, SBGUIElement parentElement, SBGUIScreen containingScreen, Action clickHandler, Vector3 offset, float rotationCwDeg, float alpha, Vector2 scale, string barTexture, string circleTexture)
	{
	}

	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		return default(SessionActionManager.SpawnReturnCode);
	}
}
