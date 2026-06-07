using System.Collections.Generic;

public abstract class EntityDecorator : Entity
{
	protected Entity core;

	public Identity Id
	{
		get
		{
			return null;
		}
	}

	public int DefinitionId
	{
		get
		{
			return 0;
		}
	}

	public EntityType AllTypes
	{
		get
		{
			return default(EntityType);
		}
	}

	public virtual EntityType Type
	{
		get
		{
			return default(EntityType);
		}
	}

	public string BlueprintName
	{
		get
		{
			return null;
		}
	}

	public string Name
	{
		get
		{
			return null;
		}
	}

	public ReadOnlyIndexer Invariable
	{
		get
		{
			return null;
		}
	}

	public ReadWriteIndexer Variable
	{
		get
		{
			return null;
		}
	}

	public virtual string SoundOnTouch
	{
		get
		{
			return null;
		}
	}

	public virtual string SoundOnSelect
	{
		get
		{
			return null;
		}
	}

	public Entity Core
	{
		get
		{
			return null;
		}
	}

	public EntityDecorator(Entity toDecorate)
	{
	}

	public void AddDecorator(Entity entity)
	{
	}

	public T GetDecorator<T>() where T : EntityDecorator
	{
		return null;
	}

	public bool HasDecorator<T>() where T : EntityDecorator
	{
		return false;
	}

	public virtual void SerializeDecorator(ref Dictionary<string, object> data)
	{
	}

	public virtual void DeserializeDecorator(Dictionary<string, object> data)
	{
	}

	public void Serialize(ref Dictionary<string, object> data)
	{
	}

	public void Deserialize(Dictionary<string, object> data)
	{
	}

	public virtual void PatchReferences(Game game)
	{
	}
}
