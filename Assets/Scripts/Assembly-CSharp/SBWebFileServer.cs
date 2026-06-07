using System;

public class SBWebFileServer
{
	private string eTagFile;

	public static DateTime LastSuccessfulSave;

	public void SetPlayerInfo(Player player)
	{
	}

	public void GetGameData(string gameID, long timestamp, SoaringContext context)
	{
	}

	public void DeleteGameData(Session session = null)
	{
	}

	private void HandleGameReset(SoaringContext context)
	{
	}

	public void SaveGameData(string gameData, SoaringContext context)
	{
	}

	public void SaveGameData(SoaringDictionary gameData, SoaringContext context)
	{
	}

	public string ReadETag()
	{
		return null;
	}

	public void DeleteETagFile()
	{
	}
}
