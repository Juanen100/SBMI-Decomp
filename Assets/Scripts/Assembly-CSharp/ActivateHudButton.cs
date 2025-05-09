#define ASSERTS_ON
using System.Collections.Generic;

public class ActivateHudButton : UiTargetingSessionActionDefinition
{
	public const string TYPE = "activate_hud_button";

	public override void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
		if (action.Status == SessionActionTracker.StatusCode.REQUESTED)
		{
			action.MarkStarted();
			TFUtils.Assert(target as SBGUIButton != null, "HudButton SessionActionDefinition expects target to be a button!");
			((SBGUIButton)target).MockClick();
			action.MarkSucceeded();
		}
	}

	public static ActivateHudButton Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ActivateHudButton activateHudButton = new ActivateHudButton();
		activateHudButton.Parse(data, id, startConditions, originatedFromQuest);
		return activateHudButton;
	}
}
