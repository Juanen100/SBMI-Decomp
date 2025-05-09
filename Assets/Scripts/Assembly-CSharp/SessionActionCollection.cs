using System.Collections.Generic;

public abstract class SessionActionCollection : SessionActionDefinition
{
	public const string COLLECTION = "collection";

	private const string ACTIONS = "actions";

	private ICollection<SessionActionDefinition> collection;

	public ICollection<SessionActionDefinition> Collection
	{
		get
		{
			return collection;
		}
	}

	public override bool ClearOnSessionChange
	{
		get
		{
			return false;
		}
	}

	protected virtual void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		List<object> list = TFUtils.LoadList<object>(data, "actions");
		collection = new List<SessionActionDefinition>();
		foreach (object item in list)
		{
			Dictionary<string, object> data2 = (Dictionary<string, object>)item;
			collection.Add(SessionActionFactory.Create(data2, new ConstantCondition(0u, true), originatedFromQuest, id++));
		}
	}

	public override void SetDynamicProperties(ref Dictionary<string, object> propertiesDict)
	{
		propertiesDict["collection"] = new List<SessionActionTracker>();
	}

	public override void PreActivate(Game game, SessionActionTracker action)
	{
		List<SessionActionTracker> list = new List<SessionActionTracker>();
		foreach (SessionActionDefinition item in Collection)
		{
			list.Add(new SessionActionTracker(item, new ConstantCondition(0u, true), action.Tag));
		}
		action.SetDynamic("collection", list);
		action.MarkStarted();
		base.PreActivate(game, action);
	}
}
