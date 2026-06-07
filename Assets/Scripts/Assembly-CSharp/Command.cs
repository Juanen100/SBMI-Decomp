using System;
using System.Collections.Generic;

public class Command
{
	public enum TYPE
	{
		MOVE = 0,
		FLIP = 1,
		RETURN = 2,
		RUSH = 3,
		EMPLOY = 4,
		ERECT = 5,
		RESIDE = 6,
		PRODUCE = 7,
		ADVANCE = 8,
		COMPLETE = 9,
		BONUS = 10,
		EXPIRE = 11,
		CLICKED = 12,
		DELEGATE_CLICK = 13,
		ABORT = 14,
		WANDER = 15,
		HUNGER = 16,
		WISH = 17,
		TEMPT = 18,
		FEED = 19,
		SPAWN = 20,
		PERFORM_TASK = 21,
		COLLECT_TASK_REWARD = 22,
		CRAFT = 23,
		CRAFTED = 24,
		CLEAR = 25,
		WAIT_AS_TASK_TARGET = 26,
		PURCHASE = 27,
		ACTIVATE = 28,
		EXPAND = 29,
		HUBCRAFT = 30,
		IDLE_PAUSE = 31,
		RESUME_FULL = 32,
		RESUME_WISHING = 33,
		GO_HOME = 34,
		STORE_RESIDENT = 35,
		FLEE = 36,
		CHEER = 37,
		TASK = 38,
		ENTER = 39,
		STAND = 40,
		BONUS_REWARD = 41,
		RUSH_TASK = 42,
		UPGRADE = 43,
		UPGRADE_COMPLETE = 44
	}

	public const string PRODUCT_ID = "product_id";

	public const string SLOT_ID = "slot_id";

	private TYPE type;

	private Identity sender;

	private Identity receiver;

	private Action onComplete;

	private ulong timeEpoch;

	private Dictionary<string, object> properties;

	public TYPE Type
	{
		get
		{
			return default(TYPE);
		}
	}

	public Identity Sender
	{
		get
		{
			return null;
		}
	}

	public Identity Receiver
	{
		get
		{
			return null;
		}
	}

	public ulong TimeEpoch
	{
		get
		{
			return 0uL;
		}
		set
		{
		}
	}

	public Action OnComplete
	{
		set
		{
		}
	}

	public object Item
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public Command(TYPE type, Identity sender, Identity receiver)
	{
	}

	public bool HasProperty(string property)
	{
		return false;
	}

	public void TryExecuteOnComplete()
	{
	}

	public string Describe()
	{
		return null;
	}

	public bool Match(Dictionary<string, object> matching)
	{
		return false;
	}
}
