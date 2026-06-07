using System;
using System.Collections.Generic;
using UnityEngine;

public class LegalPageFiles : ScriptableObject
{
	[Serializable]
	public class WebViewFile
	{
		public string fileName;

		public string fileURL;
	}

	public List<WebViewFile> webViewFiles;
}
