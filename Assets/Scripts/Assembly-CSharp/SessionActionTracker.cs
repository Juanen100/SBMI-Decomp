using System;
using System.Collections.Generic;

public class SessionActionTracker
{
	public enum StatusCode
	{
		INITIAL = 0,
		POSTPONED = 1,
		REQUESTED = 2,
		STARTED = 3,
		FINISHED_SUCCESS = 4,
		FINISHED_FAILURE = 5,
		OBLITERATED = 6
	}

	private StatusCode status;

	private SessionActionDefinition definition;

	private ConditionState activationProgress;

	private ConditionState successProgress;

	private bool manualSuccess;

	private Dictionary<string, object> dynamic;

	private string tag;

	private bool didPreActivate;

	private SessionActionTracker slave;

	private bool enslaved;

	private DateTime? postponeComplete;

	public SessionActionDefinition Definition
	{
		get
		{
			return null;
		}
	}

	public ConditionState ActivationProgress
	{
		get
		{
			return null;
		}
	}

	public ConditionState SuccessProgress
	{
		get
		{
			return null;
		}
	}

	public bool ManualSuccess
	{
		get
		{
			return false;
		}
	}

	public string Tag
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public SessionActionTracker Slave
	{
		get
		{
			return null;
		}
	}

	public StatusCode Status
	{
		get
		{
			return default(StatusCode);
		}
	}

	public bool RepeatOnFail
	{
		get
		{
			return false;
		}
	}

	public SessionActionTracker(SessionActionDefinition definition)
	{
	}

	public SessionActionTracker(SessionActionDefinition definition, ICondition startConditionsOverride)
	{
	}

	public SessionActionTracker(SessionActionDefinition definition, ICondition startConditionsOverride, string tag, bool slave = false)
	{
	}

	private SessionActionTracker(SessionActionDefinition definition, ICondition startConditionsOverride, bool manualSuccess, string tag, bool slave = false)
	{
	}

	public T GetDefinition<T>()
	{
		return default(T);
	}

	public T GetDynamic<T>(string key)
	{
		return default(T);
	}

	public void SetDynamic(string key, object val)
	{
	}

	public void MarkPostponed()
	{
	}

	public void MarkRequested()
	{
	}

	public void MarkStarted()
	{
	}

	public void MarkObliterated(Game game)
	{
	}

	public void MarkSucceeded()
	{
	}

	public void MarkSucceeded(bool failIfObliterated)
	{
	}

	public void MarkFailed()
	{
	}

	private void RecalculateProgress(Trigger trigger)
	{
	}

	public void ReActivate(Game game)
	{
	}

	public void PreActivate(Game game)
	{
	}

	public void PostComplete(Game game)
	{
	}

	public bool ActiveProcess(Game game)
	{
		return false;
	}

	public void StartPostponeTimer()
	{
	}

	public bool IsPostponeComplete()
	{
		return false;
	}

	public bool ShouldSetPostponeTimer()
	{
		return false;
	}

	private void AssertNotObliterated()
	{
	}

	public void Destroy(Game game)
	{
	}

	public override string ToString()
	{
		return null;
	}
}
