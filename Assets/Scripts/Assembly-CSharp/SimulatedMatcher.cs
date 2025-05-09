#define ASSERTS_ON
using System.Collections.Generic;
using UnityEngine;

public class SimulatedMatcher : Matcher
{
	public const string INSTANCE_ID = "simulated_guid";

	public const string DEFINITION_ID = "simulated_id";

	public const string TYPE = "simulated_type";

	public static SimulatedMatcher FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher simulatedMatcher = new SimulatedMatcher();
		simulatedMatcher.RegisterProperty("simulated_guid", dict);
		simulatedMatcher.RegisterProperty("simulated_id", dict);
		simulatedMatcher.RegisterProperty("simulated_type", dict);
		if (simulatedMatcher.IsRequired("simulated_id"))
		{
			TFUtils.Assert(simulatedMatcher.IsRequired("simulated_type"), "You need to include a simulated_type in the condition of the json to go with the specified ID");
		}
		return simulatedMatcher;
	}

	public override string DescribeSubject(Game game)
	{
		if (IsRequired("simulated_id"))
		{
			Blueprint blueprint = EntityManager.GetBlueprint(GetTarget("simulated_type"), int.Parse(GetTarget("simulated_id")));
			if (blueprint == null)
			{
				TFUtils.Assert(GetTarget("simulated_type") != null && GetTarget("simulated_type") != string.Empty, "You need to include a \"simulated_type\" for simulated matching Quest: " + ToString());
				blueprint = EntityManager.GetBlueprint(GetTarget("simulated_type"), int.Parse(GetTarget("simulated_id")));
				return "simulated with did " + GetTarget("simulated_id");
			}
			return (string)blueprint.Invariable["name"];
		}
		if (IsRequired("simulated_guid"))
		{
			if (game == null)
			{
				return "simulated with id " + GetTarget("simulated_guid");
			}
			Identity id = new Identity(GetTarget("simulated_guid"));
			Debug.LogError((string)game.simulation.FindSimulated(id).entity.Invariable["name"]);
			return (string)game.simulation.FindSimulated(id).entity.Invariable["name"];
		}
		return string.Empty;
	}
}
