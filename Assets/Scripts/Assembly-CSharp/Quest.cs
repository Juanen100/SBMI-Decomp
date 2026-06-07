using System.Collections.Generic;

public class Quest
{
	private uint did;

	private ConditionalProgress startProgress;

	private ConditionalProgress endProgress;

	private ConditionState startConditions;

	private List<ConditionState> endConditions;

	private ulong? startTime;

	private ulong? completionTime;

	private bool triggeredReminder;

	public uint Did
	{
		get
		{
			return 0u;
		}
	}

	public ConditionState StartConditions
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public List<ConditionState> EndConditions
	{
		get
		{
			return null;
		}
	}

	public ConditionalProgress StartProgress
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public ConditionalProgress EndProgress
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public ulong? StartTime
	{
		get
		{
			return null;
		}
	}

	public ulong? CompletionTime
	{
		get
		{
			return null;
		}
	}

	public bool TriggeredReminder
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public string TrackerTag
	{
		get
		{
			return null;
		}
	}

	public Quest(uint did, ConditionalProgress startProgress, ConditionalProgress endProgress, ulong? startTime, ulong? completionTime, bool triggeredAlready)
	{
	}

	public void Start(ulong utcTime)
	{
	}

	public void Complete(ulong utcTime)
	{
	}

	public static Quest FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public Dictionary<string, object> ToDict()
	{
		return null;
	}

	public SessionActionTracker InstantiateSessionAction(SessionActionDefinition definition)
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}
}
