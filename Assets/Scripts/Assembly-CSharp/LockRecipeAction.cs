using System;
using System.Collections.Generic;

public class LockRecipeAction : PersistedTriggerableAction
{
	public const string LOCK_RECIPE = "lr";

	public int did;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public LockRecipeAction(int did)
		: base("lr", Identity.Null())
	{
		this.did = did;
	}

	public new static LockRecipeAction FromDict(Dictionary<string, object> data)
	{
		int num = TFUtils.LoadInt(data, "did");
		return new LockRecipeAction(num);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["did"] = did;
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		if (did >= 0 && game.craftManager.IsRecipeUnlocked(did))
		{
			game.craftManager.LockRecipe(did);
			base.Apply(game, utcNow);
		}
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["recipes"];
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			if (Convert.ToInt32(list[i]) == did)
			{
				list.RemoveAt(i);
				break;
			}
		}
		base.Confirm(gameState);
	}

	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}
}
