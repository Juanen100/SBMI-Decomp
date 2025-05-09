using System;
using UnityEngine;

public class BuildingDrop : ItemDrop
{
	private Identity id;

	public BuildingDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, ItemDropDefinition definition, ulong creationTime, Identity id, Action callback)
		: base(position, fixedOffset, direction, definition, creationTime, callback)
	{
		this.id = id;
	}

	protected override void OnCollectionAnimationComplete(Session session)
	{
		session.TheSoundEffectManager.PlaySound("BuildingCollected");
		BuildingEntity decorator = session.TheGame.entities.Create(EntityType.BUILDING, definition.Did, id, true).GetDecorator<BuildingEntity>();
		decorator.GetDecorator<ErectableDecorator>().ErectionCompleteTime = creationTime;
		decorator.GetDecorator<ActivatableDecorator>().Activated = creationTime;
		session.TheGame.inventory.AddItem(decorator, null);
	}

	protected override void PlayTapAnimation(Session session)
	{
		session.TheSoundEffectManager.PlaySound("TapFallenBuildingItem");
	}

	protected override void PlayRewardAmountTextAnim(Session session)
	{
		if (session.TheState.GetType().Equals(typeof(Session.Playing)))
		{
			Session.Playing playing = (Session.Playing)session.TheState;
			Vector3 vector = session.TheCamera.WorldPointToScreenPoint(position);
			vector.x += (float)Screen.width * 0.0075f;
			vector.y -= (float)Screen.height * 0.0075f;
			playing.DisappearingResourceAmount(vector, 1);
		}
	}

	public static Vector2 GetScreenCollectionDestination()
	{
		return new Vector2((float)Screen.width * 0.854f, (float)Screen.height * 0.073f);
	}
}
