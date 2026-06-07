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

	public bool inUse;

	private int did;

	private int tier;

	private bool isBoardwalk;

	private GridPosition position;

	private IDisplayController sign;

	private GameObject outline;

	private List<Action> clickListeners;

	public int Id
	{
		get
		{
			return 0;
		}
	}

	public int Tier
	{
		get
		{
			return 0;
		}
	}

	public bool IsBoardwalk
	{
		get
		{
			return false;
		}
	}

	public Vector3 Position
	{
		get
		{
			return default(Vector3);
		}
	}

	public TerrainSlot(Dictionary<string, object> data)
	{
	}

	public static void MakeRealtySignPrototype(DisplayControllerManager dcm)
	{
	}

	public static List<TerrainSlotObject> LoadExpansionObjectData(List<object> data)
	{
		return null;
	}

	public static List<object> SerializeExpansionObjectData(List<TerrainSlotObject> data)
	{
		return null;
	}

	public bool Available(HashSet<int> purchasedSlots, Game game)
	{
		return false;
	}

	public void Display(DisplayControllerManager manager, BillboardDelegate billboard)
	{
	}

	public bool CheckTap(Ray ray)
	{
		return false;
	}

	public void OnUpdate(Camera camera)
	{
	}

	public void ClearSign()
	{
	}

	public void DrawOutline()
	{
	}

	public void ClearOutline()
	{
	}

	public void AddClickListener(Action handler)
	{
	}

	public bool RemoveClickListener(Action handler)
	{
		return false;
	}

	public void HandleSelection()
	{
	}
}
