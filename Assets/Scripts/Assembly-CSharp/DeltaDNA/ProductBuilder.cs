using System;
using System.Collections.Generic;

namespace DeltaDNA
{
	public class ProductBuilder
	{
		private Dictionary<string, object> realCurrency;

		private List<Dictionary<string, object>> virtualCurrencies;

		private List<Dictionary<string, object>> items;

		public ProductBuilder AddRealCurrency(string currencyType, int currencyAmount)
		{
			if (realCurrency != null)
			{
				throw new InvalidOperationException("A Product may only have one real currency");
			}
			realCurrency = new Dictionary<string, object>
			{
				{ "realCurrencyType", currencyType },
				{ "realCurrencyAmount", currencyAmount }
			};
			return this;
		}

		public ProductBuilder AddVirtualCurrency(string currencyName, string currencyType, int currencyAmount)
		{
			if (virtualCurrencies == null)
			{
				virtualCurrencies = new List<Dictionary<string, object>>();
			}
			virtualCurrencies.Add(new Dictionary<string, object> { 
			{
				"virtualCurrency",
				new Dictionary<string, object>
				{
					{ "virtualCurrencyName", currencyName },
					{ "virtualCurrencyType", currencyType },
					{ "virtualCurrencyAmount", currencyAmount }
				}
			} });
			return this;
		}

		public ProductBuilder AddItem(string itemName, string itemType, int itemAmount)
		{
			if (items == null)
			{
				items = new List<Dictionary<string, object>>();
			}
			items.Add(new Dictionary<string, object> { 
			{
				"item",
				new Dictionary<string, object>
				{
					{ "itemName", itemName },
					{ "itemType", itemType },
					{ "itemAmount", itemAmount }
				}
			} });
			return this;
		}

		public Dictionary<string, object> ToDictionary()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (realCurrency != null)
			{
				dictionary.Add("realCurrency", realCurrency);
			}
			if (virtualCurrencies != null)
			{
				dictionary.Add("virtualCurrencies", virtualCurrencies);
			}
			if (items != null)
			{
				dictionary.Add("items", items);
			}
			return dictionary;
		}
	}
}
