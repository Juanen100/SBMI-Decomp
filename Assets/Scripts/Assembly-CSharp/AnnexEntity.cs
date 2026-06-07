public class AnnexEntity : EntityDecorator
{
	public const string TYPE = "annex";

	public const string HUB_ID = "hub_id";

	public const string HUB_DID = "hub_did";

	public override EntityType Type
	{
		get
		{
			return default(EntityType);
		}
	}

	public Identity HubId
	{
		get
		{
			return null;
		}
	}

	public uint? HubDid
	{
		get
		{
			return null;
		}
	}

	public AnnexEntity(Entity toDecorate)
		: base(null)
	{
	}

	public override void PatchReferences(Game game)
	{
	}
}
