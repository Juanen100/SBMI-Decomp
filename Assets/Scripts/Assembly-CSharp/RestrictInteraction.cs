public class RestrictInteraction
{
	public const string RESTRICT_INTERACTION = "restrict_clicks";

	public const int RESTRICT_SIM_ID = int.MinValue;

	public const int RESTRICT_EXPANSION_ID = int.MinValue;

	public static readonly SBGUIElement RESTRICT_ALL_UI_ELEMENT;

	public static void AddWhitelistElement(SBGUIElement element)
	{
	}

	public static void RemoveWhitelistElement(SBGUIElement element)
	{
	}

	public static bool ContainsWhitelistElement(SBGUIElement element)
	{
		return false;
	}

	public static void AddWhitelistSimulated(Simulation simulation, Identity id)
	{
	}

	public static void AddWhitelistSimulated(Simulation simulation, int did)
	{
	}

	public static void RemoveWhitelistSimulated(Simulation simulation, Identity id)
	{
	}

	public static void RemoveWhitelistSimulated(Simulation simulation, int did)
	{
	}

	public static void AddWhitelistExpansion(Simulation simulation, int did)
	{
	}

	public static void RemoveWhitelistExpansion(Simulation simulation, int did)
	{
	}
}
