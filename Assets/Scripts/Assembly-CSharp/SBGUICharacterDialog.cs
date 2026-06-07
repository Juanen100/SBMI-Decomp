using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SBGUICharacterDialog : SBGUIModalDialog
{
	public delegate float EasingFunc(float start, float end, float duration);

	public List<DialogPrompt> prompts;

	private SBGUIAtlasImage characterIcon;

	private SBGUILabel dialogText;

	private SBGUIButton skipButton;

	private SBGUIAtlasImage speechBubble;

	private SBGUIAtlasImage dialogBoundary;

	private int dialogIndex;

	private DialogPrompt currentPrompt;

	private const float DELAY_BETWEEN_LETTERS = 0.025f;

	private bool currentlyTyping;

	private string localizedPrompt;

	private Action m_pSkipAction;

	public EventDispatcher<int> DialogChange;

	public bool autoPlay;

	private Vector3 m_pPortraitSize;

	private Vector3 characterPosition;

	private Bounds viewBounds;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void StartSequence()
	{
	}

	public void LoadSequence(List<object> sequence)
	{
	}

	public void ShowDialog(int index)
	{
	}

	[DebuggerHidden]
	private IEnumerator AnimateOut(float duration, Action completeAction)
	{
		return null;
	}

	[DebuggerHidden]
	private IEnumerator AnimateIn(float duration, Action completeAction)
	{
		return null;
	}

	private void AdjustText()
	{
	}

	[DebuggerHidden]
	private IEnumerator TypeText()
	{
		return null;
	}

	private void StopTyping()
	{
	}

	protected override void OnDisable()
	{
	}
}
