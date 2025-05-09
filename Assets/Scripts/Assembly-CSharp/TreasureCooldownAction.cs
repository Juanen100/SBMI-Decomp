using System.Collections.Generic;

public class TreasureCooldownAction : PersistedTriggerableAction
{
	public const string TREASURE_TIME = "tt";

	private ulong nextTreasureTime;

	private string persistName;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public TreasureCooldownAction(ulong nextTime, string persistName)
		: base("tt", Identity.Null())
	{
		nextTreasureTime = nextTime;
		this.persistName = persistName;
	}

	public new static TreasureCooldownAction FromDict(Dictionary<string, object> data)
	{
		ulong nextTime = TFUtils.LoadUlong(data, "time");
		string text = TFUtils.TryLoadString(data, "persistName");
		if (text == null)
		{
			text = "time_to_spawn";
		}
		return new TreasureCooldownAction(nextTime, text);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["time"] = nextTreasureTime;
		dictionary["persistName"] = persistName;
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		TreasureSpawner treasureSpawner = game.treasureManager.FindTreasureSpawner(persistName);
		treasureSpawner.Reset(nextTreasureTime);
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (dictionary.ContainsKey("treasure_state"))
		{
			((Dictionary<string, object>)dictionary["treasure_state"])[persistName] = nextTreasureTime;
		}
		else
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2[persistName] = nextTreasureTime;
			((Dictionary<string, object>)gameState["farm"])["treasure_state"] = dictionary2;
		}
		base.Confirm(gameState);
	}
}
