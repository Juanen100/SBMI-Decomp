using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTilingNode : AnimationEventHandlerDelegate, AnimationEventNodeDelegate
{
	private string boneName;

	private Vector2 tiling;

	private Dictionary<float, Vector2> offsets;

	public void HandleAnimationEvent(AnimationEvent animationEvent)
	{
	}

	public void SetupAnimationEvents(GameObject rootGameObject, AnimationClip clip, AnimationEventManager mgr)
	{
	}

	private Dictionary<float, Vector2> InitializeTilingOffsets(List<object> offsets)
	{
		return null;
	}

	public void InitializeWithData(Dictionary<string, object> dict)
	{
	}
}
