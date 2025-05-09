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
				return gameObject.transform;
			}
		}

		public Vector3 Position
		{
			get
			{
				return data.offset;
			}
		}

		public bool isVisible
		{
			get
			{
				if (gameObject.GetComponent<Renderer>() != null)
				{
					return gameObject.GetComponent<Renderer>().isVisible;
				}
				Animation[] componentsInChildren = gameObject.transform.root.gameObject.GetComponentsInChildren<Animation>();
				foreach (Animation animation in componentsInChildren)
				{
					if (animation.gameObject.GetComponentInChildren<Renderer>().isVisible)
					{
						return true;
					}
				}
				return false;
			}
		}

		public string Particles
		{
			get
			{
				return data.particles;
			}
		}

		public float TimeKey
		{
			get
			{
				return data.time;
			}
		}

		public ParticlesDelegate(GameObject go, Data data)
		{
			gameObject = go;
			this.data = data;
		}
	}

	public string nodeName;

	public Dictionary<float, Data> data;

	public List<ParticleSystemManager.Request.IDelegate> pendingRequestDelegates;

	public Dictionary<float, ParticleSystemManager.Request> activeRequests;

	public AnimationEventParticlesNode()
	{
		pendingRequestDelegates = new List<ParticleSystemManager.Request.IDelegate>();
		activeRequests = new Dictionary<float, ParticleSystemManager.Request>();
	}

	public void HandleAnimationEvent(AnimationEvent animationEvent)
	{
		if (activeRequests.ContainsKey(animationEvent.time))
		{
			ParticleSystemManager.Request request = activeRequests[animationEvent.time];
			if (request.CurrentState != ParticleSystemManager.Request.State.STATE_NONE)
			{
				return;
			}
			activeRequests.Remove(animationEvent.time);
		}
		Data data = this.data[animationEvent.time];
		GameObject go = (GameObject)animationEvent.objectReferenceParameter;
		ParticleSystemManager.Request.IDelegate item = new ParticlesDelegate(go, data);
		pendingRequestDelegates.Add(item);
	}

	public void SetupAnimationEvents(GameObject rootGameObject, AnimationClip clip, AnimationEventManager mgr)
	{
		foreach (float key in this.data.Keys)
		{
			float num = key;
			AnimationEvent animationEvent = new AnimationEvent();
			animationEvent.time = num;
			animationEvent.functionName = "HandleAnimationEvent";
			Data data = this.data[num];
			animationEvent.stringParameter = nodeName;
			animationEvent.objectReferenceParameter = TFUtils.FindGameObjectInHierarchy(rootGameObject, data.bone);
			clip.AddEvent(animationEvent);
		}
		mgr.RegisterParticleSystemDelegate(UpdateWithParticleSystemManager);
	}

	public void InitializeWithData(Dictionary<string, object> dict)
	{
		nodeName = (string)dict["name"];
		Dictionary<float, Data> dictionary = new Dictionary<float, Data>();
		foreach (object item in (List<object>)dict["key_frames"])
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)item;
			Data data = new Data();
			data.time = TFUtils.LoadFloat(dictionary2, "time");
			data.bone = (string)dictionary2["bone"];
			data.particles = (string)dictionary2["particles"];
			TFUtils.LoadVector3(out data.offset, (Dictionary<string, object>)dictionary2["offset"]);
			dictionary.Add(data.time, data);
		}
		this.data = dictionary;
	}

	public void UpdateWithParticleSystemManager(ParticleSystemManager psm)
	{
		foreach (ParticleSystemManager.Request.IDelegate pendingRequestDelegate in pendingRequestDelegates)
		{
			ParticlesDelegate particlesDelegate = pendingRequestDelegate as ParticlesDelegate;
			if (!activeRequests.ContainsKey(particlesDelegate.TimeKey))
			{
				ParticleSystemManager.Request value = psm.RequestParticles(particlesDelegate.Particles, 0, 0, 0f, pendingRequestDelegate);
				activeRequests.Add(particlesDelegate.TimeKey, value);
			}
		}
		pendingRequestDelegates.Clear();
	}
}
