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
				BasicSprite basicSprite = (BasicSprite)item.definition.DisplayController;
				return basicSprite.Position;
			}
		}

		public bool isVisible
		{
			get
			{
				return true;
			}
		}

		public RewardDropTapParticleSystemRequestDelegate(ItemDrop item)
		{
			this.item = item;
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

	protected bool autoCollectLock = true;

	protected bool isFlying;

	private Vector3 fixedOffset;

	private Vector3 velocity;

	private bool dropToTheRight;

	private bool cleanupTimerStarted;

	private bool popTimerStarted;

	private int numLandings;

	private Identity dropID;

	private float initialSpeed = UnityEngine.Random.Range(50f, 70f);

	private float landingDampeningFactor = UnityEngine.Random.Range(0.4f, 0.65f);

	private RewardDropTapParticleSystemRequestDelegate rewardDropTapParticleSystemRequestDelegate;

	private float rotationSpeedForDrop = UnityEngine.Random.Range(300f, 600f);

	private int rotationSpeedForCollect = UnityEngine.Random.Range(5, 20);

	private float startingAngle;

	private bool rotatingOnDrop = true;

	private BasicSprite debugDisplayController;

	private bool playedRewardAmountTextAnim;

	private JumpPattern rewardBouncer;

	private bool didStartJumping;

	public virtual int Value
	{
		get
		{
			return 1;
		}
	}

	public virtual Vector3 Position
	{
		get
		{
			return position;
		}
		set
		{
			position = value;
		}
	}

	public Identity DropID
	{
		get
		{
			return dropID;
		}
	}

	protected ItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, ItemDropDefinition definition, ulong creationTime, Action onCleanupComplete)
	{
		this.definition = definition;
		this.position = position;
		this.fixedOffset = fixedOffset;
		velocity = direction * initialSpeed;
		isFlying = false;
		velocity.z += 150f;
		this.creationTime = creationTime;
		this.onCleanupComplete = onCleanupComplete;
		dropID = new Identity();
		BasicSprite basicSprite = (BasicSprite)this.definition.DisplayController;
		Vector3 axis;
		basicSprite.Rotation.ToAngleAxis(out startingAngle, out axis);
		if (direction.x < direction.y)
		{
			dropToTheRight = true;
		}
		autoCollectLock = definition.ForceTapToCollect;
		float time = Time.time;
		if (autoCollectLock)
		{
			rewardBouncer = new JumpPattern(UnityEngine.Random.Range(-150f, -200f), UnityEngine.Random.Range(15f, 20f), 0.25f, UnityEngine.Random.Range(0.2f, 0.3f), 0f, time, Vector2.one);
		}
		else
		{
			rewardBouncer = new JumpPattern(UnityEngine.Random.Range(-125f, -150f), UnityEngine.Random.Range(2f, 7f), 0.25f, UnityEngine.Random.Range(0.2f, 0.3f), 0f, time, Vector2.one);
		}
		if (SBSettings.DebugDisplayControllers)
		{
			float width = this.definition.DisplayController.HitObject.Width;
			float height = this.definition.DisplayController.HitObject.Height;
			Vector2 center = new Vector2(-0.5f * width, -0.5f * height);
			BasicSprite basicSprite2 = new BasicSprite("Materials/unique/footprint", null, center, width, height, new QuadHitObject(center, width, height));
			basicSprite2.PublicInitialize();
			basicSprite2.Name = "RewardDropDebug_" + dropID;
			basicSprite2.Visible = true;
			basicSprite2.Color = Color.blue;
			basicSprite2.Alpha = 0.2f;
			basicSprite2.Resize(this.definition.DisplayController.HitObject.Center, this.definition.DisplayController.HitObject.Width, this.definition.DisplayController.HitObject.Height);
			debugDisplayController = basicSprite2;
		}
	}

	public void Pickup()
	{
		if (!TFPerfUtils.IsNonScalingDevice())
		{
			BasicSprite basicSprite = (BasicSprite)definition.DisplayController;
			Vector3 scale = basicSprite.Scale;
			scale.x = 1f;
			scale.y = 1f;
			basicSprite.Scale = scale;
		}
		cleanupTimerStarted = true;
		cleanupTime = Time.time - 1f;
		rewardDropTapParticleSystemRequestDelegate = new RewardDropTapParticleSystemRequestDelegate(this);
		autoCollectLock = false;
	}

	public void AutoPickup()
	{
		if (!autoCollectLock)
		{
			Pickup();
		}
	}

	public bool HandleTap(Session session, Ray ray)
	{
		if (cleanupTimerStarted && definition.DisplayController.Intersects(ray))
		{
			Pickup();
			PlayTapAnimation(session);
			return true;
		}
		return false;
	}

	public void PlaySoftCurrencyDropTapParticles(Session session)
	{
		session.TheGame.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Coin_Shower", 0, 0, 0f, rewardDropTapParticleSystemRequestDelegate);
	}

	public void CleanUpRewardDropTapParticles(Session session)
	{
		session.TheGame.simulation.particleSystemManager.RemoveRequestWithDelegate(rewardDropTapParticleSystemRequestDelegate);
	}

	protected abstract void OnCollectionAnimationComplete(Session session);

	protected abstract void PlayRewardAmountTextAnim(Session session);

	protected abstract void PlayTapAnimation(Session session);

	public bool OnUpdate(Session session, Camera camera, bool updateCollectionTimer)
	{
		BasicSprite basicSprite = (BasicSprite)definition.DisplayController;
		bool flag2;
		if (cleanupTimerStarted)
		{
			bool flag = UpdateCleanup(session, camera, updateCollectionTimer);
			flag2 = !flag;
			if (flag)
			{
				definition.DisplayController.Visible = false;
				RewardManager.ReleaseDisplayController(session.TheGame.simulation, definition.DisplayController);
				if (debugDisplayController != null)
				{
					debugDisplayController.Destroy();
				}
			}
		}
		else
		{
			flag2 = true;
			if (numLandings < 4)
			{
				float angle;
				Vector3 axis;
				basicSprite.Rotation.ToAngleAxis(out angle, out axis);
				float num = startingAngle - angle;
				if (rotatingOnDrop)
				{
					if (dropToTheRight)
					{
						basicSprite.Rotate(new Vector3(0f, 0f, rotationSpeedForDrop * Time.deltaTime));
					}
					else
					{
						basicSprite.Rotate(new Vector3(0f, 0f, (0f - rotationSpeedForDrop) * Time.deltaTime));
					}
				}
				if (num < 5f && numLandings >= 2 && rotatingOnDrop)
				{
					basicSprite.ResetRotation();
					rotatingOnDrop = false;
				}
				velocity.z -= 800f * Time.deltaTime;
				Vector3 vector = position;
				vector += velocity * Time.deltaTime;
				if (vector.z < 0f)
				{
					vector.z = 0f;
				}
				if (vector.z == 0f)
				{
					rotationSpeedForDrop -= rotationSpeedForDrop * 0.1f * Time.deltaTime;
					numLandings++;
					velocity.z *= 0f - landingDampeningFactor;
					if (definition.Did == 5)
					{
						session.TheSoundEffectManager.PlaySound("happiness_star_bounce");
					}
					else if (definition.Did == 1 || definition.Did == 2 || definition.Did == 3)
					{
						session.TheSoundEffectManager.PlaySound("ItemDropBounce");
					}
					else
					{
						session.TheSoundEffectManager.PlaySound("special_item_bounce");
					}
				}
				position = vector;
				definition.DisplayController.Position = position + fixedOffset;
			}
			else
			{
				basicSprite.ResetRotation();
				StartCleanupTimer(camera);
				if (debugDisplayController != null)
				{
					debugDisplayController.OnUpdate(camera, null);
					debugDisplayController.Billboard(SBCamera.BillboardDefinition);
					debugDisplayController.Position = definition.DisplayController.Position;
				}
			}
		}
		if (!flag2)
		{
			OnCollectionAnimationComplete(session);
			session.TheGame.dropManager.ExecutePickupTrigger(session.TheGame, dropID);
		}
		return flag2;
	}

	private void StartCleanupTimer(Camera camera)
	{
		if (!cleanupTimerStarted)
		{
			cleanupTimerStarted = true;
			Vector3 vector = camera.WorldToScreenPoint(position);
			Vector3 vector2 = new Vector3(definition.CleanupScreenDestination.x, definition.CleanupScreenDestination.y, 0f) - vector;
			velocity = new Vector3(vector2.x, vector2.y, 0f);
			velocity.Normalize();
			velocity *= 15f;
			float time = Time.time;
			cleanupTime = time + 4f;
		}
	}

	protected virtual bool UpdateCleanup(Session session, Camera camera, bool updateCollectionTimer)
	{
		float time = Time.time;
		if (updateCollectionTimer)
		{
			if (time >= cleanupTime && !autoCollectLock)
			{
				Vector3 vector = camera.WorldToScreenPoint(position);
				Vector3 vector2 = new Vector3(definition.CleanupScreenDestination.x, definition.CleanupScreenDestination.y, 0f) - vector;
				velocity = new Vector3(vector2.x, vector2.y, 0f);
				velocity.Normalize();
				velocity *= 15f;
				Vector3 vector3 = vector;
				vector3 += velocity;
				position = camera.ScreenToWorldPoint(vector3);
				definition.DisplayController.Position = position + fixedOffset;
				BasicSprite basicSprite = (BasicSprite)definition.DisplayController;
				isFlying = true;
				if (velocity.x < velocity.y)
				{
					basicSprite.Rotate(new Vector3(0f, 0f, -rotationSpeedForCollect));
				}
				else
				{
					basicSprite.Rotate(new Vector3(0f, 0f, rotationSpeedForCollect));
				}
				bool flag = Mathf.Abs(vector3.x - definition.CleanupScreenDestination.x) <= 30f && Mathf.Abs(vector3.y - definition.CleanupScreenDestination.y) <= 30f;
				bool flag2 = vector3.x < 0f || vector3.y < 0f || vector3.x > camera.pixelWidth || vector3.y > camera.pixelHeight;
				if (!playedRewardAmountTextAnim)
				{
					PlayRewardAmountTextAnim(session);
				}
				playedRewardAmountTextAnim = true;
				return flag || flag2;
			}
			BounceReward(camera, Time.time, rewardBouncer);
		}
		return false;
	}

	protected void BounceReward(Camera camera, float seconds, JumpPattern bouncer)
	{
		float num = 0f;
		float num2 = 0f;
		float val;
		Vector2 squish;
		bouncer.ValueAndSquishAtTime(seconds, out val, out squish);
		BasicSprite basicSprite = (BasicSprite)definition.DisplayController;
		if (!TFPerfUtils.IsNonScalingDevice())
		{
			basicSprite.Scale = TFUtils.ExpandVector(squish);
		}
		if (squish.y < 1f)
		{
			didStartJumping = true;
		}
		if (didStartJumping)
		{
			num = (squish.y - 1f) * basicSprite.Height;
			num2 = val + num;
		}
		basicSprite.Position = position + fixedOffset + basicSprite.Up * num2;
		isFlying = false;
		basicSprite.OnUpdate(camera, null);
	}

	private void StartPopTimer()
	{
		if (!popTimerStarted)
		{
			popTimerStarted = true;
			float time = Time.time;
			popTime = time + 1f;
		}
	}

	protected bool ExplodeInPlace(Session session, Camera camera, bool updateCollectionTimer, string particleFX, string soundName)
	{
		float time = Time.time;
		if (updateCollectionTimer)
		{
			if (time >= cleanupTime && !autoCollectLock)
			{
				BasicSprite basicSprite = (BasicSprite)definition.DisplayController;
				if (!TFPerfUtils.IsNonScalingDevice())
				{
					Vector3 scale = basicSprite.Scale;
					scale.x += 0.1f;
					scale.y += 0.1f;
					basicSprite.Scale = scale;
				}
				basicSprite.Alpha -= 0.1f;
				if (basicSprite.Alpha <= 0f && !popTimerStarted)
				{
					basicSprite.Alpha = 0f;
					StartPopTimer();
					session.TheSoundEffectManager.PlaySound(soundName);
					session.TheGame.simulation.particleSystemManager.RequestParticles(particleFX, 0, 0, 0f, rewardDropTapParticleSystemRequestDelegate);
					RestrictInteraction.AddWhitelistSimulated(session.TheGame.simulation, int.MinValue);
					RestrictInteraction.AddWhitelistExpansion(session.TheGame.simulation, int.MinValue);
					RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
				}
				else if (popTimerStarted && time >= popTime)
				{
					RestrictInteraction.RemoveWhitelistSimulated(session.TheGame.simulation, int.MinValue);
					RestrictInteraction.RemoveWhitelistExpansion(session.TheGame.simulation, int.MinValue);
					RestrictInteraction.RemoveWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
					return true;
				}
			}
			else
			{
				BounceReward(camera, Time.time, rewardBouncer);
			}
		}
		return false;
	}
}
