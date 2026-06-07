using UnityEngine;

public class GoodReturnRequest : GoodWidgetTransfer
{
	private Vector2 initialScreenPosition;

	public GoodReturnRequest(Vector2 initialScreenPosition, int goodId, string materialName)
		: base(0, null, 0f, 0f)
	{
	}

	public override Vector2 GetOriginalScreenPosition(Session session, Vector2 hudWidgetPosition)
	{
		return default(Vector2);
	}

	public override Vector2 GetTargetScreenPosition(Session session, Vector2 hudWidgetPosition)
	{
		return default(Vector2);
	}
}
