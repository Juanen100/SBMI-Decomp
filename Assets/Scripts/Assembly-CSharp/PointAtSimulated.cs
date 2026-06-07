using System.Collections.Generic;
using UnityEngine;

public class PointAtSimulated : SimulationSessionActionDefinition
{
	public class SimulationTargetPointer : SimulationPointer
	{
		public override Vector3 TargetPosition
		{
			get
			{
				return default(Vector3);
			}
		}

		public void Spawn(Game game, SessionActionTracker parentAction, Simulated target)
		{
		}
	}

	public const string TYPE = "point_at_simulated";

	private const string SELECTED = "selected";

	private const string INSTANCE_ID = "instance_id";

	private const string DEFINITION_ID = "definition_id";

	private const string SUBHUD_SUBTARGET = "subhud_subtarget";

	private const string RESTRICT_CLICKS = "restrict_clicks";

	private const string FRAME_CAMERA = "frame_camera";

	private SimulationTargetPointer simPointer;

	private GuideArrow subHudPointer;

	private SBGUIElement subHudUi;

	private Identity targetId;

	private int? targetDid;

	private bool targetSelected;

	private bool restrict;

	private bool restricted;

	private bool frameCamera;

	private string subHudSubTarget;

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

	private PointAtSimulated()
	{
	}

	public static PointAtSimulated Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
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

	public void SpawnSimulatedPointer(Session session, SessionActionTracker tracker, Simulated target, SBGUIElement subTarget, SBGUIScreen subTargetContainer)
	{
	}

	public void SpawnSubHudPointer(Session session, SessionActionTracker tracker, Simulated target, SBGUIElement subTarget, SBGUIScreen subTargetContainer)
	{
	}

	public void RestrictSimulated(Simulation simulation)
	{
	}

	public override void OnDestroy(Game game)
	{
	}
}
