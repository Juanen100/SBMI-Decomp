using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class GetProductDataResponse : Jsonable
	{
		public string RequestId { get; set; }

		public Dictionary<string, ProductData> ProductDataMap { get; set; }

		public List<string> UnavailableSkus { get; set; }

		public string Status { get; set; }

		public string ToJson()
		{
			return null;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			return null;
		}

		public static GetProductDataResponse CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static GetProductDataResponse CreateFromJson(string jsonMessage)
		{
			return null;
		}

		public static Dictionary<string, GetProductDataResponse> MapFromJson(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static List<GetProductDataResponse> ListFromJson(List<object> array)
		{
			return null;
		}
	}
}
