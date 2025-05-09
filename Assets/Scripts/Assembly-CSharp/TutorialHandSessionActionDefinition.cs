#define ASSERTS_ON
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandSessionActionDefinition : UiTargetingSessionActionDefinition
{
	public const string TYPE = "tutorial_hand_pointer";

	private const string SIMULATED_DID = "definition_id";

	private const string TEXTURE = "texture";

	private const string DURATION = "duration";

	private TutorialHandDragGuide hand = new TutorialHandDragGuide();

	private uint targetSimulatedDid;

	private string iconTexture;

	private float duration;

	public static TutorialHandSessionActionDefinition Create(Dictionary<string, object> data, uint id, ICondition startingConditions, uint originatedFromQuest)
	{
		TutorialHandSessionActionDefinition tutorialHandSessionActionDefinition = new TutorialHandSessionActionDefinition();
		tutorialHandSessionActionDefinition.Parse(data, id, startingConditions, originatedFromQuest);
		return tutorialHandSessionActionDefinition;
	}

	protected new void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		TFUtils.Assert(!data.ContainsKey("alpha"), "Hand Guides do not support alpha!");
		TFUtils.Assert(!data.ContainsKey("rotation"), "Hand Guides do not support rotation!");
		base.Parse(data, id, startConditions, originatedFromQuest);
		hand.Parse(data, false, Vector3.zero, 1f);
		targetSimulatedDid = TFUtils.LoadUint(data, "definition_id");
		iconTexture = TFUtils.LoadString(data, "texture");
		duration = TFUtils.LoadFloat(data, "duration");
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dict = base.ToDict();
		hand.AddToDict(ref dict);
		dict["definition_id"] = targetSimulatedDid;
		dict["texture"] = iconTexture;
		dict["duration"] = duration;
		return dict;
	}

	public override void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
		if (action.Status == SessionActionTracker.StatusCode.REQUESTED)
		{
			hand.Spawn(session.TheGame, action, target, containingScreen, session.TheGame.simulation.FindSimulated((int)targetSimulatedDid), iconTexture, duration);
			base.Handle(session, action, target, containingScreen);
		}
	}
}
