using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public abstract class Jsonable
	{
		public static Dictionary<string, object> unrollObjectIntoMap<T>(Dictionary<string, T> obj) where T : Jsonable
		{
			return null;
		}

		public static List<object> unrollObjectIntoList<T>(List<T> obj) where T : Jsonable
		{
			return null;
		}

		public abstract Dictionary<string, object> GetObjectDictionary();

		public static void CheckForErrors(Dictionary<string, object> jsonMap)
		{
		}
	}
}
