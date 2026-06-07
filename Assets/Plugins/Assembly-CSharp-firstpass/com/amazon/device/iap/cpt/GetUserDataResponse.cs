using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class GetUserDataResponse : Jsonable
	{
		public string RequestId { get; set; }

		public AmazonUserData AmazonUserData { get; set; }

		public string Status { get; set; }

		public string ToJson()
		{
			return null;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			return null;
		}

		public static GetUserDataResponse CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static GetUserDataResponse CreateFromJson(string jsonMessage)
		{
			return null;
		}

		public static Dictionary<string, GetUserDataResponse> MapFromJson(Dictionary<string, object> jsonMap)
		{
			return null;
		}

		public static List<GetUserDataResponse> ListFromJson(List<object> array)
		{
			return null;
		}
	}
}
