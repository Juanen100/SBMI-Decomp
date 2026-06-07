using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class AmazonUserData : Jsonable
	{
		public string UserId { get; set; }

		public string Marketplace { get; set; }

		public string ToJson()
		{
			return null;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			return null;
		}

		public static AmazonUserData CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static AmazonUserData CreateFromJson(string jsonMessage)
		{
			return null;
		}

		public static Dictionary<string, AmazonUserData> MapFromJson(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static List<AmazonUserData> ListFromJson(List<object> array)
		{
			return null;
		}
	}
}
