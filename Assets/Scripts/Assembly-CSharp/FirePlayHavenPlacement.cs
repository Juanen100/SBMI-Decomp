using System.Collections.Generic;

public class FirePlayHavenPlacement : SessionActionDefinition
{
	public const string TYPE = "call_playhaven";

	private const string PLACEMENT_FIELD = "placement";

	private string placement;

	public static FirePlayHavenPlacement Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		FirePlayHavenPlacement firePlayHavenPlacement = new FirePlayHavenPlacement();
		firePlayHavenPlacement.Parse(data, id, startConditions, originatedFromQuest);
		return firePlayHavenPlacement;
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new ConstantCondition(0u, true), originatedFromQuest);
		if (data.ContainsKey("placement"))
		{
			placement = (string)data["placement"];
		}
		else
		{
			TFUtils.ErrorLog("Error defining playhaven placement. No placement defined");
		}
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["placement"] = placement;
		return dictionary;
	}

	public override void PreActivate(Game game, SessionActionTracker tracker)
	{
		if (placement != null)
		{
			game.playHavenController.RequestContent(placement);
		}
	}

	public override string ToString()
	{
		return base.ToString() + "FirePlayHavenPlacement:(placement=" + placement + ")";
	}
}
