using System.Collections.Generic;

public interface Entity
{
	Identity Id { get; }

	int DefinitionId { get; }

	EntityType AllTypes { get; }

	EntityType Type { get; }

	string BlueprintName { get; }

	string Name { get; }

	ReadOnlyIndexer Invariable { get; }

	ReadWriteIndexer Variable { get; }

	string SoundOnTouch { get; }

	string SoundOnSelect { get; }

	Entity Core { get; }

	T GetDecorator<T>() where T : EntityDecorator;

	bool HasDecorator<T>() where T : EntityDecorator;

	void AddDecorator(Entity decorator);

	void PatchReferences(Game game);

	void Serialize(ref Dictionary<string, object> data);

	void Deserialize(Dictionary<string, object> data);
}
