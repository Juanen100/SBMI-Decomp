using System.Collections.Generic;
using UnityEngine;

public class PointAtSimulation : SimulationSessionActionDefinition
{
	public class SimulationLocationPointer : SimulationPointer
	{
		public void Spawn(Game game, SessionActionTracker parentAction)
		{
			SimulationLocationPointer simulationLocationPointer = new SimulationLocationPointer();
			simulationLocationPointer.Initialize(game, parentAction, offset, base.Alpha, base.Scale);
		}
	}

	public const string TYPE = "point_at_simulation";

	private SimulationLocationPointer pointer = new SimulationLocationPointer();

	private PointAtSimulation()
	{
	}

	public static PointAtSimulation Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		PointAtSimulation pointAtSimulation = new PointAtSimulation();
		pointAtSimulation.Parse(data, id, startConditions, originatedFromQuest);
		return pointAtSimulation;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		pointer.Parse(data, true, Vector3.zero, 1f);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dict = base.ToDict();
		pointer.AddToDict(ref dict);
		return dict;
	}

	public void SpawnPointer(Session session, SessionActionTracker tracker)
	{
		pointer.Spawn(session.TheGame, tracker);
		FrameCamera(session.TheCamera, pointer.TargetPosition);
	}
}
