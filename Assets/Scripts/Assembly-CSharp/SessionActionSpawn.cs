public abstract class SessionActionSpawn : ISessionActionSpawn
{
	protected SessionActionTracker parentAction;

	public SessionActionTracker ParentAction
	{
		get
		{
			return parentAction;
		}
	}

	protected virtual void RegisterNewInstance(Game game, SessionActionTracker parentAction)
	{
		this.parentAction = parentAction;
		if (parentAction.Status == SessionActionTracker.StatusCode.REQUESTED)
		{
			this.parentAction.MarkStarted();
			game.sessionActionManager.RequestProcess(game);
		}
		game.sessionActionManager.RegisterSpawn(this);
		if (ToString() == "GuideArrow")
		{
			game.simulation.soundEffectManager.PlaySound("tutorial_arrow");
		}
		if (this.parentAction.Definition.Sound != null)
		{
			game.simulation.soundEffectManager.PlaySound(this.parentAction.Definition.Sound);
		}
	}

	public virtual SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		if (parentAction.Status == SessionActionTracker.StatusCode.FINISHED_SUCCESS || parentAction.Status == SessionActionTracker.StatusCode.FINISHED_FAILURE || parentAction.Status == SessionActionTracker.StatusCode.OBLITERATED)
		{
			Destroy();
			return SessionActionManager.SpawnReturnCode.KILL;
		}
		return SessionActionManager.SpawnReturnCode.KEEP_ALIVE;
	}

	public abstract void Destroy();
}
