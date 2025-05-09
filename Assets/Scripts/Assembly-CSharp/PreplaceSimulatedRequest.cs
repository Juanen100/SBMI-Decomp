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
			return targetDid;
		}
	}

	public Vector2 Position
	{
		get
		{
			return position;
		}
	}

	public static PreplaceSimulatedRequest Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		PreplaceSimulatedRequest preplaceSimulatedRequest = new PreplaceSimulatedRequest();
		preplaceSimulatedRequest.Parse(data, id, startConditions, originatedFromQuest);
		return preplaceSimulatedRequest;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		targetDid = TFUtils.LoadInt(data, "definition_id");
		TFUtils.LoadVector2(out position, (Dictionary<string, object>)data["position"]);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["definition_id"] = targetDid;
		dictionary["position"] = position;
		return dictionary;
	}

	public void Preplace(Session session, SessionActionTracker action)
	{
		object obj = session.CheckAsyncRequest("preplace_request_dict");
		if (obj != null)
		{
			TFUtils.ErrorLog("We're clobbering another preplacement request on definition: " + targetDid);
			((Dictionary<int, Vector2>)obj)[targetDid] = position;
		}
		else
		{
			Dictionary<int, Vector2> dictionary = new Dictionary<int, Vector2>();
			dictionary.Add(targetDid, position);
			obj = dictionary;
		}
		session.AddAsyncResponse("preplace_request_dict", obj);
		action.MarkStarted();
		action.MarkSucceeded();
	}
}
