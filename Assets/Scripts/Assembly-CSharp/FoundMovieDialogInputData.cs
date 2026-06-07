using System.Collections.Generic;

public class FoundMovieDialogInputData : FoundItemDialogInputData
{
	public new const string DIALOG_TYPE = "found_movie";

	protected string movie;

	protected const string MOVIE = "movie";

	public string Movie
	{
		get
		{
			return null;
		}
	}

	public FoundMovieDialogInputData(string title, string message, string icon, string movie, string soundBeat)
		: base(0u, null)
	{
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		return null;
	}

	public new static FoundMovieDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		return null;
	}
}
