#define ASSERTS_ON
using System;
using System.Collections.Generic;
using UnityEngine;

public class Simulated
{
	public class Annex
	{
		public class ActiveState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				Task pTask = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out pTask);
				if (pTask != null && pTask.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					pTask.m_sTargetPrevDisplayState = "idle";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("idle");
				}
				simulated.DisplayThoughtState(null, simulation);
				simulated.InteractionState.SetInteractions(true, true, false, true);
				UpdateControls(simulation, simulated);
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				bool isEnabled = simulation.catalog.CanSell(simulated.entity.DefinitionId);
				string text = simulation.catalog.SellError(simulated.entity.DefinitionId);
				bool flag = entity.Stashable;
				string text2 = ((!flag) ? "!!CANNOT_STOW_PRODUCTION" : null);
				if (simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, false).Count > 0)
				{
					flag = (isEnabled = false);
					text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_BUILDING");
					text = ((text != null) ? text : "!!CANNOT_SELL_TASK_BUILDING");
				}
				else if (entity.ResidentDids != null && text == null)
				{
					int count = entity.ResidentDids.Count;
					for (int i = 0; i < count; i++)
					{
						if (simulation.game.taskManager.GetActiveTasksForSimulated(entity.ResidentDids[i], null).Count > 0)
						{
							flag = (isEnabled = false);
							text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_RESIDENT");
							text = ((text != null) ? text : "!!CANNOT_SELL_TASK_RESIDENT");
							break;
						}
					}
				}
				list.Add(new Session.StashControl(simulated, flag, text2));
				list.Add(new Session.SellControl(simulated, isEnabled, text));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable));
				if (simulation.featureManager.CheckFeature("move_reject_lock"))
				{
					list.Add(new Session.RejectControl());
				}
				simulated.InteractionState.PushControls(list);
			}
		}

		public class RelayingState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.InteractionState.Clear();
				AnnexEntity entity = simulated.GetEntity<AnnexEntity>();
				Identity identity = entity.HubId;
				if (identity == null && entity.HubDid.HasValue)
				{
					Simulated simulated2 = simulation.FindSimulated((int)entity.HubDid.Value);
					if (simulated2 != null)
					{
						identity = simulated2.Id;
					}
				}
				if (identity == null)
				{
					TFUtils.DebugLog("Trying to relay to a hub that cannot be found!");
				}
				else
				{
					simulation.Router.Send(DelegateClickCommand.Create(simulated.Id, identity));
				}
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class ShuntedCraftingState : Building.CraftingState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				simulated.SimulatedQueryable = true;
				AnnexEntity entity = simulated.GetEntity<AnnexEntity>();
				if (entity != null)
				{
					simulation.Router.Send(HUBCraftCommand.Create(simulated.Id, entity.HubId, true));
				}
				simulated.InteractionState.SetInteractions(true, true, false, true);
				UpdateControls(simulation, simulated);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				AnnexEntity entity = simulated.GetEntity<AnnexEntity>();
				if (entity != null)
				{
					simulation.Router.Send(HUBCraftCommand.Create(simulated.Id, entity.HubId, false));
				}
				base.Leave(simulation, simulated);
			}

			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				bool isEnabled = simulation.catalog.CanSell(simulated.entity.DefinitionId);
				string text = simulation.catalog.SellError(simulated.entity.DefinitionId);
				bool flag = entity.Stashable;
				string text2 = ((!flag) ? "!!CANNOT_STOW_PRODUCTION" : null);
				if (simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, false).Count > 0)
				{
					flag = (isEnabled = false);
					text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_BUILDING");
					text = ((text != null) ? text : "!!CANNOT_SELL_TASK_BUILDING");
				}
				else if (entity.ResidentDids != null && text == null)
				{
					int count = entity.ResidentDids.Count;
					for (int i = 0; i < count; i++)
					{
						if (simulation.game.taskManager.GetActiveTasksForSimulated(entity.ResidentDids[i], null).Count > 0)
						{
							flag = (isEnabled = false);
							text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_RESIDENT");
							text = ((text != null) ? text : "!!CANNOT_SELL_TASK_RESIDENT");
							break;
						}
					}
				}
				list.Add(new Session.StashControl(simulated, flag, text2));
				list.Add(new Session.SellControl(simulated, isEnabled, text));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable));
				if (simulation.featureManager.CheckFeature("move_reject_lock"))
				{
					list.Add(new Session.RejectControl());
				}
				simulated.InteractionState.PushControls(list);
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return base.Simulate(simulation, simulated, session);
			}
		}

		public class ShuntedCraftCyclingState : Building.CraftCyclingState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				simulated.SimulatedQueryable = true;
				AnnexEntity entity = simulated.GetEntity<AnnexEntity>();
				if (entity != null)
				{
					simulation.Router.Send(HUBCraftCommand.Create(simulated.Id, entity.HubId, true));
				}
				simulated.InteractionState.SetInteractions(true, true, false, true);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				AnnexEntity entity = simulated.GetEntity<AnnexEntity>();
				if (entity != null)
				{
					simulation.Router.Send(HUBCraftCommand.Create(simulated.Id, entity.HubId, false));
				}
				base.Leave(simulation, simulated);
			}
		}

		public static ActiveState Active = new ActiveState();

		public static RelayingState Relaying = new RelayingState();

		public static ShuntedCraftingState ShuntedCrafting = new ShuntedCraftingState();

		public static ShuntedCraftCyclingState ShuntedCraftCycling = new ShuntedCraftCyclingState();

		public static Simulated Extend(Simulated simulated, Simulation simulation)
		{
			TFUtils.DebugLog("Extending as annex(name=" + (string)simulated.entity.Invariable["name"] + ")");
			simulated.entity = new AnnexEntity(simulated.entity);
			SanityCheck(simulated, simulation);
			return simulated;
		}

		private static void SanityCheck(Simulated simulated, Simulation simulation)
		{
			AnnexEntity entity = simulated.GetEntity<AnnexEntity>();
			Identity hubId = entity.HubId;
			TFUtils.Assert(entity.HubDid.HasValue || simulation.FindSimulated(hubId) != null, string.Format("Could not find hub entity for this annex! Ensure it exists in starting state. \nExpectedHub={0}\nThisAnnex={1}", hubId, simulated));
		}
	}

	public class Building
	{
		public class PlacingAction : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = false;
				Setup(simulated, simulation);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				ErectableDecorator entity2 = simulated.GetEntity<ErectableDecorator>();
				if (entity2.ErectionTime != 0)
				{
					Identity identity = simulated.command["employee"] as Identity;
					simulated.Variable["employee"] = identity;
					simulation.Router.Send(MoveCommand.Create(simulated.Id, identity, simulated.PointOfInterest, simulated.Flip));
				}
				entity2.ErectionCompleteTime = TFUtils.EpochTime() + entity2.ErectionTime;
				entity.Slots = simulation.game.craftManager.GetInitialSlots(entity.DefinitionId);
				RecordPlacement(simulation, simulated);
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulated simulated, Simulation simulation)
			{
				simulated.DisplayState("default");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
				simulated.InteractionState.Clear();
				BaseTransitionBinding transition = null;
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				if (simulation.featureManager.CheckFeature("move_reject_lock"))
				{
					list.Add(new Session.StashControl(simulated, false, "!!CANNOT_STOW_PLACEMENT"));
					list.Add(new Session.SellControl(simulated, false, "!!CANNOT_SELL_PLACEMENT"));
				}
				else
				{
					list.Add(new Session.StashControl(simulated, false, string.Empty));
					list.Add(new Session.SellControl(simulated, false, string.Empty));
				}
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.SetInteractions(true, true, true, false, transition, list);
			}

			private void RecordPlacement(Simulation simulation, Simulated simulated)
			{
				Cost cost = simulation.catalog.GetCost(simulated.entity.DefinitionId);
				simulation.ModifyGameStateSimulated(simulated, new NewBuildingAction(simulated, cost));
			}
		}

		public class PrimeErectingState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				ErectableDecorator erectable = simulated.GetEntity<ErectableDecorator>();
				if (erectable.ErectionTime != 0)
				{
					simulated.timebarMixinArgs.hasTimebar = true;
					simulated.timebarMixinArgs.description = Language.Get((string)simulated.Entity.Invariable["name"]);
					simulated.timebarMixinArgs.completeTime = erectable.ErectionCompleteTime.Value;
					simulated.timebarMixinArgs.totalTime = erectable.ErectionTime;
					simulated.timebarMixinArgs.duration = erectable.ErectionTimerDuration;
					simulated.timebarMixinArgs.rushCost = erectable.BuildRushCost;
					simulated.m_pNamebarMixinArgs.m_bHasNamebar = false;
					ulong started = erectable.ErectionCompleteTime.Value - erectable.ErectionTime;
					Action<Session> execute = delegate(Session session)
					{
						Rush(session, simulated);
						int value = 0;
						Cost.Prorate(erectable.BuildRushCost, started, erectable.ErectionCompleteTime.Value, TFUtils.EpochTime()).ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out value);
						string sItemName = "Accelerate_" + erectable.Name;
						AnalyticsWrapper.LogJellyConfirmation(session.TheGame, erectable.DefinitionId, value, sItemName, "buildings", "speedup", "construction", "confirm");
					};
					Action<Session> cancel = delegate
					{
						int value = 0;
						Cost.Prorate(erectable.BuildRushCost, started, erectable.ErectionCompleteTime.Value, TFUtils.EpochTime()).ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out value);
					};
					simulated.rushParameters = new RushParameters(execute, cancel, (ulong time) => Cost.Prorate(erectable.BuildRushCost, started, erectable.ErectionCompleteTime.Value, time), Language.Get("!!RUSH_BUILD") + " " + simulated.Entity.BlueprintName, simulated.Entity.DefinitionId, delegate(Session session, Cost cost, bool canAfford)
					{
						LogRush(session, simulated, cost, canAfford);
					}, simulation.ScreenPositionFromWorldPosition(simulated.DisplayController.Position));
				}
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				session.TheGame.selected = simulated;
				ErectableDecorator entity = simulated.GetEntity<ErectableDecorator>();
				ulong? erectionCompleteTime = entity.ErectionCompleteTime;
				if (erectionCompleteTime.HasValue && erectionCompleteTime.Value > TFUtils.EpochTime())
				{
					new Session.TimebarGroup().ActivateOnSelected(session);
				}
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				session.TheGame.selected = null;
				return false;
			}

			private void Rush(Session session, Simulated simulated)
			{
				session.TheGame.simulation.Router.Send(RushCommand.Create(simulated.Id));
			}

			private void LogRush(Session session, Simulated simulated, Cost cost, bool canAfford)
			{
				session.analytics.LogRushBuild(simulated.entity.BlueprintName, cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], canAfford);
			}
		}

		public class PrimeErectingStateFriend : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			private void Rush(Session session, Simulated simulated)
			{
			}

			private void LogRush(Session session, Simulated simulated, Cost cost, bool canAfford)
			{
			}
		}

		public class ErectingState : StateActionBuildingDefault, Animated
		{
			public const string CLICK_DURATION_HANDLER = "clickDurationHandler";

			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.AddScaffolding(simulation);
				simulated.AddFence(simulation);
				ErectableDecorator entity = simulated.GetEntity<ErectableDecorator>();
				simulated.command = null;
				simulated.SimulatedQueryable = false;
				BaseTransitionBinding transition = null;
				BuildingEntity entity2 = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				list.Add(new Session.StashControl(simulated, false, "!!CANNOT_STOW_CONSTRUCTING"));
				list.Add(new Session.SellControl(simulated, false, "!!CANNOT_SELL_CONSTRUCTING"));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity2.Flippable, simulation));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.SetInteractions(true, true, true, false, transition, list);
				simulated.useFootprintIntersection = true;
				if (!entity.ErectionCompleteTime.HasValue)
				{
					entity.ErectionCompleteTime = TFUtils.EpochTime() + entity.ErectionTime;
				}
				ulong num = entity.ErectionCompleteTime.Value - TFUtils.EpochTime();
				if (num != 0)
				{
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), num);
					entity.RaisingTimeRemaining = num;
				}
				else
				{
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				}
				simulated.EnableAnimateAction(true);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.useFootprintIntersection = false;
				simulated.ClearSimulateOnce();
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.m_pNamebarMixinArgs.m_bHasNamebar = false;
				simulated.Visible = true;
				if (simulated.displayController != null && simulated.displayController.Transform.GetComponent<MeshRenderer>() != null && !simulated.displayController.Transform.GetComponent<Renderer>().material.HasProperty("_AlphaTex"))
				{
					simulated.Color = new Color(1f, 1f, 1f, 1f);
				}
				simulated.RemoveScaffolding(simulation);
				simulated.RemoveFence(simulation);
				simulated.GetEntity<ErectableDecorator>().RaisingTimeRemaining = 0.0;
				if (simulated.DisplayController is BasicSprite)
				{
					BasicSprite basicSprite = (BasicSprite)simulated.DisplayController;
					basicSprite.SetMaskPercentage(0f);
				}
				simulation.particleSystemManager.RemoveRequestWithDelegate(simulated.dustParticleSystemRequestDelegate);
				simulated.dustParticleSystemRequestDelegate.isAssigned = false;
				simulation.particleSystemManager.RemoveRequestWithDelegate(simulated.starsParticleSystemRequestDelegate);
				simulated.starsParticleSystemRequestDelegate.isAssigned = false;
				object value = null;
				if (simulated.Variable.TryGetValue("employee", out value))
				{
					Identity identity = value as Identity;
					if (identity != null)
					{
						simulation.Router.Send(ReturnCommand.Create(simulated.Id, identity));
					}
				}
				simulated.EnableAnimateAction(false);
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				simulated.SimulateOnce();
				return false;
			}

			public Vector3 Animate(Simulation simulation, Simulated simulated)
			{
				float num = 0f;
				ErectableDecorator entity = simulated.GetEntity<ErectableDecorator>();
				if (entity.ErectionTime == 0)
				{
					return Vector3.zero;
				}
				float input = (float)(entity.RaisingTimeRemaining / (double)entity.ErectionTime);
				input = TFMath.ClampF(input, 0f, 1f);
				entity.RaisingTimeRemaining -= Time.deltaTime;
				if (input > 0.5f)
				{
					if (simulated.scaffolding != null)
					{
						simulated.scaffolding.SetHeight(simulation.enclosureManager, (1f - input) / 0.5f * simulated.Height * 0.33f, SBCamera.BillboardDefinition);
					}
					num = 0f - simulated.Height;
					simulated.DisplayController.SetMaskPercentage(1f);
				}
				else
				{
					if (!simulated.dustParticleSystemRequestDelegate.isAssigned)
					{
						simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Construction_Smoke", 0, 0, 1f, simulated.dustParticleSystemRequestDelegate);
						simulated.dustParticleSystemRequestDelegate.isAssigned = true;
					}
					if (!simulated.starsParticleSystemRequestDelegate.isAssigned)
					{
						simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Construction_Stars", 0, 0, 1f, simulated.starsParticleSystemRequestDelegate);
						simulated.starsParticleSystemRequestDelegate.isAssigned = true;
					}
					float num2 = input / 0.5f;
					num = (0f - num2) * simulated.Height;
					float num3 = ((!(simulated.Height > 10f)) ? 0.15f : (20f / simulated.Height));
					simulated.DisplayController.SetMaskPercentage(num2 + num3);
				}
				return simulated.DisplayController.Up * num;
			}
		}

		public class InactiveState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				ErectableDecorator entity = simulated.GetEntity<ErectableDecorator>();
				if (entity.ErectionTime == 0L)
				{
					simulation.Router.Send(ClickedCommand.Create(simulated.Id, simulated.Id));
				}
				simulated.command = null;
				simulated.SimulatedQueryable = false;
				BaseTransitionBinding transition = null;
				BuildingEntity entity2 = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.StashControl(simulated, false, "!!CANNOT_STOW_CONSTRUCTING"));
				list.Add(new Session.SellControl(simulated, false, "!!CANNOT_SELL_CONSTRUCTING"));
				list.Add(new Session.RotateControl(simulated, entity2.Flippable));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.SetInteractions(true, true, true, true, transition, list);
				simulated.DisplayState("default");
				simulated.DisplayThoughtState(simulated.GetEntity<ErectableDecorator>().CompletionReward.Summary.ThoughtIcon, "default", simulation);
				simulation.game.triggerRouter.RouteTrigger(SimulatedTrigger.CreateTrigger(simulated, "contruction_complete"), simulation.game);
				if (entity.ErectionTime != 0L)
				{
					simulated.DisplayController.SetMaskPercentage(0f);
				}
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity.Invariable.ContainsKey("completion_sound"))
				{
					simulation.soundEffectManager.PlaySound((string)entity.Invariable["completion_sound"]);
				}
				else
				{
					simulation.soundEffectManager.PlaySound("UnveilBuilding");
				}
				simulated.DisplayState("default");
			}

			public new bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class ActiveState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				if (simulated.command != null && simulated.command.Type == Command.TYPE.HUBCRAFT)
				{
					BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
					if (entity == null)
					{
						TFUtils.ErrorLog("Building(" + simulated.Id.Describe() + "):Active Enter called with null building entity");
					}
					if ((bool)simulated.command["start"])
					{
						if (entity.BusyAnnexCount == 0)
						{
							simulated.AddSimulateOnce("enable_particles", delegate
							{
								simulated.EnableParticles(simulation, true);
							});
						}
						entity.BusyAnnexCount++;
					}
					else
					{
						if (entity.BusyAnnexCount == 1)
						{
							simulated.ClearSimulateOnce();
							simulated.EnableParticles(simulation, false);
						}
						if (entity.BusyAnnexCount > 0)
						{
							entity.BusyAnnexCount--;
						}
					}
				}
				List<int> activeSourcesWithMatchBonusForTarget = simulation.game.taskManager.GetActiveSourcesWithMatchBonusForTarget(simulation, simulated.Id);
				if (activeSourcesWithMatchBonusForTarget.Count > 0)
				{
					simulation.Router.Send(BonusRewardCommand.Create(simulated.Id, simulated.Id, activeSourcesWithMatchBonusForTarget[0]));
					simulated.command = null;
					return;
				}
				TryProduce(simulation, simulated.GetEntity<BuildingEntity>());
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				Setup(simulated, simulation);
				UpdateControls(simulation, simulated);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity == null)
				{
					TFUtils.ErrorLog("Building(" + simulated.Id.Describe() + "):Active Leave called with null building entity");
				}
				if (entity.BusyAnnexCount == 0)
				{
					simulated.ClearSimulateOnce();
					simulated.EnableParticles(simulation, false);
				}
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity == null)
				{
					TFUtils.ErrorLog("Building(" + simulated.Id.Describe() + "):Simulate called with null building entity");
				}
				if (entity.BusyAnnexCount > 0)
				{
					simulated.SimulateOnce();
				}
				return false;
			}

			public static void Setup(Simulated simulated, Simulation simulation)
			{
				simulated.InteractionState.SetInteractions(true, true, false, false);
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity.CanCraft)
				{
					simulated.InteractionState.SelectedTransition = new Session.BrowseRecipesTransition(simulated);
				}
				else if (entity.CanVend)
				{
					simulated.InteractionState.SelectedTransition = new Session.VendingTransition(simulated);
				}
				else
				{
					simulated.InteractionState.IsSelectable = true;
					simulated.m_pNamebarMixinArgs.m_bHasNamebar = true;
					simulated.m_pNamebarMixinArgs.m_sName = Language.Get(entity.Name);
					simulated.m_pNamebarMixinArgs.m_bCheckForTaskCharacters = true;
				}
				Task pTask = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out pTask);
				if (pTask != null && pTask.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					pTask.m_sTargetPrevDisplayState = "default";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("default");
				}
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}

			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				bool isEnabled = simulation.catalog.CanSell(simulated.entity.DefinitionId);
				string text = simulation.catalog.SellError(simulated.entity.DefinitionId);
				bool flag = entity.Stashable;
				string text2 = ((!flag) ? "!!CANNOT_STOW_PRODUCTION" : null);
				if (simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, false).Count > 0)
				{
					flag = (isEnabled = false);
					text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_BUILDING");
					text = ((text != null) ? text : "!!CANNOT_SELL_TASK_BUILDING");
				}
				else if (entity.ResidentDids != null)
				{
					int count = entity.ResidentDids.Count;
					for (int i = 0; i < count; i++)
					{
						if (simulation.game.taskManager.GetActiveTasksForSimulated(entity.ResidentDids[i], null).Count > 0)
						{
							flag = (isEnabled = false);
							text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_RESIDENT");
							text = ((text != null) ? text : "!!CANNOT_SELL_TASK_RESIDENT");
							break;
						}
					}
				}
				list.Add(new Session.StashControl(simulated, flag, text2));
				list.Add(new Session.SellControl(simulated, isEnabled, text));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.PushControls(list);
			}

			public void TryProduce(Simulation simulation, BuildingEntity building)
			{
				if (building.HasDecorator<PeriodicProductionDecorator>())
				{
					simulation.Router.Send(ProduceCommand.Create(building.Id, building.Id));
				}
			}
		}

		public class RequestingInterfaceState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				session.AddAsyncResponse("RequestEntityInterface", simulated, false);
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				return false;
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class ReflectingState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				List<int> activeSourcesWithMatchBonusForTarget = simulation.game.taskManager.GetActiveSourcesWithMatchBonusForTarget(simulation, simulated.Id);
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				Command command = null;
				command = ((activeSourcesWithMatchBonusForTarget.Count > 0) ? BonusRewardCommand.Create(simulated.Id, simulated.Id, activeSourcesWithMatchBonusForTarget[0]) : (entity.HasDecorator<PeriodicProductionDecorator>() ? ProduceCommand.Create(simulated.Id, simulated.Id) : ((entity.CraftRewards != null) ? AdvanceCommand.Create(simulated.Id, simulated.Id) : ((!simulation.game.craftManager.Crafting(entity.Id)) ? ActivateCommand.Create(simulated.Id, simulated.Id) : CraftCommand.Create(simulated.Id, simulated.Id)))));
				TFUtils.Assert(command != null, "This building couldn't figure out what to do!");
				simulation.Router.Send(command);
				return false;
			}

			public new void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class ReplacingState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				Setup(simulated, simulation);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			private void Setup(Simulated simulated, Simulation simulation)
			{
				simulated.DisplayState("default");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
				simulated.InteractionState.Clear();
				UpdateControls(simulation, simulated);
			}

			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
				BaseTransitionBinding transition = null;
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				bool isEnabled = simulation.catalog.CanSell(simulated.entity.DefinitionId);
				string text = simulation.catalog.SellError(simulated.entity.DefinitionId);
				if (simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, false).Count > 0)
				{
					text = ((text != null) ? text : "!!CANNOT_SELL_TASK_BUILDING");
				}
				else if (entity.ResidentDids != null)
				{
					int count = entity.ResidentDids.Count;
					for (int i = 0; i < count; i++)
					{
						if (simulation.game.taskManager.GetActiveTasksForSimulated(entity.ResidentDids[i], null).Count > 0)
						{
							text = ((text != null) ? text : "!!CANNOT_SELL_TASK_RESIDENT");
							break;
						}
					}
				}
				list.Add(new Session.SellControl(simulated, isEnabled, text));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.SetInteractions(true, true, true, false, transition, list);
			}
		}

		public abstract class ActivatingBase : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				List<Simulated> residents = GetResidents(simulation, simulated);
				UpdateBuildingState(simulated);
				RecordActions(simulation, simulated, residents);
				simulated.command = null;
				simulated.InteractionState.Clear();
				simulated.DisplayState("default");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
			}

			protected abstract List<Simulated> GetResidents(Simulation simulation, Simulated building);

			protected abstract void UpdateBuildingState(Simulated simulated);

			protected abstract void RecordActions(Simulation simulation, Simulated simulated, List<Simulated> residents);

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class ActivatingState : ActivatingBase
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
			}

			protected override List<Simulated> GetResidents(Simulation simulation, Simulated building)
			{
				List<Simulated> list = new List<Simulated>();
				List<int> residentDids = building.GetEntity<BuildingEntity>().ResidentDids;
				foreach (int item in residentDids)
				{
					Simulated simulated = null;
					if (item != -1)
					{
						simulated = TryAddResident(simulation, building, item);
					}
					if (simulated != null)
					{
						list.Add(simulated);
					}
				}
				return list;
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Building_Pop", 0, 0, 0f, simulated.activateParticleSystemRequestDelegate);
			}

			protected override void UpdateBuildingState(Simulated simulated)
			{
				ulong num = TFUtils.EpochTime();
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				ActivatableDecorator decorator = entity.GetDecorator<ActivatableDecorator>();
				decorator.Activated = num;
				if (entity.HasDecorator<PeriodicProductionDecorator>())
				{
					PeriodicProductionDecorator decorator2 = entity.GetDecorator<PeriodicProductionDecorator>();
					decorator2.ProductReadyTime = num + decorator2.RentProductionTime;
				}
			}

			protected override void RecordActions(Simulation simulation, Simulated simulated, List<Simulated> residents)
			{
				Reward reward = null;
				if (simulated.GetEntity<ErectableDecorator>().CompletionReward != null)
				{
					reward = simulated.GetEntity<ErectableDecorator>().CompletionReward.GenerateReward(simulation, false);
				}
				ulong utcNow = TFUtils.EpochTime();
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, simulated, utcNow);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Building.ActivatingState.RecordActions - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = ((count <= 0) ? null : rewardDropResults.dropIdentities[count - 1]);
				CompleteBuildingAction completeBuildingAction = new CompleteBuildingAction(simulated, residents, reward);
				completeBuildingAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, completeBuildingAction);
				completeBuildingAction.AddPickup(simulation);
				simulated.entity.PatchReferences(simulation.game);
				simulation.soundEffectManager.PlaySound("PopResourceBubble");
				simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Pop", 0, 0, 0f, simulated.rewardParticleSystemRequestDelegate);
			}

			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				ErectableDecorator entity = simulated.GetEntity<ErectableDecorator>();
				return entity.CompletionReward.GenerateReward(simulation, false);
			}
		}

		public class ReactivatingState : ActivatingBase
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity.Invariable.ContainsKey("completion_sound"))
				{
					simulation.soundEffectManager.PlaySound((string)entity.Invariable["completion_sound"]);
				}
				else
				{
					simulation.soundEffectManager.PlaySound("UnveilBuilding");
				}
				simulated.SimulatedQueryable = true;
				base.Enter(simulation, simulated);
			}

			protected override List<Simulated> GetResidents(Simulation simulation, Simulated building)
			{
				List<KeyValuePair<int, Identity>> list = (List<KeyValuePair<int, Identity>>)building.Variable["associated_entities"];
				if (list == null || list.Count <= 0)
				{
					List<int> residentDids = building.GetEntity<BuildingEntity>().ResidentDids;
					int count = residentDids.Count;
					if (count <= 0)
					{
						return null;
					}
					List<Simulated> list2 = new List<Simulated>();
					for (int i = 0; i < count; i++)
					{
						Simulated simulated = TryAddResident(simulation, building, residentDids[i]);
						if (simulated != null)
						{
							list2.Add(simulated);
						}
					}
					return list2;
				}
				List<Simulated> list3 = new List<Simulated>();
				foreach (KeyValuePair<int, Identity> item in list)
				{
					Simulated simulated2 = TryAddResident(simulation, building, item.Key, item.Value);
					if (simulated2 != null)
					{
						ResidentEntity entity = simulated2.GetEntity<ResidentEntity>();
						ulong num = entity.HungryAt;
						if (num < 0)
						{
							num = 0uL;
						}
						num += TFUtils.EpochTime();
						entity.HungryAt = num;
						list3.Add(simulated2);
					}
				}
				return list3;
			}

			protected override void UpdateBuildingState(Simulated simulated)
			{
				ulong num = TFUtils.EpochTime();
				if (simulated.HasEntity<PeriodicProductionDecorator>())
				{
					PeriodicProductionDecorator entity = simulated.GetEntity<PeriodicProductionDecorator>();
					entity.ProductReadyTime = num + entity.RentProductionTime;
				}
			}

			protected override void RecordActions(Simulation simulation, Simulated simulated, List<Simulated> residents)
			{
				simulation.ModifyGameStateSimulated(simulated, new MoveAction(simulated, residents));
			}
		}

		public class ProducingState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				List<int> activeSourcesWithMatchBonusForTarget = simulation.game.taskManager.GetActiveSourcesWithMatchBonusForTarget(simulation, simulated.Id);
				if (activeSourcesWithMatchBonusForTarget.Count > 0)
				{
					simulation.Router.Send(BonusRewardCommand.Create(simulated.Id, simulated.Id, activeSourcesWithMatchBonusForTarget[0]));
					return;
				}
				PeriodicProductionDecorator productProducer = simulated.GetEntity<PeriodicProductionDecorator>();
				ulong num = TFUtils.EpochTime();
				ulong num2 = ((num < productProducer.ProductReadyTime) ? (productProducer.ProductReadyTime - num) : 0);
				if (SBSettings.ConsoleLoggingEnabled)
				{
					TFUtils.DebugLog("Building(" + simulated.Id.Describe() + "):Producing. Ready in " + num2 + " at " + productProducer.ProductReadyTime, TFUtils.LogFilter.Buildings);
				}
				simulated.SimulatedQueryable = true;
				Task pTask = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out pTask);
				if (pTask != null && pTask.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					pTask.m_sTargetPrevDisplayState = "producing";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("producing");
				}
				simulated.DisplayThoughtState(null, simulation);
				BuildingEntity building = simulated.GetEntity<BuildingEntity>();
				UpdateControls(simulation, simulated);
				string text = Language.Get("!!TASKBAR_PRODUCING_RENT");
				if (productProducer.Product.Summary != null && !productProducer.Product.Summary.ResourceAmounts.ContainsKey(ResourceManager.SOFT_CURRENCY))
				{
					foreach (int key in productProducer.Product.Summary.ResourceAmounts.Keys)
					{
						if (key != ResourceManager.XP)
						{
							text = string.Format(Language.Get("!!TASKBAR_PRODUCING"), Language.Get(simulation.resourceManager.Resources[key].Name));
							break;
						}
					}
				}
				if (productProducer.RentRushable)
				{
					simulated.timebarMixinArgs.hasTimebar = true;
					simulated.timebarMixinArgs.description = Language.Get((string)simulated.Entity.Invariable["name"]) + "|" + text;
					simulated.timebarMixinArgs.completeTime = productProducer.ProductReadyTime;
					simulated.timebarMixinArgs.totalTime = productProducer.RentProductionTime;
					simulated.timebarMixinArgs.duration = productProducer.RentTimerDuration;
					simulated.timebarMixinArgs.rushCost = productProducer.RentRushCost;
					simulated.timebarMixinArgs.m_bCheckForTaskCharacters = true;
					simulated.m_pNamebarMixinArgs.m_bHasNamebar = false;
				}
				else
				{
					simulated.m_pNamebarMixinArgs.m_bHasNamebar = true;
					simulated.m_pNamebarMixinArgs.m_sName = Language.Get(building.Name);
					simulated.m_pNamebarMixinArgs.m_bCheckForTaskCharacters = true;
					simulated.timebarMixinArgs.hasTimebar = false;
				}
				ulong started = productProducer.ProductReadyTime - productProducer.RentProductionTime;
				Action<Session> execute = delegate(Session session)
				{
					Rush(session, simulated);
					int value = 0;
					Cost.Prorate(productProducer.RentRushCost, started, productProducer.ProductReadyTime, TFUtils.EpochTime()).ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out value);
					AnalyticsWrapper.LogJellyConfirmation(session.TheGame, building.DefinitionId, value, "Accelerate_rent_collected_" + building.Name, "buildings", "speedup", "rent", "confirm");
				};
				Action<Session> cancel = delegate
				{
					int value = 0;
					Cost.Prorate(productProducer.RentRushCost, started, productProducer.ProductReadyTime, TFUtils.EpochTime()).ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out value);
				};
				simulated.rushParameters = new RushParameters(execute, cancel, (ulong time) => Cost.Prorate(productProducer.RentRushCost, started, productProducer.ProductReadyTime, time), Language.Get("!!RUSH_RENT") + " " + productProducer.BlueprintName, productProducer.DefinitionId, delegate(Session session, Cost cost, bool canAfford)
				{
					LogRush(session, simulated, cost, canAfford);
				}, simulation.ScreenPositionFromWorldPosition(simulated.DisplayController.Position));
				simulated.AddSimulateOnce("enable_particles", delegate
				{
					simulated.EnableParticles(simulation, true);
				});
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), num2);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.ClearSimulateOnce();
				simulated.EnableParticles(simulation, false);
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.m_pNamebarMixinArgs.m_bHasNamebar = false;
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				simulated.SimulateOnce();
				return false;
			}

			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
				BaseTransitionBinding transition = null;
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				bool isEnabled = simulation.catalog.CanSell(simulated.entity.DefinitionId);
				string text = simulation.catalog.SellError(simulated.entity.DefinitionId);
				bool flag = entity.Stashable;
				string text2 = ((!flag) ? "!!CANNOT_STOW_PRODUCTION" : null);
				if (simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, false).Count > 0)
				{
					flag = (isEnabled = false);
					text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_BUILDING");
					text = ((text != null) ? text : "!!CANNOT_SELL_TASK_BUILDING");
				}
				else if (entity.ResidentDids != null)
				{
					int count = entity.ResidentDids.Count;
					for (int i = 0; i < count; i++)
					{
						if (simulation.game.taskManager.GetActiveTasksForSimulated(entity.ResidentDids[i], null).Count > 0)
						{
							flag = (isEnabled = false);
							text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_RESIDENT");
							text = ((text != null) ? text : "!!CANNOT_SELL_TASK_RESIDENT");
							break;
						}
					}
				}
				list.Add(new Session.StashControl(simulated, flag, text2));
				list.Add(new Session.SellControl(simulated, isEnabled, text));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.SetInteractions(true, true, true, false, transition, list);
			}

			private void Rush(Session session, Simulated simulated)
			{
				session.TheGame.simulation.Router.Send(RushCommand.Create(simulated.Id));
			}

			private void LogRush(Session session, Simulated simulated, Cost cost, bool canAfford)
			{
				session.analytics.LogRushRent(simulated.entity.BlueprintName, cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], canAfford);
			}
		}

		public class ProducedState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				Setup(simulation, simulated);
				UpdateControls(simulation, simulated);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				PeriodicProductionDecorator entity = simulated.GetEntity<PeriodicProductionDecorator>();
				ulong num = TFUtils.EpochTime();
				entity.ProductReadyTime = entity.RentProductionTime + num;
				SpawnDrops(simulation, simulated);
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulation simulation, Simulated simulated)
			{
				Task pTask = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out pTask);
				if (pTask != null && pTask.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					pTask.m_sTargetPrevDisplayState = "produced";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("produced");
				}
				simulated.DisplayThoughtState("produced", simulation);
				simulated.simFlags |= SimulatedFlags.FIRST_ANIMATE;
			}

			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				bool isEnabled = simulation.catalog.CanSell(simulated.entity.DefinitionId);
				string text = simulation.catalog.SellError(simulated.entity.DefinitionId);
				bool flag = entity.Stashable;
				string text2 = ((!flag) ? "!!CANNOT_STOW_PRODUCTION" : null);
				if (simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, false).Count > 0)
				{
					flag = (isEnabled = false);
					text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_BUILDING");
					text = ((text != null) ? text : "!!CANNOT_SELL_TASK_BUILDING");
				}
				else if (entity.ResidentDids != null)
				{
					int count = entity.ResidentDids.Count;
					for (int i = 0; i < count; i++)
					{
						if (simulation.game.taskManager.GetActiveTasksForSimulated(entity.ResidentDids[i], null).Count > 0)
						{
							flag = (isEnabled = false);
							text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_RESIDENT");
							text = ((text != null) ? text : "!!CANNOT_SELL_TASK_RESIDENT");
							break;
						}
					}
				}
				list.Add(new Session.StashControl(simulated, flag, text2));
				list.Add(new Session.SellControl(simulated, isEnabled, text));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.SetInteractions(true, true, false, true, null, list);
			}

			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
				Reward reward = GetReward(simulation, simulated);
				ulong utcNow = TFUtils.EpochTime();
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, simulated, utcNow);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Building.ProducedState.SpawnDrops - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = ((count <= 0) ? null : rewardDropResults.dropIdentities[count - 1]);
				CollectRentAction collectRentAction;
				if (SBSettings.UseActionFile)
				{
					collectRentAction = new CollectRentAction(simulated, reward);
				}
				else
				{
					PeriodicProductionDecorator entity = simulated.GetEntity<PeriodicProductionDecorator>();
					collectRentAction = new CollectRentAction(simulated, reward, entity.ProductReadyTime);
				}
				AnalyticsWrapper.LogRentCollected(simulation.game, simulated, reward);
				collectRentAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, collectRentAction);
				collectRentAction.AddPickup(simulation);
				simulation.analytics.LogCollectRentReward(simulated.entity.DefinitionId, simulation.resourceManager.PlayerLevelAmount);
				simulation.soundEffectManager.PlaySound("PopResourceBubble");
				simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Pop", 0, 0, 0f, simulated.rewardParticleSystemRequestDelegate);
			}

			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				PeriodicProductionDecorator entity = simulated.GetEntity<PeriodicProductionDecorator>();
				return entity.Product.GenerateReward(simulation, true, false);
			}
		}

		public class CraftingState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				List<int> activeSourcesWithMatchBonusForTarget = simulation.game.taskManager.GetActiveSourcesWithMatchBonusForTarget(simulation, simulated.Id);
				if (activeSourcesWithMatchBonusForTarget.Count > 0)
				{
					simulation.Router.Send(BonusRewardCommand.Create(simulated.Id, simulated.Id, activeSourcesWithMatchBonusForTarget[0]));
					return;
				}
				simulated.InteractionState.SetInteractions(true, true, true, false);
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity.CanCraft)
				{
					simulated.InteractionState.SelectedTransition = new Session.BrowseRecipesTransition(simulated);
				}
				Task pTask = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out pTask);
				if (pTask != null && pTask.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					pTask.m_sTargetPrevDisplayState = "crafting";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("crafting");
				}
				simulated.DisplayThoughtState(null, simulation);
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.m_pNamebarMixinArgs.m_bHasNamebar = false;
				simulated.AddSimulateOnce("enable_particles", delegate
				{
					simulated.EnableParticles(simulation, true);
				});
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.ClearSimulateOnce();
				simulated.EnableParticles(simulation, false);
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				simulated.SimulateOnce();
				return false;
			}
		}

		public class CraftedState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				Setup(simulation, simulated);
				UpdateControls(simulation, simulated);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				if (simulation.craftManager.Crafting(simulated.Id))
				{
					return;
				}
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				Reward reward = GetReward(simulation, simulated);
				ulong utcNow = TFUtils.EpochTime();
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, simulated, utcNow);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Building.CraftedState.Leave - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = ((count <= 0) ? null : rewardDropResults.dropIdentities[count - 1]);
				CraftCollectAction craftCollectAction = new CraftCollectAction(simulated.Id, reward);
				craftCollectAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, craftCollectAction);
				craftCollectAction.AddPickup(simulation);
				simulation.analytics.LogCollectCraftedGood(simulated.entity.DefinitionId, simulation.resourceManager.PlayerLevelAmount);
				Dictionary<int, int> resourceAmounts = reward.ResourceAmounts;
				foreach (KeyValuePair<int, int> item in resourceAmounts)
				{
					if (item.Value > 0 && simulation.craftManager.ContainsRecipe(item.Key))
					{
						CraftingRecipe recipeById = simulation.craftManager.GetRecipeById(item.Key);
						if (recipeById != null)
						{
							AnalyticsWrapper.LogCraftCollected(simulation.game, entity, item.Key, item.Value, recipeById.recipeName);
						}
					}
				}
				simulation.soundEffectManager.PlaySound("PopResourceBubble");
				simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Pop", 0, 0, 0f, simulated.rewardParticleSystemRequestDelegate);
				entity.ClearCraftingRewards();
			}

			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				bool isEnabled = simulation.catalog.CanSell(simulated.entity.DefinitionId);
				string text = simulation.catalog.SellError(simulated.entity.DefinitionId);
				bool flag = entity.Stashable;
				string text2 = ((!flag) ? "!!CANNOT_STOW_PRODUCTION" : null);
				if (simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, false).Count > 0)
				{
					flag = (isEnabled = false);
					text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_BUILDING");
					text = ((text != null) ? text : "!!CANNOT_SELL_TASK_BUILDING");
				}
				else if (entity.ResidentDids != null)
				{
					int count = entity.ResidentDids.Count;
					for (int i = 0; i < count; i++)
					{
						if (simulation.game.taskManager.GetActiveTasksForSimulated(entity.ResidentDids[i], null).Count > 0)
						{
							flag = (isEnabled = false);
							text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_RESIDENT");
							text = ((text != null) ? text : "!!CANNOT_SELL_TASK_RESIDENT");
							break;
						}
					}
				}
				list.Add(new Session.StashControl(simulated, flag, text2));
				list.Add(new Session.SellControl(simulated, isEnabled, text));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.SetInteractions(true, true, false, true, null, list);
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulation simulation, Simulated simulated)
			{
				simulated.InteractionState.SetInteractions(true, false, false, true);
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				string text;
				if (entity.OverrideRewardTexture != null)
				{
					text = entity.OverrideRewardTexture;
				}
				else if (entity.Slots > 1)
				{
					TFUtils.ErrorLog("For buildings with multiple production slots, specify an override reward texture!");
					text = simulation.resourceManager.Resources[ResourceManager.XP].GetResourceTexture();
				}
				else if (entity.CraftRewards == null)
				{
					TFUtils.ErrorLog("Buildings that reach the crafted state should have CraftRewards set. How did you get here?");
					text = simulation.resourceManager.Resources[ResourceManager.XP].GetResourceTexture();
				}
				else
				{
					text = entity.CraftRewards.ThoughtIcon;
				}
				Task pTask = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out pTask);
				if (pTask != null && pTask.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					pTask.m_sTargetPrevDisplayState = "produced";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("produced");
				}
				if (text != null)
				{
					simulated.DisplayThoughtState(text, "produced", simulation);
				}
				else
				{
					simulated.DisplayThoughtState("produced", simulation);
				}
			}

			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
			}

			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				return entity.CraftRewards;
			}
		}

		public class CraftCyclingState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (simulated.command != null && simulated.command.Type == Command.TYPE.CRAFTED)
				{
					TFUtils.Assert(simulated.command.HasProperty("slot_id"), "Missing SlotID property from Crafted Command");
					if (simulated.command.HasProperty("slot_id"))
					{
						int num = (int)simulated.command["slot_id"];
						CraftingInstance craftingInstance = simulation.craftManager.GetCraftingInstance(simulated.Id, num);
						if (craftingInstance != null)
						{
							simulation.craftManager.RemoveCraftingInstance(simulated.Id, num);
							entity.CraftingComplete(craftingInstance.reward);
							CraftingRecipe recipeById = simulation.craftManager.GetRecipeById(craftingInstance.recipeId);
							simulation.soundEffectManager.PlaySound(recipeById.readySoundImmediate);
							simulation.soundEffectManager.PlaySound(recipeById.readySoundBeat, recipeById.beatLength);
							if (craftingInstance.recipeId == 1000)
							{
								simulation.soundEffectManager.PlaySound("lettuce_ready");
							}
							else if (craftingInstance.recipeId == 1003)
							{
								simulation.soundEffectManager.PlaySound("tomato_ready");
							}
							CraftCompleteAction action = new CraftCompleteAction(simulated.Id, num, craftingInstance.reward);
							simulation.ModifyGameStateSimulated(simulated, action);
						}
					}
				}
				simulated.command = null;
				simulated.InteractionState.SetInteractions(true, true, true, true);
				Task pTask = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out pTask);
				if (pTask != null && pTask.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					pTask.m_sTargetPrevDisplayState = "crafting";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("crafting");
				}
				if (entity.CraftRewards != null)
				{
					if (entity.CraftRewards.ThoughtIcon != null)
					{
						simulated.DisplayThoughtState(entity.CraftRewards.ThoughtIcon, "produced", simulation);
					}
					else
					{
						simulated.DisplayThoughtState("produced", simulation);
					}
				}
				if (!simulation.craftManager.Crafting(simulated.Id))
				{
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				}
			}
		}

		public class CraftingCollectState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.InteractionState.SetInteractions(false, false, false, false);
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				SpawnDrops(simulation, simulated);
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				entity.ClearCraftingRewards();
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
				Reward reward = GetReward(simulation, simulated);
				ulong utcNow = TFUtils.EpochTime();
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, simulated, utcNow);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Building.CraftingCollectState.SpawnDrops - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = ((count <= 0) ? null : rewardDropResults.dropIdentities[count - 1]);
				CraftCollectAction craftCollectAction = new CraftCollectAction(simulated.Id, reward);
				craftCollectAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, craftCollectAction);
				craftCollectAction.AddPickup(simulation);
				simulation.soundEffectManager.PlaySound("PopResourceBubble");
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity != null)
				{
					Dictionary<int, int> resourceAmounts = reward.ResourceAmounts;
					foreach (KeyValuePair<int, int> item in resourceAmounts)
					{
						if (item.Value > 0 && simulation.craftManager.ContainsRecipe(item.Key))
						{
							CraftingRecipe recipeById = simulation.craftManager.GetRecipeById(item.Key);
							if (recipeById != null)
							{
								AnalyticsWrapper.LogCraftCollected(simulation.game, entity, item.Key, item.Value, recipeById.recipeName);
							}
						}
					}
				}
				simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Pop", 0, 0, 0f, simulated.rewardParticleSystemRequestDelegate);
			}

			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				return entity.CraftRewards;
			}
		}

		public class RushingBuildState : RushingSomething
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				ErectableDecorator entity = simulated.GetEntity<ErectableDecorator>();
				simulated.CalculateRushCompletionPercent(entity.ErectionCompleteTime.Value, entity.ErectionTime);
				entity.ErectionCompleteTime = TFUtils.EpochTime();
				base.Enter(simulation, simulated);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				Identity identity = ((!simulated.Variable.ContainsKey("employee")) ? null : (simulated.Variable["employee"] as Identity));
				if (identity != null)
				{
					simulation.FindSimulated(identity).ClearPendingCommands();
					simulation.Router.Send(ReturnCommand.Create(simulated.Id, identity));
				}
				simulation.game.notificationManager.CancelNotification("build:" + simulated.Id.Describe());
				Cost cost = new Cost();
				cost += simulated.GetEntity<ErectableDecorator>().BuildRushCost;
				cost.Prorate((float)simulated.Variable[RUSH_PERCENT]);
				simulation.ModifyGameStateSimulated(simulated, new RushBuildAction(simulated.Id, cost, simulated.GetEntity<ErectableDecorator>().ErectionCompleteTime.Value));
			}

			protected override Cost GetRushCost(Simulation simulation, Simulated simulated)
			{
				return simulated.GetEntity<ErectableDecorator>().BuildRushCost;
			}
		}

		public class RushingProductState : RushingSomething
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				PeriodicProductionDecorator entity = simulated.GetEntity<PeriodicProductionDecorator>();
				simulated.CalculateRushCompletionPercent(entity.ProductReadyTime, entity.RentProductionTime);
				entity.ProductReadyTime = TFUtils.EpochTime();
				base.Enter(simulation, simulated);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				Cost cost = new Cost();
				PeriodicProductionDecorator entity = simulated.GetEntity<PeriodicProductionDecorator>();
				cost += entity.RentRushCost;
				cost.Prorate((float)simulated.Variable[RUSH_PERCENT]);
				simulation.ModifyGameStateSimulated(simulated, new RushRentAction(simulated.Id, cost, entity.ProductReadyTime));
			}

			protected override Cost GetRushCost(Simulation simulation, Simulated simulated)
			{
				return simulated.GetEntity<PeriodicProductionDecorator>().RentRushCost;
			}
		}

		public class RushingCraftState : RushingSomething
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				if (simulated.command == null)
				{
					TFUtils.ErrorLog("RushingCraftState:Enter - command is null");
					return;
				}
				TFUtils.Assert(simulated.command.HasProperty("slot_id"), string.Concat("Trying to rush building ", simulated.Id, " without supplying a slot_id"));
				int num = (int)simulated.command["slot_id"];
				TFUtils.DebugLog("Building(" + simulated.Id.Describe() + "):RushingCraft on slot " + num);
				CraftingInstance craftingInstance = simulation.craftManager.GetCraftingInstance(simulated.Id, num);
				if (craftingInstance == null)
				{
					simulated.command = null;
					TFUtils.ErrorLog("RushingCraftState:Enter - instance is null");
					return;
				}
				CraftingRecipe recipeById = simulation.craftManager.GetRecipeById(craftingInstance.recipeId);
				if (recipeById == null)
				{
					simulated.command = null;
					TFUtils.ErrorLog("RushingCraftState:Enter - recipe is null");
					return;
				}
				int num2 = simulation.Router.CancelMatching(Command.TYPE.CRAFTED, simulated.Id, simulated.Id, new Dictionary<string, object> { { "slot_id", num } });
				TFUtils.Assert(num2 == 1, "Expected rush to only cancel exactly 1 command in the command router. But is instead replacing " + num2 + " commands targetting " + num);
				simulated.CalculateRushCompletionPercent(craftingInstance.ReadyTimeUtc, recipeById.craftTime);
				Cost cost = new Cost();
				cost += recipeById.rushCost;
				cost.Prorate((float)simulated.Variable[RUSH_PERCENT]);
				simulation.ModifyGameStateSimulated(simulated, new RushCraftAction(simulated.Id, num, cost, craftingInstance.ReadyTimeUtc, craftingInstance.reward));
				simulation.Router.Send(CraftedCommand.Create(simulated.Id, simulated.Id, num));
				ResourceManager resourceManager = simulation.resourceManager;
				if (resourceManager.CanPay(cost))
				{
					resourceManager.Apply(new Cost(cost), simulation.game);
				}
				else
				{
					TFUtils.Assert(false, "You don't have enough money! Consider showing an insufficient funds dialog before getting here!");
				}
				simulated.command = null;
			}

			public override void CancelCurrentCommands(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulation.game.notificationManager.CancelNotification("craft:" + simulated.Id.Describe());
			}

			protected override Cost GetRushCost(Simulation simulation, Simulated simulated)
			{
				throw new NotImplementedException("Get Rush Cost is currently not supported for Craft Buildings");
			}
		}

		public class FriendsParkInactiveState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = false;
				simulated.InteractionState.SetInteractions(false, false, false, false);
				simulated.simFlags |= SimulatedFlags.FORCE_ANIMATE_ACTION;
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class TaskFeedState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<BuildingEntity>().TaskSourceFeedDID = (int)simulated.command["source_id"];
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulation.soundEffectManager.PlaySound("MatchBonus_Ready");
				Setup(simulation, simulated);
				UpdateControls(simulation, simulated);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulation.soundEffectManager.PlaySound("MatchBonus_Open");
				CollectAndRecordRewards(simulation, simulated);
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulation simulation, Simulated simulated)
			{
				Task pTask = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out pTask);
				if (pTask != null && pTask.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					pTask.m_sTargetPrevDisplayState = "default";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("default");
				}
				simulated.DisplayThoughtState("bonus_ready", simulation);
				simulated.simFlags |= SimulatedFlags.FIRST_ANIMATE;
			}

			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				List<IControlBinding> list = new List<IControlBinding>();
				bool isEnabled = simulation.catalog.CanSell(simulated.entity.DefinitionId);
				string text = simulation.catalog.SellError(simulated.entity.DefinitionId);
				bool flag = entity.Stashable;
				string text2 = ((!flag) ? "!!CANNOT_STOW_PRODUCTION" : null);
				if (simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, false).Count > 0)
				{
					flag = (isEnabled = false);
					text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_BUILDING");
					text = ((text != null) ? text : "!!CANNOT_SELL_TASK_BUILDING");
				}
				else if (entity.ResidentDids != null)
				{
					int count = entity.ResidentDids.Count;
					for (int i = 0; i < count; i++)
					{
						if (simulation.game.taskManager.GetActiveTasksForSimulated(entity.ResidentDids[i], null).Count > 0)
						{
							flag = (isEnabled = false);
							text2 = ((text2 != null) ? text2 : "!!CANNOT_STOW_TASK_RESIDENT");
							text = ((text != null) ? text : "!!CANNOT_SELL_TASK_RESIDENT");
							break;
						}
					}
				}
				list.Add(new Session.StashControl(simulated, flag, text2));
				list.Add(new Session.SellControl(simulated, isEnabled, text));
				list.Add(new Session.AcceptPlacementControl());
				list.Add(new Session.RotateControl(simulated, entity.Flippable));
				list.Add(new Session.RejectControl());
				simulated.InteractionState.SetInteractions(true, true, false, true, null, list);
			}

			private void CollectAndRecordRewards(Simulation simulation, Simulated simulated)
			{
				Simulated simulated2 = simulation.FindSimulated(simulated.GetEntity<BuildingEntity>().TaskSourceFeedDID);
				ResidentEntity entity = simulated2.GetEntity<ResidentEntity>();
				Reward matchBonus = entity.MatchBonus;
				entity.MatchBonus = null;
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(matchBonus, simulation, simulated, TFUtils.EpochTime());
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Building.TaskFeedState.CollectAndRecordRewards - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = ((count <= 0) ? null : rewardDropResults.dropIdentities[count - 1]);
				CollectMatchBonusAction collectMatchBonusAction = new CollectMatchBonusAction(simulated2.Id, matchBonus);
				collectMatchBonusAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated2, collectMatchBonusAction);
				collectMatchBonusAction.AddPickup(simulation);
				AnalyticsWrapper.LogBonusChest(simulation.game, simulated2, matchBonus);
				if (ResourceManager.SPONGY_GAMES_CURRENCY >= 0)
				{
					int value = 0;
					if (matchBonus.ResourceAmounts.TryGetValue(ResourceManager.SPONGY_GAMES_CURRENCY, out value))
					{
						SBMISoaring.AddFoodToCharacter(value, simulated.GetEntity<ResidentEntity>().DefinitionId);
					}
				}
			}
		}

		public class TaskFeedCollectingState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				Task pTask = null;
				string activeDisplayStateForTarget = simulation.game.taskManager.GetActiveDisplayStateForTarget(simulated.Id, out pTask);
				if (pTask != null && pTask.m_bAtTarget && !string.IsNullOrEmpty(activeDisplayStateForTarget))
				{
					pTask.m_sTargetPrevDisplayState = "default";
					simulated.DisplayState(activeDisplayStateForTarget);
				}
				else
				{
					simulated.DisplayState("default");
				}
				simulated.DisplayThoughtState("bonus_collect", simulation);
				simulated.InteractionState.SetInteractions(false, false, false, false);
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), 1uL);
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		public delegate void Setup(Simulation simulation, Simulated simulated);

		public const string WORKER = "employee";

		public static PlacingAction Placing = new PlacingAction();

		public static PrimeErectingState PrimeErecting = new PrimeErectingState();

		public static PrimeErectingStateFriend PrimeErectingFriend = new PrimeErectingStateFriend();

		public static ErectingState Erecting = new ErectingState();

		public static InactiveState Inactive = new InactiveState();

		public static ActiveState Active = new ActiveState();

		public static RequestingInterfaceState RequestingInterface = new RequestingInterfaceState();

		public static ReflectingState Reflecting = new ReflectingState();

		public static ReplacingState Replacing = new ReplacingState();

		public static ActivatingState Activating = new ActivatingState();

		public static ReactivatingState Reactivating = new ReactivatingState();

		public static ProducingState Producing = new ProducingState();

		public static ProducedState Produced = new ProducedState();

		public static CraftingState Crafting = new CraftingState();

		public static CraftedState Crafted = new CraftedState();

		public static CraftCyclingState CraftCycling = new CraftCyclingState();

		public static CraftingCollectState CraftingCollect = new CraftingCollectState();

		public static RushingBuildState RushingBuild = new RushingBuildState();

		public static RushingProductState RushingProduct = new RushingProductState();

		public static RushingCraftState RushingCraft = new RushingCraftState();

		public static FriendsParkInactiveState FriendParkInactive = new FriendsParkInactiveState();

		public static TaskFeedState TaskFeed = new TaskFeedState();

		public static TaskFeedCollectingState TaskFeedCollecting = new TaskFeedCollectingState();

		public static Simulated Load(BuildingEntity buildingEntity, Simulation simulation, Vector2 position, bool flip, ulong utcNow)
		{
			if (flip && !buildingEntity.Flippable)
			{
				flip = false;
			}
			ErectableDecorator decorator = buildingEntity.GetDecorator<ErectableDecorator>();
			ActivatableDecorator decorator2 = buildingEntity.GetDecorator<ActivatableDecorator>();
			string text = ((decorator2.Activated != 0L) ? "reflecting" : "prime_erecting");
			if (SBSettings.ConsoleLoggingEnabled)
			{
				TFUtils.DebugLog(string.Concat("Loading building(name=", (string)buildingEntity.Invariable["name"], ", id=", buildingEntity.Id, ", did=", buildingEntity.DefinitionId, ", state=", text), TFUtils.LogFilter.Buildings);
			}
			Simulated simulated = simulation.CreateSimulated(buildingEntity, EntityManager.BuildingActions[text], position);
			simulated.Warp(position, simulation);
			simulated.Visible = true;
			simulated.Flip = flip;
			simulated.simFlags |= SimulatedFlags.FIRST_ANIMATE;
			if (text == "prime_erecting" && !simulated.Variable.ContainsKey("employee"))
			{
				Simulated simulated2 = simulation.SpawnWorker(simulated);
				ulong delay = decorator.ErectionTime;
				if (decorator.ErectionCompleteTime.HasValue)
				{
					delay = decorator.ErectionCompleteTime.Value - utcNow;
				}
				simulation.Router.Send(EmployCommand.Create(Identity.Null(), simulated.Id, simulated2.Id));
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated2.Id), delay);
				simulation.Router.Send(ReturnCommand.Create(simulated.Id, simulated2.Id), delay);
				simulation.Router.Send(ErectCommand.Create(simulated2.Id, simulated2.Id, Identity.Null(), 0uL));
				simulated.Variable["employee"] = simulated2.Id;
				AdjustWorkerPosition(simulated, simulation);
			}
			else if (text == "prime_erecting")
			{
				AdjustWorkerPosition(simulated, simulation);
			}
			simulated.SetFootprint(simulation);
			return simulated;
		}

		public static void AdjustWorkerPosition(Simulated building, Simulation simulation)
		{
			if (!building.Variable.ContainsKey("employee"))
			{
				return;
			}
			Identity identity = building.Variable["employee"] as Identity;
			if (identity != null)
			{
				Simulated simulated = simulation.FindSimulated(identity);
				if (simulated != null && simulated.Position != building.PointOfInterest)
				{
					simulated.Warp(building.PointOfInterest, simulation);
				}
			}
		}

		public static Simulated TryAddResident(Simulation simulation, Simulated building, int? residentDid, Identity existingResidentInstance = null)
		{
			if (residentDid.HasValue)
			{
				ulong num = TFUtils.EpochTime();
				Simulated simulated;
				if (existingResidentInstance == null)
				{
					ResidentEntity decorator = simulation.EntityManager.Create(EntityType.RESIDENT, residentDid.Value, null, true).GetDecorator<ResidentEntity>();
					if (decorator.Disabled)
					{
						return null;
					}
					decorator.HungerResourceId = null;
					decorator.PreviousResourceId = null;
					decorator.WishExpiresAt = null;
					decorator.HungryAt = num;
					decorator.MatchBonus = null;
					simulated = Resident.Load(decorator, building.Id, decorator.WishExpiresAt, decorator.HungerResourceId, decorator.PreviousResourceId, decorator.HungryAt, null, decorator.MatchBonus, simulation, num);
				}
				else
				{
					ResidentEntity decorator = (ResidentEntity)simulation.EntityManager.GetEntity(existingResidentInstance);
					if (decorator.Disabled)
					{
						return null;
					}
					simulated = Resident.Load(decorator, building.Id, decorator.WishExpiresAt, decorator.HungerResourceId, decorator.PreviousResourceId, decorator.HungryAt, decorator.FullnessLength, decorator.MatchBonus, simulation, num);
				}
				if (simulated != null)
				{
					simulated.Warp(building.PointOfInterest, simulation);
					simulated.Visible = true;
				}
				return simulated;
			}
			return null;
		}

		public static List<Simulated> FindResidents(Simulation simulation, Simulated building)
		{
			List<Simulated> list = new List<Simulated>();
			BuildingEntity entity = building.GetEntity<BuildingEntity>();
			if (entity.HasResident)
			{
				foreach (Simulated simulated in simulation.GetSimulateds())
				{
					if (simulated.entity is ResidentEntity && simulated.GetEntity<ResidentEntity>().Residence.Equals(building.Id))
					{
						list.Add(simulated);
					}
				}
			}
			return list;
		}

		public static void AddResidentToGameState(Dictionary<string, object> gameState, string residentId, int residentDid, string residenceId, ulong residentHungryTime)
		{
			List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["units"];
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["did"] = residentDid;
			dictionary["label"] = residentId;
			dictionary["residence"] = residenceId;
			dictionary["feed_ready_time"] = residentHungryTime;
			dictionary["waiting"] = false;
			dictionary["active"] = true;
			list.Add(dictionary);
		}

		public static void RemoveResidentsFromGameState(Dictionary<string, object> gameState, string buildingId)
		{
			List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["units"];
			Predicate<object> match = (object u) => ((string)((Dictionary<string, object>)u)["residence"]).Equals(buildingId);
			list.RemoveAll(match);
		}
	}

	public class Debris
	{
		public class UnpurchasedState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				Setup(simulation, simulated);
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.Variable["purchased"] = true;
				simulated.DisplayState("default");
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				simulated.InteractionState.IsSelectable = Session.TheDebugManager.debugPlaceObjects;
				return false;
			}

			public static void Setup(Simulation simulation, Simulated simulated)
			{
				simulated.InteractionState.Clear();
				simulated.DisplayState("inactive");
			}
		}

		public class InactiveState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				Setup(simulation, simulated);
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("default");
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("inactive");
				simulated.InteractionState.SetInteractions(false, false, true, false);
				simulated.InteractionState.PushControls(new List<IControlBinding>
				{
					new Session.ClearDebrisControl(simulated)
				});
			}
		}

		public class ClearingState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				ClearableDecorator entity = simulated.GetEntity<ClearableDecorator>();
				if (!entity.HasStartedClearing)
				{
					simulation.soundEffectManager.PlaySound("StartDebrisClearing");
					entity.ClearCompleteTime = TFUtils.EpochTime() + entity.ClearTime;
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), entity.ClearTime);
				}
				else
				{
					simulation.Router.CancelMatching(Command.TYPE.COMPLETE, simulated.Id, simulated.Id);
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), entity.RemainingTime(TFUtils.EpochTime()));
				}
				simulated.command = null;
				Setup(simulation, simulated);
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.ClearSimulateOnce();
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.InteractionState.Clear();
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				simulated.SimulateOnce();
				return false;
			}

			public static void Setup(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("clearing");
				simulated.DisplayThoughtState("clearing", simulation);
				simulated.InteractionState.SetInteractions(false, false, true, true);
				simulated.simFlags |= SimulatedFlags.FIRST_ANIMATE;
			}
		}

		public class ClearingMoreInfoState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulation.soundEffectManager.PlaySound("StartDebrisClearing");
				ClearableDecorator entity = simulated.GetEntity<ClearableDecorator>();
				TFUtils.DebugLog("Debris(" + simulated.Id.Describe() + "):ClearingMoreInfo. Ready in " + entity.ClearTime);
				simulated.command = null;
				simulated.DisplayState("clearing_more_info");
				simulated.DisplayThoughtState("clearing_more_info", simulation);
				simulated.InteractionState.SetInteractions(false, false, true, true);
				IDisplayController thoughtItemBubbleDisplayController = simulated.thoughtItemBubbleDisplayController;
				SBGUILabel dynamicThinkingLabel = simulated.DynamicThinkingLabel;
				thoughtItemBubbleDisplayController.AttachGUIElementToTarget(dynamicThinkingLabel, "BN_CLOCK_TIME");
				dynamicThinkingLabel.transform.rotation = thoughtItemBubbleDisplayController.Transform.rotation;
				dynamicThinkingLabel.transform.localScale = new Vector3(-10f, 10f, -1f);
				simulated.ThinkingGhostButton.ClickEvent += delegate
				{
					if (!simulation.Whitelisted || simulation.CheckWhitelist(simulated))
					{
						simulation.Router.Send(RushCommand.Create(simulated.Id));
					}
				};
				ulong delay = Math.Min(CalculateRemainingTime(simulated), 8uL);
				simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id), delay);
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulation.Router.CancelMatching(Command.TYPE.ABORT, simulated.Id, simulated.Id);
				simulated.DisplayState("clearing");
				simulated.DisplayThoughtState("clearing", simulation);
				simulated.RemoveDynamicThinkingElements();
				simulated.ClearSimulateOnce();
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.InteractionState.Clear();
			}

			public ulong CalculateRemainingTime(Simulated simulated)
			{
				ClearableDecorator entity = simulated.GetEntity<ClearableDecorator>();
				ulong utcNow = TFUtils.EpochTime();
				return entity.RemainingTime(utcNow);
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				string text = string.Format(TFUtils.DurationToString(CalculateRemainingTime(simulated)));
				simulated.DynamicThinkingLabel.Text = text;
				SBGUIButton thinkingGhostButton = simulated.ThinkingGhostButton;
				thinkingGhostButton.SetParent(null);
				thinkingGhostButton.SetScreenPosition(simulation.ScreenPositionFromWorldPosition(simulated.thoughtItemBubbleDisplayController.Position));
				thinkingGhostButton.UpdateCollider();
				return false;
			}
		}

		public class PrimingRushState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				ClearableDecorator clearable = simulated.GetEntity<ClearableDecorator>();
				ulong started = clearable.ClearCompleteTime.Value - clearable.ClearTime;
				Action<Session> execute = delegate(Session session)
				{
					simulation.Router.Send(RushCommand.Create(simulated.Id));
					int value = 0;
					Cost.Prorate(clearable.ClearingRushCost, started, clearable.ClearCompleteTime.Value, TFUtils.EpochTime()).ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out value);
					AnalyticsWrapper.LogJellyConfirmation(session.TheGame, clearable.DefinitionId, value, "Accelerate_" + clearable.Name, "debris", "speedup", "debris", "confirm");
				};
				Action<Session> cancel = delegate
				{
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
					int value = 0;
					Cost.Prorate(clearable.ClearingRushCost, started, clearable.ClearCompleteTime.Value, TFUtils.EpochTime()).ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out value);
				};
				simulated.rushParameters = new RushParameters(execute, cancel, (ulong time) => Cost.Prorate(clearable.ClearingRushCost, started, clearable.ClearCompleteTime.Value, time), clearable.BlueprintName, clearable.DefinitionId, delegate(Session session, Cost cost, bool canAfford)
				{
					LogRush(session, simulated, cost, canAfford);
				}, simulation.ScreenPositionFromWorldPosition(simulated.ThoughtDisplayController.Position));
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				session.AddAsyncResponse("request_rush_sim", simulated);
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				return false;
			}

			private void LogRush(Session session, Simulated simulated, Cost cost, bool canAfford)
			{
				session.analytics.LogRushClear(simulated.entity.BlueprintName, cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], canAfford);
			}
		}

		public class RushingClearingState : RushingSomething
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				ClearableDecorator entity = simulated.GetEntity<ClearableDecorator>();
				TFUtils.Assert(entity.ClearCompleteTime.HasValue, "Should be setting up the clear complete time before we get here.");
				simulated.CalculateRushCompletionPercent(entity.ClearCompleteTime.Value, entity.ClearTime);
				entity.ClearCompleteTime = TFUtils.EpochTime();
				base.Enter(simulation, simulated);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				Cost cost = new Cost();
				cost += simulated.GetEntity<ClearableDecorator>().ClearingRushCost;
				cost.Prorate((float)simulated.Variable[RUSH_PERCENT]);
				simulation.ModifyGameStateSimulated(simulated, new RushDebrisAction(simulated.Id, cost, simulated.GetEntity<ClearableDecorator>().ClearCompleteTime.Value));
			}

			protected override Cost GetRushCost(Simulation simulation, Simulated simulated)
			{
				return simulated.GetEntity<ClearableDecorator>().ClearingRushCost;
			}
		}

		public class DeletingState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				Setup(simulation, simulated);
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				SpawnDrops(simulation, simulated);
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.InteractionState.SetInteractions(false, false, false, true);
				simulated.DisplayState("deleting");
				simulated.DisplayThoughtState(simulated.GetEntity<ClearableDecorator>().ClearingReward.Summary.ThoughtIcon, "deleting", simulation);
				simulated.simFlags |= SimulatedFlags.FIRST_ANIMATE;
			}

			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
				Reward reward = GetReward(simulation, simulated);
				ulong utcNow = TFUtils.EpochTime();
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, simulated, utcNow);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Debris.DeletingState.SpawnDrops - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = ((count <= 0) ? null : rewardDropResults.dropIdentities[count - 1]);
				DebrisCompleteAction debrisCompleteAction = new DebrisCompleteAction(simulated.Id, reward);
				debrisCompleteAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, debrisCompleteAction);
				debrisCompleteAction.AddPickup(simulation);
				simulation.soundEffectManager.PlaySound("PopResourceBubble");
				simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Pop", 0, 0, 0f, simulated.thoughtBubblePopParticleRequestDelegate);
			}

			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				return simulated.GetEntity<ClearableDecorator>().ClearingReward.GenerateReward(simulation, false);
			}
		}

		public class DeletedState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.DisplayState(null);
				simulated.DisplayThoughtState(null, simulation);
				simulated.InteractionState.Clear();
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return true;
			}
		}

		public delegate void Setup(Simulation simulation, Simulated simulated);

		public static UnpurchasedState Unpurchased = new UnpurchasedState();

		public static InactiveState Inactive = new InactiveState();

		public static ClearingState Clearing = new ClearingState();

		public static ClearingMoreInfoState ClearingMoreInfo = new ClearingMoreInfoState();

		public static PrimingRushState PrimingRush = new PrimingRushState();

		public static RushingClearingState RushingClearing = new RushingClearingState();

		public static DeletingState Deleting = new DeletingState();

		public static DeletedState Deleted = new DeletedState();

		public static Simulated Load(DebrisEntity debrisEntity, Simulation simulation, Vector2 position, ulong utcNow)
		{
			ClearableDecorator decorator = debrisEntity.GetDecorator<ClearableDecorator>();
			PurchasableDecorator decorator2 = debrisEntity.GetDecorator<PurchasableDecorator>();
			string key;
			if (!decorator2.Purchased)
			{
				key = "unpurchased";
			}
			else
			{
				if (decorator.ClearCompleteTime.HasValue)
				{
					ulong? clearCompleteTime = decorator.ClearCompleteTime;
					if (clearCompleteTime.HasValue && clearCompleteTime.Value <= utcNow)
					{
						key = "deleting";
						goto IL_00b7;
					}
				}
				if (decorator.ClearCompleteTime.HasValue)
				{
					ulong? clearCompleteTime2 = decorator.ClearCompleteTime;
					if (clearCompleteTime2.HasValue && clearCompleteTime2.Value > utcNow)
					{
						key = "clearing";
						goto IL_00b7;
					}
				}
				key = "inactive";
			}
			goto IL_00b7;
			IL_00b7:
			Simulated simulated = simulation.CreateSimulated(debrisEntity, EntityManager.DebrisActions[key], position);
			simulated.Warp(position, simulation);
			simulated.Visible = true;
			simulated.SetFootprint(simulation);
			return simulated;
		}
	}

	public class Disabled
	{
		public class DisabledState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return true;
			}
		}

		public static DisabledState Disable = new DisabledState();
	}

	public abstract class FollowingPath
	{
		private const float TOL = 10f;

		private const float TOLSQ = 100f;

		private const int PATHING_BUDGET = 200;

		public bool PathFound(Simulation simulation, Simulated simulated)
		{
			return null != simulated.Variable["path"];
		}

		public void FindPath(Simulation simulation, Simulated simulated)
		{
			TerrainPathing terrainPathing = simulated.Variable["pathing"] as TerrainPathing;
			if (terrainPathing != null)
			{
				simulated.Variable["pathGoal"] = terrainPathing.Goal;
				switch (terrainPathing.Seek(200))
				{
				case PathFinder2.PROGRESS.DONE:
				{
					Path<GridPosition> path;
					terrainPathing.BuildPath(out path);
					path.Begin();
					simulated.ClearPathInfo();
					simulated.Variable["path"] = path;
					simulated.Variable["pathing"] = null;
					simulated.Variable["speed.variance"] = GetMovespeedVariance();
					break;
				}
				case PathFinder2.PROGRESS.FAILED:
					simulated.commands.Clear();
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
					break;
				}
			}
		}

		public bool FollowPath(Simulation simulation, Simulated simulated)
		{
			bool result = false;
			Path<GridPosition> path = simulated.Variable["path"] as Path<GridPosition>;
			if (path.Done())
			{
				simulated.Warp(simulated.position[1]);
				result = true;
			}
			else
			{
				Vector2 vector = simulation.Terrain.ComputeWorldPosition(path.Current);
				Vector2 vector2 = vector - simulated.Position;
				float sqrMagnitude = vector2.sqrMagnitude;
				if (sqrMagnitude < 100f)
				{
					path.Next();
					result = FollowPath(simulation, simulated);
				}
				else
				{
					float num = Mathf.Sqrt(sqrMagnitude);
					vector2 /= num;
					simulated.Position += Mathf.Min(num, simulation.TimeStep * ((float)simulated.Variable["speed"] + GetSpeedAddition(simulated)) * (float)simulated.Variable["speed.variance"]) * vector2;
				}
			}
			return result;
		}

		public bool FollowPathSimulate(Simulation simulation, Simulated simulated)
		{
			if (PathFound(simulation, simulated))
			{
				if (FollowPath(simulation, simulated))
				{
					simulated.command = null;
					return true;
				}
			}
			else
			{
				FindPath(simulation, simulated);
			}
			return false;
		}

		public static void GetWaypointPath(Simulation simulation, Simulated simulated)
		{
			int num = 100;
			simulated.Variable["pathing"] = null;
			simulated.ClearPathInfo();
			Waypoint waypoint = null;
			while (waypoint == null && num > 0)
			{
				waypoint = simulation.GetRandomWaypoint();
				num--;
			}
			if (waypoint != null)
			{
				simulated.Variable["pathing"] = simulation.CreatePathing(simulated.Position, waypoint.Position);
			}
		}

		public void RandomWanderSimulate(Simulation simulation, Simulated simulated)
		{
			if (!simulated.Variable.ContainsKey("path") || (simulated.Variable["path"] == null && simulated.Variable["pathing"] == null) || FollowPathSimulate(simulation, simulated))
			{
				GetWaypointPath(simulation, simulated);
			}
		}

		private static float GetMovespeedVariance()
		{
			return UnityEngine.Random.Range(0.95f, 1.05f);
		}

		protected virtual float GetSpeedAddition(Simulated simulated)
		{
			return 0f;
		}
	}

	public class Landmark
	{
		public class UnpurchasedState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				Setup(simulated, simulation);
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.Variable["purchased"] = true;
				simulated.DisplayState("default");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulated simulated, Simulation simulation)
			{
				simulated.DisplayState("inactive");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}
		}

		public class InactiveState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				Setup(simulated, simulation);
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("default");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulated simulated, Simulation simulation)
			{
				simulated.DisplayState("inactive");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}
		}

		public class ActiveState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				Setup(simulated, simulation);
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("default");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulated simulated, Simulation simulation)
			{
				simulated.DisplayState("active");
				if (simulated.thoughtDisplayController != null)
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}
		}

		public delegate void Setup(Simulated simulated, Simulation simulation);

		public static UnpurchasedState Unpurchased = new UnpurchasedState();

		public static InactiveState Inactive = new InactiveState();

		public static ActiveState Active = new ActiveState();

		public static Simulated Load(LandmarkEntity landmarkEntity, Simulation simulation, Vector2 position, ulong utcNow)
		{
			string text = "inactive";
			text = ((!landmarkEntity.GetDecorator<PurchasableDecorator>().Purchased) ? "unpurchased" : ((landmarkEntity.GetDecorator<ActivatableDecorator>().Activated != 0L) ? "active" : "inactive"));
			Simulated simulated = simulation.CreateSimulated(landmarkEntity, EntityManager.LandmarkActions[text], position);
			simulated.Warp(position, simulation);
			simulated.Visible = true;
			simulated.SetFootprint(simulation);
			return simulated;
		}
	}

	public class Resident
	{
		public class IdleState : StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public void Setup(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.DisplayState("idle");
				simulated.DisplayThoughtState(null, simulation);
				simulated.InteractionState.SetInteractions(false, false, false, true);
			}
		}

		public class IdleFullState : IdleState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				simulated.SimulatedQueryable = true;
				simulated.GetEntity<ResidentEntity>().HomeAvailability = true;
				simulated.GetEntity<ResidentEntity>().StartCheckForResume();
				simulated.DisplayState("idle");
				simulated.InteractionState.SetInteractions(false, false, false, true);
				simulated.command = null;
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().StopCheckForResume();
				simulated.InteractionState.Clear();
				base.Leave(simulation, simulated);
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				bool result = base.Simulate(simulation, simulated, session);
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id);
				if (activeTasksForSimulated.Count > 0)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					return result;
				}
				if (simulated.GetEntity<ResidentEntity>().CheckForResume())
				{
					simulation.Router.Send(ResumeFullCommand.Create(simulated.Id, simulated.Id));
				}
				return result;
			}
		}

		public class IdleWishingState : IdleState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				simulated.SimulatedQueryable = true;
				simulated.GetEntity<ResidentEntity>().HomeAvailability = true;
				simulated.GetEntity<ResidentEntity>().StartCheckForResume();
				simulated.DisplayState("idle");
				simulated.InteractionState.SetInteractions(false, false, false, true);
				simulated.command = null;
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().StopCheckForResume();
				simulated.InteractionState.Clear();
				base.Leave(simulation, simulated);
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				bool result = base.Simulate(simulation, simulated, session);
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id);
				if (activeTasksForSimulated.Count > 0)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					return result;
				}
				if (simulated.GetEntity<ResidentEntity>().CheckForResume())
				{
					simulation.Router.Send(ResumeWishingCommand.Create(simulated.Id, simulated.Id));
				}
				return result;
			}
		}

		public class MovingState : FollowingPath, StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
				simulated.Variable["pathing"] = simulation.CreatePathing(simulated.Position, (Vector2)simulated.command["position"]);
				simulated.ClearPathInfo();
				simulated.SimulatedQueryable = true;
				simulated.DisplayState("walk");
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (FollowPathSimulate(simulation, simulated))
				{
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				}
				return false;
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class GoHomeState : FollowingPath, StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
				simulated.Variable["pathing"] = simulation.CreatePathing(simulated.Position, (Vector2)simulated.command["home_position"]);
				simulated.ClearPathInfo();
				simulated.command = null;
				simulated.DisplayState("walk");
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (FollowPathSimulate(simulation, simulated))
				{
					simulation.Router.Send(StoreResidentCommand.Create(simulated.Entity.Id, simulated.Entity.Id));
				}
				return false;
			}
		}

		public class StoreResidentState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
				simulated.ClearPathInfo();
				simulated.Variable["path"] = null;
				simulated.command = null;
				simulated.SimulatedQueryable = false;
				SwarmManager.Instance.StoreResident(simulation, simulated.GetEntity<ResidentEntity>());
			}
		}

		public class ResidingState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
				TFUtils.DebugLog("Resident(" + simulated.Id.Describe() + "):Residing(" + (simulated.command["residence"] as Identity).Describe() + ")");
				simulated.command = null;
			}
		}

		public class WanderingFullState : FollowingPath, StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				ulong delay = Math.Max(0uL, CalculateRemainingFullnessTime(simulated));
				Command command = HungerCommand.Create(simulated.Id, simulated.Id);
				simulation.Router.CancelMatching(command.Type, command.Sender, command.Receiver);
				simulation.Router.Send(command, delay);
				simulated.DisplayState("walk");
				simulated.DisplayThoughtState(null, simulation);
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.InteractionState.SetInteractions(false, false, false, true);
				simulated.SimulatedQueryable = true;
				simulated.GetEntity<ResidentEntity>().StartCheckForIdle();
				simulated.GetEntity<ResidentEntity>().HomeAvailability = true;
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().StopCheckForIdle();
				simulated.Warp(simulated.Position);
				simulation.Router.CancelMatching(Command.TYPE.HUNGER, simulated.Id, simulated.Id);
				simulated.InteractionState.Clear();
			}

			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id);
				if (activeTasksForSimulated.Count > 0)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					return false;
				}
				if (simulated.GetEntity<ResidentEntity>().CheckForIdle())
				{
					simulated.Variable["pathing"] = null;
					simulated.ClearPathInfo();
					simulated.Warp(simulated.Position);
					simulation.Router.Send(IdlePauseCommand.Create(simulated.Id, simulated.Id));
				}
				else
				{
					RandomWanderSimulate(simulation, simulated);
				}
				return false;
			}

			protected ulong CalculateRemainingFullnessTime(Simulated simulated)
			{
				return simulated.GetEntity<ResidentEntity>().HungryAt - TFUtils.EpochTime();
			}
		}

		public class WanderingHungryState : FollowingPath, StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				ulong num = 0uL;
				if (entity.HungerResourceId.HasValue)
				{
					num = 0uL;
				}
				else
				{
					num = (ulong)UnityEngine.Random.Range(entity.WishCooldownMin, entity.WishCooldownMax);
					entity.WishExpiresAt = TFUtils.EpochTime() + num + (ulong)entity.WishDuration;
				}
				if (simulation.featureManager.CheckFeature("resident_wishes"))
				{
					simulation.Router.Send(WishCommand.Create(simulated.Id, simulated.Id), num);
				}
				else
				{
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), 60uL);
				}
				FollowingPath.GetWaypointPath(simulation, simulated);
				simulated.DisplayState("walk");
				simulated.DisplayThoughtState(null, simulation);
				simulated.InteractionState.SetInteractions(false, false, false, true);
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayThoughtState(null, simulation);
				simulated.Warp(simulated.Position);
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id);
				if (activeTasksForSimulated.Count > 0)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					return false;
				}
				RandomWanderSimulate(simulation, simulated);
				return false;
			}
		}

		public abstract class WishingForSomething : FollowingPath, StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				TFUtils.Assert(simulated.ThoughtMaskDisplayController != null, "Simulateds that wish for something must have a thought mask display controller. This one does not. Sim=" + simulated);
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (entity.WishExpiresAt.HasValue)
				{
					ulong? wishExpiresAt = entity.WishExpiresAt;
					if (!wishExpiresAt.HasValue || wishExpiresAt.Value > TFUtils.EpochTime())
					{
						goto IL_0088;
					}
				}
				entity.WishExpiresAt = TFUtils.EpochTime() + (ulong)entity.WishDuration;
				goto IL_0088;
				IL_0088:
				FollowingPath.GetWaypointPath(simulation, simulated);
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.InteractionState.SetInteractions(false, false, false, true);
				simulated.DisplayState("walk");
				entity.StartCheckForIdle();
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().StopCheckForIdle();
				simulated.Warp(simulated.Position);
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id);
				if (activeTasksForSimulated.Count > 0)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					return false;
				}
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				TFUtils.Assert(entity.WishExpiresAt.HasValue, "No expiration time set... something is wrong!");
				if (entity.CheckForIdle())
				{
					simulated.Variable["pathing"] = null;
					simulated.ClearPathInfo();
					simulated.Warp(simulated.Position);
					simulation.Router.Send(IdlePauseCommand.Create(simulated.Id, simulated.Id));
				}
				else
				{
					RandomWanderSimulate(simulation, simulated);
				}
				if (TFUtils.EpochTime() >= entity.WishExpiresAt.Value && entity.HungerResourceId.HasValue && !simulation.resourceManager.Resources[entity.HungerResourceId.Value].IgnoreWishDurationTimer)
				{
					entity.PreviousResourceId = entity.HungerResourceId;
					entity.HungerResourceId = null;
					simulation.ModifyGameStateSimulated(simulated, new FailWishAction(simulated));
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
					simulated.DisplayThoughtState(null, simulation);
					simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Thought_Pop", 0, 0, 0f, simulated.thoughtBubblePopParticleRequestDelegate);
				}
				return false;
			}
		}

		public class WishingForFoodState : WishingForSomething
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				entity.HomeAvailability = true;
				simulated.SimulatedQueryable = true;
				int? hungerResourceId = entity.HungerResourceId;
				if (hungerResourceId.HasValue)
				{
					CdfDictionary<int> cdfDictionary = null;
					if (entity.CostumeDID.HasValue)
					{
						CostumeManager.Costume costume = simulation.game.costumeManager.GetCostume(entity.CostumeDID.Value);
						if (costume != null && costume.m_nWishTableDID >= 0)
						{
							cdfDictionary = simulation.game.wishTableManager.GetWishTable(costume.m_nWishTableDID);
						}
					}
					if (cdfDictionary == null)
					{
						cdfDictionary = simulation.game.wishTableManager.GetWishTable(entity.WishTableDID);
					}
					if (cdfDictionary != null)
					{
						cdfDictionary = cdfDictionary.Where((int productId) => productId == hungerResourceId.Value, true);
					}
					if (cdfDictionary == null || cdfDictionary.Count <= 0)
					{
						hungerResourceId = null;
					}
				}
				if (!hungerResourceId.HasValue)
				{
					hungerResourceId = GenerateHungerResourceID(simulation, entity);
					if (hungerResourceId.HasValue && hungerResourceId.GetValueOrDefault() == ResourceManager.DEFAULT_WISH && hungerResourceId.HasValue)
					{
						hungerResourceId = null;
						simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
					}
					if (!hungerResourceId.HasValue)
					{
						entity.WishExpiresAt = 0uL;
					}
					else if (!simulation.resourceManager.Resources.ContainsKey(hungerResourceId.Value))
					{
						TFUtils.WarningLog("Missing resource: " + hungerResourceId.Value);
						TFUtils.DebugLog("Attempting to wish for a resource we do not have: " + hungerResourceId.Value);
						entity.WishExpiresAt = 0uL;
					}
					else
					{
						if (hungerResourceId == 9100 && hungerResourceId.HasValue)
						{
							if (simulated.StateModifierString == null)
							{
								simulated.StateModifierString = "jerky{0}";
								simulated.DisplayState(simulated.GetDisplayState());
							}
						}
						else
						{
							RefreshModifiedDisplayState(simulated);
						}
						entity.HungerResourceId = hungerResourceId;
						simulation.ModifyGameStateSimulated(simulated, new NewWishAction(simulated.Id, hungerResourceId.Value, entity.PreviousResourceId, entity.WishExpiresAt.Value));
					}
				}
				int? forcedWish = simulated.forcedWish;
				if (forcedWish.HasValue)
				{
					entity.HungerResourceId = simulated.forcedWish;
					hungerResourceId = simulated.forcedWish;
				}
				if (hungerResourceId.HasValue)
				{
					Resource resource = simulation.resourceManager.Resources[hungerResourceId.Value];
					string resourceTexture = resource.GetResourceTexture();
					simulated.DisplayThoughtState(resourceTexture, "hungry", simulation);
				}
				else
				{
					simulated.DisplayThoughtState(null, simulation);
				}
			}
		}

		public class TemptedState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				Command command = simulated.command;
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
				int value = simulated.GetEntity<ResidentEntity>().HungerResourceId.Value;
				int temptingWith = (int)command["product_id"];
				string resourceTexture = simulation.resourceManager.Resources[temptingWith].GetResourceTexture();
				int? num = entity.GrossItemsWishTableDID;
				int? num2 = entity.ForbiddenItemsWishTableDID;
				bool flag = false;
				bool flag2 = false;
				if (num.HasValue)
				{
					CdfDictionary<int> cdfDictionary = simulation.game.wishTableManager.GetWishTable(entity.GrossItemsWishTableDID);
					if (cdfDictionary != null)
					{
						cdfDictionary = cdfDictionary.Where((int productID) => productID == temptingWith, true);
					}
					if (cdfDictionary != null && cdfDictionary.Count > 0)
					{
						flag = true;
					}
				}
				if (num2.HasValue)
				{
					CdfDictionary<int> cdfDictionary2 = simulation.game.wishTableManager.GetWishTable(entity.ForbiddenItemsWishTableDID);
					if (cdfDictionary2 != null)
					{
						cdfDictionary2 = cdfDictionary2.Where((int productID) => productID == temptingWith, true);
					}
					if (cdfDictionary2 != null && cdfDictionary2.Count > 0)
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					simulated.showUnavailableIcon = true;
					simulated.DisplayState("gimme");
					simulated.DisplayThoughtState("empty.png", "acceptable", simulation);
					AttachLabelToThoughtBubbleBone(simulated, simulated.thoughtItemBubbleDisplayController, Language.Get("!!FEED_NO_WAY_COMMENT"));
					AttachProductIconToThoughtBubbleBone(simulated, simulated.thoughtItemBubbleDisplayController, resourceTexture);
					simulation.soundEffectManager.PlaySound("NotTempted");
					simulated.DisplayState("NotTempted");
				}
				else if (flag)
				{
					simulated.showUnavailableIcon = false;
					simulated.DisplayState("deny");
					simulated.DisplayThoughtState("empty.png", "acceptable", simulation);
					AttachLabelToThoughtBubbleBone(simulated, simulated.thoughtItemBubbleDisplayController, Language.Get("!!FEED_YUCK_COMMENT"));
					AttachProductIconToThoughtBubbleBone(simulated, simulated.thoughtItemBubbleDisplayController, resourceTexture);
					simulation.soundEffectManager.PlaySound("NotTempted");
				}
				else if (value == temptingWith)
				{
					simulated.showUnavailableIcon = false;
					simulated.DisplayState("gimme");
					simulated.DisplayThoughtState("empty.png", "gimme", simulation);
					AttachLabelToThoughtBubbleBone(simulated, simulated.thoughtItemBubbleDisplayController, Language.Get("!!FEED_MATCH_COMMENT"));
					AttachProductIconToThoughtBubbleBone(simulated, simulated.thoughtItemBubbleDisplayController, resourceTexture);
					simulation.soundEffectManager.PlaySound("VeryTempted");
				}
				else
				{
					simulated.showUnavailableIcon = false;
					simulated.DisplayState("acceptable");
					simulated.DisplayThoughtState("empty.png", "acceptable", simulation);
					AttachLabelToThoughtBubbleBone(simulated, simulated.thoughtItemBubbleDisplayController, Language.Get("!!FEED_MISMATCH_COMMENT"));
					AttachProductIconToThoughtBubbleBone(simulated, simulated.thoughtItemBubbleDisplayController, resourceTexture);
					simulation.soundEffectManager.PlaySound("SomewhatTempted");
				}
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.RemoveDynamicThinkingElements();
			}

			private void AttachLabelToThoughtBubbleBone(Simulated simulated, IDisplayController target, string text)
			{
				SBGUILabel dynamicThinkingLabel = simulated.DynamicThinkingLabel;
				dynamicThinkingLabel.Text = text;
				AttachHelper(target, "BN_TYPE", dynamicThinkingLabel);
				dynamicThinkingLabel.transform.localScale = new Vector3(-10f, 10f, -3f);
			}

			private void AttachProductIconToThoughtBubbleBone(Simulated simulated, IDisplayController target, string textureOverride)
			{
				SBGUIAtlasImage dynamicThinkingIcon = simulated.DynamicThinkingIcon;
				dynamicThinkingIcon.SetTextureFromAtlas(textureOverride);
				AttachHelper(target, "BN_ITEM", dynamicThinkingIcon);
				dynamicThinkingIcon.transform.localScale = new Vector3(-4f, 4f, -1f);
			}

			private void AttachHelper(IDisplayController controller, string target, SBGUIElement element)
			{
				if (controller == null)
				{
					Debug.LogError("Cannot attach food icon to a null skeleton!");
					return;
				}
				controller.AttachGUIElementToTarget(element, target);
				element.transform.rotation = controller.Transform.rotation;
			}
		}

		public class NotTemptedState : TransitionallyAnimating
		{
			protected override string DisplayStateName
			{
				get
				{
					return "deny";
				}
			}

			protected override string DisplayThoughtStateName
			{
				get
				{
					return null;
				}
			}

			protected override int AnimationLength
			{
				get
				{
					return 2;
				}
			}

			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				simulated.SimulatedQueryable = true;
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				TFUtils.Assert(entity.HungryAt > TFUtils.EpochTime(), "Unit entered NotTempted state, but it appears to be hungry. Something is probably wrong.");
				simulation.soundEffectManager.PlaySound("NotTempted");
				entity.HomeAvailability = false;
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), 3uL);
			}

			protected override string GetDisplayThoughtMaterial(Simulation simulation, Simulated simulated)
			{
				return null;
			}
		}

		public class PrimingRushFullnessState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				entity.HomeAvailability = false;
				Action<Session> execute = delegate(Session session)
				{
					simulation.Router.Send(RushCommand.Create(simulated.Id));
					int value = 0;
					entity.FullnessRushCostNow().ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out value);
					AnalyticsWrapper.LogJellyConfirmation(session.TheGame, entity.DefinitionId, value, entity.Name, "characters", "speedup", "fullness", "confirm");
				};
				Action<Session> cancel = delegate
				{
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
					int value = 0;
					entity.FullnessRushCostNow().ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out value);
				};
				simulated.rushParameters = new RushParameters(execute, cancel, (ulong time) => entity.FullnessRushCostNow(), entity.BlueprintName, entity.DefinitionId, delegate(Session session, Cost cost, bool canAfford)
				{
					LogRush(session, simulated, cost, canAfford);
				}, simulation.ScreenPositionFromWorldPosition(simulated.ThoughtDisplayController.Position));
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				session.AddAsyncResponse("request_rush_sim", simulated);
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				return false;
			}

			private void LogRush(Session session, Simulated simulated, Cost cost, bool canAfford)
			{
				session.analytics.LogRushFullness(simulated.entity.BlueprintName, cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], canAfford);
			}
		}

		public class RushingFullnessState : RushingSomething
		{
			public override void CancelCurrentCommands(Simulation simulation, Simulated simulated)
			{
				simulation.Router.CancelMatching(Command.TYPE.HUNGER, simulated.Id, simulated.Id);
			}

			public override void Enter(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				simulated.SimulatedQueryable = true;
				simulated.CalculateRushCompletionPercent(entity.HungryAt, entity.FullnessLength);
				entity.HomeAvailability = false;
				base.Enter(simulation, simulated);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				simulated.CalculateRushCompletionPercent(entity.HungryAt, entity.FullnessLength);
				Cost rushCost = entity.FullnessRushCostNow();
				entity.HungryAt = TFUtils.EpochTime();
				simulation.Router.Send(HungerCommand.Create(simulated.Id, simulated.Id), 0uL);
				simulation.ModifyGameStateSimulated(simulated, new RushHungerAction(simulated.Id, rushCost, entity.HungryAt));
				simulated.timebarMixinArgs.hasTimebar = false;
			}

			protected override Cost GetRushCost(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				return entity.FullnessRushCostFull;
			}
		}

		public class TryEatState : StateAction
		{
			private const string REQUEST_ERROR_PULSE = "RequestOpenInventory";

			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				Command command = simulated.command;
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				entity.HomeAvailability = false;
				bool flag = false;
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id);
				int num = entity.HungerResourceId.Value;
				bool flag2 = false;
				if (command.HasProperty("product_id"))
				{
					num = (int)command["product_id"];
					if (num == entity.HungerResourceId)
					{
						flag = true;
						if (!simulation.resourceManager.Resources[entity.HungerResourceId.Value].ForceNoWishPayout)
						{
							flag2 = true;
						}
						if (simulated.forcedWish == num)
						{
							simulated.forcedWish = null;
						}
					}
					else
					{
						if ((entity.HungerResourceId.HasValue && simulation.resourceManager.Resources[entity.HungerResourceId.Value].ForceWishMatch) || simulation.resourceManager.Resources[num].ForceWishMatch)
						{
							if (activeTasksForSimulated.Count > 0)
							{
								simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id), 0uL);
							}
							else
							{
								simulation.Router.Send(WishCommand.Create(simulated.Id, simulated.Id), 0uL);
							}
							return;
						}
						flag = false;
						simulated.DisplayThoughtState("empty.png", "hungry", simulation);
					}
					entity.PreviousResourceId = entity.HungerResourceId;
					entity.HungerResourceId = num;
				}
				if (simulation.resourceManager.HasEnough(num, 1))
				{
					if (flag2)
					{
						GenerateAndRecordBonusEarned((uint)num, simulation, simulated);
					}
					if (simulation.resourceManager.Resources[num].EatenSound != null)
					{
						simulation.soundEffectManager.PlaySound(simulation.resourceManager.Resources[num].EatenSound);
					}
					else
					{
						simulation.soundEffectManager.PlaySound("FeedUnit");
					}
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
					entity.WishExpiresAt = null;
					entity.FullnessLength = (ulong)simulation.resourceManager.Resources[num].FullnessTime;
					entity.FullnessRushCostFull = ResourceManager.CalculateFullnessRushCost(entity.FullnessLength);
					if (!flag)
					{
						StartHungerTimer(simulated.GetEntity<ResidentEntity>(), simulation);
					}
				}
				else
				{
					TFUtils.DebugLog("Simulated(" + simulated.Id.Describe() + ") could not eat!");
					simulation.soundEffectManager.PlaySound("DontHaveAny");
					simulated.Variable["RequestOpenInventory"] = true;
					if (activeTasksForSimulated.Count > 0)
					{
						simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id), 0uL);
						return;
					}
					simulation.Router.Send(WishCommand.Create(simulated.Id, simulated.Id), 0uL);
				}
				simulated.timebarMixinArgs.hasTimebar = false;
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				if (simulated.Variable.ContainsKey("RequestOpenInventory"))
				{
					simulated.Variable.Remove("RequestOpenInventory");
				}
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id);
				if (activeTasksForSimulated.Count > 0)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					return false;
				}
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				int value = entity.HungerResourceId.Value;
				object value2 = null;
				if (simulated.Variable.TryGetValue("RequestOpenInventory", out value2) && (bool)value2)
				{
					session.AddAsyncResponse("ShowInventoryHudWidget", true);
					session.AddAsyncResponse("PulseResourceError", value);
					simulated.Variable["RequestOpenInventory"] = false;
				}
				return false;
			}

			private void GenerateAndRecordBonusEarned(uint fedProductId, Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				Reward reward = null;
				if (entity.ForcedBonusReward != null)
				{
					reward = entity.ForcedBonusReward.GenerateReward(simulation, true);
					entity.ForcedBonusReward = null;
				}
				else
				{
					int num = entity.BonusPaytables.Length;
					for (int i = 0; i < num; i++)
					{
						reward = ((i != 0) ? (entity.BonusPaytables[i].Spin(fedProductId, simulation, Paytable.CONSOLATION_REWARD) + reward) : entity.BonusPaytables[i].Spin(fedProductId, simulation, Paytable.CONSOLATION_REWARD));
					}
					if (!simulation.featureManager.CheckFeature("recipe_drops") && reward.RecipeUnlocks != null && reward.RecipeUnlocks.Count > 0)
					{
						Dictionary<int, int> dictionary = new Dictionary<int, int>();
						dictionary.Add(ResourceManager.SOFT_CURRENCY, 5);
						reward = new Reward(dictionary, null, null, null, null, null, null, null, false, simulation.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].GetResourceTexture());
					}
					if (reward.RecipeUnlocks != null && reward.RecipeUnlocks.Count > 0)
					{
						foreach (int recipeUnlock in reward.RecipeUnlocks)
						{
							simulation.craftManager.ReserveRecipe(recipeUnlock);
						}
					}
				}
				TFUtils.Assert(reward != null, "Got a null match bonus! Need to adjust the bonus paytables so they always get something!");
				entity.MatchBonus = reward;
				if (reward != null)
				{
					simulation.ModifyGameStateSimulated(simulated, new EarnMatchBonusAction(entity.Id, reward));
				}
			}
		}

		public class WaitingForDeliveryState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				simulated.command = null;
				int value = simulated.GetEntity<ResidentEntity>().HungerResourceId.Value;
				if (simulation.resourceManager.HasEnough(value, 1))
				{
					session.AddAsyncResponse("GoodDeliveryRequest", new GoodToSimulatedDeliveryRequest(simulated, value, simulation.resourceManager.Resources[value].GetResourceTexture()));
				}
				simulation.Router.Send(OfferFoodCommand.Create(simulated.Id, simulated.Id, value));
				return false;
			}
		}

		public abstract class TransitionallyAnimating : StateActionDefault
		{
			protected abstract string DisplayStateName { get; }

			protected abstract string DisplayThoughtStateName { get; }

			protected abstract int AnimationLength { get; }

			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.DisplayState(DisplayStateName);
				string displayThoughtMaterial = GetDisplayThoughtMaterial(simulation, simulated);
				if (displayThoughtMaterial != null)
				{
					simulated.DisplayThoughtState(displayThoughtMaterial, DisplayThoughtStateName, simulation);
				}
				else
				{
					simulated.DisplayThoughtState(DisplayThoughtStateName, simulation);
				}
				simulated.InteractionState.SetInteractions(false, false, false, true);
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), (ulong)AnimationLength);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("default");
				simulated.DisplayThoughtState(null, simulation);
			}

			protected abstract string GetDisplayThoughtMaterial(Simulation simulation, Simulated simulated);
		}

		public class CheeringState : TransitionallyAnimating
		{
			protected override string DisplayStateName
			{
				get
				{
					return "cheer";
				}
			}

			protected override string DisplayThoughtStateName
			{
				get
				{
					return null;
				}
			}

			protected override int AnimationLength
			{
				get
				{
					return 2;
				}
			}

			protected override string GetDisplayThoughtMaterial(Simulation simulation, Simulated simulated)
			{
				return null;
			}
		}

		public class EatingState : TransitionallyAnimating
		{
			protected override string DisplayStateName
			{
				get
				{
					return "eat";
				}
			}

			protected override string DisplayThoughtStateName
			{
				get
				{
					return "hungry";
				}
			}

			protected override int AnimationLength
			{
				get
				{
					return 3;
				}
			}

			protected override string GetDisplayThoughtMaterial(Simulation simulation, Simulated simulated)
			{
				return simulation.resourceManager.Resources[simulated.GetEntity<ResidentEntity>().HungerResourceId.Value].GetResourceTexture();
			}

			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				simulated.SimulatedQueryable = true;
				SpawnAndRecordRewards(simulation, simulated);
				TFUtils.Assert(simulated.ThoughtDisplayController != null, "This simulated doesn't have a thought mask display controller assigned! simulated=" + simulated);
				if (simulated.ThoughtMaskDisplayController != null)
				{
					simulated.ThoughtMaskDisplayController.DisplayState("consume");
				}
				simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Food_Crumbs", 0, 0, 0f, simulated.eatParticleRequestDelegate);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				base.Leave(simulation, simulated);
				simulation.soundEffectManager.PlaySound("EatComplete");
				simulated.GetEntity<ResidentEntity>().HungerResourceId = null;
			}

			private void SpawnAndRecordRewards(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				TFUtils.Assert(entity.HungerResourceId.HasValue, "Trying to spawn rewards but resident has not HungerResourceID. Did you clear it too early?");
				int value = entity.HungerResourceId.Value;
				simulation.resourceManager.Spend(value, 1, simulation.game);
				if (simulation.resourceManager.Resources[value].ForceNoWishPayout)
				{
					simulation.game.analytics.LogCharacterFeed(entity.DefinitionId, value);
					AnalyticsWrapper.LogCharacterFeed(simulation.game, entity, value, null);
					FeedUnitAction feedUnitAction = new FeedUnitAction(simulated, (ulong)simulation.resourceManager.Resources[value].FullnessTime, value, entity.PreviousResourceId, null);
					feedUnitAction.AddDropData(simulated, null);
					simulation.ModifyGameStateSimulated(simulated, feedUnitAction);
					return;
				}
				Reward reward = simulation.resourceManager.Resources[value].Reward.GenerateReward(simulation, false);
				ulong utcNow = TFUtils.EpochTime();
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, simulated, utcNow);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Resident.EatingState.SpawnAndRecordRewards - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = ((count <= 0) ? null : rewardDropResults.dropIdentities[count - 1]);
				simulation.game.analytics.LogCharacterFeed(entity.DefinitionId, value);
				AnalyticsWrapper.LogCharacterFeed(simulation.game, entity, value, reward);
				FeedUnitAction feedUnitAction2 = new FeedUnitAction(simulated, (ulong)simulation.resourceManager.Resources[value].FullnessTime, value, entity.PreviousResourceId, reward);
				feedUnitAction2.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, feedUnitAction2);
			}
		}

		public class TryBonusSpinState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				simulated.command = null;
				entity.HomeAvailability = false;
				if (entity.MatchBonus != null)
				{
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), 0uL);
				}
				else
				{
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id), 0uL);
				}
			}
		}

		public class WaitingForCollectBonusState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				TFUtils.Assert(simulated.GetEntity<ResidentEntity>().MatchBonus != null, "Got into WaitingForCollectBonus but there is no earned bonus!");
				simulated.DisplayState("acceptable");
				simulated.DisplayThoughtState("bonus_ready", simulation);
				simulation.soundEffectManager.PlaySound("MatchBonus_Ready");
				simulated.InteractionState.SetInteractions(false, false, false, true);
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulation.soundEffectManager.PlaySound("MatchBonus_Open");
				CollectAndRecordRewards(simulation, simulated);
				StartHungerTimer(simulated.GetEntity<ResidentEntity>(), simulation);
			}

			private void CollectAndRecordRewards(Simulation simulation, Simulated simulated)
			{
				RefreshModifiedDisplayState(simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				Reward matchBonus = entity.MatchBonus;
				entity.MatchBonus = null;
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(matchBonus, simulation, simulated, TFUtils.EpochTime());
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Resident.WaitingForCollectBonusState.CollectAndRecordRewards - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = ((count <= 0) ? null : rewardDropResults.dropIdentities[count - 1]);
				CollectMatchBonusAction collectMatchBonusAction = new CollectMatchBonusAction(simulated.Id, matchBonus);
				collectMatchBonusAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, collectMatchBonusAction);
				collectMatchBonusAction.AddPickup(simulation);
				AnalyticsWrapper.LogBonusChest(simulation.game, simulated, matchBonus);
				if (ResourceManager.SPONGY_GAMES_CURRENCY >= 0)
				{
					int value = 0;
					if (matchBonus.ResourceAmounts.TryGetValue(ResourceManager.SPONGY_GAMES_CURRENCY, out value))
					{
						SBMISoaring.AddFoodToCharacter(value, simulated.GetEntity<ResidentEntity>().DefinitionId);
					}
				}
			}
		}

		public class CheeringAfterBonusState : CheeringState
		{
			protected override string DisplayThoughtStateName
			{
				get
				{
					return "bonus_collect";
				}
			}
		}

		public class StartingWanderCycleState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id);
				if (activeTasksForSimulated.Count > 0)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					return;
				}
				simulated.SimulatedQueryable = true;
				simulated.InteractionState.Clear();
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.Variable["speed"] = simulated.Invariable["base_speed"];
				simulation.Router.Send(WanderCommand.Create(simulated.Id, simulated.Id));
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
			}
		}

		public class RequestingInterfaceState : StateAction
		{
			private bool bComplete;

			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				bComplete = false;
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (bComplete)
				{
					session.CheckAsyncRequest("RequestEntityInterface");
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
					bComplete = false;
					return false;
				}
				if (simulation.game.taskManager.GetTaskingStateForSimulated(simulation, simulated.entity.DefinitionId, simulated.Id) == TaskManager._eBlueprintTaskingState.eNone)
				{
					simulated.InteractionState.SelectedTransition = new Session.UnitIdleTransition(simulated);
				}
				else
				{
					simulated.InteractionState.SelectedTransition = new Session.UnitBusyTransition(simulated);
				}
				session.AddAsyncResponse("RequestEntityInterface", simulated, false);
				bComplete = true;
				return false;
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class ReflectingState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.Variable["speed"] = simulated.Invariable["base_speed"];
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id);
				if (activeTasksForSimulated.Count > 0)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
					return false;
				}
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				Command command = null;
				command = ((!entity.HungerResourceId.HasValue) ? ResumeFullCommand.Create(simulated.Id, simulated.Id) : ResumeWishingCommand.Create(simulated.Id, simulated.Id));
				simulation.Router.Send(command);
				return false;
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class TaskDelegatingState : StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id);
				Task task = null;
				if (activeTasksForSimulated.Count > 0)
				{
					task = activeTasksForSimulated[0];
				}
				if (task == null)
				{
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
					return;
				}
				TaskData._eTaskType eTaskType = task.m_pTaskData.m_eTaskType;
				if (eTaskType == TaskData._eTaskType.eEnter && simulated.GetEntity<ResidentEntity>().MatchBonus != null)
				{
					simulation.Router.Send(EnterCommand.Create(simulated.Id, simulated.Id));
				}
				else if (task.GetTimeLeft() == 0)
				{
					simulation.game.triggerRouter.RouteTrigger(ReevaluateTrigger.CreateTrigger(), simulation.game);
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				}
				else if (eTaskType == TaskData._eTaskType.eEnter || eTaskType == TaskData._eTaskType.eActivate || eTaskType == TaskData._eTaskType.eStand)
				{
					Simulated simulated2 = simulation.FindSimulated(task.m_pTargetIdentity);
					simulation.Router.Send(MoveCommand.Create(simulated.Id, simulated.Id, simulated2.PointOfInterest + task.m_pTaskData.m_pPosOffsetFromTarget, simulated2.Flip));
				}
				else
				{
					simulation.Router.Send(WanderCommand.Create(simulated.Id, simulated.Id));
				}
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class TaskUpdateState : FollowingPath, StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id);
				if (activeTasksForSimulated.Count > 0)
				{
					entity.m_pTask = activeTasksForSimulated[0];
				}
				simulated.command = null;
				if (entity.HungerResourceId.HasValue)
				{
					Resource resource = simulation.resourceManager.Resources[entity.HungerResourceId.Value];
					string resourceTexture = resource.GetResourceTexture();
					simulated.DisplayThoughtState(resourceTexture, "hungry", simulation);
				}
				else
				{
					simulated.DisplayThoughtState(null, simulation);
				}
				simulated.InteractionState.SetInteractions(false, false, false, true, simulated.InteractionState.SelectedTransition);
				simulated.timebarMixinArgs.hasTimebar = false;
				simulated.SimulatedQueryable = true;
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (entity.m_pTask.GetTimeLeft() == 0)
				{
					simulation.game.triggerRouter.RouteTrigger(ReevaluateTrigger.CreateTrigger(), simulation.game);
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
				}
				ulong num = TFUtils.EpochTime();
				int? hungerResourceId = null;
				if (entity.HungerResourceId.HasValue)
				{
					ulong? wishExpiresAt = entity.WishExpiresAt;
					if (wishExpiresAt.HasValue && wishExpiresAt.Value <= num)
					{
						hungerResourceId = GenerateHungerResourceID(simulation, entity);
						if (hungerResourceId.HasValue)
						{
							entity.PreviousResourceId = entity.HungerResourceId;
						}
					}
				}
				else if (entity.HungryAt <= num)
				{
					hungerResourceId = GenerateHungerResourceID(simulation, entity);
				}
				if (hungerResourceId.HasValue && hungerResourceId.GetValueOrDefault() == ResourceManager.DEFAULT_WISH && hungerResourceId.HasValue)
				{
					hungerResourceId = null;
				}
				if (hungerResourceId.HasValue)
				{
					entity.HungerResourceId = hungerResourceId;
					entity.WishExpiresAt = num + (ulong)entity.WishDuration;
					simulation.ModifyGameStateSimulated(simulated, new NewWishAction(simulated.Id, entity.HungerResourceId.Value, entity.PreviousResourceId, entity.WishExpiresAt.Value));
					ShowNewHungerResource(simulation, simulated, entity.HungerResourceId.Value);
				}
				return false;
			}

			protected virtual void ShowNewHungerResource(Simulation simulation, Simulated simulated, int nDID)
			{
				Resource resource = simulation.resourceManager.Resources[nDID];
				string resourceTexture = resource.GetResourceTexture();
				simulated.DisplayThoughtState(resourceTexture, "hungry", simulation);
				simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Thought_Pop", 0, 0, 0f, simulated.thoughtBubblePopParticleRequestDelegate);
			}
		}

		public class TaskIdleState : TaskUpdateState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				float fWanderTime = entity.m_pTask.m_pTaskData.m_fWanderTime;
				float fIdleTime = entity.m_pTask.m_pTaskData.m_fIdleTime;
				if (fIdleTime <= 0f && fWanderTime > 0f)
				{
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
					return;
				}
				simulated.DisplayState(entity.m_pTask.m_pTaskData.m_sSourceDisplayStateIdle);
				entity.StartCheckForResume(Mathf.RoundToInt(fIdleTime), Mathf.RoundToInt(fIdleTime));
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				base.Leave(simulation, simulated);
				simulated.GetEntity<ResidentEntity>().StopCheckForResume();
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				base.Simulate(simulation, simulated, session);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				float fWanderTime = entity.m_pTask.m_pTaskData.m_fWanderTime;
				if (fWanderTime > 0f && entity.CheckForResume())
				{
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
				}
				return false;
			}
		}

		public class TaskWanderState : TaskUpdateState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				float fWanderTime = entity.m_pTask.m_pTaskData.m_fWanderTime;
				float fIdleTime = entity.m_pTask.m_pTaskData.m_fIdleTime;
				if (fWanderTime <= 0f && fIdleTime > 0f)
				{
					simulation.Router.Send(IdlePauseCommand.Create(simulated.Id, simulated.Id));
					return;
				}
				float fMovementSpeed = entity.m_pTask.m_pTaskData.m_fMovementSpeed;
				if (fMovementSpeed > 0f)
				{
					simulated.Variable["speed"] = fMovementSpeed;
				}
				simulated.DisplayState(entity.m_pTask.m_pTaskData.m_sSourceDisplayStateWalk);
				entity.StartCheckForIdle(Mathf.RoundToInt(fWanderTime), Mathf.RoundToInt(fWanderTime));
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				base.Leave(simulation, simulated);
				simulated.Warp(simulated.Position);
				simulated.GetEntity<ResidentEntity>().StopCheckForIdle();
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				base.Simulate(simulation, simulated, session);
				RandomWanderSimulate(simulation, simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				float fIdleTime = entity.m_pTask.m_pTaskData.m_fIdleTime;
				if (fIdleTime > 0f && entity.CheckForIdle())
				{
					simulated.Variable["pathing"] = null;
					simulated.ClearPathInfo();
					simulated.Warp(simulated.Position);
					simulation.Router.Send(IdlePauseCommand.Create(simulated.Id, simulated.Id));
				}
				return false;
			}
		}

		public class TaskMovingState : TaskUpdateState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				Command command = simulated.command;
				base.Enter(simulation, simulated);
				simulated.command = command;
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				entity.m_bReachedTaskTarget = false;
				if (!entity.m_pTask.m_bMovingToTarget)
				{
					entity.m_pTask.m_bMovingToTarget = true;
					entity.m_pTask.m_ulMovingTimeStart = TFUtils.EpochTime();
				}
				float fMovementSpeed = entity.m_pTask.m_pTaskData.m_fMovementSpeed;
				TFUtils.ErrorLog("simulated's current speed: " + fMovementSpeed);
				if (fMovementSpeed > 0f)
				{
					simulated.Variable["speed"] = fMovementSpeed;
				}
				entity.m_pTaskTargetPosition = (Vector2)simulated.command["position"];
				if (simulated.Position == entity.m_pTaskTargetPosition)
				{
					simulated.command = null;
					ReachedTarget(simulation, simulated);
					return;
				}
				simulated.Variable["pathing"] = simulation.CreatePathing(simulated.Position, entity.m_pTaskTargetPosition);
				simulated.ClearPathInfo();
				simulated.DisplayState(entity.m_pTask.m_pTaskData.m_sSourceDisplayStateWalk);
				simulated.command = null;
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (entity.m_bReachedTaskTarget)
				{
					return false;
				}
				base.Simulate(simulation, simulated, session);
				Simulated simulated2 = simulation.FindSimulated(entity.m_pTask.m_pTargetIdentity);
				if (simulated2 != null && !(session.TheState is Session.MoveBuilding) && entity.m_pTaskTargetPosition != simulated2.PointOfInterest + entity.m_pTask.m_pTaskData.m_pPosOffsetFromTarget)
				{
					entity.m_pTaskTargetPosition = simulated2.PointOfInterest + entity.m_pTask.m_pTaskData.m_pPosOffsetFromTarget;
					simulated.Variable["pathing"] = simulation.CreatePathing(simulated.Position, entity.m_pTaskTargetPosition);
					simulated.ClearPathInfo();
				}
				if (FollowPathSimulate(simulation, simulated))
				{
					ReachedTarget(simulation, simulated);
				}
				return false;
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				base.Leave(simulation, simulated);
				simulated.Warp(simulated.Position);
				simulated.ClearPathInfo();
			}

			private void ReachedTarget(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				entity.m_bReachedTaskTarget = true;
				simulated.Position = entity.m_pTaskTargetPosition;
				simulated.Warp(simulated.Position);
				TaskData._eTaskType eTaskType = entity.m_pTask.m_pTaskData.m_eTaskType;
				if (eTaskType == TaskData._eTaskType.eEnter)
				{
					simulated.DisplayThoughtState(null, simulation);
					simulated.Visible = false;
					simulation.Router.Send(EnterCommand.Create(simulated.Id, simulated.Id));
				}
				else
				{
					simulation.Router.Send(StandCommand.Create(simulated.Id, simulated.Id));
				}
			}
		}

		public class TaskEnterState : TaskUpdateState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				simulated.InteractionState.Clear();
				simulated.Visible = false;
				simulated.Variable["pathing"] = null;
				simulated.DisplayThoughtState(null, simulation);
				simulated.ClearPathInfo();
				simulated.Position = new Vector3(9999999f, 9999999f, 9999999f);
				simulated.Warp(simulated.Position);
				entity.m_pTask.m_bAtTarget = true;
				if (entity.m_pTask.m_bMovingToTarget)
				{
					entity.m_pTask.m_bMovingToTarget = false;
					ulong num = TFUtils.EpochTime() - entity.m_pTask.m_ulMovingTimeStart;
					entity.m_pTask.m_ulMovingTimeStart = 0uL;
					entity.m_pTask.m_ulCompleteTime += num;
					entity.m_pTask.m_ulStartTime += num;
					simulation.ModifyGameState(new TaskUpdateAction(entity.m_pTask));
				}
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				base.Simulate(simulation, simulated, session);
				return false;
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				base.Leave(simulation, simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				Simulated simulated2 = simulation.FindSimulated(entity.m_pTask.m_pTargetIdentity);
				if (simulated2 == null)
				{
					simulated2 = simulation.FindSimulated(entity.Residence);
				}
				simulated.Position = simulated2.PointOfInterest + entity.m_pTask.m_pTaskData.m_pPosOffsetFromTarget;
				simulated.Warp(simulated.Position);
			}

			protected override void ShowNewHungerResource(Simulation simulation, Simulated simulated, int nDID)
			{
			}
		}

		public class TaskEnterFeedState : TaskUpdateState
		{
			private const string REQUEST_ERROR_PULSE = "RequestOpenInventory";

			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				simulated.InteractionState.Clear();
				simulated.Visible = false;
				simulated.Variable["pathing"] = null;
				simulated.DisplayThoughtState(null, simulation);
				simulated.ClearPathInfo();
				simulated.Position = new Vector3(9999999f, 9999999f, 9999999f);
				simulated.Warp(simulated.Position);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				entity.m_pTask.m_bAtTarget = true;
				if (entity.m_pTask.m_bMovingToTarget)
				{
					entity.m_pTask.m_bMovingToTarget = false;
					ulong num = TFUtils.EpochTime() - entity.m_pTask.m_ulMovingTimeStart;
					entity.m_pTask.m_ulMovingTimeStart = 0uL;
					entity.m_pTask.m_ulCompleteTime += num;
					entity.m_pTask.m_ulStartTime += num;
					simulation.ModifyGameState(new TaskUpdateAction(entity.m_pTask));
				}
				if (!entity.HungerResourceId.HasValue)
				{
					return;
				}
				int value = entity.HungerResourceId.Value;
				if (simulation.resourceManager.HasEnough(value, 1))
				{
					GenerateAndRecordBonusEarned((uint)value, simulation, simulated);
					if (simulation.resourceManager.Resources[value].EatenSound != null)
					{
						simulation.soundEffectManager.PlaySound(simulation.resourceManager.Resources[value].EatenSound);
					}
					else
					{
						simulation.soundEffectManager.PlaySound("FeedUnit");
					}
					entity.WishExpiresAt = null;
					entity.FullnessLength = (ulong)simulation.resourceManager.Resources[value].FullnessTime;
					entity.FullnessRushCostFull = ResourceManager.CalculateFullnessRushCost(entity.FullnessLength);
					SpawnAndRecordRewards(simulation, simulated);
					int value2 = entity.HungerResourceId.Value;
					entity.PreviousResourceId = value2;
					entity.HungerResourceId = null;
					entity.WishExpiresAt = null;
					entity.HungryAt = TFUtils.EpochTime() + entity.FullnessLength;
					Simulated simulated2 = simulation.FindSimulated(entity.m_pTask.m_pTargetIdentity);
					simulation.Router.Send(BonusRewardCommand.Create(simulated.Id, simulated2.Id, simulated.entity.DefinitionId));
				}
				else
				{
					TFUtils.DebugLog("Simulated(" + simulated.Id.Describe() + ") could not eat!");
					simulation.soundEffectManager.PlaySound("DontHaveAny");
					simulated.Variable["RequestOpenInventory"] = true;
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
				}
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				base.Simulate(simulation, simulated, session);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (entity.MatchBonus == null)
				{
					simulation.Router.Send(TaskCommand.Create(simulated.Id, simulated.Id));
				}
				return false;
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				base.Leave(simulation, simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				simulated.Position = simulation.FindSimulated(entity.m_pTask.m_pTargetIdentity).PointOfInterest + entity.m_pTask.m_pTaskData.m_pPosOffsetFromTarget;
				simulated.Warp(simulated.Position);
			}

			protected override void ShowNewHungerResource(Simulation simulation, Simulated simulated, int nDID)
			{
			}

			private void GenerateAndRecordBonusEarned(uint fedProductId, Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				Reward reward = null;
				if (entity.ForcedBonusReward != null)
				{
					reward = entity.ForcedBonusReward.GenerateReward(simulation, true);
					entity.ForcedBonusReward = null;
				}
				else
				{
					int num = entity.BonusPaytables.Length;
					for (int i = 0; i < num; i++)
					{
						reward = ((i != 0) ? (entity.BonusPaytables[i].Spin(fedProductId, simulation, Paytable.CONSOLATION_REWARD) + reward) : entity.BonusPaytables[i].Spin(fedProductId, simulation, Paytable.CONSOLATION_REWARD));
					}
					if (!simulation.featureManager.CheckFeature("recipe_drops") && reward.RecipeUnlocks != null && reward.RecipeUnlocks.Count > 0)
					{
						Dictionary<int, int> dictionary = new Dictionary<int, int>();
						dictionary.Add(ResourceManager.SOFT_CURRENCY, 5);
						reward = new Reward(dictionary, null, null, null, null, null, null, null, false, simulation.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].GetResourceTexture());
					}
					if (reward.RecipeUnlocks != null && reward.RecipeUnlocks.Count > 0)
					{
						foreach (int recipeUnlock in reward.RecipeUnlocks)
						{
							simulation.craftManager.ReserveRecipe(recipeUnlock);
						}
					}
				}
				TFUtils.Assert(reward != null, "Got a null match bonus! Need to adjust the bonus paytables so they always get something!");
				entity.MatchBonus = reward;
				if (reward != null)
				{
					simulation.ModifyGameStateSimulated(simulated, new EarnMatchBonusAction(entity.Id, reward));
				}
			}

			private void SpawnAndRecordRewards(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				TFUtils.Assert(entity.HungerResourceId.HasValue, "Trying to spawn rewards but resident has not HungerResourceID. Did you clear it too early?");
				int value = entity.HungerResourceId.Value;
				simulation.resourceManager.Spend(value, 1, simulation.game);
				if (simulation.resourceManager.Resources[value].ForceNoWishPayout)
				{
					simulation.game.analytics.LogCharacterFeed(entity.DefinitionId, value);
					AnalyticsWrapper.LogCharacterFeed(simulation.game, entity, value, null);
					FeedUnitAction feedUnitAction = new FeedUnitAction(simulated, (ulong)simulation.resourceManager.Resources[value].FullnessTime, value, entity.PreviousResourceId, null);
					feedUnitAction.AddDropData(simulated, null);
					simulation.ModifyGameStateSimulated(simulated, feedUnitAction);
					return;
				}
				Simulated simulated2 = simulation.FindSimulated(entity.m_pTask.m_pTargetIdentity);
				Reward reward = simulation.resourceManager.Resources[value].Reward.GenerateReward(simulation, false);
				ulong utcNow = TFUtils.EpochTime();
				simulated2.DisplayThoughtState("bonus_ready", simulation);
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, simulated2, utcNow);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Resident.EatingState.SpawnAndRecordRewards - dropResults is null");
					return;
				}
				simulated2.DisplayThoughtState(null, simulation);
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = ((count <= 0) ? null : rewardDropResults.dropIdentities[count - 1]);
				simulation.game.analytics.LogCharacterFeed(entity.DefinitionId, value);
				AnalyticsWrapper.LogCharacterFeed(simulation.game, entity, value, reward);
				FeedUnitAction feedUnitAction2 = new FeedUnitAction(simulated, (ulong)simulation.resourceManager.Resources[value].FullnessTime, value, entity.PreviousResourceId, reward);
				feedUnitAction2.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, feedUnitAction2);
			}
		}

		public class TaskStandState : TaskUpdateState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				base.Enter(simulation, simulated);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				simulated.Variable["pathing"] = null;
				simulated.ClearPathInfo();
				simulated.DisplayState(entity.m_pTask.m_pTaskData.m_sSourceDisplayStateIdle);
				if (!entity.m_pTask.m_bAtTarget)
				{
					entity.m_pTask.m_bAtTarget = true;
					if (entity.m_pTask.m_pTaskData.m_eTaskType == TaskData._eTaskType.eActivate)
					{
						Simulated simulated2 = simulation.FindSimulated(entity.m_pTask.m_pTargetIdentity);
						if (simulated2 != null)
						{
							entity.m_pTask.m_sTargetPrevDisplayState = simulated2.GetDisplayState();
							simulated2.DisplayState(entity.m_pTask.m_pTaskData.m_sTargetDisplayState);
						}
					}
				}
				simulated.Position = simulation.FindSimulated(entity.m_pTask.m_pTargetIdentity).PointOfInterest + entity.m_pTask.m_pTaskData.m_pPosOffsetFromTarget;
				simulated.Warp(simulated.Position);
				if (entity.m_pTask.m_bMovingToTarget)
				{
					entity.m_pTask.m_bMovingToTarget = false;
					ulong num = TFUtils.EpochTime() - entity.m_pTask.m_ulMovingTimeStart;
					entity.m_pTask.m_ulMovingTimeStart = 0uL;
					entity.m_pTask.m_ulCompleteTime += num;
					entity.m_pTask.m_ulStartTime += num;
					simulation.ModifyGameState(new TaskUpdateAction(entity.m_pTask));
				}
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				base.Simulate(simulation, simulated, session);
				return false;
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				base.Leave(simulation, simulated);
			}
		}

		public class TaskCollectRewardState : FollowingPath, StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.Visible = true;
				int count = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id).Count;
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (count == 0)
				{
					return;
				}
				Task task = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id)[0];
				int nDID = task.m_pTaskData.m_nDID;
				Simulated simulated2 = null;
				if (task.m_bAtTarget)
				{
					TFUtils.ErrorLog("pTask.m_bAtTarget - line 2867 Simulated.Resident.cs");
					task.m_bAtTarget = false;
					if (entity.m_pTask.m_pTaskData.m_eTaskType == TaskData._eTaskType.eActivate && !string.IsNullOrEmpty(entity.m_pTask.m_sTargetPrevDisplayState))
					{
						simulated2 = simulation.FindSimulated(entity.m_pTask.m_pTargetIdentity);
						if (simulated2 != null)
						{
							simulated2.DisplayState(entity.m_pTask.m_sTargetPrevDisplayState);
						}
					}
				}
				if (task.m_bMovingToTarget)
				{
					TFUtils.ErrorLog("pTask.m_bMovingToTarget - line 2881 Simulated.Resident.cs");
					task.m_bMovingToTarget = false;
					ulong num = TFUtils.EpochTime() - task.m_ulMovingTimeStart;
					task.m_ulMovingTimeStart = 0uL;
					task.m_ulCompleteTime += num;
					task.m_ulStartTime += num;
					simulation.ModifyGameState(new TaskUpdateAction(task));
				}
				if (simulated2 == null && task.m_pTargetIdentity != null)
				{
					simulated2 = simulation.FindSimulated(task.m_pTargetIdentity);
					if (simulated2 != null)
					{
						ulong ulCompleteTime = task.m_ulCompleteTime;
						task.m_ulCompleteTime = TFUtils.EpochTime();
						simulated2.UpdateControls(simulation);
						task.m_ulCompleteTime = ulCompleteTime;
						TFUtils.ErrorLog("pTargetSim != null - line 2894 Simulated.Resident.cs");
					}
				}
				simulated.Variable["speed"] = simulated.Invariable["base_speed"];
				simulated.DisplayState("walk");
				TFUtils.ErrorLog("simulated speed: " + simulated.Variable["speed"]);
				if (task.m_pTaskData.tasksHasBonus.Contains(nDID))
				{
					if (simulation.game.paytableManager.paytableTaskCheck.Contains(nDID))
					{
						simulated.DisplayThoughtState("TreasureChest_Closed.png", "task_collect", simulation);
					}
				}
				else
				{
					simulated.DisplayThoughtState(task.m_pTaskData.m_pReward.ThoughtIcon, "task_collect", simulation);
				}
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
				simulation.soundEffectManager.PlaySound("MatchBonus_Ready");
				simulated.InteractionState.SetInteractions(false, false, true, true);
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
				Task task = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id)[0];
				int nDID = task.m_pTaskData.m_nDID;
				simulation.soundEffectManager.PlaySound("MatchBonus_Open");
				if (task.m_pTaskData.tasksHasBonus.Contains(nDID) && simulation.game.paytableManager.paytableTaskCheck.Contains(nDID))
				{
					GenerateAndRecordTaskBonusEarned(nDID, task, simulation, simulated);
				}
				CollectAndRecordRewards(simulation, simulated);
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				RandomWanderSimulate(simulation, simulated);
				return false;
			}

			private void CollectAndRecordRewards(Simulation simulation, Simulated simulated)
			{
				TFUtils.ErrorLog("into CollectAndRecordRewards - line 2969 Simulated.Resident.cs");
				Task task = simulation.game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id)[0];
				int nDID = task.m_pTaskData.m_nDID;
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				Reward matchBonus = entity.MatchBonus;
				entity.MatchBonus = null;
				RewardManager.RewardDropResults rewardDropResults;
				if (simulated.taskBonusReward != null)
				{
					TFUtils.ErrorLog("simulated.taskBonusReward != null  - line 2985 Simulated.Resident.cs");
					matchBonus = simulated.taskBonusReward;
					rewardDropResults = RewardManager.GenerateRewardDrops(matchBonus, simulation, simulated, TFUtils.EpochTime());
					simulated.taskBonusReward = null;
				}
				else
				{
					matchBonus = task.m_pTaskData.m_pReward;
					rewardDropResults = RewardManager.GenerateRewardDrops(matchBonus, simulation, simulated, TFUtils.EpochTime());
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = ((count <= 0) ? null : rewardDropResults.dropIdentities[count - 1]);
				simulation.game.analytics.LogTaskCompleted(nDID, simulation.game.resourceManager.PlayerLevelAmount);
				AnalyticsWrapper.LogTaskCompleted(simulation.game, task);
				simulation.game.taskManager.IncrementTaskCompletionCount(nDID);
				TaskCompleteAction taskCompleteAction = new TaskCompleteAction(simulated.Id, task, matchBonus, simulation.game.taskManager.GetTaskCompletionCount(nDID));
				CollectMatchBonusAction collectMatchBonusAction = new CollectMatchBonusAction(simulated.Id, matchBonus);
				taskCompleteAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, taskCompleteAction);
				taskCompleteAction.AddPickup(simulation);
				collectMatchBonusAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, collectMatchBonusAction);
				collectMatchBonusAction.AddPickup(simulation);
				simulation.soundEffectManager.PlaySound(task.m_pTaskData.m_sFinishVO);
				simulation.soundEffectManager.PlaySound(task.m_pTaskData.m_sFinishSound);
				simulation.game.taskManager.RemoveActiveTask(task.m_pTaskData.m_nDID);
				simulation.UpdateControls();
			}

			private void GenerateAndRecordTaskBonusEarned(int taskDID, Task pTask, Simulation simulation, Simulated simulated)
			{
				TFUtils.ErrorLog("made it into GenerateAndRecordTaskBonusEarned - line 3035 Simulated.Resident.cs");
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				Reward reward = null;
				if (entity.ForcedBonusReward == null)
				{
					TFUtils.ErrorLog("entity.ForcedBonusReward = null - line 3049 Simulated.Resident.cs");
					int num = entity.BonusPaytables.Length;
					int num2 = 0;
					for (int i = 0; i < num; i++)
					{
						if (i == 0)
						{
							reward = entity.BonusPaytables[i].Spin(taskDID, simulation, Paytable.CONSOLATION_REWARD) + pTask.m_pTaskData.m_pReward;
						}
						else if (i != 0 && num2 >= 1)
						{
						}
						num2++;
					}
					if (!simulation.featureManager.CheckFeature("recipe_drops"))
					{
					}
					if (reward.RecipeUnlocks != null && reward.RecipeUnlocks.Count > 0)
					{
						TFUtils.ErrorLog("bonus.RecipeUnlocks != null - line 3099 Simulated.Resident.cs");
						foreach (int recipeUnlock in reward.RecipeUnlocks)
						{
							simulation.craftManager.ReserveRecipe(recipeUnlock);
						}
					}
					else if (reward.BuildingUnlocks != null && reward.BuildingUnlocks.Count <= 0)
					{
					}
				}
				TFUtils.Assert(reward != null, "Got a null match bonus! Need to adjust the bonus paytables so they always get something!");
				if (reward != null)
				{
					simulation.ModifyGameStateSimulated(simulated, new EarnMatchBonusAction(entity.Id, reward));
					simulated.taskBonusReward = reward;
				}
			}
		}

		public class TaskCheerAfterCollectState : CheeringState
		{
			protected override string DisplayThoughtStateName
			{
				get
				{
					return null;
				}
			}
		}

		public static IdleState Idle = new IdleState();

		public static IdleFullState IdleFull = new IdleFullState();

		public static IdleWishingState IdleWishing = new IdleWishingState();

		public static MovingState Moving = new MovingState();

		public static GoHomeState GoHome = new GoHomeState();

		public static StoreResidentState StoreResident = new StoreResidentState();

		public static ResidingState Residing = new ResidingState();

		public static WanderingFullState WanderingFull = new WanderingFullState();

		public static WanderingHungryState WanderingHungry = new WanderingHungryState();

		public static WishingForFoodState WishingForFood = new WishingForFoodState();

		public static TemptedState Tempted = new TemptedState();

		public static NotTemptedState NotTempted = new NotTemptedState();

		public static PrimingRushFullnessState PrimingRushFullness = new PrimingRushFullnessState();

		public static RushingFullnessState RushingFullness = new RushingFullnessState();

		public static TryEatState TryEat = new TryEatState();

		public static WaitingForDeliveryState WaitingForDelivery = new WaitingForDeliveryState();

		public static CheeringState Cheering = new CheeringState();

		public static EatingState Eating = new EatingState();

		public static TryBonusSpinState TryBonusSpin = new TryBonusSpinState();

		public static WaitingForCollectBonusState WaitingForCollectBonus = new WaitingForCollectBonusState();

		public static CheeringAfterBonusState CheeringAfterBonus = new CheeringAfterBonusState();

		public static StartingWanderCycleState StartingWanderCycle = new StartingWanderCycleState();

		public static RequestingInterfaceState RequestingInterface = new RequestingInterfaceState();

		public static ReflectingState Reflecting = new ReflectingState();

		public static TaskDelegatingState TaskDelegating = new TaskDelegatingState();

		public static TaskIdleState TaskIdle = new TaskIdleState();

		public static TaskWanderState TaskWander = new TaskWanderState();

		public static TaskMovingState TaskMoving = new TaskMovingState();

		public static TaskEnterState TaskEnter = new TaskEnterState();

		public static TaskEnterFeedState TaskEnterFeed = new TaskEnterFeedState();

		public static TaskStandState TaskStand = new TaskStandState();

		public static TaskCollectRewardState TaskCollectReward = new TaskCollectRewardState();

		public static TaskCheerAfterCollectState TaskCheerAfterCollect = new TaskCheerAfterCollectState();

		public static Simulated Load(ResidentEntity residentEntity, Identity residenceId, ulong? wishExpiresAt, int? hungerId, int? prevHungerId, ulong nextHungerTime, ulong? fullnessLength, Reward matchBonus, Simulation simulation, ulong utcNow)
		{
			residentEntity.Residence = residenceId;
			residentEntity.HungryAt = nextHungerTime;
			if (fullnessLength.HasValue)
			{
				residentEntity.FullnessLength = fullnessLength.Value;
			}
			else
			{
				residentEntity.FullnessLength = nextHungerTime - TFUtils.EpochTime();
			}
			residentEntity.FullnessRushCostFull = ResourceManager.CalculateFullnessRushCost(residentEntity.FullnessLength);
			residentEntity.HungerResourceId = hungerId;
			residentEntity.PreviousResourceId = prevHungerId;
			residentEntity.WishExpiresAt = wishExpiresAt;
			residentEntity.MatchBonus = matchBonus;
			if (!residentEntity.CostumeDID.HasValue && residentEntity.DefaultCostumeDID.HasValue)
			{
				residentEntity.CostumeDID = residentEntity.DefaultCostumeDID;
			}
			StateAction stateAction = EntityManager.ResidentActions["idle"];
			List<uint> bonusPaytableIds = residentEntity.BonusPaytableIds;
			PaytableManager paytableManager = simulation.game.paytableManager;
			Paytable[] array = null;
			if (residentEntity.JoinPaytables.Value)
			{
				Paytable paytable = null;
				foreach (uint item in bonusPaytableIds)
				{
					paytable = paytableManager.Get(item).Join(paytable);
				}
				paytable.Normalize();
				array = new Paytable[1] { paytable };
			}
			else
			{
				int count = bonusPaytableIds.Count;
				array = new Paytable[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = paytableManager.Get(bonusPaytableIds[i]);
				}
			}
			residentEntity.BonusPaytables = array;
			TFUtils.DebugLog(string.Concat("Loading resident(name=", (string)residentEntity.Invariable["name"], ", id=", residentEntity.Id, ", did=", residentEntity.DefinitionId, ", state=", stateAction.ToString(), ", nextHungerTime=", nextHungerTime, "(", nextHungerTime - TFUtils.EpochTime(), " seconds from now), hungerResourceId=", hungerId, ", wishExpiresAt=", wishExpiresAt, "(", (!wishExpiresAt.HasValue) ? ((ulong?)null) : new ulong?(wishExpiresAt.Value - TFUtils.EpochTime()), " seconds from now), residenceId=", residenceId, ", loadedPaytableCount=", bonusPaytableIds.Count));
			Simulated simulated = simulation.FindSimulated(residenceId);
			if (simulated == null)
			{
				if (!SoaringInternal.IsProductionMode)
				{
					TFUtils.Assert(simulated != null, "Don't know where to place the resident since there is no residence that owns it.");
				}
				return null;
			}
			Simulated simulated2 = simulation.CreateSimulated(residentEntity, stateAction, simulated.PointOfInterest);
			simulated2.Warp(simulated.PointOfInterest, simulation);
			simulated2.Visible = true;
			simulated2.IsSwarmManaged = true;
			SwarmManager.Instance.AddResident(residentEntity);
			List<Task> activeTasksForSimulated = simulation.game.taskManager.GetActiveTasksForSimulated(simulated2.entity.DefinitionId, simulated2.Id);
			if (activeTasksForSimulated.Count > 0)
			{
				if (activeTasksForSimulated[0].m_pTaskData.m_eTaskType == TaskData._eTaskType.eEnter)
				{
					if (residentEntity.MatchBonus != null)
					{
						simulated2.EnterInitialState(EntityManager.ResidentActions["task_enter_feed"], simulation);
					}
					else
					{
						simulated2.EnterInitialState(EntityManager.ResidentActions["task_enter"], simulation);
					}
				}
				else if (activeTasksForSimulated[0].m_pTaskData.m_eTaskType == TaskData._eTaskType.eStand || activeTasksForSimulated[0].m_pTaskData.m_eTaskType == TaskData._eTaskType.eActivate)
				{
					simulated2.EnterInitialState(EntityManager.ResidentActions["task_stand"], simulation);
				}
				else
				{
					simulated2.EnterInitialState(EntityManager.ResidentActions["task_delegating"], simulation);
				}
			}
			else if (residentEntity.MatchBonus != null)
			{
				simulated2.EnterInitialState(EntityManager.ResidentActions["wait_bonus"], simulation);
			}
			else
			{
				simulated2.EnterInitialState(EntityManager.ResidentActions["start_wander"], simulation);
			}
			SanityChecks(residentEntity, simulation.game);
			if (residentEntity.CostumeDID.HasValue)
			{
				CostumeManager.Costume costume = simulation.game.costumeManager.GetCostume(residentEntity.CostumeDID.Value);
				simulated2.SetCostume(costume);
			}
			return simulated2;
		}

		private static void SanityChecks(ResidentEntity residentEntity, Game game)
		{
			CdfDictionary<int> wishTable = game.wishTableManager.GetWishTable(residentEntity.WishTableDID);
			WishTableSanityCheck(residentEntity, wishTable, null);
			List<CostumeManager.Costume> costumesForUnit = game.costumeManager.GetCostumesForUnit(residentEntity.DefinitionId);
			int count = costumesForUnit.Count;
			for (int i = 0; i < count; i++)
			{
				CostumeManager.Costume costume = costumesForUnit[i];
				if (costume.m_nWishTableDID >= 0)
				{
					WishTableSanityCheck(residentEntity, game.wishTableManager.GetWishTable(costume.m_nWishTableDID), costume);
				}
			}
			int? defaultCostumeDID = residentEntity.DefaultCostumeDID;
			if (defaultCostumeDID.HasValue && !game.costumeManager.IsCostumeValidForUnit(residentEntity.DefinitionId, defaultCostumeDID.Value))
			{
				TFUtils.Assert(false, string.Format("The resident({0}) has a default costume did of {1} but no costume with that did exists for this resident.", residentEntity.BlueprintName, defaultCostumeDID.Value));
			}
		}

		private static void WishTableSanityCheck(ResidentEntity pResidentEntity, CdfDictionary<int> pWishTable, CostumeManager.Costume pCostume)
		{
			if (pWishTable == null)
			{
				string text = "Resident did: " + pResidentEntity.DefinitionId + " has an undefined wishtable did: " + pResidentEntity.WishTableDID;
				if (pCostume != null)
				{
					text = text + " for costume did: " + pCostume.m_nDID;
				}
				TFUtils.Assert(false, text);
				return;
			}
			if (pCostume == null)
			{
				pWishTable.Validate(true, string.Format("The resident{0})", pResidentEntity.BlueprintName));
			}
			else
			{
				pWishTable.Validate(true, string.Format("The costume did:{0})", pCostume.m_nDID));
			}
			foreach (int item in pWishTable.ValuesClone)
			{
				int num = pResidentEntity.BonusPaytables.Length;
				bool flag = false;
				for (int i = 0; i < num; i++)
				{
					flag = pResidentEntity.BonusPaytables[i].CanWager((uint)item) || flag;
					pResidentEntity.BonusPaytables[i].ValidateProbabilities();
				}
				if (!flag)
				{
					string text = ((pCostume != null) ? string.Format("The resident({0}) using costume({1}) can wish for product({2}) but has not bonus paytable to reward it!", pResidentEntity.BlueprintName, pCostume.m_nDID, item) : string.Format("The resident({0}) can wish for product({1}) but has not bonus paytable to reward it!", pResidentEntity.BlueprintName, item));
					TFUtils.Assert(false, text);
				}
			}
		}

		private static void StartHungerTimer(ResidentEntity resident, Simulation simulation)
		{
			resident.HungryAt = TFUtils.EpochTime() + resident.FullnessLength;
			simulation.Router.Send(HungerCommand.Create(resident.Id, resident.Id), resident.FullnessLength);
		}

		public static void RefreshModifiedDisplayState(Simulated simulated)
		{
			if (simulated.StateModifierString != null)
			{
				string displayState = simulated.GetDisplayState();
				if (displayState != null)
				{
					displayState = displayState.Replace(string.Format(simulated.StateModifierString, string.Empty), string.Empty);
					simulated.StateModifierString = null;
					simulated.DisplayState(displayState);
				}
			}
		}

		private static int? GenerateHungerResourceID(Simulation pSimulation, ResidentEntity pEntity)
		{
			int? result = null;
			if (pEntity == null)
			{
				return null;
			}
			CdfDictionary<int> cdfDictionary = null;
			if (pEntity.CostumeDID.HasValue)
			{
				CostumeManager.Costume costume = pSimulation.game.costumeManager.GetCostume(pEntity.CostumeDID.Value);
				if (costume != null && costume.m_nWishTableDID >= 0)
				{
					cdfDictionary = pSimulation.game.wishTableManager.GetWishTable(costume.m_nWishTableDID);
				}
			}
			if (cdfDictionary == null)
			{
				cdfDictionary = pSimulation.game.wishTableManager.GetWishTable(pEntity.WishTableDID);
			}
			if (cdfDictionary != null)
			{
				if (pEntity.PreviousResourceId == 9100)
				{
					cdfDictionary = cdfDictionary.Where((int productId) => productId != 9100, true);
				}
				if (!pSimulation.featureManager.CheckFeature("resident_wishes_full_pool"))
				{
					ICollection<int> craftableProducts = pSimulation.craftManager.UnlockedProductsDeepCopy;
					cdfDictionary = cdfDictionary.Where((int productId) => craftableProducts.Contains(productId), true);
				}
				result = cdfDictionary.Spin(ResourceManager.DEFAULT_WISH);
			}
			else
			{
				TFUtils.DebugLog("error - entity missing wishpool: " + pEntity.BlueprintName);
			}
			return result;
		}
	}

	public class Treasure
	{
		public class BuriedState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.DisplayState("inactive");
				simulated.InteractionState.SetInteractions(false, false, false, true);
				simulated.simFlags |= SimulatedFlags.FIRST_ANIMATE;
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulation.particleSystemManager.RemoveRequestWithDelegate(simulated.starsParticleSystemRequestDelegate);
				simulated.starsParticleSystemRequestDelegate.isAssigned = false;
				TreasureEntity entity = simulated.GetEntity<TreasureEntity>();
				simulation.ModifyGameStateSimulated(simulated, new TreasureUncoverAction(simulated.Id, TFUtils.EpochTime() + entity.ClearTime));
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (!simulated.starsParticleSystemRequestDelegate.isAssigned)
				{
					simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Sparkles_Rising2", 0, 0, 1f, simulated.starsParticleSystemRequestDelegate);
					simulated.starsParticleSystemRequestDelegate.isAssigned = true;
				}
				return false;
			}
		}

		public class UncoveringState : StateAction, Animated
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				TreasureEntity entity = simulated.GetEntity<TreasureEntity>();
				if (!entity.HasStartedClearing)
				{
					simulation.soundEffectManager.PlaySound("DiggingForTreasure");
					TFUtils.DebugLog("Treasure(" + simulated.Id.Describe() + "):Uncovering. Ready in " + entity.ClearTime);
					entity.ClearCompleteTime = TFUtils.EpochTime() + entity.ClearTime;
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), entity.ClearTime);
				}
				else
				{
					simulation.Router.CancelMatching(Command.TYPE.COMPLETE, simulated.Id, simulated.Id);
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), entity.ClearTimeRemaining);
				}
				simulated.command = null;
				entity.RaisingTimeRemaining = entity.ClearTimeRemaining;
				simulated.EnableAnimateAction(true);
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				TreasureEntity entity = simulated.GetEntity<TreasureEntity>();
				simulated.ClearSimulateOnce();
				entity.RaisingTimeRemaining = 0f;
				BasicSprite basicSprite = (BasicSprite)simulated.DisplayController;
				basicSprite.SetMaskPercentage(0f);
				simulation.particleSystemManager.RemoveRequestWithDelegate(simulated.dustParticleSystemRequestDelegate);
				simulated.dustParticleSystemRequestDelegate.isAssigned = false;
				simulated.ClearSimulateOnce();
				simulated.EnableAnimateAction(false);
				if (entity.Quickclear)
				{
					simulation.Router.Send(ClickedCommand.Create(simulated.Id, simulated.Id));
				}
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public Vector3 Animate(Simulation simulation, Simulated simulated)
			{
				float num = 0f;
				TreasureEntity entity = simulated.GetEntity<TreasureEntity>();
				float input = ((entity.ClearTime == 0) ? 0f : (entity.RaisingTimeRemaining / (float)entity.ClearTime));
				entity.RaisingTimeRemaining -= Time.deltaTime;
				input = TFMath.ClampF(input, 0f, 1f);
				if (!simulated.dustParticleSystemRequestDelegate.isAssigned)
				{
					simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Construction_Smoke", 0, 0, 1f, simulated.dustParticleSystemRequestDelegate);
					simulated.dustParticleSystemRequestDelegate.offset = new Vector3(0f, 0f, 10f);
					simulated.dustParticleSystemRequestDelegate.isAssigned = true;
				}
				num = (0f - input) * simulated.Height / 2f;
				simulated.DisplayState("default");
				simulated.DisplayController.SetMaskPercentage(input);
				return simulated.DisplayController.Up * num;
			}
		}

		public class ClaimingState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("default");
				simulated.command = null;
				simulated.InteractionState.SetInteractions(false, false, false, true, new Session.ShowTreasureRewardTransition(simulated));
				simulated.simFlags |= SimulatedFlags.FIRST_ANIMATE;
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				SpawnDrops(simulation, simulated);
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
				Reward reward = GetReward(simulation, simulated);
				ulong utcNow = TFUtils.EpochTime();
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, new Vector3(simulated.Position.x, simulated.Position.y, 20f), utcNow);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Treasure.ClaimingState.SpawnDrops - dropResults is null");
					return;
				}
				int count = rewardDropResults.dropIdentities.Count;
				Identity dropID = ((count <= 0) ? null : rewardDropResults.dropIdentities[count - 1]);
				TreasureSpawner treasureTiming = simulated.GetEntity<TreasureEntity>().TreasureTiming;
				treasureTiming.MarkCollected();
				simulation.game.analytics.LogChestPickup(simulated.entity.DefinitionId);
				AnalyticsWrapper.LogChestPickup(simulation.game, simulated, reward);
				TreasureCollectAction treasureCollectAction = new TreasureCollectAction(simulated.Id, reward, treasureTiming.PersistName, treasureTiming.TimeToTreasure);
				treasureCollectAction.AddDropData(simulated, dropID);
				simulation.ModifyGameStateSimulated(simulated, treasureCollectAction);
				treasureCollectAction.AddPickup(simulation);
			}

			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				return simulated.GetEntity<TreasureEntity>().ClearingReward.GenerateReward(simulation, false);
			}
		}

		public class DeletingState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.DisplayState(null);
				simulated.DisplayThoughtState(null, simulation);
				simulated.InteractionState.Clear();
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return true;
			}
		}

		public class ClaimingStateFriend : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("default");
				simulated.command = null;
				simulated.InteractionState.SetInteractions(false, false, false, true, new Session.ShowTreasureRewardTransition(simulated));
				simulated.simFlags |= SimulatedFlags.FIRST_ANIMATE;
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				SpawnDrops(simulation, simulated);
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public int CheckHasValue(SoaringDictionary tobeChecked, string key)
			{
				int result = 0;
				SoaringValue soaringValue = tobeChecked.soaringValue(key);
				if (soaringValue != null)
				{
					result = soaringValue;
				}
				return result;
			}

			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
				Reward reward = GetReward(simulation, simulated);
				ulong utcNow = TFUtils.EpochTime();
				RewardManager.RewardDropResults rewardDropResults = RewardManager.GenerateRewardDrops(reward, simulation, new Vector3(simulated.Position.x, simulated.Position.y, 20f), utcNow);
				if (rewardDropResults == null)
				{
					TFUtils.ErrorLog("Treasure.ClaimingStateFriend.SpawnDrops - dropResults is null");
					return;
				}
				SoaringDictionary soaringDictionary = (SoaringDictionary)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_friends_reward_key");
				int num = 0;
				foreach (int key in reward.ResourceAmounts.Keys)
				{
					num = 0;
					if (key == ResourceManager.SOFT_CURRENCY)
					{
						num = CheckHasValue(soaringDictionary, "SBMI_friends_coinreward_key");
						num += reward.ResourceAmounts[key];
						soaringDictionary.setValue(num, "SBMI_friends_coinreward_key");
					}
					else if (key == ResourceManager.HARD_CURRENCY)
					{
						num = CheckHasValue(soaringDictionary, "SBMI_friends_jellyreward_key");
						num += reward.ResourceAmounts[key];
						soaringDictionary.setValue(num, "SBMI_friends_jellyreward_key");
					}
					else if (key == ResourceManager.XP)
					{
						num = CheckHasValue(soaringDictionary, "SBMI_friends_xpreward_key");
						num += reward.ResourceAmounts[key];
						soaringDictionary.setValue(num, "SBMI_friends_xpreward_key");
					}
				}
				num = CheckHasValue(soaringDictionary, "SBMI_friends_chestscollected_key");
				num++;
				soaringDictionary.setValue(num, "SBMI_friends_chestscollected_key");
				num = SBMISoaring.PatchTownTreasureCollected;
				num++;
				SBMISoaring.PatchTownTreasureCollected = num;
				Soaring.UpdateUserProfile(Soaring.Player.CustomData);
				AnalyticsWrapper.LogPatchyChestPickup(simulation.game, simulated, reward);
			}

			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				return simulated.GetEntity<TreasureEntity>().ClearingReward.GenerateReward(simulation, false);
			}
		}

		public class BuriedStateFriend : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.DisplayState("inactive");
				simulated.InteractionState.SetInteractions(false, false, false, true);
				simulated.simFlags |= SimulatedFlags.FIRST_ANIMATE;
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulation.particleSystemManager.RemoveRequestWithDelegate(simulated.starsParticleSystemRequestDelegate);
				simulated.starsParticleSystemRequestDelegate.isAssigned = false;
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (!simulated.starsParticleSystemRequestDelegate.isAssigned)
				{
					simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Sparkles_Rising2", 0, 0, 1f, simulated.starsParticleSystemRequestDelegate);
					simulated.starsParticleSystemRequestDelegate.isAssigned = true;
				}
				return false;
			}
		}

		public static BuriedState Buried = new BuriedState();

		public static UncoveringState Uncovering = new UncoveringState();

		public static ClaimingState Claiming = new ClaimingState();

		public static DeletingState Deleting = new DeletingState();

		public static ClaimingStateFriend Claiming_Friend = new ClaimingStateFriend();

		public static BuriedStateFriend Buried_Friend = new BuriedStateFriend();

		public static Simulated Load(TreasureEntity treasureEntity, Simulation simulation, Vector2 position, ulong utcNow)
		{
			string key;
			if (treasureEntity.ClearCompleteTime.HasValue)
			{
				ulong? clearCompleteTime = treasureEntity.ClearCompleteTime;
				if (clearCompleteTime.HasValue && clearCompleteTime.Value <= utcNow)
				{
					key = "claiming";
					goto IL_0091;
				}
			}
			if (treasureEntity.ClearCompleteTime.HasValue)
			{
				ulong? clearCompleteTime2 = treasureEntity.ClearCompleteTime;
				if (clearCompleteTime2.HasValue && clearCompleteTime2.Value > utcNow)
				{
					key = "uncovering";
					goto IL_0091;
				}
			}
			key = "buried";
			goto IL_0091;
			IL_0091:
			Simulated simulated = simulation.CreateSimulated(treasureEntity, EntityManager.TreasureActions[key], position);
			simulated.Warp(position, simulation);
			simulated.Visible = true;
			simulated.SetFootprint(simulation);
			return simulated;
		}
	}

	public class Wanderer
	{
		public class SpawnState : StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (entity.DisableIfWillFlee.Value && !entity.DisableFlee.Value)
				{
					simulation.Router.Send(AbortCommand.Create(simulated.Id, simulated.Id));
					return;
				}
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.InteractionState.SetInteractions(false, false, false, true);
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				Waypoint randomWaypoint = simulation.GetRandomWaypoint();
				if (randomWaypoint == null)
				{
					return false;
				}
				Vector2 position = randomWaypoint.Position;
				simulated.Warp(position, simulation);
				simulated.Visible = true;
				simulation.Router.Send(WanderCommand.Create(simulated.Id, simulated.Id));
				return false;
			}
		}

		public class HiddenState : StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = false;
				simulated.InteractionState.Clear();
				simulated.Visible = false;
				simulated.Variable["pathing"] = null;
				simulated.DisplayThoughtState(null, simulation);
				simulated.ClearPathInfo();
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (entity.DisableIfWillFlee.Value && !entity.DisableFlee.Value)
				{
					return false;
				}
				if (entity.HideExpiresAt.HasValue)
				{
					ulong? hideExpiresAt = entity.HideExpiresAt;
					if (!hideExpiresAt.HasValue || hideExpiresAt.Value > TFUtils.EpochTime())
					{
						goto IL_0096;
					}
				}
				simulation.Router.Send(SpawnCommand.Create(simulated.Id, simulated.Id, simulated.GetEntity<ResidentEntity>().BlueprintName));
				goto IL_0096;
				IL_0096:
				return false;
			}
		}

		public class IdleState : StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.DisplayState("idle");
				simulated.DisplayThoughtState(null, simulation);
				simulated.InteractionState.SetInteractions(false, false, false, true);
				simulated.GetEntity<ResidentEntity>().StartCheckForResume();
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.GetEntity<ResidentEntity>().StopCheckForResume();
				simulated.InteractionState.Clear();
			}

			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (simulated.GetEntity<ResidentEntity>().CheckForResume())
				{
					simulation.Router.Send(WanderCommand.Create(simulated.Id, simulated.Id));
				}
				return false;
			}
		}

		public class WanderingState : FollowingPath, StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.InteractionState.SetInteractions(false, false, false, true);
				simulated.DisplayState("walk");
				simulated.DisplayThoughtState(null, simulation);
				FollowingPath.GetWaypointPath(simulation, simulated);
				simulated.GetEntity<ResidentEntity>().StartCheckForIdle();
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.Variable["pathing"] = null;
				simulated.ClearPathInfo();
				simulated.Warp(simulated.Position);
				simulated.GetEntity<ResidentEntity>().StopCheckForIdle();
			}

			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				RandomWanderSimulate(simulation, simulated);
				if (simulated.GetEntity<ResidentEntity>().CheckForIdle())
				{
					simulation.Router.Send(IdlePauseCommand.Create(simulated.Id, simulated.Id));
				}
				return false;
			}
		}

		public class ClickedState : StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.InteractionState.Clear();
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				if (entity.DisableFlee.HasValue && entity.DisableFlee.Value)
				{
					simulation.Router.Send(CheerCommand.Create(simulated.Id, simulated.Id));
				}
				else
				{
					simulation.Router.Send(FleeCommand.Create(simulated.Id, simulated.Id));
				}
				simulation.ModifyGameStateSimulated(simulated, new TapWandererAction(simulated.Id, entity.DefinitionId));
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class FleeingState : FollowingPath, StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.SimulatedQueryable = true;
				simulated.InteractionState.Clear();
				simulated.Variable["pathing"] = simulation.CreatePathing(simulated.Position, simulation.GetRandomWaypoint().Position);
				simulated.ClearPathInfo();
				simulated.DisplayState("walk");
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (FollowPathSimulate(simulation, simulated))
				{
					ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
					entity.HideExpiresAt = TFUtils.EpochTime() + (ulong)entity.HideDuration;
					simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
					simulation.ModifyGameStateSimulated(simulated, new HideWandererAction(simulated.Id, entity.DefinitionId, entity.HideExpiresAt.Value));
				}
				return false;
			}

			protected override float GetSpeedAddition(Simulated simulated)
			{
				return (float)simulated.Variable["speed"] * 6f;
			}
		}

		public abstract class TransitionallyAnimating : StateActionDefault
		{
			protected abstract string DisplayStateName { get; }

			protected abstract string DisplayThoughtStateName { get; }

			protected abstract int AnimationLength { get; }

			public override void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.DisplayState(DisplayStateName);
				string displayThoughtMaterial = GetDisplayThoughtMaterial(simulation, simulated);
				if (displayThoughtMaterial != null)
				{
					simulated.DisplayThoughtState(displayThoughtMaterial, DisplayThoughtStateName, simulation);
				}
				else
				{
					simulated.DisplayThoughtState(DisplayThoughtStateName, simulation);
				}
				simulated.InteractionState.Clear();
				simulated.GetEntity<ResidentEntity>().HomeAvailability = false;
				simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id), (ulong)AnimationLength);
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("default");
				simulated.DisplayThoughtState(null, simulation);
			}

			protected abstract string GetDisplayThoughtMaterial(Simulation simulation, Simulated simulated);
		}

		public class CheeringState : TransitionallyAnimating
		{
			protected override string DisplayStateName
			{
				get
				{
					return "cheer";
				}
			}

			protected override string DisplayThoughtStateName
			{
				get
				{
					return null;
				}
			}

			protected override int AnimationLength
			{
				get
				{
					return 2;
				}
			}

			protected override string GetDisplayThoughtMaterial(Simulation simulation, Simulated simulated)
			{
				return null;
			}
		}

		public static SpawnState Spawn = new SpawnState();

		public static HiddenState Hidden = new HiddenState();

		public static IdleState Idle = new IdleState();

		public static WanderingState Wandering = new WanderingState();

		public static ClickedState Clicked = new ClickedState();

		public static FleeingState Fleeing = new FleeingState();

		public static CheeringState Cheering = new CheeringState();

		public static Simulated Load(ResidentEntity residentEntity, ulong? hideExpiresAt, bool? disableFlee, Simulation simulation, ulong utcNow)
		{
			StateAction stateAction = EntityManager.WandererActions["hidden"];
			TFUtils.DebugLog(string.Concat("Loading wanderer(name=", (string)residentEntity.Invariable["name"], ", id=", residentEntity.Id, ", did=", residentEntity.DefinitionId, ", state=", stateAction.ToString()));
			Vector2 position = simulation.GetRandomWaypoint().Position;
			Simulated simulated = simulation.CreateSimulated(residentEntity, stateAction, position);
			simulated.Visible = false;
			simulated.IsSwarmManaged = false;
			residentEntity.Wanderer = true;
			residentEntity.HideExpiresAt = hideExpiresAt;
			residentEntity.DisableFlee = disableFlee;
			return simulated;
		}

		public static void AddWandererToGameState(Dictionary<string, object> gameState, string wandererId, int wandererDid)
		{
			List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["wanderers"];
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["did"] = wandererDid;
			dictionary["label"] = wandererId;
			list.Add(dictionary);
		}
	}

	public class Worker
	{
		public class IdleState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.command = null;
				simulated.Warp(simulated.Position);
				simulated.DisplayState("idle");
				simulated.DisplayThoughtState(null, simulation);
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class MovingState : FollowingPath, StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulated.Variable["pathing"] = simulation.CreatePathing(simulated.Position, (Vector2)simulated.command["position"]);
				simulated.ClearPathInfo();
				simulated.command = null;
				simulated.DisplayState("walk");
				simulated.DisplayThoughtState(null, simulation);
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (FollowPathSimulate(simulation, simulated))
				{
					simulation.Router.Send(ErectCommand.Create(simulated.Id, simulated.Id, Identity.Null(), 0uL));
				}
				return false;
			}
		}

		public class ReturningState : FollowingPath, StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				Simulated closestWorkerSpawner = simulation.GetClosestWorkerSpawner(simulated.Position);
				Vector2 goal = simulated.Position;
				if (closestWorkerSpawner != null)
				{
					goal = closestWorkerSpawner.PointOfInterest;
				}
				simulated.Variable["pathing"] = simulation.CreatePathing(simulated.Position, goal);
				simulated.ClearPathInfo();
				simulated.command = null;
				simulated.DisplayState("walk");
				simulated.DisplayThoughtState(null, simulation);
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				if (FollowPathSimulate(simulation, simulated))
				{
					return true;
				}
				return false;
			}
		}

		public class ErectingState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
				simulation.soundEffectManager.PlaySound("Construction");
				simulated.DisplayState("work");
				simulated.DisplayThoughtState(null, simulation);
				TFUtils.DebugLog("Worker(" + simulated.Id.Describe() + "):Erecting(" + (simulated.command["building"] as Identity).Describe() + ")");
				simulated.command = null;
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
				simulated.DisplayState("default");
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public static IdleState Idle = new IdleState();

		public static MovingState Moving = new MovingState();

		public static ReturningState Returning = new ReturningState();

		public static ErectingState Erecting = new ErectingState();
	}

	public interface StateAction
	{
		void Enter(Simulation simulation, Simulated simulated);

		void Leave(Simulation simulation, Simulated simulated);

		bool Simulate(Simulation simulation, Simulated simulated, Session session);
	}

	public abstract class StateActionDefault : StateAction
	{
		public abstract void Enter(Simulation simulation, Simulated simulated);

		public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
		{
			return false;
		}

		public virtual void Leave(Simulation simulation, Simulated simulated)
		{
		}
	}

	public abstract class StateActionBuildingDefault : StateActionDefault
	{
		public virtual void UpdateControls(Simulation simulation, Simulated simulated)
		{
		}
	}

	public abstract class RushingSomething : StateActionDefault
	{
		public virtual void CancelCurrentCommands(Simulation simulation, Simulated simulated)
		{
			int num = simulation.Router.CancelMatching(Command.TYPE.COMPLETE, simulated.Id, simulated.Id);
		}

		public override void Enter(Simulation simulation, Simulated simulated)
		{
			simulated.command = null;
			simulated.InteractionState.Clear();
			CancelCurrentCommands(simulation, simulated);
			simulation.Router.Send(CompleteCommand.Create(simulated.Id, simulated.Id));
			Cost cost = new Cost();
			cost += GetRushCost(simulation, simulated);
			cost.Prorate((float)simulated.Variable[RUSH_PERCENT]);
			ResourceManager resourceManager = simulation.resourceManager;
			if (resourceManager.CanPay(cost))
			{
				resourceManager.Apply(new Cost(cost), simulation.game);
			}
			else
			{
				TFUtils.Assert(false, "You don't have enough money! Consider showing an insufficient funds dialog before getting here!");
			}
			simulated.rushParameters = null;
		}

		protected abstract Cost GetRushCost(Simulation simulation, Simulated simulated);
	}

	public class RushParameters
	{
		public Cost.CostAtTime cost;

		public string subject;

		public int did;

		public Action<Session> execute;

		public Action<Session> cancel;

		public Action<Session, Cost, bool> log;

		public Vector2 screenPosition;

		public RushParameters(Action<Session> execute, Action<Session> cancel, Cost.CostAtTime cost, string subject, int did, Action<Session, Cost, bool> log, Vector2 screenPosition)
		{
			this.execute = execute;
			this.cost = cost;
			this.cancel = cancel;
			this.subject = subject;
			this.log = log;
			this.screenPosition = screenPosition;
			this.did = did;
		}
	}

	public interface Animated
	{
		Vector3 Animate(Simulation simulation, Simulated simulated);
	}

	[Flags]
	public enum SimulatedFlags
	{
		MOBILE = 1,
		BUILDING_ANIM_PATH = 2,
		FIRST_ANIMATE = 4,
		FORCE_ANIMATE_ACTION = 8,
		FORCE_ANIMATE_FOOTPRINT = 0x10,
		FORCE_ANIMATE_BOUNCE = 0x20,
		FORCE_ANIMATE_BOUNCE_START = 0x40,
		FORCE_ANIMATE_BOUNCE_END = 0x80
	}

	public struct PendingCommand
	{
		public Command c;

		public float? delay;
	}

	public class ParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		protected Simulated simulated;

		public virtual Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		public virtual Vector3 Position
		{
			get
			{
				if (simulated.particleDisplayOffsetWorld.HasValue)
				{
					return simulated.displayController.Position + simulated.particleDisplayOffsetWorld.Value;
				}
				return simulated.thoughtDisplayController.Position;
			}
		}

		public virtual bool isVisible
		{
			get
			{
				return simulated.displayController.isVisible;
			}
		}

		public ParticleSystemRequestDelegate(Simulated simulated)
		{
			this.simulated = simulated;
		}
	}

	public class RewardParticleRequestDelegate : ParticleSystemRequestDelegate
	{
		public override Vector3 Position
		{
			get
			{
				return simulated.thoughtItemBubbleDisplayController.Position;
			}
		}

		public RewardParticleRequestDelegate(Simulated theSimulated)
			: base(theSimulated)
		{
		}
	}

	public class ThoughtBubblePopParticleRequestDelegate : RewardParticleRequestDelegate
	{
		public override bool isVisible
		{
			get
			{
				return true;
			}
		}

		public ThoughtBubblePopParticleRequestDelegate(Simulated theSimulated)
			: base(theSimulated)
		{
		}
	}

	public class EatParticleRequestDelegate : RewardParticleRequestDelegate
	{
		public override Vector3 Position
		{
			get
			{
				Paperdoll paperdoll = (Paperdoll)simulated.displayController;
				Transform transform = paperdoll.GetBone("BN_MOUTH_OPENMOUTH");
				if (transform == null)
				{
					transform = paperdoll.GetBone("BN_HEAD");
				}
				if (transform == null)
				{
					transform = paperdoll.GetBone("BN_HIP");
				}
				if (transform == null)
				{
					transform = paperdoll.GetBone("BN_ROOT");
				}
				if (transform == null)
				{
					transform = paperdoll.Transform;
				}
				return transform.position;
			}
		}

		public override bool isVisible
		{
			get
			{
				return true;
			}
		}

		public EatParticleRequestDelegate(Simulated simulated)
			: base(simulated)
		{
		}
	}

	public class ActivateParticleRequestDelegate : ParticleSystemRequestDelegate
	{
		public override Vector3 Position
		{
			get
			{
				return simulated.displayController.Position;
			}
		}

		public ActivateParticleRequestDelegate(Simulated theSimulated)
			: base(theSimulated)
		{
		}
	}

	public class SimulatedParticleRequestDelegate : ParticleSystemRequestDelegate
	{
		public bool isAssigned;

		public Vector3 offset = new Vector3(0f, 0f, 20f);

		public override Vector3 Position
		{
			get
			{
				Vector3 vector = new Vector3(simulated.Position.x, simulated.Position.y);
				Vector3 forward = simulated.displayController.Forward;
				vector += forward;
				return vector + offset;
			}
		}

		public SimulatedParticleRequestDelegate(Simulated theSimulated)
			: base(theSimulated)
		{
		}
	}

	public struct TimebarMixinArgs
	{
		public bool hasTimebar;

		public string description;

		public ulong completeTime;

		public ulong totalTime;

		public float duration;

		public Cost rushCost;

		public bool m_bCheckForTaskCharacters;
	}

	public struct NamebarMixinArgs
	{
		public bool m_bHasNamebar;

		public string m_sName;

		public bool m_bCheckForTaskCharacters;
	}

	private const int LOCK_WISH_DELAY = 60;

	public const bool DEBUG_LOG_STATEMACHINES = false;

	public const int TEMPTABLE_THRESHOLD = 0;

	private const string TIMEBAR_RUNNING = "timebar_running";

	public const string REQUEST_RUSH = "request_rush_sim";

	public const string IGNORE_REQUEST_RUSH = "ignore_request_rush_sim";

	private const string DC_EXT_NONE = "";

	private const string DC_EXT_FLIP = ".flip";

	private const string SIMULATE_ONCE = "simulate_once";

	public const string SHOW_TIMEBAR = "show_timebar";

	public const string SHOW_NAMEBAR = "show_namebar";

	public const string ENABLE_PARTICLES = "enable_particles";

	public Reward taskBonusReward;

	public Entity entity;

	public int? forcedWish;

	public bool showUnavailableIcon;

	public RushParameters rushParameters;

	private string mStateModifierString;

	public SimulatedFlags simFlags;

	private List<Action> clickListeners = new List<Action>();

	private static readonly List<StateAction> prioritizedActions = new List<StateAction>
	{
		EntityManager.BuildingActions["crafted"],
		EntityManager.BuildingActions["crafting"],
		EntityManager.BuildingActions["reflecting"],
		EntityManager.DebrisActions["deleting"],
		EntityManager.DebrisActions["clearing"],
		EntityManager.DebrisActions["clearing_more"],
		EntityManager.DebrisActions["priming_rush"],
		EntityManager.DebrisActions["inactive"]
	};

	private static readonly List<StateAction> priorityOrder = new List<StateAction>
	{
		EntityManager.ResidentActions["wait_bonus"],
		EntityManager.ResidentActions["task_collect_reward"],
		EntityManager.ResidentActions["wishing"],
		EntityManager.ResidentActions["task_wander"],
		EntityManager.ResidentActions["task_idle"],
		EntityManager.ResidentActions["task_stand"],
		EntityManager.ResidentActions["wander_full"],
		EntityManager.ResidentActions["idle_full"],
		EntityManager.ResidentActions["task_moving"],
		EntityManager.ResidentActions["tempted"],
		EntityManager.ResidentActions["not_tempted"]
	};

	public static readonly Color COLOR_FOOTPRINT_FREE = new Color(0.01f, 1f, 0.01f, 0.5f);

	public static readonly Color COLOR_FOOTPRINT_BLOCKED = new Color(1f, 0.01f, 0.01f, 0.5f);

	public static readonly Color COLOR_STANDARD = new Color(1f, 1f, 1f, 1f);

	public static readonly Color COLOR_DRAGGING = new Color(1f, 1f, 1f, 0.5f);

	public static readonly string RUSH_PERCENT = "rush_percent";

	public ParticleSystemRequestDelegate particleSystemRequestDelegate;

	public ParticleSystemRequestDelegate rewardParticleSystemRequestDelegate;

	public ThoughtBubblePopParticleRequestDelegate thoughtBubblePopParticleRequestDelegate;

	public EatParticleRequestDelegate eatParticleRequestDelegate;

	public ActivateParticleRequestDelegate activateParticleSystemRequestDelegate;

	public SimulatedParticleRequestDelegate starsParticleSystemRequestDelegate;

	public SimulatedParticleRequestDelegate dustParticleSystemRequestDelegate;

	public TimebarMixinArgs timebarMixinArgs;

	public NamebarMixinArgs m_pNamebarMixinArgs;

	private Vector2[] position = new Vector2[2]
	{
		default(Vector2),
		default(Vector2)
	};

	private Vector2 snapPosition = new Vector2(0f, 0f);

	private Vector2 pointOfInterestOffset = new Vector2(0f, 0f);

	private bool workerSpawner;

	private bool isWaypoint = true;

	private bool simulatedQueryable;

	private AlignedBox footprint;

	private AlignedBox box = new AlignedBox(-1f, 1f, -1f, 1f);

	public AlignedBox snapBox = new AlignedBox(-1f, 1f, -1f, 1f);

	public AlignedBox prevSceneBox = new AlignedBox(0f, 0f, 0f, 0f);

	private Queue<Command> commands = new Queue<Command>();

	private Command command;

	protected TriggerableMixin triggerable = new TriggerableMixin();

	private StateMachine<StateAction, Command.TYPE> machine;

	private StateAction action;

	private Dictionary<StateAction, Queue<Command>> delegatedCommands = new Dictionary<StateAction, Queue<Command>>();

	private bool visible;

	private Vector3 thoughtDisplayOffsetScreen = Vector3.zero;

	private Vector3? thoughtDisplayOffsetWorld;

	private Dictionary<string, Vector3> thoughtDisplayScreenOffsets;

	private Vector3 thoughtMaskDisplayOffsetScreen = Vector3.zero;

	private Vector3? thoughtMaskDisplayOffsetWorld;

	private PeriodicPattern periodicMovement;

	private PeriodicPattern thoughtItemBubbleScalingMajor;

	private PeriodicPattern thoughtItemBubbleScalingMinor;

	private IDisplayController thoughtDisplayController;

	private IDisplayController thoughtMaskDisplayController;

	private IDisplayController thoughtItemBubbleDisplayController;

	private Vector3? thoughtItemBubbleDisplayOffsetWorld;

	private Vector3 thoughtItemBubbleDisplayOffsetScreen = new Vector3(0f, -5f, 0.01f);

	private SBGUIShadowedLabel thinkingLabel;

	private SBGUIShadowedLabel thinkingSkipLabel;

	private SBGUIShadowedLabel thinkingSkipJjCounter;

	private SBGUIAtlasImage thinkingIcon;

	private SBGUIButton thinkingGhostButton;

	private Vector3 displayOffsetScreen = Vector3.zero;

	private Vector3? displayOffsetWorld;

	private Vector3 textureOriginScreen = Vector3.zero;

	private Vector3? textureOriginWorld;

	private IDisplayController displayController;

	private string displayControllerExtension = string.Empty;

	private bool displayControllerFlipped;

	private IDisplayController footprintDisplayController;

	private static IDisplayController footprintDisplayControllerShared;

	private IDisplayController dropShadowDisplayController;

	private InteractionState interactionState = new InteractionState();

	private int selectionPriorityBaggage;

	private List<PendingCommand> pendingCommands = new List<PendingCommand>();

	private ParticleSystemManager.Request particlesRequest;

	private Vector3 particleDisplayOffsetScreen = Vector3.zero;

	private Vector3? particleDisplayOffsetWorld;

	private Scaffolding scaffolding;

	private Fence fence;

	private bool useFootprintIntersection;

	private bool debugHitBoxesVisible;

	private bool debugFootprintsVisible;

	private IDisplayController debugQuadHitBoxDisplayController;

	private IDisplayController debugThoughtBoxDisplayController;

	private IDisplayController debugAlignedBoxDisplayController;

	private string hitMeshName;

	private bool separateTap;

	private DateTime bounceStartTime;

	private DateTime bounceStartStartTime;

	private DateTime bounceEndStartTime;

	private bool calledBounceStart;

	private Color originalColor;

	private readonly Color BLOCKER_COLOR = new Color(1f, 0.55f, 0.45f);

	private bool swarmManaged;

	public Identity Id
	{
		get
		{
			return entity.Id;
		}
	}

	public string Description
	{
		get
		{
			return string.Format("sim( {0}, name( {1} ) )", entity.Id, entity.Name);
		}
	}

	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			visible = value;
			displayController.Visible = visible;
			if (dropShadowDisplayController != null)
			{
				dropShadowDisplayController.Visible = visible;
				dropShadowDisplayController.Alpha = 0.7f;
			}
			if (SBSettings.DebugDisplayControllers)
			{
				debugQuadHitBoxDisplayController.Visible = visible && debugHitBoxesVisible;
				debugAlignedBoxDisplayController.Visible = visible && debugFootprintsVisible;
				if (debugAlignedBoxDisplayController.Visible)
				{
					UpdateDebugFootprint();
				}
				if (debugThoughtBoxDisplayController != null)
				{
					debugThoughtBoxDisplayController.Visible = visible && debugHitBoxesVisible && thoughtDisplayController.GetDisplayState() != null;
				}
			}
		}
	}

	public bool DebugHitBoxesVisible
	{
		set
		{
			if (SBSettings.DebugDisplayControllers)
			{
				debugHitBoxesVisible = value;
				Visible = visible;
			}
		}
	}

	public bool SimulatedQueryable
	{
		get
		{
			return simulatedQueryable;
		}
		set
		{
			simulatedQueryable = value;
		}
	}

	public bool DebugFootprintsVisible
	{
		set
		{
			if (SBSettings.DebugDisplayControllers)
			{
				debugFootprintsVisible = value;
				Visible = visible;
			}
		}
	}

	public float Alpha
	{
		get
		{
			return displayController.Alpha;
		}
		set
		{
			displayController.Alpha = value;
			if (thoughtDisplayController != null && thoughtDisplayController.Visible)
			{
				thoughtDisplayController.Alpha = value;
				if (thoughtMaskDisplayController != null && thoughtMaskDisplayController.Visible)
				{
					thoughtMaskDisplayController.Alpha = value;
				}
			}
			if (thoughtItemBubbleDisplayController != null && thoughtItemBubbleDisplayController.Visible)
			{
				thoughtItemBubbleDisplayController.Alpha = value;
			}
		}
	}

	public Color Color
	{
		get
		{
			return displayController.Color;
		}
		set
		{
			displayController.Color = value;
			if (thoughtDisplayController != null && thoughtDisplayController.Visible)
			{
				thoughtDisplayController.Color = value;
				if (thoughtMaskDisplayController != null && thoughtMaskDisplayController.Visible)
				{
					thoughtMaskDisplayController.Color = value;
				}
			}
			if (thoughtItemBubbleDisplayController != null && thoughtItemBubbleDisplayController.Visible)
			{
				thoughtItemBubbleDisplayController.Color = value;
			}
		}
	}

	private float Width
	{
		get
		{
			return displayController.Width;
		}
	}

	private float Height
	{
		get
		{
			return displayController.Height;
		}
	}

	public IDisplayController DisplayController
	{
		get
		{
			return displayController;
		}
	}

	public SBGUIShadowedLabel DynamicThinkingLabel
	{
		get
		{
			if (thinkingLabel == null)
			{
				thinkingLabel = (SBGUIShadowedLabel)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/ThinkingTextLabel");
				thinkingLabel.name = string.Concat(Id, "_ThinkingLabel");
				thinkingLabel.SetParent(null);
			}
			thinkingLabel.SetActive(true);
			return thinkingLabel;
		}
	}

	public SBGUIShadowedLabel DynamicThinkingSkipLabel
	{
		get
		{
			if (thinkingSkipLabel == null)
			{
				thinkingSkipLabel = (SBGUIShadowedLabel)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/ThinkingTextLabel");
				thinkingSkipLabel.name = string.Concat(Id, "_ThinkingSkipLabel");
				thinkingSkipLabel.SetParent(null);
			}
			thinkingSkipLabel.SetActive(true);
			return thinkingSkipLabel;
		}
	}

	public SBGUIShadowedLabel DynamicThinkingSkipJjCounter
	{
		get
		{
			if (thinkingSkipJjCounter == null)
			{
				thinkingSkipJjCounter = (SBGUIShadowedLabel)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/ThinkingTextLabel");
				thinkingSkipJjCounter.name = string.Concat(Id, "_ThinkingSkipCounter");
				thinkingSkipJjCounter.SetParent(null);
			}
			thinkingSkipJjCounter.SetActive(true);
			return thinkingSkipJjCounter;
		}
	}

	public SBGUIAtlasImage DynamicThinkingIcon
	{
		get
		{
			if (thinkingIcon == null)
			{
				thinkingIcon = (SBGUIAtlasImage)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/ThinkingIcon");
				thinkingIcon.name = string.Concat(Id, "_ThinkingIcon");
				thinkingIcon.SetParent(null);
			}
			thinkingIcon.SetActive(true);
			if (thinkingIcon.FindChild("UnavailableIcon") != null)
			{
				thinkingIcon.FindChild("UnavailableIcon").SetActive(showUnavailableIcon);
			}
			return thinkingIcon;
		}
	}

	public SBGUIButton ThinkingGhostButton
	{
		get
		{
			if (thinkingGhostButton == null)
			{
				thinkingGhostButton = (SBGUIButton)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/ThinkingGhostButton");
				thinkingGhostButton.name = string.Concat(Id, "_ThinkingGhostButton");
				thinkingGhostButton.SetParent(null);
			}
			thinkingGhostButton.SetActive(true);
			thinkingGhostButton.SetVisible(false);
			return thinkingGhostButton;
		}
	}

	public bool FootprintVisible
	{
		get
		{
			return footprintDisplayController != null;
		}
		set
		{
			if (value)
			{
				simFlags |= SimulatedFlags.FORCE_ANIMATE_FOOTPRINT;
				float num;
				float num2;
				if (Flip)
				{
					AlignedBox alignedBox = entity.Invariable["footprint.flip"] as AlignedBox;
					num = alignedBox.Width;
					num2 = alignedBox.Height;
				}
				else
				{
					num = footprint.xmax;
					num2 = footprint.ymax;
				}
				if (footprintDisplayControllerShared != null && !footprintDisplayControllerShared.IsDestroyed)
				{
					((BasicSprite)footprintDisplayControllerShared).Resize(Vector2.zero, num, num2);
				}
				else
				{
					footprintDisplayControllerShared = new BasicSprite("Materials/unique/footprint", null, new Vector2(-0.5f * num, -0.5f * num2), num, num2);
					((BasicSprite)footprintDisplayControllerShared).PublicInitialize();
				}
				footprintDisplayControllerShared.Visible = true;
				footprintDisplayController = footprintDisplayControllerShared;
			}
			else
			{
				simFlags &= ~SimulatedFlags.FORCE_ANIMATE_FOOTPRINT;
				if (footprintDisplayControllerShared != null)
				{
					footprintDisplayControllerShared.Visible = false;
				}
				footprintDisplayController = null;
			}
		}
	}

	public Color FootprintColor
	{
		set
		{
			if (footprintDisplayController != null)
			{
				footprintDisplayController.Color = value;
			}
		}
	}

	private string StateModifierString
	{
		get
		{
			return mStateModifierString;
		}
		set
		{
			mStateModifierString = value;
		}
	}

	public Vector2 Position
	{
		get
		{
			return position[1];
		}
		set
		{
			position[0] = position[1];
			position[1] = value;
			snapPosition = value;
			UpdateAlignedBox();
		}
	}

	public Vector2 PositionCenter
	{
		get
		{
			return new Vector2(position[1].x + footprint.Width / 2f, position[1].y + footprint.Height / 2f);
		}
	}

	public Vector2 SnapPosition
	{
		get
		{
			return snapPosition;
		}
		set
		{
			snapPosition = value;
			UpdateAlignedBox();
		}
	}

	public Vector2 ThoughtDisplayOffsetScreen
	{
		get
		{
			return thoughtDisplayOffsetScreen;
		}
	}

	public Vector3 ThoughtDisplayOffsetWorld
	{
		get
		{
			TFUtils.Assert(thoughtDisplayOffsetWorld.HasValue, "You should calculate the world offset before trying to call this property!");
			return thoughtDisplayOffsetWorld.GetValueOrDefault();
		}
	}

	public Vector3 DisplayOffsetWorld
	{
		get
		{
			Vector3? vector = displayOffsetWorld;
			if (vector.HasValue)
			{
				return displayOffsetWorld.Value;
			}
			return Vector3.zero;
		}
	}

	public Vector3 TextureOriginWorld
	{
		get
		{
			Vector3? vector = textureOriginWorld;
			if (vector.HasValue)
			{
				return textureOriginWorld.Value;
			}
			return Vector3.zero;
		}
	}

	public Vector2 ThoughtMaskDisplayOffsetScreen
	{
		get
		{
			return thoughtMaskDisplayOffsetScreen;
		}
	}

	public Vector3 ThoughtMaskDisplayOffsetWorld
	{
		get
		{
			TFUtils.Assert(thoughtMaskDisplayOffsetWorld.HasValue, "You should calculate the world offset before trying to call this property!");
			return thoughtMaskDisplayOffsetWorld.GetValueOrDefault();
		}
	}

	public IDisplayController ThoughtDisplayController
	{
		get
		{
			return thoughtDisplayController;
		}
	}

	public IDisplayController ThoughtMaskDisplayController
	{
		get
		{
			return thoughtMaskDisplayController;
		}
	}

	public Vector2 PointOfInterest
	{
		get
		{
			return Position + pointOfInterestOffset;
		}
	}

	public bool WorkerSpawner
	{
		get
		{
			return workerSpawner;
		}
	}

	public bool IsWaypoint
	{
		get
		{
			return isWaypoint;
		}
	}

	public AlignedBox Box
	{
		get
		{
			return box;
		}
	}

	public AlignedBox SnapBox
	{
		get
		{
			return snapBox;
		}
	}

	public AlignedBox Footprint
	{
		get
		{
			return footprint;
		}
	}

	public InteractionState InteractionState
	{
		get
		{
			return interactionState;
		}
	}

	public List<Action> ClickListeners
	{
		get
		{
			return new List<Action>(clickListeners.ToArray());
		}
	}

	public int SelectionPriority
	{
		get
		{
			int num = SelectionPriorityBaggage;
			int num2 = prioritizedActions.IndexOf(action);
			if (num2 == -1)
			{
				num2 = prioritizedActions.Count;
			}
			if (!interactionState.IsSelectable && !interactionState.HasClickCommandFunctionality)
			{
				num += prioritizedActions.Count;
			}
			return num2 + num;
		}
	}

	public int SelectionPriorityBaggage
	{
		get
		{
			return selectionPriorityBaggage;
		}
		set
		{
			selectionPriorityBaggage = value;
		}
	}

	public int TemptationPriority
	{
		get
		{
			return -1 * (priorityOrder.IndexOf(action) + 1);
		}
	}

	public bool Flip
	{
		get
		{
			return displayControllerFlipped;
		}
		set
		{
			displayControllerFlipped = value;
			string displayState = displayController.GetDisplayState();
			int num = displayState.LastIndexOf('.');
			string state = displayState.Substring(0, (num < 0) ? displayState.Length : num);
			if (value)
			{
				displayController.DefaultDisplayState = "default.flip";
				if (entity.Invariable.ContainsKey("display.default.flip.position_offset") && entity.Invariable["display.default.flip.position_offset"] != null)
				{
					displayOffsetScreen = (Vector3)entity.Invariable["display.default.flip.position_offset"];
				}
				displayControllerExtension = ".flip";
				footprint = (AlignedBox)entity.Invariable["footprint.flip"];
			}
			else
			{
				if (entity.Invariable["display.position_offset"] != null)
				{
					displayOffsetScreen = (Vector3)entity.Invariable["display.position_offset"];
				}
				displayController.DefaultDisplayState = "default";
				displayControllerExtension = string.Empty;
				footprint = (AlignedBox)entity.Invariable["footprint"];
			}
			DisplayState(state);
			UpdateAlignedBox();
			UpdateDebugFootprint();
		}
	}

	public ReadOnlyIndexer Invariable
	{
		get
		{
			return entity.Invariable;
		}
	}

	public ReadWriteIndexer Variable
	{
		get
		{
			return entity.Variable;
		}
	}

	public bool IsSwarmManaged
	{
		get
		{
			return swarmManaged;
		}
		set
		{
			swarmManaged = value;
		}
	}

	public Entity Entity
	{
		get
		{
			return entity;
		}
	}

	public Simulated(Simulation simulation, Entity entity, Vector2 position)
	{
		this.entity = entity;
		footprint = this.entity.Invariable["footprint"] as AlignedBox;
		Position = position;
		Position = position;
		if (this.entity.Invariable["dropshadow"] != null)
		{
			dropShadowDisplayController = new DeferredDisplayController(this.entity.Invariable["dropshadow"] as IDisplayController, simulation.EntityManager.DisplayControllerManager);
			dropShadowDisplayController.Visible = true;
		}
		machine = this.entity.Invariable["machine"] as StateMachine<StateAction, Command.TYPE>;
		foreach (StateAction state in machine.States)
		{
			delegatedCommands.Add(state, new Queue<Command>());
		}
		if ((bool)this.entity.Invariable["thought_display_movement"])
		{
			float num = 8f;
			periodicMovement = new Sinusoid(0f, 12f, num, UnityEngine.Random.Range(0f, num));
		}
		if (this.entity.Invariable["display.position_offset"] != null)
		{
			displayOffsetScreen = (Vector3)this.entity.Invariable["display.position_offset"];
		}
		if (this.entity.Invariable.ContainsKey("display.texture_origin") && this.entity.Invariable["display.texture_origin"] != null)
		{
			textureOriginScreen = (Vector3)this.entity.Invariable["display.texture_origin"];
		}
		if (this.entity.Invariable.ContainsKey("display.mesh_name") && this.entity.Invariable["display.mesh_name"] != null)
		{
			if (this.entity.Invariable.ContainsKey("display.separate_tap") && this.entity.Invariable["display.separate_tap"] != null)
			{
				separateTap = (bool)this.entity.Invariable["display.separate_tap"];
			}
			hitMeshName = (string)this.entity.Invariable["display.mesh_name"];
			displayController = (this.entity.Invariable["display"] as IDisplayController).CloneWithHitMesh(simulation.EntityManager.DisplayControllerManager, hitMeshName, separateTap);
		}
		else
		{
			displayController = (this.entity.Invariable["display"] as IDisplayController).Clone(simulation.EntityManager.DisplayControllerManager);
		}
		displayController.Billboard(BillboardDelegate);
		if (this.entity.Invariable["thought_display"] != null)
		{
			if (this.entity.Invariable["thought_display.position_offset"] != null)
			{
				thoughtDisplayOffsetScreen = (Vector3)this.entity.Invariable["thought_display.position_offset"];
			}
			thoughtDisplayController = new DeferredDisplayController(this.entity.Invariable["thought_display"] as IDisplayController, simulation.EntityManager.DisplayControllerManager);
			thoughtDisplayController.Billboard(SBCamera.BillboardDefinition);
			if (this.entity.Invariable["thought_mask_display"] != null)
			{
				thoughtMaskDisplayController = new DeferredDisplayController(this.entity.Invariable["thought_mask_display"] as IDisplayController, simulation.EntityManager.DisplayControllerManager);
				thoughtMaskDisplayController.Billboard(BillboardDelegate);
				if (this.entity.Invariable["thought_mask_display.position_offset"] != null)
				{
					thoughtMaskDisplayOffsetScreen = (Vector3)this.entity.Invariable["thought_mask_display.position_offset"];
				}
			}
			if (this.entity.Invariable["thought_item_bubble_display"] != null)
			{
				thoughtItemBubbleDisplayController = new DeferredDisplayController(this.entity.Invariable["thought_item_bubble_display"] as IDisplayController, simulation.EntityManager.DisplayControllerManager);
				thoughtItemBubbleDisplayController.Billboard(SBCamera.BillboardDefinition);
				if (this.entity.Invariable["thought_item_bubble_display.position_offset"] != null)
				{
					thoughtItemBubbleDisplayOffsetScreen = (Vector3)this.entity.Invariable["thought_item_bubble_display.position_offset"];
				}
				if (TFPerfUtils.IsNonScalingDevice())
				{
					thoughtItemBubbleScalingMajor = new ConstantPattern(1f);
					thoughtItemBubbleScalingMinor = new ConstantPattern(1f);
				}
				else
				{
					thoughtItemBubbleScalingMajor = new Sinusoid(1f, 1.2f, 10f, UnityEngine.Random.Range(0f, 10f));
					thoughtItemBubbleScalingMinor = new Sinusoid(1f, 1.05f, UnityEngine.Random.Range(1f, 2f), 0f);
				}
			}
		}
		if (thoughtDisplayController != null && HasEntity<ResidentEntity>())
		{
			thoughtDisplayController.HitObject.Height -= 15f;
		}
		if (SBSettings.DebugDisplayControllers)
		{
			BasicSprite basicSprite = (this.entity.Invariable["debugBoxSprite"] as IDisplayController).Clone(simulation.EntityManager.DisplayControllerManager) as BasicSprite;
			basicSprite.Visible = true;
			basicSprite.Color = Color.magenta;
			basicSprite.Alpha = 0.2f;
			basicSprite.Resize(displayController.HitObject.Center, displayController.HitObject.Width, displayController.HitObject.Height);
			debugQuadHitBoxDisplayController = basicSprite;
			debugQuadHitBoxDisplayController.Billboard(SBCamera.BillboardDefinition);
			if (thoughtDisplayController != null)
			{
				BasicSprite basicSprite2 = (this.entity.Invariable["debugBoxSprite"] as IDisplayController).Clone(simulation.EntityManager.DisplayControllerManager) as BasicSprite;
				basicSprite2.Visible = true;
				basicSprite2.Color = Color.red;
				basicSprite2.Alpha = 0.4f;
				basicSprite2.Resize(thoughtDisplayController.HitObject.Center, thoughtDisplayController.HitObject.Width, thoughtDisplayController.HitObject.Height);
				debugThoughtBoxDisplayController = basicSprite2;
				debugThoughtBoxDisplayController.Billboard(SBCamera.BillboardDefinition);
			}
			BasicSprite basicSprite3 = (this.entity.Invariable["footprintSprite"] as IDisplayController).Clone(simulation.EntityManager.DisplayControllerManager) as BasicSprite;
			basicSprite3.Visible = true;
			basicSprite3.Color = Color.green;
			basicSprite3.Alpha = 0.4f;
			debugAlignedBoxDisplayController = basicSprite3;
		}
		if (this.entity.Invariable.ContainsKey("point_of_interest"))
		{
			pointOfInterestOffset = (Vector2)this.entity.Invariable["point_of_interest"];
		}
		if (this.entity.Invariable.ContainsKey("worker_spawner"))
		{
			workerSpawner = (bool)this.entity.Invariable["worker_spawner"];
		}
		if (this.entity.Invariable.ContainsKey("is_waypoint"))
		{
			isWaypoint = (bool)this.entity.Invariable["is_waypoint"];
		}
		if (this.entity.Invariable.ContainsKey("fx.producing.position_offset"))
		{
			particleDisplayOffsetScreen = (Vector3)this.entity.Invariable["fx.producing.position_offset"];
		}
		particleSystemRequestDelegate = new ParticleSystemRequestDelegate(this);
		rewardParticleSystemRequestDelegate = new RewardParticleRequestDelegate(this);
		thoughtBubblePopParticleRequestDelegate = new ThoughtBubblePopParticleRequestDelegate(this);
		eatParticleRequestDelegate = new EatParticleRequestDelegate(this);
		activateParticleSystemRequestDelegate = new ActivateParticleRequestDelegate(this);
		dustParticleSystemRequestDelegate = new SimulatedParticleRequestDelegate(this);
		starsParticleSystemRequestDelegate = new SimulatedParticleRequestDelegate(this);
		simFlags = SimulatedFlags.FIRST_ANIMATE;
		if (HasEntity<StructureDecorator>() && GetEntity<StructureDecorator>().Immobile)
		{
			simFlags |= SimulatedFlags.BUILDING_ANIM_PATH;
		}
		else
		{
			simFlags |= SimulatedFlags.MOBILE;
		}
	}

	public void ClearPathInfo()
	{
		Variable["path"] = null;
	}

	private void UpdateAlignedBox()
	{
		box.xmin = position[1].x + footprint.xmin;
		box.xmax = position[1].x + footprint.xmax;
		box.ymin = position[1].y + footprint.ymin;
		box.ymax = position[1].y + footprint.ymax;
		snapBox.xmin = snapPosition.x + footprint.xmin;
		snapBox.xmax = snapPosition.x + footprint.xmax;
		snapBox.ymin = snapPosition.y + footprint.ymin;
		snapBox.ymax = snapPosition.y + footprint.ymax;
		if (scaffolding != null)
		{
			scaffolding.SetEnclosureBox(box);
		}
		if (fence != null)
		{
			fence.SetEnclosureBox(box);
		}
	}

	public string UseStateModifierString(string state)
	{
		if (mStateModifierString == null)
		{
			return state;
		}
		return string.Format(mStateModifierString, state);
	}

	private void UpdateDebugFootprint()
	{
		if (!SBSettings.DebugDisplayControllers || (!debugHitBoxesVisible && !debugFootprintsVisible))
		{
			return;
		}
		BasicSprite basicSprite = debugAlignedBoxDisplayController as BasicSprite;
		if (basicSprite != null)
		{
			if (Flip)
			{
				AlignedBox alignedBox = entity.Invariable["footprint.flip"] as AlignedBox;
				basicSprite.Resize(new Vector2(-0.5f * alignedBox.Width, -0.5f * alignedBox.Height), alignedBox.Width, alignedBox.Height);
			}
			else
			{
				AlignedBox alignedBox2 = entity.Invariable["footprint"] as AlignedBox;
				basicSprite.Resize(new Vector2(-0.5f * alignedBox2.Width, -0.5f * alignedBox2.Height), alignedBox2.Width, alignedBox2.Height);
			}
			simFlags |= SimulatedFlags.FORCE_ANIMATE_FOOTPRINT;
		}
	}

	public void AddClickListener(Action handler)
	{
		clickListeners.Add(handler);
	}

	public bool RemoveClickListener(Action handler)
	{
		return clickListeners.Remove(handler);
	}

	protected bool IntersectsFootprint(Ray ray)
	{
		Vector3 zero = Vector3.zero;
		zero.x = Position.x;
		zero.y = Position.y;
		float enter;
		if (new Plane(Vector3.forward, zero).Raycast(ray, out enter))
		{
			Vector3 vector = ray.origin + ray.direction * enter;
			return box.xmin < vector.x && vector.x < box.xmax && box.ymin < vector.y && vector.y < box.ymax;
		}
		return false;
	}

	public bool Intersects(Ray ray)
	{
		return displayController.Intersects(ray) || (thoughtDisplayController != null && thoughtDisplayController.GetDisplayState() != null && thoughtDisplayController.Visible && thoughtDisplayController.Intersects(ray)) || (useFootprintIntersection && IntersectsFootprint(ray));
	}

	public void LoadInitialState(StateAction action)
	{
		this.action = action;
	}

	public void EnterInitialState(StateAction action, Simulation simulation)
	{
		this.action = action;
		this.action.Enter(simulation, this);
	}

	public void Push(Command command)
	{
		commands.Enqueue(command);
	}

	public bool HasEntity<T>() where T : EntityDecorator
	{
		return entity.HasDecorator<T>();
	}

	public T GetEntity<T>() where T : EntityDecorator
	{
		return entity.GetDecorator<T>();
	}

	public void SetFootprint(Simulation simulation, bool enable = true)
	{
		Terrain terrain = ((simulation == null) ? null : simulation.Terrain);
		bool flag = HasEntity<StructureDecorator>() && GetEntity<StructureDecorator>().IsObstacle;
		if (terrain != null && box.xmin >= 0f && flag)
		{
			terrain.SetOrClearObstacle(box, enable);
			simulation.ResetAllAffectedPaths(box);
		}
	}

	public void Warp(Vector2 position, Simulation simulation = null)
	{
		Position = position;
		Position = position;
	}

	public void FlipWarp(Simulation simulation = null)
	{
		if ((simFlags & SimulatedFlags.FORCE_ANIMATE_FOOTPRINT) != 0)
		{
			simFlags = SimulatedFlags.BUILDING_ANIM_PATH | SimulatedFlags.FORCE_ANIMATE_FOOTPRINT;
			if (footprintDisplayController != null)
			{
				footprintDisplayController.Position = snapPosition + new Vector2(footprintDisplayController.Width * 0.5f, footprintDisplayController.Height * 0.5f);
			}
		}
		else
		{
			simFlags = SimulatedFlags.BUILDING_ANIM_PATH;
		}
		displayOffsetWorld = CameraOffsetToWorldVector(displayOffsetScreen, simulation.TheCamera);
		Vector3 vector = new Vector3((box.xmax + box.xmin) * 0.5f, (box.ymax + box.ymin) * 0.5f, 0f);
		displayController.Position = displayOffsetWorld.Value + vector;
	}

	public void AddScaffolding(Simulation simulation)
	{
		if (scaffolding == null)
		{
			scaffolding = simulation.enclosureManager.AddScaffolding(Box, SBCamera.BillboardDefinition);
		}
		if (!scaffolding.IsInitialized())
		{
			scaffolding = null;
		}
	}

	public void RemoveScaffolding(Simulation simulation)
	{
		if (scaffolding != null)
		{
			simulation.enclosureManager.RemoveScaffolding(scaffolding);
			scaffolding = null;
		}
	}

	public void AddFence(Simulation simulation)
	{
		if (fence == null)
		{
			fence = simulation.enclosureManager.AddFence(Box, SBCamera.BillboardDefinition);
		}
		if (!fence.IsInitialized())
		{
			fence = null;
		}
	}

	public void RemoveFence(Simulation simulation)
	{
		if (fence != null)
		{
			simulation.enclosureManager.RemoveFence(fence);
			fence = null;
		}
	}

	public bool Simulate(Simulation simulation, Session session)
	{
		if (command == null && commands.Count != 0)
		{
			command = commands.Dequeue();
			StateAction result;
			if (machine.Transition(action, command.Type, out result))
			{
				StateAction stateAction = action;
				action.Leave(simulation, this);
				action = result;
				command.TryExecuteOnComplete();
				action.Enter(simulation, this);
			}
			else if (machine.Delegate(action, command.Type, out result))
			{
				delegatedCommands[result].Enqueue(command);
			}
			else
			{
				command.TryExecuteOnComplete();
				command = null;
			}
		}
		return action.Simulate(simulation, this, session);
	}

	public void UpdateControls(Simulation simulation)
	{
		if (action != null && action is StateActionBuildingDefault)
		{
			(action as StateActionBuildingDefault).UpdateControls(simulation, this);
		}
	}

	public void DestroyDisplayControllers()
	{
		displayController.Destroy();
		if (thoughtDisplayController != null)
		{
			thoughtDisplayController.Destroy();
			if (thoughtMaskDisplayController != null)
			{
				thoughtMaskDisplayController.Destroy();
			}
		}
		if (thoughtItemBubbleDisplayController != null)
		{
			thoughtItemBubbleDisplayController.Destroy();
		}
		if (dropShadowDisplayController != null)
		{
			dropShadowDisplayController.Destroy();
		}
		if (SBSettings.DebugDisplayControllers)
		{
			debugQuadHitBoxDisplayController.Destroy();
			debugAlignedBoxDisplayController.Destroy();
			if (debugThoughtBoxDisplayController != null)
			{
				debugThoughtBoxDisplayController.Destroy();
			}
		}
		if (thinkingIcon != null)
		{
			UnityEngine.Object.Destroy(thinkingIcon.gameObject);
			thinkingIcon = null;
		}
		if (thinkingSkipLabel != null)
		{
			UnityEngine.Object.Destroy(thinkingSkipLabel.gameObject);
			thinkingSkipLabel = null;
		}
		if (thinkingSkipJjCounter != null)
		{
			UnityEngine.Object.Destroy(thinkingSkipJjCounter.gameObject);
			thinkingSkipJjCounter = null;
		}
		if (thinkingLabel != null)
		{
			UnityEngine.Object.Destroy(thinkingLabel.gameObject);
			thinkingLabel = null;
		}
		if (thinkingGhostButton != null)
		{
			UnityEngine.Object.Destroy(thinkingGhostButton.gameObject);
			thinkingGhostButton = null;
		}
	}

	public void Destroy(Simulation simulation)
	{
		TFUtils.DebugLog("Responding to Destroy Command with default action for Simulated(" + Id.Describe() + ")");
		DestroyDisplayControllers();
		simulation.particleSystemManager.RemoveRequestWithDelegate(particleSystemRequestDelegate);
		simulation.particleSystemManager.RemoveRequestWithDelegate(activateParticleSystemRequestDelegate);
		simulation.particleSystemManager.RemoveRequestWithDelegate(rewardParticleSystemRequestDelegate);
		simulation.particleSystemManager.RemoveRequestWithDelegate(dustParticleSystemRequestDelegate);
		simulation.particleSystemManager.RemoveRequestWithDelegate(starsParticleSystemRequestDelegate);
		command = null;
	}

	public void FirstAnimate(Simulation simulation)
	{
		if (!displayOffsetWorld.HasValue)
		{
			displayOffsetWorld = CameraOffsetToWorldVector(displayOffsetScreen, simulation.TheCamera);
		}
		if ((simFlags & SimulatedFlags.BUILDING_ANIM_PATH) != 0 && !textureOriginWorld.HasValue)
		{
			if (textureOriginScreen != Vector3.zero)
			{
				textureOriginWorld = CameraOffsetToWorldVector(textureOriginScreen, simulation.TheCamera);
			}
			else
			{
				textureOriginWorld = Vector3.zero;
			}
		}
		if (thoughtDisplayController != null)
		{
			Vector3 vector = -simulation.TheCamera.transform.forward * 1f;
			if (!thoughtDisplayOffsetWorld.HasValue)
			{
				thoughtDisplayOffsetWorld = CameraOffsetToWorldVector(thoughtDisplayOffsetScreen, simulation.TheCamera);
			}
			if (thoughtItemBubbleDisplayController != null && !thoughtItemBubbleDisplayOffsetWorld.HasValue)
			{
				thoughtItemBubbleDisplayOffsetWorld = CameraOffsetToWorldVector(thoughtItemBubbleDisplayOffsetScreen, simulation.TheCamera);
				Vector3? vector2 = thoughtItemBubbleDisplayOffsetWorld;
				thoughtItemBubbleDisplayOffsetWorld = ((!vector2.HasValue) ? ((Vector3?)null) : new Vector3?(vector2.Value + vector));
			}
			if (!thoughtMaskDisplayOffsetWorld.HasValue)
			{
				thoughtMaskDisplayOffsetWorld = CameraOffsetToWorldVector(thoughtMaskDisplayOffsetScreen, simulation.TheCamera);
				Vector3? vector3 = thoughtMaskDisplayOffsetWorld;
				thoughtMaskDisplayOffsetWorld = ((!vector3.HasValue) ? ((Vector3?)null) : new Vector3?(vector3.Value + vector * 2f));
			}
		}
		simFlags &= ~SimulatedFlags.FIRST_ANIMATE;
	}

	public void EnableAnimateAction(bool enable)
	{
		if (enable)
		{
			simFlags |= SimulatedFlags.FORCE_ANIMATE_ACTION;
			return;
		}
		simFlags &= ~SimulatedFlags.FORCE_ANIMATE_ACTION;
		Vector3 vector = new Vector3((box.xmax + box.xmin) * 0.5f, (box.ymax + box.ymin) * 0.5f, 0f);
		displayController.Position = displayOffsetWorld.Value + vector - textureOriginWorld.Value;
	}

	public void Animate(Simulation simulation)
	{
		if ((simFlags & SimulatedFlags.BUILDING_ANIM_PATH) != 0)
		{
			Vector3 vector = Vector3.zero;
			if ((simFlags & SimulatedFlags.FORCE_ANIMATE_ACTION) != 0 && action is Animated)
			{
				vector = ((Animated)action).Animate(simulation, this);
			}
			if ((simFlags & SimulatedFlags.FORCE_ANIMATE_FOOTPRINT) != 0 && footprintDisplayController != null)
			{
				footprintDisplayController.Position = snapPosition + new Vector2(footprintDisplayController.Width * 0.5f, footprintDisplayController.Height * 0.5f);
			}
			Vector3 vector2 = new Vector3((box.xmax + box.xmin) * 0.5f, (box.ymax + box.ymin) * 0.5f, 0f);
			displayController.Position = displayOffsetWorld.Value + vector2 - textureOriginWorld.Value + vector;
		}
		else
		{
			Vector2 vector3 = (position[1] - position[0]) * simulation.Interpolant + position[0];
			Vector3 vector4 = new Vector3(vector3.x, vector3.y, 0f);
			displayController.Position = vector4 + displayOffsetWorld.Value;
			if (dropShadowDisplayController != null)
			{
				dropShadowDisplayController.Position = vector4 + displayOffsetWorld.Value;
			}
		}
		if ((simFlags & SimulatedFlags.FORCE_ANIMATE_BOUNCE) != 0)
		{
			AnimateBounce(simulation);
		}
		if ((simFlags & SimulatedFlags.FORCE_ANIMATE_BOUNCE_START) != 0)
		{
			AnimateBounceStart(simulation);
		}
		if ((simFlags & SimulatedFlags.FORCE_ANIMATE_BOUNCE_END) != 0)
		{
			AnimateBounceEnd(simulation);
		}
	}

	public void Bounce()
	{
		simFlags |= SimulatedFlags.FORCE_ANIMATE_BOUNCE;
		bounceStartTime = DateTime.Now;
	}

	public void BounceStart()
	{
		simFlags |= SimulatedFlags.FORCE_ANIMATE_BOUNCE_START;
		bounceStartStartTime = DateTime.Now;
	}

	public void BounceEnd()
	{
		simFlags |= SimulatedFlags.FORCE_ANIMATE_BOUNCE_END;
		bounceEndStartTime = DateTime.Now;
	}

	public void BounceCleanup()
	{
		if (calledBounceStart)
		{
			BounceEnd();
		}
	}

	public void AnimateBounce(Simulation simulation)
	{
		float num = (float)(DateTime.Now - bounceStartTime).TotalSeconds;
		AnimateScaleAndFlip(simulation.bounceEndInterpolator.GetHermiteAtTime(num));
		if (num > simulation.bounceInterpolator.MaxTime)
		{
			simFlags &= ~SimulatedFlags.FORCE_ANIMATE_BOUNCE;
		}
	}

	public void AnimateBounceStart(Simulation simulation)
	{
		float num = (float)(DateTime.Now - bounceStartStartTime).TotalSeconds;
		calledBounceStart = true;
		AnimateScaleAndFlip(simulation.bounceStartInterpolator.GetHermiteAtTime(num));
		if (num > simulation.bounceStartInterpolator.MaxTime)
		{
			simFlags &= ~SimulatedFlags.FORCE_ANIMATE_BOUNCE_START;
		}
	}

	public void AnimateBounceEnd(Simulation simulation)
	{
		float num = (float)(DateTime.Now - bounceEndStartTime).TotalSeconds;
		calledBounceStart = false;
		AnimateScaleAndFlip(simulation.bounceEndInterpolator.GetHermiteAtTime(num));
		if (num > simulation.bounceEndInterpolator.MaxTime)
		{
			simFlags &= ~SimulatedFlags.FORCE_ANIMATE_BOUNCE_END;
		}
	}

	public void AnimateScaleAndFlip(Vector3 scale)
	{
		if (!TFPerfUtils.IsNonScalingDevice())
		{
			if (Flip)
			{
				scale.x *= -1f;
			}
			displayController.Scale = scale;
		}
	}

	public void AnimateDebugHitBox(Simulation simulation)
	{
		if (SBSettings.DebugDisplayControllers && (debugHitBoxesVisible || debugFootprintsVisible))
		{
			debugQuadHitBoxDisplayController.Position = displayController.Position;
			debugQuadHitBoxDisplayController.OnUpdate(simulation.TheCamera, null);
			Vector2 vector = (position[1] - position[0]) * simulation.Interpolant + position[0];
			Vector3 vector2 = new Vector3(vector.x, vector.y, 0f);
			debugAlignedBoxDisplayController.Position = vector2;
			if (debugThoughtBoxDisplayController != null && thoughtDisplayController.GetDisplayState() != null)
			{
				debugThoughtBoxDisplayController.Position = thoughtDisplayController.Position;
				debugThoughtBoxDisplayController.OnUpdate(simulation.TheCamera, null);
			}
		}
	}

	public void AnimateOtherControllers(Simulation simulation)
	{
		TFUtils.Assert(thoughtDisplayOffsetWorld.HasValue, "Need to set thoughtDisplayOffsetWorld before calling AnimateOtherControllers!");
		float atTime = (float)simulation.Time;
		float z = ((periodicMovement == null) ? 0f : periodicMovement.ValueAtTime(atTime));
		Vector3 vector = displayController.Position + thoughtDisplayOffsetWorld.Value + new Vector3(0f, 0f, z);
		thoughtDisplayController.Position = vector;
		if (thoughtMaskDisplayController != null)
		{
			thoughtMaskDisplayController.Position = vector + thoughtMaskDisplayOffsetWorld.Value;
			DisplayControllerFlags flags = thoughtMaskDisplayController.Flags;
			if ((flags & (DisplayControllerFlags.SWITCHED_STATE | DisplayControllerFlags.NEED_UPDATE)) != 0)
			{
				thoughtMaskDisplayController.OnUpdate(simulation.TheCamera, simulation.particleSystemManager);
				thoughtMaskDisplayController.Flags = flags & ~DisplayControllerFlags.SWITCHED_STATE;
			}
		}
		TFUtils.Assert(thoughtItemBubbleDisplayController != null, "Simulateds are all assumed to have thoughtItemBubbleDisplayController");
		DisplayControllerFlags flags2 = thoughtItemBubbleDisplayController.Flags;
		if ((flags2 & DisplayControllerFlags.VISIBLE_AND_VALID_STATE) != 0)
		{
			thoughtItemBubbleDisplayController.Position = vector + thoughtItemBubbleDisplayOffsetWorld.Value;
			if (periodicMovement != null)
			{
				float num = thoughtItemBubbleScalingMajor.ValueAtTime(atTime);
				float num2 = thoughtItemBubbleScalingMinor.ValueAtTime(atTime);
				float num3 = num * num2;
				thoughtItemBubbleDisplayController.Scale = new Vector3(num3, num3, num3);
			}
			if ((flags2 & (DisplayControllerFlags.SWITCHED_STATE | DisplayControllerFlags.NEED_UPDATE)) != 0)
			{
				thoughtItemBubbleDisplayController.OnUpdate(simulation.TheCamera, simulation.particleSystemManager);
				thoughtItemBubbleDisplayController.Flags = flags2 & ~DisplayControllerFlags.SWITCHED_STATE;
			}
		}
	}

	public void DisplayState(string state)
	{
		state = UseStateModifierString(state);
		string text = ((state == null) ? null : (state + displayControllerExtension));
		displayController.DisplayState(text);
		string property = "display." + text + ".mesh_name";
		if (entity.Invariable.ContainsKey(property) && entity.Invariable[property] != null)
		{
			hitMeshName = (string)entity.Invariable[property];
			displayController.ChangeMesh(text, hitMeshName);
		}
		if (dropShadowDisplayController != null)
		{
			dropShadowDisplayController.Visible = state != null;
		}
		if (SBSettings.DebugDisplayControllers)
		{
			debugQuadHitBoxDisplayController.Visible = state != null && debugHitBoxesVisible;
			debugAlignedBoxDisplayController.Visible = state != null && debugFootprintsVisible;
			if (debugThoughtBoxDisplayController != null)
			{
				debugThoughtBoxDisplayController.Visible = debugQuadHitBoxDisplayController.Visible && thoughtDisplayController.GetDisplayState() != null;
			}
		}
	}

	public string GetDisplayState()
	{
		return displayController.GetDisplayState();
	}

	public void DisplayThoughtState(string state, Simulation simulation)
	{
		string displayState = thoughtItemBubbleDisplayController.GetDisplayState();
		string text = "thought_display." + state;
		thoughtDisplayController.DisplayState(state);
		if (state != null && displayState == null)
		{
			FirstAnimate(simulation);
			AnimateOtherControllers(simulation);
		}
		string property = text + ".position_offset";
		if (!entity.Invariable.ContainsKey(property))
		{
			property = "thought_display.default.position_offset";
		}
		if (entity.Invariable.ContainsKey(property))
		{
			Vector3 vector = (Vector3)entity.Invariable[property];
			if (thoughtDisplayOffsetScreen != vector)
			{
				thoughtDisplayOffsetScreen = vector;
				thoughtDisplayOffsetWorld = CameraOffsetToWorldVector(thoughtDisplayOffsetScreen, simulation.TheCamera);
			}
		}
		if (thoughtMaskDisplayController != null)
		{
			thoughtMaskDisplayController.DisplayState(state);
		}
		if (thoughtItemBubbleDisplayController != null)
		{
			thoughtItemBubbleDisplayController.DisplayState(state);
		}
		if (SBSettings.DebugDisplayControllers && debugThoughtBoxDisplayController != null)
		{
			debugThoughtBoxDisplayController.Visible = debugQuadHitBoxDisplayController.Visible && thoughtDisplayController.GetDisplayState() != null;
		}
	}

	public void DisplayThoughtState(string overrideSubjectMaterial, string state, Simulation simulation)
	{
		DisplayThoughtState(state, simulation);
		TFUtils.Assert(overrideSubjectMaterial != null, "Cannot set thought display's subject material to null");
		thoughtDisplayController.UpdateMaterialOrTexture(overrideSubjectMaterial);
		DisplayThoughtItemBubbleState(state, simulation);
	}

	public void RemoveDynamicThinkingElements()
	{
		if (thinkingGhostButton != null)
		{
			thinkingGhostButton.ClearClickEvents();
		}
		Action<SBGUIElement> action = delegate(SBGUIElement el)
		{
			if (el != null)
			{
				el.SetParent(null);
				el.SetActive(false);
				UnityEngine.Object.Destroy(el.gameObject);
			}
		};
		action(thinkingIcon);
		action(thinkingLabel);
		action(thinkingSkipLabel);
		action(thinkingSkipJjCounter);
		action(thinkingGhostButton);
	}

	public void DisplayThoughtItemBubbleState(string state, Simulation simulation)
	{
		if (thoughtItemBubbleDisplayController != null)
		{
			string displayState = thoughtItemBubbleDisplayController.GetDisplayState();
			thoughtItemBubbleDisplayController.DisplayState(state);
			if (state != null && displayState == null)
			{
				FirstAnimate(simulation);
				AnimateOtherControllers(simulation);
			}
		}
	}

	public void SetCostume(CostumeManager.Costume costume)
	{
		Paperdoll paperdoll = displayController as Paperdoll;
		if (paperdoll != null)
		{
			paperdoll.ApplyCostumeWithLOD(costume, entity.DefinitionId);
		}
	}

	public void AddPendingCommand(PendingCommand pc)
	{
		pendingCommands.Add(pc);
	}

	public void ClearPendingCommands()
	{
		pendingCommands.Clear();
	}

	public void SendPendingCommands(Simulation simulation)
	{
		foreach (PendingCommand pendingCommand in pendingCommands)
		{
			float? delay = pendingCommand.delay;
			if (!delay.HasValue)
			{
				simulation.Router.Send(pendingCommand.c);
				continue;
			}
			CommandRouter router = simulation.Router;
			Command c = pendingCommand.c;
			float? delay2 = pendingCommand.delay;
			router.Send(c, (ulong)delay2.Value);
		}
	}

	public float ComputeCircumscribedRadius()
	{
		float num = (Box.xmax - Box.xmin) / 2f;
		float num2 = (Box.ymax - Box.ymin) / 2f;
		return Mathf.Sqrt(num * num + num2 * num2);
	}

	private Vector2 ComputeRandomOffsetFromTarget(Simulated target)
	{
		if (target.entity is ResidentEntity)
		{
			float num = ComputeCircumscribedRadius();
			float num2 = target.ComputeCircumscribedRadius();
			return UnityEngine.Random.insideUnitCircle.normalized * (num + num2);
		}
		return Vector2.zero;
	}

	public void TeleportUnitToTargetIfNeeded(Identity targetId, Simulation simulation)
	{
		Simulated simulated = simulation.FindSimulated(targetId);
		if (simulated != null)
		{
			Vector2 vector = ComputeRandomOffsetFromTarget(simulated);
			Vector2 vector2 = simulated.PointOfInterest + vector;
			Position = vector2;
		}
	}

	public void EnableParticles(Simulation simulation, bool particlesEnabled)
	{
		if (particlesEnabled)
		{
			if (entity.Invariable.ContainsKey("fx.producing"))
			{
				ParticleSystemManager.Request request = (ParticleSystemManager.Request)entity.Invariable["fx.producing"];
				particlesRequest = simulation.particleSystemManager.RequestParticles(request.effectsName, request.initialPriority, request.subsequentPriority, request.cyclingPeriod, particleSystemRequestDelegate);
				if (particlesRequest != null && !particleDisplayOffsetWorld.HasValue)
				{
					particleDisplayOffsetWorld = CameraOffsetToWorldVector(particleDisplayOffsetScreen, simulation.TheCamera);
				}
			}
		}
		else if (particlesRequest != null)
		{
			simulation.particleSystemManager.RemoveParticleSystemRequest(particlesRequest);
			particlesRequest = null;
		}
	}

	public override string ToString()
	{
		return string.Concat("[Simulated (ID=", Id, ", name=", entity.Invariable["name"], ", type=", entity.AllTypes, ", DID=", entity.DefinitionId, ", State=", action, ")]");
	}

	private Vector3 CameraOffsetToWorldVector(Vector3 offset, Camera camera)
	{
		double num = (double)offset.x * 0.1302;
		double num2 = (double)offset.y * 0.1302;
		Vector3 right = camera.transform.right;
		Vector3 up = camera.transform.up;
		Vector3 vector = offset.z * camera.transform.forward;
		right *= (float)num;
		up *= (float)num2;
		return right + up + vector;
	}

	public void CalculateRushCompletionPercent(ulong endTime, ulong totalTime)
	{
		Variable[RUSH_PERCENT] = (float)(endTime - TFUtils.EpochTime()) / (float)totalTime;
	}

	public void AddSimulateOnce(string key, Action action)
	{
		Dictionary<string, Action> dictionary;
		if (Variable.ContainsKey("simulate_once"))
		{
			dictionary = (Dictionary<string, Action>)Variable["simulate_once"];
		}
		else
		{
			dictionary = new Dictionary<string, Action>();
			Variable["simulate_once"] = dictionary;
		}
		dictionary[key] = action;
	}

	public void ClearSimulateOnce()
	{
		if (Variable.ContainsKey("simulate_once"))
		{
			Variable.Remove("simulate_once");
		}
	}

	public void SimulateOnce()
	{
		if (!Variable.ContainsKey("simulate_once"))
		{
			return;
		}
		foreach (Action value in ((Dictionary<string, Action>)Variable["simulate_once"]).Values)
		{
			value();
		}
		Variable.Remove("simulate_once");
	}

	public void RemoveSimulateOnceAction(string key)
	{
		if (Variable.ContainsKey("simulate_once"))
		{
			Dictionary<string, Action> dictionary = (Dictionary<string, Action>)Variable["simulate_once"];
			if (dictionary.ContainsKey(key))
			{
				dictionary.Remove(key);
			}
		}
	}

	public void DisableInteractions()
	{
	}

	public void BillboardDelegate(Transform t, IDisplayController idc)
	{
		if (idc.isPerspectiveInArt)
		{
			SBCamera.BillboardDefinition(t, idc);
			return;
		}
		Vector3 vector = SBCamera.CameraUp();
		idc.BillboardScaling = Vector3.one;
		Vector3 rhs = new Vector3(0f - footprint.xmax, footprint.ymax, 0f);
		float x = 1f / Vector3.Dot(new Vector3(-1f, 1f, 0f).normalized, rhs.normalized);
		idc.BillboardScaling = new Vector3(x, 1f, 1f);
		t.LookAt(Vector3.Cross(vector, rhs), vector);
	}

	public void BlockerHighlight()
	{
		originalColor = displayController.Color;
		if (displayController.Transform.GetComponent<Renderer>() != null && displayController.Transform.GetComponent<Renderer>().material.mainTexture.name.StartsWith("FishHouse") && displayController.Transform.GetComponent<Renderer>().material.shader == Shader.Find("Custom/TwoImageColorOverlay"))
		{
			displayController.Transform.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/TransparentTint");
			displayController.Color *= BLOCKER_COLOR;
		}
		else
		{
			displayController.Color = BLOCKER_COLOR;
		}
	}

	public void ClearBlockerHighlight()
	{
		if (displayController.Transform.GetComponent<Renderer>() != null && displayController.Transform.GetComponent<Renderer>().material.mainTexture.name.StartsWith("FishHouse"))
		{
			displayController.Transform.GetComponent<Renderer>().material.shader = Shader.Find("Custom/TwoImageColorOverlay");
		}
		displayController.Color = originalColor;
	}

	public void SetDisplayOffsetWorld(Simulation simulation)
	{
		thoughtDisplayOffsetWorld = CameraOffsetToWorldVector(thoughtDisplayOffsetScreen, simulation.TheCamera);
	}
}
