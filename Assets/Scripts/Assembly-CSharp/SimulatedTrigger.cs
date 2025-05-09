using System.Collections.Generic;

public static class SimulatedTrigger
{
	public const string SIMULATED = "simulated";

	public const string CONSTRUCTION_COMPLETE = "contruction_complete";

	public static ITrigger CreateTrigger(Simulated simulated, string type)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["simulated_guid"] = simulated.entity.Id;
		dictionary["simulated_id"] = simulated.entity.DefinitionId;
		dictionary["simulated_type"] = EntityTypeNamingHelper.TypeToString(simulated.entity.AllTypes);
		dictionary["simulated"] = simulated;
		return new Trigger(type, dictionary);
	}
}
