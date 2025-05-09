using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager
{
	private const float DELAY_BETWEEN_ITEM_DROP = 5f;

	private const float DELAY_TICK = 1f;

	private volatile Action dialogNeededCallback;

	private List<ItemDrop> itemDrops = new List<ItemDrop>();

	private List<ItemDrop> pendingItemDrops = new List<ItemDrop>();

	private Dictionary<string, Dictionary<string, object>> pickupTriggers = new Dictionary<string, Dictionary<string, object>>();

	private float DelayItemDrop;

	private float delayBetweenItemDrop = UnityEngine.Random.Range(4f, 7f);

	private bool clearDrops;

	public Action DialogNeededCallback
	{
		set
		{
			dialogNeededCallback = value;
		}
	}

	public void AddPickupTrigger(Dictionary<string, object> newTrigger)
	{
		if (newTrigger.ContainsKey("dropID"))
		{
			string key = (string)newTrigger["dropID"];
			pickupTriggers[key] = newTrigger;
		}
	}

	public void RemovePickupTrigger(Identity dropID)
	{
		string key = dropID.Describe();
		pickupTriggers.Remove(key);
	}

	public static void AddPickupTriggerToGameState(Dictionary<string, object> gamestate, Dictionary<string, object> newTrigger)
	{
		((List<object>)((Dictionary<string, object>)gamestate["farm"])["drop_pickups"]).Add(newTrigger);
	}

	public static void RemovePickupTriggerFromGameState(Dictionary<string, object> gamestate, string dropID)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gamestate["farm"];
		((List<object>)dictionary["drop_pickups"]).RemoveAll(delegate(object obj)
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)obj;
			return dictionary2.ContainsKey("dropID") && (string)dictionary2["dropID"] == dropID;
		});
	}

	private void DoPickupTrigger(Game game, Identity dropID, Dictionary<string, object> triggerDict)
	{
		if (triggerDict.ContainsKey("target"))
		{
			Identity id = new Identity((string)triggerDict["target"]);
			PickupDropAction action = new PickupDropAction(id, dropID);
			game.Record(action);
			ITrigger trigger = Trigger.FromDict(triggerDict);
			game.triggerRouter.RouteTrigger(trigger, game);
		}
	}

	public void ExecutePickupTrigger(Game game, Identity dropID)
	{
		string key = dropID.Describe();
		if (pickupTriggers.ContainsKey(key))
		{
			Dictionary<string, object> triggerDict = pickupTriggers[key];
			pickupTriggers.Remove(key);
			DoPickupTrigger(game, dropID, triggerDict);
		}
	}

	public void ExecuteAllPickupTriggers(Game game)
	{
		foreach (KeyValuePair<string, Dictionary<string, object>> pickupTrigger in pickupTriggers)
		{
			Dictionary<string, object> value = pickupTrigger.Value;
			Identity dropID = new Identity(pickupTrigger.Key);
			DoPickupTrigger(game, dropID, value);
		}
		pickupTriggers.Clear();
	}

	public void AddDrops(Vector3 initialPosition, List<ItemDropCtor> dropCtors, List<Identity> dropIDs, Simulation simulation)
	{
		Ray ray = simulation.TheCamera.ScreenPointToRay(Vector3.zero);
		float num = (initialPosition - simulation.TheCamera.transform.position).magnitude * 0.5f;
		Vector3 fixedOffset = ray.direction * (0f - num);
		foreach (ItemDropCtor dropCtor in dropCtors)
		{
			Vector3 direction = new Vector3(UnityEngine.Random.Range(-1f, 0f), UnityEngine.Random.Range(-1f, 0f), 1f);
			direction.Normalize();
			ItemDrop itemDrop = dropCtor.CreateItemDrop(initialPosition, fixedOffset, direction, OnDialogNeeded);
			pendingItemDrops.Add(itemDrop);
			dropIDs.Add(itemDrop.DropID);
		}
	}

	private void OnDialogNeeded()
	{
		dialogNeededCallback();
	}

	public void OnUpdate(Session session, Camera camera, bool updateCollectionTimer)
	{
		if (clearDrops)
		{
			clearDrops = false;
			PickupAll();
		}
		if (DelayItemDrop <= 0f)
		{
			if (pendingItemDrops.Count != 0)
			{
				int index = pendingItemDrops.Count - 1;
				ItemDrop item = pendingItemDrops[index];
				pendingItemDrops.RemoveAt(index);
				itemDrops.Add(item);
			}
			DelayItemDrop = delayBetweenItemDrop;
		}
		DelayItemDrop -= 1f;
		int count = itemDrops.Count;
		if (count == 0)
		{
			return;
		}
		List<ItemDrop> list = null;
		List<ItemDrop> list2 = new List<ItemDrop>(itemDrops);
		for (int i = 0; i < count; i++)
		{
			if (!list2[i].OnUpdate(session, camera, updateCollectionTimer))
			{
				if (list == null)
				{
					list = new List<ItemDrop>();
				}
				list.Add(list2[i]);
			}
		}
		if (list != null)
		{
			int count2 = list.Count;
			for (int j = 0; j < count2; j++)
			{
				itemDrops.Remove(list[j]);
			}
		}
	}

	public void MarkForClearCurrentDrops()
	{
		clearDrops = true;
	}

	private void PickupAll()
	{
		int count = pendingItemDrops.Count;
		for (int i = 0; i < count; i++)
		{
			itemDrops.Add(pendingItemDrops[i]);
		}
		pendingItemDrops.Clear();
		count = itemDrops.Count;
		for (int j = 0; j < count; j++)
		{
			itemDrops[j].AutoPickup();
		}
	}

	public bool ProcessTap(Session session, Ray ray)
	{
		bool result = false;
		int count = itemDrops.Count;
		for (int i = 0; i < count; i++)
		{
			if (itemDrops[i].HandleTap(session, ray))
			{
				result = true;
			}
		}
		return result;
	}
}
