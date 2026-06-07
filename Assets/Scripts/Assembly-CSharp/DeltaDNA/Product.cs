using System.Collections.Generic;

namespace DeltaDNA
{
	public class Product<T> : Params where T : Product<T>
	{
		private List<Dictionary<string, object>> virtualCurrencies;

		private List<Dictionary<string, object>> items;

		private static readonly IDictionary<string, int> ISO4217;

		static Product()
		{
		}

		public T SetRealCurrency(string type, int amount)
		{
			return null;
		}

		public T AddVirtualCurrency(string name, string type, long amount)
		{
			return null;
		}

		public T AddItem(string name, string type, int amount)
		{
			return null;
		}

		public static int ConvertCurrency(string code, decimal value)
		{
			return 0;
		}
	}
	public class Product : Product<Product>
	{
	}
}
