using System.Collections.Generic;

public class ResourceAmountsMigration : Migration
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
		if (!dictionary.TryGetValue("resources", out value))
		{
			TFUtils.ErrorLog("Farm is missing resources");
			return false;
		}
		List<object> list = value as List<object>;
		if (list == null)
		{
			TFUtils.ErrorLog("Resources is not a list");
			return false;
		}
		foreach (object item in list)
		{
			Dictionary<string, object> dictionary2 = item as Dictionary<string, object>;
			if (dictionary2 == null)
			{
				TFUtils.ErrorLog("Resource is not a dictionary");
				return false;
			}
			if (dictionary2.ContainsKey("amount") || !dictionary2.ContainsKey("amount_earned") || !dictionary2.ContainsKey("amount_spent") || !dictionary2.ContainsKey("amount_purchased"))
			{
				if (!dictionary2.TryGetValue("amount", out value))
				{
					TFUtils.ErrorLog("Resource is missing amount");
					return false;
				}
				if (!(value is long))
				{
					TFUtils.ErrorLog("Resource amount is not int");
					return false;
				}
				long num = (long)value;
				dictionary2.Remove("amount");
				dictionary2.Add("amount_earned", num);
				dictionary2.Add("amount_spent", 0);
				dictionary2.Add("amount_purchased", 0);
			}
		}
		return true;
	}
}
