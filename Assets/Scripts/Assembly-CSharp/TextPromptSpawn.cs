public class TextPromptSpawn : SessionActionSpawn
{
	private UiSpawnMixin uiMixin;

	private static SessionActionTextPromptPrefab cachedPrefab;

	private SessionActionTextPromptPrefab prompt;

	private int instanceCount;

	private TextPrompt.Anchor anchorTarget;

	public void Spawn(Game game, SessionActionTracker parentAction, SBGUIScreen parentScreen, string text, TextPrompt.Anchor anchor)
	{
	}

	protected void RegisterNewInstance(Game game, SessionActionTracker parentAction, SBGUIScreen parentScreen, string text, TextPrompt.Anchor anchor)
	{
	}

	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		return default(SessionActionManager.SpawnReturnCode);
	}

	public override void Destroy()
	{
	}

	private SessionActionTextPromptPrefab GetPrefab()
	{
		return null;
	}
}
