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
			kSoaringSessionType = new string[2];
			kSoaringSessionType[0] = "one-way";
			kSoaringSessionType[1] = "persistent-one-way";
			kSoaringQueryType = new string[4];
			kSoaringQueryType[0] = "random";
			kSoaringQueryType[2] = "range";
			kSoaringQueryType[1] = "list";
			kSoaringQueryType[3] = "list2";
		}

		public static string GetSoaringSessionString(SessionType type)
		{
			return kSoaringSessionType[(int)type];
		}

		public static string GetSoaringSessionQueryTypeString(QueryType type)
		{
			return kSoaringQueryType[(int)type];
		}
	}

	public static string GetSoaringSessionTypeString(SessionType type)
	{
		return SoaringSessionStringList.GetSoaringSessionString(type);
	}

	public static string GetSoaringSessionQueryTypeString(QueryType type)
	{
		return SoaringSessionStringList.GetSoaringSessionQueryTypeString(type);
	}
}
