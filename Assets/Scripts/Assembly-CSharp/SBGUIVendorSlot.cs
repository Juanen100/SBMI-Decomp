using System;
using System.Collections;
using UnityEngine;

public class SBGUIVendorSlot : SBGUIElement
{
	protected const float PULSE_RATE = 0.75f;

	protected const float TINT_LOW = 0.5f;

	protected const float TINT_HIGH = 0.75f;

	protected const float SCALE_LOW = 1f;

	protected const float SCALE_HIGH = 1.2f;

	public SBGUIAtlasImage selectionStarburst;

	public SBGUIAtlasButton slotBackground;

	public SBGUILabel quantityLabel;

	public SBGUIAtlasImage quantityCircle;

	public SBGUIAtlasImage itemIcon;

	public SBGUIAtlasImage lockedMask;

	private int slotId;

	private bool empty;

	private Vector3 prefabItemIconPos;

	private Vector3 prefabStarburstPos;

	private bool isSpecial;

	private bool transitioning;

	protected bool lerpHigh = true;

	protected float specialInterp;

	protected float tintValue;

	protected float scaleValue;

	public bool Empty
	{
		get
		{
			return empty;
		}
	}

	public int SlotID
	{
		get
		{
			return slotId;
		}
		set
		{
			if (value >= 0)
			{
				slotId = value;
			}
		}
	}

	public bool IsSpecial
	{
		get
		{
			return isSpecial;
		}
		set
		{
			isSpecial = value;
		}
	}

	public static SBGUIVendorSlot CreateVendorSlot(Session session, SBGUIVendorScreen vendorScreen)
	{
		SBGUIVendorSlot slot = (SBGUIVendorSlot)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/VendorSlot");
		if (slot.selectionStarburst == null)
		{
			slot.selectionStarburst = (SBGUIAtlasImage)slot.FindChild("selection_starburst");
			slot.prefabStarburstPos = slot.selectionStarburst.tform.localPosition;
		}
		if (slot.slotBackground == null)
		{
			slot.slotBackground = (SBGUIAtlasButton)slot.FindChild("slot_background");
		}
		if (slot.quantityLabel == null)
		{
			slot.quantityLabel = (SBGUILabel)slot.FindChild("quantity_label");
		}
		if (slot.quantityCircle == null)
		{
			slot.quantityCircle = (SBGUIAtlasImage)slot.FindChild("quantity_circle");
		}
		if (slot.itemIcon == null)
		{
			slot.itemIcon = (SBGUIAtlasImage)slot.FindChild("icon");
			slot.prefabItemIconPos = slot.itemIcon.tform.localPosition;
		}
		if (slot.lockedMask == null)
		{
			slot.lockedMask = (SBGUIAtlasImage)slot.FindChild("locked_mask");
		}
		slot.SetHighlight(false);
		slot.AttachActionToButton("slot_background", delegate
		{
			if (!slot.empty)
			{
				session.TheSoundEffectManager.PlaySound("HighlightItem");
				vendorScreen.HighlightSlot(session, slot);
			}
			else
			{
				session.TheSoundEffectManager.PlaySound("Error");
			}
		});
		return slot;
	}

	public void SetHighlight(bool highlight, bool skipAnimation = false)
	{
		if (!skipAnimation && !empty)
		{
			if (highlight)
			{
				StopAllCoroutines();
				StartCoroutine(AnimateIn(1f, Easing.EaseOutElastic));
			}
			else
			{
				StopAllCoroutines();
				StartCoroutine(AnimateOut(0.2f, Easing.Linear));
			}
		}
		else if (selectionStarburst != null)
		{
			selectionStarburst.SetActive(highlight);
		}
	}

	public void SetEmpty(bool setting, bool specialVendingSlot = false)
	{
		empty = setting;
		if (specialVendingSlot)
		{
			slotBackground.SetActive(!setting);
		}
		else
		{
			Vector3 vector = new Vector3(0f, 0f, -0.04f);
			if (!setting)
			{
				itemIcon.tform.localPosition = prefabItemIconPos + vector;
				selectionStarburst.tform.localPosition = prefabStarburstPos + vector;
			}
			else
			{
				itemIcon.tform.localPosition = prefabItemIconPos - vector;
				selectionStarburst.tform.localPosition = prefabStarburstPos - vector;
			}
		}
		if (lockedMask != null)
		{
			lockedMask.SetActive(setting);
		}
	}

	private IEnumerator AnimateIn(float duration, Func<float, float, float, float> easingMethod)
	{
		if (!(selectionStarburst == null))
		{
			transitioning = true;
			selectionStarburst.SetActive(true);
			float interp = 0f;
			while (interp <= 1f)
			{
				interp += Time.deltaTime / duration;
				Vector3 interpPos = Easing.Vector3Easing(Vector3.zero, Vector3.one, interp, easingMethod);
				selectionStarburst.tform.localScale = interpPos;
				yield return null;
			}
			if (IsSpecial)
			{
				tintValue = 0.5f;
				scaleValue = 1f;
				lerpHigh = true;
				specialInterp = 0f;
			}
			transitioning = false;
		}
	}

	private IEnumerator AnimateOut(float duration, Func<float, float, float, float> easingMethod)
	{
		if (!(selectionStarburst == null))
		{
			transitioning = true;
			float interp = 0f;
			while (interp <= 1f)
			{
				interp += Time.deltaTime / duration;
				Vector3 interpPos = Easing.Vector3Easing(Vector3.one, Vector3.zero, interp, easingMethod);
				selectionStarburst.tform.localScale = interpPos;
				yield return null;
			}
			transitioning = false;
			selectionStarburst.SetActive(false);
		}
	}

	public static string GetSessionActionId(VendorDefinition vendorDef)
	{
		return string.Format("Slot_{0}", vendorDef.did);
	}

	public void Update()
	{
		if (!IsSpecial && selectionStarburst != null && selectionStarburst.IsActive())
		{
			selectionStarburst.tform.RotateAround(new Vector3(0f, 0f, 1f), -1f * Time.deltaTime);
		}
		if (!IsSpecial || !(selectionStarburst != null) || !selectionStarburst.IsActive() || transitioning)
		{
			return;
		}
		if (specialInterp <= 1f)
		{
			specialInterp += Time.deltaTime / 0.75f;
		}
		else
		{
			specialInterp = 0f;
			if (lerpHigh)
			{
				lerpHigh = false;
			}
			else
			{
				lerpHigh = true;
			}
		}
		if (lerpHigh)
		{
			tintValue = Mathf.SmoothStep(0.5f, 0.75f, specialInterp);
			scaleValue = Mathf.SmoothStep(1f, 1.2f, specialInterp);
		}
		else
		{
			tintValue = Mathf.SmoothStep(0.75f, 0.5f, specialInterp);
			scaleValue = Mathf.SmoothStep(1.2f, 1f, specialInterp);
		}
		selectionStarburst.GetComponent<MeshRenderer>().material.SetColor("_TintColor", new Color(tintValue, tintValue, tintValue, 1f));
		selectionStarburst.transform.localScale = new Vector3(scaleValue, 1f, 1f);
	}
}
