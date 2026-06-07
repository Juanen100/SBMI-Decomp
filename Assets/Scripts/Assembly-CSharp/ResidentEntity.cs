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

	protected RewardDefinition forcedBonusReward;

	private const string BONUS_PAYTABLE = "bonus_paytable";

	public Task m_pTask;

	public Vector2 m_pTaskTargetPosition;

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
			return default(EntityType);
		}
	}

	public bool Disabled
	{
		get
		{
			return false;
		}
	}

	public float TimerDuration
	{
		get
		{
			return 0f;
		}
	}

	public RewardDefinition FavoriteReward
	{
		get
		{
			return null;
		}
	}

	public RewardDefinition SatisfiedReward
	{
		get
		{
			return null;
		}
	}

	public ulong HungryAt
	{
		get
		{
			return 0uL;
		}
		set
		{
		}
	}

	public ulong FullnessLength
	{
		get
		{
			return 0uL;
		}
		set
		{
		}
	}

	public Cost FullnessRushCostFull
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public Identity Residence
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public int? HungerResourceId
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public int? PreviousResourceId
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public int? CostumeDID
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public int? DefaultCostumeDID
	{
		get
		{
			return null;
		}
	}

	public int WishTableDID
	{
		get
		{
			return 0;
		}
	}

	public int GrossItemsWishTableDID
	{
		get
		{
			return 0;
		}
	}

	public int ForbiddenItemsWishTableDID
	{
		get
		{
			return 0;
		}
	}

	public int? TemptedItemDID
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public int WishCooldownMin
	{
		get
		{
			return 0;
		}
	}

	public int WishCooldownMax
	{
		get
		{
			return 0;
		}
	}

	public int WishDuration
	{
		get
		{
			return 0;
		}
	}

	public ulong? WishExpiresAt
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public ulong? HideExpiresAt
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public int HideDuration
	{
		get
		{
			return 0;
		}
	}

	public int AutoQuestIntro
	{
		get
		{
			return 0;
		}
	}

	public int AutoQuestOutro
	{
		get
		{
			return 0;
		}
	}

	public string DialogPortrait
	{
		get
		{
			return null;
		}
	}

	public string QuestReminderIcon
	{
		get
		{
			return null;
		}
	}

	public bool? DisableFlee
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public bool? DisableIfWillFlee
	{
		get
		{
			return null;
		}
	}

	public bool? JoinPaytables
	{
		get
		{
			return null;
		}
	}

	public List<uint> BonusPaytableIds
	{
		get
		{
			return null;
		}
	}

	public Paytable[] BonusPaytables
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public Reward MatchBonus
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public bool Wanderer
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public RewardDefinition ForcedBonusReward
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public bool HomeAvailability
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public ResidentEntity(Entity toDecorate)
		: base(null)
	{
	}

	public Cost FullnessRushCostNow()
	{
		return null;
	}

	public float FullnessPercentage()
	{
		return 0f;
	}

	public static Dictionary<string, object> GetWandererGameState(Dictionary<string, object> gameState, Identity unitId)
	{
		return null;
	}

	public static Dictionary<string, object> GetWandererGameState(Dictionary<string, object> gameState, int did)
	{
		return null;
	}

	public static Dictionary<string, object> GetUnitGameState(Dictionary<string, object> gameState, Identity unitId)
	{
		return null;
	}

	public static Dictionary<string, object> GetUnitGameState(Dictionary<string, object> gameState, int did)
	{
		return null;
	}

	private static Dictionary<string, object> GetGameState(Dictionary<string, object> gameState, Identity unitId, string key)
	{
		return null;
	}

	private static Dictionary<string, object> GetGameState(Dictionary<string, object> gameState, int did, string key)
	{
		return null;
	}

	public static void UpdateHungerTimeInGameState(Dictionary<string, object> gameState, Identity unitId, ulong hungerReadyTime)
	{
	}

	public static void UpdateHungerTimeInUnitState(Dictionary<string, object> unitState, ulong hungerReadyTime)
	{
	}

	public static void SetActiveStatusInUnitState(Dictionary<string, object> unitState, bool active)
	{
	}

	public void StartCheckForIdle()
	{
	}

	public void StartCheckForIdle(int nDurationMin, int nDurationMax)
	{
	}

	public void StopCheckForIdle()
	{
	}

	public bool CheckForIdle()
	{
		return false;
	}

	public void ClearCheckForIdle()
	{
	}

	public void StartCheckForResume()
	{
	}

	public void StartCheckForResume(int nDurationMin, int nDurationMax)
	{
	}

	public void StopCheckForResume()
	{
	}

	public bool CheckForResume()
	{
		return false;
	}
}
