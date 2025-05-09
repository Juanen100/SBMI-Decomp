#define ASSERTS_ON
using System.Collections.Generic;
using UnityEngine;

public class ScreenMaskSimulation : SessionActionDefinition
{
	public const string TYPE_SIMULATED = "screenmask_simulated";

	public const string TYPE_SIMULATION = "screenmask_simulation";

	public const string TYPE_EXPANSION = "screenmask_expansion";

	private const string RADIUS = "radius";

	private const string TEXTURE = "texture";

	private const string OFFSET = "offset";

	private const string SELECTED = "selected";

	private const string INSTANCE_ID = "instance_id";

	private const string DEFINITION_ID = "definition_id";

	private const string SLOT_ID = "slot_id";

	private const string SUBHUD_SUBTARGET = "subhud_subtarget";

	private ScreenMaskSpawn.ScreenMaskType maskType;

	private float radius;

	private Vector3 offset = Vector3.zero;

	private string texture;

	private Identity targetId;

	private int? targetDid;

	private bool targetSelected;

	private string subHudSubTarget;

	private SBGUIElement subHudUi;

	private int? slotId;

	private bool restrict;

	private bool restricted;

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

	private ScreenMaskSimulation(ScreenMaskSpawn.ScreenMaskType maskType)
	{
		this.maskType = maskType;
	}

	public static ScreenMaskSimulation Create(ScreenMaskSpawn.ScreenMaskType maskType, Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ScreenMaskSimulation screenMaskSimulation = new ScreenMaskSimulation(maskType);
		screenMaskSimulation.Parse(data, id, startConditions, originatedFromQuest);
		return screenMaskSimulation;
	}

	public void SpawnSimulationMask(Game game, SessionActionTracker tracker)
	{
		ScreenMaskSpawn.Spawn(ScreenMaskSpawn.ScreenMaskType.SIMULATION, game, tracker, null, null, null, null, radius, texture, offset);
	}

	public void SpawnSimulatedMask(Game game, SessionActionTracker tracker, Simulated target)
	{
		ScreenMaskSpawn.Spawn(ScreenMaskSpawn.ScreenMaskType.SIMULATED, game, tracker, null, null, target, null, radius, texture, offset);
		RestrictSimulated(game.simulation);
	}

	public void SpawnSubHudMask(Game game, SessionActionTracker tracker, SBGUIElement subTarget, SBGUIScreen subTargetContainer)
	{
		ScreenMaskSpawn.Spawn(ScreenMaskSpawn.ScreenMaskType.ELEMENT, game, tracker, subTarget, subTargetContainer, null, null, radius, texture, offset);
		subHudUi = subTarget;
		RestrictSimulated(game.simulation);
	}

	public void SpawnExpansionMask(Game game, SessionActionTracker tracker)
	{
		TFUtils.Assert(slotId.HasValue && game.terrain.ExpansionSlots.ContainsKey(slotId.Value), "Trying to startup an Expansion Mask without a valid slot id given");
		TerrainSlot slot = game.terrain.ExpansionSlots[slotId.Value];
		ScreenMaskSpawn.Spawn(ScreenMaskSpawn.ScreenMaskType.EXPANSION, game, tracker, null, null, null, slot, radius, texture, offset);
		RestrictExpansion(game.simulation);
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		radius = (float)TFUtils.LoadInt(data, "radius") * 0.01f;
		texture = TFUtils.TryLoadString(data, "texture");
		if (data.ContainsKey("offset"))
		{
			TFUtils.LoadVector3(out offset, TFUtils.LoadDict(data, "offset"));
		}
		if (maskType == ScreenMaskSpawn.ScreenMaskType.SIMULATED)
		{
			string text = TFUtils.TryLoadString(data, "instance_id");
			if (text != null)
			{
				targetId = new Identity(text);
			}
			targetDid = TFUtils.TryLoadNullableInt(data, "definition_id");
			targetSelected = data.ContainsKey("selected") && (bool)data["selected"];
			subHudSubTarget = TFUtils.TryLoadString(data, "subhud_subtarget");
		}
		if (maskType == ScreenMaskSpawn.ScreenMaskType.EXPANSION)
		{
			TFUtils.Assert(data.ContainsKey("slot_id"), "Setup an Expansion Screenmask without defining a 'slot_id' in the definition.");
			slotId = TFUtils.TryLoadInt(data, "slot_id");
		}
		if ((maskType == ScreenMaskSpawn.ScreenMaskType.SIMULATED || maskType == ScreenMaskSpawn.ScreenMaskType.EXPANSION) && data.ContainsKey("restrict_clicks"))
		{
			restrict = (bool)data["restrict_clicks"];
		}
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["radius"] = radius;
		dictionary["texture"] = texture;
		dictionary["offset"] = offset;
		if (targetId != null)
		{
			dictionary["instance_id"] = targetId;
		}
		int? num = targetDid;
		if (num.HasValue)
		{
			dictionary["definition_id"] = targetDid;
		}
		if (targetSelected)
		{
			dictionary["selected"] = targetSelected;
		}
		dictionary["subhud_subtarget"] = subHudSubTarget;
		dictionary["restrict_clicks"] = restrict;
		return dictionary;
	}

	public void RestrictSimulated(Simulation simulation)
	{
		if (restrict)
		{
			restricted = true;
			TFUtils.Assert(targetId != null || targetDid.HasValue, "Restricted Screenmask Simulated action has no valid target restriction - must specify target ID or DID.");
			if (targetId != null)
			{
				RestrictInteraction.AddWhitelistSimulated(simulation, targetId);
			}
			else if (targetDid.HasValue)
			{
				RestrictInteraction.AddWhitelistSimulated(simulation, targetDid.Value);
			}
			if (subHudUi != null)
			{
				RestrictInteraction.AddWhitelistElement(subHudUi);
			}
			RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
			RestrictInteraction.AddWhitelistExpansion(simulation, int.MinValue);
		}
	}

	public void RestrictExpansion(Simulation simulation)
	{
		if (restrict)
		{
			restricted = true;
			TFUtils.Assert(slotId.HasValue, "Restricted Screenmask Expansion action has no valid target restriction - must specify target ID or DID.");
			RestrictInteraction.AddWhitelistExpansion(simulation, slotId.Value);
			RestrictInteraction.AddWhitelistSimulated(simulation, int.MinValue);
			RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
		}
	}

	public override void OnDestroy(Game game)
	{
		if (restricted)
		{
			RestrictInteraction.RemoveWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
			if (targetId != null)
			{
				RestrictInteraction.RemoveWhitelistSimulated(game.simulation, targetId);
			}
			else if (targetDid.HasValue)
			{
				RestrictInteraction.RemoveWhitelistSimulated(game.simulation, targetDid.Value);
			}
			if (subHudUi != null)
			{
				RestrictInteraction.RemoveWhitelistElement(subHudUi);
			}
			if (slotId.HasValue)
			{
				RestrictInteraction.RemoveWhitelistExpansion(game.simulation, slotId.Value);
				RestrictInteraction.RemoveWhitelistSimulated(game.simulation, int.MinValue);
			}
			else
			{
				RestrictInteraction.RemoveWhitelistExpansion(game.simulation, int.MinValue);
			}
			restricted = false;
		}
	}
}
