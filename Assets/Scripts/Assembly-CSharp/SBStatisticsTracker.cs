using System.Collections;
using UnityEngine;

public class SBStatisticsTracker : MonoBehaviour
{
	private const int BUCKET_COUNT = 7;

	private int acceptableFrameCount;

	private int totalFrames;

	private long lastTick;

	private float renderTime;

	private int[] frameRateBuckets;

	private Session session;

	private int[] lowRanges = new int[7] { 0, 30, 50, 75, 100, 150, 200 };

	private int[] highRanges = new int[7] { 30, 50, 75, 100, 150, 200, 99999999 };

	public bool Paused;

	public Session TheSession
	{
		set
		{
			session = value;
		}
	}

	public SBStatisticsTracker()
	{
		initBuckets();
	}

	private void Start()
	{
		TFUtils.DebugLog("Tracking Framerate statistics");
		StartCoroutine(SendStatistics());
	}

	public void OnApplicationPause(bool paused)
	{
		Paused = paused;
	}

	private int getBucket(int lastFrameRenderMS)
	{
		for (int i = 0; i < 7; i++)
		{
			if (lastFrameRenderMS >= lowRanges[i] && lastFrameRenderMS < highRanges[i])
			{
				return i;
			}
		}
		TFUtils.ErrorLog("Error. Last Render MS " + lastFrameRenderMS + " does not fit in a bucket");
		return 7;
	}

	private string getBucketName(int bucket)
	{
		return string.Format("{0}to{1}", lowRanges[bucket], highRanges[bucket]);
	}

	private void initBuckets()
	{
		frameRateBuckets = new int[7];
		for (int i = 0; i < 7; i++)
		{
			frameRateBuckets[i] = 0;
		}
	}

	private void Update()
	{
		renderTime += 1f;
		totalFrames++;
		int lastFrameRenderMS = (int)(Time.deltaTime * 1000f);
		int bucket = getBucket(lastFrameRenderMS);
		frameRateBuckets[bucket]++;
	}

	private IEnumerator SendStatistics()
	{
		while (true)
		{
			if (!Paused)
			{
				for (int i = 0; i < 7; i++)
				{
					session.Analytics.LogFrameRenderRates(getBucketName(i), frameRateBuckets[i]);
				}
				initBuckets();
			}
			yield return new WaitForSeconds(SBSettings.StatisticsTrackingInterval);
		}
	}
}
