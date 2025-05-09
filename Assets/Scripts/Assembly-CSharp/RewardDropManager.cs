using UnityEngine;

public class RewardDropManager
{
	public const int defaultWidth = 16;

	public const int defaultHeight = 16;

	public const string recipeIcon = "RecipeIcon.png";

	public const string movieIcon = "MovieIcon.png";

	private const int START_POOL_SIZE = 10;

	private static int sDropId;

	private TFPool<BasicSprite> spritePool;

	public RewardDropManager()
	{
		spritePool = TFPool<BasicSprite>.CreatePool(10, MakeDrop);
	}

	private static BasicSprite MakeDrop()
	{
		Vector2 center = new Vector2(0f, 0f);
		BasicSprite basicSprite = new BasicSprite(null, "RecipeIcon.png", center, 16f, 16f, new QuadHitObject(center, 32f, 32f));
		basicSprite.PublicInitialize();
		basicSprite.Name = "RewardDrop_" + sDropId++;
		basicSprite.Visible = false;
		return basicSprite;
	}

	public IDisplayController CreateDrop(float width, float height, string material, string texture)
	{
		bool allocated = false;
		BasicSprite basicSprite = spritePool.Create(delegate
		{
			allocated = true;
			Vector2 center = new Vector2(0f, 0f);
			BasicSprite basicSprite2 = new BasicSprite(material, texture, center, width, height, new QuadHitObject(center, width * 2f, height * 2f));
			basicSprite2.PublicInitialize();
			basicSprite2.Name = "RewardDrop_" + sDropId++;
			return basicSprite2;
		});
		if (!allocated)
		{
			basicSprite.Scale = Vector3.one;
			if (basicSprite.Width != width || basicSprite.Height != height)
			{
				basicSprite.Resize(Vector2.zero, width, height);
			}
			if (texture != null)
			{
				if (!basicSprite.MaterialName.Equals(texture))
				{
					basicSprite.UpdateMaterialOrTexture(texture);
				}
			}
			else if (!basicSprite.MaterialName.Equals(material))
			{
				basicSprite.UpdateMaterialOrTexture(material);
			}
		}
		basicSprite.Position = new Vector3(0f, 0f, -1f);
		basicSprite.Visible = true;
		return basicSprite;
	}

	public IDisplayController CreateDrop(Resource resource)
	{
		return CreateDrop(resource.Width, resource.Height, null, resource.GetResourceTexture());
	}

	public IDisplayController CreateDrop(string texture)
	{
		return CreateDrop(16f, 16f, null, texture);
	}

	public bool ReleaseDrop(IDisplayController drop)
	{
		bool result = false;
		BasicSprite basicSprite = drop as BasicSprite;
		if (basicSprite != null && spritePool.Release(basicSprite))
		{
			result = true;
			drop.Visible = false;
		}
		return result;
	}
}
