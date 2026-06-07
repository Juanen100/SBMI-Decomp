using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class PurchaseReceipt : Jsonable
	{
		public string ReceiptId { get; set; }

		public long CancelDate { get; set; }

		public long PurchaseDate { get; set; }

		public string Sku { get; set; }

		public string ProductType { get; set; }

		public string ToJson()
		{
			return null;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			return null;
		}

		public static PurchaseReceipt CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static PurchaseReceipt CreateFromJson(string jsonMessage)
		{
			return null;
		}

		public static Dictionary<string, PurchaseReceipt> MapFromJson(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static List<PurchaseReceipt> ListFromJson(List<object> array)
		{
			return null;
		}
	}
}
