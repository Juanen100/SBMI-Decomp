#define ASSERTS_ON
using System;
using UnityEngine;

public class QuestReminderBanner : ClickableUiPointer
{
	private const string PREFAB_NAME = "Prefabs/GUI/Widgets/QuestReminder_Banner";

	private SBGUIPulseButton bannerSubElement;

	private JumpPattern periodicSquisher;

	public void Spawn(Game game, SessionActionTracker parentAction, SBGUIElement parentElement, SBGUIScreen containingScreen, Action clickHandler, string barTexture, string circleTexture)
	{
		QuestReminderBanner questReminderBanner = new QuestReminderBanner();
		questReminderBanner.Initialize(game, parentAction, parentElement, containingScreen, clickHandler, offset + new Vector3(0f, 0f, 1f), base.Rotation, base.Alpha, base.Scale, barTexture, circleTexture);
	}

	protected void Initialize(Game game, SessionActionTracker action, SBGUIElement parentElement, SBGUIScreen containingScreen, Action clickHandler, Vector3 offset, float rotationCwDeg, float alpha, Vector2 scale, string barTexture, string circleTexture)
	{
		base.Initialize(game, action, offset, rotationCwDeg, alpha, scale, parentElement, containingScreen, "Prefabs/GUI/Widgets/QuestReminder_Banner");
		if (barTexture != null)
		{
			SBGUIPulseButton sBGUIPulseButton = (SBGUIPulseButton)base.Element.FindChild("QuestReminder_Bar");
			sBGUIPulseButton.SetTextureFromAtlas(barTexture);
		}
		if (circleTexture != null)
		{
			SBGUIPulseImage sBGUIPulseImage = (SBGUIPulseImage)base.Element;
			sBGUIPulseImage.SetTextureFromAtlas(circleTexture);
		}
		bannerSubElement = base.Element.FindChild("QuestReminder_Bar").gameObject.GetComponent<SBGUIPulseButton>();
		SBGUIPulseButton component = bannerSubElement.gameObject.GetComponent<SBGUIPulseButton>();
		component.ClickEvent += clickHandler;
		TFUtils.Assert(bannerSubElement != null, "Could not find child Quest Reminder Bar on prefab!");
		base.Element.gameObject.transform.localPosition = new Vector3(0f, 0f, 0.05f);
		SBGUIPulseImage component2 = base.Element.gameObject.GetComponent<SBGUIPulseImage>();
		SBGUIAtlasImage component3 = base.Element.gameObject.GetComponent<SBGUIAtlasImage>();
		component2.InitializePulser(component3.Size, 1.5f, 0.25f);
		if (!TFPerfUtils.IsNonScalingDevice())
		{
			component2.Pulser.PulseOneShot();
		}
		periodicSquisher = new JumpPattern(-1f, 2f, 0.5f, 0.15f, 0f, Time.time, Vector2.one);
	}

	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		if (periodicSquisher != null)
		{
			float val;
			Vector2 squish;
			periodicSquisher.ValueAndSquishAtTime(Time.time, out val, out squish);
			if (!TFPerfUtils.IsNonScalingDevice())
			{
				bannerSubElement.gameObject.transform.localScale = squish;
			}
		}
		if (parentAction.Status == SessionActionTracker.StatusCode.FINISHED_SUCCESS || parentAction.Status == SessionActionTracker.StatusCode.FINISHED_FAILURE || parentAction.Status == SessionActionTracker.StatusCode.OBLITERATED)
		{
			Destroy();
			return SessionActionManager.SpawnReturnCode.KILL;
		}
		return SessionActionManager.SpawnReturnCode.KEEP_ALIVE;
	}
}
