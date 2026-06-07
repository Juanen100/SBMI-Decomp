using System;
using System.Collections.Generic;
using UnityEngine;

public class SessionActionTextPromptPrefab : SBGUIElement
{
	public float ZDepth;

	public Vector2 BottomOffset;

	public Vector2 CenterOffset;

	public Vector2 TopOffset;

	public Vector3 LowResolutionScale;

	private SBGUIButton frame;

	private SBGUILabel label;

	private SBGUIAtlasImage labelBoundary;

	private Dictionary<TextPrompt.Anchor, SBGUIElement> anchors;

	private Dictionary<TextPrompt.Anchor, Vector3> offsets;

	public void SetLabel(string text)
	{
	}

	public void SetAnchoredPosition(TextPrompt.Anchor position)
	{
	}

	public void SetClickCallback(Action clickCallback)
	{
	}

	protected override void Awake()
	{
	}
}
