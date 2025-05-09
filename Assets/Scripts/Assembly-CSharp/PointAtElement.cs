#define ASSERTS_ON
using System.Collections.Generic;
using UnityEngine;

public class PointAtElement : UiTargetingSessionActionDefinition
{
	public const string TYPE = "point_at_element";

	private GuideArrow pointer = new GuideArrow();

	private PointAtElement()
	{
	}

	public static PointAtElement Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		PointAtElement pointAtElement = new PointAtElement();
		pointAtElement.Parse(data, id, startConditions, originatedFromQuest);
		return pointAtElement;
	}

	public override void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
		if (action.Status != SessionActionTracker.StatusCode.REQUESTED)
		{
			return;
		}
		TFUtils.Assert(target.SessionActionId == base.Target || target.name == base.DynamicSubTarget || target.name == base.DynamicScrolledSubTarget, string.Format("Calling handle on an element that does not match an expected target! Expected {0}, {1}, or {2}. Found {3}/{4}.", base.Target, base.DynamicSubTarget, base.DynamicScrolledSubTarget, target.SessionActionId, target.name));
		if (!pointer.ElementIsInGoodState(target))
		{
			return;
		}
		SBGUIElement[] componentsInChildren = target.GetComponentsInChildren<SBGUIElement>();
		foreach (SBGUIElement sBGUIElement in componentsInChildren)
		{
			if (sBGUIElement.name != null && sBGUIElement.name.Contains("TutorialPointer"))
			{
				return;
			}
		}
		containingScreen.UsedInSessionAction = true;
		pointer.Spawn(session.TheGame, action, target, containingScreen);
		base.Handle(session, action, target, containingScreen);
	}

	protected new void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, originatedFromQuest);
		pointer.Parse(data, false, Vector3.zero, 0.01f);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dict = base.ToDict();
		pointer.AddToDict(ref dict);
		return dict;
	}
}
