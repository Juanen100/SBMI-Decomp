using System;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSlot
{
	private static BasicSprite defaultSign;

	public Cost cost;

	public List<TerrainSlotObject> debris;

	public List<TerrainSlotObject> landmarks;

	public List<GridPosition> sectors;

	public List<Vector3> outlinePoints;

	public List<int> requiredSlots;

	public volatile bool inUse;

	private int did;

	private int tier;

	private bool isBoardwalk;

	private GridPosition position;

	private IDisplayController sign;

	private GameObject outline;

	private List<Action> clickListeners = new List<Action>();

	public int Id
	{
		get
		{
			return did;
		}
	}

	public int Tier
	{
		get
		{
			return tier;
		}
	}

	public bool IsBoardwalk
	{
		get
		{
			return isBoardwalk;
		}
	}

	public Vector3 Position
	{
		get
		{
			return new Vector3(20 * position.col, 20 * position.row);
		}
	}

	public TerrainSlot(Dictionary<string, object> data)
	{
		int row = TFUtils.LoadInt(data, "row");
		int col = TFUtils.LoadInt(data, "col");
		did = TFUtils.LoadInt(data, "did");
		tier = TFUtils.LoadInt(data, "tier");
		position = new GridPosition(row, col);
		if (data.ContainsKey("is_boardwalk"))
		{
			isBoardwalk = TFUtils.LoadBool(data, "is_boardwalk");
		}
		else
		{
			isBoardwalk = false;
		}
		if (data.ContainsKey("cost") && data["cost"] != null)
		{
			cost = Cost.FromDict((Dictionary<string, object>)data["cost"]);
		}
		debris = LoadExpansionObjectData((List<object>)data["debris"]);
		landmarks = LoadExpansionObjectData((List<object>)data["landmarks"]);
		sectors = new List<GridPosition>();
		foreach (Dictionary<string, object> item in (List<object>)data["sectors"])
		{
			int row2 = TFUtils.LoadInt(item, "row");
			int col2 = TFUtils.LoadInt(item, "col");
			sectors.Add(new GridPosition(row2, col2));
		}
		int num = 120;
		outlinePoints = new List<Vector3>();
		foreach (Dictionary<string, object> item2 in (List<object>)data["outline"])
		{
			int num2 = TFUtils.LoadInt(item2, "row");
			int num3 = TFUtils.LoadInt(item2, "col");
			outlinePoints.Add(new Vector3(num3 * num, num2 * num, 0f));
		}
		requiredSlots = ((List<object>)data["required_slots"]).ConvertAll((object x) => Convert.ToInt32(x));
	}

	public static void MakeRealtySignPrototype(DisplayControllerManager dcm)
	{
		Vector2 center = new Vector2(0f, -10f);
		defaultSign = new BasicSprite(null, "RealtySign.png", center, 11f, 20f, new QuadHitObject(center, 22f, 40f));
	}

	public static List<TerrainSlotObject> LoadExpansionObjectData(List<object> data)
	{
		List<TerrainSlotObject> list = new List<TerrainSlotObject>();
		foreach (Dictionary<string, object> datum in data)
		{
			TerrainSlotObject item = new TerrainSlotObject
			{
				did = TFUtils.LoadInt(datum, "did")
			};
			if (datum.ContainsKey("label"))
			{
				item.id = new Identity((string)datum["label"]);
			}
			int row = TFUtils.LoadInt(datum, "y");
			int col = TFUtils.LoadInt(datum, "x");
			item.position = new GridPosition(row, col);
			list.Add(item);
		}
		return list;
	}

	public static List<object> SerializeExpansionObjectData(List<TerrainSlotObject> data)
	{
		List<object> list = new List<object>();
		foreach (TerrainSlotObject datum in data)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["did"] = datum.did;
			dictionary["label"] = datum.id.Describe();
			dictionary["x"] = datum.position.X;
			dictionary["y"] = datum.position.Y;
			list.Add(dictionary);
		}
		return list;
	}

	public bool Available(HashSet<int> purchasedSlots, Game game)
	{
		if (purchasedSlots.Contains(Id))
		{
			return false;
		}
		foreach (int requiredSlot in requiredSlots)
		{
			if (!purchasedSlots.Contains(requiredSlot))
			{
				return false;
			}
		}
		if (isBoardwalk && !game.featureManager.CheckFeature("purchase_expansions_boardwalk"))
		{
			return false;
		}
		return true;
	}

	public void Display(DisplayControllerManager manager, BillboardDelegate billboard)
	{
		if (sign == null)
		{
			sign = defaultSign.Clone(manager);
			sign.Billboard(billboard);
			sign.Visible = false;
		}
	}

	public bool CheckTap(Ray ray)
	{
		return sign != null && sign.Visible && sign.Intersects(ray);
	}

	public void OnUpdate(Camera camera)
	{
		if (sign != null)
		{
			sign.Visible = true;
			sign.Position = Position;
			sign.OnUpdate(camera, null);
		}
	}

	public void ClearSign()
	{
		if (sign != null)
		{
			sign.Destroy();
			sign = null;
		}
	}

	public void DrawOutline()
	{
		if (outlinePoints.Count > 1 && !(null != outline))
		{
			int vertexCount = outlinePoints.Count * 2 + 1;
			outline = new GameObject("TerrainSlotOutline");
			outline.AddComponent(typeof(LineRenderer));
			LineRenderer lineRenderer = (LineRenderer)outline.GetComponent<Renderer>();
			lineRenderer.SetWidth(1f, 1f);
			lineRenderer.SetVertexCount(vertexCount);
			for (int i = 0; i < outlinePoints.Count; i++)
			{
				lineRenderer.SetPosition(i, outlinePoints[i]);
			}
			lineRenderer.SetPosition(outlinePoints.Count, outlinePoints[0]);
			for (int j = 1; j <= outlinePoints.Count; j++)
			{
				lineRenderer.SetPosition(outlinePoints.Count + j, outlinePoints[outlinePoints.Count - j]);
			}
			lineRenderer.material = (Material)Resources.Load("Materials/unique/outline");
		}
	}

	public void ClearOutline()
	{
		if (null != outline)
		{
			UnityEngine.Object.Destroy(outline);
			outline = null;
		}
	}

	public void AddClickListener(Action handler)
	{
		clickListeners.Add(handler);
	}

	public bool RemoveClickListener(Action handler)
	{
		return clickListeners.Remove(handler);
	}

	public void HandleSelection()
	{
		Action[] array = clickListeners.ToArray();
		foreach (Action action in array)
		{
			action();
		}
	}
}
