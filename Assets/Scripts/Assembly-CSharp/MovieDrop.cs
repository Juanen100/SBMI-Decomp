using System;
using UnityEngine;

public class MovieDrop : ItemDrop
{
	public MovieDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, ItemDropDefinition definition, ulong creationTime, Action callback)
		: base(default(Vector3), default(Vector3), default(Vector3), null, 0uL, null)
	{
	}

	protected override void OnCollectionAnimationComplete(Session session)
	{
	}

	protected override bool UpdateCleanup(Session session, Camera camera, bool updateCollectionTimer)
	{
		return false;
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
