using System;
using MTools;
using UnityEngine;

public static class SoaringTime
{
	private const int cTimeVariance = 240;

	private const int cTimezoneVariance = 1;

	private const int cTimehackTolerance = 25;

	private static DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

	private static long sSoaring_ServerTimeOffset = 0L;

	private static long sSoaring_LastServerTime = 0L;

	private static long sRelative_LastServerUpdateTime = 0L;

	private static long sDevice_SystemBootTime = 0L;

	private static long sDevice_SystemTimeDiff = 0L;

	private static int mUTCOffset = 0;

	private static float mTimeHackProbability = 0f;

	public static long LastServerTime
	{
		get
		{
			return sSoaring_LastServerTime + sSoaring_ServerTimeOffset;
		}
	}

	public static long AdjustedServerTime
	{
		get
		{
			long lastServerTime = LastServerTime;
			if (lastServerTime == 0L)
			{
				return UserCurrentUTCUnixTime;
			}
			long num = CurrentDeviceTimeSinceBoot - sRelative_LastServerUpdateTime;
			return num + lastServerTime + sDevice_SystemTimeDiff;
		}
	}

	public static DateTime Epoch
	{
		get
		{
			return EpochTime;
		}
	}

	public static long UserCurrentUTCUnixTime
	{
		get
		{
			return (long)(DateTime.UtcNow - EpochTime).TotalSeconds;
		}
	}

	public static long CurrentDeviceTimeSinceBoot
	{
		get
		{
			return SoaringPlatform.SystemTimeSinceBootTime();
		}
	}

	public static long DeviceBootTime
	{
		get
		{
			return sDevice_SystemBootTime + sDevice_SystemTimeDiff;
		}
	}

	private static DateTime ServerAdjustedDateTime()
	{
		return EpochTime.AddSeconds(AdjustedServerTime);
	}

	internal static void Register()
	{
		SoaringInternal.instance.RegisterModule(new SoaringServerTimeModule());
	}

	internal static void UpdateServerTime(long l)
	{
		if ((sDevice_SystemTimeDiff != 0L || sDevice_SystemBootTime == 0L) && l > 0)
		{
			sDevice_SystemBootTime = SoaringPlatform.SystemBootTime();
			sDevice_SystemTimeDiff = 0L;
		}
		sRelative_LastServerUpdateTime = CurrentDeviceTimeSinceBoot;
		sSoaring_LastServerTime = l - sSoaring_ServerTimeOffset;
		Save();
	}

	internal static void Load()
	{
		bool flag = false;
		try
		{
			MBinaryReader fileStream = ResourceUtils.GetFileStream("SoaringTime", "Soaring", "dat", 1);
			if (fileStream != null && fileStream.IsOpen())
			{
				int num = fileStream.ReadInt();
				if (num == 3)
				{
					mTimeHackProbability = 0f;
					sDevice_SystemTimeDiff = 0L;
					sSoaring_LastServerTime = fileStream.ReadLong();
					sRelative_LastServerUpdateTime = fileStream.ReadLong();
					sDevice_SystemBootTime = fileStream.ReadLong();
					mUTCOffset = fileStream.ReadInt();
					long num2 = fileStream.ReadLong();
					TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
					int num3 = (int)currentTimeZone.GetUtcOffset(DateTime.Now).TotalHours;
					if (num3 != mUTCOffset)
					{
					}
					long num4 = SoaringPlatform.SystemBootTime();
					if (sDevice_SystemBootTime == 0L)
					{
						sDevice_SystemBootTime = num4;
					}
					sDevice_SystemTimeDiff = num4 - sDevice_SystemBootTime;
					if (sDevice_SystemTimeDiff - 240 < 0)
					{
						SoaringDebug.Log("Timestamp Missmatch: Time: Difference: " + sDevice_SystemTimeDiff + " | " + num4 + " | " + sDevice_SystemBootTime, LogType.Error);
						mTimeHackProbability += 5 * (sDevice_SystemTimeDiff / 240);
					}
					if (AdjustedServerTime < num2)
					{
						SoaringDebug.Log("Timestamp Missmatch: Ajusted Time: " + AdjustedServerTime + " < " + num2, LogType.Error);
						mTimeHackProbability += 10f;
					}
					fileStream.Close();
					fileStream = null;
					flag = true;
				}
			}
		}
		catch
		{
		}
		if (!flag)
		{
			SetDefaults();
		}
		SoaringDebug.Log("Timestamp: Diff: " + sDevice_SystemTimeDiff + " Last: " + sSoaring_LastServerTime + " Device Boot: " + sDevice_SystemBootTime + " Current Since: " + CurrentDeviceTimeSinceBoot + " Adjusted: " + AdjustedServerTime + " Loaded: " + flag, LogType.Log);
	}

	private static void Save()
	{
		try
		{
			string writePath = ResourceUtils.GetWritePath("SoaringTime.dat", "Soaring", 1);
			MBinaryWriter mBinaryWriter = new MBinaryWriter();
			if (mBinaryWriter.Open(writePath, true, true, "bak") && mBinaryWriter.IsOpen())
			{
				mBinaryWriter.Write(3);
				mBinaryWriter.Write(sSoaring_LastServerTime);
				mBinaryWriter.Write(sRelative_LastServerUpdateTime);
				mBinaryWriter.Write(sDevice_SystemBootTime);
				mBinaryWriter.Write(mUTCOffset);
				mBinaryWriter.Write(AdjustedServerTime);
				mBinaryWriter.Flush();
				mBinaryWriter.Close();
				mBinaryWriter = null;
			}
		}
		catch
		{
		}
	}

	private static void SetDefaults()
	{
		UpdateServerTime(UserCurrentUTCUnixTime);
	}
}
