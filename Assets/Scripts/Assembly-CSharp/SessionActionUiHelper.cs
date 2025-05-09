#define ASSERTS_ON
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SessionActionUiHelper
{
	private static List<string> APPLICABLE_TARGETING_TYPES = new List<string> { "activate_hud_button", "quest_reminder", "tutorial_hand_pointer", "point_at_element", "screenmask_element" };

	private static List<string> APPLICABLE_GENERAL_TYPES = new List<string> { "text_prompt" };

	public static void HandleCommonSessionActions(Session session, List<SBGUIScreen> screens, SessionActionTracker action)
	{
		string type = action.Definition.Type;
		if (APPLICABLE_TARGETING_TYPES.Contains(type))
		{
			UiTargetingSessionActionDefinition definition = (UiTargetingSessionActionDefinition)action.Definition;
			string target = definition.Target;
			{
				foreach (SBGUIScreen screen in screens)
				{
					SBGUIElement sBGUIElement = screen.FindChildSessionActionId(target, false);
					if (sBGUIElement == null && screen is SBGUIScrollableDialog)
					{
						LoadDialogBuffer();
						SBGUIScrollableDialog sBGUIScrollableDialog = screen as SBGUIScrollableDialog;
						if (sBGUIScrollableDialog.region != null && sBGUIScrollableDialog.region.subViewMarker != null)
						{
							SBGUIElement[] componentsInChildren = sBGUIScrollableDialog.region.subViewMarker.GetComponentsInChildren<SBGUIElement>(false);
							for (int num = componentsInChildren.Length - 1; num > 0; num--)
							{
								if (componentsInChildren[num].SessionActionId == target)
								{
									sBGUIElement = componentsInChildren[num];
									break;
								}
							}
						}
					}
					if (!(sBGUIElement != null))
					{
						continue;
					}
					if (definition.DynamicSubTarget != null)
					{
						sBGUIElement = sBGUIElement.FindDynamicSubElementSessionActionId(definition.DynamicSubTarget, false);
						TFUtils.Assert(sBGUIElement != null, "Had problems finding the DynamicSubTarget(" + definition.DynamicSubTarget + ")");
						definition.Handle(session, action, sBGUIElement, screen);
					}
					else if (definition.DynamicScrolledSubTarget != null)
					{
						SBGUISlottedScrollableDialog sBGUISlottedScrollableDialog = (SBGUISlottedScrollableDialog)sBGUIElement;
						sBGUISlottedScrollableDialog.FindDynamicSubElementInScrollRegionSessionActionIdAsync(definition.DynamicScrolledSubTarget, delegate(SBGUIElement foundElement)
						{
							definition.Handle(session, action, foundElement, screen);
						});
					}
					else
					{
						definition.Handle(session, action, sBGUIElement, screen);
					}
				}
				return;
			}
		}
		if (!APPLICABLE_GENERAL_TYPES.Contains(type))
		{
			return;
		}
		TextPrompt textPrompt = (TextPrompt)action.Definition;
		foreach (SBGUIScreen screen2 in screens)
		{
			textPrompt.Handle(session, action, screen2);
		}
	}

	private static IEnumerator LoadDialogBuffer()
	{
		yield return new WaitForSeconds(3f);
	}
}
