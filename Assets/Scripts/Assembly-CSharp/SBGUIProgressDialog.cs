using System;

public class SBGUIProgressDialog : SBGUIModalDialog
{
	private SBGUIProgressMeter meter;

	private SBGUILabel durationLabel;

	private SBGUILabel rushLabel;

	private int maxJellyCost;

	protected override void Awake()
	{
	}

	public void Setup(string title, string description, Action onClose)
	{
	}

	public void Setup(string title, string description, Action onClose, bool rewardVisible, Cost rushCost, Action onRush)
	{
	}
}
