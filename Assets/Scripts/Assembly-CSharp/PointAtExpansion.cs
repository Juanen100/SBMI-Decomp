#define ASSERTS_ON
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
				TFUtils.Assert(slot != null, "Trying to target a slot, but slot target is null!");
				return TFUtils.ExpandVector(slot.Position);
			}
		}

		public void Spawn(Game game, SessionActionTracker parentAction, TerrainSlot target)
		{
			TFUtils.Assert(target != null, "Must specify a target slot.");
			SimulationExpansionPointer simulationExpansionPointer = new SimulationExpansionPointer();
			simulationExpansionPointer.Initialize(game, parentAction, offset, base.Alpha, base.Scale, target);
		}
	}

	public const string TYPE = "point_at_expansion";

	private const string SLOT_ID = "slot_id";

	private SimulationExpansionPointer pointer = new SimulationExpansionPointer();

	private int targetDid;

	private bool restrict;

	private bool restricted;

	private PointAtExpansion()
	{
	}

	public static PointAtExpansion Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		PointAtExpansion pointAtExpansion = new PointAtExpansion();
		pointAtExpansion.Parse(data, id, startConditions, originatedFromQuest);
		return pointAtExpansion;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		targetDid = TFUtils.LoadInt(data, "slot_id");
		pointer.Parse(data, true, Vector3.zero, 1f);
		if (data.ContainsKey("restrict_clicks"))
		{
			restrict = (bool)data["restrict_clicks"];
		}
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dict = base.ToDict();
		pointer.AddToDict(ref dict);
		return dict;
	}

	public void SpawnPointer(Session session, SessionActionTracker tracker)
	{
		Game theGame = session.TheGame;
		TFUtils.Assert(theGame.terrain.ExpansionSlots.ContainsKey(targetDid), "Missing a target Slot for Point at Expansion: " + targetDid);
		TerrainSlot terrainSlot = theGame.terrain.ExpansionSlots[targetDid];
		pointer.Spawn(theGame, tracker, terrainSlot);
		RestrictExpansion(theGame.simulation);
		FrameCamera(session.TheCamera, terrainSlot.Position);
	}

	public void RestrictExpansion(Simulation simulation)
	{
		if (restrict)
		{
			restricted = true;
			RestrictInteraction.AddWhitelistExpansion(simulation, targetDid);
			RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
			RestrictInteraction.AddWhitelistSimulated(simulation, int.MinValue);
		}
	}

	public override void OnDestroy(Game game)
	{
		if (restricted)
		{
			RestrictInteraction.RemoveWhitelistExpansion(game.simulation, targetDid);
			RestrictInteraction.RemoveWhitelistSimulated(game.simulation, int.MinValue);
			RestrictInteraction.RemoveWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
			restricted = false;
		}
	}
}
