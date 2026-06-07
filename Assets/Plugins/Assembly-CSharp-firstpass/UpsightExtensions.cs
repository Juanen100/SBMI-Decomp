using System;

public static class UpsightExtensions
{
	private static readonly DateTime UnixEpoch;

	public static long ToUnixTimestamp(this DateTime dateTime)
	{
		return 0L;
	}

	public static DateTime ToDateTime(this long timestamp)
	{
		return default(DateTime);
	}

	public static DateTime ToDateTime(this double timestamp)
	{
		return default(DateTime);
	}
}
