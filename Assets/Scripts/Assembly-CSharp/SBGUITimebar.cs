using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SBGUITimebar : SBGUIElement
{
	public delegate Vector3 HostPosition();

	public delegate float EasingFunc(float start, float end, float duration);

	public delegate bool UpdateProgress();

	public float elapsed;

	private Dictionary<string, SBGUIElement> dict;

	private SBGUIProgressMeter meter;

	private SBGUILabel durationLabel;

	private SBGUILabel rushLabel;

	private SBGUIButton rushButton;

	private SBGUIButton watchAdButton;

	private Action closeFinishedAction;

	private SBGUICharacterArrowList m_pTaskCharacterList;

	private int maxJellyCost;

	private string originalRushButtonSessionActionId;

	public SBGUIButton RushButton
	{
		get
		{
			return null;
		}
	}

	protected override void Awake()
	{
	}

	public void Setup(Session session, uint ownerDid, string description, ulong completeTime, ulong totalTime, float duration, Cost rushCost, Action onRush, HostPosition hPosition, Action onFinish, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked, Action onWatchAd)
	{
	}

	public Vector2 GetRushButtonScreenPosition()
	{
		return default(Vector2);
	}

	[DebuggerHidden]
	private IEnumerator ScaleCoroutine(float startScale, float endScale, float duration, EasingFunc easing)
	{
		return null;
	}

	[DebuggerHidden]
	private IEnumerator TimeoutCoroutine(float duration, HostPosition hPosition, UpdateProgress updateProgress)
	{
		return null;
	}

	public void SetProgress(float percent, ulong duration)
	{
	}

	[DebuggerHidden]
	private IEnumerator CloseCoroutine()
	{
		return null;
	}

	public void Close()
	{
	}

	public void RemoveCompleteAction()
	{
	}
}
