#define ASSERTS_ON
using System.Collections.Generic;
using MiniJSON;
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

	public EnclosureManager()
	{
		scaffoldingDefs = new Dictionary<string, PieceDef>();
		fenceDefs = new Dictionary<string, PieceDef>();
		LoadDefinitionsFromSpread();
		allScaffolds = new List<Enclosure>();
		allFences = new List<Enclosure>();
	}

	private void LoadDefinitionsFromSpread()
	{
		string text = "Enclosure";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null)
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			PieceDef pieceDef = new PieceDef();
			pieceDef.width = instance.GetFloatCell(sheetIndex, rowIndex, "width");
			pieceDef.height = instance.GetFloatCell(sheetIndex, rowIndex, "width");
			pieceDef.scale = new Vector3(instance.GetFloatCell(sheetIndex, rowIndex, "scale x"), instance.GetFloatCell(sheetIndex, rowIndex, "scale y"), instance.GetFloatCell(sheetIndex, rowIndex, "scale z"));
			pieceDef.textureOrigin = new Vector3(instance.GetFloatCell(sheetIndex, rowIndex, "texture origin X"), instance.GetFloatCell(sheetIndex, rowIndex, "texture origin y"), 0f);
			pieceDef.sequenceOffset = new Vector3(instance.GetFloatCell(sheetIndex, rowIndex, "sequence offset x"), instance.GetFloatCell(sheetIndex, rowIndex, "sequence offset y"), instance.GetFloatCell(sheetIndex, rowIndex, "sequence offset z"));
			pieceDef.placementOffset = new Vector3(instance.GetFloatCell(sheetIndex, rowIndex, "placement offset x"), instance.GetFloatCell(sheetIndex, rowIndex, "placement offset y"), 0f);
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, "name");
			switch (stringCell)
			{
			case "back_corner":
				pieceDef.type = PieceType.BACK_CORNER;
				break;
			case "back_lcorner":
				pieceDef.type = PieceType.BACK_LCORNER;
				break;
			case "back_left":
				pieceDef.type = PieceType.BACK_LEFT;
				break;
			case "back_rcorner":
				pieceDef.type = PieceType.BACK_RCORNER;
				break;
			case "back_right":
				pieceDef.type = PieceType.BACK_RIGHT;
				break;
			case "front_corner":
				pieceDef.type = PieceType.FRONT_CORNER;
				break;
			case "front_lcorner":
				pieceDef.type = PieceType.FRONT_LCORNER;
				break;
			case "front_left":
				pieceDef.type = PieceType.FRONT_LEFT;
				break;
			case "front_rcorner":
				pieceDef.type = PieceType.FRONT_RCORNER;
				break;
			case "front_right":
				pieceDef.type = PieceType.FRONT_RIGHT;
				break;
			default:
				TFUtils.Assert(true, " Enclosure.csv has unknown defininiton for " + stringCell);
				break;
			}
			if (instance.GetStringCell(sheetIndex, rowIndex, "type") == "fence")
			{
				fenceDefs[stringCell] = pieceDef;
			}
			else
			{
				scaffoldingDefs[stringCell] = pieceDef;
			}
		}
	}

	private void LoadDefinitions(string filename, Dictionary<string, PieceDef> defs)
	{
		TFUtils.DebugLog("Loading Enclosure definition file: " + filename);
		string json = TFUtils.ReadAllText(filename);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(json);
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			string key = item.Key;
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)item.Value;
			float width = TFUtils.LoadFloat(dictionary2, "width");
			float height = TFUtils.LoadFloat(dictionary2, "height");
			Vector3 v = Vector3.one;
			if (dictionary2.ContainsKey("scale"))
			{
				TFUtils.LoadVector3(out v, (Dictionary<string, object>)dictionary2["scale"]);
			}
			Vector3 v2 = Vector3.zero;
			if (dictionary2.ContainsKey("placement_offset"))
			{
				TFUtils.LoadVector3(out v2, (Dictionary<string, object>)dictionary2["placement_offset"]);
			}
			Vector3 v3 = Vector3.zero;
			if (dictionary2.ContainsKey("texture_origin"))
			{
				TFUtils.LoadVector3(out v3, (Dictionary<string, object>)dictionary2["texture_origin"]);
				v3 *= 0.1302f;
			}
			Vector3 v4 = Vector3.zero;
			if (dictionary2.ContainsKey("sequence_offset"))
			{
				TFUtils.LoadVector3(out v4, (Dictionary<string, object>)dictionary2["sequence_offset"]);
			}
			PieceDef pieceDef = new PieceDef();
			switch (key)
			{
			case "back_corner":
				pieceDef.type = PieceType.BACK_CORNER;
				break;
			case "back_lcorner":
				pieceDef.type = PieceType.BACK_LCORNER;
				break;
			case "back_left":
				pieceDef.type = PieceType.BACK_LEFT;
				break;
			case "back_rcorner":
				pieceDef.type = PieceType.BACK_RCORNER;
				break;
			case "back_right":
				pieceDef.type = PieceType.BACK_RIGHT;
				break;
			case "front_corner":
				pieceDef.type = PieceType.FRONT_CORNER;
				break;
			case "front_lcorner":
				pieceDef.type = PieceType.FRONT_LCORNER;
				break;
			case "front_left":
				pieceDef.type = PieceType.FRONT_LEFT;
				break;
			case "front_rcorner":
				pieceDef.type = PieceType.FRONT_RCORNER;
				break;
			case "front_right":
				pieceDef.type = PieceType.FRONT_RIGHT;
				break;
			default:
				TFUtils.Assert(true, filename + " has unknown defininiton for " + key);
				break;
			}
			pieceDef.width = width;
			pieceDef.height = height;
			pieceDef.scale = v;
			pieceDef.placementOffset = v2;
			pieceDef.textureOrigin = v3;
			pieceDef.sequenceOffset = v4;
			defs[key] = pieceDef;
		}
		TFUtils.Assert(defs.ContainsKey("back_corner"), filename + " is missing a definition for back_corner");
		TFUtils.Assert(defs.ContainsKey("back_lcorner"), filename + " is missing a definition for back_lcorner");
		TFUtils.Assert(defs.ContainsKey("back_left"), filename + " is missing a definition for back_left");
		TFUtils.Assert(defs.ContainsKey("back_rcorner"), filename + " is missing a definition for back_rcorner");
		TFUtils.Assert(defs.ContainsKey("back_right"), filename + " is missing a definition for back_right");
		TFUtils.Assert(defs.ContainsKey("front_corner"), filename + " is missing a definition for front_corner");
		TFUtils.Assert(defs.ContainsKey("front_lcorner"), filename + " is missing a definition for front_lcorner");
		TFUtils.Assert(defs.ContainsKey("front_left"), filename + " is missing a definition for front_left");
		TFUtils.Assert(defs.ContainsKey("front_rcorner"), filename + " is missing a definition for front_rcorner");
		TFUtils.Assert(defs.ContainsKey("front_right"), filename + " is missing a definition for front_right");
	}

	public Scaffolding AddScaffolding(AlignedBox box, BillboardDelegate billboard)
	{
		Scaffolding scaffolding = new Scaffolding(box, this, billboard);
		allScaffolds.Add(scaffolding);
		return scaffolding;
	}

	public void RemoveScaffolding(Scaffolding s)
	{
		s.Destroy();
		allScaffolds.Remove(s);
	}

	public Fence AddFence(AlignedBox box, BillboardDelegate billboard)
	{
		Fence fence = new Fence(box, this, billboard);
		allFences.Add(fence);
		return fence;
	}

	public void RemoveFence(Fence s)
	{
		s.Destroy();
		allFences.Remove(s);
	}

	public void OnUpdate(Simulation simulation)
	{
		foreach (Scaffolding allScaffold in allScaffolds)
		{
			allScaffold.OnUpdate(simulation, this);
		}
		foreach (Fence allFence in allFences)
		{
			allFence.OnUpdate(simulation, this);
		}
	}

	public Vector3 CalcPosition(PieceType type, AlignedBox box)
	{
		switch (type)
		{
		case PieceType.BACK_CORNER:
			return new Vector3(box.xmax, box.ymax, 0f);
		case PieceType.BACK_LCORNER:
			return new Vector3(box.xmax, box.ymin, 0f);
		case PieceType.BACK_LEFT:
			return new Vector3(box.xmax, box.ymax, 0f);
		case PieceType.BACK_RCORNER:
			return new Vector3(box.xmin, box.ymax, 0f);
		case PieceType.BACK_RIGHT:
			return new Vector3(box.xmax, box.ymax, 0f);
		case PieceType.FRONT_CORNER:
			return new Vector3(box.xmin, box.ymin, 0f);
		case PieceType.FRONT_LCORNER:
			return new Vector3(box.xmax, box.ymin, 0f);
		case PieceType.FRONT_LEFT:
			return new Vector3(box.xmin, box.ymin, 0f);
		case PieceType.FRONT_RCORNER:
			return new Vector3(box.xmin, box.ymax, 0f);
		case PieceType.FRONT_RIGHT:
			return new Vector3(box.xmin, box.ymin, 0f);
		default:
			return Vector3.zero;
		}
	}
}
