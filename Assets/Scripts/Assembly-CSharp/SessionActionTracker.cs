#define ASSERTS_ON
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
			return definition;
		}
	}

	public ConditionState ActivationProgress
	{
		get
		{
			return activationProgress;
		}
	}

	public ConditionState SuccessProgress
	{
		get
		{
			return successProgress;
		}
	}

	public bool ManualSuccess
	{
		get
		{
			return manualSuccess;
		}
	}

	public string Tag
	{
		get
		{
			return tag;
		}
		set
		{
			tag = value;
		}
	}

	public SessionActionTracker Slave
	{
		get
		{
			return slave;
		}
	}

	public StatusCode Status
	{
		get
		{
			if (status == StatusCode.REQUESTED || status == StatusCode.STARTED)
			{
				switch (successProgress.Examine())
				{
				case ConditionResult.PASS:
					status = StatusCode.FINISHED_SUCCESS;
					break;
				case ConditionResult.FAIL:
					if (definition.IsFailproof)
					{
						status = StatusCode.FINISHED_SUCCESS;
					}
					else
					{
						status = StatusCode.FINISHED_FAILURE;
					}
					break;
				}
			}
			return status;
		}
	}

	public bool RepeatOnFail
	{
		get
		{
			return !enslaved && Definition.RepeatOnFail;
		}
	}

	public SessionActionTracker(SessionActionDefinition definition)
		: this(definition, definition.StartConditions)
	{
	}

	public SessionActionTracker(SessionActionDefinition definition, ICondition startConditionsOverride)
		: this(definition, startConditionsOverride, null)
	{
	}

	public SessionActionTracker(SessionActionDefinition definition, ICondition startConditionsOverride, string tag, bool slave = false)
		: this(definition, startConditionsOverride, false, tag, slave)
	{
	}

	private SessionActionTracker(SessionActionDefinition definition, ICondition startConditionsOverride, bool manualSuccess, string tag, bool slave = false)
	{
		status = StatusCode.INITIAL;
		this.definition = definition;
		activationProgress = new ConditionState(startConditionsOverride);
		successProgress = new ConditionState(definition.SucceedConditions);
		dynamic = new Dictionary<string, object>();
		this.definition.SetDynamicProperties(ref dynamic);
		this.manualSuccess = manualSuccess;
		this.tag = tag;
		enslaved = slave;
		activationProgress.Recalculate(null, Trigger.Null);
		successProgress.Recalculate(null, Trigger.Null);
	}

	public T GetDefinition<T>()
	{
		return (T)Convert.ChangeType(definition, typeof(T));
	}

	public T GetDynamic<T>(string key)
	{
		TFUtils.Assert(dynamic.ContainsKey(key), "Cannot lookup a key that does not exist. Did you forget to call SessionActionDefinition.SetDynamicProperties?");
		return (T)dynamic[key];
	}

	public void SetDynamic(string key, object val)
	{
		TFUtils.Assert(dynamic.ContainsKey(key), "Cannot set a key that does not exist. Did you forget to call SessionActionDefinition.SetDynamicProperties?");
		dynamic[key] = val;
	}

	public void MarkPostponed()
	{
		AssertNotObliterated();
		if (status != StatusCode.INITIAL)
		{
			TFUtils.Assert(status == StatusCode.INITIAL, "Trying to mark a session action tracker to the POSTPONED state, but it is not in the INITIAL state. Did something else already request it?\naction=" + this);
		}
		status = StatusCode.POSTPONED;
	}

	public void MarkRequested()
	{
		AssertNotObliterated();
		if (status != StatusCode.INITIAL && status != StatusCode.POSTPONED)
		{
			TFUtils.Assert(status == StatusCode.INITIAL || status == StatusCode.POSTPONED, "Trying to mark a session action tracker to the REQUESTED state, but it is not in the INITIAL or POSTPONED state. Did something else already request it?\naction=" + this);
		}
		status = StatusCode.REQUESTED;
	}

	public void MarkStarted()
	{
		AssertNotObliterated();
		if (status != StatusCode.REQUESTED)
		{
			TFUtils.Assert(status == StatusCode.REQUESTED, "Trying to advance a session action to the STARTED state, but it is not in the REQUESTED state. Did something else already start it?\naction=" + this);
		}
		status = StatusCode.STARTED;
		if (slave != null)
		{
			slave.activationProgress.Recalculate(null, DumbCondition.PASS_TRIGGER);
		}
	}

	public void MarkObliterated(Game game)
	{
		status = StatusCode.OBLITERATED;
		definition.OnObliterate(game, this);
		if (slave != null)
		{
			slave.MarkObliterated(game);
		}
	}

	public void MarkSucceeded()
	{
		MarkSucceeded(true);
		if (slave != null && slave.status == StatusCode.STARTED)
		{
			slave.MarkSucceeded();
		}
	}

	public void MarkSucceeded(bool failIfObliterated)
	{
		if (failIfObliterated)
		{
			AssertNotObliterated();
		}
		if (status == StatusCode.FINISHED_FAILURE)
		{
			TFUtils.Assert(false, "Shouldn't try to succeed a session action that has already failed.\naction=" + this);
		}
		RecalculateProgress(DumbCondition.PASS_TRIGGER);
	}

	public void MarkFailed()
	{
		AssertNotObliterated();
		TFUtils.Assert(status != StatusCode.FINISHED_SUCCESS, "Shouldn't try to fail a session action that has already succeeded.");
		Trigger trigger = ((!definition.IsFailproof) ? DumbCondition.FAIL_TRIGGER : DumbCondition.PASS_TRIGGER);
		RecalculateProgress(trigger);
	}

	private void RecalculateProgress(Trigger trigger)
	{
		successProgress.Recalculate(null, trigger);
		for (SessionActionTracker sessionActionTracker = slave; sessionActionTracker != null; sessionActionTracker = sessionActionTracker.slave)
		{
			sessionActionTracker.successProgress.Recalculate(null, trigger);
		}
	}

	public void ReActivate(Game game)
	{
		MarkObliterated(game);
		Definition.OnDestroy(game);
		didPreActivate = false;
		status = StatusCode.INITIAL;
		slave = null;
	}

	public void PreActivate(Game game)
	{
		if (!didPreActivate)
		{
			didPreActivate = true;
			definition.PreActivate(game, this);
			if (definition.Slave != null)
			{
				TFUtils.Assert(slave == null, "Each tracker may have only 1 slave. You should not be trying to replace an existing one.");
				slave = new SessionActionTracker(definition.Slave, new DumbCondition(0u), true, tag, true);
				game.sessionActionManager.Request(slave, game);
			}
		}
	}

	public void PostComplete(Game game)
	{
		definition.PostComplete(game, this);
	}

	public bool ActiveProcess(Game game)
	{
		return definition.ActiveProcess(game, this);
	}

	public void StartPostponeTimer()
	{
		postponeComplete = DateTime.Now.AddSeconds(definition.Postpone);
	}

	public bool IsPostponeComplete()
	{
		return postponeComplete.Value.CompareTo(DateTime.Now) <= 0;
	}

	public bool ShouldSetPostponeTimer()
	{
		DateTime? dateTime = postponeComplete;
		return !dateTime.HasValue && definition.Postpone > 0f;
	}

	private void AssertNotObliterated()
	{
		TFUtils.Assert(status != StatusCode.OBLITERATED, "This tracker has been obliterated. Should not be trying to change that.");
	}

	public void Destroy(Game game)
	{
		definition.OnDestroy(game);
	}

	public override string ToString()
	{
		return string.Concat("SessionActionTracker:(status=", status, ", definition=", definition.ToString(), ", tag=", tag, ", activationProgress=", activationProgress.ToString(), ", successProgress=", successProgress.ToString(), ", dynamics=", TFUtils.DebugDictToString(dynamic), ")");
	}
}
