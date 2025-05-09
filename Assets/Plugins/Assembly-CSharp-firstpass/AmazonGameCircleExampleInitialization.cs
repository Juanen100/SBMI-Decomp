using System;
using UnityEngine;

public class AmazonGameCircleExampleInitialization : AmazonGameCircleExampleBase
{
	public enum EInitializationStatus
	{
		Uninitialized = 0,
		InitializationRequested = 1,
		Ready = 2,
		Unavailable = 3
	}

	private const string pluginName = "Amazon GameCircle";

	private const string initializationmenuTitle = "Initialization";

	private const string usesLeaderboardsLabel = "Use Leaderboards";

	private const string usesAchievementsLabel = "Use Achievements";

	private const string usesWhispersyncLabel = "Use Whispersync";

	private const string toastLocationLabel = "Popup Location";

	private const string popupsDisabledLabel = "Popups Disabled";

	private const string popupsEnabledLabel = "Popups Enabled";

	private const string pluginFailedToInitializeLabel = "Failed to initialize: {0}";

	private const string loadingTimeLabel = "{0,5:N1} seconds";

	private EInitializationStatus initializationStatus;

	private DateTime initRequestTime;

	private bool usesLeaderboards = true;

	private bool usesAchievements = true;

	private bool usesWhispersync = true;

	private GameCirclePopupLocation toastLocation = GameCirclePopupLocation.BOTTOM_CENTER;

	private string[] toastLocations;

	private bool enablePopups = true;

	private string gameCircleInitializationStatusLabel;

	private readonly string pluginInitializationButton = string.Format("Initialize {0}", "Amazon GameCircle");

	private readonly string pluginInitializedLabel = string.Format("{0} is ready for use.", "Amazon GameCircle");

	public EInitializationStatus InitializationStatus
	{
		get
		{
			return initializationStatus;
		}
	}

	public AmazonGameCircleExampleInitialization()
	{
		toastLocations = Enum.GetNames(typeof(GameCirclePopupLocation));
	}

	public override string MenuTitle()
	{
		return "Initialization";
	}

	public override void DrawMenu()
	{
		switch (InitializationStatus)
		{
		case EInitializationStatus.Uninitialized:
			DisplayInitGameCircleMenu();
			break;
		case EInitializationStatus.InitializationRequested:
			AmazonGameCircleExampleGUIHelpers.BoxedCenteredLabel("Amazon GameCircle");
			DisplayLoadingGameCircleMenu();
			break;
		case EInitializationStatus.Unavailable:
			DisplayGameCircleUnavailableMenu();
			break;
		case EInitializationStatus.Ready:
			break;
		}
	}

	private void DisplayInitGameCircleMenu()
	{
		if (GUILayout.Button(string.Format(pluginInitializationButton, "Amazon GameCircle")))
		{
			InitializeGameCircle();
		}
		GUILayout.BeginHorizontal();
		GUILayout.Label(GUIContent.none);
		GUILayout.BeginVertical(GUI.skin.box);
		GUILayout.Label(GUIContent.none);
		usesLeaderboards = GUILayout.Toggle(usesLeaderboards, "Use Leaderboards");
		GUILayout.Label(GUIContent.none);
		usesAchievements = GUILayout.Toggle(usesAchievements, "Use Achievements");
		GUILayout.Label(GUIContent.none);
		usesWhispersync = GUILayout.Toggle(usesWhispersync, "Use Whispersync");
		AmazonGameCircleExampleGUIHelpers.AnchoredLabel("Popup Location", TextAnchor.LowerCenter);
		if (toastLocations != null)
		{
			toastLocation = (GameCirclePopupLocation)GUILayout.SelectionGrid((int)toastLocation, toastLocations, 3);
		}
		GUILayout.Label(GUIContent.none);
		if (GUILayout.Button((!enablePopups) ? "Popups Disabled" : "Popups Enabled"))
		{
			enablePopups = !enablePopups;
		}
		GUILayout.EndVertical();
		GUILayout.Label(GUIContent.none);
		GUILayout.EndHorizontal();
	}

	private void DisplayLoadingGameCircleMenu()
	{
		if (!string.IsNullOrEmpty(gameCircleInitializationStatusLabel))
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(gameCircleInitializationStatusLabel);
		}
		AmazonGameCircleExampleGUIHelpers.CenteredLabel(string.Format("{0,5:N1} seconds", (DateTime.Now - initRequestTime).TotalSeconds));
	}

	private void DisplayGameCircleUnavailableMenu()
	{
		if (!string.IsNullOrEmpty(gameCircleInitializationStatusLabel))
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(gameCircleInitializationStatusLabel);
		}
	}

	private void InitializeGameCircle()
	{
		initializationStatus = EInitializationStatus.InitializationRequested;
		SubscribeToGameCircleInitializationEvents();
		initRequestTime = DateTime.Now;
		AGSClient.Init(usesLeaderboards, usesAchievements, usesWhispersync);
	}

	private void SubscribeToGameCircleInitializationEvents()
	{
		AGSClient.ServiceReadyEvent += ServiceReadyHandler;
		AGSClient.ServiceNotReadyEvent += ServiceNotReadyHandler;
	}

	private void UnsubscribeFromGameCircleInitializationEvents()
	{
		AGSClient.ServiceReadyEvent -= ServiceReadyHandler;
		AGSClient.ServiceNotReadyEvent -= ServiceNotReadyHandler;
	}

	private void ServiceNotReadyHandler(string error)
	{
		initializationStatus = EInitializationStatus.Unavailable;
		gameCircleInitializationStatusLabel = string.Format("Failed to initialize: {0}", error);
		UnsubscribeFromGameCircleInitializationEvents();
	}

	private void ServiceReadyHandler()
	{
		initializationStatus = EInitializationStatus.Ready;
		gameCircleInitializationStatusLabel = pluginInitializedLabel;
		UnsubscribeFromGameCircleInitializationEvents();
		AGSClient.SetPopUpEnabled(enablePopups);
		AGSClient.SetPopUpLocation(toastLocation);
	}
}
