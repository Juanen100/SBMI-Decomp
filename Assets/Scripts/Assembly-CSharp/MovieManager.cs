#define ASSERTS_ON
using System.Collections.Generic;
using MiniJSON;

public class MovieManager
{
	private static readonly string MOVIE_PATH = "Video";

	private HashSet<int> unlocked;

	private Dictionary<int, MovieInfo> movies;

	public HashSet<int> UnlockedMovies
	{
		get
		{
			return new HashSet<int>(unlocked);
		}
	}

	public MovieManager()
	{
		unlocked = new HashSet<int>();
		movies = new Dictionary<int, MovieInfo>();
		LoadMoviesFromSpread();
	}

	public MovieInfo GetMovieInfoById(int id)
	{
		return movies[id];
	}

	public void UnlockMovie(int id)
	{
		TFUtils.Assert(movies.ContainsKey(id), "Unlocking an unknown movie!");
		unlocked.Add(id);
	}

	public void UnlockAllMovies()
	{
		foreach (KeyValuePair<int, MovieInfo> movie in movies)
		{
			int key = movie.Key;
			if (!unlocked.Contains(key))
			{
				UnlockMovie(key);
			}
		}
	}

	public void UnlockAllMoviesToGamestate(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (!dictionary.ContainsKey("movies"))
		{
			dictionary["movies"] = new List<object>();
		}
		List<object> list = (List<object>)dictionary["movies"];
		foreach (KeyValuePair<int, MovieInfo> movie in movies)
		{
			int key = movie.Key;
			if (!list.Contains(key))
			{
				list.Add(key);
			}
		}
	}

	private string[] GetFilesToLoad()
	{
		return Config.MOVIE_PATH;
	}

	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	private void LoadMovies()
	{
		string[] filesToLoad = GetFilesToLoad();
		string[] array = filesToLoad;
		foreach (string filePath in array)
		{
			string filePathFromString = GetFilePathFromString(filePath);
			TFUtils.DebugLog("Loading movie info: " + filePathFromString);
			string json = TFUtils.ReadAllText(filePathFromString);
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(json);
			switch ((string)dictionary["type"])
			{
			case "movie":
			{
				MovieInfo movieInfo = new MovieInfo(dictionary);
				movies.Add(movieInfo.Did, movieInfo);
				break;
			}
			}
		}
	}

	private void LoadMoviesFromSpread()
	{
		string text = "Movies";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null)
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			dictionary.Clear();
			dictionary.Add("did", instance.GetIntCell(sheetIndex, rowIndex, "did"));
			dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "name"));
			dictionary.Add("description", instance.GetStringCell(sheetIndex, rowIndex, "description"));
			dictionary.Add("collect_name", instance.GetStringCell(sheetIndex, rowIndex, "collect name"));
			dictionary.Add("movie", instance.GetStringCell(sheetIndex, rowIndex, "movie"));
			dictionary.Add("texture", instance.GetStringCell(sheetIndex, rowIndex, "texture"));
			MovieInfo movieInfo = new MovieInfo(dictionary);
			movies.Add(movieInfo.Did, movieInfo);
		}
	}
}
