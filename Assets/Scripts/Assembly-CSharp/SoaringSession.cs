public class SoaringSession
{
	public enum SessionType
	{
		OneWay = 0,
		PersistantOneWay = 1
	}

	public enum QueryType
	{
		Random = 0,
		List = 1,
		Range = 2,
		List2 = 3
	}

	public static class SoaringSessionStringList
	{
		private static string[] kSoaringSessionType;

		private static string[] kSoaringQueryType;

		static SoaringSessionStringList()
		{
		}

		public static string GetSoaringSessionString(SessionType type)
		{
			return null;
		}

		public static string GetSoaringSessionQueryTypeString(QueryType type)
		{
			return null;
		}
	}

	public static string GetSoaringSessionTypeString(SessionType type)
	{
		return null;
	}

	public static string GetSoaringSessionQueryTypeString(QueryType type)
	{
		return null;
	}
}
