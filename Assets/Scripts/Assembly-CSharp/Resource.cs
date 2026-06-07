using System.Collections.Generic;

public class Resource
{
	public const string RESOURCE_AMOUNTS = "resource_amounts";

	private int did;

	private int amountSpent;

	private int amountEarned;

	private int amountPurchased;

	private int maxAmount;

	private int currencyDisplayQuestTrigger;

	private string name;

	private string name_plural;

	private string tag;

	private string texture;

	private string collectedSound;

	private string tapSound;

	private string eatenSound;

	private float width;

	private float height;

	private float jellyConversion;

	private RewardDefinition reward;

	private int fullnessTime;

	private bool forceTapToCollect;

	private bool forceWishMatch;

	private bool forceNoWishPayout;

	private bool ignoreWishDurationTimer;

	private bool consumable;

	public bool Consumable
	{
		get
		{
			return false;
		}
	}

	public RewardDefinition Reward
	{
		get
		{
			return null;
		}
	}

	public int Amount
	{
		get
		{
			return 0;
		}
	}

	public int AmountPurchased
	{
		get
		{
			return 0;
		}
	}

	public string Name
	{
		get
		{
			return null;
		}
	}

	public string Name_Plural
	{
		get
		{
			return null;
		}
	}

	public string Tag
	{
		get
		{
			return null;
		}
	}

	public int Did
	{
		get
		{
			return 0;
		}
	}

	public int CurrencyDisplayQuestTrigger
	{
		get
		{
			return 0;
		}
	}

	public string CollectedSound
	{
		get
		{
			return null;
		}
	}

	public string TapSound
	{
		get
		{
			return null;
		}
	}

	public string EatenSound
	{
		get
		{
			return null;
		}
	}

	public float Width
	{
		get
		{
			return 0f;
		}
	}

	public float Height
	{
		get
		{
			return 0f;
		}
	}

	public float HardCurrencyConversion
	{
		get
		{
			return 0f;
		}
	}

	public int FullnessTime
	{
		get
		{
			return 0;
		}
	}

	public bool ForceTapToCollect
	{
		get
		{
			return false;
		}
	}

	public bool ForceWishMatch
	{
		get
		{
			return false;
		}
	}

	public bool ForceNoWishPayout
	{
		get
		{
			return false;
		}
	}

	public bool IgnoreWishDurationTimer
	{
		get
		{
			return false;
		}
	}

	public Resource(string name, string name_plural, string tag, float width, float height, int maxAmount, string texture, string collectedSound, string tapSound, string eatenSound, RewardDefinition reward, float jellyConversion, int fullnessTime, bool forceTapToCollect, bool forceWishMatch, bool ignoreWishDurationTimer, bool forceNoWishPayout, int did, int currencyDisplayQuestTrigger, bool consumable)
	{
	}

	public Resource(Resource other)
	{
	}

	public string GetResourceTexture()
	{
		return null;
	}

	public string GetResourceTexture(int amount)
	{
		return null;
	}

	public void AddAmount(int amountToAdd)
	{
	}

	public void SubtractAmount(int amountToSubtract)
	{
	}

	public void SetAmountEarned(int newAmount)
	{
	}

	public void SetAmounts(int amountEarned, int amountSpent)
	{
	}

	public void SetAmountPurchased(int amountPurchased)
	{
	}

	public static int Prorate(int amount, float percentLeft)
	{
		return 0;
	}

	public static void AddToTriggerData(ref Dictionary<string, object> data, int did)
	{
	}

	public static void AddToTriggerData(ref Dictionary<string, object> data, int did, int amount)
	{
	}
}
