using UnityEngine;

public class SoaringAdView : MonoBehaviour
{
	private static Texture2D sBlankTexture;

	private static GUIStyle sBlankStyle;

	private const int OptimalScreenWidth = 960;

	private const int OptimalScreenHeight = 640;

	private static GameObject displayObject;

	private SoaringAdData mAdvertData;

	private SoaringAdServer mAdServer;

	private Rect mDisplayRect;

	private Vector2 mScreenSize;

	private SoaringContext mContext;

	public static SoaringAdView CreateAdView(SoaringAdData adData, SoaringAdServer server, SoaringContext context)
	{
		return null;
	}

	public void Initialize(SoaringAdData adData, SoaringAdServer adServer, SoaringContext context)
	{
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
	}
}
