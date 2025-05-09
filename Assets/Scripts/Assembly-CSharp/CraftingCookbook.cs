using System;
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
		identity = TFUtils.LoadInt(data, "id");
		sessionActionId = TFUtils.LoadString(data, "session_action_id");
		cancelButtonTexture = TFUtils.LoadString(data, "texture.cancelbutton");
		recipeSlotTexture = TFUtils.LoadString(data, "texture.slot");
		titleTexture = TFUtils.LoadString(data, "texture.title");
		titleIconTexture = TFUtils.LoadString(data, "texture.titleicon");
		backgroundColor = ((List<object>)data["background.color"]).ConvertAll((object x) => Convert.ToInt32(x));
		buttonIcon = TFUtils.LoadNullableString(data, "button.icon");
		buttonLabel = Language.Get(TFUtils.LoadString(data, "button.label"));
		openSound = TFUtils.LoadString(data, "open_sound");
		closeSound = TFUtils.LoadString(data, "close_sound");
		music = TFUtils.LoadNullableString(data, "music");
		recipes = ((List<object>)data["recipes"]).ConvertAll((object x) => Convert.ToInt32(x));
	}

	public int[] GetRecipes()
	{
		return recipes.ToArray();
	}
}
