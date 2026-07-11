using System.Collections;
using System.IO;
using UnityEngine;

public class GooglePlayDownload : MonoBehaviour
{
    private string mainPath;
    private string expPath;

    private string customOBBUrl = "https://markut.com/main.21198.com.mtvn.sbmigoogleplay.obb";
    private float timeoutSeconds = 30f;

    private float downloadProgress = 0f;
    private bool isFallbackDownloading = false;

    private void Start()
    {
        expPath = GooglePlayDownloader.GetExpansionFilePath();

        if (expPath == null)
        {
            Debug.Log("External storage is not available!");
            return;
        }

        mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);

        if (mainPath != null)
        {
            StartCoroutine(CoroutineLoadLevel());
        }
        else
        {
            StartCoroutine(TryGooglePlayThenFallback());
        }
    }

    private IEnumerator TryGooglePlayThenFallback()
    {
        Debug.Log("Trying Google Play download...");
        GooglePlayDownloader.FetchOBB();

        float elapsed = 0f;
        while (elapsed < timeoutSeconds)
        {
            yield return new WaitForSeconds(0.5f);
            elapsed += 0.5f;

            mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
            if (mainPath != null)
            {
                Debug.Log("Google Play OBB ready.");
                StartCoroutine(CoroutineLoadLevel());
                yield break;
            }
        }

        Debug.LogWarning("Google Play timed out. Falling back to custom URL...");
        StartCoroutine(DownloadFromCustomUrl());
    }

    private IEnumerator DownloadFromCustomUrl()
    {
        Debug.Log("Downloading OBB from: " + customOBBUrl);

        isFallbackDownloading = true;
        downloadProgress = 0f;

        WWW www = new WWW(customOBBUrl);

        while (!www.isDone)
        {
            downloadProgress = www.progress;
            yield return null;
        }

        isFallbackDownloading = false;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("Custom download failed: " + www.error);
            yield break;
        }

        Debug.Log("Download complete. Saving OBB...");
        string savePath = Path.Combine(expPath, "main.21198.com.mtvn.sbmigoogleplay.obb");
        File.WriteAllBytes(savePath, www.bytes);
        Debug.Log("OBB saved to: " + savePath);

        mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
        StartCoroutine(CoroutineLoadLevel());
    }

    protected IEnumerator CoroutineLoadLevel()
    {
        bool testResourceLoaded = false;
        while (!testResourceLoaded)
        {
            yield return new WaitForSeconds(0.5f);
            mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
            if (mainPath != null)
            {
                testResourceLoaded = true;
            }
        }
        Application.LoadLevel(1);
    }

    private void OnGUI()
    {
        if (!isFallbackDownloading) return;

        int percent = Mathf.RoundToInt(downloadProgress * 100);
        string text = "Downloading... " + percent + "%";

        GUI.Label(
            new Rect(10, Screen.height - 30, 300, 20),
            text
        );
    }

    private void Update()
    {
    }
}
