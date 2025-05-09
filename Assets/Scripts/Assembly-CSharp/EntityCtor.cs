public class EntityCtor : Ctor<Entity>
{
	private Blueprint blueprint;

	public EntityCtor(Blueprint blueprint)
	{
		this.blueprint = blueprint;
	}

	public Entity Create()
	{
		return Create(new Identity());
	}

	public Entity Create(Identity id)
	{
		EntityType primaryType = blueprint.PrimaryType;
		Entity entity = new CoreEntity(id, blueprint);
		switch (primaryType)
		{
		case EntityType.BUILDING:
			entity = new BuildingEntity(entity);
			break;
		case EntityType.ANNEX:
			entity = new BuildingEntity(entity);
			entity = new AnnexEntity(entity);
			break;
		case EntityType.DEBRIS:
			entity = new DebrisEntity(entity);
			break;
		case EntityType.RESIDENT:
		case EntityType.WORKER:
		case EntityType.WANDERER:
			entity = new ResidentEntity(entity);
			break;
		case EntityType.LANDMARK:
			entity = new LandmarkEntity(entity);
			break;
		case EntityType.TREASURE:
			entity = new TreasureEntity(entity);
			break;
		default:
			TFUtils.ErrorLog(string.Concat("Unexpected entity type (", primaryType, ")"));
			break;
		}
		return entity;
	}
}
