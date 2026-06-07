using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class ProductData : Jsonable
	{
		public string Sku { get; set; }

		public string ProductType { get; set; }

		public string Price { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string SmallIconUrl { get; set; }

		public string ToJson()
		{
			return null;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			return null;
		}

		public static ProductData CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static ProductData CreateFromJson(string jsonMessage)
		{
			return null;
		}

		public static Dictionary<string, ProductData> MapFromJson(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static List<ProductData> ListFromJson(List<object> array)
		{
			return null;
		}
	}
}
