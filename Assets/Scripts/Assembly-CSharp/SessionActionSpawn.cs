public abstract class SessionActionSpawn : ISessionActionSpawn
{
	protected SessionActionTracker parentAction;

	public SessionActionTracker ParentAction
	{
		get
		{
			return null;
		}
	}

	protected virtual void RegisterNewInstance(Game game, SessionActionTracker parentAction)
	{
	}

	public virtual SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		return default(SessionActionManager.SpawnReturnCode);
	}

	public abstract void Destroy();
}
