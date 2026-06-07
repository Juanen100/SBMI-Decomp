using System.Collections.Generic;

public abstract class PersistedDialogInputData : DialogInputData
{
	public PersistedDialogInputData(uint sequenceId, string type, string soundImmediate, string soundBeat)
		: base(0u, null, null, null)
	{
	}

	public static PersistedDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public abstract Dictionary<string, object> ToPersistenceDict();

	protected virtual void BuildPersistenceDict(ref Dictionary<string, object> dict, string dialogType)
	{
	}
}
