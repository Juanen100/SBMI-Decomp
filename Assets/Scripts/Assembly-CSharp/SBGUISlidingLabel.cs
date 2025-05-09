using System.Collections;
using UnityEngine;

public class SBGUISlidingLabel : SBGUILabel
{
	public delegate string UpdateText();

	private bool running;

	private UpdateText updateTextDelegate;

	public void AnimatedSliding(Vector2 endOffset, float endAlpha, float duration, bool destroyOnFinish = false, UpdateText updateText = null)
	{
		if (!running)
		{
			Vector2 screenPosition = GetScreenPosition();
			StartCoroutine(AnimatedSlidingCoroutine(endPosition: new Vector2(screenPosition.x + endOffset.x, screenPosition.y + endOffset.y), startPosition: screenPosition, endAlpha: endAlpha, duration: duration, destroyOnFinish: destroyOnFinish));
			updateTextDelegate = updateText;
		}
	}

	private IEnumerator AnimatedSlidingCoroutine(Vector2 startPosition, Vector2 endPosition, float endAlpha, float duration, bool destroyOnFinish)
	{
		if (running)
		{
			yield return null;
		}
		float startAlpha = 1f;
		float elapsed = 0f;
		running = true;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float elapsedOverDuration = elapsed / duration;
			float currentX = Mathf.Lerp(startPosition.x, endPosition.x, elapsedOverDuration);
			float currentY = Mathf.Lerp(startPosition.y, endPosition.y, elapsedOverDuration);
			float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedOverDuration);
			SetAlpha(currentAlpha);
			SetScreenPosition(currentX, currentY);
			if (updateTextDelegate != null)
			{
				SetText(updateTextDelegate());
			}
			yield return null;
		}
		running = false;
		if (destroyOnFinish)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
