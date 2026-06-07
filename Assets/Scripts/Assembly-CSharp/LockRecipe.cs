using System.Collections.Generic;

public class LockRecipe : SessionActionDefinition
{
	public const string TYPE = "lock_recipe";

	public const string RECIPE_ID = "id";

	private int? recipeID;

	public static LockRecipe Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		return null;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public void Handle(Session session, SessionActionTracker action)
	{
	}
}
