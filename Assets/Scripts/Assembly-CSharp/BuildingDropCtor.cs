using System;
using UnityEngine;

public class BuildingDropCtor : ItemDropCtor
{
	private const float BUILDING_DROP_SCALE = 0.7f;

	private Identity id;

	public BuildingDropCtor(ItemDropDefinition definition, Identity id, ulong creationTime)
		: base(definition, creationTime)
	{
		this.id = id;
		base.definition.DisplayController.Scale = new Vector3(0.7f, 0.7f, 0.7f);
	}

	public override ItemDrop CreateItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, Action onCleanupComplete)
	{
		return new BuildingDrop(position, fixedOffset, direction, definition, creationTime, id, onCleanupComplete);
	}
}
