using System;
using System.Collections.Generic;

public class CoreEntity : Entity
{
	public const string REQUEST_INTERFACE = "RequestEntityInterface";

	private Identity id;

	protected Dictionary<string, object> iproperties;

	protected ReadOnlyIndexer iindexer;

	protected Dictionary<string, object> vproperties;

	protected ReadWriteIndexer vindexer;

	private Dictionary<Type, Entity> decorators;

	private int did;

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

	public string SoundOnSelect
	{
		get
		{
			return null;
		}
	}

	public string SoundOnTouch
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

	public EntityType Type
	{
		get
		{
			return default(EntityType);
		}
	}

	public EntityType AllTypes
	{
		get
		{
			return default(EntityType);
		}
	}

	public CoreEntity(Identity id, Blueprint blueprint)
	{
	}

	public void AddDecorator(Entity decorator)
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

	public virtual void PatchReferences(Game game)
	{
	}

	public void Serialize(ref Dictionary<string, object> data)
	{
	}

	public void Deserialize(Dictionary<string, object> data)
	{
	}

	public static Type TypeFromString(string typeStr)
	{
		return null;
	}
}
