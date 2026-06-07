using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class SkusInput : Jsonable
	{
		public List<string> Skus { get; set; }

		public string ToJson()
		{
			return null;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			return null;
		}

		public static SkusInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static SkusInput CreateFromJson(string jsonMessage)
		{
			return null;
		}

		public static Dictionary<string, SkusInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static List<SkusInput> ListFromJson(List<object> array)
		{
			return null;
		}
	}
}
