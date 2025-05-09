#define ASSERTS_ON
using System;
using System.Collections.Generic;
using UnityEngine;

public class SessionActionTextPromptPrefab : SBGUIElement
{
	public float ZDepth = 0.1f;

	public Vector2 BottomOffset = Vector2.zero;

	public Vector2 CenterOffset = Vector2.zero;

	public Vector2 TopOffset = Vector2.zero;

	public Vector3 LowResolutionScale = Vector3.one;

	private SBGUIButton frame;

	private SBGUILabel label;

	private SBGUIAtlasImage labelBoundary;

	private Dictionary<TextPrompt.Anchor, SBGUIElement> anchors = new Dictionary<TextPrompt.Anchor, SBGUIElement>();

	private Dictionary<TextPrompt.Anchor, Vector3> offsets = new Dictionary<TextPrompt.Anchor, Vector3>();

	public void SetLabel(string text)
	{
		label.SetText(text);
		label.AdjustText(labelBoundary);
	}

	public void SetAnchoredPosition(TextPrompt.Anchor position)
	{
		frame.transform.localPosition = anchors[position].transform.localPosition + offsets[position];
	}

	public void SetClickCallback(Action clickCallback)
	{
		frame.ClearClickEvents();
		frame.ClickEvent += clickCallback;
	}

	protected override void Awake()
	{
		frame = FindChild("speech_bubble").gameObject.GetComponent<SBGUIButton>();
		label = (SBGUILabel)FindChild("label");
		labelBoundary = (SBGUIAtlasImage)FindChild("label_boundary");
		SBGUIElement sBGUIElement = FindChild("top");
		SBGUIElement sBGUIElement2 = FindChild("center");
		SBGUIElement sBGUIElement3 = FindChild("bottom");
		TFUtils.Assert(frame != null, "Could not find gameobject child named 'speech_bubble'!");
		TFUtils.Assert(label != null, "Could not find gameobject child named 'label'!");
		TFUtils.Assert(sBGUIElement != null, "Could not find gameobject child named 'top'!");
		TFUtils.Assert(sBGUIElement2 != null, "Could not find gameobject child named 'center'!");
		TFUtils.Assert(sBGUIElement3 != null, "Could not find gameobject child named 'bottom'!");
		anchors[TextPrompt.Anchor.Top] = sBGUIElement;
		anchors[TextPrompt.Anchor.Center] = sBGUIElement2;
		anchors[TextPrompt.Anchor.Bottom] = sBGUIElement3;
		offsets[TextPrompt.Anchor.Top] = new Vector3(TopOffset.x, TopOffset.y, ZDepth);
		offsets[TextPrompt.Anchor.Center] = new Vector3(CenterOffset.x, CenterOffset.y, ZDepth);
		offsets[TextPrompt.Anchor.Bottom] = new Vector3(BottomOffset.x, BottomOffset.y, ZDepth);
		if (TFUtils.GetDeviceLandscapeAspectRatio() == "3:2")
		{
			base.transform.localScale = LowResolutionScale;
		}
	}
}
