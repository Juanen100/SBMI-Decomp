using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class SkuInput : Jsonable
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

		public static SkuInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static SkuInput CreateFromJson(string jsonMessage)
		{
			return null;
		}

		public static Dictionary<string, SkuInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static List<SkuInput> ListFromJson(List<object> array)
		{
			return null;
		}
	}
}
