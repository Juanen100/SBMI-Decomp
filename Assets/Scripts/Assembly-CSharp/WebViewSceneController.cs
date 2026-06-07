using System.Collections;
using System.Diagnostics;
using NickTemplate;
using UnityEngine;

public class WebViewSceneController : MonoBehaviour
{
	public LegalPageFiles legalPageFiles;

	private WebViewFactory webViewFactory;

	private WebViewObject webViewObject;

	private string pageName;

	private string remoteURL;

	private string localPath;

	private void Awake()
	{
	}

	private void UpdatePage(int pageIndex)
	{
	}

	private void CopyDefaultPage()
	{
	}

	private void GetFile(string path)
	{
	}

	[DebuggerHidden]
	private IEnumerator GetFileAndroid(string path)
	{
		return null;
	}

	[DebuggerHidden]
	private IEnumerator RefreshPage()
	{
		return null;
	}

	private void UpdateWebView()
	{
	}

	public void OpenWebView(int fileIndex)
	{
	}

	public void CloseWebView()
	{
	}

	public string LocalPath()
	{
		return null;
	}
}
