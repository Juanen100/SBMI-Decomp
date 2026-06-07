using System;
using UnityEngine;

public class WebViewObject : MonoBehaviour
{
	private Action<string> onJS;

	private Action<string> onError;

	private Action<string> onHttpError;

	private Action<string> onStarted;

	private Action<string> onLoaded;

	private bool visibility;

	private int mMarginLeft;

	private int mMarginTop;

	private int mMarginRight;

	private int mMarginBottom;

	private Rect rect;

	private AndroidJavaObject webView;

	private bool mVisibility;

	private bool mIsKeyboardVisible0;

	private bool mIsKeyboardVisible;

	private float mResumedTimestamp;

	public Rect WebViewRect
	{
		get
		{
			return default(Rect);
		}
	}

	public bool IsKeyboardVisible
	{
		get
		{
			return false;
		}
	}

	private void OnApplicationPause(bool paused)
	{
	}

	private void Update()
	{
	}

	public void SetKeyboardVisible(string pIsVisible)
	{
	}

	public int AdjustBottomMargin(int bottom)
	{
		return 0;
	}

	public static bool IsWebViewAvailable()
	{
		return false;
	}

	public void Init(Action<string> cb = null, bool transparent = false, string ua = "", Action<string> err = null, Action<string> httpErr = null, Action<string> ld = null, bool enableWKWebView = false, Action<string> started = null)
	{
	}

	protected virtual void OnDestroy()
	{
	}

	public void SetCenterPositionWithScale(Vector2 center, Vector2 scale)
	{
	}

	public void SetMargins(int left, int top, int right, int bottom)
	{
	}

	public void SetVisibility(bool v)
	{
	}

	public bool GetVisibility()
	{
		return false;
	}

	public void LoadURL(string url)
	{
	}

	public void LoadHTML(string html, string baseUrl)
	{
	}

	public void EvaluateJS(string js)
	{
	}

	public int Progress()
	{
		return 0;
	}

	public bool CanGoBack()
	{
		return false;
	}

	public bool CanGoForward()
	{
		return false;
	}

	public void GoBack()
	{
	}

	public void GoForward()
	{
	}

	public void CallOnError(string error)
	{
	}

	public void CallOnHttpError(string error)
	{
	}

	public void CallOnStarted(string url)
	{
	}

	public void CallOnLoaded(string url)
	{
	}

	public void CallFromJS(string message)
	{
	}

	public void AddCustomHeader(string headerKey, string headerValue)
	{
	}

	public string GetCustomHeaderValue(string headerKey)
	{
		return null;
	}

	public void RemoveCustomHeader(string headerKey)
	{
	}

	public void ClearCustomHeader()
	{
	}

	public void ClearCookies()
	{
	}
}
