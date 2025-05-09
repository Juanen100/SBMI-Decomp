using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DebugManager
{
	private const string FRAMERATE_COUNTER = "hudfps";

	public bool debugPlaceObjects;

	public bool framerateCounter;

	public bool showHitBoxes;

	public bool showFootprints;

	public bool showExpansionBorders;

	public bool freeCameraMode;

	private Session session;

	public DebugManager(Session session)
	{
		this.session = session;
	}

	public void ToggleDebugPlaceObjects(Session session)
	{
		debugPlaceObjects = !debugPlaceObjects;
		Simulation simulation = session.TheGame.simulation;
		foreach (Simulated simulated in simulation.GetSimulateds())
		{
			if (simulated.entity is DebrisEntity)
			{
				simulated.InteractionState.ClearControls();
				if (!debugPlaceObjects)
				{
					simulated.InteractionState.PushControls(new List<IControlBinding>
					{
						new Session.ClearDebrisControl(simulated)
					});
				}
			}
		}
	}

	public void ToggleFramerateCounter(Session session)
	{
		GameObject gameObject = (GameObject)session.CheckAsyncRequest("hudfps");
		if (gameObject == null)
		{
			gameObject = UnityGameResources.CreateEmpty("hudfps");
			gameObject.AddComponent<HUDFPS>();
			session.AddAsyncResponse("hudfps", gameObject);
			framerateCounter = true;
		}
		else
		{
			UnityGameResources.Destroy(gameObject);
			framerateCounter = false;
		}
	}

	public void ToggleHitBoxes(Simulation simulation)
	{
		showHitBoxes = !showHitBoxes;
		simulation.UpdateDebugHitBoxes();
	}

	public void ToggleFootprints(Simulation simulation)
	{
		showFootprints = !showFootprints;
		simulation.UpdateDebugFootprints();
	}

	public void ToggleRMT()
	{
		session.TheGame.store.rmtEnabled = !session.TheGame.store.rmtEnabled;
	}

	public void ToggleExpansionBorders(Simulation simulation)
	{
		showExpansionBorders = !showExpansionBorders;
		simulation.UpdateDebugExpansionBorders();
	}

	public void DeleteGameData()
	{
		session.StopGameSaveTimer();
		session.WebFileServer.DeleteGameData(session);
		if (Directory.Exists(session.ThePlayer.CacheDir()))
		{
			Directory.Delete(session.ThePlayer.CacheDir(), true);
		}
	}

	public void ResetAchievements()
	{
		GameObject gameObject = GameObject.Find("SBGameCenterManager");
		SBGameCenterManager component = gameObject.GetComponent<SBGameCenterManager>();
		component.ResetAchievements();
	}

	public void ToggleFreeCameraMode(SBCamera camera)
	{
		freeCameraMode = !freeCameraMode;
		camera.freeCameraMode = freeCameraMode;
	}

	public void CompleteAllQuests()
	{
		session.TheGame.questManager.DebugCompleteAllQuests(session.TheGame);
	}

	public void ResetDeviceID()
	{
	}
}
