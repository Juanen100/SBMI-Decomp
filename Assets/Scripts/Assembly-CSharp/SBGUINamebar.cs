using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SBGUINamebar : SBGUIElement
{
	public delegate Vector3 HostPosition();

	public delegate float EasingFunc(float start, float end, float duration);

	public delegate bool UpdateProgress();

	public float elapsed;

	private Dictionary<string, SBGUIElement> dict;

	private SBGUILabel nameLabel;

	private Action closeFinishedAction;

	private SBGUICharacterArrowList m_pTaskCharacterList;

	private HostPosition m_hPosition;

	protected override void Awake()
	{
	}

	public void Setup(Session session, string name, HostPosition hPosition, Action onFinish, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
	}

	private void Update()
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
	private IEnumerator TimeoutCoroutine(float duration, HostPosition hPosition)
	{
		return null;
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
