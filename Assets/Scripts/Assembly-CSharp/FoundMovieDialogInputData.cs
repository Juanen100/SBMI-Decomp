using System.Collections.Generic;

public class FoundMovieDialogInputData : FoundItemDialogInputData
{
	public new const string DIALOG_TYPE = "found_movie";

	protected const string MOVIE = "movie";

	protected string movie;

	public string Movie
	{
		get
		{
			return movie;
		}
	}

	public FoundMovieDialogInputData(string title, string message, string icon, string movie, string soundBeat)
		: base(title, message, icon, soundBeat)
	{
		this.movie = movie;
	}

	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dict = new Dictionary<string, object>();
		base.BuildPersistenceDict(ref dict, "found_movie");
		dict["title"] = title;
		dict["message"] = message;
		dict["icon"] = icon;
		dict["movie"] = movie;
		if (base.SoundBeat != null)
		{
			dict["sound_beat"] = base.SoundBeat;
		}
		return dict;
	}

	public new static FoundMovieDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		string text = TFUtils.LoadString(dict, "title");
		string text2 = TFUtils.LoadString(dict, "message");
		string text3 = TFUtils.LoadString(dict, "icon");
		string text4 = TFUtils.LoadString(dict, "movie");
		string text5 = TFUtils.TryLoadString(dict, "sound_beat");
		return new FoundMovieDialogInputData(text, text2, text3, text4, text5);
	}
}
