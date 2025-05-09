public interface ISessionActionSpawn
{
	SessionActionManager.SpawnReturnCode OnUpdate(Game game);

	void Destroy();
}
