using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public static class SessionActionUiHelper
{
	private static List<string> APPLICABLE_TARGETING_TYPES;

	private static List<string> APPLICABLE_GENERAL_TYPES;

	public static void HandleCommonSessionActions(Session session, List<SBGUIScreen> screens, SessionActionTracker action)
	{
	}

	[DebuggerHidden]
	private static IEnumerator LoadDialogBuffer()
	{
		return null;
	}
}
