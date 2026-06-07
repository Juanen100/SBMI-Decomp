using System.Collections.Generic;

public abstract class SessionActionDefinition
{
	private const string TYPE = "type";

	private const string START_CONDITIONS = "start_conditions";

	protected const string SUCCEED_CONDITIONS = "end_conditions";

	private const string FAILPROOF = "failproof";

	private const string SOUND = "sound";

	private const string SLAVE = "slave";

	private const string POSTPONE = "postpone";

	private uint id;

	private string type;

	private ICondition startConditions;

	private ICondition succeedConditions;

	private bool usingDefaultSucceedConditions;

	private bool isFailproof;

	private string sound;

	private float postpone;

	private SessionActionDefinition slave;

	public uint Id
	{
		get
		{
			return 0u;
		}
	}

	public string Type
	{
		get
		{
			return null;
		}
	}

	public string Sound
	{
		get
		{
			return null;
		}
	}

	public float Postpone
	{
		get
		{
			return 0f;
		}
	}

	public ICondition StartConditions
	{
		get
		{
			return null;
		}
	}

	public ICondition SucceedConditions
	{
		get
		{
			return null;
		}
	}

	public virtual bool RepeatOnFail
	{
		get
		{
			return false;
		}
	}

	public virtual bool IsFailproof
	{
		get
		{
			return false;
		}
	}

	public SessionActionDefinition Slave
	{
		get
		{
			return null;
		}
	}

	public virtual bool ClearOnSessionChange
	{
		get
		{
			return false;
		}
	}

	public bool UsingDefaultSucceedConditions
	{
		get
		{
			return false;
		}
	}

	public virtual Dictionary<string, object> ToDict()
	{
		return null;
	}

	protected virtual void Parse(Dictionary<string, object> actionData, uint id, ICondition startConditions, ICondition defaultSuccessConditions, uint originatedFromQuest)
	{
	}

	public virtual void PreActivate(Game game, SessionActionTracker action)
	{
	}

	public virtual bool ActiveProcess(Game game, SessionActionTracker action)
	{
		return false;
	}

	public virtual void PostComplete(Game game, SessionActionTracker action)
	{
	}

	public virtual void SetDynamicProperties(ref Dictionary<string, object> propertiesDict)
	{
	}

	public virtual void OnObliterate(Game game, SessionActionTracker tracker)
	{
	}

	public virtual void OnDestroy(Game game)
	{
	}

	public override string ToString()
	{
		return null;
	}
}
