using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager
{
	private const float DELAY_BETWEEN_ITEM_DROP = 5f;

	private const float DELAY_TICK = 1f;

	private Action dialogNeededCallback;

	private List<ItemDrop> itemDrops;

	private List<ItemDrop> pendingItemDrops;

	private Dictionary<string, Dictionary<string, object>> pickupTriggers;

	private float DelayItemDrop;

	private float delayBetweenItemDrop;

	private bool clearDrops;

	public Action DialogNeededCallback
	{
		set
		{
		}
	}

	public void AddPickupTrigger(Dictionary<string, object> newTrigger)
	{
	}

	public void RemovePickupTrigger(Identity dropID)
	{
	}

	public static void AddPickupTriggerToGameState(Dictionary<string, object> gamestate, Dictionary<string, object> newTrigger)
	{
	}

	public static void RemovePickupTriggerFromGameState(Dictionary<string, object> gamestate, string dropID)
	{
	}

	private void DoPickupTrigger(Game game, Identity dropID, Dictionary<string, object> triggerDict)
	{
	}

	public void ExecutePickupTrigger(Game game, Identity dropID)
	{
	}

	public void ExecuteAllPickupTriggers(Game game)
	{
	}

	public void AddDrops(Vector3 initialPosition, List<ItemDropCtor> dropCtors, List<Identity> dropIDs, Simulation simulation)
	{
	}

	private void OnDialogNeeded()
	{
	}

	public void OnUpdate(Session session, Camera camera, bool updateCollectionTimer)
	{
	}

	public void MarkForClearCurrentDrops()
	{
	}

	private void PickupAll()
	{
	}

	public bool ProcessTap(Session session, Ray ray)
	{
		return false;
	}
}
