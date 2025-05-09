using System;
using System.Collections.Generic;

public class RestockVendorAction : PersistedSimulatedAction
{
	public const string VENDOR_RESTOCK = "vr";

	private Dictionary<string, object> generalInstances;

	private Dictionary<string, object> specialInstances;

	private ulong nextRestock;

	private ulong nextSpecialRestock;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public RestockVendorAction(Identity id, ulong restockTime, ulong specialRestockTime, Dictionary<string, object> generalInstances, Dictionary<string, object> specialInstances)
		: base("vr", id, typeof(RestockVendorAction).ToString())
	{
		nextRestock = restockTime;
		nextSpecialRestock = specialRestockTime;
		this.generalInstances = generalInstances;
		this.specialInstances = specialInstances;
	}

	public static RestockVendorAction Create(Identity id, ulong restockTime, ulong specialRestockTime, Dictionary<int, VendingInstance> generalInstances, Dictionary<int, VendingInstance> specialInstances)
	{
		Dictionary<string, object> dictionary;
		if (generalInstances != null)
		{
			dictionary = new Dictionary<string, object>(generalInstances.Count);
			foreach (KeyValuePair<int, VendingInstance> generalInstance in generalInstances)
			{
				dictionary.Add(generalInstance.Key.ToString(), generalInstance.Value.ToDict());
			}
		}
		else
		{
			dictionary = new Dictionary<string, object>();
		}
		Dictionary<string, object> dictionary2;
		if (specialInstances != null)
		{
			dictionary2 = new Dictionary<string, object>(specialInstances.Count);
			foreach (KeyValuePair<int, VendingInstance> specialInstance in specialInstances)
			{
				dictionary2.Add(specialInstance.Key.ToString(), specialInstance.Value.ToDict());
			}
		}
		else
		{
			dictionary2 = new Dictionary<string, object>();
		}
		return new RestockVendorAction(id, restockTime, specialRestockTime, dictionary, dictionary2);
	}

	public new static RestockVendorAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		ulong restockTime = TFUtils.LoadUlong(data, "general_restock");
		ulong specialRestockTime = TFUtils.LoadUlong(data, "special_restock");
		Dictionary<string, object> dictionary = ((!data.ContainsKey("general_instances")) ? new Dictionary<string, object>() : TFUtils.LoadDict(data, "general_instances"));
		Dictionary<string, object> dictionary2 = ((!data.ContainsKey("special_instances")) ? new Dictionary<string, object>() : TFUtils.LoadDict(data, "special_instances"));
		return new RestockVendorAction(id, restockTime, specialRestockTime, dictionary, dictionary2);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["general_restock"] = nextRestock;
		if (generalInstances != null)
		{
			dictionary["general_instances"] = generalInstances;
		}
		dictionary["special_restock"] = nextSpecialRestock;
		if (specialInstances != null)
		{
			dictionary["special_instances"] = specialInstances;
		}
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		VendingDecorator vendingDecorator = null;
		Simulated simulated = game.simulation.FindSimulated(target);
		if (simulated != null)
		{
			vendingDecorator = simulated.GetEntity<VendingDecorator>();
		}
		if (vendingDecorator != null)
		{
			vendingDecorator.RestockTime = nextRestock;
			vendingDecorator.SpecialRestockTime = nextSpecialRestock;
		}
		game.vendingManager.LoadVendorInstances(target, generalInstances, specialInstances);
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["vending"];
		string targetString = target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find(match);
		if (dictionary == null)
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2.Add("label", targetString);
			dictionary = dictionary2;
			list.Add(dictionary);
		}
		if (generalInstances != null)
		{
			dictionary["general_instances"] = generalInstances;
		}
		if (specialInstances != null)
		{
			dictionary["special_instances"] = specialInstances;
		}
		List<object> list2 = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		Dictionary<string, object> dictionary3 = (Dictionary<string, object>)list2.Find(match);
		if (dictionary3 != null)
		{
			dictionary3["general_restock"] = nextRestock;
			dictionary3["special_restock"] = nextSpecialRestock;
		}
		base.Confirm(gameState);
	}
}
