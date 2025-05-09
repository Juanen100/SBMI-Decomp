using System;
using System.Collections.Generic;
using UnityEngine;

public class ResidentEntity : EntityDecorator
{
	public const string HUNGRY_AT = "hungry_at";

	public const string FULLNESS_LENGTH = "fullness_length";

	public const string FULLNESS_RUSH_COST = "fullness_rush_cost";

	public const string WISH_PRODUCT_ID = "wish_product_id";

	public const string PREV_WISH_PRODUCT_ID = "prev_wish_product_id";

	public const string WISH_EXPIRES_AT = "wish_expires_at";

	public const string WISH_COOLDOWN_MIN = "wish_cooldown_min";

	public const string WISH_COOLDOWN_MAX = "wish_cooldown_max";

	public const string WISH_DURATION = "wish_duration";

	public const string LOADED_BONUS_PAYTABLES = "match_bonus_paytables";

	public const string MATCH_BONUS = "match_bonus";

	public const string HIDE_EXPIRES_AT = "hide_expires_at";

	public const string HIDE_DURATION = "hide_duration";

	public const string DISABLE_FLEE = "disable_flee";

	public const string DISABLE_IF_WILL_FLEE = "disable_if_will_flee";

	public const string JOIN_PAYTABLES = "join_paytables";

	public const string COSTUME_DID = "costume_did";

	public const string DEFAULT_COSTUME_DID = "default_costume_did";

	public const string GROSS_ITEM_ID = "gross_items_wish_table_id";

	public const string FORBIDDEN_ITEM_ID = "forbidden_items_wish_table_id";

	public const string TEMPTED_ITEM_ID = "tempted_item_id";

	private const string BONUS_PAYTABLE = "bonus_paytable";

	protected RewardDefinition forcedBonusReward;

	public Task m_pTask;

	public Vector2 m_pTaskTargetPosition = Vector2.zero;

	public bool m_bReachedTaskTarget;

	private ulong idleTimer;

	private int timeToNextIdle;

	private ulong resumeTimer;

	private int timeToNextResume;

	private bool homeAvailability;

	public override EntityType Type
	{
		get
		{
			if (Wanderer)
			{
				return EntityType.WANDERER;
			}
			return EntityType.RESIDENT;
		}
	}

	public bool Disabled
	{
		get
		{
			return (bool)Invariable["disabled"];
		}
	}

	public float TimerDuration
	{
		get
		{
			return (float)Invariable["timer_duration"];
		}
	}

	public RewardDefinition FavoriteReward
	{
		get
		{
			return (RewardDefinition)Invariable["favorite_reward"];
		}
	}

	public RewardDefinition SatisfiedReward
	{
		get
		{
			return (RewardDefinition)Invariable["satisfaction_reward"];
		}
	}

	public ulong HungryAt
	{
		get
		{
			return (ulong)Variable["hungry_at"];
		}
		set
		{
			Variable["hungry_at"] = value;
		}
	}

	public ulong FullnessLength
	{
		get
		{
			return (ulong)Variable["fullness_length"];
		}
		set
		{
			Variable["fullness_length"] = value;
		}
	}

	public Cost FullnessRushCostFull
	{
		get
		{
			return (Cost)Variable["fullness_rush_cost"];
		}
		set
		{
			Variable["fullness_rush_cost"] = value;
		}
	}

	public Identity Residence
	{
		get
		{
			if (!Variable.ContainsKey("residence"))
			{
				return Identity.Null();
			}
			return (Identity)Variable["residence"];
		}
		set
		{
			Variable["residence"] = value;
		}
	}

	public int? HungerResourceId
	{
		get
		{
			return (!Variable.ContainsKey("wish_product_id")) ? ((int?)null) : ((int?)Variable["wish_product_id"]);
		}
		set
		{
			Variable["wish_product_id"] = value;
		}
	}

	public int? PreviousResourceId
	{
		get
		{
			return (!Variable.ContainsKey("prev_wish_product_id")) ? ((int?)null) : ((int?)Variable["prev_wish_product_id"]);
		}
		set
		{
			Variable["prev_wish_product_id"] = value;
		}
	}

	public int? CostumeDID
	{
		get
		{
			return (!Variable.ContainsKey("costume_did")) ? ((int?)null) : ((int?)Variable["costume_did"]);
		}
		set
		{
			Variable["costume_did"] = value;
		}
	}

	public int? DefaultCostumeDID
	{
		get
		{
			return (int?)Invariable["default_costume_did"];
		}
	}

	public int WishTableDID
	{
		get
		{
			return (int)Invariable["wish_table_did"];
		}
	}

	public int GrossItemsWishTableDID
	{
		get
		{
			return (int)Invariable["gross_items_wish_table_id"];
		}
	}

	public int ForbiddenItemsWishTableDID
	{
		get
		{
			return (int)Invariable["forbidden_items_wish_table_id"];
		}
	}

	public int? TemptedItemDID
	{
		get
		{
			return (!Variable.ContainsKey("product_id")) ? ((int?)null) : ((int?)Variable["product_id"]);
		}
		set
		{
			Variable["product_id"] = value;
		}
	}

	public int WishCooldownMin
	{
		get
		{
			return (int)Invariable["wish_cooldown_min"];
		}
	}

	public int WishCooldownMax
	{
		get
		{
			return (int)Invariable["wish_cooldown_max"];
		}
	}

	public int WishDuration
	{
		get
		{
			return (int)Invariable["wish_duration"];
		}
	}

	public ulong? WishExpiresAt
	{
		get
		{
			return (ulong?)Variable["wish_expires_at"];
		}
		set
		{
			Variable["wish_expires_at"] = value;
		}
	}

	public ulong? HideExpiresAt
	{
		get
		{
			return (ulong?)Variable["hide_expires_at"];
		}
		set
		{
			Variable["hide_expires_at"] = value;
		}
	}

	public int HideDuration
	{
		get
		{
			return (int)Invariable["hide_duration"];
		}
	}

	public int AutoQuestIntro
	{
		get
		{
			if (Invariable.ContainsKey("auto_quest_intro"))
			{
				return (int)Invariable["auto_quest_intro"];
			}
			return -1;
		}
	}

	public int AutoQuestOutro
	{
		get
		{
			if (Invariable.ContainsKey("auto_quest_outro"))
			{
				return (int)Invariable["auto_quest_outro"];
			}
			return -1;
		}
	}

	public string DialogPortrait
	{
		get
		{
			if (Invariable.ContainsKey("character_dialog_portrait"))
			{
				return (string)Invariable["character_dialog_portrait"];
			}
			return null;
		}
	}

	public string QuestReminderIcon
	{
		get
		{
			if (Invariable.ContainsKey("quest_reminder_icon"))
			{
				return (string)Invariable["quest_reminder_icon"];
			}
			return null;
		}
	}

	public bool? DisableFlee
	{
		get
		{
			bool value = false;
			if (Variable.ContainsKey("disable_flee"))
			{
				bool? flag = (bool?)Variable["disable_flee"];
				if (flag.HasValue)
				{
					return flag.Value;
				}
				return value;
			}
			return value;
		}
		set
		{
			Variable["disable_flee"] = value;
		}
	}

	public bool? DisableIfWillFlee
	{
		get
		{
			bool value = false;
			if (Invariable.ContainsKey("disable_if_will_flee"))
			{
				bool? flag = (bool?)Invariable["disable_if_will_flee"];
				if (flag.HasValue)
				{
					return flag.Value;
				}
				return value;
			}
			return value;
		}
	}

	public bool? JoinPaytables
	{
		get
		{
			bool value = true;
			if (Invariable.ContainsKey("join_paytables"))
			{
				bool? flag = (bool?)Invariable["join_paytables"];
				if (flag.HasValue)
				{
					return flag.Value;
				}
				return value;
			}
			return value;
		}
	}

	public List<uint> BonusPaytableIds
	{
		get
		{
			return (List<uint>)Invariable["match_bonus_paytables"];
		}
	}

	public Paytable[] BonusPaytables
	{
		get
		{
			return (Paytable[])Variable["bonus_paytable"];
		}
		set
		{
			Variable["bonus_paytable"] = value;
		}
	}

	public Reward MatchBonus
	{
		get
		{
			return (Reward)Variable["match_bonus"];
		}
		set
		{
			Variable["match_bonus"] = value;
		}
	}

	public bool Wanderer
	{
		get
		{
			if (!Variable.ContainsKey("wanderer"))
			{
				return false;
			}
			return (bool)Variable["wanderer"];
		}
		set
		{
			Variable["wanderer"] = value;
		}
	}

	public RewardDefinition ForcedBonusReward
	{
		get
		{
			return forcedBonusReward;
		}
		set
		{
			forcedBonusReward = value;
		}
	}

	public bool HomeAvailability
	{
		get
		{
			if (Invariable.ContainsKey("go_home_exempt") && (bool)Invariable["go_home_exempt"])
			{
				return false;
			}
			return homeAvailability;
		}
		set
		{
			homeAvailability = value;
		}
	}

	public ResidentEntity(Entity toDecorate)
		: base(toDecorate)
	{
	}

	public Cost FullnessRushCostNow()
	{
		Cost cost = new Cost(FullnessRushCostFull);
		double num = HungryAt - TFUtils.EpochTime();
		float percentLeft = (float)(num / (double)FullnessLength);
		cost.Prorate(percentLeft);
		return cost;
	}

	public float FullnessPercentage()
	{
		double num = HungryAt - TFUtils.EpochTime();
		return Mathf.Clamp01(1f - (float)(num / (double)FullnessLength));
	}

	public static Dictionary<string, object> GetWandererGameState(Dictionary<string, object> gameState, Identity unitId)
	{
		return GetGameState(gameState, unitId, "wanderers");
	}

	public static Dictionary<string, object> GetWandererGameState(Dictionary<string, object> gameState, int did)
	{
		return GetGameState(gameState, did, "wanderers");
	}

	public static Dictionary<string, object> GetUnitGameState(Dictionary<string, object> gameState, Identity unitId)
	{
		return GetGameState(gameState, unitId, "units");
	}

	public static Dictionary<string, object> GetUnitGameState(Dictionary<string, object> gameState, int did)
	{
		return GetGameState(gameState, did, "units");
	}

	private static Dictionary<string, object> GetGameState(Dictionary<string, object> gameState, Identity unitId, string key)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])[key];
		string targetString = unitId.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		return (Dictionary<string, object>)list.Find(match);
	}

	private static Dictionary<string, object> GetGameState(Dictionary<string, object> gameState, int did, string key)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])[key];
		Predicate<object> match = (object b) => TFUtils.LoadInt((Dictionary<string, object>)b, "did") == did;
		return (Dictionary<string, object>)list.Find(match);
	}

	public static void UpdateHungerTimeInGameState(Dictionary<string, object> gameState, Identity unitId, ulong hungerReadyTime)
	{
		Dictionary<string, object> unitGameState = GetUnitGameState(gameState, unitId);
		UpdateHungerTimeInUnitState(unitGameState, hungerReadyTime);
	}

	public static void UpdateHungerTimeInUnitState(Dictionary<string, object> unitState, ulong hungerReadyTime)
	{
		if (unitState.ContainsKey("feed_ready_time"))
		{
			unitState["feed_ready_time"] = hungerReadyTime;
		}
		else
		{
			unitState.Add("feed_ready_time", hungerReadyTime);
		}
	}

	public static void SetActiveStatusInUnitState(Dictionary<string, object> unitState, bool active)
	{
		unitState["active"] = active;
	}

	public void StartCheckForIdle()
	{
		StartCheckForIdle((int)Invariable["idle.cooldown.min"], (int)Invariable["idle.cooldown.max"] + 1);
	}

	public void StartCheckForIdle(int nDurationMin, int nDurationMax)
	{
		idleTimer = TFUtils.EpochTime();
		timeToNextIdle = UnityEngine.Random.Range(nDurationMin, nDurationMax);
	}

	public void StopCheckForIdle()
	{
		idleTimer = 0uL;
		timeToNextIdle = 0;
	}

	public bool CheckForIdle()
	{
		if (idleTimer == 0L || timeToNextIdle == 0)
		{
			return false;
		}
		ulong num = TFUtils.EpochTime() - idleTimer;
		if (num >= (ulong)timeToNextIdle)
		{
			return true;
		}
		return false;
	}

	public void ClearCheckForIdle()
	{
		idleTimer = TFUtils.EpochTime() - 1;
		timeToNextIdle = -1;
	}

	public void StartCheckForResume()
	{
		StartCheckForResume((int)Invariable["idle.duration.min"], (int)Invariable["idle.duration.max"] + 1);
	}

	public void StartCheckForResume(int nDurationMin, int nDurationMax)
	{
		resumeTimer = TFUtils.EpochTime();
		timeToNextResume = UnityEngine.Random.Range(nDurationMin, nDurationMax);
	}

	public void StopCheckForResume()
	{
		resumeTimer = 0uL;
		timeToNextResume = 0;
	}

	public bool CheckForResume()
	{
		if (resumeTimer == 0L || timeToNextResume == 0)
		{
			return false;
		}
		ulong num = TFUtils.EpochTime() - resumeTimer;
		if (num >= (ulong)timeToNextResume)
		{
			return true;
		}
		return false;
	}
}
