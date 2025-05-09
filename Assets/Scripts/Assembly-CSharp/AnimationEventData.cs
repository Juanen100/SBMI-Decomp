using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventData : AnimationEventHandlerDelegate
{
	private Dictionary<string, AnimationEventNodeDelegate> eventDict;

	private Dictionary<string, AnimationEventHandlerDelegate> handlerDict;

	public AnimationEventData()
	{
		eventDict = new Dictionary<string, AnimationEventNodeDelegate>();
		handlerDict = new Dictionary<string, AnimationEventHandlerDelegate>();
	}

	public void LoadAnimationEventDataWithDictionary(Dictionary<string, object> dict)
	{
		if (!dict.ContainsKey("nodes"))
		{
			return;
		}
		List<object> list = (List<object>)dict["nodes"];
		foreach (object item in list)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)item;
			string text = (string)dictionary["type"];
			string key = (string)dictionary["name"];
			if (text.Equals("tiling_offset"))
			{
				AnimationEventTilingNode animationEventTilingNode = new AnimationEventTilingNode();
				animationEventTilingNode.InitializeWithData(dictionary);
				eventDict.Add(key, animationEventTilingNode);
				handlerDict.Add(key, animationEventTilingNode);
				continue;
			}
			if (text.Equals("particles"))
			{
				AnimationEventParticlesNode animationEventParticlesNode = new AnimationEventParticlesNode();
				animationEventParticlesNode.InitializeWithData(dictionary);
				eventDict.Add(key, animationEventParticlesNode);
				handlerDict.Add(key, animationEventParticlesNode);
				continue;
			}
			if (text.Equals("visibility"))
			{
				AnimationEventVisibility animationEventVisibility = new AnimationEventVisibility();
				animationEventVisibility.InitializeWithData(dictionary);
				eventDict.Add(key, animationEventVisibility);
				handlerDict.Add(key, animationEventVisibility);
				continue;
			}
			throw new NotImplementedException(string.Format("animation event type {0} not implemented", text));
		}
	}

	public void SetupAnimationEvents(GameObject rootGameObject, Animation unityAnimation, AnimationClip clip, AnimationEventManager mgr)
	{
		AnimationEventHandlerComponent animationEventHandlerComponent = unityAnimation.gameObject.GetComponent<AnimationEventHandlerComponent>();
		if (animationEventHandlerComponent == null)
		{
			animationEventHandlerComponent = unityAnimation.gameObject.AddComponent<AnimationEventHandlerComponent>();
		}
		animationEventHandlerComponent.animationEventHandlerDelegate = this;
		foreach (string key in eventDict.Keys)
		{
			AnimationEventNodeDelegate animationEventNodeDelegate = eventDict[key];
			animationEventNodeDelegate.SetupAnimationEvents(rootGameObject, clip, mgr);
		}
	}

	public void HandleAnimationEvent(AnimationEvent animationEvent)
	{
		AnimationEventHandlerDelegate animationEventHandlerDelegate = handlerDict[animationEvent.stringParameter];
		if (animationEventHandlerDelegate != null)
		{
			animationEventHandlerDelegate.HandleAnimationEvent(animationEvent);
		}
	}
}
