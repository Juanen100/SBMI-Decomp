using System.Collections.Generic;
using UnityEngine;

public class AnimationEventVisibility : AnimationEventHandlerDelegate, AnimationEventNodeDelegate
{
	private string eventName;

	private string meshName;

	private Dictionary<float, bool> visibilities;

	public void HandleAnimationEvent(AnimationEvent animationEvent)
	{
		GameObject gameObject = (GameObject)animationEvent.objectReferenceParameter;
		bool flag = visibilities[animationEvent.time];
		if (gameObject.GetComponent<Renderer>().isVisible != flag)
		{
			gameObject.GetComponent<Renderer>().enabled = flag;
		}
	}

	public void SetupAnimationEvents(GameObject rootGameObject, AnimationClip clip, AnimationEventManager mgr)
	{
		GameObject gameObject = TFUtils.FindGameObjectInHierarchy(rootGameObject, meshName);
		if (!(gameObject != null))
		{
			return;
		}
		foreach (float key in visibilities.Keys)
		{
			float time = key;
			AnimationEvent animationEvent = new AnimationEvent();
			animationEvent.time = time;
			animationEvent.functionName = "HandleAnimationEvent";
			animationEvent.stringParameter = eventName;
			animationEvent.objectReferenceParameter = gameObject;
			clip.AddEvent(animationEvent);
		}
	}

	public void InitializeWithData(Dictionary<string, object> dict)
	{
		eventName = (string)dict["name"];
		meshName = (string)dict["mesh"];
		visibilities = new Dictionary<float, bool>();
		List<object> list = (List<object>)dict["key_frames"];
		foreach (Dictionary<string, object> item in list)
		{
			visibilities.Add(TFUtils.LoadFloat(item, "time"), (bool)item["visible"]);
		}
	}
}
