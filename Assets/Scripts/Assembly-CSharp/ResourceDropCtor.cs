using System;
using UnityEngine;

public class ResourceDropCtor : ItemDropCtor
{
	private int amount;

	public int Amount
	{
		get
		{
			return amount;
		}
	}

	public ResourceDropCtor(ItemDropDefinition definition, int amount, ulong creationTime)
		: base(definition, creationTime)
	{
		this.amount = amount;
	}

	public override ItemDrop CreateItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, Action onCleanupComplete)
	{
		return new ResourceDrop(position, fixedOffset, direction, definition, creationTime, amount, onCleanupComplete);
	}
}
