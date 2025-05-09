using System.Collections.Generic;

public class AutoQuestAllDoneAction : PersistedTriggerableAction
{
	public const string AUTO_QUEST_ALL_DONE = "aqad";

	protected uint questId;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public AutoQuestAllDoneAction(uint questId)
		: base("aqad", Identity.Null())
	{
		this.questId = questId;
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dest = base.ToDict();
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["did"] = questId;
		return TFUtils.ConcatenateDictionaryInPlace(dest, dictionary);
	}

	public override void Process(Game game)
	{
		base.Process(game);
	}

	public override void Apply(Game game, ulong utcNow)
	{
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		base.Confirm(gameState);
	}

	protected void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		data["quest_id"] = (int)questId;
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}
}
