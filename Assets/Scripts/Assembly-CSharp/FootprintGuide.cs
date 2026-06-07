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

	private FootprintGuideSpawn spawnTemplate;

	public static FootprintGuide Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
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

	public override string ToString()
	{
		return null;
	}

	public void SpawnFootprint(Game game, SessionActionTracker tracker)
	{
	}

	public override void OnDestroy(Game game)
	{
	}
}
