using System.Collections.Generic;

public abstract class PersistedTriggerableAction : PersistedActionBuffer.PersistedAction, ITriggerable
{
	protected TriggerableMixin triggerable;

	public abstract bool IsUserInitiated { get; }

	public PersistedTriggerableAction(string type, Identity target)
		: base(null, null)
	{
	}

	public override void Process(Game game)
	{
	}

	public override void Apply(Game game, ulong utcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}

	public virtual ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return null;
	}

	public virtual ITrigger CreateTrigger(string type)
	{
		return null;
	}
}
