using System;

public class AmazonGameCircleExampleInitialization : AmazonGameCircleExampleBase
{
	public enum EInitializationStatus
	{
		Uninitialized = 0,
		InitializationRequested = 1,
		Ready = 2,
		Unavailable = 3
	}

	private EInitializationStatus initializationStatus;

	private DateTime initRequestTime;

	private bool usesLeaderboards;

	private bool usesAchievements;

	private bool usesWhispersync;

	private GameCirclePopupLocation toastLocation;

	private string[] toastLocations;

	private bool enablePopups;

	private string gameCircleInitializationStatusLabel;

	private const string pluginName = "Amazon GameCircle";

	private readonly string pluginInitializationButton;

	private const string initializationmenuTitle = "Initialization";

	private const string usesLeaderboardsLabel = "Use Leaderboards";

	private const string usesAchievementsLabel = "Use Achievements";

	private const string usesWhispersyncLabel = "Use Whispersync";

	private const string toastLocationLabel = "Popup Location";

	private const string popupsDisabledLabel = "Popups Disabled";

	private const string popupsEnabledLabel = "Popups Enabled";

	private const string pluginFailedToInitializeLabel = "Failed to initialize: {0}";

	private readonly string pluginInitializedLabel;

	private const string loadingTimeLabel = "{0,5:N1} seconds";

	public EInitializationStatus InitializationStatus
	{
		get
		{
			return default(EInitializationStatus);
		}
	}

	public override string MenuTitle()
	{
		return null;
	}

	public override void DrawMenu()
	{
	}

	private void DisplayInitGameCircleMenu()
	{
	}

	private void DisplayLoadingGameCircleMenu()
	{
	}

	private void DisplayGameCircleUnavailableMenu()
	{
	}

	private void InitializeGameCircle()
	{
	}

	private void SubscribeToGameCircleInitializationEvents()
	{
	}

	private void UnsubscribeFromGameCircleInitializationEvents()
	{
	}

	private void ServiceNotReadyHandler(string error)
	{
	}

	private void ServiceReadyHandler()
	{
	}
}
