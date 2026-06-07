using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class PurchaseResponse : Jsonable
	{
		public string RequestId { get; set; }

		public AmazonUserData AmazonUserData { get; set; }

		public PurchaseReceipt PurchaseReceipt { get; set; }

		public string Status { get; set; }

		public string ToJson()
		{
			return null;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			return null;
		}

		public static PurchaseResponse CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static PurchaseResponse CreateFromJson(string jsonMessage)
		{
			return null;
		}

		public static Dictionary<string, PurchaseResponse> MapFromJson(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static List<PurchaseResponse> ListFromJson(List<object> array)
		{
			return null;
		}
	}
}
