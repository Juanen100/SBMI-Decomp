using System.Collections.Generic;
using UnityEngine;

public class AmazonGameCircleExample : MonoBehaviour
{
	private AmazonGameCircleExampleInitialization initializationMenu = new AmazonGameCircleExampleInitialization();

	private List<AmazonGameCircleExampleBase> gameCircleExampleMenus = new List<AmazonGameCircleExampleBase>();

	private bool initialized;

	private Vector2 scroll = Vector2.zero;

	private bool uiInitialized;

	private GUISkin localGuiSkin;

	private GUISkin originalGuiSkin;

	private void Start()
	{
		Initialize();
	}

	private void OnGUI()
	{
		InitializeUI();
		ApplyLocalUISkin();
		AmazonGameCircleExampleGUIHelpers.BeginMenuLayout();
		scroll = GUILayout.BeginScrollView(scroll);
		if (initializationMenu.InitializationStatus != AmazonGameCircleExampleInitialization.EInitializationStatus.Ready)
		{
			initializationMenu.DrawMenu();
		}
		else
		{
			foreach (AmazonGameCircleExampleBase gameCircleExampleMenu in gameCircleExampleMenus)
			{
				GUILayout.BeginVertical(GUI.skin.box);
				gameCircleExampleMenu.foldoutOpen = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(gameCircleExampleMenu.foldoutOpen, gameCircleExampleMenu.MenuTitle());
				if (gameCircleExampleMenu.foldoutOpen)
				{
					gameCircleExampleMenu.DrawMenu();
				}
				GUILayout.EndVertical();
			}
		}
		GUILayout.EndScrollView();
		AmazonGameCircleExampleGUIHelpers.EndMenuLayout();
		RevertLocalUISkin();
	}

	private void Initialize()
	{
		if (!initialized)
		{
			initialized = true;
			gameCircleExampleMenus.Add(new AmazonGameCircleExampleProfiles());
			gameCircleExampleMenus.Add(new AmazonGameCircleExampleAchievements());
			gameCircleExampleMenus.Add(new AmazonGameCircleExampleLeaderboards());
			gameCircleExampleMenus.Add(new AmazonGameCircleExampleWhispersync());
		}
	}

	private void InitializeUI()
	{
		if (!uiInitialized)
		{
			uiInitialized = true;
			localGuiSkin = GUI.skin;
			originalGuiSkin = GUI.skin;
			AmazonGameCircleExampleGUIHelpers.SetGUISkinTouchFriendly(localGuiSkin);
		}
	}

	private void ApplyLocalUISkin()
	{
		GUI.skin = localGuiSkin;
	}

	private void RevertLocalUISkin()
	{
		GUI.skin = originalGuiSkin;
	}
}
