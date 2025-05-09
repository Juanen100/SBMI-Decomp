using UnityEngine;

public class DownloadObbExample : MonoBehaviour
{
	private void OnGUI()
	{
		if (!GooglePlayDownloader.RunningOnAndroid())
		{
			GUI.Label(new Rect(10f, 10f, Screen.width - 10, 20f), "Use GooglePlayDownloader only on Android device!");
			return;
		}
		string expansionFilePath = GooglePlayDownloader.GetExpansionFilePath();
		if (expansionFilePath == null)
		{
			GUI.Label(new Rect(10f, 10f, Screen.width - 10, 20f), "External storage is not available!");
			return;
		}
		string mainOBBPath = GooglePlayDownloader.GetMainOBBPath(expansionFilePath);
		string patchOBBPath = GooglePlayDownloader.GetPatchOBBPath(expansionFilePath);
		GUI.Label(new Rect(10f, 10f, Screen.width - 10, 20f), "Main = ..." + ((mainOBBPath != null) ? mainOBBPath.Substring(expansionFilePath.Length) : " NOT AVAILABLE"));
		GUI.Label(new Rect(10f, 25f, Screen.width - 10, 20f), "Patch = ..." + ((patchOBBPath != null) ? patchOBBPath.Substring(expansionFilePath.Length) : " NOT AVAILABLE"));
		if ((mainOBBPath == null || patchOBBPath == null) && GUI.Button(new Rect(10f, 100f, 100f, 100f), "Fetch OBBs"))
		{
			GooglePlayDownloader.FetchOBB();
		}
	}
}
