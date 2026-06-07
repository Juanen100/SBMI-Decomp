public class RewardDropManager
{
	public const int defaultWidth = 16;

	public const int defaultHeight = 16;

	public const string recipeIcon = "RecipeIcon.png";

	public const string movieIcon = "MovieIcon.png";

	private static int sDropId;

	private const int START_POOL_SIZE = 10;

	private TFPool<BasicSprite> spritePool;

	private static BasicSprite MakeDrop()
	{
		return null;
	}

	public IDisplayController CreateDrop(float width, float height, string material, string texture)
	{
		return null;
	}

	public IDisplayController CreateDrop(Resource resource)
	{
		return null;
	}

	public IDisplayController CreateDrop(string texture)
	{
		return null;
	}

	public bool ReleaseDrop(IDisplayController drop)
	{
		return false;
	}
}
