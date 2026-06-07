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
			return null;
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
	}

	public override void SetDynamicProperties(ref Dictionary<string, object> propertiesDict)
	{
	}

	public override void PreActivate(Game game, SessionActionTracker action)
	{
	}
}
