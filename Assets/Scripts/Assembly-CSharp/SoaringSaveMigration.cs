using System.Collections.Generic;

public class SoaringSaveMigration : Migration
{
	public override bool MigrateGamestate(Dictionary<string, object> gamestate, StaticContentLoader contentLoader)
	{
		object value;
		if (!gamestate.TryGetValue("farm", out value))
		{
			TFUtils.ErrorLog("Unable to access farm");
			return false;
		}
		Dictionary<string, object> dictionary = value as Dictionary<string, object>;
		if (dictionary == null)
		{
			TFUtils.ErrorLog("Farm is not a dictionary");
			return false;
		}
		if (!dictionary.TryGetValue("buildings", out value))
		{
			TFUtils.ErrorLog("Farm is missing buildings");
			return false;
		}
		List<object> list = value as List<object>;
		if (list == null)
		{
			TFUtils.ErrorLog("Buildings is not a list");
			return false;
		}
		foreach (object item in list)
		{
			Dictionary<string, object> dictionary2 = item as Dictionary<string, object>;
			if (dictionary2 != null && dictionary2.ContainsKey("craft.rewards") && dictionary2.TryGetValue("craft.rewards", out value) && value != null)
			{
				dictionary2.Remove("craft.rewards");
				dictionary2.Add("craft_rewards", value);
			}
		}
		return true;
	}
}
