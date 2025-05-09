#define ASSERTS_ON
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
				TFUtils.Assert(simulated != null, "Trying to target a simulated, but simulated target is null!");
				return TFUtils.ExpandVector(simulated.Position) + simulated.ThoughtDisplayOffsetWorld;
			}
		}

		public void Spawn(Game game, SessionActionTracker parentAction, Simulated target)
		{
			TFUtils.Assert(target != null, "Must specify a target simulated.");
			SimulationTargetPointer simulationTargetPointer = new SimulationTargetPointer();
			simulationTargetPointer.Initialize(game, parentAction, offset, base.Alpha, base.Scale, target);
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

	private bool frameCamera = true;

	private string subHudSubTarget;

	public Identity TargetId
	{
		get
		{
			return targetId;
		}
	}

	public int? TargetDid
	{
		get
		{
			return targetDid;
		}
	}

	public string SubHudSubTarget
	{
		get
		{
			return subHudSubTarget;
		}
	}

	public bool TargetSelected
	{
		get
		{
			return targetSelected;
		}
	}

	private PointAtSimulated()
	{
	}

	public static PointAtSimulated Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		PointAtSimulated pointAtSimulated = new PointAtSimulated();
		pointAtSimulated.Parse(data, id, startConditions, originatedFromQuest);
		return pointAtSimulated;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		string text = TFUtils.TryLoadString(data, "instance_id");
		if (text != null)
		{
			targetId = new Identity(text);
		}
		targetDid = TFUtils.TryLoadNullableInt(data, "definition_id");
		targetSelected = data.ContainsKey("selected") && (bool)data["selected"];
		subHudSubTarget = TFUtils.TryLoadString(data, "subhud_subtarget");
		if (subHudSubTarget == null)
		{
			simPointer = new SimulationTargetPointer();
			simPointer.Parse(data, false, new Vector3(-5f, -5f, 0f), 1f);
		}
		else
		{
			subHudPointer = new GuideArrow();
			subHudPointer.Parse(data, false, new Vector3(0f, -0.3f, 0f), 0.01f);
		}
		if (data.ContainsKey("restrict_clicks"))
		{
			restrict = (bool)data["restrict_clicks"];
		}
		if (data.ContainsKey("frame_camera"))
		{
			frameCamera = (bool)data["frame_camera"];
		}
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dict = base.ToDict();
		if (simPointer != null)
		{
			simPointer.AddToDict(ref dict);
		}
		if (subHudPointer != null)
		{
			subHudPointer.AddToDict(ref dict);
		}
		if (targetId != null)
		{
			dict["instance_id"] = targetId;
		}
		int? num = targetDid;
		if (num.HasValue)
		{
			dict["definition_id"] = targetDid;
		}
		if (targetSelected)
		{
			dict["selected"] = targetSelected;
		}
		dict["subhud_subtarget"] = subHudSubTarget;
		dict["restrict_clicks"] = restrict;
		dict["frame_camera"] = frameCamera;
		return dict;
	}

	public void SpawnSimulatedPointer(Session session, SessionActionTracker tracker, Simulated target, SBGUIElement subTarget, SBGUIScreen subTargetContainer)
	{
		TFUtils.Assert(subHudPointer == null, "This PointAtSimulated was parsed to point at a SubHud and not a Simulated!");
		List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(target.entity.DefinitionId, target.Id);
		if (activeTasksForSimulated.Count <= 0 || activeTasksForSimulated[0].m_pTaskData.m_eTaskType != TaskData._eTaskType.eEnter)
		{
			simPointer.Spawn(session.TheGame, tracker, target);
			RestrictSimulated(session.TheGame.simulation);
			if (frameCamera)
			{
				FrameCamera(session.TheCamera, target.PositionCenter);
			}
		}
	}

	public void SpawnSubHudPointer(Session session, SessionActionTracker tracker, Simulated target, SBGUIElement subTarget, SBGUIScreen subTargetContainer)
	{
		TFUtils.Assert(simPointer == null, "This PointAtSimulated was parsed to point at a Simulated and not its subHud!");
		subHudUi = subTarget;
		subHudPointer.Spawn(session.TheGame, tracker, subTarget, subTargetContainer);
		RestrictSimulated(session.TheGame.simulation);
		if (frameCamera)
		{
			FrameCamera(session.TheCamera, target.PositionCenter);
		}
	}

	public void RestrictSimulated(Simulation simulation)
	{
		if (restrict)
		{
			restricted = true;
			TFUtils.Assert(targetId != null || targetDid.HasValue, "Restricted Point At Simulated action has no valid target restriction - must specify target ID or DID.");
			if (subHudUi != null)
			{
				RestrictInteraction.AddWhitelistElement(subHudUi);
			}
			else if (targetId != null)
			{
				RestrictInteraction.AddWhitelistSimulated(simulation, targetId);
			}
			else if (targetDid.HasValue)
			{
				RestrictInteraction.AddWhitelistSimulated(simulation, targetDid.Value);
			}
			RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
			RestrictInteraction.AddWhitelistSimulated(simulation, int.MinValue);
			RestrictInteraction.AddWhitelistExpansion(simulation, int.MinValue);
		}
	}

	public override void OnDestroy(Game game)
	{
		if (restricted)
		{
			if (subHudPointer != null && subHudUi != null)
			{
				RestrictInteraction.RemoveWhitelistElement(subHudUi);
			}
			else if (targetId != null)
			{
				RestrictInteraction.RemoveWhitelistSimulated(game.simulation, targetId);
			}
			else if (targetDid.HasValue)
			{
				RestrictInteraction.RemoveWhitelistSimulated(game.simulation, targetDid.Value);
			}
			RestrictInteraction.RemoveWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
			RestrictInteraction.RemoveWhitelistExpansion(game.simulation, int.MinValue);
			RestrictInteraction.RemoveWhitelistSimulated(game.simulation, int.MinValue);
			restricted = false;
		}
	}
}
