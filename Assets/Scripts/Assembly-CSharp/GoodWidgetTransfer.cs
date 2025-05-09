using UnityEngine;

public abstract class GoodWidgetTransfer
{
	public SBGUIPulseImage icon;

	public float speed;

	public float dRad;

	public int goodId;

	public string materialName;

	public GoodWidgetTransfer(int goodId, string materialName, float speed, float dRad)
	{
		this.speed = speed;
		this.dRad = dRad;
		this.goodId = goodId;
		this.materialName = materialName;
	}

	public abstract Vector2 GetOriginalScreenPosition(Session session, Vector2 hudWidgetPosition);

	public abstract Vector2 GetTargetScreenPosition(Session session, Vector2 hudWidgetPosition);
}
