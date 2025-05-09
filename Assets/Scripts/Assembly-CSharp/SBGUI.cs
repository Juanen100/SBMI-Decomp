#define ASSERTS_ON
using System.Collections.Generic;
using UnityEngine;

public class SBGUI : MonoBehaviour
{
	private const float Z_STEP = 0.001f;

	private List<SBGUIScreen> GUIScreenStack = new List<SBGUIScreen>();

	private Dictionary<SBGUIElement, int> whitelistedUI = new Dictionary<SBGUIElement, int>();

	private Dictionary<SBGUIElement, int> backUpWhitelistedUI = new Dictionary<SBGUIElement, int>();

	public SBGUIScreen GUIScreen;

	private static SBGUI instance;

	public int GUIScreenCount
	{
		get
		{
			return GUIScreenStack.Count;
		}
	}

	public static SBGUI GetInstance()
	{
		if (instance == null && Application.isPlaying)
		{
			GameObject gameObject = new GameObject("AutomaticallyCreatedSBGUI");
			SetInstance(gameObject.AddComponent<SBGUI>());
		}
		return instance;
	}

	public static SBGUI GetCurrentInstance()
	{
		return instance;
	}

	private static bool SetInstance(SBGUI inst)
	{
		if (instance != null && instance != inst)
		{
			return false;
		}
		instance = inst;
		return true;
	}

	public static Vector2 Touch2Screen(Vector2 p)
	{
		Vector2 result = p;
		result.y = GetScreenHeight() - result.y;
		return result;
	}

	public static Rect Touch2Screen(Rect r)
	{
		Rect result = r;
		result.y = GetScreenHeight() - result.y - result.height;
		return result;
	}

	protected virtual void OnEnable()
	{
		if (!SetInstance(this))
		{
			Object.DestroyImmediate(this);
		}
	}

	public SBGUIScreen LoadAndPushScreen(string prefabName)
	{
		SBGUIElement sBGUIElement = InstantiatePrefab(prefabName);
		if (sBGUIElement != null)
		{
			SBGUIScreen sBGUIScreen = sBGUIElement as SBGUIScreen;
			if (sBGUIScreen != null)
			{
				PushGUIScreen(sBGUIScreen);
				return sBGUIScreen;
			}
		}
		return null;
	}

	private string DebugPrintGuiStack()
	{
		if (GUIScreenStack.Count == 0)
		{
			return "[empty_stack]";
		}
		string text = string.Empty;
		for (int num = GUIScreenStack.Count - 1; num >= 0; num--)
		{
			string text2 = text;
			text = text2 + "\n[" + num + "] " + GUIScreenStack[num];
		}
		return text;
	}

	public void PushGUIScreen(SBGUIScreen screen)
	{
		InsertGUIScreen(screen, 0);
		if (whitelistedUI.Count > 0)
		{
			screen.MuteButtons(true);
		}
		if (whitelistedUI.Count == 0)
		{
			screen.MuteButtons(false);
		}
		foreach (SBGUIElement key in whitelistedUI.Keys)
		{
			key.MuteButtons(false);
		}
	}

	public void InsertGUIScreen(SBGUIScreen screen, int depth)
	{
		GUIScreenStack.Insert(GUIScreenStack.Count - depth, screen);
		if (depth == 0)
		{
			GUIScreen = screen;
			if ((bool)GUIScreen)
			{
				GUIScreen.OnScreenStart(screen);
			}
		}
	}

	public SBGUIScreen PeekGUIScreen()
	{
		return GUIScreen;
	}

	public SBGUIScreen PopGUIScreen()
	{
		SBGUIScreen result = null;
		if ((bool)GUIScreen)
		{
			GUIScreen.OnScreenEnd(GUIScreen);
		}
		if (GUIScreenStack.Count > 0)
		{
			GUIScreen = GUIScreenStack[GUIScreenStack.Count - 1];
			GUIScreenStack.RemoveAt(GUIScreenStack.Count - 1);
			result = GUIScreen;
		}
		return result;
	}

	public SBGUIScreen RemoveGUIScreen(int depth)
	{
		SBGUIScreen result = GUIScreenStack[GUIScreenStack.Count - 1 - depth];
		RemoveGUIScreens(depth, 1);
		return result;
	}

	public void RemoveGUIScreens(int depth, int layers)
	{
		List<SBGUIScreen> list = new List<SBGUIScreen>();
		int num = GUIScreenStack.Count - (depth + layers);
		for (int i = num; i < num + layers; i++)
		{
			list.Add(GUIScreenStack[i]);
		}
		GUIScreenStack.RemoveRange(num, layers);
		foreach (SBGUIScreen item in list)
		{
			if (!GUIScreenStack.Contains(item))
			{
				item.OnScreenEnd(item);
			}
		}
	}

	public bool ContainsGUIScreen(SBGUIScreen screen)
	{
		return GUIScreenStack.Contains(screen);
	}

	public bool ContainsGUIScreen<T>()
	{
		int count = GUIScreenStack.Count;
		for (int i = 0; i < count; i++)
		{
			if (GUIScreenStack[i] is T)
			{
				return true;
			}
		}
		return false;
	}

	public static SBGUIElement InstantiatePrefab(string prefabName)
	{
		GameObject gameObject = (GameObject)Resources.Load(prefabName);
		if (gameObject != null)
		{
			GameObject gameObject2 = (GameObject)Object.Instantiate(gameObject);
			if (gameObject2 != null)
			{
				SBGUIElement sBGUIElement = (SBGUIElement)gameObject2.GetComponent(typeof(SBGUIElement));
				if (sBGUIElement != null)
				{
					return sBGUIElement;
				}
			}
		}
		TFUtils.Assert(false, string.Format("Tried to instantiate a prefab but failed! name={0}", prefabName));
		return null;
	}

	public void WhitelistElement(SBGUIElement element)
	{
		MuteScreens(true);
		if (whitelistedUI.ContainsKey(element))
		{
			Dictionary<SBGUIElement, int> dictionary2;
			Dictionary<SBGUIElement, int> dictionary = (dictionary2 = whitelistedUI);
			SBGUIElement key2;
			SBGUIElement key = (key2 = element);
			int num = dictionary2[key2];
			dictionary[key] = num + 1;
		}
		else
		{
			whitelistedUI[element] = 1;
		}
		foreach (SBGUIElement key3 in whitelistedUI.Keys)
		{
			key3.MuteButtons(false);
		}
	}

	public void UnWhitelistElement(SBGUIElement element)
	{
		if (whitelistedUI.ContainsKey(element))
		{
			Dictionary<SBGUIElement, int> dictionary2;
			Dictionary<SBGUIElement, int> dictionary = (dictionary2 = whitelistedUI);
			SBGUIElement key2;
			SBGUIElement key = (key2 = element);
			int num = dictionary2[key2];
			dictionary[key] = num - 1;
			if (whitelistedUI[element] <= 0)
			{
				whitelistedUI.Remove(element);
				element.MuteButtons(true);
			}
			if (whitelistedUI.Count == 0)
			{
				MuteScreens(false);
			}
		}
	}

	public void RestoreWhiteList()
	{
		foreach (SBGUIElement key in backUpWhitelistedUI.Keys)
		{
			if (key != null)
			{
				if (!whitelistedUI.ContainsKey(key))
				{
					whitelistedUI.Add(key, backUpWhitelistedUI[key]);
				}
				key.MuteButtons(false);
			}
		}
		backUpWhitelistedUI.Clear();
	}

	public void ResetWhiteList()
	{
		foreach (SBGUIElement key in whitelistedUI.Keys)
		{
			if (key != null)
			{
				if (!backUpWhitelistedUI.ContainsKey(key))
				{
					backUpWhitelistedUI.Add(key, whitelistedUI[key]);
				}
				key.MuteButtons(false);
			}
		}
		whitelistedUI.Clear();
	}

	public void PrintWhiteList()
	{
		foreach (SBGUIElement key in whitelistedUI.Keys)
		{
			Debug.LogError("white list:" + key.name);
		}
	}

	private string PrintUnrestrictedElements()
	{
		string text = string.Empty;
		foreach (SBGUIElement key in whitelistedUI.Keys)
		{
			string text2 = text;
			text = text2 + "{" + key.name + ":" + whitelistedUI[key] + "}";
		}
		return text;
	}

	private void MuteScreens(bool mute)
	{
		foreach (SBGUIScreen item in GUIScreenStack)
		{
			item.MuteButtons(mute);
		}
	}

	private static Camera GetEditorCamera()
	{
		return null;
	}

	public static float GetScreenWidth()
	{
		Camera editorCamera = GetEditorCamera();
		return (!(editorCamera != null)) ? ((float)Screen.width) : editorCamera.pixelWidth;
	}

	public static float GetScreenHeight()
	{
		Camera editorCamera = GetEditorCamera();
		return (!(editorCamera != null)) ? ((float)Screen.height) : editorCamera.pixelHeight;
	}

	public static float GetDpi()
	{
		float num = Screen.dpi;
		if (num == 0f)
		{
			num = 100f;
		}
		return num;
	}

	public bool CheckWhitelisted(SBGUIElement elem)
	{
		return whitelistedUI.ContainsKey(elem);
	}
}
