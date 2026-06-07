using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Simulated
{
	public class Annex
	{
		public class ActiveState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
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
			}
		}

		public class RelayingState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
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
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class ShuntedCraftCyclingState : Building.CraftCyclingState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		public static ActiveState Active;

		public static RelayingState Relaying;

		public static ShuntedCraftingState ShuntedCrafting;

		public static ShuntedCraftCyclingState ShuntedCraftCycling;

		public static Simulated Extend(Simulated simulated, Simulation simulation)
		{
			return null;
		}

		private static void SanityCheck(Simulated simulated, Simulation simulation)
		{
		}
	}

	public class Building
	{
		public class PlacingAction : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulated simulated, Simulation simulation)
			{
			}

			private void RecordPlacement(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class PrimeErectingState : StateActionBuildingDefault
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
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public Vector3 Animate(Simulation simulation, Simulated simulated)
			{
				return default(Vector3);
			}
		}

		public class UpgradingState : StateActionBuildingDefault, Animated
		{
			public const string CLICK_DURATION_HANDLER = "clickDurationHandler";

			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			private void Rush(Session session, Simulated simulated)
			{
			}

			private void LogRush(Session session, Simulated simulated, Cost cost, bool canAfford)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public Vector3 Animate(Simulation simulation, Simulated simulated)
			{
				return default(Vector3);
			}
		}

		public class InactiveState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
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
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulated simulated, Simulation simulation)
			{
			}

			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
			}

			public void TryProduce(Simulation simulation, BuildingEntity building)
			{
			}
		}

		public class RequestingInterfaceState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
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
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
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
			}

			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
			}
		}

		public abstract class ActivatingBase : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
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
			}

			protected override List<Simulated> GetResidents(Simulation simulation, Simulated building)
			{
				return null;
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			protected override void UpdateBuildingState(Simulated simulated)
			{
			}

			protected override void RecordActions(Simulation simulation, Simulated simulated, List<Simulated> residents)
			{
			}

			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				return null;
			}
		}

		public class ReactivatingState : ActivatingBase
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			protected override List<Simulated> GetResidents(Simulation simulation, Simulated building)
			{
				return null;
			}

			protected override void UpdateBuildingState(Simulated simulated)
			{
			}

			protected override void RecordActions(Simulation simulation, Simulated simulated, List<Simulated> residents)
			{
			}
		}

		public class ProducingState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
			}

			private void Rush(Session session, Simulated simulated)
			{
			}

			private void LogRush(Session session, Simulated simulated, Cost cost, bool canAfford)
			{
			}
		}

		public class ProducedState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulation simulation, Simulated simulated)
			{
			}

			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
			}

			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
			}

			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				return null;
			}
		}

		public class CraftingState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class CraftedState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulation simulation, Simulated simulated)
			{
			}

			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
			}

			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				return null;
			}
		}

		public class CraftCyclingState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class CraftingCollectState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
			}

			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				return null;
			}
		}

		public class RushingBuildState : RushingSomething
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			protected override Cost GetRushCost(Simulation simulation, Simulated simulated)
			{
				return null;
			}
		}

		public class RushingProductState : RushingSomething
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			protected override Cost GetRushCost(Simulation simulation, Simulated simulated)
			{
				return null;
			}
		}

		public class RushingCraftState : RushingSomething
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void CancelCurrentCommands(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			protected override Cost GetRushCost(Simulation simulation, Simulated simulated)
			{
				return null;
			}
		}

		public class FriendsParkInactiveState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
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
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulation simulation, Simulated simulated)
			{
			}

			public override void UpdateControls(Simulation simulation, Simulated simulated)
			{
			}

			private void CollectAndRecordRewards(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class TaskFeedCollectingState : StateActionBuildingDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
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

		public static PlacingAction Placing;

		public static PrimeErectingState PrimeErecting;

		public static PrimeErectingStateFriend PrimeErectingFriend;

		public static ErectingState Erecting;

		public static UpgradingState Upgrading;

		public static InactiveState Inactive;

		public static ActiveState Active;

		public static RequestingInterfaceState RequestingInterface;

		public static ReflectingState Reflecting;

		public static ReplacingState Replacing;

		public static ActivatingState Activating;

		public static ReactivatingState Reactivating;

		public static ProducingState Producing;

		public static ProducedState Produced;

		public static CraftingState Crafting;

		public static CraftedState Crafted;

		public static CraftCyclingState CraftCycling;

		public static CraftingCollectState CraftingCollect;

		public static RushingBuildState RushingBuild;

		public static RushingProductState RushingProduct;

		public static RushingCraftState RushingCraft;

		public static FriendsParkInactiveState FriendParkInactive;

		public static TaskFeedState TaskFeed;

		public static TaskFeedCollectingState TaskFeedCollecting;

		public const string WORKER = "employee";

		public static Simulated Load(BuildingEntity buildingEntity, Simulation simulation, Vector2 position, bool flip, ulong utcNow)
		{
			return null;
		}

		public static void AdjustWorkerPosition(Simulated building, Simulation simulation)
		{
		}

		public static Simulated TryAddResident(Simulation simulation, Simulated building, int? residentDid, Identity existingResidentInstance = null)
		{
			return null;
		}

		public static List<Simulated> FindResidents(Simulation simulation, Simulated building)
		{
			return null;
		}

		public static void AddResidentToGameState(Dictionary<string, object> gameState, string residentId, int residentDid, string residenceId, ulong residentHungryTime)
		{
		}

		public static void RemoveResidentsFromGameState(Dictionary<string, object> gameState, string buildingId)
		{
		}
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
		}

		public override void Enter(Simulation simulation, Simulated simulated)
		{
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
				return default(Vector3);
			}
		}

		public virtual bool isVisible
		{
			get
			{
				return false;
			}
		}

		public ParticleSystemRequestDelegate(Simulated simulated)
		{
		}
	}

	public class RewardParticleRequestDelegate : ParticleSystemRequestDelegate
	{
		public override Vector3 Position
		{
			get
			{
				return default(Vector3);
			}
		}

		public RewardParticleRequestDelegate(Simulated theSimulated)
			: base(null)
		{
		}
	}

	public class ThoughtBubblePopParticleRequestDelegate : RewardParticleRequestDelegate
	{
		public override bool isVisible
		{
			get
			{
				return false;
			}
		}

		public ThoughtBubblePopParticleRequestDelegate(Simulated theSimulated)
			: base(null)
		{
		}
	}

	public class EatParticleRequestDelegate : RewardParticleRequestDelegate
	{
		public override Vector3 Position
		{
			get
			{
				return default(Vector3);
			}
		}

		public override bool isVisible
		{
			get
			{
				return false;
			}
		}

		public EatParticleRequestDelegate(Simulated simulated)
			: base(null)
		{
		}
	}

	public class ActivateParticleRequestDelegate : ParticleSystemRequestDelegate
	{
		public override Vector3 Position
		{
			get
			{
				return default(Vector3);
			}
		}

		public ActivateParticleRequestDelegate(Simulated theSimulated)
			: base(null)
		{
		}
	}

	public class SimulatedParticleRequestDelegate : ParticleSystemRequestDelegate
	{
		public bool isAssigned;

		public Vector3 offset;

		public override Vector3 Position
		{
			get
			{
				return default(Vector3);
			}
		}

		public SimulatedParticleRequestDelegate(Simulated theSimulated)
			: base(null)
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

	[StructLayout((LayoutKind)0, Size = 24)]
	public struct NamebarMixinArgs
	{
		public bool m_bHasNamebar;

		public string m_sName;

		public bool m_bCheckForTaskCharacters;
	}

	public class Debris
	{
		public class UnpurchasedState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class InactiveState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class ClearingState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class ClearingMoreInfoState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public ulong CalculateRemainingTime(Simulated simulated)
			{
				return 0uL;
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class PrimingRushState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			private void LogRush(Session session, Simulated simulated, Cost cost, bool canAfford)
			{
			}
		}

		public class RushingClearingState : RushingSomething
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			protected override Cost GetRushCost(Simulation simulation, Simulated simulated)
			{
				return null;
			}
		}

		public class DeletingState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulation simulation, Simulated simulated)
			{
			}

			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
			}

			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				return null;
			}
		}

		public class DeletedState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public delegate void Setup(Simulation simulation, Simulated simulated);

		public static UnpurchasedState Unpurchased;

		public static InactiveState Inactive;

		public static ClearingState Clearing;

		public static ClearingMoreInfoState ClearingMoreInfo;

		public static PrimingRushState PrimingRush;

		public static RushingClearingState RushingClearing;

		public static DeletingState Deleting;

		public static DeletedState Deleted;

		public static Simulated Load(DebrisEntity debrisEntity, Simulation simulation, Vector2 position, ulong utcNow)
		{
			return null;
		}
	}

	public class Disabled
	{
		public class DisabledState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public static DisabledState Disable;
	}

	public abstract class FollowingPath
	{
		private const float TOL = 10f;

		private const float TOLSQ = 100f;

		private const int PATHING_BUDGET = 200;

		public bool PathFound(Simulation simulation, Simulated simulated)
		{
			return false;
		}

		public void FindPath(Simulation simulation, Simulated simulated)
		{
		}

		public bool FollowPath(Simulation simulation, Simulated simulated)
		{
			return false;
		}

		public bool FollowPathSimulate(Simulation simulation, Simulated simulated)
		{
			return false;
		}

		public static void GetWaypointPath(Simulation simulation, Simulated simulated)
		{
		}

		public void RandomWanderSimulate(Simulation simulation, Simulated simulated)
		{
		}

		private static float GetMovespeedVariance()
		{
			return 0f;
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
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulated simulated, Simulation simulation)
			{
			}
		}

		public class InactiveState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulated simulated, Simulation simulation)
			{
			}
		}

		public class ActiveState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public static void Setup(Simulated simulated, Simulation simulation)
			{
			}
		}

		public delegate void Setup(Simulated simulated, Simulation simulation);

		public static UnpurchasedState Unpurchased;

		public static InactiveState Inactive;

		public static ActiveState Active;

		public static Simulated Load(LandmarkEntity landmarkEntity, Simulation simulation, Vector2 position, ulong utcNow)
		{
			return null;
		}
	}

	public class Resident
	{
		public class IdleState : StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
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
			}
		}

		public class IdleFullState : IdleState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class IdleWishingState : IdleState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class MovingState : FollowingPath, StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
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
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class StoreResidentState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class ResidingState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class WanderingFullState : FollowingPath, StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			protected ulong CalculateRemainingFullnessTime(Simulated simulated)
			{
				return 0uL;
			}
		}

		public class WanderingHungryState : FollowingPath, StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public abstract class WishingForSomething : FollowingPath, StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class WishingForFoodState : WishingForSomething
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class TemptedState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			private void AttachLabelToThoughtBubbleBone(Simulated simulated, IDisplayController target, string text)
			{
			}

			private void AttachProductIconToThoughtBubbleBone(Simulated simulated, IDisplayController target, string textureOverride)
			{
			}

			private void AttachHelper(IDisplayController controller, string target, SBGUIElement element)
			{
			}
		}

		public class NotTemptedState : TransitionallyAnimating
		{
			protected override string DisplayStateName
			{
				get
				{
					return null;
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
					return 0;
				}
			}

			public override void Enter(Simulation simulation, Simulated simulated)
			{
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
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			private void LogRush(Session session, Simulated simulated, Cost cost, bool canAfford)
			{
			}
		}

		public class RushingFullnessState : RushingSomething
		{
			public override void CancelCurrentCommands(Simulation simulation, Simulated simulated)
			{
			}

			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			protected override Cost GetRushCost(Simulation simulation, Simulated simulated)
			{
				return null;
			}
		}

		public class TryEatState : StateAction
		{
			private const string REQUEST_ERROR_PULSE = "RequestOpenInventory";

			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			private void GenerateAndRecordBonusEarned(uint fedProductId, Simulation simulation, Simulated simulated)
			{
			}
		}

		public class WaitingForDeliveryState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
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
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			protected abstract string GetDisplayThoughtMaterial(Simulation simulation, Simulated simulated);
		}

		public class CheeringState : TransitionallyAnimating
		{
			protected override string DisplayStateName
			{
				get
				{
					return null;
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
					return 0;
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
					return null;
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
					return 0;
				}
			}

			protected override string GetDisplayThoughtMaterial(Simulation simulation, Simulated simulated)
			{
				return null;
			}

			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			private void SpawnAndRecordRewards(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class TryBonusSpinState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class WaitingForCollectBonusState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			private void CollectAndRecordRewards(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class CheeringAfterBonusState : CheeringState
		{
			protected override string DisplayThoughtStateName
			{
				get
				{
					return null;
				}
			}
		}

		public class StartingWanderCycleState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class RequestingInterfaceState : StateAction
		{
			private bool bComplete;

			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
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
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
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
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			protected virtual void ShowNewHungerResource(Simulation simulation, Simulated simulated, int nDID)
			{
			}
		}

		public class TaskIdleState : TaskUpdateState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class TaskWanderState : TaskUpdateState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class TaskMovingState : TaskUpdateState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			private void ReachedTarget(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class TaskEnterState : TaskUpdateState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
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
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			protected override void ShowNewHungerResource(Simulation simulation, Simulated simulated, int nDID)
			{
			}

			private void GenerateAndRecordBonusEarned(uint fedProductId, Simulation simulation, Simulated simulated)
			{
			}

			private void SpawnAndRecordRewards(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class TaskStandState : TaskUpdateState
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}
		}

		public class TaskCollectRewardState : FollowingPath, StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			private void CollectAndRecordRewards(Simulation simulation, Simulated simulated)
			{
			}

			private void GenerateAndRecordTaskBonusEarned(int taskDID, Task pTask, Simulation simulation, Simulated simulated)
			{
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

		public static IdleState Idle;

		public static IdleFullState IdleFull;

		public static IdleWishingState IdleWishing;

		public static MovingState Moving;

		public static GoHomeState GoHome;

		public static StoreResidentState StoreResident;

		public static ResidingState Residing;

		public static WanderingFullState WanderingFull;

		public static WanderingHungryState WanderingHungry;

		public static WishingForFoodState WishingForFood;

		public static TemptedState Tempted;

		public static NotTemptedState NotTempted;

		public static PrimingRushFullnessState PrimingRushFullness;

		public static RushingFullnessState RushingFullness;

		public static TryEatState TryEat;

		public static WaitingForDeliveryState WaitingForDelivery;

		public static CheeringState Cheering;

		public static EatingState Eating;

		public static TryBonusSpinState TryBonusSpin;

		public static WaitingForCollectBonusState WaitingForCollectBonus;

		public static CheeringAfterBonusState CheeringAfterBonus;

		public static StartingWanderCycleState StartingWanderCycle;

		public static RequestingInterfaceState RequestingInterface;

		public static ReflectingState Reflecting;

		public static TaskDelegatingState TaskDelegating;

		public static TaskIdleState TaskIdle;

		public static TaskWanderState TaskWander;

		public static TaskMovingState TaskMoving;

		public static TaskEnterState TaskEnter;

		public static TaskEnterFeedState TaskEnterFeed;

		public static TaskStandState TaskStand;

		public static TaskCollectRewardState TaskCollectReward;

		public static TaskCheerAfterCollectState TaskCheerAfterCollect;

		public static Simulated Load(ResidentEntity residentEntity, Identity residenceId, ulong? wishExpiresAt, int? hungerId, int? prevHungerId, ulong nextHungerTime, ulong? fullnessLength, Reward matchBonus, Simulation simulation, ulong utcNow)
		{
			return null;
		}

		private static void SanityChecks(ResidentEntity residentEntity, Game game)
		{
		}

		private static void WishTableSanityCheck(ResidentEntity pResidentEntity, CdfDictionary<int> pWishTable, CostumeManager.Costume pCostume)
		{
		}

		private static void StartHungerTimer(ResidentEntity resident, Simulation simulation)
		{
		}

		public static void RefreshModifiedDisplayState(Simulated simulated)
		{
		}

		private static int? GenerateHungerResourceID(Simulation pSimulation, ResidentEntity pEntity)
		{
			return null;
		}
	}

	public class Treasure
	{
		public class BuriedState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class UncoveringState : StateAction, Animated
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public Vector3 Animate(Simulation simulation, Simulated simulated)
			{
				return default(Vector3);
			}
		}

		public class ClaimingState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
			}

			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				return null;
			}
		}

		public class DeletingState : StateActionDefault
		{
			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class ClaimingStateFriend : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			public int CheckHasValue(SoaringDictionary tobeChecked, string key)
			{
				return 0;
			}

			private void SpawnDrops(Simulation simulation, Simulated simulated)
			{
			}

			private Reward GetReward(Simulation simulation, Simulated simulated)
			{
				return null;
			}
		}

		public class BuriedStateFriend : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public static BuriedState Buried;

		public static UncoveringState Uncovering;

		public static ClaimingState Claiming;

		public static DeletingState Deleting;

		public static ClaimingStateFriend Claiming_Friend;

		public static BuriedStateFriend Buried_Friend;

		public static Simulated Load(TreasureEntity treasureEntity, Simulation simulation, Vector2 position, ulong utcNow)
		{
			return null;
		}
	}

	public class Wanderer
	{
		public class SpawnState : StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class HiddenState : StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class IdleState : StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class WanderingState : FollowingPath, StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public virtual bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class ClickedState : StateAction
		{
			public virtual void Enter(Simulation simulation, Simulated simulated)
			{
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
			}

			public virtual void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}

			protected override float GetSpeedAddition(Simulated simulated)
			{
				return 0f;
			}
		}

		public abstract class TransitionallyAnimating : StateActionDefault
		{
			protected abstract string DisplayStateName { get; }

			protected abstract string DisplayThoughtStateName { get; }

			protected abstract int AnimationLength { get; }

			public override void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public override void Leave(Simulation simulation, Simulated simulated)
			{
			}

			protected abstract string GetDisplayThoughtMaterial(Simulation simulation, Simulated simulated);
		}

		public class CheeringState : TransitionallyAnimating
		{
			protected override string DisplayStateName
			{
				get
				{
					return null;
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
					return 0;
				}
			}

			protected override string GetDisplayThoughtMaterial(Simulation simulation, Simulated simulated)
			{
				return null;
			}
		}

		public static SpawnState Spawn;

		public static HiddenState Hidden;

		public static IdleState Idle;

		public static WanderingState Wandering;

		public static ClickedState Clicked;

		public static FleeingState Fleeing;

		public static CheeringState Cheering;

		public static Simulated Load(ResidentEntity residentEntity, ulong? hideExpiresAt, bool? disableFlee, Simulation simulation, ulong utcNow)
		{
			return null;
		}

		public static void AddWandererToGameState(Dictionary<string, object> gameState, string wandererId, int wandererDid)
		{
		}
	}

	public class Worker
	{
		public class IdleState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
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
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class ReturningState : FollowingPath, StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public class ErectingState : StateAction
		{
			public void Enter(Simulation simulation, Simulated simulated)
			{
			}

			public void Leave(Simulation simulation, Simulated simulated)
			{
			}

			public bool Simulate(Simulation simulation, Simulated simulated, Session session)
			{
				return false;
			}
		}

		public static IdleState Idle;

		public static MovingState Moving;

		public static ReturningState Returning;

		public static ErectingState Erecting;
	}

	public const bool DEBUG_LOG_STATEMACHINES = false;

	public Entity entity;

	public int? forcedWish;

	public bool showUnavailableIcon;

	public RushParameters rushParameters;

	private string mStateModifierString;

	public SimulatedFlags simFlags;

	private List<Action> clickListeners;

	private static readonly List<StateAction> prioritizedActions;

	public const int TEMPTABLE_THRESHOLD = 0;

	private static readonly List<StateAction> priorityOrder;

	public static readonly Color COLOR_FOOTPRINT_FREE;

	public static readonly Color COLOR_FOOTPRINT_BLOCKED;

	public static readonly Color COLOR_STANDARD;

	public static readonly Color COLOR_DRAGGING;

	public static readonly string RUSH_PERCENT;

	public ParticleSystemRequestDelegate particleSystemRequestDelegate;

	public ParticleSystemRequestDelegate rewardParticleSystemRequestDelegate;

	public ThoughtBubblePopParticleRequestDelegate thoughtBubblePopParticleRequestDelegate;

	public EatParticleRequestDelegate eatParticleRequestDelegate;

	public ActivateParticleRequestDelegate activateParticleSystemRequestDelegate;

	public SimulatedParticleRequestDelegate starsParticleSystemRequestDelegate;

	public SimulatedParticleRequestDelegate dustParticleSystemRequestDelegate;

	public TimebarMixinArgs timebarMixinArgs;

	private const string TIMEBAR_RUNNING = "timebar_running";

	public NamebarMixinArgs m_pNamebarMixinArgs;

	private Vector2[] position;

	private Vector2 snapPosition;

	private Vector2 pointOfInterestOffset;

	private bool workerSpawner;

	private bool isWaypoint;

	private bool simulatedQueryable;

	private AlignedBox footprint;

	private AlignedBox box;

	public AlignedBox snapBox;

	public AlignedBox prevSceneBox;

	private Queue<Command> commands;

	private Command command;

	protected TriggerableMixin triggerable;

	private StateMachine<StateAction, Command.TYPE> machine;

	private StateAction action;

	private Dictionary<StateAction, Queue<Command>> delegatedCommands;

	private bool visible;

	public const string REQUEST_RUSH = "request_rush_sim";

	public const string IGNORE_REQUEST_RUSH = "ignore_request_rush_sim";

	private Vector3 thoughtDisplayOffsetScreen;

	private Vector3? thoughtDisplayOffsetWorld;

	private Dictionary<string, Vector3> thoughtDisplayScreenOffsets;

	private Vector3 thoughtMaskDisplayOffsetScreen;

	private Vector3? thoughtMaskDisplayOffsetWorld;

	private PeriodicPattern periodicMovement;

	private PeriodicPattern thoughtItemBubbleScalingMajor;

	private PeriodicPattern thoughtItemBubbleScalingMinor;

	private IDisplayController thoughtDisplayController;

	private IDisplayController thoughtMaskDisplayController;

	private IDisplayController thoughtItemBubbleDisplayController;

	private Vector3? thoughtItemBubbleDisplayOffsetWorld;

	private Vector3 thoughtItemBubbleDisplayOffsetScreen;

	private SBGUIShadowedLabel thinkingLabel;

	private SBGUIShadowedLabel thinkingSkipLabel;

	private SBGUIShadowedLabel thinkingSkipJjCounter;

	private SBGUIAtlasImage thinkingIcon;

	private SBGUIButton thinkingGhostButton;

	private Vector3 displayOffsetScreen;

	private Vector3? displayOffsetWorld;

	private Vector3 textureOriginScreen;

	private Vector3? textureOriginWorld;

	private IDisplayController displayController;

	private const string DC_EXT_NONE = "";

	private const string DC_EXT_FLIP = ".flip";

	private string displayControllerExtension;

	private bool displayControllerFlipped;

	private IDisplayController footprintDisplayController;

	private static IDisplayController footprintDisplayControllerShared;

	private IDisplayController dropShadowDisplayController;

	private InteractionState interactionState;

	private int selectionPriorityBaggage;

	private List<PendingCommand> pendingCommands;

	private ParticleSystemManager.Request particlesRequest;

	private Vector3 particleDisplayOffsetScreen;

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

	private const string SIMULATE_ONCE = "simulate_once";

	public const string SHOW_TIMEBAR = "show_timebar";

	public const string SHOW_NAMEBAR = "show_namebar";

	public const string ENABLE_PARTICLES = "enable_particles";

	private bool calledBounceStart;

	private Color originalColor;

	private readonly Color BLOCKER_COLOR;

	private bool swarmManaged;

	public Reward taskBonusReward;

	private const int LOCK_WISH_DELAY = 60;

	public Identity Id
	{
		get
		{
			return null;
		}
	}

	public string Description
	{
		get
		{
			return null;
		}
	}

	public bool Visible
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool DebugHitBoxesVisible
	{
		set
		{
		}
	}

	public bool SimulatedQueryable
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool DebugFootprintsVisible
	{
		set
		{
		}
	}

	public float Alpha
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public Color Color
	{
		get
		{
			return default(Color);
		}
		set
		{
		}
	}

	private float Width
	{
		get
		{
			return 0f;
		}
	}

	private float Height
	{
		get
		{
			return 0f;
		}
	}

	public IDisplayController DisplayController
	{
		get
		{
			return null;
		}
	}

	public SBGUIShadowedLabel DynamicThinkingLabel
	{
		get
		{
			return null;
		}
	}

	public SBGUIShadowedLabel DynamicThinkingSkipLabel
	{
		get
		{
			return null;
		}
	}

	public SBGUIShadowedLabel DynamicThinkingSkipJjCounter
	{
		get
		{
			return null;
		}
	}

	public SBGUIAtlasImage DynamicThinkingIcon
	{
		get
		{
			return null;
		}
	}

	public SBGUIButton ThinkingGhostButton
	{
		get
		{
			return null;
		}
	}

	public bool FootprintVisible
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public Color FootprintColor
	{
		set
		{
		}
	}

	private string StateModifierString
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public Vector2 Position
	{
		get
		{
			return default(Vector2);
		}
		set
		{
		}
	}

	public Vector2 PositionCenter
	{
		get
		{
			return default(Vector2);
		}
	}

	public Vector2 SnapPosition
	{
		get
		{
			return default(Vector2);
		}
		set
		{
		}
	}

	public Vector2 ThoughtDisplayOffsetScreen
	{
		get
		{
			return default(Vector2);
		}
	}

	public Vector3 ThoughtDisplayOffsetWorld
	{
		get
		{
			return default(Vector3);
		}
	}

	public Vector3 DisplayOffsetWorld
	{
		get
		{
			return default(Vector3);
		}
	}

	public Vector3 TextureOriginWorld
	{
		get
		{
			return default(Vector3);
		}
	}

	public Vector2 ThoughtMaskDisplayOffsetScreen
	{
		get
		{
			return default(Vector2);
		}
	}

	public Vector3 ThoughtMaskDisplayOffsetWorld
	{
		get
		{
			return default(Vector3);
		}
	}

	public IDisplayController ThoughtDisplayController
	{
		get
		{
			return null;
		}
	}

	public IDisplayController ThoughtMaskDisplayController
	{
		get
		{
			return null;
		}
	}

	public Vector2 PointOfInterest
	{
		get
		{
			return default(Vector2);
		}
	}

	public bool WorkerSpawner
	{
		get
		{
			return false;
		}
	}

	public bool IsWaypoint
	{
		get
		{
			return false;
		}
	}

	public AlignedBox Box
	{
		get
		{
			return null;
		}
	}

	public AlignedBox SnapBox
	{
		get
		{
			return null;
		}
	}

	public AlignedBox Footprint
	{
		get
		{
			return null;
		}
	}

	public InteractionState InteractionState
	{
		get
		{
			return null;
		}
	}

	public List<Action> ClickListeners
	{
		get
		{
			return null;
		}
	}

	public int SelectionPriority
	{
		get
		{
			return 0;
		}
	}

	public int SelectionPriorityBaggage
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public int TemptationPriority
	{
		get
		{
			return 0;
		}
	}

	public bool Flip
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public ReadOnlyIndexer Invariable
	{
		get
		{
			return null;
		}
	}

	public ReadWriteIndexer Variable
	{
		get
		{
			return null;
		}
	}

	public bool IsSwarmManaged
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public Entity Entity
	{
		get
		{
			return null;
		}
	}

	public StateAction Action
	{
		get
		{
			return null;
		}
	}

	public bool UseFootprintIntersection
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public Simulated(Simulation simulation, Entity entity, Vector2 position)
	{
	}

	private void UpdateAlignedBox()
	{
	}

	public string UseStateModifierString(string state)
	{
		return null;
	}

	private void UpdateDebugFootprint()
	{
	}

	public void AddClickListener(Action handler)
	{
	}

	public bool RemoveClickListener(Action handler)
	{
		return false;
	}

	protected bool IntersectsFootprint(Ray ray)
	{
		return false;
	}

	public bool Intersects(Ray ray)
	{
		return false;
	}

	public void LoadInitialState(StateAction action)
	{
	}

	public void EnterInitialState(StateAction action, Simulation simulation)
	{
	}

	public void Push(Command command)
	{
	}

	public bool HasEntity<T>() where T : EntityDecorator
	{
		return false;
	}

	public T GetEntity<T>() where T : EntityDecorator
	{
		return null;
	}

	public void SetFootprint(Simulation simulation, bool enable = true)
	{
	}

	public void Warp(Vector2 position, Simulation simulation = null)
	{
	}

	public void FlipWarp(Simulation simulation = null)
	{
	}

	public void AddScaffolding(Simulation simulation)
	{
	}

	public void RemoveScaffolding(Simulation simulation)
	{
	}

	public void AddFence(Simulation simulation)
	{
	}

	public void RemoveFence(Simulation simulation)
	{
	}

	public bool Simulate(Simulation simulation, Session session)
	{
		return false;
	}

	public void UpdateControls(Simulation simulation)
	{
	}

	public void DestroyDisplayControllers()
	{
	}

	public void Destroy(Simulation simulation)
	{
	}

	public void FirstAnimate(Simulation simulation)
	{
	}

	public void EnableAnimateAction(bool enable)
	{
	}

	public void Animate(Simulation simulation)
	{
	}

	public void Bounce()
	{
	}

	public void BounceStart()
	{
	}

	public void BounceEnd()
	{
	}

	public void BounceCleanup()
	{
	}

	public void AnimateBounce(Simulation simulation)
	{
	}

	public void AnimateBounceStart(Simulation simulation)
	{
	}

	public void AnimateBounceEnd(Simulation simulation)
	{
	}

	public void AnimateScaleAndFlip(Vector3 scale)
	{
	}

	public void AnimateDebugHitBox(Simulation simulation)
	{
	}

	public void AnimateOtherControllers(Simulation simulation)
	{
	}

	public void DisplayState(string state)
	{
	}

	public string GetDisplayState()
	{
		return null;
	}

	public void DisplayThoughtState(string state, Simulation simulation)
	{
	}

	public void DisplayThoughtState(string overrideSubjectMaterial, string state, Simulation simulation)
	{
	}

	public void RemoveDynamicThinkingElements()
	{
	}

	public void DisplayThoughtItemBubbleState(string state, Simulation simulation)
	{
	}

	public void SetCostume(CostumeManager.Costume costume)
	{
	}

	public void AddPendingCommand(PendingCommand pc)
	{
	}

	public void ClearPendingCommands()
	{
	}

	public void SendPendingCommands(Simulation simulation)
	{
	}

	public float ComputeCircumscribedRadius()
	{
		return 0f;
	}

	private Vector2 ComputeRandomOffsetFromTarget(Simulated target)
	{
		return default(Vector2);
	}

	public void TeleportUnitToTargetIfNeeded(Identity targetId, Simulation simulation)
	{
	}

	public void EnableParticles(Simulation simulation, bool particlesEnabled)
	{
	}

	public override string ToString()
	{
		return null;
	}

	private Vector3 CameraOffsetToWorldVector(Vector3 offset, Camera camera)
	{
		return default(Vector3);
	}

	public void CalculateRushCompletionPercent(ulong endTime, ulong totalTime)
	{
	}

	public void AddSimulateOnce(string key, Action action)
	{
	}

	public void ClearSimulateOnce()
	{
	}

	public void SimulateOnce()
	{
	}

	public void RemoveSimulateOnceAction(string key)
	{
	}

	public void DisableInteractions()
	{
	}

	public void BillboardDelegate(Transform t, IDisplayController idc)
	{
	}

	public void BlockerHighlight()
	{
	}

	public void ClearBlockerHighlight()
	{
	}

	public void SetDisplayOffsetWorld(Simulation simulation)
	{
	}

	public void ClearPathInfo()
	{
	}
}
