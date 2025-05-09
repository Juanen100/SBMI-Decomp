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
			return id;
		}
	}

	public int DefinitionId
	{
		get
		{
			return did;
		}
	}

	public string BlueprintName
	{
		get
		{
			return (string)Invariable["blueprint"];
		}
	}

	public string Name
	{
		get
		{
			return (string)Invariable["name"];
		}
	}

	public ReadOnlyIndexer Invariable
	{
		get
		{
			return iindexer;
		}
	}

	public ReadWriteIndexer Variable
	{
		get
		{
			return vindexer;
		}
	}

	public string SoundOnSelect
	{
		get
		{
			return (string)Invariable["sound_on_select"];
		}
	}

	public string SoundOnTouch
	{
		get
		{
			return (string)Invariable["sound_on_touch"];
		}
	}

	public Entity Core
	{
		get
		{
			return this;
		}
	}

	public EntityType Type
	{
		get
		{
			return EntityType.CORE;
		}
	}

	public EntityType AllTypes
	{
		get
		{
			EntityType entityType = Type;
			foreach (EntityDecorator value in decorators.Values)
			{
				entityType |= value.Type;
			}
			return entityType;
		}
	}

	public CoreEntity(Identity id, Blueprint blueprint)
	{
		this.id = id;
		iproperties = blueprint.InvariableProperties();
		iindexer = new ReadOnlyIndexer(iproperties);
		did = (int)Invariable["did"];
		vproperties = blueprint.VariableProperties();
		vindexer = new ReadWriteIndexer(vproperties);
		decorators = new Dictionary<Type, Entity>();
	}

	public void AddDecorator(Entity decorator)
	{
		if (!decorators.ContainsKey(decorator.GetType()))
		{
			decorators.Add(decorator.GetType(), decorator);
		}
	}

	public T GetDecorator<T>() where T : EntityDecorator
	{
		Type typeFromHandle = typeof(T);
		Entity value = null;
		if (decorators.TryGetValue(typeFromHandle, out value))
		{
			return value as T;
		}
		TFUtils.ErrorLog("Could not find Entity decorator of type " + typeFromHandle.ToString());
		return (T)Activator.CreateInstance(typeFromHandle, this);
	}

	public bool HasDecorator<T>() where T : EntityDecorator
	{
		return decorators.ContainsKey(typeof(T));
	}

	public virtual void PatchReferences(Game game)
	{
	}

	public void Serialize(ref Dictionary<string, object> data)
	{
		foreach (EntityDecorator value in decorators.Values)
		{
			value.SerializeDecorator(ref data);
		}
	}

	public void Deserialize(Dictionary<string, object> data)
	{
		foreach (EntityDecorator value in decorators.Values)
		{
			value.DeserializeDecorator(data);
		}
	}

	public static Type TypeFromString(string typeStr)
	{
		return typeof(CoreEntity);
	}
}
