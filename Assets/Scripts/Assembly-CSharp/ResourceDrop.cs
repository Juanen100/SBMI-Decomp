#define ASSERTS_ON
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDrop : ItemDrop
{
	private int amount;

	public override int Value
	{
		get
		{
			return amount;
		}
	}

	public ResourceDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, ItemDropDefinition definition, ulong creationTime, int amount, Action callback)
		: base(position, fixedOffset, direction, definition, creationTime, callback)
	{
		this.amount = amount;
	}

	public static string MakeResourceKey(int did)
	{
		return "ItemDropReachedHud_" + did;
	}

	protected override void OnCollectionAnimationComplete(Session session)
	{
		int numQueuedDialogInputs = session.TheGame.dialogPackageManager.GetNumQueuedDialogInputs();
		string key = MakeResourceKey(definition.Did);
		int? num = (int?)session.CheckAsyncRequest(key);
		int num2 = (num.HasValue ? num.Value : 0);
		num2 += Value;
		session.AddAsyncResponse(key, num2);
		ResourceManager resourceManager = session.TheGame.resourceManager;
		if (resourceManager.Resources[definition.Did].CollectedSound != null)
		{
			if (definition.Did == 3)
			{
				if (amount >= 8)
				{
					session.TheSoundEffectManager.PlaySound("coin_big");
				}
				else
				{
					session.TheSoundEffectManager.PlaySound("coin_single");
				}
			}
			else
			{
				session.TheSoundEffectManager.PlaySound(resourceManager.Resources[definition.Did].CollectedSound);
			}
		}
		else
		{
			session.TheSoundEffectManager.PlaySound("ItemCollected");
		}
		List<Reward> rewards = null;
		IResourceProgressCalculator resourceCalculator = session.TheGame.simulation.resourceCalculatorManager.GetResourceCalculator(definition.Did);
		if (resourceCalculator != null)
		{
			resourceCalculator.GetRewardsForIncreasingResource(session.TheGame.simulation, resourceManager.Resources, amount, out rewards);
		}
		resourceManager.Add(definition.Did, amount, session.TheGame);
		if (rewards != null)
		{
			TFUtils.Assert(definition.Did == ResourceManager.XP, "The resource calculator derivative rewards only works for level-ups as a result of gaining XP! This should not be happening!");
			int num3 = 0;
			foreach (Reward item2 in rewards)
			{
				resourceManager.Add(ResourceManager.LEVEL, 1, session.TheGame);
				if (item2 != null)
				{
					session.TheGame.ApplyReward(item2, creationTime, false);
					int level = resourceManager.Query(ResourceManager.LEVEL);
					session.TheGame.ModifyGameState(new LevelUpAction(level, item2, TFUtils.EpochTime()));
				}
				num3++;
			}
			if (rewards.Count > 0)
			{
				LevelUpDialogInputData item = new LevelUpDialogInputData(resourceManager.Query(ResourceManager.LEVEL), rewards);
				session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData> { item });
				if (numQueuedDialogInputs == 0 && onCleanupComplete != null)
				{
					onCleanupComplete();
				}
			}
		}
		CleanUpRewardDropTapParticles(session);
	}

	protected override void PlayTapAnimation(Session session)
	{
		if (!isFlying)
		{
			if (definition.Did == ResourceManager.SOFT_CURRENCY)
			{
				PlaySoftCurrencyDropTapParticles(session);
			}
			if (session.TheGame.resourceManager.Resources[definition.Did].TapSound != null)
			{
				session.TheSoundEffectManager.PlaySound(session.TheGame.resourceManager.Resources[definition.Did].TapSound);
			}
			else
			{
				session.TheSoundEffectManager.PlaySound("TapFallenItem");
			}
		}
	}

	protected override void PlayRewardAmountTextAnim(Session session)
	{
		if (session.TheState.GetType().Equals(typeof(Session.Playing)))
		{
			Session.Playing playing = (Session.Playing)session.TheState;
			Vector3 vector = session.TheCamera.WorldPointToScreenPoint(position);
			vector.x += (float)Screen.width * 0.0075f;
			vector.y -= (float)Screen.height * 0.0075f;
			playing.DisappearingResourceAmount(vector, amount);
		}
	}

	public static Vector2 GetScreenCollectionDestination(int resourceDid)
	{
		if (resourceDid == ResourceManager.SOFT_CURRENCY)
		{
			return new Vector2((float)Screen.width * 0.912f, (float)Screen.height * 0.956f);
		}
		if (resourceDid == ResourceManager.HARD_CURRENCY)
		{
			return new Vector2((float)Screen.width * 0.912f, (float)Screen.height * 0.909f);
		}
		if (resourceDid == ResourceManager.XP)
		{
			return new Vector2((float)Screen.width * 0.3955f, (float)Screen.height * 0.9427f);
		}
		return new Vector2((float)Screen.width * 0.95f, (float)Screen.height * 0.82f);
	}
}
