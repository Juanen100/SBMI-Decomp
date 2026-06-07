using System.Collections.Generic;

public class RateMeDialogInputData : PersistedDialogInputData
{
	public const string DIALOG_TYPE = "rate_me";

	public RateMeDialogInputData()
		: base(0u, null, null, null)
	{
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		return null;
	}

	public new static RateMeDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		return null;
	}
}
