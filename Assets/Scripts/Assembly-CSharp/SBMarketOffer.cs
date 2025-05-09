using System;
using System.Collections.Generic;

public class SBMarketOffer
{
	public string type;

	public int identity;

	public bool itemLocked;

	public string itemName;

	public string description;

	public string innerOffer;

	public string material;

	public string texture;

	public string buttonTexture;

	public Dictionary<int, int> cost;

	public Dictionary<int, int> data;

	public int width;

	public int height;

	public string resultType;

	public int microEventDID;

	public bool microEventOnly;

	public bool isSaleItem;

	public bool isNewItem;

	public bool isLimitedItem;

	public float salePercent;

	public SBMarketOffer(Dictionary<string, object> offer)
	{
		identity = Convert.ToInt32(offer["identity"]);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)offer["cost"];
		cost = new Dictionary<int, int>();
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			cost[Convert.ToInt32(item.Key)] = Convert.ToInt32(item.Value);
		}
		if (offer.ContainsKey("type"))
		{
			type = TFUtils.LoadString(offer, "type");
		}
		if (offer.ContainsKey("name"))
		{
			itemName = Language.Get(TFUtils.LoadString(offer, "name"));
		}
		if (offer.ContainsKey("description"))
		{
			description = Language.Get(TFUtils.LoadString(offer, "description"));
		}
		if (offer.ContainsKey("code"))
		{
			innerOffer = TFUtils.LoadString(offer, "code");
		}
		if (offer.ContainsKey("result_type"))
		{
			resultType = TFUtils.LoadString(offer, "result_type");
		}
		if (offer.ContainsKey("data"))
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)offer["data"];
			data = new Dictionary<int, int>();
			foreach (KeyValuePair<string, object> item2 in dictionary2)
			{
				data[Convert.ToInt32(item2.Key)] = Convert.ToInt32(item2.Value);
			}
		}
		if (offer.ContainsKey("button_texture"))
		{
			buttonTexture = TFUtils.LoadString(offer, "button_texture");
		}
		if (offer.ContainsKey("micro_event_did"))
		{
			microEventDID = TFUtils.LoadInt(offer, "micro_event_did");
		}
		else
		{
			microEventDID = -1;
		}
		if (offer.ContainsKey("event_only"))
		{
			microEventOnly = TFUtils.LoadBool(offer, "event_only");
		}
		else
		{
			microEventOnly = false;
		}
		if (offer.ContainsKey("sale_banner"))
		{
			isSaleItem = TFUtils.LoadBool(offer, "sale_banner");
		}
		else
		{
			isSaleItem = false;
		}
		if (offer.ContainsKey("sale_percent"))
		{
			salePercent = TFUtils.LoadFloat(offer, "sale_percent");
		}
		if (offer.ContainsKey("new_banner"))
		{
			isNewItem = TFUtils.LoadBool(offer, "new_banner");
		}
		else
		{
			isNewItem = false;
		}
		if (offer.ContainsKey("limited_banner"))
		{
			isLimitedItem = TFUtils.LoadBool(offer, "limited_banner");
		}
		else
		{
			isLimitedItem = false;
		}
		if (offer.ContainsKey("display"))
		{
			dictionary = (Dictionary<string, object>)offer["display"];
			width = TFUtils.LoadInt(dictionary, "width");
			height = TFUtils.LoadInt(dictionary, "height");
			dictionary = (Dictionary<string, object>)offer["display.default"];
			if (dictionary.ContainsKey("texture"))
			{
				texture = (string)dictionary["texture"];
			}
			if (dictionary.ContainsKey("material"))
			{
				material = (string)dictionary["material"];
			}
		}
	}
}
