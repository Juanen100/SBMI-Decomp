using System;
using UnityEngine;

public class BuildingDrop : ItemDrop
{
	private Identity id;

	public BuildingDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, ItemDropDefinition definition, ulong creationTime, Identity id, Action callback)
		: base(default(Vector3), default(Vector3), default(Vector3), null, 0uL, null)
	{
	}

	protected override void OnCollectionAnimationComplete(Session session)
	{
	}

	protected override void PlayTapAnimation(Session session)
	{
	}

	protected override void PlayRewardAmountTextAnim(Session session)
	{
	}

	public static Vector2 GetScreenCollectionDestination()
	{
		return default(Vector2);
	}
}
