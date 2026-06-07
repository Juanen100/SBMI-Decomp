using System.Collections.Generic;
using UnityEngine;

public class AnimationEventData : AnimationEventHandlerDelegate
{
	private Dictionary<string, AnimationEventNodeDelegate> eventDict;

	private Dictionary<string, AnimationEventHandlerDelegate> handlerDict;

	public void LoadAnimationEventDataWithDictionary(Dictionary<string, object> dict)
	{
	}

	public void SetupAnimationEvents(GameObject rootGameObject, Animation unityAnimation, AnimationClip clip, AnimationEventManager mgr)
	{
	}

	public void HandleAnimationEvent(AnimationEvent animationEvent)
	{
	}
}
