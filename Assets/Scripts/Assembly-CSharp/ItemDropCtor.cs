using System;
using UnityEngine;

public abstract class ItemDropCtor
{
	protected ItemDropDefinition definition;

	protected ulong creationTime;

	public ItemDropDefinition Definition
	{
		get
		{
			return null;
		}
	}

	protected ItemDropCtor(ItemDropDefinition definition, ulong creationTime)
	{
	}

	public abstract ItemDrop CreateItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, Action onCleanupComplete);
}
