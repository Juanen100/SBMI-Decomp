using System.Collections.Generic;

public class MovieManager
{
	private static readonly string MOVIE_PATH;

	private HashSet<int> unlocked;

	private Dictionary<int, MovieInfo> movies;

	public HashSet<int> UnlockedMovies
	{
		get
		{
			return null;
		}
	}

	public MovieInfo GetMovieInfoById(int id)
	{
		return null;
	}

	public void UnlockMovie(int id)
	{
	}

	public void UnlockAllMovies()
	{
	}

	public void UnlockAllMoviesToGamestate(Dictionary<string, object> gameState)
	{
	}

	private string[] GetFilesToLoad()
	{
		return null;
	}

	private string GetFilePathFromString(string filePath)
	{
		return null;
	}

	private void LoadMovies()
	{
	}

	private void LoadMoviesFromSpread()
	{
	}
}
