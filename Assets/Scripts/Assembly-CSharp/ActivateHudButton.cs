using System.Collections.Generic;

public class ActivateHudButton : UiTargetingSessionActionDefinition
{
	public const string TYPE = "activate_hud_button";

	public override void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
	}

	public static ActivateHudButton Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		return null;
	}
}
