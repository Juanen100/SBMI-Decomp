using System.Collections.Generic;

public class SessionActionArray : SessionActionCollection
{
	public const string TYPE = "array";

	public static SessionActionArray Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		SessionActionArray sessionActionArray = new SessionActionArray();
		sessionActionArray.Parse(data, id, startConditions, originatedFromQuest);
		return sessionActionArray;
	}

	public override void PreActivate(Game game, SessionActionTracker action)
	{
		base.PreActivate(game, action);
		List<SessionActionTracker> dynamic = action.GetDynamic<List<SessionActionTracker>>("collection");
		foreach (SessionActionTracker item in dynamic)
		{
			game.sessionActionManager.Request(item, game);
		}
	}

	public override bool ActiveProcess(Game game, SessionActionTracker action)
	{
		bool flag = false;
		bool flag2 = false;
		List<SessionActionTracker> dynamic = action.GetDynamic<List<SessionActionTracker>>("collection");
		foreach (SessionActionTracker item in dynamic)
		{
			if (item.Status == SessionActionTracker.StatusCode.OBLITERATED)
			{
				action.MarkObliterated(game);
				flag = true;
			}
			else if (item.Status == SessionActionTracker.StatusCode.FINISHED_FAILURE)
			{
				action.MarkFailed();
				flag = true;
			}
			else if (item.Status != SessionActionTracker.StatusCode.FINISHED_SUCCESS)
			{
				flag2 = true;
			}
		}
		if (!flag2 && action.Status == SessionActionTracker.StatusCode.STARTED)
		{
			action.MarkSucceeded();
			flag = true;
		}
		return base.ActiveProcess(game, action) || flag;
	}

	public override void PostComplete(Game game, SessionActionTracker action)
	{
		List<SessionActionTracker> dynamic = action.GetDynamic<List<SessionActionTracker>>("collection");
		if (action.Status == SessionActionTracker.StatusCode.OBLITERATED)
		{
			dynamic.ForEach(delegate(SessionActionTracker t)
			{
				t.MarkObliterated(game);
			});
		}
		else if (action.Status == SessionActionTracker.StatusCode.FINISHED_FAILURE)
		{
			dynamic.ForEach(delegate(SessionActionTracker t)
			{
				if (t.Status != SessionActionTracker.StatusCode.FINISHED_SUCCESS)
				{
					t.MarkFailed();
				}
			});
		}
		else if (action.Status == SessionActionTracker.StatusCode.FINISHED_SUCCESS)
		{
			dynamic.ForEach(delegate(SessionActionTracker t)
			{
				if (t.Status != SessionActionTracker.StatusCode.FINISHED_FAILURE)
				{
					t.MarkSucceeded();
				}
			});
		}
		base.PostComplete(game, action);
	}
}
