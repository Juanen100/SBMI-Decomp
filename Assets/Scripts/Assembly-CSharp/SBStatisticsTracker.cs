using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class SBStatisticsTracker : MonoBehaviour
{
	private int acceptableFrameCount;

	private int totalFrames;

	private long lastTick;

	private float renderTime;

	private int[] frameRateBuckets;

	private const int BUCKET_COUNT = 7;

	private Session session;

	private int[] lowRanges;

	private int[] highRanges;

	public bool Paused;

	public Session TheSession
	{
		set
		{
		}
	}

	private void Start()
	{
	}

	public void OnApplicationPause(bool paused)
	{
	}

	private int getBucket(int lastFrameRenderMS)
	{
		return 0;
	}

	private string getBucketName(int bucket)
	{
		return null;
	}

	private void initBuckets()
	{
	}

	private void Update()
	{
	}

	[DebuggerHidden]
	private IEnumerator SendStatistics()
	{
		return null;
	}
}
