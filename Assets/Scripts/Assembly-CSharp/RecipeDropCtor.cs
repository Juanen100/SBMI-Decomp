using System;
using UnityEngine;

public class RecipeDropCtor : ItemDropCtor
{
	public RecipeDropCtor(ItemDropDefinition definition, ulong creationTime)
		: base(definition, creationTime)
	{
	}

	public override ItemDrop CreateItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, Action onCleanupComplete)
	{
		return new RecipeDrop(position, fixedOffset, direction, definition, creationTime, onCleanupComplete);
	}
}
