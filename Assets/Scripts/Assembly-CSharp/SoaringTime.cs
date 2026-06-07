using System;

public static class SoaringTime
{
	private static DateTime EpochTime;

	private static long sSoaring_ServerTimeOffset;

	private static long sSoaring_LastServerTime;

	private static long sRelative_LastServerUpdateTime;

	private static long sDevice_SystemBootTime;

	private static long sDevice_SystemTimeDiff;

	private static int mUTCOffset;

	private static float mTimeHackProbability;

	private const int cTimeVariance = 240;

	private const int cTimezoneVariance = 1;

	private const int cTimehackTolerance = 25;

	public static long LastServerTime
	{
		get
		{
			return 0L;
		}
	}

	public static long AdjustedServerTime
	{
		get
		{
			return 0L;
		}
	}

	public static DateTime Epoch
	{
		get
		{
			return default(DateTime);
		}
	}

	public static long UserCurrentUTCUnixTime
	{
		get
		{
			return 0L;
		}
	}

	public static long CurrentDeviceTimeSinceBoot
	{
		get
		{
			return 0L;
		}
	}

	public static long DeviceBootTime
	{
		get
		{
			return 0L;
		}
	}

	private static DateTime ServerAdjustedDateTime()
	{
		return default(DateTime);
	}

	internal static void Register()
	{
	}

	internal static void UpdateServerTime(long l)
	{
	}

	internal static void Load()
	{
	}

	private static void Save()
	{
	}

	private static void SetDefaults()
	{
	}
}
