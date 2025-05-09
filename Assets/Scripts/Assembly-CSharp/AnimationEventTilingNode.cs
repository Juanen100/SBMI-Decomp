using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTilingNode : AnimationEventHandlerDelegate, AnimationEventNodeDelegate
{
	private string boneName;

	private Vector2 tiling;

	private Dictionary<float, Vector2> offsets;

	public void HandleAnimationEvent(AnimationEvent animationEvent)
	{
		GameObject gameObject = (GameObject)animationEvent.objectReferenceParameter;
		Vector2 offset = offsets[animationEvent.time];
		SkinnedMeshRenderer component = gameObject.GetComponent<SkinnedMeshRenderer>();
		component.material.SetTextureOffset("_MainTex", offset);
	}

	public void SetupAnimationEvents(GameObject rootGameObject, AnimationClip clip, AnimationEventManager mgr)
	{
		GameObject gameObject = TFUtils.FindGameObjectInHierarchy(rootGameObject, boneName);
		if (!(gameObject != null))
		{
			return;
		}
		SkinnedMeshRenderer component = gameObject.GetComponent<SkinnedMeshRenderer>();
		component.material.SetTextureScale("_MainTex", tiling);
		foreach (float key in offsets.Keys)
		{
			float time = key;
			AnimationEvent animationEvent = new AnimationEvent();
			animationEvent.time = time;
			animationEvent.functionName = "HandleAnimationEvent";
			animationEvent.stringParameter = boneName;
			animationEvent.objectReferenceParameter = gameObject;
			clip.AddEvent(animationEvent);
		}
	}

	private Dictionary<float, Vector2> InitializeTilingOffsets(List<object> offsets)
	{
		Dictionary<float, Vector2> dictionary = new Dictionary<float, Vector2>();
		foreach (object offset in offsets)
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)offset;
			float key = TFUtils.LoadFloat(dictionary2, "time");
			Vector2 v;
			TFUtils.LoadVector2(out v, (Dictionary<string, object>)dictionary2["offset"]);
			dictionary.Add(key, v);
		}
		return dictionary;
	}

	public void InitializeWithData(Dictionary<string, object> dict)
	{
		boneName = (string)dict["name"];
		TFUtils.LoadVector2(out tiling, (Dictionary<string, object>)dict["tiling"]);
		offsets = InitializeTilingOffsets((List<object>)dict["key_frames"]);
	}
}
