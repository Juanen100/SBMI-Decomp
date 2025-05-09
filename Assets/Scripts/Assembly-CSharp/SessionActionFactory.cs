#define ASSERTS_ON
using System.Collections.Generic;

public static class SessionActionFactory
{
	private static uint nextId;

	public static SessionActionDefinition Create(Dictionary<string, object> data, ICondition startingConditions)
	{
		return Create(data, startingConditions, 0u, nextId++);
	}

	public static SessionActionDefinition Create(Dictionary<string, object> data, ICondition startingConditions, uint originatedFromQuest, uint id)
	{
		string text = TFUtils.LoadString(data, "type");
		switch (text)
		{
		case "array":
			return SessionActionArray.Create(data, id, startingConditions, originatedFromQuest);
		case "sequence":
			return SessionActionSequence.Create(data, id, startingConditions, originatedFromQuest);
		case "text_prompt":
			return TextPrompt.Create(data, id, startingConditions, originatedFromQuest);
		case "activate_hud_button":
			return ActivateHudButton.Create(data, id, startingConditions, originatedFromQuest);
		case "quest_reminder":
			return QuestReminder.Create(data, id, startingConditions, originatedFromQuest);
		case "tutorial_hand_pointer":
			return TutorialHandSessionActionDefinition.Create(data, id, startingConditions, originatedFromQuest);
		case "point_at_element":
			return PointAtElement.Create(data, id, startingConditions, originatedFromQuest);
		case "point_at_simulated":
			return PointAtSimulated.Create(data, id, startingConditions, originatedFromQuest);
		case "point_at_simulation":
			return PointAtSimulation.Create(data, id, startingConditions, originatedFromQuest);
		case "point_at_expansion":
			return PointAtExpansion.Create(data, id, startingConditions, originatedFromQuest);
		case "footprint_guide":
			return FootprintGuide.Create(data, id, startingConditions, originatedFromQuest);
		case "preplace_simulated_request":
			return PreplaceSimulatedRequest.Create(data, id, startingConditions, originatedFromQuest);
		case "force_wish":
			return ForceResidentHunger.Create(data, id, startingConditions, originatedFromQuest);
		case "force_bonus_reward":
			return ForceResidentBonusReward.Create(data, id, startingConditions, originatedFromQuest);
		case "force_crafting_instance_ready":
			return ForceCraftingInstanceReady.Create(data, id, startingConditions, originatedFromQuest);
		case "force_crafting_instance_slot":
			return ForceCraftingInstanceSlot.Create(data, id, startingConditions, originatedFromQuest);
		case "force_rent_ready":
			return ForceRentReady.Create(data, id, startingConditions, originatedFromQuest);
		case "lock_input":
			return LockInput.Create(data, id, startingConditions, originatedFromQuest);
		case "mock_click_simulated":
			return MockClickSimulated.Create(data, id, startingConditions, originatedFromQuest);
		case "mock_click_simulated_cancel":
			return MockClickSimulatedCancel.Create(data, id, startingConditions, originatedFromQuest);
		case "screenmask_element":
			return ScreenMaskElement.Create(data, id, startingConditions, originatedFromQuest);
		case "screenmask_simulated":
			return ScreenMaskSimulation.Create(ScreenMaskSpawn.ScreenMaskType.SIMULATED, data, id, startingConditions, originatedFromQuest);
		case "screenmask_simulation":
			return ScreenMaskSimulation.Create(ScreenMaskSpawn.ScreenMaskType.SIMULATION, data, id, startingConditions, originatedFromQuest);
		case "screenmask_expansion":
			return ScreenMaskSimulation.Create(ScreenMaskSpawn.ScreenMaskType.EXPANSION, data, id, startingConditions, originatedFromQuest);
		case "call_playhaven":
			return FirePlayHavenPlacement.Create(data, id, startingConditions, originatedFromQuest);
		case "force_produce":
			return ForceProduce.Create(data, id, startingConditions, originatedFromQuest);
		case "achievement_unlock":
			return AchievementUnlock.Create(data, id, startingConditions, originatedFromQuest);
		case "force_treasure_spawn":
			return ForceTreasureSpawn.Create(data, id, startingConditions, originatedFromQuest);
		case "spawn_wanderer":
			return SpawnWanderer.Create(data, id, startingConditions, originatedFromQuest);
		case "lock_recipe":
			return LockRecipe.Create(data, id, startingConditions, originatedFromQuest);
		case "disable_flee":
			return DisableFlee.Create(data, id, startingConditions, originatedFromQuest);
		case "spawn_resident":
			return SpawnResident.Create(data, id, startingConditions, originatedFromQuest);
		case "start_micro_event":
			return StartMicroEvent.Create(data, id, startingConditions, originatedFromQuest);
		case "complete_micro_event":
			return CompleteMicroEvent.Create(data, id, startingConditions, originatedFromQuest);
		default:
			TFUtils.Assert(false, string.Format("Encountered unrecognized SessionActionDefinition Type('{0}') in data:{1}", text, TFUtils.DebugDictToString(data)));
			return null;
		}
	}
}
