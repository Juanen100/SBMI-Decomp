using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class GetPurchaseUpdatesResponse : Jsonable
	{
		public string RequestId { get; set; }

		public AmazonUserData AmazonUserData { get; set; }

		public List<PurchaseReceipt> Receipts { get; set; }

		public string Status { get; set; }

		public bool HasMore { get; set; }

		public string ToJson()
		{
			return null;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			return null;
		}

		public static GetPurchaseUpdatesResponse CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static GetPurchaseUpdatesResponse CreateFromJson(string jsonMessage)
		{
			return null;
		}

		public static Dictionary<string, GetPurchaseUpdatesResponse> MapFromJson(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static List<GetPurchaseUpdatesResponse> ListFromJson(List<object> array)
		{
			return null;
		}
	}
}
