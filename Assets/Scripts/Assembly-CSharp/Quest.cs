#define ASSERTS_ON
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
			return did;
		}
	}

	public ConditionState StartConditions
	{
		get
		{
			return startConditions;
		}
		set
		{
			startConditions = value;
		}
	}

	public List<ConditionState> EndConditions
	{
		get
		{
			return endConditions;
		}
	}

	public ConditionalProgress StartProgress
	{
		get
		{
			return startProgress;
		}
		set
		{
			startProgress = value;
		}
	}

	public ConditionalProgress EndProgress
	{
		get
		{
			return endProgress;
		}
		set
		{
			endProgress = value;
		}
	}

	public ulong? StartTime
	{
		get
		{
			return startTime;
		}
	}

	public ulong? CompletionTime
	{
		get
		{
			return completionTime;
		}
	}

	public bool TriggeredReminder
	{
		get
		{
			return triggeredReminder;
		}
		set
		{
			triggeredReminder = value;
		}
	}

	public string TrackerTag
	{
		get
		{
			return "quest_" + did;
		}
	}

	public Quest(uint did, ConditionalProgress startProgress, ConditionalProgress endProgress, ulong? startTime, ulong? completionTime, bool triggeredAlready)
	{
		this.did = did;
		this.startTime = startTime;
		this.completionTime = completionTime;
		this.startProgress = startProgress;
		this.endProgress = endProgress;
		triggeredReminder = triggeredAlready;
		endConditions = new List<ConditionState>();
	}

	public void Start(ulong utcTime)
	{
		startTime = utcTime;
	}

	public void Complete(ulong utcTime)
	{
		completionTime = utcTime;
	}

	public static Quest FromDict(Dictionary<string, object> data)
	{
		uint num = TFUtils.LoadUint(data, "did");
		Dictionary<string, object> dictionary = TFUtils.LoadDict(data, "conditions");
		if (dictionary.Count == 0)
		{
			return null;
		}
		ConditionalProgressSerializer conditionalProgressSerializer = new ConditionalProgressSerializer();
		ConditionalProgress conditionalProgress = conditionalProgressSerializer.DeserializeProgress(TFUtils.LoadList<object>(dictionary, "met_start_condition_ids"));
		ConditionalProgress conditionalProgress2 = conditionalProgressSerializer.DeserializeProgress(TFUtils.LoadList<object>(dictionary, "met_end_condition_ids"));
		ulong? num2 = TFUtils.LoadNullableUlong(data, "start_time");
		ulong? num3 = TFUtils.LoadNullableUlong(data, "completion_time");
		bool triggeredAlready = false;
		bool? flag = TFUtils.TryLoadBool(data, "reminded");
		if (flag.HasValue)
		{
			triggeredAlready = flag.Value;
		}
		if (!num2.HasValue && num3.HasValue)
		{
			num2 = num3;
		}
		return new Quest(num, conditionalProgress, conditionalProgress2, num2, num3, triggeredAlready);
	}

	public Dictionary<string, object> ToDict()
	{
		TFUtils.Assert(startConditions != null && endConditions != null, "Quest object not valid. Cannot hydrate properly.");
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		ConditionalProgressSerializer conditionalProgressSerializer = new ConditionalProgressSerializer();
		ConditionalProgress progress = startConditions.Dehydrate();
		dictionary2["met_start_condition_ids"] = conditionalProgressSerializer.SerializeProgress(progress);
		ConditionalProgress progress2 = ConditionState.DehydrateChunks(endConditions);
		dictionary2["met_end_condition_ids"] = conditionalProgressSerializer.SerializeProgress(progress2);
		dictionary["conditions"] = dictionary2;
		dictionary["start_time"] = TFUtils.NullableToObject(startTime);
		dictionary["completion_time"] = TFUtils.NullableToObject(completionTime);
		dictionary["did"] = did;
		dictionary["reminded"] = triggeredReminder;
		return dictionary;
	}

	public SessionActionTracker InstantiateSessionAction(SessionActionDefinition definition)
	{
		return new SessionActionTracker(definition);
	}

	public override string ToString()
	{
		return "[Quest (did=" + did + ", reminded=" + triggeredReminder + ", startTime=" + startTime.ToString() + ", completeTime=" + completionTime.ToString() + ", startProgress=" + ((startProgress == null) ? "null" : startProgress.ToString()) + ", endProgress=" + ((endProgress == null) ? "null" : endProgress.ToString()) + ", startConditions=" + ((startConditions == null) ? "null" : startConditions.ToString()) + ", endConditions=" + ((endConditions == null) ? "null" : endConditions.ToString()) + ")]";
	}
}
