using System;

namespace MTools
{
	public static class MTime
	{
		private static DateTime sOriginTimeStamp;

		private static DateTime sLocalOriginTimestamp;

		private static uint sTimeStampAdjust;

		public const uint cSecondsInDay = 86400u;

		public const uint cSecondsInHour = 3600u;

		private static double sCurrentTime;

		public const int TimeZone_UTC = 0;

		public const int TimeZone_Eastern = -5;

		public const int TimeZone_Central = -6;

		public const int TimeZone_Mountain = -7;

		public const int TimeZone_Pacific = -7;

		public static void LoadCurrentTime()
		{
		}

		public static void SetCurrentTime(ulong cTime)
		{
		}

		public static bool ConstantTimestampWithinDays(int days, long ts)
		{
			return false;
		}

		public static long CurrentTimeSinceEpoch()
		{
			return 0L;
		}

		public static long TimeSinceEpoch(string time)
		{
			return 0L;
		}

		public static long TimeSinceEpoch(DateTime time)
		{
			return 0L;
		}

		public static long ParseForEasternTime(string time)
		{
			return 0L;
		}

		public static long ParseForTimeZone(string parsetime, int timezone)
		{
			return 0L;
		}

		public static long ParseForTimeZoneSinceEpoch(string parsetime, int timezone)
		{
			return 0L;
		}

		public static long ConstantTimeStamp()
		{
			return 0L;
		}

		public static double ConstantTimeStampPrecise()
		{
			return 0.0;
		}

		public static long ConstantTimeStampFromTime(int year, int month, int day)
		{
			return 0L;
		}

		public static double ConstantTimeStampFromTimePrecise(int year, int month, int day)
		{
			return 0.0;
		}

		public static long ConstantTimeStampFromDate(DateTime dateTime)
		{
			return 0L;
		}

		public static double ConstantTimeStampFromDatePrecise(DateTime dateTime)
		{
			return 0.0;
		}

		public static long LocalTimeStamp()
		{
			return 0L;
		}

		public static double LocalTimeStampPrecise()
		{
			return 0.0;
		}

		public static long GenerateTimestampForSave(long timestamp)
		{
			return 0L;
		}

		public static long GenerateTimestampForSave()
		{
			return 0L;
		}

		public static long ExtractTimestampFromSave(long timestamp)
		{
			return 0L;
		}
	}
}
