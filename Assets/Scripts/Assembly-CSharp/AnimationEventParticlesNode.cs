using System.Collections.Generic;
using UnityEngine;

public class AnimationEventParticlesNode : AnimationEventHandlerDelegate, AnimationEventNodeDelegate
{
	public class Data
	{
		public float time;

		public string bone;

		public string particles;

		public Vector3 offset;
	}

	public class ParticlesDelegate : ParticleSystemManager.Request.IDelegate
	{
		private GameObject gameObject;

		private Data data;

		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		public Vector3 Position
		{
			get
			{
				return default(Vector3);
			}
		}

		public bool isVisible
		{
			get
			{
				return false;
			}
		}

		public string Particles
		{
			get
			{
				return null;
			}
		}

		public float TimeKey
		{
			get
			{
				return 0f;
			}
		}

		public ParticlesDelegate(GameObject go, Data data)
		{
		}
	}

	public string nodeName;

	public Dictionary<float, Data> data;

	public List<ParticleSystemManager.Request.IDelegate> pendingRequestDelegates;

	public Dictionary<float, ParticleSystemManager.Request> activeRequests;

	public void HandleAnimationEvent(AnimationEvent animationEvent)
	{
	}

	public void SetupAnimationEvents(GameObject rootGameObject, AnimationClip clip, AnimationEventManager mgr)
	{
	}

	public void InitializeWithData(Dictionary<string, object> dict)
	{
	}

	public void UpdateWithParticleSystemManager(ParticleSystemManager psm)
	{
	}
}
