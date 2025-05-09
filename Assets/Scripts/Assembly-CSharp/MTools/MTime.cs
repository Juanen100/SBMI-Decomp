using System;

namespace MTools
{
	public static class MTime
	{
		public const uint cSecondsInDay = 86400u;

		public const uint cSecondsInHour = 3600u;

		public const int TimeZone_UTC = 0;

		public const int TimeZone_Eastern = -5;

		public const int TimeZone_Central = -6;

		public const int TimeZone_Mountain = -7;

		public const int TimeZone_Pacific = -7;

		private static DateTime sOriginTimeStamp = new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private static DateTime sLocalOriginTimestamp = DateTime.UtcNow;

		private static uint sTimeStampAdjust = 11411671u;

		private static double sCurrentTime = 0.0;

		public static void LoadCurrentTime()
		{
		}

		public static void SetCurrentTime(ulong cTime)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dateTime.AddMilliseconds(sCurrentTime);
			double num = (dateTime - sOriginTimeStamp).TotalSeconds;
			if (num > sCurrentTime)
			{
				sCurrentTime = num;
			}
		}

		public static bool ConstantTimestampWithinDays(int days, long ts)
		{
			long num = ConstantTimeStamp();
			return ts - 86400L * (long)days <= num && ts >= num;
		}

		public static long CurrentTimeSinceEpoch()
		{
			return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
		}

		public static long TimeSinceEpoch(string time)
		{
			return (long)(DateTime.Parse(time) - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
		}

		public static long TimeSinceEpoch(DateTime time)
		{
			return (long)(time - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
		}

		public static long ParseForEasternTime(string time)
		{
			return ParseForTimeZone(time, -5);
		}

		public static long ParseForTimeZone(string parsetime, int timezone)
		{
			DateTime dateTime = DateTime.Parse(parsetime);
			if (dateTime.Kind != DateTimeKind.Utc)
			{
				dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, DateTimeKind.Utc);
			}
			long num = ConstantTimeStampFromDate(dateTime);
			return num + 3600L * (long)timezone;
		}

		public static long ParseForTimeZoneSinceEpoch(string parsetime, int timezone)
		{
			DateTime time = DateTime.Parse(parsetime);
			if (time.Kind != DateTimeKind.Utc)
			{
				time = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, DateTimeKind.Utc);
			}
			long num = TimeSinceEpoch(time);
			return num + 3600L * (long)timezone;
		}

		public static long ConstantTimeStamp()
		{
			return (long)(DateTime.UtcNow - sOriginTimeStamp).TotalSeconds;
		}

		public static double ConstantTimeStampPrecise()
		{
			return (DateTime.UtcNow - sOriginTimeStamp).TotalSeconds;
		}

		public static long ConstantTimeStampFromTime(int year, int month, int day)
		{
			return (long)(new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc) - sOriginTimeStamp).TotalSeconds;
		}

		public static double ConstantTimeStampFromTimePrecise(int year, int month, int day)
		{
			return (new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc) - sOriginTimeStamp).TotalSeconds;
		}

		public static long ConstantTimeStampFromDate(DateTime dateTime)
		{
			if (dateTime.Kind != DateTimeKind.Utc)
			{
				dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
			}
			return (long)(dateTime - sOriginTimeStamp).TotalSeconds;
		}

		public static double ConstantTimeStampFromDatePrecise(DateTime dateTime)
		{
			if (dateTime.Kind != DateTimeKind.Utc)
			{
				dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
			}
			return (dateTime - sOriginTimeStamp).TotalSeconds;
		}

		public static long LocalTimeStamp()
		{
			return (long)(DateTime.UtcNow - sLocalOriginTimestamp).TotalSeconds;
		}

		public static double LocalTimeStampPrecise()
		{
			return (DateTime.UtcNow - sLocalOriginTimestamp).TotalSeconds;
		}

		public static long GenerateTimestampForSave(long timestamp)
		{
			long num = timestamp + sTimeStampAdjust;
			long num2 = (timestamp & 0xFF) >> 24;
			long num3 = timestamp & 0xFF;
			num = timestamp & 0xFFFFFFFFFFFF00L;
			num |= num2;
			return num | (num3 << 24);
		}

		public static long GenerateTimestampForSave()
		{
			long num = ConstantTimeStamp() + sTimeStampAdjust;
			long num2 = (num & 0xFF) >> 24;
			long num3 = num & 0xFF;
			num &= 0xFFFFFFFFFFFF00L;
			num |= num2;
			return num | (num3 << 24);
		}

		public static long ExtractTimestampFromSave(long timestamp)
		{
			long num = timestamp;
			long num2 = (num & 0xFF) >> 24;
			long num3 = num & 0xFF;
			num &= 0xFFFFFFFFFFFF00L;
			num |= num2;
			num |= num3 << 24;
			return num - sTimeStampAdjust;
		}
	}
}
