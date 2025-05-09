using System.Collections.Generic;
using UnityEngine;
using Yarg;

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
			return consumable;
		}
	}

	public RewardDefinition Reward
	{
		get
		{
			return reward;
		}
	}

	public int Amount
	{
		get
		{
			return amountEarned + amountPurchased - amountSpent;
		}
	}

	public int AmountPurchased
	{
		get
		{
			return amountPurchased;
		}
	}

	public string Name
	{
		get
		{
			return name;
		}
	}

	public string Name_Plural
	{
		get
		{
			return name_plural;
		}
	}

	public string Tag
	{
		get
		{
			return tag;
		}
	}

	public int Did
	{
		get
		{
			return did;
		}
	}

	public int CurrencyDisplayQuestTrigger
	{
		get
		{
			return currencyDisplayQuestTrigger;
		}
	}

	public string CollectedSound
	{
		get
		{
			return collectedSound;
		}
	}

	public string TapSound
	{
		get
		{
			return tapSound;
		}
	}

	public string EatenSound
	{
		get
		{
			return eatenSound;
		}
	}

	public float Width
	{
		get
		{
			return width;
		}
	}

	public float Height
	{
		get
		{
			return height;
		}
	}

	public float HardCurrencyConversion
	{
		get
		{
			return jellyConversion;
		}
	}

	public int FullnessTime
	{
		get
		{
			return fullnessTime;
		}
	}

	public bool ForceTapToCollect
	{
		get
		{
			return forceTapToCollect;
		}
	}

	public bool ForceWishMatch
	{
		get
		{
			return forceWishMatch;
		}
	}

	public bool ForceNoWishPayout
	{
		get
		{
			return forceNoWishPayout;
		}
	}

	public bool IgnoreWishDurationTimer
	{
		get
		{
			return ignoreWishDurationTimer;
		}
	}

	public Resource(string name, string name_plural, string tag, float width, float height, int maxAmount, string texture, string collectedSound, string tapSound, string eatenSound, RewardDefinition reward, float jellyConversion, int fullnessTime, bool forceTapToCollect, bool forceWishMatch, bool ignoreWishDurationTimer, bool forceNoWishPayout, int did, int currencyDisplayQuestTrigger, bool consumable)
	{
		this.name = name;
		this.name_plural = name_plural;
		this.tag = tag;
		this.width = width;
		this.height = height;
		this.maxAmount = maxAmount;
		this.texture = texture;
		this.collectedSound = collectedSound;
		this.tapSound = tapSound;
		this.eatenSound = eatenSound;
		this.reward = reward;
		this.jellyConversion = jellyConversion;
		this.fullnessTime = fullnessTime;
		this.forceTapToCollect = forceTapToCollect;
		this.forceWishMatch = forceWishMatch;
		this.ignoreWishDurationTimer = ignoreWishDurationTimer;
		this.forceNoWishPayout = forceNoWishPayout;
		this.did = did;
		this.currencyDisplayQuestTrigger = currencyDisplayQuestTrigger;
		this.consumable = consumable;
	}

	public Resource(Resource other)
	{
		name = other.name;
		name_plural = other.name_plural;
		tag = other.tag;
		width = other.width;
		height = other.height;
		maxAmount = other.maxAmount;
		texture = other.texture;
		collectedSound = other.collectedSound;
		tapSound = other.tapSound;
		eatenSound = other.eatenSound;
		reward = other.reward;
		jellyConversion = other.jellyConversion;
		fullnessTime = other.fullnessTime;
		forceTapToCollect = other.forceTapToCollect;
		forceWishMatch = other.forceWishMatch;
		ignoreWishDurationTimer = other.ignoreWishDurationTimer;
		forceNoWishPayout = other.forceNoWishPayout;
		did = other.did;
		amountEarned = other.amountEarned;
		amountPurchased = other.amountPurchased;
		amountSpent = other.amountSpent;
		currencyDisplayQuestTrigger = other.currencyDisplayQuestTrigger;
		consumable = other.consumable;
	}

	public string GetResourceTexture()
	{
		return texture;
	}

	public string GetResourceTexture(int amount)
	{
		string text = null;
		if (did == ResourceManager.SOFT_CURRENCY)
		{
			if (amount <= 7)
			{
				text = "IconMoney_1.png";
			}
			else if (amount > 7 && amount <= 20)
			{
				text = "IconMoney_2.png";
			}
			else if (amount > 20 && amount <= 54)
			{
				text = "IconMoney_3.png";
			}
			else if (amount > 54 && amount <= 143)
			{
				text = "IconMoney_4.png";
			}
			else if (amount > 143 && amount <= 376)
			{
				text = "IconMoney_5.png";
			}
			else if (amount > 376 && amount <= 986)
			{
				text = "IconMoney_6.png";
			}
			else if (amount > 986)
			{
				text = "IconMoney_6.png";
			}
		}
		else if (did == ResourceManager.XP)
		{
			if (amount <= 2)
			{
				text = "IconXP_1.png";
			}
			else if (amount > 2 && amount <= 7)
			{
				text = "IconXP_2.png";
			}
			else if (amount > 7 && amount <= 20)
			{
				text = "IconXP_3.png";
			}
			else if (amount > 20 && amount <= 54)
			{
				text = "IconXP_4.png";
			}
			else if (amount > 54 && amount <= 143)
			{
				text = "IconXP_5.png";
			}
			else if (amount > 143 && amount <= 376)
			{
				text = "IconXP_6.png";
			}
			else if (amount > 376 && amount <= 986)
			{
				text = "IconXP_6.png";
			}
			else if (amount > 986)
			{
				text = "IconXP_6.png";
			}
		}
		else if (did == ResourceManager.DEFAULT_JJ)
		{
			switch (amount)
			{
			case 1:
				text = "IconJellyfishJelly_1.png";
				break;
			case 25:
				text = "IconJellyfishJelly_2.png";
				break;
			case 50:
				text = "IconJellyfishJelly_2.png";
				break;
			}
		}
		if (text != null)
		{
			AtlasCoords atlasCoords = YGTextureLibrary.GetAtlasCoords(text).atlasCoords;
			width = (float)TFAnimatedSprite.CalcWorldSize(atlasCoords.frame.width, 0.8);
			height = (float)TFAnimatedSprite.CalcWorldSize(atlasCoords.frame.height, 0.8);
		}
		return text;
	}

	public void AddAmount(int amountToAdd)
	{
		SetAmountEarned(amountEarned + amountToAdd);
	}

	public void SubtractAmount(int amountToSubtract)
	{
		amountSpent += amountToSubtract;
	}

	public void SetAmountEarned(int newAmount)
	{
		amountEarned = amountSpent + Mathf.Min(newAmount - amountSpent, maxAmount);
	}

	public void SetAmounts(int amountEarned, int amountSpent)
	{
		this.amountSpent = amountSpent;
		SetAmountEarned(amountEarned);
	}

	public void SetAmountPurchased(int amountPurchased)
	{
		this.amountPurchased = amountPurchased;
	}

	public static int Prorate(int amount, float percentLeft)
	{
		if (percentLeft < 0f)
		{
			percentLeft = 0f;
		}
		else if (percentLeft > 1f)
		{
			percentLeft = 1f;
		}
		return Mathf.CeilToInt(percentLeft * (float)amount);
	}

	public static void AddToTriggerData(ref Dictionary<string, object> data, int did)
	{
		AddToTriggerData(ref data, did, 1);
	}

	public static void AddToTriggerData(ref Dictionary<string, object> data, int did, int amount)
	{
		if (!data.ContainsKey("resource_amounts"))
		{
			data["resource_amounts"] = new Dictionary<string, object>();
		}
		((Dictionary<string, object>)data["resource_amounts"])[did.ToString()] = amount;
	}
}
