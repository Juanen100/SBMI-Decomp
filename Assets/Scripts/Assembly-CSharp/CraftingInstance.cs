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
			return readyTimeUtc;
		}
		set
		{
			readyTimeUtc = value;
		}
	}

	public ulong ReadyTimeFromNow
	{
		get
		{
			return readyTimeUtc - TFUtils.EpochTime();
		}
		set
		{
			readyTimeUtc = value + TFUtils.EpochTime();
		}
	}

	public CraftingInstance(Dictionary<string, object> data)
	{
		buildingLabel = new Identity((string)data["building_label"]);
		readyTimeUtc = TFUtils.LoadUlong(data, "ready_time");
		recipeId = TFUtils.LoadInt(data, "recipe_id");
		reward = Reward.FromObject(data["reward"]);
		slotId = TFUtils.LoadInt(data, "slot_id");
	}

	public CraftingInstance(Identity label, int recipeId, ulong readyTimeUtc, Reward reward, int slotId)
	{
		buildingLabel = label;
		this.readyTimeUtc = readyTimeUtc;
		this.recipeId = recipeId;
		this.reward = reward;
		this.slotId = slotId;
	}

	public override string ToString()
	{
		return string.Concat("[CraftingInstance (label=", buildingLabel, ", readyTimeUtc= ", readyTimeUtc, ", recipeId= ", recipeId, ", reward= ", reward, ", slotId= ", slotId, ")]");
	}
}
