using System.Collections;
using UnityEngine;

public class GooglePlayDownload : MonoBehaviour
{
	private string mainPath;

	private string expPath;

	private void Start()
	{
		expPath = GooglePlayDownloader.GetExpansionFilePath();
		if (expPath == null)
		{
			Debug.Log("External storage is not available!");
			return;
		}
		mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
		if (mainPath == null)
		{
			GooglePlayDownloader.FetchOBB();
		}
		StartCoroutine(CoroutineLoadLevel());
	}

	protected IEnumerator CoroutineLoadLevel()
	{
		bool testResourceLoaded = false;
		while (!testResourceLoaded)
		{
			yield return new WaitForSeconds(0.5f);
			TFUtils.DebugLog("1");
			mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
			if (mainPath != null)
			{
				TFUtils.DebugLog("2");
				testResourceLoaded = true;
			}
		}
		Application.LoadLevel(1);
	}

	private void Update()
	{
	}
}
