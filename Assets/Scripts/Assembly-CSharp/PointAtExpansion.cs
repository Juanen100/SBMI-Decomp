using System.Collections.Generic;
using UnityEngine;

public class PointAtExpansion : SimulationSessionActionDefinition
{
	public class SimulationExpansionPointer : SimulationPointer
	{
		public override Vector3 TargetPosition
		{
			get
			{
				return default(Vector3);
			}
		}

		public void Spawn(Game game, SessionActionTracker parentAction, TerrainSlot target)
		{
		}
	}

	public const string TYPE = "point_at_expansion";

	private SimulationExpansionPointer pointer;

	private const string SLOT_ID = "slot_id";

	private int targetDid;

	private bool restrict;

	private bool restricted;

	private PointAtExpansion()
	{
	}

	public static PointAtExpansion Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
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

	public void RestrictExpansion(Simulation simulation)
	{
	}

	public override void OnDestroy(Game game)
	{
	}
}
