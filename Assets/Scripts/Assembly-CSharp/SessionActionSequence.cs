using System.Collections.Generic;

public class SessionActionSequence : SessionActionCollection
{
	public const string TYPE = "sequence";

	public const string STEP = "step";

	public static SessionActionSequence Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		SessionActionSequence sessionActionSequence = new SessionActionSequence();
		sessionActionSequence.Parse(data, id, startConditions, originatedFromQuest);
		return sessionActionSequence;
	}

	public override void SetDynamicProperties(ref Dictionary<string, object> propertiesDict)
	{
		propertiesDict["step"] = 0;
		base.SetDynamicProperties(ref propertiesDict);
	}

	public override bool ActiveProcess(Game game, SessionActionTracker action)
	{
		bool result = false;
		if (action.Definition.Type == "sequence")
		{
			List<SessionActionTracker> steps = action.GetDynamic<List<SessionActionTracker>>("collection");
			if (action.Status == SessionActionTracker.StatusCode.OBLITERATED)
			{
				ObliterateAllSteps(ref steps, game);
			}
			if (action.Status == SessionActionTracker.StatusCode.STARTED)
			{
				int num = action.GetDynamic<int>("step");
				bool flag = true;
				while (flag)
				{
					flag = false;
					if (num >= steps.Count)
					{
						action.MarkSucceeded();
						result = true;
						continue;
					}
					SessionActionTracker sessionActionTracker = steps[num];
					switch (sessionActionTracker.Status)
					{
					case SessionActionTracker.StatusCode.INITIAL:
						game.sessionActionManager.Request(sessionActionTracker, game);
						result = true;
						break;
					case SessionActionTracker.StatusCode.FINISHED_SUCCESS:
						num++;
						flag = true;
						result = true;
						break;
					case SessionActionTracker.StatusCode.FINISHED_FAILURE:
						ObliterateAllSteps(ref steps, game);
						action.MarkFailed();
						result = true;
						break;
					}
				}
				action.SetDynamic("step", num);
			}
		}
		return result;
	}

	public override void OnObliterate(Game game, SessionActionTracker tracker)
	{
		ActiveProcess(game, tracker);
	}

	private void ObliterateAllSteps(ref List<SessionActionTracker> steps, Game game)
	{
		foreach (SessionActionTracker step in steps)
		{
			game.sessionActionManager.Obliterate(step.Definition, game);
		}
	}
}
