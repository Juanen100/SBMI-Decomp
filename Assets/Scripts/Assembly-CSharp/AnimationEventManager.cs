using System.Collections.Generic;

public class AnimationEventManager
{
	public delegate void UpdateWithParticleSystemManagerDelegate(ParticleSystemManager psm);

	private Dictionary<string, AnimationEventData> animationEvents;

	private List<UpdateWithParticleSystemManagerDelegate> particleSystemManagerDelegates;

	public void AddAnimationEventsWithFile(string animationEventsFile)
	{
	}

	public void AddAnimationEventsWithBlueprint(Dictionary<string, object> dict)
	{
	}

	public AnimationEventData FindAnimationEventData(string key)
	{
		return null;
	}

	public void Clear()
	{
	}

	public void RegisterParticleSystemDelegate(UpdateWithParticleSystemManagerDelegate d)
	{
	}

	public void RemoveParticleSystemDelegate(UpdateWithParticleSystemManagerDelegate d)
	{
	}

	public void UpdateWithParticleSystemManager(ParticleSystemManager psm)
	{
	}
}
