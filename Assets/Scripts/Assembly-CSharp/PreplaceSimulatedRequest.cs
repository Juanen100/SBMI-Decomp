using System.Collections.Generic;
using UnityEngine;

public class PreplaceSimulatedRequest : SimulationSessionActionDefinition
{
	public const string TYPE = "preplace_simulated_request";

	public const string PREPLACE_REQUEST_DICT = "preplace_request_dict";

	private const string DEFINITION_ID = "definition_id";

	private const string POSITION = "position";

	private int targetDid;

	private Vector2 position;

	public int? TargetDid
	{
		get
		{
			return null;
		}
	}

	public Vector2 Position
	{
		get
		{
			return default(Vector2);
		}
	}

	public static PreplaceSimulatedRequest Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		return null;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public void Preplace(Session session, SessionActionTracker action)
	{
	}
}
