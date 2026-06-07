using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class SBGUIVendorSlot : SBGUIElement
{
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

	protected bool lerpHigh;

	protected float specialInterp;

	protected float tintValue;

	protected float scaleValue;

	protected const float PULSE_RATE = 0.75f;

	protected const float TINT_LOW = 0.5f;

	protected const float TINT_HIGH = 0.75f;

	protected const float SCALE_LOW = 1f;

	protected const float SCALE_HIGH = 1.2f;

	public bool Empty
	{
		get
		{
			return false;
		}
	}

	public int SlotID
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public bool IsSpecial
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static SBGUIVendorSlot CreateVendorSlot(Session session, SBGUIVendorScreen vendorScreen)
	{
		return null;
	}

	public void SetHighlight(bool highlight, bool skipAnimation = false)
	{
	}

	public void SetEmpty(bool setting, bool specialVendingSlot = false)
	{
	}

	[DebuggerHidden]
	private IEnumerator AnimateIn(float duration, Func<float, float, float, float> easingMethod)
	{
		return null;
	}

	[DebuggerHidden]
	private IEnumerator AnimateOut(float duration, Func<float, float, float, float> easingMethod)
	{
		return null;
	}

	public static string GetSessionActionId(VendorDefinition vendorDef)
	{
		return null;
	}

	public void Update()
	{
	}
}
