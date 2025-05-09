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
			return definition;
		}
	}

	protected ItemDropCtor(ItemDropDefinition definition, ulong creationTime)
	{
		this.definition = definition;
		this.creationTime = creationTime;
	}

	public abstract ItemDrop CreateItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, Action onCleanupComplete);
}
