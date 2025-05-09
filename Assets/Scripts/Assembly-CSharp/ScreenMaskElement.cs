using System.Collections.Generic;
using UnityEngine;

public class ScreenMaskElement : UiTargetingSessionActionDefinition
{
	public const string TYPE = "screenmask_element";

	private const string RADIUS = "radius";

	private const string TEXTURE = "texture";

	private const string OFFSET = "offset";

	private float radius;

	private Vector3 offset = Vector3.zero;

	private string texture;

	public static ScreenMaskElement Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ScreenMaskElement screenMaskElement = new ScreenMaskElement();
		screenMaskElement.Parse(data, id, startConditions, originatedFromQuest);
		return screenMaskElement;
	}

	public override void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
		if (action.Status == SessionActionTracker.StatusCode.REQUESTED)
		{
			containingScreen.UsedInSessionAction = true;
			bool useSecondCam = base.DynamicScrolledSubTarget != null;
			ScreenMaskSpawn.Spawn(ScreenMaskSpawn.ScreenMaskType.ELEMENT, session.TheGame, action, target, containingScreen, null, null, radius, texture, offset, useSecondCam);
			base.Handle(session, action, target, containingScreen);
		}
	}

	protected new void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, originatedFromQuest);
		radius = (float)TFUtils.LoadInt(data, "radius") * 0.01f;
		texture = TFUtils.TryLoadString(data, "texture");
		if (data.ContainsKey("offset"))
		{
			TFUtils.LoadVector3(out offset, TFUtils.LoadDict(data, "offset"));
			offset *= 0.01f;
		}
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["radius"] = radius;
		dictionary["texture"] = texture;
		dictionary["offset"] = offset;
		return dictionary;
	}
}
