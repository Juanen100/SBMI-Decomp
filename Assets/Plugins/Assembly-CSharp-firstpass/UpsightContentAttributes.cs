public class UpsightContentAttributes
{
	public enum Type
	{
		UNKNOWN = 0,
		ANNOUNCEMENT = 1,
		INTERNAL_CROSS_PROMOTION = 2,
		REWARDS = 3,
		VIRTUAL_GOODS_PROMOTION = 4,
		OPT_IN = 5,
		ADS = 6,
		MORE_GAMES = 7,
		VIDEO_CAMPAIGN = 8,
		CUSTOM_VIEW = 9,
		MEDIATION = 10
	}

	public Type ContentType { get; private set; }

	public string ContentProperties { get; private set; }

	public static UpsightContentAttributes FromJson(string json, out string scope)
	{
		scope = null;
		return null;
	}

	public override string ToString()
	{
		return null;
	}
}
