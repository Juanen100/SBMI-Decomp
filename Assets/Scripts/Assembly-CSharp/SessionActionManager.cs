#define ASSERTS_ON
using System;
using System.Collections.Generic;

public class SessionActionManager : ITriggerObserver
{
	public enum SpawnReturnCode
	{
		KEEP_ALIVE = 0,
		KILL = 1
	}

	public delegate void Handler(Session session, List<SBGUIScreen> hud, SessionActionTracker action);

	public const bool CONDENSED_LOGGING = true;

	private Dictionary<string, Action<SessionActionTracker>> listeners;

	private List<SessionActionTracker> readiedActions;

	private List<SessionActionTracker> postponedActions;

	private List<SessionActionTracker> runningActions;

	private List<ISessionActionSpawn> spawns;

	private Dictionary<ITrigger, bool> triggersToProcess = new Dictionary<ITrigger, bool>();

	public SessionActionManager()
	{
		listeners = new Dictionary<string, Action<SessionActionTracker>>();
		readiedActions = new List<SessionActionTracker>();
		postponedActions = new List<SessionActionTracker>();
		runningActions = new List<SessionActionTracker>();
		spawns = new List<ISessionActionSpawn>();
	}

	public void SetActionHandler(string id, Session session, List<SBGUIScreen> searchableScreens, Handler handler)
	{
		TFUtils.Assert(!listeners.ContainsKey(id), "Clobbering existing action handler with the given id(" + id + ")");
		listeners[id] = delegate(SessionActionTracker a)
		{
			handler(session, searchableScreens, a);
		};
	}

	public void ClearActionHandler(string id, Session session)
	{
		listeners.Remove(id);
		ClearStaleTrackers(id, session);
	}

	public void ClearActions()
	{
		foreach (ISessionActionSpawn spawn in spawns)
		{
			spawn.Destroy();
		}
		spawns.Clear();
		readiedActions.Clear();
		postponedActions.Clear();
		runningActions.Clear();
		listeners.Clear();
	}

	public void ClearStaleTrackers(string id, Session session)
	{
		foreach (ISessionActionSpawn spawn in spawns)
		{
			spawn.Destroy();
		}
		spawns.Clear();
		SessionActionTracker[] array = runningActions.ToArray();
		foreach (SessionActionTracker sessionActionTracker in array)
		{
			if (sessionActionTracker.Definition.ClearOnSessionChange && sessionActionTracker.Status != SessionActionTracker.StatusCode.OBLITERATED && sessionActionTracker.Status != SessionActionTracker.StatusCode.FINISHED_SUCCESS)
			{
				runningActions.Remove(sessionActionTracker);
				sessionActionTracker.ReActivate(session.TheGame);
				Request(sessionActionTracker, session.TheGame);
			}
		}
		MakeDirty();
	}

	public bool ExistsActionHandler(string id)
	{
		return listeners.ContainsKey(id);
	}

	public void RequestProcess(Game game)
	{
		MakeDirty();
	}

	public void ProcessTrigger(ITrigger trigger, Game game)
	{
		triggersToProcess[trigger] = true;
	}

	public void Request(SessionActionTracker sessionAction, Game game)
	{
		Request(sessionAction, game, sessionAction.Tag);
	}

	public void Request(SessionActionTracker sessionAction, Game game, string tagOverride)
	{
		sessionAction.Tag = tagOverride;
		TFUtils.Assert(sessionAction != null, "Don't request to track a null session action.");
		TFUtils.Assert(!readiedActions.Contains(sessionAction), "Re-requesting a session action. You should not do that.");
		TFUtils.Assert(!postponedActions.Contains(sessionAction), "Re-requesting a session action. You should not do that.");
		TFUtils.Assert(!runningActions.Contains(sessionAction), "Re-requesting a session action. You should not do that.");
		if (sessionAction.Status != SessionActionTracker.StatusCode.INITIAL && sessionAction.Status != SessionActionTracker.StatusCode.POSTPONED)
		{
			TFUtils.Assert(false, "Requested session action should be in the initial state. Encountered:" + sessionAction.Status);
		}
		if (sessionAction.ShouldSetPostponeTimer())
		{
			sessionAction.StartPostponeTimer();
			sessionAction.MarkPostponed();
			postponedActions.Add(sessionAction);
		}
		else
		{
			sessionAction.MarkRequested();
			readiedActions.Add(sessionAction);
			MakeDirty();
		}
	}

	public void Obliterate(SessionActionDefinition actionDefinition, Game game)
	{
		List<SessionActionTracker> list = AllActions();
		foreach (SessionActionTracker item in list)
		{
			if (actionDefinition == item.Definition)
			{
				Obliterate(item, game);
				break;
			}
		}
	}

	public void Obliterate(SessionActionTracker actionTracker, Game game)
	{
		if (actionTracker.Status != SessionActionTracker.StatusCode.OBLITERATED)
		{
			actionTracker.MarkObliterated(game);
			MakeDirty();
		}
	}

	public void ObliterateAnyTagged(string tag, Game game)
	{
		Predicate<SessionActionTracker> match = (SessionActionTracker action) => action.Tag == tag;
		List<SessionActionTracker> list = AllActions().FindAll(match);
		foreach (SessionActionTracker item in list)
		{
			Obliterate(item, game);
		}
	}

	public void OnUpdate(Game game)
	{
		List<SessionActionTracker> list = new List<SessionActionTracker>(postponedActions);
		foreach (SessionActionTracker item in list)
		{
			if (item.Status == SessionActionTracker.StatusCode.OBLITERATED)
			{
				postponedActions.Remove(item);
			}
			if (item.IsPostponeComplete())
			{
				postponedActions.Remove(item);
				Request(item, game);
			}
		}
		List<ITrigger> list2 = new List<ITrigger>();
		foreach (KeyValuePair<ITrigger, bool> item2 in triggersToProcess)
		{
			list2.Add(item2.Key);
		}
		triggersToProcess.Clear();
		foreach (ITrigger item3 in list2)
		{
			ProcessActions(item3, game);
		}
		List<ISessionActionSpawn> list3 = new List<ISessionActionSpawn>();
		foreach (ISessionActionSpawn spawn in spawns)
		{
			if (spawn.OnUpdate(game) == SpawnReturnCode.KILL)
			{
				list3.Add(spawn);
			}
		}
		foreach (ISessionActionSpawn item4 in list3)
		{
			spawns.Remove(item4);
		}
		if (list3.Count > 0)
		{
			MakeDirty();
		}
	}

	public void RegisterSpawn(ISessionActionSpawn spawn)
	{
		spawns.Add(spawn);
	}

	private void ProcessActions(ITrigger trigger, Game game)
	{
		bool flag = false;
		List<SessionActionTracker> list = new List<SessionActionTracker>();
		List<SessionActionTracker> list2 = new List<SessionActionTracker>(readiedActions);
		foreach (SessionActionTracker item in list2)
		{
			if (item.Status == SessionActionTracker.StatusCode.OBLITERATED)
			{
				list.Add(item);
				continue;
			}
			item.ActivationProgress.Recalculate(game, trigger);
			if (item.ActivationProgress.Examine() != ConditionResult.PASS)
			{
				continue;
			}
			item.PreActivate(game);
			ICollection<Action<SessionActionTracker>> collection = new List<Action<SessionActionTracker>>(listeners.Values);
			foreach (Action<SessionActionTracker> item2 in collection)
			{
				item2(item);
			}
			if (item.Status == SessionActionTracker.StatusCode.STARTED)
			{
				runningActions.Add(item);
				flag = true;
			}
			else if (item.Status == SessionActionTracker.StatusCode.FINISHED_SUCCESS || item.Status == SessionActionTracker.StatusCode.FINISHED_FAILURE)
			{
				list.Add(item);
				flag = true;
			}
		}
		foreach (SessionActionTracker runningAction in runningActions)
		{
			readiedActions.Remove(runningAction);
			flag |= runningAction.ActiveProcess(game);
			runningAction.SuccessProgress.Recalculate(game, trigger);
			if (runningAction.Status == SessionActionTracker.StatusCode.FINISHED_SUCCESS || runningAction.Status == SessionActionTracker.StatusCode.FINISHED_FAILURE || runningAction.Status == SessionActionTracker.StatusCode.OBLITERATED)
			{
				runningAction.PostComplete(game);
				list.Add(runningAction);
			}
		}
		foreach (SessionActionTracker item3 in list)
		{
			readiedActions.Remove(item3);
			postponedActions.Remove(item3);
			runningActions.Remove(item3);
			item3.Destroy(game);
			if (item3.Status == SessionActionTracker.StatusCode.FINISHED_FAILURE && item3.RepeatOnFail)
			{
				Request(new SessionActionTracker(item3.Definition, new ConstantCondition(item3.Definition.StartConditions.FindNextId(), true), item3.Tag), game);
			}
		}
		if (flag)
		{
			MakeDirty();
		}
	}

	private void MakeDirty()
	{
		triggersToProcess[Trigger.Null] = true;
	}

	private bool IsDirty()
	{
		return triggersToProcess.Count > 0;
	}

	private List<SessionActionTracker> AllActions()
	{
		List<SessionActionTracker> list = new List<SessionActionTracker>(readiedActions);
		list.AddRange(runningActions);
		list.AddRange(postponedActions);
		return list;
	}
}
