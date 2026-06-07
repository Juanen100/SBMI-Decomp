using System;
using UnityEngine;

public abstract class ProductionSlotShell
{
	protected SBGUIProductionSlot core;

	protected bool activated;

	private int slotId;

	public int SlotId
	{
		get
		{
			return 0;
		}
	}

	public Vector2 Position
	{
		get
		{
			return default(Vector2);
		}
	}

	public ProductionSlotShell(SBGUIProductionSlot core, int slotId)
	{
	}

	public abstract void UpdateInfo(BuildingEntity producer, int slot, Action<int> rushHandler, Game game, Action<int> watchADHandler, bool isAdAvailable);
}
