using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class ResetInput : Jsonable
	{
		public bool Reset { get; set; }

		public string ToJson()
		{
			return null;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			return null;
		}

		public static ResetInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static ResetInput CreateFromJson(string jsonMessage)
		{
			return null;
		}

		public static Dictionary<string, ResetInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static List<ResetInput> ListFromJson(List<object> array)
		{
			return null;
		}
	}
}
