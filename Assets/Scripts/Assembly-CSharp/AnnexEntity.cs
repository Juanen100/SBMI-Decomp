public class AnnexEntity : EntityDecorator
{
	public const string TYPE = "annex";

	public const string HUB_ID = "hub_id";

	public const string HUB_DID = "hub_did";

	public override EntityType Type
	{
		get
		{
			return EntityType.ANNEX;
		}
	}

	public Identity HubId
	{
		get
		{
			if (Invariable.ContainsKey("hub_id"))
			{
				return (Identity)Invariable["hub_id"];
			}
			return null;
		}
	}

	public uint? HubDid
	{
		get
		{
			if (Invariable.ContainsKey("hub_did"))
			{
				return (uint?)Invariable["hub_did"];
			}
			return null;
		}
	}

	public AnnexEntity(Entity toDecorate)
		: base(toDecorate)
	{
		new StructureDecorator(this);
	}

	public override void PatchReferences(Game game)
	{
		if (Invariable.ContainsKey("hub_id"))
		{
			Identity hubId = HubId;
			BuildingEntity decorator = game.entities.GetEntity(hubId).GetDecorator<BuildingEntity>();
			decorator.RegisterAnnex(this);
		}
		else if (Invariable.ContainsKey("hub_did"))
		{
			Simulated simulated = game.simulation.FindSimulated((int)HubDid.Value);
			if (simulated != null)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				entity.RegisterAnnex(this);
			}
		}
		base.PatchReferences(game);
	}
}
