using System.Collections.Generic;

public class FailWishAction : PersistedSimulatedAction
{
	public const string FAIL_WISH = "fw";

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public FailWishAction(Simulated unit)
		: this(unit.Id)
	{
	}

	private FailWishAction(Identity id)
		: base("fw", id, typeof(FailWishAction).ToString())
	{
	}

	public new static FailWishAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		return new FailWishAction(id);
	}

	public override Dictionary<string, object> ToDict()
	{
		return base.ToDict();
	}

	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		simulated.ClearPendingCommands();
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		entity.PreviousResourceId = entity.HungerResourceId;
		entity.HungerResourceId = null;
		entity.WishExpiresAt = utcNow;
		simulated.EnterInitialState(EntityManager.ResidentActions["wander_hungry"], game.simulation);
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> unitGameState = ResidentEntity.GetUnitGameState(gameState, target);
		if (unitGameState.ContainsKey("wish_product_id"))
		{
			unitGameState["prev_wish_product_id"] = unitGameState["wish_product_id"];
		}
		unitGameState.Remove("wish_product_id");
		unitGameState.Remove("wish_expires_at");
		base.Confirm(gameState);
	}
}
