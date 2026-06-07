using UnityEngine;

public class FootprintGuideSpawn : SessionActionSpawn
{
	private const string MATERIAL = "Materials/unique/footprint";

	private static BasicSprite template;

	private BasicSprite sprite;

	public void Spawn(Game game, SessionActionTracker parentAction, Vector3 position, float width, float height)
	{
	}

	protected void RegisterNewInstance(Game game, SessionActionTracker parentAction, Vector3 position, float width, float height)
	{
	}

	public override void Destroy()
	{
	}

	private static BasicSprite CreateTemplate()
	{
		return null;
	}
}
