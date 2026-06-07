using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class RequestOutput : Jsonable
	{
		public string RequestId { get; set; }

		public string ToJson()
		{
			return null;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			return null;
		}

		public static RequestOutput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static RequestOutput CreateFromJson(string jsonMessage)
		{
			return null;
		}

		public static Dictionary<string, RequestOutput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static List<RequestOutput> ListFromJson(List<object> array)
		{
			return null;
		}
	}
}
