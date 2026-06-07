using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class SubscriptionExpiredEvent : Jsonable
	{
		public string Sku { get; set; }

		public string ToJson()
		{
			return null;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			return null;
		}

		public static SubscriptionExpiredEvent CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static SubscriptionExpiredEvent CreateFromJson(string jsonMessage)
		{
			return null;
		}

		public static Dictionary<string, SubscriptionExpiredEvent> MapFromJson(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static List<SubscriptionExpiredEvent> ListFromJson(List<object> array)
		{
			return null;
		}
	}
}
