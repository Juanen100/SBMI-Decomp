using System;

public class SBGUIProgressDialog : SBGUIModalDialog
{
	private SBGUIProgressMeter meter;

	private SBGUILabel durationLabel;

	private SBGUILabel rushLabel;

	private int maxJellyCost = 1;

	protected override void Awake()
	{
		rewardMarker = FindChild("reward_marker");
		meter = (SBGUIProgressMeter)FindChild("progress_meter");
		durationLabel = (SBGUILabel)FindChild("duration_label");
		base.Awake();
	}

	public void Setup(string title, string description, Action onClose)
	{
		Cost cost = new Cost();
		cost.ResourceAmounts[0] = 0;
		Setup(title, description, onClose, false, cost, null);
	}

	public void Setup(string title, string description, Action onClose, bool rewardVisible, Cost rushCost, Action onRush)
	{
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("building_label");
		SBGUILabel sBGUILabel2 = (SBGUILabel)FindChild("task_label");
		sBGUILabel.SetText(title);
		sBGUILabel2.SetText(description);
		SBGUIElement sBGUIElement = FindChild("close");
		SBGUIButton component = sBGUIElement.GetComponent<SBGUIButton>();
		AttachAnalyticsToButton("close", component);
		component.ClickEvent += delegate
		{
			Close();
		};
		component.ClickEvent += onClose;
		SBGUIElement sBGUIElement2 = FindChild("rush_button");
		if (onRush == null)
		{
			sBGUIElement2.SetActive(false);
		}
		else
		{
			SBGUIButton component2 = sBGUIElement2.GetComponent<SBGUIButton>();
			AttachAnalyticsToButton("rush", component2);
			component2.ClickEvent += delegate
			{
				Close();
			};
			component2.ClickEvent += onRush;
			rushLabel = (SBGUILabel)FindChild("rush_cost_label");
			rushLabel.SetText(rushCost.ResourceAmounts[rushCost.GetOnlyCostKey()].ToString());
			maxJellyCost = rushCost.ResourceAmounts[rushCost.GetOnlyCostKey()];
		}
		if (!rewardVisible)
		{
			SBGUIElement sBGUIElement3 = FindChild("reward");
			sBGUIElement3.SetActive(false);
		}
	}
}
