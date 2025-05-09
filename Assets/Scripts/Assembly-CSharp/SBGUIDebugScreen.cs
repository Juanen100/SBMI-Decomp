using System;
using UnityEngine;

public class SBGUIDebugScreen : SBGUIModalDialog
{
	private SBGUILabel toggleFreeEditLabel;

	private SBGUILabel toggleFramerateCounterLabel;

	private SBGUILabel toggleHitBoxesLabel;

	private SBGUILabel toggleFootprintsLabel;

	private SBGUILabel toggleExpansionBordersLabel;

	private SBGUILabel toggleFreeCameraLabel;

	private SBGUILabel bundleVersionLabel;

	private SBGUILabel simTimeLabel;

	public override void SetParent(SBGUIElement element)
	{
		SetTransformParent(element);
	}

	public void Setup(Session session)
	{
		toggleFreeEditLabel = (SBGUILabel)FindChild("free_edit_label");
		toggleFramerateCounterLabel = (SBGUILabel)FindChild("framerate_counter_label");
		toggleHitBoxesLabel = (SBGUILabel)FindChild("toggle_hit_boxes_label");
		toggleFootprintsLabel = (SBGUILabel)FindChild("toggle_footprints_label");
		toggleExpansionBordersLabel = (SBGUILabel)FindChild("toggle_expansion_borders_label");
		toggleFreeCameraLabel = (SBGUILabel)FindChild("toggle_free_camera_label");
		simTimeLabel = (SBGUILabel)FindChild("sim_time_label");
		bundleVersionLabel = (SBGUILabel)FindChild("bundle_version_label");
		string deviceLanguage = Language.getDeviceLanguage();
		string deviceLocale = Language.getDeviceLocale();
		string text = string.Empty;
		if (SoaringInternal.Campaign != null)
		{
			text = SoaringInternal.Campaign.Group;
		}
		bundleVersionLabel.SetText("Version: " + SBSettings.BundleVersion + " U: " + Application.unityVersion + "\nLanguage: " + deviceLanguage + " Local: " + deviceLocale + " Group: " + text);
		Refresh();
	}

	public void Refresh()
	{
		if (Session.TheDebugManager.debugPlaceObjects)
		{
			toggleFreeEditLabel.SetText("Free Edit Mode: ON");
		}
		else
		{
			toggleFreeEditLabel.SetText("Free Edit Mode: OFF");
		}
		if (Session.TheDebugManager.framerateCounter)
		{
			toggleFramerateCounterLabel.SetText("Framerate Counter: ON");
		}
		else
		{
			toggleFramerateCounterLabel.SetText("Framerate Counter: OFF");
		}
		if (Session.TheDebugManager.showHitBoxes)
		{
			toggleHitBoxesLabel.SetText("Hit Boxes: ON");
		}
		else
		{
			toggleHitBoxesLabel.SetText("Hit Boxes: OFF");
		}
		if (Session.TheDebugManager.showFootprints)
		{
			toggleFootprintsLabel.SetText("Footprints: ON");
		}
		else
		{
			toggleFootprintsLabel.SetText("Footprints: OFF");
		}
		if (Session.TheDebugManager.showExpansionBorders)
		{
			toggleExpansionBordersLabel.SetText("Expansion Borders: ON");
		}
		else
		{
			toggleExpansionBordersLabel.SetText("Expansion Borders: OFF");
		}
		if (Session.TheDebugManager.freeCameraMode)
		{
			toggleFreeCameraLabel.SetText("Free Camera: ON");
		}
		else
		{
			toggleFreeCameraLabel.SetText("Free Camera: OFF");
		}
	}

	private new void Update()
	{
		DateTime utcNow = DateTime.UtcNow;
		string text = string.Format("{0:ddd, MMM d, yyyy}", utcNow);
		TimeSpan timeSpan = new DateTime(utcNow.Ticks + Convert.ToInt64(TFUtils.AddTimeOffset) * 10000000).Subtract(utcNow);
		int num = (int)timeSpan.TotalDays / 7;
		string text2 = "|+" + num.ToString("00") + ":" + (timeSpan.Days - num * 7).ToString("00") + ":" + timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
		simTimeLabel.SetText(text + text2);
	}
}
