using System.Collections.Generic;

public static class Features
{
	public const string FEATURE_TREASURE = "allow_treasure";

	public const string FEATURE_DRIFTWOOD = "allow_driftwood";

	public const string FEATURE_DRIFTWOOD_TUTORIAL = "allow_driftwood_tutorial";

	public const string FEATURE_JUST_IN_TIME_JJ_PURCHASES = "jit_jj_purchases";

	public const string FEATURE_EXPAND = "purchase_expansions";

	public const string FEATURE_EXPAND_BOARDWALK = "purchase_expansions_boardwalk";

	public const string FEATURE_DEBRIS_CLEARING = "debris_clearing";

	public const string FEATURE_WISHES = "resident_wishes";

	public const string FEATURE_WISH_FULL_POOL = "resident_wishes_full_pool";

	public const string FEATURE_INVENTORY_SOFT = "inventory_soft";

	public const string FEATURE_STASHING_SOFT = "stash_soft";

	public const string FEATURE_SELLING_SOFT = "sell_soft";

	public const string FEATURE_MOVE_REJECT_LOCK = "move_reject_lock";

	public const string FEATURE_RECIPE_DROPS = "recipe_drops";

	public const string FEATURE_AUTO_FEED_LOCKOUT = "autofeed";

	public const string FEATURE_ALLOW_RANDOM_QUESTS = "allow_random_quests";

	public const string FEATURE_ALLOW_AUTO_QUESTS = "allow_auto_quests";

	public const string FEATURE_ALLOW_PRODUCTION_SLOT_PURCHASE = "allow_production_slot_purchase";

	public const string FEATURE_TUTORIAL_COMPLETE = "unrestrict_clicks";

	public static readonly HashSet<string> FeatureSet = new HashSet<string>
	{
		"jit_jj_purchases", "purchase_expansions", "debris_clearing", "resident_wishes", "resident_wishes_full_pool", "inventory_soft", "stash_soft", "sell_soft", "move_reject_lock", "recipe_drops",
		"autofeed", "allow_random_quests", "allow_auto_quests", "allow_production_slot_purchase", "allow_treasure", "allow_driftwood", "allow_driftwood_tutorial", "unrestrict_clicks", "purchase_expansions_boardwalk"
	};
}
