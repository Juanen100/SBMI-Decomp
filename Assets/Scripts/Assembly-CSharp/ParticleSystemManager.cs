using System;
using System.Collections.Generic;
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
				return false;
			}
		}

		public State CurrentState
		{
			get
			{
				return default(State);
			}
		}

		public void Init(bool firstService, State state)
		{
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

	private string[] ParticleEffects;

	private bool mDisableInstanceAssert;

	private List<Request> requestPool;

	private List<Request> servicingRequests;

	private Dictionary<string, List<GameObject>> particleSystemPools;

	private Dictionary<string, List<Request>> waitingRequests;

	private Action<Request> updateWaitAction;

	private Action<Request> updateServiceAction;

	protected List<GameObject> MakeSystemPool(string effectsName, int maxCount)
	{
		return null;
	}

	private void ReleaseParticlesWithRequest(Request r)
	{
	}

	public void RemoveRequestWithDelegate(Request.IDelegate d)
	{
	}

	public Request RequestParticles(string effectsName, int initialPriority, int subsequentPriority, float cyclingPeriod, Request.IDelegate requestDelegate)
	{
		return null;
	}

	public bool RemoveParticleSystemRequest(Request request)
	{
		return false;
	}

	public int Compare(Request a, Request b)
	{
		return 0;
	}

	private void ServiceWaitingRequests(string effectsName)
	{
	}

	private void ServiceWaitingRequests(string effectsName, List<GameObject> particleEffectPool, List<Request> requests)
	{
	}

	private void UpdateServicingRequest(Request r)
	{
	}

	private void UpdateWaitingRequest(Request r)
	{
	}

	public void Update(string effectsName)
	{
	}

	public void OnUpdate()
	{
	}
}
