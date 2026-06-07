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

	private Dictionary<ITrigger, bool> triggersToProcess;

	public void SetActionHandler(string id, Session session, List<SBGUIScreen> searchableScreens, Handler handler)
	{
	}

	public void ClearActionHandler(string id, Session session)
	{
	}

	public void ClearActions()
	{
	}

	public void ClearStaleTrackers(string id, Session session)
	{
	}

	public bool ExistsActionHandler(string id)
	{
		return false;
	}

	public void RequestProcess(Game game)
	{
	}

	public void ProcessTrigger(ITrigger trigger, Game game)
	{
	}

	public void Request(SessionActionTracker sessionAction, Game game)
	{
	}

	public void Request(SessionActionTracker sessionAction, Game game, string tagOverride)
	{
	}

	public void Obliterate(SessionActionDefinition actionDefinition, Game game)
	{
	}

	public void Obliterate(SessionActionTracker actionTracker, Game game)
	{
	}

	public void ObliterateAnyTagged(string tag, Game game)
	{
	}

	public void OnUpdate(Game game)
	{
	}

	public void RegisterSpawn(ISessionActionSpawn spawn)
	{
	}

	private void ProcessActions(ITrigger trigger, Game game)
	{
	}

	private void MakeDirty()
	{
	}

	private bool IsDirty()
	{
		return false;
	}

	private List<SessionActionTracker> AllActions()
	{
		return null;
	}
}
