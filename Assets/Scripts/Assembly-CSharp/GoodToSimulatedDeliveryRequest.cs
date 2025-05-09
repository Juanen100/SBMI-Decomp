using UnityEngine;

public class GoodToSimulatedDeliveryRequest : GoodWidgetTransfer
{
	private Simulated targetSimulated;

	public GoodToSimulatedDeliveryRequest(Simulated targetSimulated, int goodId, string materialName)
		: base(goodId, materialName, 30f, 20f)
	{
		this.targetSimulated = targetSimulated;
	}

	public override Vector2 GetOriginalScreenPosition(Session session, Vector2 hudWidgetPosition)
	{
		return hudWidgetPosition;
	}

	public override Vector2 GetTargetScreenPosition(Session session, Vector2 hudWidgetPosition)
	{
		Vector3 position = targetSimulated.ThoughtDisplayController.Position;
		return session.TheCamera.WorldPointToScreenPoint(position);
	}
}
