using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class SBGUISlidingLabel : SBGUILabel
{
	public delegate string UpdateText();

	private bool running;

	private UpdateText updateTextDelegate;

	public void AnimatedSliding(Vector2 endOffset, float endAlpha, float duration, bool destroyOnFinish = false, UpdateText updateText = null)
	{
	}

	[DebuggerHidden]
	private IEnumerator AnimatedSlidingCoroutine(Vector2 startPosition, Vector2 endPosition, float endAlpha, float duration, bool destroyOnFinish)
	{
		return null;
	}
}
