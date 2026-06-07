using System.Collections.Generic;
using UnityEngine;

public class ScreenMaskSimulation : SessionActionDefinition
{
	public const string TYPE_SIMULATED = "screenmask_simulated";

	public const string TYPE_SIMULATION = "screenmask_simulation";

	public const string TYPE_EXPANSION = "screenmask_expansion";

	private ScreenMaskSpawn.ScreenMaskType maskType;

	private float radius;

	private Vector3 offset;

	private string texture;

	private Identity targetId;

	private int? targetDid;

	private bool targetSelected;

	private string subHudSubTarget;

	private SBGUIElement subHudUi;

	private int? slotId;

	private bool restrict;

	private bool restricted;

	private const string RADIUS = "radius";

	private const string TEXTURE = "texture";

	private const string OFFSET = "offset";

	private const string SELECTED = "selected";

	private const string INSTANCE_ID = "instance_id";

	private const string DEFINITION_ID = "definition_id";

	private const string SLOT_ID = "slot_id";

	private const string SUBHUD_SUBTARGET = "subhud_subtarget";

	public Identity TargetId
	{
		get
		{
			return null;
		}
	}

	public int? TargetDid
	{
		get
		{
			return null;
		}
	}

	public string SubHudSubTarget
	{
		get
		{
			return null;
		}
	}

	public bool TargetSelected
	{
		get
		{
			return false;
		}
	}

	private ScreenMaskSimulation(ScreenMaskSpawn.ScreenMaskType maskType)
	{
	}

	public static ScreenMaskSimulation Create(ScreenMaskSpawn.ScreenMaskType maskType, Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		return null;
	}

	public void SpawnSimulationMask(Game game, SessionActionTracker tracker)
	{
	}

	public void SpawnSimulatedMask(Game game, SessionActionTracker tracker, Simulated target)
	{
	}

	public void SpawnSubHudMask(Game game, SessionActionTracker tracker, SBGUIElement subTarget, SBGUIScreen subTargetContainer)
	{
	}

	public void SpawnExpansionMask(Game game, SessionActionTracker tracker)
	{
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public void RestrictSimulated(Simulation simulation)
	{
	}

	public void RestrictExpansion(Simulation simulation)
	{
	}

	public override void OnDestroy(Game game)
	{
	}
}
