using System.Collections.Generic;

public class LockInput : SessionActionDefinition
{
	public const string TYPE = "lock_input";

	private bool activated;

	public static LockInput Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		LockInput lockInput = new LockInput();
		lockInput.Parse(data, id, startConditions, originatedFromQuest);
		return lockInput;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
	}

	public void Handle(Session session, SessionActionTracker action)
	{
		action.MarkStarted();
		RestrictInteraction.AddWhitelistSimulated(session.TheGame.simulation, int.MinValue);
		RestrictInteraction.AddWhitelistExpansion(session.TheGame.simulation, int.MinValue);
		RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
		activated = true;
	}

	public override void OnDestroy(Game game)
	{
		if (activated)
		{
			RestrictInteraction.RemoveWhitelistSimulated(game.simulation, int.MinValue);
			RestrictInteraction.RemoveWhitelistExpansion(game.simulation, int.MinValue);
			RestrictInteraction.RemoveWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
			activated = false;
		}
	}
}
