using System;
using System.Collections.Generic;
using UnityEngine;

public class RmtProduct
{
	public string localizedprice;

	public string currencyCode;

	public string productId;

	public float price;

	public RmtProduct(Dictionary<string, object> data)
	{
		productId = TFUtils.LoadString(data, "productId");
		localizedprice = TFUtils.LoadString(data, "localizedprice");
		currencyCode = TFUtils.LoadString(data, "currencyCode", "USD");
		try
		{
			if (data.ContainsKey("price"))
			{
				price = TFUtils.LoadFloat(data, "price");
			}
			else
			{
				float.TryParse(localizedprice, out price);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("RmtProduct: Failed to parse price: " + ex.Message);
			price = 0f;
		}
	}
}
