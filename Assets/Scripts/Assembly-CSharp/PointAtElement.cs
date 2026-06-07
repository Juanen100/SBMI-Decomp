using System.Collections.Generic;

public class PointAtElement : UiTargetingSessionActionDefinition
{
	public const string TYPE = "point_at_element";

	private GuideArrow pointer;

	private PointAtElement()
	{
	}

	public static PointAtElement Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		return null;
	}

	public override void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
	}

	protected new void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}
}
