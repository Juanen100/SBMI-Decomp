using System.Collections.Generic;

public static class SessionActionSimulationHelper
{
	public static void HandleCommonSessionActions(Session session, List<SBGUIScreen> screens, SessionActionTracker action)
	{
		switch (action.Definition.Type)
		{
		case "point_at_simulated":
		{
			PointAtSimulated pointAtSimulated = (PointAtSimulated)action.Definition;
			CreateSpawn(session, action, pointAtSimulated.TargetSelected, pointAtSimulated.TargetId, pointAtSimulated.TargetDid, pointAtSimulated.SubHudSubTarget, pointAtSimulated, null);
			DeactivateQuestTracker(session);
			break;
		}
		case "point_at_simulation":
		{
			PointAtSimulation pointAtSimulation = (PointAtSimulation)action.Definition;
			pointAtSimulation.SpawnPointer(session, action);
			DeactivateQuestTracker(session);
			break;
		}
		case "point_at_expansion":
		{
			PointAtExpansion pointAtExpansion = (PointAtExpansion)action.Definition;
			pointAtExpansion.SpawnPointer(session, action);
			DeactivateQuestTracker(session);
			break;
		}
		case "screenmask_simulated":
		{
			ScreenMaskSimulation screenMaskSimulation3 = (ScreenMaskSimulation)action.Definition;
			CreateSpawn(session, action, screenMaskSimulation3.TargetSelected, screenMaskSimulation3.TargetId, screenMaskSimulation3.TargetDid, screenMaskSimulation3.SubHudSubTarget, null, screenMaskSimulation3);
			DeactivateQuestTracker(session);
			break;
		}
		case "screenmask_simulation":
		{
			ScreenMaskSimulation screenMaskSimulation = (ScreenMaskSimulation)action.Definition;
			screenMaskSimulation.SpawnSimulationMask(session.TheGame, action);
			DeactivateQuestTracker(session);
			break;
		}
		case "screenmask_expansion":
		{
			ScreenMaskSimulation screenMaskSimulation2 = (ScreenMaskSimulation)action.Definition;
			screenMaskSimulation2.SpawnExpansionMask(session.TheGame, action);
			DeactivateQuestTracker(session);
			break;
		}
		case "footprint_guide":
		{
			FootprintGuide footprintGuide = (FootprintGuide)action.Definition;
			footprintGuide.SpawnFootprint(session.TheGame, action);
			DeactivateQuestTracker(session);
			break;
		}
		case "preplace_simulated_request":
		{
			PreplaceSimulatedRequest preplaceSimulatedRequest = (PreplaceSimulatedRequest)action.Definition;
			preplaceSimulatedRequest.Preplace(session, action);
			DeactivateQuestTracker(session);
			break;
		}
		case "force_wish":
		{
			ForceResidentHunger forceResidentHunger = (ForceResidentHunger)action.Definition;
			forceResidentHunger.Handle(session, action);
			DeactivateQuestTracker(session);
			break;
		}
		case "force_bonus_reward":
		{
			ForceResidentBonusReward forceResidentBonusReward = (ForceResidentBonusReward)action.Definition;
			forceResidentBonusReward.Handle(session, action);
			DeactivateQuestTracker(session);
			break;
		}
		case "force_crafting_instance_ready":
		{
			ForceCraftingInstanceReady forceCraftingInstanceReady = (ForceCraftingInstanceReady)action.Definition;
			forceCraftingInstanceReady.Handle(session, action);
			DeactivateQuestTracker(session);
			break;
		}
		case "force_crafting_instance_slot":
		{
			ForceCraftingInstanceSlot forceCraftingInstanceSlot = (ForceCraftingInstanceSlot)action.Definition;
			forceCraftingInstanceSlot.Handle(session, action);
			DeactivateQuestTracker(session);
			break;
		}
		case "force_rent_ready":
		{
			ForceRentReady forceRentReady = (ForceRentReady)action.Definition;
			forceRentReady.Handle(session, action);
			DeactivateQuestTracker(session);
			break;
		}
		case "force_produce":
		{
			ForceProduce forceProduce = (ForceProduce)action.Definition;
			forceProduce.Handle(session, action);
			DeactivateQuestTracker(session);
			break;
		}
		case "force_treasure_spawn":
		{
			ForceTreasureSpawn forceTreasureSpawn = (ForceTreasureSpawn)action.Definition;
			forceTreasureSpawn.Handle(session, action);
			break;
		}
		case "spawn_wanderer":
		{
			SpawnWanderer spawnWanderer = (SpawnWanderer)action.Definition;
			spawnWanderer.Handle(session, action);
			break;
		}
		case "spawn_resident":
		{
			SpawnResident spawnResident = (SpawnResident)action.Definition;
			spawnResident.Handle(session, action);
			break;
		}
		case "disable_flee":
		{
			DisableFlee disableFlee = (DisableFlee)action.Definition;
			disableFlee.Handle(session, action);
			break;
		}
		case "lock_recipe":
		{
			LockRecipe lockRecipe = (LockRecipe)action.Definition;
			lockRecipe.Handle(session, action);
			break;
		}
		case "lock_input":
		{
			LockInput lockInput = (LockInput)action.Definition;
			lockInput.Handle(session, action);
			DeactivateQuestTracker(session);
			break;
		}
		case "mock_click_simulated":
		{
			MockClickSimulated mockClickSimulated = (MockClickSimulated)action.Definition;
			if (mockClickSimulated.TargetDid.HasValue || mockClickSimulated.TargetId != null)
			{
				Simulated simulated = null;
				if (mockClickSimulated.TargetId != null)
				{
					simulated = session.TheGame.simulation.FindSimulated(mockClickSimulated.TargetId);
				}
				else if (mockClickSimulated.TargetDid.HasValue)
				{
					simulated = session.TheGame.simulation.FindSimulated(mockClickSimulated.TargetDid.Value);
				}
				mockClickSimulated.HandleClick(session, action, simulated);
			}
			break;
		}
		case "mock_click_simulated_cancel":
		{
			MockClickSimulatedCancel mockClickSimulatedCancel = (MockClickSimulatedCancel)action.Definition;
			mockClickSimulatedCancel.HandleCancel(session, action);
			break;
		}
		case "start_micro_event":
		{
			StartMicroEvent startMicroEvent = (StartMicroEvent)action.Definition;
			startMicroEvent.Handle(session, action);
			break;
		}
		case "complete_micro_event":
		{
			CompleteMicroEvent completeMicroEvent = (CompleteMicroEvent)action.Definition;
			completeMicroEvent.Handle(session, action);
			break;
		}
		}
	}

	private static void CreateSpawn(Session session, SessionActionTracker action, bool targetSelected, Identity targetId, int? targetDid, string subHudSubTarget, PointAtSimulated pointAtSimul, ScreenMaskSimulation screenMaskSimul)
	{
		Simulated simulated = null;
		if (targetSelected)
		{
			if (targetId != null)
			{
				if (session.TheGame.selected.Id == targetId)
				{
					simulated = session.TheGame.selected;
				}
			}
			else if (targetDid.HasValue)
			{
				if (session.TheGame.selected.entity.DefinitionId == targetDid.Value)
				{
					simulated = session.TheGame.selected;
				}
			}
			else if (subHudSubTarget != null)
			{
				if (targetId != null)
				{
					if (session.TheGame.selected.Id == targetId)
					{
						simulated = session.TheGame.selected;
					}
				}
				else if (targetDid.HasValue && session.TheGame.selected.entity.DefinitionId == targetDid.Value)
				{
					simulated = session.TheGame.selected;
				}
				SBGUIScreen scratchScreen = session.TheGame.simulation.scratchScreen;
				SBGUIElement sBGUIElement = scratchScreen.FindChildSessionActionId(DecorateSessionActionId(0u, subHudSubTarget), false);
				if (sBGUIElement != null)
				{
					if (pointAtSimul != null)
					{
						pointAtSimul.SpawnSubHudPointer(session, action, simulated, sBGUIElement, scratchScreen);
					}
					else
					{
						screenMaskSimul.SpawnSubHudMask(session.TheGame, action, sBGUIElement, scratchScreen);
					}
				}
			}
		}
		else if (targetId != null)
		{
			simulated = session.TheGame.simulation.FindSimulated(targetId);
		}
		else if (targetDid.HasValue)
		{
			simulated = session.TheGame.simulation.FindSimulated(targetDid);
		}
		if (simulated == null)
		{
			return;
		}
		if (subHudSubTarget == null)
		{
			if (pointAtSimul != null)
			{
				pointAtSimul.SpawnSimulatedPointer(session, action, simulated, null, null);
			}
			else
			{
				screenMaskSimul.SpawnSimulatedMask(session.TheGame, action, simulated);
			}
			return;
		}
		SBGUIScreen scratchScreen2 = session.TheGame.simulation.scratchScreen;
		SBGUIElement sBGUIElement2 = scratchScreen2.FindChildSessionActionId(DecorateSessionActionId((uint)simulated.entity.DefinitionId, subHudSubTarget), false);
		if (sBGUIElement2 != null)
		{
			if (pointAtSimul != null)
			{
				pointAtSimul.SpawnSubHudPointer(session, action, simulated, sBGUIElement2, scratchScreen2);
			}
			else
			{
				screenMaskSimul.SpawnSubHudMask(session.TheGame, action, sBGUIElement2, scratchScreen2);
			}
		}
	}

	public static void EnableHandler(Session session, bool enabled)
	{
		string id = "simulation";
		if (enabled)
		{
			if (!session.TheGame.sessionActionManager.ExistsActionHandler(id))
			{
				session.TheGame.sessionActionManager.SetActionHandler(id, session, null, HandleCommonSessionActions);
			}
		}
		else if (session.TheGame.sessionActionManager.ExistsActionHandler(id))
		{
			session.TheGame.sessionActionManager.ClearActionHandler(id, session);
		}
	}

	public static string DecorateSessionActionId(uint ownerDid, string targetToken)
	{
		return "SubHud_" + targetToken;
	}

	private static void DeactivateQuestTracker(Session session)
	{
	}
}
