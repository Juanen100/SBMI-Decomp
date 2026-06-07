using System.Collections.Generic;

public class PointAtSimulation : SimulationSessionActionDefinition
{
	public class SimulationLocationPointer : SimulationPointer
	{
		public void Spawn(Game game, SessionActionTracker parentAction)
		{
		}
	}

	public const string TYPE = "point_at_simulation";

	private SimulationLocationPointer pointer;

	private PointAtSimulation()
	{
	}

	public static PointAtSimulation Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		return null;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public void SpawnPointer(Session session, SessionActionTracker tracker)
	{
	}
}
