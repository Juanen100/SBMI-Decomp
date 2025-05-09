using UnityEngine;

public class FootprintGuideSpawn : SessionActionSpawn
{
	private const string MATERIAL = "Materials/unique/footprint";

	private static BasicSprite template = CreateTemplate();

	private BasicSprite sprite;

	public FootprintGuideSpawn()
	{
		if (template == null || template.IsDestroyed)
		{
			template = CreateTemplate();
		}
	}

	public void Spawn(Game game, SessionActionTracker parentAction, Vector3 position, float width, float height)
	{
		FootprintGuideSpawn footprintGuideSpawn = new FootprintGuideSpawn();
		footprintGuideSpawn.RegisterNewInstance(game, parentAction, position, width, height);
	}

	protected void RegisterNewInstance(Game game, SessionActionTracker parentAction, Vector3 position, float width, float height)
	{
		base.RegisterNewInstance(game, parentAction);
		sprite = template;
		sprite.Resize(new Vector2(-0.5f * width, -0.5f * height), width, height);
		sprite.Position = position;
		sprite.Visible = true;
		sprite.Color = Simulated.COLOR_FOOTPRINT_FREE;
		sprite.OnUpdate(game.simulation.TheCamera, null);
	}

	public override void Destroy()
	{
		if (sprite != null)
		{
			sprite.Visible = false;
			sprite = null;
		}
	}

	private static BasicSprite CreateTemplate()
	{
		float num = 20f;
		float num2 = 20f;
		Vector2 center = new Vector2(-0.5f * num, -0.5f * num2);
		BasicSprite basicSprite = new BasicSprite("Materials/unique/footprint", null, center, num, num2);
		basicSprite.PublicInitialize();
		basicSprite.Name = "FootprintGuide";
		return basicSprite;
	}
}
