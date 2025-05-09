#define ASSERTS_ON
using System.Collections.Generic;
using UnityEngine;

public class FootprintGuide : SimulationSessionActionDefinition
{
	public const string TYPE = "footprint_guide";

	private const string POSITION = "position";

	private const string WIDTH = "width";

	private const string HEIGHT = "height";

	private const string LOCK_PLACEMENT = "lock_placement";

	private Vector3 position;

	private float width;

	private float height;

	private bool lockPlacement;

	private FootprintGuideSpawn spawnTemplate = new FootprintGuideSpawn();

	public static FootprintGuide Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		FootprintGuide footprintGuide = new FootprintGuide();
		footprintGuide.Parse(data, id, startConditions, originatedFromQuest);
		return footprintGuide;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		TFUtils.LoadVector3(out position, (Dictionary<string, object>)data["position"]);
		width = TFUtils.LoadFloat(data, "width");
		height = TFUtils.LoadFloat(data, "height");
		bool? flag = TFUtils.TryLoadBool(data, "lock_placement");
		if (flag.HasValue)
		{
			lockPlacement = flag.Value;
		}
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["position"] = position;
		dictionary["width"] = width;
		dictionary["height"] = height;
		dictionary["lock_placement"] = lockPlacement;
		return dictionary;
	}

	public override string ToString()
	{
		return string.Concat(base.ToString(), "FootprintGuide:(position=", position, "width=", width, "height=", height, "lock_placement=", lockPlacement, ")");
	}

	public void SpawnFootprint(Game game, SessionActionTracker tracker)
	{
		spawnTemplate.Spawn(game, tracker, position, width, height);
		TFUtils.Assert(game.terrain.FootprintGuide == null, "Trying to add a footprint guide when one already exists!");
		if (lockPlacement)
		{
			AlignedBox footprintGuide = new AlignedBox(position.x, position.x + width, position.y, position.y + height);
			game.terrain.FootprintGuide = footprintGuide;
		}
	}

	public override void OnDestroy(Game game)
	{
		base.OnDestroy(game);
		if (lockPlacement)
		{
			game.terrain.FootprintGuide = null;
		}
	}
}
