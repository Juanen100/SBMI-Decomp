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
			return slotId;
		}
	}

	public Vector2 Position
	{
		get
		{
			return core.GetScreenPosition();
		}
	}

	public ProductionSlotShell(SBGUIProductionSlot core, int slotId)
	{
		this.slotId = slotId;
		this.core = core;
		this.core.SetActive(true);
		this.core.icon.SetTextureFromAtlas("empty.png");
		this.core.background.SetVisible(false);
		this.core.label.Text = string.Empty;
		this.core.rushButton.SessionActionId = string.Empty;
		this.core.rushButton.SetVisible(false);
		this.core.rushCostLabel.Text = string.Empty;
		this.core.transform.localPosition = Vector3.zero;
		activated = false;
	}

	public abstract void UpdateInfo(BuildingEntity producer, int slot, Action<int> rushHandler, Game game);
}
