using UnityEngine;

public class RestrictInteraction
{
	public const string RESTRICT_INTERACTION = "restrict_clicks";

	public const int RESTRICT_SIM_ID = int.MinValue;

	public const int RESTRICT_EXPANSION_ID = int.MinValue;

	public static readonly SBGUIElement RESTRICT_ALL_UI_ELEMENT = new GameObject("dummy").AddComponent<SBGUIDummyElement>();

	public static void AddWhitelistElement(SBGUIElement element)
	{
		SBGUI.GetInstance().WhitelistElement(element);
		SBGUI.GetInstance().RestoreWhiteList();
	}

	public static void RemoveWhitelistElement(SBGUIElement element)
	{
		SBGUI.GetInstance().UnWhitelistElement(element);
		SBGUI.GetInstance().ResetWhiteList();
	}

	public static bool ContainsWhitelistElement(SBGUIElement element)
	{
		return SBGUI.GetInstance().CheckWhitelisted(element);
	}

	public static void AddWhitelistSimulated(Simulation simulation, Identity id)
	{
		simulation.WhitelistSimulated(id);
	}

	public static void AddWhitelistSimulated(Simulation simulation, int did)
	{
		simulation.WhitelistSimulated(did);
	}

	public static void RemoveWhitelistSimulated(Simulation simulation, Identity id)
	{
		simulation.UnWhitelistSimulated(id);
	}

	public static void RemoveWhitelistSimulated(Simulation simulation, int did)
	{
		simulation.UnWhitelistSimulated(did);
	}

	public static void AddWhitelistExpansion(Simulation simulation, int did)
	{
		simulation.WhitelistExpansion(did);
	}

	public static void RemoveWhitelistExpansion(Simulation simulation, int did)
	{
		simulation.UnWhitelistExpansion(did);
	}
}
