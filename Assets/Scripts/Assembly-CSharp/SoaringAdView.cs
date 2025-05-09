using UnityEngine;

public class SoaringAdView : MonoBehaviour
{
	private const int OptimalScreenWidth = 960;

	private const int OptimalScreenHeight = 640;

	private static Texture2D sBlankTexture;

	private static GUIStyle sBlankStyle;

	private static GameObject displayObject;

	private SoaringAdData mAdvertData;

	private SoaringAdServer mAdServer;

	private Rect mDisplayRect;

	private Vector2 mScreenSize = new Vector2(0f, 0f);

	private SoaringContext mContext;

	public static SoaringAdView CreateAdView(SoaringAdData adData, SoaringAdServer server, SoaringContext context)
	{
		if (adData == null)
		{
			return null;
		}
		if (sBlankTexture == null)
		{
			sBlankStyle = new GUIStyle();
			sBlankTexture = new Texture2D(1, 1);
			sBlankTexture.SetPixel(0, 0, Color.clear);
			sBlankTexture.Apply();
		}
		if (displayObject != null)
		{
			return null;
		}
		displayObject = new GameObject("SoaringAdView");
		SoaringAdView soaringAdView = displayObject.AddComponent<SoaringAdView>();
		soaringAdView.Initialize(adData, server, context);
		return soaringAdView;
	}

	public void Initialize(SoaringAdData adData, SoaringAdServer adServer, SoaringContext context)
	{
		mAdvertData = adData;
		mAdServer = adServer;
		mContext = context;
		Update();
	}

	private void Update()
	{
		if (mAdvertData == null)
		{
			return;
		}
		float num = Screen.height;
		float num2 = Screen.width;
		if ((mScreenSize.x == num2 || mScreenSize.x == num2) && (mScreenSize.y == num2 || mScreenSize.y == num2))
		{
			return;
		}
		float num3 = num2 / 640f;
		float num4 = num / 640f;
		float num5 = num3;
		if (num4 < num3)
		{
			num5 = num4;
		}
		float num6 = mAdvertData.AdTexture.height;
		float num7 = mAdvertData.AdTexture.width;
		float num8 = num2 / (float)mAdvertData.AdTexture.width;
		float num9 = num / (float)mAdvertData.AdTexture.height;
		if (num8 < 1f || num9 < 1f)
		{
			float num10 = num8;
			if (num9 < num8)
			{
				num10 = num9;
			}
			num7 *= num10;
			num6 *= num10;
		}
		else
		{
			num7 *= num5;
			num6 *= num5;
		}
		float left = num2 / 2f - num7 / 2f;
		float top = num / 2f - num6 / 2f;
		mDisplayRect = new Rect(left, top, num7, num6);
		mScreenSize.x = Screen.width;
		mScreenSize.y = Screen.height;
	}

	private void OnGUI()
	{
		if (mScreenSize.x == 0f || mScreenSize.y == 0f)
		{
			return;
		}
		if (GUI.Button(new Rect(0f, 0f, mScreenSize.x, mScreenSize.y), sBlankTexture, sBlankStyle))
		{
			Soaring.Delegate.OnAdServed(true, mAdvertData, SoaringAdServerState.Closed, mContext);
			mAdvertData.Invalidate();
			displayObject = null;
			Object.Destroy(base.gameObject);
			return;
		}
		GUI.DrawTexture(mDisplayRect, mAdvertData.AdTexture, ScaleMode.StretchToFill);
		if (GUI.Button(mDisplayRect, sBlankTexture, sBlankStyle))
		{
			Soaring.Delegate.OnAdServed(true, mAdvertData, SoaringAdServerState.Clicked, mContext);
			mAdvertData.OpenAdPage();
			mAdServer.MarkAdAsShown(mAdvertData);
			mAdvertData.Invalidate();
			displayObject = null;
			Object.Destroy(base.gameObject);
		}
	}
}
