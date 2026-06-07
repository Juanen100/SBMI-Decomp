using System;
using UnityEngine;

public class ResourceDropCtor : ItemDropCtor
{
	private int amount;

	public int Amount
	{
		get
		{
			return 0;
		}
	}

	public ResourceDropCtor(ItemDropDefinition definition, int amount, ulong creationTime)
		: base(null, 0uL)
	{
	}

	public override ItemDrop CreateItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, Action onCleanupComplete)
	{
		return null;
	}
}
