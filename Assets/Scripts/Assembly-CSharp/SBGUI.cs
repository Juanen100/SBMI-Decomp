using System.Collections.Generic;
using UnityEngine;

public class SBGUI : MonoBehaviour
{
	private List<SBGUIScreen> GUIScreenStack;

	private Dictionary<SBGUIElement, int> whitelistedUI;

	private Dictionary<SBGUIElement, int> backUpWhitelistedUI;

	public SBGUIScreen GUIScreen;

	private const float Z_STEP = 0.001f;

	private static SBGUI instance;

	public int GUIScreenCount
	{
		get
		{
			return 0;
		}
	}

	public static SBGUI GetInstance()
	{
		return null;
	}

	public static SBGUI GetCurrentInstance()
	{
		return null;
	}

	private static bool SetInstance(SBGUI inst)
	{
		return false;
	}

	public static Vector2 Touch2Screen(Vector2 p)
	{
		return default(Vector2);
	}

	public static Rect Touch2Screen(Rect r)
	{
		return default(Rect);
	}

	protected virtual void OnEnable()
	{
	}

	public SBGUIScreen LoadAndPushScreen(string prefabName)
	{
		return null;
	}

	private string DebugPrintGuiStack()
	{
		return null;
	}

	public void PushGUIScreen(SBGUIScreen screen)
	{
	}

	public void InsertGUIScreen(SBGUIScreen screen, int depth)
	{
	}

	public SBGUIScreen PeekGUIScreen()
	{
		return null;
	}

	public SBGUIScreen PopGUIScreen()
	{
		return null;
	}

	public SBGUIScreen RemoveGUIScreen(int depth)
	{
		return null;
	}

	public void RemoveGUIScreens(int depth, int layers)
	{
	}

	public bool ContainsGUIScreen(SBGUIScreen screen)
	{
		return false;
	}

	public bool ContainsGUIScreen<T>()
	{
		return false;
	}

	public static SBGUIElement InstantiatePrefab(string prefabName)
	{
		return null;
	}

	public void WhitelistElement(SBGUIElement element)
	{
	}

	public void UnWhitelistElement(SBGUIElement element)
	{
	}

	public void RestoreWhiteList()
	{
	}

	public void ResetWhiteList()
	{
	}

	public void PrintWhiteList()
	{
	}

	private string PrintUnrestrictedElements()
	{
		return null;
	}

	private void MuteScreens(bool mute)
	{
	}

	private static Camera GetEditorCamera()
	{
		return null;
	}

	public static float GetScreenWidth()
	{
		return 0f;
	}

	public static float GetScreenHeight()
	{
		return 0f;
	}

	public static float GetDpi()
	{
		return 0f;
	}

	public bool CheckWhitelisted(SBGUIElement elem)
	{
		return false;
	}
}
