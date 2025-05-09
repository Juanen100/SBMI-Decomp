using System;
using System.Collections.Generic;

public class DebrisCompleteAction : PersistedSimulatedAction
{
	public const string DEBRIS_COMPLETE = "dc";

	private Reward reward;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public DebrisCompleteAction(Identity id, Reward reward)
		: base("dc", id, typeof(DebrisCompleteAction).ToString())
	{
		this.reward = reward;
	}

	public new static DebrisCompleteAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Reward reward = Reward.FromObject(data["reward"]);
		DebrisCompleteAction debrisCompleteAction = new DebrisCompleteAction(id, reward);
		debrisCompleteAction.DropTargetDataFromDict(data);
		return debrisCompleteAction;
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["reward"] = reward.ToDict();
		DropTargetDataToDict(dictionary);
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		simulated.SetFootprint(game.simulation, false);
		game.simulation.RemoveSimulated(simulated);
		game.entities.Destroy(target);
		game.ApplyReward(reward, GetTime());
		AddPickup(game.simulation);
		simulated.ClearPendingCommands();
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		try
		{
			List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["debris"];
			string targetString = target.Describe();
			Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
			object obj = list.Find(match);
			if (obj == null)
			{
				string message = "DebrisCompleteAction.Confirm - No Debris Found: " + targetString;
				TFUtils.ErrorLog(message);
				throw new Exception(message);
			}
			list.Remove(obj);
			RewardManager.ApplyToGameState(reward, GetTime(), gameState);
			AddPickupToGameState(gameState);
			base.Confirm(gameState);
		}
		catch (Exception message2)
		{
			TFUtils.ErrorLog(message2);
		}
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
	}
}
