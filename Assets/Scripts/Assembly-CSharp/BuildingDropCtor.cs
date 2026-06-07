using System;
using UnityEngine;

public class BuildingDropCtor : ItemDropCtor
{
	private const float BUILDING_DROP_SCALE = 0.7f;

	private Identity id;

	public BuildingDropCtor(ItemDropDefinition definition, Identity id, ulong creationTime)
		: base(null, 0uL)
	{
	}

	public override ItemDrop CreateItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, Action onCleanupComplete)
	{
		return null;
	}
}
