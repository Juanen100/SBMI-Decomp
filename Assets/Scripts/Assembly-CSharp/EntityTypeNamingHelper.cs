public class EntityTypeNamingHelper
{
	public static string GetBlueprintName(EntityType primaryType, int did)
	{
		return GetBlueprintName(primaryType, did, false);
	}

	public static string GetBlueprintName(EntityType primaryType, int did, bool ignoreNotFoundError)
	{
		return GetBlueprintName(TypeToString(primaryType, ignoreNotFoundError), did);
	}

	public static string GetBlueprintName(string primaryType, int did)
	{
		return string.Format("{0}_{1}", primaryType, did);
	}

	public static EntityType StringToType(string type)
	{
		return StringToType(type, false);
	}

	public static EntityType StringToType(string type, bool ignoreNotFoundError)
	{
		switch (type)
		{
		case "unit":
			return EntityType.RESIDENT;
		case "worker":
			return EntityType.WORKER;
		case "wanderer":
			return EntityType.WANDERER;
		case "debris":
			return EntityType.DEBRIS;
		case "landmark":
			return EntityType.LANDMARK;
		case "building":
			return EntityType.BUILDING;
		case "annex":
			return EntityType.ANNEX;
		case "treasure":
			return EntityType.TREASURE;
		case "costume":
			return EntityType.COSTUME;
		default:
			if (!ignoreNotFoundError)
			{
				TFUtils.ErrorLog("Encountered unknown type (" + type + ")");
			}
			return EntityType.INVALID;
		}
	}

	public static string TypeToString(EntityType type)
	{
		return TypeToString(type, false);
	}

	public static string TypeToString(EntityType type, bool ignoreNotFoundError)
	{
		if ((type & EntityType.RESIDENT) != EntityType.INVALID)
		{
			return "unit";
		}
		if ((type & EntityType.WORKER) != EntityType.INVALID)
		{
			return "worker";
		}
		if ((type & EntityType.WANDERER) != EntityType.INVALID)
		{
			return "wanderer";
		}
		if ((type & EntityType.DEBRIS) != EntityType.INVALID)
		{
			return "debris";
		}
		if ((type & EntityType.LANDMARK) != EntityType.INVALID)
		{
			return "landmark";
		}
		if ((type & EntityType.ANNEX) != EntityType.INVALID)
		{
			return "annex";
		}
		if ((type & EntityType.BUILDING) != EntityType.INVALID)
		{
			return "building";
		}
		if ((type & EntityType.TREASURE) != EntityType.INVALID)
		{
			return "treasure";
		}
		if ((type & EntityType.COSTUME) != EntityType.INVALID)
		{
			return "costume";
		}
		if (!ignoreNotFoundError)
		{
			TFUtils.ErrorLog("Encountered unknown type (" + type.ToString() + ")");
		}
		return null;
	}
}
