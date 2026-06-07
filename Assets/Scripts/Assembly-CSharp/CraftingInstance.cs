using System.Collections.Generic;

public class CraftingInstance
{
	public Identity buildingLabel;

	public int slotId;

	private ulong readyTimeUtc;

	public int recipeId;

	public Reward reward;

	public bool rushed;

	public ulong ReadyTimeUtc
	{
		get
		{
			return 0uL;
		}
		set
		{
		}
	}

	public ulong ReadyTimeFromNow
	{
		get
		{
			return 0uL;
		}
		set
		{
		}
	}

	public CraftingInstance(Dictionary<string, object> data)
	{
	}

	public CraftingInstance(Identity label, int recipeId, ulong readyTimeUtc, Reward reward, int slotId)
	{
	}

	public override string ToString()
	{
		return null;
	}
}
