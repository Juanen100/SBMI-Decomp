#define ASSERTS_ON
using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

public class ParticleSystemManager : IComparer<ParticleSystemManager.Request>
{
	public class Request
	{
		public interface IDelegate
		{
			Transform ParentTransform { get; }

			Vector3 Position { get; }

			bool isVisible { get; }
		}

		public enum State
		{
			STATE_NONE = 0,
			STATE_WAIT = 1,
			STATE_PLAY = 2
		}

		public string effectsName;

		public IDelegate clientDelegate;

		public int initialPriority;

		public int subsequentPriority;

		public float cyclingPeriod;

		public float elapsedTime;

		public GameObject particleSystemGameObject;

		private bool firstService;

		private State state;

		public bool FirstService
		{
			get
			{
				return firstService;
			}
		}

		public State CurrentState
		{
			get
			{
				return state;
			}
		}

		public Request()
		{
			effectsName = "Prefabs/FX/Fx_Bubble_Chimney";
			initialPriority = 0;
			subsequentPriority = 0;
			cyclingPeriod = 1f;
		}

		public void Init(bool firstService, State state)
		{
			this.state = state;
			this.firstService = firstService;
			elapsedTime = 0f;
		}
	}

	private const float PLAY_CYCLING_TIME = 1f;

	private const int DEFAULT_PRIORITY = 0;

	public const string kBubbleChimneyPrefab = "Prefabs/FX/Fx_Bubble_Chimney";

	public const string kBubbleScreenWipePrefab = "Prefabs/FX/Fx_Bubble_Screen_Wipe";

	public const string kBubblePopPrefab = "Prefabs/FX/Fx_Bubble_Pop";

	public const string kThoughtBubblePopPrefab = "Prefabs/FX/Fx_Bubble_Thought_Pop";

	public const string kEatPrefab = "Prefabs/FX/Fx_Food_Crumbs";

	public const string kBubbleBuildingPopPrefab = "Prefabs/FX/Fx_Bubble_Building_Pop";

	public const string kConstructionSmokePrefab = "Prefabs/FX/Fx_Construction_Smoke";

	public const string kConstructionStarsPrefab = "Prefabs/FX/Fx_Construction_Stars";

	public const string kTreasureStarsPrefab = "Prefabs/FX/Fx_Sparkles_Rising2";

	public const string kConfettiSquareScreenWipePrefab = "Prefabs/FX/Fx_Confetti_Squares";

	public const string kConfettiSquigglesScreenWipePrefab = "Prefabs/FX/Fx_Confetti_Squiggles";

	public const string kBalloon1ScreenWipePrefab = "Prefabs/FX/Fx_Confetti_Balloons_01";

	public const string kBalloon2ScreenWipePrefab = "Prefabs/FX/Fx_Confetti_Balloons_02";

	public const string kSeaFlowersScreenWipePrefab = "Prefabs/FX/Fx_Seaflowers_Quest_Complete";

	public const string kBubble2ScreenWipePrefab = "Prefabs/FX/Fx_Bubble_Quest_Complete";

	public const string BUBBLE_CLICK_PREFAB = "Prefabs/FX/Fx_Bubble_Click";

	public const string TAP_COIN_SHOWER_PREFAB = "Prefabs/FX/Fx_Coin_Shower";

	public const string TAP_JELLY_SHOWER_PREFAB = "Prefabs/FX/Fx_Jelly_Shower";

	public const string TAP_GLASS_BREAK_PREFAB = "Prefabs/FX/Fx_Glass_Break";

	public const string TAP_FILM_ROLL_PREFAB = "Prefabs/FX/Fx_Film_Roll";

	public const string FOG1_DRIFT_PREFAB = "Prefabs/FX/Fx_Fog1_Drift";

	public const string FOG2_DRIFT_PREFAB = "Prefabs/FX/Fx_Fog2_Drift";

	public const string FOG3_DRIFT_PREFAB = "Prefabs/FX/Fx_Fog3_Drift";

	public const string FOG4_DRIFT_PREFAB = "Prefabs/FX/Fx_Fog4_Drift";

	public const string FOG5_DRIFT_PREFAB = "Prefabs/FX/Fx_Fog5_Drift";

	private string[] ParticleEffects = new string[0];

	private bool mDisableInstanceAssert;

	private List<Request> requestPool;

	private List<Request> servicingRequests;

	private Dictionary<string, List<GameObject>> particleSystemPools;

	private Dictionary<string, List<Request>> waitingRequests;

	private Action<Request> updateWaitAction;

	private Action<Request> updateServiceAction;

	public ParticleSystemManager()
	{
		string text = CommonUtils.TextureForDeviceOverride("particle_effects");
		string text2 = null;
		if (text != "particle_effects")
		{
			text2 = TFUtils.GetStreamingAssetsFile(text);
		}
		else
		{
			string text3 = string.Empty;
			if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
			{
				text3 += "_lr2";
			}
			else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
			{
				text3 += "_lr";
			}
			text2 = TFUtils.GetStreamingAssetsFile(string.Format("{0}{1}.json", "particle_effects", text3));
		}
		string json = TFUtils.ReadAllText(text2);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(json);
		int num = 0;
		num = TFUtils.LoadInt(dictionary, "MAX_REQUEST_INSTANCES", 100);
		mDisableInstanceAssert = TFUtils.LoadBool(dictionary, "DISABLE_INSTANCE_ASSERT", false);
		requestPool = new List<Request>(num);
		for (int i = 0; i < num; i++)
		{
			requestPool.Add(new Request());
		}
		particleSystemPools = new Dictionary<string, List<GameObject>>();
		waitingRequests = new Dictionary<string, List<Request>>();
		Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dictionary["EFFECTS"];
		ParticleEffects = new string[dictionary2.Count];
		int num2 = 0;
		foreach (string key in dictionary2.Keys)
		{
			Dictionary<string, object> data = (Dictionary<string, object>)dictionary2[key];
			int maxCount = TFUtils.LoadInt(data, "MAX", 1);
			string text4 = TFUtils.LoadString(data, "PATH");
			if (string.IsNullOrEmpty(text4))
			{
				text4 = key;
			}
			ParticleEffects[num2] = key;
			num2++;
			particleSystemPools.Add(key, MakeSystemPool(text4, maxCount));
			waitingRequests.Add(key, new List<Request>());
		}
		servicingRequests = new List<Request>();
		updateWaitAction = UpdateWaitingRequest;
		updateServiceAction = UpdateServicingRequest;
	}

	protected List<GameObject> MakeSystemPool(string effectsName, int maxCount)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < maxCount; i++)
		{
			GameObject gameObject = UnityGameResources.Create(effectsName);
			ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
			if (component != null)
			{
				component.enableEmission = false;
			}
			else
			{
				ParticleEmitter component2 = gameObject.GetComponent<ParticleEmitter>();
				component2.emit = false;
			}
			list.Add(gameObject);
		}
		return list;
	}

	private void ReleaseParticlesWithRequest(Request r)
	{
		if (r.particleSystemGameObject != null)
		{
			ParticleSystem component = r.particleSystemGameObject.GetComponent<ParticleSystem>();
			if (component != null)
			{
				component.enableEmission = false;
				component.Stop();
			}
			else
			{
				ParticleEmitter component2 = r.particleSystemGameObject.GetComponent<ParticleEmitter>();
				component2.emit = false;
			}
			particleSystemPools[r.effectsName].Add(r.particleSystemGameObject);
			r.particleSystemGameObject.transform.parent = null;
			r.particleSystemGameObject = null;
		}
	}

	public void RemoveRequestWithDelegate(Request.IDelegate d)
	{
		foreach (Request servicingRequest in servicingRequests)
		{
			if (servicingRequest.clientDelegate == d)
			{
				servicingRequests.Remove(servicingRequest);
				ReleaseParticlesWithRequest(servicingRequest);
				return;
			}
		}
		foreach (string key in waitingRequests.Keys)
		{
			List<Request> list = waitingRequests[key];
			foreach (Request item in list)
			{
				if (item.clientDelegate == d)
				{
					list.Remove(item);
					ReleaseParticlesWithRequest(item);
					return;
				}
			}
		}
	}

	public Request RequestParticles(string effectsName, int initialPriority, int subsequentPriority, float cyclingPeriod, Request.IDelegate requestDelegate)
	{
		TFUtils.Assert(requestDelegate != null, "RequestParticles received null requestDelegate. This will cause problems when the request is processed.\nEffectRequestName=" + effectsName);
		if (!mDisableInstanceAssert)
		{
			TFUtils.Assert(requestPool.Count > 0, "RequestParticles ran out of requests");
		}
		if (requestPool.Count <= 0 || TFPerfUtils.IsNonParticleDevice())
		{
			return null;
		}
		Request request = requestPool[0];
		request.effectsName = effectsName;
		request.initialPriority = initialPriority;
		request.subsequentPriority = subsequentPriority;
		request.cyclingPeriod = cyclingPeriod;
		request.clientDelegate = requestDelegate;
		requestPool.RemoveAt(0);
		request.Init(true, Request.State.STATE_WAIT);
		waitingRequests[request.effectsName].Add(request);
		return request;
	}

	public bool RemoveParticleSystemRequest(Request request)
	{
		bool flag = false;
		TFUtils.Assert(request != null, "RemoveParticleSystemRequest(): null request parameter");
		TFUtils.Assert(request.effectsName != null, "RemoveParticleSystemRequest(): request has null effectsName");
		if (servicingRequests.Contains(request))
		{
			flag = servicingRequests.Remove(request);
			if (flag && request.particleSystemGameObject != null)
			{
				ReleaseParticlesWithRequest(request);
			}
			requestPool.Add(request);
		}
		else if (waitingRequests[request.effectsName].Contains(request))
		{
			flag = waitingRequests[request.effectsName].Remove(request);
			requestPool.Add(request);
		}
		return flag;
	}

	public int Compare(Request a, Request b)
	{
		if (a.clientDelegate.isVisible && !b.clientDelegate.isVisible)
		{
			return -1;
		}
		if (!a.clientDelegate.isVisible && b.clientDelegate.isVisible)
		{
			return 1;
		}
		if (a.FirstService && !b.FirstService)
		{
			return -1;
		}
		if (!a.FirstService && b.FirstService)
		{
			return 1;
		}
		if (a.FirstService && b.FirstService)
		{
			if (a.initialPriority > b.initialPriority)
			{
				return -1;
			}
			if (a.initialPriority < b.initialPriority)
			{
				return 1;
			}
		}
		if (a.subsequentPriority > b.subsequentPriority)
		{
			return -1;
		}
		if (a.subsequentPriority < b.subsequentPriority)
		{
			return 1;
		}
		if (a.elapsedTime > b.elapsedTime)
		{
			return -1;
		}
		if (a.elapsedTime < b.elapsedTime)
		{
			return 1;
		}
		return 0;
	}

	private void ServiceWaitingRequests(string effectsName)
	{
		ServiceWaitingRequests(effectsName, particleSystemPools[effectsName], waitingRequests[effectsName]);
	}

	private void ServiceWaitingRequests(string effectsName, List<GameObject> particleEffectPool, List<Request> requests)
	{
		requests.Sort(this);
		int num = Math.Min(particleEffectPool.Count, requests.Count);
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			Request request = requests[i];
			TFUtils.Assert(request.clientDelegate != null, "Cannot process unassigned client deligate for request(" + request.effectsName + ")");
			if (!request.clientDelegate.isVisible)
			{
				break;
			}
			num2++;
			GameObject gameObject = (request.particleSystemGameObject = particleEffectPool[i]);
			ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
			if (component != null)
			{
				component.enableEmission = true;
				component.Play();
			}
			else
			{
				ParticleEmitter component2 = gameObject.GetComponent<ParticleEmitter>();
				component2.emit = true;
				component2.enabled = true;
			}
			if (request.clientDelegate.ParentTransform != null)
			{
				request.particleSystemGameObject.transform.parent = request.clientDelegate.ParentTransform;
				request.particleSystemGameObject.transform.localPosition = request.clientDelegate.Position;
				request.particleSystemGameObject.transform.localRotation = Quaternion.identity;
				request.particleSystemGameObject.transform.localScale = Vector3.one;
				GameObject gameObject2 = TFUtils.FindParentGameObjectInHierarchy(request.particleSystemGameObject, "BN_ROOT");
				if (gameObject2 != null && gameObject2.transform.parent != null)
				{
					GameObject gameObject3 = gameObject2.transform.parent.gameObject;
					if (gameObject3.transform.localScale.x < 0f)
					{
						if (component != null && component.startSpeed > 0f)
						{
							component.startSpeed *= -1f;
						}
					}
					else if (component.startSpeed < 0f)
					{
						component.startSpeed = Math.Abs(component.startSpeed);
					}
				}
			}
			else
			{
				request.particleSystemGameObject.transform.position = request.clientDelegate.Position;
			}
			request.Init(false, Request.State.STATE_PLAY);
			servicingRequests.Add(request);
		}
		if (num2 > 0)
		{
			waitingRequests[effectsName].RemoveRange(0, num2);
			particleSystemPools[effectsName].RemoveRange(0, num2);
		}
	}

	private void UpdateServicingRequest(Request r)
	{
		r.elapsedTime += Time.deltaTime;
		ParticleSystem component = r.particleSystemGameObject.GetComponent<ParticleSystem>();
		if (component != null && !component.loop)
		{
			if (r.elapsedTime > component.duration)
			{
				ReleaseParticlesWithRequest(r);
				servicingRequests.Remove(r);
				r.Init(false, Request.State.STATE_NONE);
				requestPool.Add(r);
			}
		}
		else if (r.elapsedTime > r.cyclingPeriod || !r.clientDelegate.isVisible)
		{
			ReleaseParticlesWithRequest(r);
			servicingRequests.Remove(r);
			r.Init(false, Request.State.STATE_WAIT);
			waitingRequests[r.effectsName].Add(r);
		}
	}

	private void UpdateWaitingRequest(Request r)
	{
		r.elapsedTime += Time.deltaTime;
	}

	public void Update(string effectsName)
	{
		List<Request> list = waitingRequests[effectsName];
		list.ForEach(updateWaitAction);
		List<GameObject> list2 = particleSystemPools[effectsName];
		if (list2.Count > 0 && list.Count > 0)
		{
			ServiceWaitingRequests(effectsName, list2, list);
		}
	}

	public void OnUpdate()
	{
		servicingRequests.ForEach(updateServiceAction);
		string[] particleEffects = ParticleEffects;
		foreach (string effectsName in particleEffects)
		{
			Update(effectsName);
		}
	}
}
