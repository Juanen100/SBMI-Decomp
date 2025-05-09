using System.Collections.Generic;

public class LockRecipe : SessionActionDefinition
{
	public const string TYPE = "lock_recipe";

	public const string RECIPE_ID = "id";

	private int? recipeID;

	public static LockRecipe Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		LockRecipe lockRecipe = new LockRecipe();
		lockRecipe.Parse(data, id, startConditions, originatedFromQuest);
		return lockRecipe;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		recipeID = TFUtils.TryLoadInt(data, "id");
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		int? num = recipeID;
		dictionary["id"] = (num.HasValue ? recipeID : new int?(-1));
		return dictionary;
	}

	public void Handle(Session session, SessionActionTracker action)
	{
		action.MarkStarted();
		int? num = recipeID;
		if (num.HasValue)
		{
			int? num2 = recipeID;
			if ((!num2.HasValue || num2.Value >= 0) && session.TheGame.craftManager.IsRecipeUnlocked(recipeID.Value))
			{
				if (!session.TheGame.craftManager.LockRecipe(recipeID.Value))
				{
					action.MarkFailed();
					return;
				}
				session.TheGame.simulation.ModifyGameState(new LockRecipeAction(recipeID.Value));
				action.MarkSucceeded();
				return;
			}
		}
		action.MarkFailed();
	}
}
