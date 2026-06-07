using System;
using UnityEngine;

public class MovieDropCtor : ItemDropCtor
{
	public MovieDropCtor(ItemDropDefinition definition, ulong creationTime)
		: base(null, 0uL)
	{
	}

	public override ItemDrop CreateItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, Action onCleanupComplete)
	{
		return null;
	}
}
