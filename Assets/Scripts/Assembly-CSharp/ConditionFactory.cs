#define ASSERTS_ON
using System.Collections.Generic;

public static class ConditionFactory
{
	public static ICondition FromDict(Dictionary<string, object> dict)
	{
		if (dict == null)
		{
			return null;
		}
		if (!dict.ContainsKey("type"))
		{
			TFUtils.Assert(dict.ContainsKey("type"), "Quest Condition does not contain a 'type' and will be unusable. Data=" + TFUtils.DebugDictToString(dict));
		}
		if (!dict.ContainsKey("id"))
		{
			TFUtils.Assert(dict.ContainsKey("id"), "Quest Condition does not contain a 'id' and will be unusable. Data=" + TFUtils.DebugDictToString(dict));
		}
		string text = (string)dict["type"];
		ICondition result = null;
		switch (text)
		{
		case "tree":
			result = ConditionTree.FromDict(dict);
			break;
		case "query_simulated":
			result = QuerySimulatedCondition.FromDict(dict);
			break;
		case "update_resource":
			result = ModifyResourceAmountCondition.FromDict(dict);
			break;
		case "collect_rent":
			result = CollectRentCondition.FromDict(dict);
			break;
		case "rush_rent":
			result = RushRentCondition.FromDict(dict);
			break;
		case "place":
			result = PlaceCondition.FromDict(dict);
			break;
		case "complete_building":
			result = CompleteBuildingCondition.FromDict(dict);
			break;
		case "move":
			result = MoveCondition.FromDict(dict);
			break;
		case "pave":
			result = PaveCondition.FromDict(dict);
			break;
		case "feed_unit":
			result = FeedUnitCondition.FromDict(dict);
			break;
		case "collect_match_bonus":
			result = CollectMatchBonusCondition.FromDict(dict);
			break;
		case "redeem_reward":
			result = RedeemRewardsCondition.FromDict(dict);
			break;
		case "got_unlockable":
			result = GotUnlockableCondition.FromDict(dict);
			break;
		case "constant":
			result = ConstantCondition.FromDict(dict);
			break;
		case "complete_quest":
			result = CompleteQuestCondition.FromDict(dict);
			break;
		case "auto_quest_all_done":
			result = AutoQuestAllDoneCondition.FromDict(dict);
			break;
		case "start_quest":
			result = StartQuestCondition.FromDict(dict);
			break;
		case "progress_quest":
			result = ProgressQuestCondition.FromDict(dict);
			break;
		case "expand":
			result = ExpandCondition.FromDict(dict);
			break;
		case "remove_debris":
			result = RemoveDebrisCondition.FromDict(dict);
			break;
		case "start_building":
			result = StartBuildingCondition.FromDict(dict);
			break;
		case "craft_start":
			result = CraftStartCondition.FromDict(dict);
			break;
		case "craft_ready":
			result = CraftReadyCondition.FromDict(dict);
			break;
		case "craft_collect":
			result = CraftCollectCondition.FromDict(dict);
			break;
		case "auto_quest_craft_collect":
			result = AutoQuestCraftCollectCondition.FromDict(dict);
			break;
		case "purchase_production_slot":
			result = PurchaseCraftingSlotCondition.FromDict(dict);
			break;
		case "constructed":
			result = ConstructedCondition.FromDict(dict);
			break;
		case "tap_wanderer":
			result = TapWandererCondition.FromDict(dict);
			break;
		case "hide_wanderer":
			result = HideWandererCondition.FromDict(dict);
			break;
		case "button_tap":
			result = ButtonTapCondition.FromDict(dict);
			break;
		case "task_complete":
			result = TaskCompleteCondition.FromDict(dict);
			break;
		case "task_start":
			result = TaskStartCondition.FromDict(dict);
			break;
		case "change_costume":
			result = ChangeCostumeCondition.FromDict(dict);
			break;
		default:
			TFUtils.Assert(false, "This Condition uses an unknown condition type: " + text);
			break;
		}
		return result;
	}
}
