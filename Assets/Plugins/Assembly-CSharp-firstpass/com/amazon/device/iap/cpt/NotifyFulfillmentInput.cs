using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class NotifyFulfillmentInput : Jsonable
	{
		public string ReceiptId { get; set; }

		public string FulfillmentResult { get; set; }

		public string ToJson()
		{
			return null;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			return null;
		}

		public static NotifyFulfillmentInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static NotifyFulfillmentInput CreateFromJson(string jsonMessage)
		{
			return null;
		}

		public static Dictionary<string, NotifyFulfillmentInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static List<NotifyFulfillmentInput> ListFromJson(List<object> array)
		{
			return null;
		}
	}
}
