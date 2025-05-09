using System.Collections.Generic;

public class NewWishAction : PersistedSimulatedAction
{
	public const string NEW_WISH = "nw";

	public const ulong INVALID_ULONG = ulong.MaxValue;

	public int wishProductId;

	public int? prevWishProductId;

	public ulong expiresAt;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public NewWishAction(Identity id, int wishProductId, int? prevWishProductId, ulong expiresAt)
		: base("nw", id, typeof(NewWishAction).ToString())
	{
		this.wishProductId = wishProductId;
		this.expiresAt = expiresAt;
		this.prevWishProductId = prevWishProductId;
	}

	public new static NewWishAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		int num = TFUtils.LoadInt(data, "wish_product_id");
		ulong num2 = TFUtils.LoadUlong(data, "wish_expires_at");
		int? num3 = TFUtils.TryLoadInt(data, "prev_wish_product_id");
		return new NewWishAction(id, num, num3, num2);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["wish_product_id"] = wishProductId;
		dictionary["wish_expires_at"] = expiresAt;
		if (prevWishProductId.HasValue)
		{
			dictionary["prev_wish_product_id"] = prevWishProductId;
		}
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		entity.PreviousResourceId = prevWishProductId;
		entity.HungerResourceId = wishProductId;
		entity.WishExpiresAt = expiresAt;
		simulated.ClearPendingCommands();
		simulated.EnterInitialState(EntityManager.ResidentActions["wishing"], game.simulation);
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> unitGameState = ResidentEntity.GetUnitGameState(gameState, target);
		if (prevWishProductId.HasValue)
		{
			unitGameState["prev_wish_product_id"] = prevWishProductId;
		}
		if (unitGameState.ContainsKey("wish_product_id"))
		{
			unitGameState["wish_product_id"] = wishProductId;
		}
		else
		{
			unitGameState.Add("wish_product_id", wishProductId);
		}
		unitGameState["wish_expires_at"] = expiresAt;
		base.Confirm(gameState);
	}
}
