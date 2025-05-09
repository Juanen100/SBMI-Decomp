#define ASSERTS_ON
using System.Collections.Generic;

public abstract class EntityDecorator : Entity
{
	protected Entity core;

	public Identity Id
	{
		get
		{
			return core.Id;
		}
	}

	public int DefinitionId
	{
		get
		{
			return core.DefinitionId;
		}
	}

	public EntityType AllTypes
	{
		get
		{
			return core.AllTypes;
		}
	}

	public virtual EntityType Type
	{
		get
		{
			return core.Type;
		}
	}

	public string BlueprintName
	{
		get
		{
			return core.BlueprintName;
		}
	}

	public string Name
	{
		get
		{
			return core.Name;
		}
	}

	public ReadOnlyIndexer Invariable
	{
		get
		{
			return core.Invariable;
		}
	}

	public ReadWriteIndexer Variable
	{
		get
		{
			return core.Variable;
		}
	}

	public virtual string SoundOnTouch
	{
		get
		{
			return core.SoundOnTouch;
		}
	}

	public virtual string SoundOnSelect
	{
		get
		{
			return core.SoundOnSelect;
		}
	}

	public Entity Core
	{
		get
		{
			return core;
		}
	}

	public EntityDecorator(Entity toDecorate)
	{
		core = toDecorate.Core;
		toDecorate.AddDecorator(this);
		TFUtils.Assert(core != null, "checking that we are not null");
	}

	public void AddDecorator(Entity entity)
	{
		if (entity is CoreEntity)
		{
			core = entity;
			return;
		}
		TFUtils.Assert(core != null, "CoreEntity is not set yet");
		core.AddDecorator(entity);
	}

	public T GetDecorator<T>() where T : EntityDecorator
	{
		return core.GetDecorator<T>();
	}

	public bool HasDecorator<T>() where T : EntityDecorator
	{
		return core.HasDecorator<T>();
	}

	public virtual void SerializeDecorator(ref Dictionary<string, object> data)
	{
	}

	public virtual void DeserializeDecorator(Dictionary<string, object> data)
	{
	}

	public void Serialize(ref Dictionary<string, object> data)
	{
		core.Serialize(ref data);
	}

	public void Deserialize(Dictionary<string, object> data)
	{
		core.Deserialize(data);
	}

	public virtual void PatchReferences(Game game)
	{
		core.PatchReferences(game);
	}
}
