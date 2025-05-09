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
			return id;
		}
	}

	public string Type
	{
		get
		{
			return type;
		}
	}

	public string Sound
	{
		get
		{
			return sound;
		}
	}

	public float Postpone
	{
		get
		{
			return postpone;
		}
	}

	public ICondition StartConditions
	{
		get
		{
			return startConditions;
		}
	}

	public ICondition SucceedConditions
	{
		get
		{
			return succeedConditions;
		}
	}

	public virtual bool RepeatOnFail
	{
		get
		{
			return true;
		}
	}

	public virtual bool IsFailproof
	{
		get
		{
			return isFailproof;
		}
	}

	public SessionActionDefinition Slave
	{
		get
		{
			return slave;
		}
	}

	public virtual bool ClearOnSessionChange
	{
		get
		{
			return true;
		}
	}

	public bool UsingDefaultSucceedConditions
	{
		get
		{
			return usingDefaultSucceedConditions;
		}
	}

	public virtual Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("type", type);
		dictionary.Add("sound", sound);
		dictionary.Add("postpone", postpone);
		if (!usingDefaultSucceedConditions)
		{
			dictionary.Add("end_conditions", ((LoadableCondition)succeedConditions).ToDict());
		}
		dictionary.Add("failproof", isFailproof ? 1 : 0);
		if (slave != null)
		{
			dictionary["slave"] = slave.ToDict();
		}
		return dictionary;
	}

	protected virtual void Parse(Dictionary<string, object> actionData, uint id, ICondition startConditions, ICondition defaultSuccessConditions, uint originatedFromQuest)
	{
		this.id = id;
		this.startConditions = startConditions;
		type = TFUtils.LoadString(actionData, "type");
		if (actionData.ContainsKey("failproof"))
		{
			isFailproof = ((TFUtils.LoadInt(actionData, "failproof") != 0) ? true : false);
		}
		if (actionData.ContainsKey("sound"))
		{
			sound = TFUtils.LoadString(actionData, "sound");
		}
		else
		{
			sound = null;
		}
		postpone = ((!actionData.ContainsKey("postpone")) ? 0f : TFUtils.LoadFloat(actionData, "postpone"));
		if (actionData.ContainsKey("end_conditions"))
		{
			succeedConditions = ConditionFactory.FromDict(TFUtils.LoadDict(actionData, "end_conditions"));
			usingDefaultSucceedConditions = false;
		}
		else
		{
			succeedConditions = defaultSuccessConditions;
			usingDefaultSucceedConditions = true;
		}
		if (actionData.ContainsKey("slave"))
		{
			slave = SessionActionFactory.Create(TFUtils.LoadDict(actionData, "slave"), new DumbCondition(0u), originatedFromQuest, 0u);
		}
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
		return string.Concat("SessionActionDefinition:(id=", id, ", type=", type, ", start_conditions=", startConditions.ToString(), ", succeed_conditions=", succeedConditions.ToString(), ", sound=", sound, ", postpone=", postpone, ", failproof=", isFailproof, ", slave=", slave, ")");
	}
}
