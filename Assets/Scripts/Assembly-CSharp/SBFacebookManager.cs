using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class SBFacebookManager : MonoBehaviour
{
	private static SBFacebookManager _instance;

	private Session session;

	private WebViewObject webViewObject;

	private string facebookGraphUrl;

	private bool receivedRedirectReponse;

	private Action<bool> LoginCallback;

	public static SBFacebookManager Instance
	{
		get
		{
			return null;
		}
	}

	public bool FacebookLoggedIn
	{
		get
		{
			return false;
		}
	}

	public void Awake()
	{
	}

	[DebuggerHidden]
	private IEnumerator VerifyToken(string tokenToVerify)
	{
		return null;
	}

	private void InitializeWebView()
	{
	}

	private void CallFacebookLogin()
	{
	}

	public void Login(Action<bool> loginCallback)
	{
	}

	public void Logout()
	{
	}

	private void CloseWebView()
	{
	}

	[DebuggerHidden]
	private IEnumerator PollWebView()
	{
		return null;
	}

	public void AddAdditionalCredentials(string facebookId)
	{
	}

	private void OnGUI()
	{
	}
}
