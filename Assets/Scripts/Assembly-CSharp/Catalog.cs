using System.Collections.Generic;

public class Catalog
{
	private Dictionary<string, object> catalogDict;

	private Dictionary<int, Cost> costs;

	private Dictionary<int, Cost> sellCosts;

	private Dictionary<int, string> descriptions;

	private Dictionary<string, Dictionary<string, object>> offersByCode;

	private List<string> premiumCodes;

	private Dictionary<int, bool> canSell;

	private Dictionary<int, string> sellErrors;

	public Dictionary<string, object> CatalogDict
	{
		get
		{
			return null;
		}
	}

	public List<string> PremiumCodes
	{
		get
		{
			return null;
		}
	}

	private void LoadCatalog()
	{
	}

	private Dictionary<string, object> LoadCatalogFromSpread()
	{
		return null;
	}

	private bool LoadItemCategoriesFromSpread(Dictionary<string, object> pData)
	{
		return false;
	}

	private bool LoadItemIdentitiesFromSpread(Dictionary<string, object> pData)
	{
		return false;
	}

	private bool LoadIAPIdentitiesFromSpread(Dictionary<string, object> pData)
	{
		return false;
	}

	public Dictionary<string, object> GetOfferByCode(string code)
	{
		return null;
	}

	private void LoadCostsHelper(Dictionary<int, Cost> costsDict, Dictionary<string, object> offerDict, string key)
	{
	}

	private Cost GetCostHelper(Dictionary<int, Cost> dict, int did)
	{
		return null;
	}

	public Cost GetCost(int did)
	{
		return null;
	}

	public Cost GetSellCost(int did)
	{
		return null;
	}

	public string GetDescription(int did)
	{
		return null;
	}

	public bool CanSell(int did)
	{
		return false;
	}

	public string SellError(int did)
	{
		return null;
	}

	public void GetNameAndTypeForDID(int nDID, out string sName, out string sType)
	{
		sName = null;
		sType = null;
	}

	public static string ConvertTypeToDeltaDNAType(string sType)
	{
		return null;
	}
}
