using System;
using UnityEngine;

public class ResourceDrop : ItemDrop
{
	private int amount;

	public override int Value
	{
		get
		{
			return 0;
		}
	}

	public ResourceDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, ItemDropDefinition definition, ulong creationTime, int amount, Action callback)
		: base(default(Vector3), default(Vector3), default(Vector3), null, 0uL, null)
	{
	}

	public static string MakeResourceKey(int did)
	{
		return null;
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

	public static Vector2 GetScreenCollectionDestination(int resourceDid)
	{
		return default(Vector2);
	}
}
