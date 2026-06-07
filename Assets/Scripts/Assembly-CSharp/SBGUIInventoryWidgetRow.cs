using System;
using Yarg;

public class SBGUIInventoryWidgetRow : SBGUIElement
{
	public SBGUIPulseButton Icon;

	public SBGUILabel Label;

	private int amount;

	private int productId;

	private int fakeDeduction;

	private Action<int, YGEvent> onDragCallback;

	private Action<YGEvent> onUiEventCallback;

	private SoundEffectManager sfxMgr;

	public int Product
	{
		get
		{
			return 0;
		}
	}

	public void Initialize(SoundEffectManager sfxMgr, Action<YGEvent> onUiEventCallback, Action<int, YGEvent> onDragCallback, string textureName)
	{
	}

	public override void SetVisible(bool viz)
	{
	}

	public void SetRecipeIcon(string texture)
	{
	}

	public void SetProductToTrack(int productId)
	{
	}

	private void SetAmount(int quantity)
	{
	}

	public void OnUpdate(ResourceManager resourceMgr, float topHideThreshold, float bottomHideThreshold)
	{
	}

	public void PulseError()
	{
	}

	public void PulseError(int count)
	{
	}

	public void IncrementDeductionsForTick()
	{
	}

	private void ResetToNeutral()
	{
	}

	private void HandleDragEvent(YGEvent evt)
	{
	}
}
