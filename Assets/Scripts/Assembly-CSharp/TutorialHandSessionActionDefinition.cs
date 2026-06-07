using System.Collections.Generic;

public class TutorialHandSessionActionDefinition : UiTargetingSessionActionDefinition
{
	public const string TYPE = "tutorial_hand_pointer";

	private const string SIMULATED_DID = "definition_id";

	private const string TEXTURE = "texture";

	private const string DURATION = "duration";

	private TutorialHandDragGuide hand;

	private uint targetSimulatedDid;

	private string iconTexture;

	private float duration;

	public static TutorialHandSessionActionDefinition Create(Dictionary<string, object> data, uint id, ICondition startingConditions, uint originatedFromQuest)
	{
		return null;
	}

	protected new void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
	}
}
