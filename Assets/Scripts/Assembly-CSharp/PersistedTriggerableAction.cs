using System.Collections.Generic;

public abstract class PersistedTriggerableAction : PersistedActionBuffer.PersistedAction, ITriggerable
{
	protected TriggerableMixin triggerable = new TriggerableMixin();

	public abstract bool IsUserInitiated { get; }

	public PersistedTriggerableAction(string type, Identity target)
		: base(type, target)
	{
	}

	public override void Process(Game game)
	{
	}

	public override void Apply(Game game, ulong utcNow)
	{
		if (IsUserInitiated)
		{
			game.playtimeRegistrar.UpdatePlaytime(GetTime());
		}
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		if (IsUserInitiated)
		{
			Dictionary<string, object> dictionary = TFUtils.LoadDict(gameState, "playtime");
			ulong num = TFUtils.LoadUlong(dictionary, "time_at");
			ulong utcLast = TFUtils.LoadUlong(dictionary, "last");
			ulong delta;
			if (!PlaytimeRegistrar.IsTimeout(utcLast, GetTime(), out delta))
			{
				num += delta;
				dictionary["time_at"] = num;
			}
			dictionary["last"] = GetTime();
		}
		base.Confirm(gameState);
	}

	public virtual ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return Trigger.Null;
	}

	public virtual ITrigger CreateTrigger(string type)
	{
		return Trigger.Null;
	}
}
