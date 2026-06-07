using System;
using System.Collections.Generic;
using UnityEngine;

public class ExampleLegalPageController : MonoBehaviour
{
	[Serializable]
	public class ButtonInfo
	{
		public string name;

		public int fileIndex;
	}

	private WebViewSceneController webViewSC;

	public List<ButtonInfo> buttonInfo;

	private int lastPageIndex;

	private void Awake()
	{
	}

	private void OnGUI()
	{
	}
}
