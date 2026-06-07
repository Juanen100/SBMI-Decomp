using System.Collections.Generic;

public class SwarmManager
{
	private static SwarmManager _instance;

	public const int LOW_MEM_SOFT_MIN = 0;

	public const int LOW_MEM_SOFT_MAX = 1000;

	public const int HIGH_MEM_SOFT_MIN = 0;

	public const int HIGH_MEM_SOFT_MAX = 1000;

	private const float SHUFFLE_INTERVAL_MIN = 10f;

	private const float SHUFFLE_INTERVAL_MAX = 30f;

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
			return null;
		}
	}

	public void Cleanup()
	{
	}

	public void AddResident(ResidentEntity entity)
	{
	}

	public void RemoveResident(ResidentEntity entity, Simulated building)
	{
	}

	public void SwitchResident(ResidentEntity entity)
	{
	}

	public void RestoreResidents(Simulation simulation, Simulated building)
	{
	}

	private void MoveResidentOutside(Simulation simulation)
	{
	}

	private List<ResidentEntity> GetValidOutsideList()
	{
		return null;
	}

	private void MoveResidentInside(Simulation simulation)
	{
	}

	public void StoreResident(Simulation simulation, ResidentEntity entity)
	{
	}

	public void ResidentInIdle(Simulation simulation, ResidentEntity entity)
	{
	}

	public void OnUpdate(Simulation simulation, float dT)
	{
	}

	private void ShuffleResident(Simulation simulation)
	{
	}
}
