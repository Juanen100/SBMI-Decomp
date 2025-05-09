using System.Collections.Generic;

public class SBGUIInventoryTabButton : SBGUIButton
{
	private static Dictionary<string, string> tabNames;

	public string tabName;

	public void SetSelected(bool selected)
	{
		if (!selected)
		{
		}
	}

	private static void SetupTabNames()
	{
		tabNames = new Dictionary<string, string>();
		tabNames["building"] = "Buildings";
		tabNames["trees"] = "Trees";
		tabNames["unit"] = "Characters";
		tabNames["worker"] = "Characters";
		tabNames["decoration"] = "Decorations";
	}

	private static string GetTabName(string t)
	{
		if (tabNames == null)
		{
			SetupTabNames();
		}
		string value = string.Empty;
		tabNames.TryGetValue(t, out value);
		return value;
	}
}
