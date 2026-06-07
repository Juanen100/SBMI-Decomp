using System.Collections.Generic;
using UnityEngine;

public class EnclosureManager
{
	public enum PieceType
	{
		BACK_CORNER = 0,
		BACK_LCORNER = 1,
		BACK_LEFT = 2,
		BACK_RCORNER = 3,
		BACK_RIGHT = 4,
		FRONT_CORNER = 5,
		FRONT_LCORNER = 6,
		FRONT_LEFT = 7,
		FRONT_RCORNER = 8,
		FRONT_RIGHT = 9
	}

	public class FlasherDef
	{
		public SpriteAnimationModel animationModel;

		public Vector2 positionOffset;

		public float width;

		public float height;

		public string placement;

		public Vector3 placementOffset;
	}

	public class PieceDef
	{
		public PieceType type;

		public float height;

		public float width;

		public Vector3 scale;

		public Vector3 placementOffset;

		public Vector3 textureOrigin;

		public Vector3 sequenceOffset;
	}

	public const string NAME_BACK_CORNER = "back_corner";

	public const string NAME_BACK_LCORNER = "back_lcorner";

	public const string NAME_BACK_LEFT = "back_left";

	public const string NAME_BACK_RCORNER = "back_rcorner";

	public const string NAME_BACK_RIGHT = "back_right";

	public const string NAME_FRONT_CORNER = "front_corner";

	public const string NAME_FRONT_LCORNER = "front_lcorner";

	public const string NAME_FRONT_LEFT = "front_left";

	public const string NAME_FRONT_RCORNER = "front_rcorner";

	public const string NAME_FRONT_RIGHT = "front_right";

	private List<Enclosure> allScaffolds;

	private List<Enclosure> allFences;

	public List<FlasherDef> flasherDefs;

	public Dictionary<string, PieceDef> scaffoldingDefs;

	public Dictionary<string, PieceDef> fenceDefs;

	private void LoadDefinitionsFromSpread()
	{
	}

	private void LoadDefinitions(string filename, Dictionary<string, PieceDef> defs)
	{
	}

	public Scaffolding AddScaffolding(AlignedBox box, BillboardDelegate billboard)
	{
		return null;
	}

	public void RemoveScaffolding(Scaffolding s)
	{
	}

	public Fence AddFence(AlignedBox box, BillboardDelegate billboard)
	{
		return null;
	}

	public void RemoveFence(Fence s)
	{
	}

	public void OnUpdate(Simulation simulation)
	{
	}

	public Vector3 CalcPosition(PieceType type, AlignedBox box)
	{
		return default(Vector3);
	}
}
