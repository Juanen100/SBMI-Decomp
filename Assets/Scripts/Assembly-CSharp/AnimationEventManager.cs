#define ASSERTS_ON
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

public class AnimationEventManager
{
	public delegate void UpdateWithParticleSystemManagerDelegate(ParticleSystemManager psm);

	private Dictionary<string, AnimationEventData> animationEvents;

	private List<UpdateWithParticleSystemManagerDelegate> particleSystemManagerDelegates;

	public AnimationEventManager()
	{
		animationEvents = new Dictionary<string, AnimationEventData>();
		particleSystemManagerDelegates = new List<UpdateWithParticleSystemManagerDelegate>();
	}

	public void AddAnimationEventsWithFile(string animationEventsFile)
	{
		if (!animationEvents.ContainsKey(animationEventsFile))
		{
			AnimationEventData animationEventData = new AnimationEventData();
			TextAsset textAsset = (TextAsset)Resources.Load(animationEventsFile, typeof(TextAsset));
			TFUtils.Assert(textAsset != null, animationEventsFile);
			Dictionary<string, object> dict = (Dictionary<string, object>)Json.Deserialize(textAsset.text);
			animationEventData.LoadAnimationEventDataWithDictionary(dict);
			animationEvents.Add(animationEventsFile, animationEventData);
		}
	}

	public void AddAnimationEventsWithBlueprint(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("animation_events"))
		{
			AddAnimationEventsWithFile((string)dict["animation_events"]);
		}
	}

	public AnimationEventData FindAnimationEventData(string key)
	{
		AnimationEventData value = null;
		TFUtils.Assert(animationEvents.TryGetValue(key, out value), "AnimationEventData for " + key + " does not exist.");
		return value;
	}

	public void Clear()
	{
		particleSystemManagerDelegates.Clear();
	}

	public void RegisterParticleSystemDelegate(UpdateWithParticleSystemManagerDelegate d)
	{
		particleSystemManagerDelegates.Add(d);
	}

	public void RemoveParticleSystemDelegate(UpdateWithParticleSystemManagerDelegate d)
	{
		particleSystemManagerDelegates.Remove(d);
	}

	public void UpdateWithParticleSystemManager(ParticleSystemManager psm)
	{
		int count = particleSystemManagerDelegates.Count;
		for (int i = 0; i < count; i++)
		{
			particleSystemManagerDelegates[i](psm);
		}
	}
}
