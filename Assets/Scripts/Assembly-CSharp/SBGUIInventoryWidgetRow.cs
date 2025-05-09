#define ASSERTS_ON
using System;
using UnityEngine;
using Yarg;

[RequireComponent(typeof(DragButton))]
[RequireComponent(typeof(YG2DRectangle))]
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
			return productId;
		}
	}

	public void Initialize(SoundEffectManager sfxMgr, Action<YGEvent> onUiEventCallback, Action<int, YGEvent> onDragCallback, string textureName)
	{
		TFUtils.Assert(Icon != null, "Must set an Icon on this InventoryWidgetRow");
		TFUtils.Assert(Label != null, "Must set a Label on this InventoryWidgetRow");
		this.sfxMgr = sfxMgr;
		this.onDragCallback = onDragCallback;
		this.onUiEventCallback = onUiEventCallback;
		Icon.InitializePulser(Icon.RestingSize, Icon.Amplitude, Icon.Period, ResetToNeutral);
		Icon.SetTextureFromAtlas(textureName);
		DragButton component = Icon.GetComponent<DragButton>();
		component.DragEvent.AddListener(HandleDragEvent);
		ResetToNeutral();
		GetComponent<DragButton>().DragEvent.AddListener(this.onUiEventCallback);
	}

	public override void SetVisible(bool viz)
	{
		base.SetVisible(viz);
		Icon.SetVisible(viz);
		Label.SetVisible(viz);
	}

	public void SetRecipeIcon(string texture)
	{
		Icon.SetTextureFromAtlas(texture);
	}

	public void SetProductToTrack(int productId)
	{
		this.productId = productId;
		SessionActionId = "GrubWidget_Row_" + productId;
		Icon.SessionActionId = "GrubWidget_Icon_" + productId;
	}

	private void SetAmount(int quantity)
	{
		if (amount != quantity)
		{
			amount = quantity;
			Label.SetText(quantity.ToString());
		}
	}

	public void OnUpdate(ResourceManager resourceMgr, float topHideThreshold, float bottomHideThreshold)
	{
		Vector2 screenPosition = GetScreenPosition();
		if (screenPosition.y > bottomHideThreshold || screenPosition.y < topHideThreshold)
		{
			if (IsActive())
			{
				SetActive(false);
			}
		}
		else if (!IsActive())
		{
			SetActive(true);
		}
		SetAmount(resourceMgr.Query(productId) - fakeDeduction);
		fakeDeduction = 0;
	}

	public void PulseError()
	{
		PulseError(3);
	}

	public void PulseError(int count)
	{
		Icon.Pulser.PulseOneShot(count);
		Label.SetColor(Color.red);
	}

	public void IncrementDeductionsForTick()
	{
		fakeDeduction++;
	}

	private void ResetToNeutral()
	{
		Label.SetColor(Color.white);
	}

	private void HandleDragEvent(YGEvent evt)
	{
		onUiEventCallback(evt);
		if (evt.type == YGEvent.TYPE.TOUCH_BEGIN)
		{
			if (amount > 0)
			{
				onDragCallback(productId, evt);
				return;
			}
			sfxMgr.PlaySound("Error");
			PulseError(0);
		}
	}
}
