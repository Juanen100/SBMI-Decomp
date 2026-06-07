using System.Collections.Generic;
using UnityEngine;

public class AnimationEventVisibility : AnimationEventHandlerDelegate, AnimationEventNodeDelegate
{
	private string eventName;

	private string meshName;

	private Dictionary<float, bool> visibilities;

	public void HandleAnimationEvent(AnimationEvent animationEvent)
	{
	}

	public void SetupAnimationEvents(GameObject rootGameObject, AnimationClip clip, AnimationEventManager mgr)
	{
	}

	public void InitializeWithData(Dictionary<string, object> dict)
	{
	}
}
