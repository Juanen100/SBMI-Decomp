using System;
using UnityEngine;

public class MovieDropCtor : ItemDropCtor
{
	public MovieDropCtor(ItemDropDefinition definition, ulong creationTime)
		: base(definition, creationTime)
	{
	}

	public override ItemDrop CreateItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, Action onCleanupComplete)
	{
		return new MovieDrop(position, fixedOffset, direction, definition, creationTime, onCleanupComplete);
	}
}
