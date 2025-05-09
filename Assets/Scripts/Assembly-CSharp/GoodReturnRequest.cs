using UnityEngine;

public class GoodReturnRequest : GoodWidgetTransfer
{
	private Vector2 initialScreenPosition;

	public GoodReturnRequest(Vector2 initialScreenPosition, int goodId, string materialName)
		: base(goodId, materialName, 40f, 20f)
	{
		this.initialScreenPosition = initialScreenPosition;
	}

	public override Vector2 GetOriginalScreenPosition(Session session, Vector2 hudWidgetPosition)
	{
		return initialScreenPosition;
	}

	public override Vector2 GetTargetScreenPosition(Session session, Vector2 hudWidgetPosition)
	{
		return hudWidgetPosition;
	}
}
