using System;
using UnityEngine;

public abstract class ItemDrop
{
	private class RewardDropTapParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		protected ItemDrop item;

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

		public RewardDropTapParticleSystemRequestDelegate(ItemDrop item)
		{
		}
	}

	private const float CLEANUP_DELAY = 4f;

	private const float CLEANUP_SPEED = 15f;

	private const float DROP_GRAVITY = 800f;

	private const float CLEANUP_TOLERANCE = 30f;

	private const float ROTATION_TOLERANCE = 5f;

	private const int MAX_LANDINGS = 4;

	private const float POP_DELAY = 1f;

	private const float POP_ALPHA_RATE = 0.1f;

	private const float POP_SCALE_RATE = 0.1f;

	public ItemDropDefinition definition;

	protected ulong creationTime;

	protected Vector3 position;

	protected float cleanupTime;

	protected float popTime;

	protected Action onCleanupComplete;

	protected bool autoCollectLock;

	protected bool isFlying;

	private Vector3 fixedOffset;

	private Vector3 velocity;

	private bool dropToTheRight;

	private bool cleanupTimerStarted;

	private bool popTimerStarted;

	private int numLandings;

	private Identity dropID;

	private float initialSpeed;

	private float landingDampeningFactor;

	private RewardDropTapParticleSystemRequestDelegate rewardDropTapParticleSystemRequestDelegate;

	private float rotationSpeedForDrop;

	private int rotationSpeedForCollect;

	private float startingAngle;

	private bool rotatingOnDrop;

	private BasicSprite debugDisplayController;

	private bool playedRewardAmountTextAnim;

	private JumpPattern rewardBouncer;

	private bool didStartJumping;

	public virtual int Value
	{
		get
		{
			return 0;
		}
	}

	public virtual Vector3 Position
	{
		get
		{
			return default(Vector3);
		}
		set
		{
		}
	}

	public Identity DropID
	{
		get
		{
			return null;
		}
	}

	protected ItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, ItemDropDefinition definition, ulong creationTime, Action onCleanupComplete)
	{
	}

	public void Pickup()
	{
	}

	public void AutoPickup()
	{
	}

	public bool HandleTap(Session session, Ray ray)
	{
		return false;
	}

	public void PlaySoftCurrencyDropTapParticles(Session session)
	{
	}

	public void CleanUpRewardDropTapParticles(Session session)
	{
	}

	protected abstract void OnCollectionAnimationComplete(Session session);

	protected abstract void PlayRewardAmountTextAnim(Session session);

	protected abstract void PlayTapAnimation(Session session);

	public bool OnUpdate(Session session, Camera camera, bool updateCollectionTimer)
	{
		return false;
	}

	private void StartCleanupTimer(Camera camera)
	{
	}

	protected virtual bool UpdateCleanup(Session session, Camera camera, bool updateCollectionTimer)
	{
		return false;
	}

	protected void BounceReward(Camera camera, float seconds, JumpPattern bouncer)
	{
	}

	private void StartPopTimer()
	{
	}

	protected bool ExplodeInPlace(Session session, Camera camera, bool updateCollectionTimer, string particleFX, string soundName)
	{
		return false;
	}
}
