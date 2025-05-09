using System;
using System.Collections.Generic;

public class VendorDefinition
{
	public const string TYPE = "vendor";

	public const int COUNT = 12;

	public List<int> generalStock;

	public List<int> specialStock;

	public int did;

	public string sessionActionId;

	public string cancelButtonTexture;

	public string titleTexture;

	public string titleIconTexture;

	public List<int> backgroundColor;

	public string buttonLabel;

	public string openSound;

	public string closeSound;

	public string music;

	private Cost rushCost;

	private int count = 12;

	public Cost RushCost
	{
		get
		{
			return rushCost;
		}
	}

	public int InstanceCount
	{
		get
		{
			return count;
		}
	}

	public VendorDefinition(Dictionary<string, object> data)
	{
		did = TFUtils.LoadInt(data, "did");
		sessionActionId = TFUtils.LoadString(data, "session_action_id");
		cancelButtonTexture = TFUtils.LoadString(data, "texture.cancelbutton");
		titleTexture = TFUtils.LoadString(data, "texture.title");
		titleIconTexture = TFUtils.LoadString(data, "texture.titleicon");
		backgroundColor = ((List<object>)data["background.color"]).ConvertAll((object x) => Convert.ToInt32(x));
		buttonLabel = Language.Get(TFUtils.LoadString(data, "button.label"));
		openSound = TFUtils.LoadString(data, "open_sound");
		closeSound = TFUtils.LoadString(data, "close_sound");
		music = TFUtils.LoadNullableString(data, "music");
		if (data.ContainsKey("general"))
		{
			generalStock = ((List<object>)data["general"]).ConvertAll((object x) => Convert.ToInt32(x));
		}
		else
		{
			generalStock = new List<int>();
		}
		if (data.ContainsKey("specials"))
		{
			specialStock = ((List<object>)data["specials"]).ConvertAll((object x) => Convert.ToInt32(x));
		}
		else
		{
			specialStock = new List<int>();
		}
		rushCost = Cost.FromObject(data["restock_cost"]);
		if (data.ContainsKey("stock_count"))
		{
			count = TFUtils.LoadInt(data, "stock_count");
		}
	}
}
