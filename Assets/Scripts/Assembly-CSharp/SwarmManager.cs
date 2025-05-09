using System.Collections.Generic;
using UnityEngine;

public class SwarmManager
{
	public const int LOW_MEM_SOFT_MIN = 0;

	public const int LOW_MEM_SOFT_MAX = 1000;

	public const int HIGH_MEM_SOFT_MIN = 0;

	public const int HIGH_MEM_SOFT_MAX = 1000;

	private const float SHUFFLE_INTERVAL_MIN = 10f;

	private const float SHUFFLE_INTERVAL_MAX = 30f;

	private static SwarmManager _instance;

	private float nextShuffle;

	private List<ResidentEntity> outsideList;

	private List<ResidentEntity> transitionList;

	private Dictionary<Simulated, Identity> inHomeList;

	private int minSoftResidents;

	private int maxSoftResidents;

	public static SwarmManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new SwarmManager();
			}
			return _instance;
		}
	}

	public SwarmManager()
	{
		outsideList = new List<ResidentEntity>();
		transitionList = new List<ResidentEntity>();
		inHomeList = new Dictionary<Simulated, Identity>();
		if (CommonUtils.TextureLod() < CommonUtils.LevelOfDetail.Standard)
		{
			minSoftResidents = 0;
			maxSoftResidents = 1000;
		}
		else
		{
			minSoftResidents = 0;
			maxSoftResidents = 1000;
		}
	}

	public void Cleanup()
	{
		outsideList.Clear();
		transitionList.Clear();
		inHomeList.Clear();
		_instance = null;
	}

	public void AddResident(ResidentEntity entity)
	{
		if (entity == null)
		{
			TFUtils.WarningLog("Tried to add a null resident entity to swarm manager.");
		}
		else if (!outsideList.Contains(entity))
		{
			outsideList.Add(entity);
		}
	}

	public void RemoveResident(ResidentEntity entity, Simulated building)
	{
		if (entity == null)
		{
			TFUtils.WarningLog("Tried to remove a null resident entity to swarm manager.");
			return;
		}
		if (outsideList.Contains(entity))
		{
			outsideList.Remove(entity);
		}
		if (transitionList.Contains(entity))
		{
			transitionList.Remove(entity);
		}
		if (building != null && inHomeList.ContainsKey(building))
		{
			inHomeList.Remove(building);
		}
	}

	public void SwitchResident(ResidentEntity entity)
	{
		if (entity == null)
		{
			TFUtils.WarningLog("Tried to switch a null resident entity to swarm manager.");
			return;
		}
		if (outsideList.Contains(entity))
		{
			outsideList.Remove(entity);
		}
		if (!transitionList.Contains(entity))
		{
			transitionList.Add(entity);
		}
	}

	public void RestoreResidents(Simulation simulation, Simulated building)
	{
		if (!inHomeList.ContainsKey(building))
		{
			return;
		}
		List<int> residentDids = building.GetEntity<BuildingEntity>().ResidentDids;
		foreach (int item in residentDids)
		{
			Simulated simulated = Simulated.Building.TryAddResident(simulation, building, item);
			if (simulated == null)
			{
				TFUtils.ErrorLog("SwarmManager.RestoreResident - Failed to retrieve resident from building");
			}
			if (inHomeList.ContainsKey(building))
			{
				inHomeList.Remove(building);
			}
		}
	}

	private void MoveResidentOutside(Simulation simulation)
	{
	}

	private List<ResidentEntity> GetValidOutsideList()
	{
		List<ResidentEntity> list = new List<ResidentEntity>();
		foreach (ResidentEntity outside in outsideList)
		{
			list.Add(outside);
		}
		return list;
	}

	private void MoveResidentInside(Simulation simulation)
	{
		ResidentEntity residentEntity = null;
		List<ResidentEntity> validOutsideList = GetValidOutsideList();
		while (residentEntity == null && validOutsideList.Count > 0)
		{
			int index = Random.Range(0, validOutsideList.Count);
			residentEntity = validOutsideList[index];
			if (!residentEntity.HomeAvailability)
			{
				validOutsideList.Remove(residentEntity);
				residentEntity = null;
			}
		}
		if (residentEntity != null)
		{
			SwitchResident(residentEntity);
			Simulated simulated = simulation.FindSimulated(residentEntity.Residence);
			if (simulated == null)
			{
				TFUtils.ErrorLog("Failed to find residents home");
			}
			simulation.FindSimulated(residentEntity.Id).ClearPendingCommands();
			simulation.Router.Send(GoHomeCommand.Create(residentEntity.Id, residentEntity.Id, simulated.PointOfInterest));
		}
	}

	public void StoreResident(Simulation simulation, ResidentEntity entity)
	{
		if (entity == null)
		{
			TFUtils.ErrorLog("Tried to store a null resident in swarm manager");
		}
		if (outsideList.Contains(entity))
		{
			outsideList.Remove(entity);
		}
		if (transitionList.Contains(entity))
		{
			transitionList.Remove(entity);
		}
		Simulated key = simulation.FindSimulated(entity.Residence);
		inHomeList.Add(key, entity.Id);
		ulong hungryAt = entity.HungryAt;
		ulong num = TFUtils.EpochTime();
		ulong hungryAt2 = hungryAt - num;
		entity.HungryAt = hungryAt2;
		Simulated simulated = simulation.FindSimulated(entity.Id);
		simulation.RemoveSimulated(simulated);
	}

	public void ResidentInIdle(Simulation simulation, ResidentEntity entity)
	{
		Simulated simulated = simulation.FindSimulated(entity.Residence);
		if (simulated == null)
		{
			TFUtils.ErrorLog("Failed to find residents home");
		}
		simulation.FindSimulated(entity.Id).ClearPendingCommands();
		simulation.Router.Send(GoHomeCommand.Create(entity.Id, entity.Id, simulated.PointOfInterest));
	}

	public void OnUpdate(Simulation simulation, float dT)
	{
		nextShuffle -= dT;
		if (nextShuffle <= 0f)
		{
			ShuffleResident(simulation);
			nextShuffle = dT + Random.Range(10f, 30f);
		}
		if (outsideList.Count < minSoftResidents)
		{
			MoveResidentOutside(simulation);
		}
		else if (outsideList.Count > maxSoftResidents)
		{
			MoveResidentInside(simulation);
		}
		if (transitionList.Count <= 0)
		{
			return;
		}
		foreach (ResidentEntity transition in transitionList)
		{
			if (transition.Invariable["action"] is Simulated.Resident.IdleState)
			{
				ResidentInIdle(simulation, transition);
			}
		}
	}

	private void ShuffleResident(Simulation simulation)
	{
	}
}
