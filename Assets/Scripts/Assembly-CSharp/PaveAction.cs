using System.Collections.Generic;

public class PaveAction : PersistedTriggerableAction
{
	public class PaveElement
	{
		public GridPosition position;

		public PaveElement(GridPosition position)
		{
		}
	}

	public const string PAVE = "np";

	public List<PaveElement> path;

	public Cost cost;

	public TriggerableMixin Triggerable
	{
		get
		{
			return null;
		}
	}

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public PaveAction(Identity id, List<PaveElement> path, Cost cost)
		: base(null, null)
	{
	}

	public new static PaveAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Apply(Game game, ulong utcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}

	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return null;
	}
}
