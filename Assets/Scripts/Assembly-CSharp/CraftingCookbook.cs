using System.Collections.Generic;

public class CraftingCookbook
{
	public const string TYPE = "cookbook";

	public const int DEFAULT_ID = 1;

	protected List<int> recipes;

	public int identity;

	public string sessionActionId;

	public string cancelButtonTexture;

	public string recipeSlotTexture;

	public string titleTexture;

	public string titleIconTexture;

	public List<int> backgroundColor;

	public string buttonIcon;

	public string buttonLabel;

	public string openSound;

	public string closeSound;

	public string music;

	public CraftingCookbook(Dictionary<string, object> data)
	{
	}

	public int[] GetRecipes()
	{
		return null;
	}
}
