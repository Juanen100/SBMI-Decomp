using System;
using System.IO;
using UnityEngine;

public class SBWebFileServer
{
	private string eTagFile;

	public static DateTime LastSuccessfulSave;

	public void SetPlayerInfo(Player player)
	{
		eTagFile = player.CacheFile("lastETag");
	}

	public void GetGameData(string gameID, long timestamp, SoaringContext context)
	{
		Soaring.RequestSessionData("game", timestamp, context);
	}

	public void DeleteGameData(Session session = null)
	{
		if (session != null)
		{
			SoaringContext soaringContext = Game.CreateSoaringGameResponderContext(HandleGameReset);
			soaringContext.addValue(new SoaringObject(session), "session");
			session.TheGame.CanSave = false;
			TextAsset textAsset = (TextAsset)Resources.Load("Default/DefaultPark");
			if (!(textAsset == null))
			{
				SoaringDictionary gameData = new SoaringDictionary(textAsset.text);
				Resources.UnloadAsset(textAsset);
				SaveGameData(gameData, soaringContext);
			}
		}
	}

	private void HandleGameReset(SoaringContext context)
	{
		if (context != null)
		{
			SoaringObject soaringObject = (SoaringObject)context.objectWithKey("session");
			if (soaringObject != null)
			{
				Session session = (Session)soaringObject.Object;
				session.ThePlayer.DeleteTimestamp();
				session.reinitializeSession = true;
				session.ChangeState("Sync");
			}
		}
	}

	public void SaveGameData(string gameData, SoaringContext context)
	{
		Soaring.SendSessionData("game", SoaringSession.SessionType.PersistantOneWay, new SoaringDictionary(gameData), context);
	}

	public void SaveGameData(SoaringDictionary gameData, SoaringContext context)
	{
		Soaring.SendSessionData("game", SoaringSession.SessionType.PersistantOneWay, gameData, context);
	}

	public string ReadETag()
	{
		lock (eTagFile)
		{
			if (TFUtils.FileIsExists(eTagFile))
			{
				return TFUtils.ReadAllText(eTagFile);
			}
			return null;
		}
	}

	public void DeleteETagFile()
	{
		lock (eTagFile)
		{
			TFUtils.WarningLog("DeleteETagFile");
			if (TFUtils.FileIsExists(eTagFile))
			{
				File.Delete(eTagFile);
			}
		}
	}
}
