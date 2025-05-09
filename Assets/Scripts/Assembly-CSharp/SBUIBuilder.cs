#define ASSERTS_ON
using System;
using System.Collections.Generic;
using MTools;
using UnityEngine;
using Yarg;

public static class SBUIBuilder
{
	public class ScreenContext
	{
		public int layers;

		public ScreenContext next;

		public override string ToString()
		{
			string text = ((next != null) ? next.ToString() : "null");
			return "[layers: " + layers + "]->" + text;
		}
	}

	private class InteractionStripButtonInfo
	{
		public const string PREFAB_NAME = "ButtonStandinBig";

		public string textureToUse;

		public IControlBinding control;

		public InteractionStripButtonInfo(string textureToUse, IControlBinding control)
		{
			this.textureToUse = textureToUse;
			this.control = control;
		}
	}

	public delegate SBGUIScreen MakeScreen();

	public const float kErrorMessageScale = 0.85f;

	public const float kErrorTitleScale = 0.45f;

	public const float ENABLED_ALPHA = 1f;

	public const float DISABLED_ALPHA = 0.25f;

	public const int MAX_EDIT_MODE_BARS = 14;

	private const string TRACKING_RESOURCE_AMOUNTS = "TrackingResourceAmounts";

	private const string TRACKING_RESOURCE_PERCENTAGES = "TrackingResourcePercentages";

	private static GUIMainView mainView;

	private static Dictionary<string, SBGUIScreen> sCache = new Dictionary<string, SBGUIScreen>();

	private static ScreenContext topContext = null;

	private static string game_revision = null;

	private static Vector3 invWidgetStartPos = new Vector3(-999999f, -999999f, -999999f);

	private static Transform specialBarParent = null;

	private static Vector3 specialBarStartPosition = Vector3.zero;

	private static TFPool<SBGUITimebar> sTimebarPool;

	private static TFPool<SBGUINamebar> sNamebarPool;

	private static Dictionary<int, string> ResourcePrefixes;

	private static readonly Dictionary<Type, string> controlToTextureMap = new Dictionary<Type, string>
	{
		{
			typeof(Session.AcceptPlacementControl),
			"ActionStrip_Accept.png"
		},
		{
			typeof(Session.RejectControl),
			"ActionStrip_Cancel.png"
		},
		{
			typeof(Session.RushControl),
			"IconJellyfishJelly.png"
		},
		{
			typeof(Session.SellControl),
			"ActionStrip_Sell_new.png"
		},
		{
			typeof(Session.StashControl),
			"ActionStrip_Stash.png"
		},
		{
			typeof(Session.RotateControl),
			"EditModeFlipIconOn.png"
		},
		{
			typeof(Session.ClearDebrisControl),
			"ActionStrip_Debris.png"
		},
		{
			typeof(Session.BrowseRecipesControl),
			"ActionStrip_Enter.png"
		}
	};

	private static Dictionary<string, SBGUIButton> sInteractionStripButtons = new Dictionary<string, SBGUIButton>();

	private static SBGUIElement sInteractionStrip;

	private static GUIMainView MainView
	{
		get
		{
			if (mainView == null)
			{
				mainView = GUIMainView.GetInstance();
			}
			return mainView;
		}
	}

	public static ScreenContext PushNewScreenContext()
	{
		ScreenContext screenContext = new ScreenContext();
		screenContext.next = topContext;
		topContext = screenContext;
		return topContext;
	}

	public static void ReleaseScreenContexts(ScreenContext start, ScreenContext end)
	{
		TFUtils.Assert(topContext != null || (start == null && end == null), "Cannot try to release screens since there are no contexts in SBUIBuilder");
		TFUtils.Assert(start != null, "Cannot release a null context on a non-empty gui stack.");
		ScreenContext screenContext = null;
		int num = 0;
		int num2 = 0;
		for (ScreenContext next = topContext; next != start; next = next.next)
		{
			screenContext = next;
			num += next.layers;
		}
		for (ScreenContext next = start; next != end; next = next.next)
		{
			num2 += next.layers;
		}
		ReleaseScreens(num, num2);
		screenContext.next = end;
	}

	private static SBGUIScreen OptionalCacheScreen(string key, MakeScreen make, out bool instantiated)
	{
		instantiated = false;
		if (TFPerfUtils.MemoryLod < CommonUtils.LevelOfDetail.Standard)
		{
			SBGUIScreen result = make();
			instantiated = true;
			return result;
		}
		if (!sCache.ContainsKey(key))
		{
			SBGUIScreen value = make();
			sCache[key] = value;
			instantiated = true;
		}
		else
		{
			sCache[key].MuteButtons(false);
		}
		return sCache[key];
	}

	private static SBGUIScreen CacheScreen(string key, MakeScreen make, out bool instantiated)
	{
		instantiated = false;
		if (!sCache.ContainsKey(key))
		{
			SBGUIScreen value = make();
			sCache[key] = value;
			instantiated = true;
		}
		return sCache[key];
	}

	private static void PushScreen(SBGUIScreen screen)
	{
		topContext.layers++;
		SBGUI.GetInstance().PushGUIScreen(screen);
	}

	public static SBGUIScreen PeekTopScreen()
	{
		return SBGUI.GetInstance().PeekGUIScreen();
	}

	public static SBGUIScreen ReleaseTopScreen()
	{
		return ReleaseScreen(0);
	}

	public static SBGUIScreen ReleaseScreen(int depth)
	{
		ScreenContext next = topContext;
		int num = depth;
		while ((num >= next.layers) ? true : false)
		{
			num -= next.layers;
			next = next.next;
		}
		next.layers--;
		TFUtils.Assert(next.layers >= 0, "Shouldn't be getting negative layer counts. Something must have gone wrong!");
		SBGUIScreen sBGUIScreen = SBGUI.GetInstance().RemoveGUIScreen(depth);
		TFUtils.Assert(sBGUIScreen != null, "Removed a GUI Screen at depth " + depth + ", but it's null)");
		if (!SBGUI.GetInstance().ContainsGUIScreen(sBGUIScreen))
		{
			if (!sCache.ContainsValue(sBGUIScreen))
			{
				sBGUIScreen.Close();
			}
			else
			{
				sBGUIScreen.Deactivate();
			}
		}
		if (sCache.ContainsValue(sBGUIScreen))
		{
			sBGUIScreen.OnPutIntoCache.FireEvent();
		}
		return sBGUIScreen;
	}

	public static void ReleaseScreens(int depth, int layers)
	{
		TFUtils.Assert(depth + layers <= SBGUI.GetInstance().GUIScreenCount, string.Format("Cannot remove {0} layers at depth {1} from GUI Stack with count {2}.", layers, depth, SBGUI.GetInstance().GUIScreenCount));
		while (layers > 0)
		{
			ReleaseScreen(depth);
			layers--;
		}
	}

	public static void ClearScreenCache()
	{
		foreach (string key in sCache.Keys)
		{
			SBGUIScreen sBGUIScreen = sCache[key];
			sBGUIScreen.Deactivate();
			UnityEngine.Object.Destroy(sBGUIScreen.gameObject);
		}
		sCache.Clear();
	}

	public static SBGUIAcceptUI MakeAndPushAcceptUI(Session session, Action<SBGUIEvent, Session> guiEventHandler, Action acceptButtonClickHandler)
	{
		MakeScreen make = () => (SBGUIAcceptUI)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/AcceptUI");
		bool instantiated;
		SBGUIAcceptUI sBGUIAcceptUI = (SBGUIAcceptUI)OptionalCacheScreen("Prefabs/GUI/Screens/AcceptUI", make, out instantiated);
		if (instantiated)
		{
			AddTrackingForResources(sBGUIAcceptUI, session);
		}
		UpdateTrackingForSpecialResource(sBGUIAcceptUI, session);
		sBGUIAcceptUI.session = session;
		SBGUIButton button = sBGUIAcceptUI.AttachActionToButton("accept", acceptButtonClickHandler);
		UpdateGuiEventHandler(session, guiEventHandler);
		sBGUIAcceptUI.gameObject.SetActiveRecursively(true);
		sBGUIAcceptUI.ReactivateButton(button);
		PushScreen(sBGUIAcceptUI);
		return sBGUIAcceptUI;
	}

	public static SBGUIScreen MakeAndPushScratchLayer(Session session)
	{
		SBGUIScreen sBGUIScreen = SBGUIScreen.Create(null, session);
		PushScreen(sBGUIScreen);
		return sBGUIScreen;
	}

	public static SBGUIScreen MakeAndPushEmptyUI(Session session, Action<SBGUIEvent, Session> guiEventHandler)
	{
		SBGUIScreen sBGUIScreen = SBGUIScreen.Create(null, session);
		UpdateGuiEventHandler(session, guiEventHandler);
		PushScreen(sBGUIScreen);
		return sBGUIScreen;
	}

	public static Action<SBGUIEvent, Session> UpdateGuiEventHandler(Session session, Action<SBGUIEvent, Session> guiEventHandler)
	{
		if (session == null)
		{
			TFUtils.Assert(false, "Session is null");
			return null;
		}
		GUIMainView instance = GUIMainView.GetInstance();
		Action<YGEvent> oldYargHandler = instance.FinalEventListener.GetListener();
		Action<SBGUIEvent, Session> result = delegate(SBGUIEvent e, Session s)
		{
			oldYargHandler(e);
		};
		instance.ClearFinalEventListener();
		instance.FinalEventListener.AddListener(delegate(YGEvent x)
		{
			session.TheCamera.HandleGUIEvent(new SBGUIEvent(x));
		});
		if (guiEventHandler != null)
		{
			instance.FinalEventListener.AddListener(delegate(YGEvent x)
			{
				guiEventHandler(new SBGUIEvent(x), session);
			});
		}
		return result;
	}

	private static void AddTrackingForResource(SBGUIScreen screen, string labelKey, int resourceId)
	{
		SBGUILabel sBGUILabel = (SBGUILabel)screen.FindChild(labelKey);
		string text = resourceId.ToString();
		screen.dynamicLabels[text] = sBGUILabel;
		if (!((List<string>)screen.dynamicProperties["TrackingResourceAmounts"]).Contains(text))
		{
			((List<string>)screen.dynamicProperties["TrackingResourceAmounts"]).Add(text);
		}
		sBGUILabel.Text = text;
	}

	private static void AddTrackingForResources(SBGUIScreen screen, Session session)
	{
		TFUtils.Assert(screen != null, "Cannot add tracking to a null screen");
		TFUtils.Assert(session != null, "Cannot add tracking to a null session");
		screen.session = session;
		List<string> value = new List<string>();
		screen.dynamicProperties["TrackingResourceAmounts"] = value;
		screen.UpdateCallback.ClearListeners();
		screen.UpdateCallback.AddListener(UpdateStandardUI);
		AddTrackingForResource(screen, "money_label", ResourceManager.SOFT_CURRENCY);
		AddTrackingForResource(screen, "premium_label", ResourceManager.HARD_CURRENCY);
		AddTrackingForResource(screen, "level_label", ResourceManager.LEVEL);
		List<string> value2 = new List<string>();
		screen.dynamicProperties["TrackingResourcePercentages"] = value2;
		SBGUIProgressMeter value3 = (SBGUIProgressMeter)screen.FindChild("xp_meter");
		string text = ResourceManager.XP.ToString();
		screen.dynamicMeters[text] = value3;
		((List<string>)screen.dynamicProperties["TrackingResourcePercentages"]).Add(text);
		SBGUILabel value4 = (SBGUILabel)screen.FindChild("amount_xp_label");
		screen.dynamicLabels["amount_xp_label"] = value4;
	}

	private static void UpdateTrackingForSpecialResource(SBGUIScreen screen, Session session)
	{
		SBGUIElement sBGUIElement = screen.FindChild("inventory_widget");
		if (invWidgetStartPos == new Vector3(-999999f, -999999f, -999999f))
		{
			invWidgetStartPos = sBGUIElement.tform.localPosition;
		}
		if (specialBarParent == null)
		{
			SBGUIElement sBGUIElement2 = screen.FindChild("special_bar");
			specialBarParent = sBGUIElement2.transform.parent;
			specialBarStartPosition = sBGUIElement2.transform.localPosition;
		}
		bool flag = false;
		if (ResourceManager.SPECIAL_CURRENCY >= 0)
		{
			int currencyDisplayQuestTrigger = session.TheGame.resourceManager.Resources[ResourceManager.SPECIAL_CURRENCY].CurrencyDisplayQuestTrigger;
			if (currencyDisplayQuestTrigger < 0 || session.TheGame.questManager.IsQuestActive((uint)currencyDisplayQuestTrigger) || session.TheGame.questManager.IsQuestCompleted((uint)currencyDisplayQuestTrigger))
			{
				SBGUIElement sBGUIElement3 = screen.FindChild("special_bar");
				if (sBGUIElement3 == null)
				{
					sBGUIElement3 = GUIMainView.GetInstance().transform.Find("special_bar").GetComponent<SBGUIElement>();
					sBGUIElement3.transform.parent = specialBarParent;
					sBGUIElement3.transform.localPosition = specialBarStartPosition;
					sBGUIElement3.gameObject.layer = specialBarParent.gameObject.layer;
				}
				AddTrackingForResource(screen, "special_label", ResourceManager.SPECIAL_CURRENCY);
				string resourceTexture = session.TheGame.resourceManager.Resources[ResourceManager.SPECIAL_CURRENCY].GetResourceTexture();
				((SBGUIPulseImage)screen.FindChild("special_icon")).SetTextureFromAtlas(resourceTexture);
				sBGUIElement.tform.localPosition = invWidgetStartPos;
				sBGUIElement3.SetVisible(true);
				sBGUIElement3.SetActive(true);
			}
			else
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			SBGUIElement sBGUIElement4 = screen.FindChild("special_bar");
			if (sBGUIElement4 != null)
			{
				sBGUIElement4.SetVisible(false);
				sBGUIElement4.SetActive(false);
				sBGUIElement4.SetParent(null);
				sBGUIElement.tform.localPosition = invWidgetStartPos + new Vector3(0f, 0.309429f, 0f);
			}
		}
	}

	private static void UpdateQuestCounter(SBGUIScreen screen, Session session)
	{
		SBGUIStandardScreen sBGUIStandardScreen = (SBGUIStandardScreen)screen;
		sBGUIStandardScreen.QuestCountLabel.SetText(sBGUIStandardScreen.GetQuestButtonCount.ToString());
		sBGUIStandardScreen.HideZeroQuestCount();
	}

	public static SBGUIScreen MakeAndPushStartingProgress(Session session, Action privacyHandler, Action<SBGUIEvent, Session> guiEventHandler, bool makeLoadingBar, bool bPatchy)
	{
		string empty = string.Empty;
		empty = ((!bPatchy) ? "Prefabs/GUI/Screens/StartingProgress" : "Prefabs/GUI/Screens/PatchyLoadingScreen");
		SBGUIScreen sBGUIScreen = (SBGUIScreen)SBGUI.InstantiatePrefab(empty);
		sBGUIScreen.session = session;
		sBGUIScreen.SetParent(null);
		SBScaleForLanguage component = sBGUIScreen.GetComponent<SBScaleForLanguage>();
		if (component != null)
		{
			component.Scale();
		}
		sBGUIScreen.AttachActionToButton("privacy_policy", privacyHandler);
		UpdateGuiEventHandler(session, guiEventHandler);
		SBGUIProgressMeter sBGUIProgressMeter = (SBGUIProgressMeter)sBGUIScreen.FindChild("progress_meter");
		sBGUIScreen.dynamicMeters["loading"] = sBGUIProgressMeter;
		if (!makeLoadingBar)
		{
			sBGUIProgressMeter.GetComponent<Renderer>().enabled = makeLoadingBar;
			sBGUIProgressMeter.gameObject.SetActiveRecursively(makeLoadingBar);
		}
		SBGUILabel sBGUILabel = (SBGUILabel)sBGUIScreen.FindChild("count_label");
		sBGUIScreen.dynamicLabels["progress"] = sBGUILabel;
		sBGUILabel.GetComponent<Renderer>().enabled = makeLoadingBar;
		if (makeLoadingBar)
		{
			MakeActivityIndicator(sBGUIScreen);
		}
		SBGUIElement sBGUIElement = sBGUIScreen.FindChild("loading_spinner");
		if (sBGUIElement != null)
		{
			if (sBGUIElement.GetComponent<Renderer>() != null)
			{
				sBGUIElement.GetComponent<Renderer>().enabled = makeLoadingBar;
			}
			sBGUIElement.gameObject.SetActiveRecursively(makeLoadingBar);
		}
		PushScreen(sBGUIScreen);
		return sBGUIScreen;
	}

	public static SBGUIStandardScreen MakeAndPushStandardUI(Session session, bool allowHiding, Action<SBGUIEvent, Session> guiEventHandler, Action shopClickHandler, Action inventoryClickHandler, Action optionsHandler, Action editClickHandler, Action pavingClickHandler, Action<int, YGEvent> startDragOutHandler, Action<YGEvent> dragThroughHandler, Action openIAPTabHandlerSoft, Action openIAPTabHandlerHard, Action communityEventClickHandler, Action patchyClickHandler, bool editing = false)
	{
		MakeScreen make = () => (SBGUIStandardScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/StandardUI");
		bool instantiated;
		SBGUIStandardScreen screen = (SBGUIStandardScreen)CacheScreen("Prefabs/GUI/Screens/StandardUI", make, out instantiated);
		screen.session = session;
		if (instantiated)
		{
			screen.Initialize(session);
			AddTrackingForResources(screen, session);
			screen.AttachActionToButton("quest", delegate
			{
				screen.ToggleQuestTracker(session, false, true);
			});
		}
		else
		{
			screen.RefreshFromCache();
		}
		UpdateTrackingForSpecialResource(screen, session);
		screen.EnableFullHiding(allowHiding);
		UpdateGuiEventHandler(session, guiEventHandler);
		Action action = delegate
		{
			Vector3 localPosition = screen.questMarker.transform.localPosition;
			localPosition.y -= 0.7f;
			screen.questMarker.transform.localPosition = localPosition;
		};
		Action action2 = delegate
		{
			Vector3 localPosition = screen.questMarker.transform.localPosition;
			localPosition.y += 0.7f;
			screen.questMarker.transform.localPosition = localPosition;
		};
		SBGUILabel sBGUILabel = (SBGUILabel)screen.FindChild("happy_label");
		sBGUILabel.SetText(Language.Get("!!HAPPINESS"));
		Action action3 = delegate
		{
			AnalyticsWrapper.LogEventButtonClick(session.TheGame);
			if (communityEventClickHandler != null)
			{
				communityEventClickHandler();
			}
		};
		screen.ClearButtonActions("marketplace");
		screen.ClearButtonActions("inventory");
		screen.ClearButtonActions("settings");
		screen.ClearButtonActions("edit");
		screen.ClearButtonActions("jj_button");
		screen.ClearButtonActions("coin_button");
		screen.ClearButtonActions("quest_scroll_up");
		screen.ClearButtonActions("quest_scroll_down");
		screen.ClearButtonActions("editpath_toggle");
		screen.ClearButtonActions("community_event");
		screen.ClearButtonActions("patchy");
		SBGUIButton sBGUIButton = screen.AttachActionToButton("marketplace", shopClickHandler);
		screen.AttachActionToButton("inventory", inventoryClickHandler);
		SBGUIButton sBGUIButton2 = screen.AttachActionToButton("settings", optionsHandler);
		screen.AttachActionToButton("edit", editClickHandler);
		screen.AttachActionToButton("editpath_toggle", pavingClickHandler);
		screen.AttachActionToButton("jj_button", openIAPTabHandlerHard);
		screen.AttachActionToButton("coin_button", openIAPTabHandlerSoft);
		screen.AttachActionToButton("quest_scroll_up", action);
		screen.AttachActionToButton("quest_scroll_down", action2);
		screen.AttachActionToButton("community_event", communityEventClickHandler);
		SBGUIButton sBGUIButton3 = screen.AttachActionToButton("patchy", patchyClickHandler);
		if (sBGUIButton3 != null)
		{
			screen.AttachActionToButton("patchy", patchyClickHandler);
		}
		screen.SetInventoryWidgetDraggingCallbacks(startDragOutHandler, dragThroughHandler);
		PushScreen(screen);
		screen.EnableButtons(true);
		SBGUIAtlasButton sBGUIAtlasButton = (SBGUIAtlasButton)screen.FindChild("edit");
		SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)screen.FindChild("edit_mode_fish_person");
		SBGUIAtlasImage sBGUIAtlasImage2 = (SBGUIAtlasImage)screen.FindChild("edit_mode_text");
		SBGUIAtlasImage sBGUIAtlasImage3 = (SBGUIAtlasImage)screen.FindChild("edit_mode_fence_post");
		SBGUIAtlasImage sBGUIAtlasImage4 = (SBGUIAtlasImage)screen.FindChild("edit_mode_placard");
		SBGUIAtlasButton sBGUIAtlasButton2 = (SBGUIAtlasButton)screen.FindChild("editpath_toggle");
		SBGUILabel sBGUILabel2 = (SBGUILabel)screen.FindChild("debug_info");
		List<SBGUIAtlasImage> list = new List<SBGUIAtlasImage>();
		for (int num = 0; num < 14; num++)
		{
			list.Add((SBGUIAtlasImage)screen.FindChild("edit_mode_bar_" + (num + 1)));
		}
		if (SBSettings.ShowDebug)
		{
			if (game_revision == null)
			{
				try
				{
					MBinaryReader fileStream = ResourceUtils.GetFileStream("git_revision", null, "bytes");
					if (fileStream != null)
					{
						byte[] array = fileStream.ReadAllBytes();
						game_revision = " Commit: ";
						for (int num2 = 0; num2 < array.Length; num2++)
						{
							game_revision += (char)array[num2];
						}
						game_revision += "\n";
					}
					sBGUILabel2.Size *= 0.9f;
				}
				catch
				{
					game_revision = string.Empty;
				}
			}
			string text = string.Empty;
			try
			{
				if (Soaring.IsInitialized)
				{
					sBGUILabel2.Size *= 1.2f;
					text = " " + SoaringTime.Epoch.AddSeconds(SoaringTime.AdjustedServerTime).ToString();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Timestamp Threw Error: " + ex.Message);
				game_revision = string.Empty;
			}
			sBGUILabel2.SetText("ID: " + Soaring.Player.UserTag + " Ver: " + SBSettings.BundleVersion + game_revision + SBSettings.SERVER_URL + text);
			sBGUILabel2.SetVisible(true);
		}
		else
		{
			sBGUILabel2.SetVisible(false);
		}
		if (editing)
		{
			sBGUIAtlasButton.SetTextureFromAtlas("EditModeIcon_On.png");
			sBGUIAtlasImage.SetVisible(true);
			sBGUIAtlasImage2.SetVisible(true);
			sBGUIAtlasImage3.SetVisible(true);
			sBGUIAtlasImage4.SetVisible(true);
			sBGUIAtlasButton2.SetVisible(true);
			sBGUIAtlasButton2.enabled = true;
			sBGUIAtlasButton2.SetTextureFromAtlas("EditToggle_Building.png");
			for (int num3 = 0; num3 < list.Count; num3++)
			{
				list[num3].SetVisible(true);
			}
			screen.HideElementsForEditMode(true);
			sBGUIButton.EnableButtons(false);
			sBGUIButton2.EnableButtons(false);
			if (sBGUIButton3 != null)
			{
				sBGUIButton3.SetActive(false);
			}
		}
		else
		{
			sBGUIAtlasButton.SetTextureFromAtlas("EditModeIcon_Off.png");
			sBGUIAtlasImage.SetVisible(false);
			sBGUIAtlasImage2.SetVisible(false);
			sBGUIAtlasImage3.SetVisible(false);
			sBGUIAtlasImage4.SetVisible(false);
			sBGUIAtlasButton2.SetVisible(false);
			sBGUIAtlasButton2.enabled = false;
			for (int num4 = 0; num4 < list.Count; num4++)
			{
				list[num4].SetVisible(false);
			}
			screen.HideElementsForEditMode(false);
			sBGUIButton.EnableButtons(true);
			sBGUIButton2.EnableButtons(true);
			if (sBGUIButton3 != null)
			{
				sBGUIButton3.SetActive(false);
			}
		}
		session.AddAsyncResponse("standardUI", screen, false);
		UpdateQuestCounter(screen, session);
		screen.DisableInactiveElements();
		return screen;
	}

	public static SBGUIStandardScreen MakeAndPushPavingUI(Session session, Action<SBGUIEvent, Session> guiEventHandler, Action acceptHandler, Action editHandler, Action inventoryHandler)
	{
		SBGUIStandardScreen sBGUIStandardScreen = MakeAndPushStandardUI(session, false, guiEventHandler, null, inventoryHandler, null, editHandler, acceptHandler, null, null, null, null, null, null, true);
		SBGUIButton sBGUIButton = (SBGUIButton)sBGUIStandardScreen.FindChild("community_event");
		if (sBGUIButton != null)
		{
			sBGUIButton.SetActive(false);
		}
		SBGUIAtlasButton sBGUIAtlasButton = (SBGUIAtlasButton)sBGUIStandardScreen.FindChild("editpath_toggle");
		sBGUIAtlasButton.SetTextureFromAtlas("EditToggle_Path.png");
		return sBGUIStandardScreen;
	}

	public static SBGUIInsufficientResourcesDialog MakeAndPushInsufficientResourcesDialog(Session session, Dictionary<int, int> insufficientResourceIds, Dictionary<string, int> insufficientResourceTextures, int? rmtCost, string rmtTexture, string acceptLabel, Action okButtonHandler, Action cancelButtonHandler)
	{
		string title = Language.Get("!!PREFAB_NOT_ENOUGH_RESOURCES");
		if (insufficientResourceIds.Count == 1)
		{
			foreach (int key in insufficientResourceIds.Keys)
			{
				string text = Language.Get(session.TheGame.resourceManager.Resources[key].Name);
				title = Language.Get("!!PREFAB_NOT_ENOUGH") + " " + text;
			}
		}
		string message = Language.Get("!!PREFAB_STILL_NEED");
		SBGUIInsufficientResourcesDialog sBGUIInsufficientResourcesDialog = (SBGUIInsufficientResourcesDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/InsufficientResourcesDialog");
		sBGUIInsufficientResourcesDialog.session = session;
		sBGUIInsufficientResourcesDialog.SetParent(null);
		sBGUIInsufficientResourcesDialog.transform.localPosition = Vector3.zero;
		sBGUIInsufficientResourcesDialog.AttachActionToButton("nevermind_button", cancelButtonHandler);
		sBGUIInsufficientResourcesDialog.AttachActionToButton("shopping_button", okButtonHandler);
		sBGUIInsufficientResourcesDialog.AttachActionToButton("TouchableBackground", cancelButtonHandler);
		if (insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.HALLOWEEN_CURRENCY].Did) || insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.CHRISTMAS_CURRENCY].Did) || insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.CHRISTMAS_CURRENCY_V2].Did) || insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.VALENTINES_CURRENCY].Did) || insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.SPONGY_GAMES_CURRENCY].Did) || (ResourceManager.SPECIAL_CURRENCY >= 0 && insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.SPECIAL_CURRENCY].Did)))
		{
			rmtCost = null;
		}
		sBGUIInsufficientResourcesDialog.SetUp(title, message, acceptLabel, insufficientResourceTextures, rmtCost, rmtTexture, string.Empty);
		if (!session.TheGame.featureManager.CheckFeature("jit_jj_purchases"))
		{
			SBGUIButton component = sBGUIInsufficientResourcesDialog.FindChild("shopping_button").GetComponent<SBGUIButton>();
			component.ClickEvent -= okButtonHandler;
			component.EnableButtons(false);
			component.GetComponent<YGAtlasSprite>().SetColor(new Color(1f, 1f, 1f, 0.15f));
			sBGUIInsufficientResourcesDialog.FindChild("cost_label").GetComponent<SBGUILabel>().SetColor(new Color(0.5f, 0.5f, 0.5f));
		}
		if (insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.HALLOWEEN_CURRENCY].Did) || insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.CHRISTMAS_CURRENCY].Did) || insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.CHRISTMAS_CURRENCY_V2].Did) || insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.VALENTINES_CURRENCY].Did) || insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.SPONGY_GAMES_CURRENCY].Did) || (ResourceManager.SPECIAL_CURRENCY >= 0 && insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.SPECIAL_CURRENCY].Did)))
		{
			SBGUIElement component2 = sBGUIInsufficientResourcesDialog.FindChild("shopping_buttonframe").GetComponent<SBGUIElement>();
			component2.SetVisible(false);
			SBGUIButton component3 = sBGUIInsufficientResourcesDialog.FindChild("shopping_button").GetComponent<SBGUIButton>();
			component3.ClickEvent -= okButtonHandler;
			component3.EnableButtons(false);
			component3.SetVisible(false);
			SBGUILabel component4 = sBGUIInsufficientResourcesDialog.FindChild("shopping_label").GetComponent<SBGUILabel>();
			component4.SetVisible(false);
			SBGUILabel component5 = sBGUIInsufficientResourcesDialog.FindChild("cost_label").GetComponent<SBGUILabel>();
			component5.SetVisible(false);
			SBGUIElement component6 = sBGUIInsufficientResourcesDialog.FindChild("cost_marker").GetComponent<SBGUIElement>();
			component6.SetVisible(false);
			SBGUIElement component7 = sBGUIInsufficientResourcesDialog.FindChild("nevermind_buttonframe").GetComponent<SBGUIElement>();
			component7.SetPosition(sBGUIInsufficientResourcesDialog.GetScreenPosition().x, component7.GetScreenPosition().y, component7.WorldPosition.z);
			SBGUIButton component8 = sBGUIInsufficientResourcesDialog.FindChild("nevermind_button").GetComponent<SBGUIButton>();
			component8.SetPosition(sBGUIInsufficientResourcesDialog.GetScreenPosition().x, component8.GetScreenPosition().y, component8.WorldPosition.z);
		}
		PushScreen(sBGUIInsufficientResourcesDialog);
		return sBGUIInsufficientResourcesDialog;
	}

	public static SBGUIConfirmationDialog MakeAndPushConfirmationDialog(Session session, Action<SBGUIEvent, Session> guiEventHandler, string title, string message, string acceptLabel, string cancelLabel, Dictionary<string, int> resources, Action okButtonHandler, Action cancelButtonHandler, bool unmutable = false)
	{
		SBGUIConfirmationDialog sBGUIConfirmationDialog = (SBGUIConfirmationDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/ConfirmationDialog");
		sBGUIConfirmationDialog.session = session;
		if (unmutable)
		{
			(sBGUIConfirmationDialog.FindChild("cancel_button") as SBGUIButton).unmutable = true;
			(sBGUIConfirmationDialog.FindChild("okay_button") as SBGUIButton).unmutable = true;
		}
		if (guiEventHandler != null)
		{
			UpdateGuiEventHandler(session, guiEventHandler);
		}
		if (cancelButtonHandler != null)
		{
			sBGUIConfirmationDialog.AttachActionToButton("cancel_button", cancelButtonHandler);
		}
		sBGUIConfirmationDialog.AttachActionToButton("TouchableBackground", delegate
		{
		});
		sBGUIConfirmationDialog.AttachActionToButton("okay_button", okButtonHandler);
		sBGUIConfirmationDialog.SetUp(title, message, acceptLabel, cancelLabel, resources, string.Empty);
		SBGUIStandardScreen sBGUIStandardScreen = (SBGUIStandardScreen)session.CheckAsyncRequest("standard_screen");
		if (sBGUIStandardScreen != null)
		{
			sBGUIStandardScreen.EnableUI(false);
			session.AddAsyncResponse("standard_screen", sBGUIStandardScreen);
		}
		PushScreen(sBGUIConfirmationDialog);
		return sBGUIConfirmationDialog;
	}

	public static SBGUIFoundItemScreen MakeAndPushAcknowledgeDialog(Session session, Action<SBGUIEvent, Session> guiEventHandler, string title, string message, string texture, string acceptLabel, Action okButtonHandler)
	{
		SBGUIFoundItemScreen sBGUIFoundItemScreen = (SBGUIFoundItemScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/FoundItemScreen");
		sBGUIFoundItemScreen.session = session;
		if (guiEventHandler != null)
		{
			UpdateGuiEventHandler(session, guiEventHandler);
		}
		sBGUIFoundItemScreen.AttachActionToButton("okay", okButtonHandler);
		sBGUIFoundItemScreen.AttachActionToButton("TouchableBackground", delegate
		{
		});
		sBGUIFoundItemScreen.Setup(title, message, texture, false, string.Empty);
		session.TheSoundEffectManager.PlaySound("Error");
		SBGUIStandardScreen sBGUIStandardScreen = (SBGUIStandardScreen)session.CheckAsyncRequest("standard_screen");
		if (sBGUIStandardScreen != null)
		{
			sBGUIStandardScreen.EnableUI(false);
			session.AddAsyncResponse("standard_screen", sBGUIStandardScreen);
		}
		PushScreen(sBGUIFoundItemScreen);
		return sBGUIFoundItemScreen;
	}

	public static SBGUIConfirmationDialog MakeAndPushExpansionDialog(Session session, Action<SBGUIEvent, Session> guiEventHandler, string title, string message, string acceptLabel, string cancelLabel, Dictionary<string, int> resources, Action okButtonHandler, Action cancelButtonHandler, bool unmutable = false)
	{
		SBGUIConfirmationDialog sBGUIConfirmationDialog = (SBGUIConfirmationDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/ExpansionDialog");
		sBGUIConfirmationDialog.session = session;
		if (unmutable)
		{
			(sBGUIConfirmationDialog.FindChild("cancel_button") as SBGUIButton).unmutable = true;
			(sBGUIConfirmationDialog.FindChild("okay_button") as SBGUIButton).unmutable = true;
		}
		if (guiEventHandler != null)
		{
			UpdateGuiEventHandler(session, guiEventHandler);
		}
		if (cancelButtonHandler != null)
		{
			sBGUIConfirmationDialog.AttachActionToButton("cancel_button", cancelButtonHandler);
		}
		sBGUIConfirmationDialog.AttachActionToButton("TouchableBackground", delegate
		{
		});
		sBGUIConfirmationDialog.AttachActionToButton("okay_button", okButtonHandler);
		sBGUIConfirmationDialog.SetUp(title, message, acceptLabel, cancelLabel, resources, Language.Get("!!PREFAB_COSTS"));
		SBGUIStandardScreen sBGUIStandardScreen = (SBGUIStandardScreen)session.CheckAsyncRequest("standard_screen");
		if (sBGUIStandardScreen != null)
		{
			sBGUIStandardScreen.EnableUI(false);
			session.AddAsyncResponse("standard_screen", sBGUIStandardScreen);
		}
		PushScreen(sBGUIConfirmationDialog);
		return sBGUIConfirmationDialog;
	}

	public static SBGUIGetJellyDialog MakeAndPushGetJellyDialog(Session session, Action<SBGUIEvent, Session> guiEventHandler, string title, string message, string question, string acceptLabel, string cancelLabel, Dictionary<string, int> resources, Action okButtonHandler, Action cancelButtonHandler, bool unmutable = false)
	{
		SBGUIGetJellyDialog sBGUIGetJellyDialog = (SBGUIGetJellyDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/GetJellyDialog");
		sBGUIGetJellyDialog.session = session;
		if (unmutable)
		{
			(sBGUIGetJellyDialog.FindChild("cancel_button") as SBGUIButton).unmutable = true;
			(sBGUIGetJellyDialog.FindChild("okay_button") as SBGUIButton).unmutable = true;
		}
		if (guiEventHandler != null)
		{
			UpdateGuiEventHandler(session, guiEventHandler);
		}
		if (cancelButtonHandler != null)
		{
			sBGUIGetJellyDialog.AttachActionToButton("cancel_button", cancelButtonHandler);
		}
		sBGUIGetJellyDialog.AttachActionToButton("TouchableBackground", delegate
		{
		});
		sBGUIGetJellyDialog.AttachActionToButton("okay_button", okButtonHandler);
		sBGUIGetJellyDialog.SetUp(title, message, question, acceptLabel, cancelLabel, resources);
		SBGUIStandardScreen sBGUIStandardScreen = (SBGUIStandardScreen)session.CheckAsyncRequest("standard_screen");
		if (sBGUIStandardScreen != null)
		{
			sBGUIStandardScreen.EnableUI(false);
			session.AddAsyncResponse("standard_screen", sBGUIStandardScreen);
		}
		PushScreen(sBGUIGetJellyDialog);
		return sBGUIGetJellyDialog;
	}

	public static SBGUIMicroConfirmDialog MakeAndPushJjMicroConfirmDialog(Session session, Action<SBGUIEvent, Session> overrideGuiEventHandler, string message, Cost.CostAtTime jjAmount, Action acceptHandler, Action cancelHandler, Vector2 screenPosition)
	{
		SBGUIMicroConfirmDialog sBGUIMicroConfirmDialog = (SBGUIMicroConfirmDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/HardSpendMicroconfirm");
		if (overrideGuiEventHandler != null)
		{
			UpdateGuiEventHandler(session, overrideGuiEventHandler);
		}
		sBGUIMicroConfirmDialog.AttachActionToButton("cancel_button", cancelHandler);
		sBGUIMicroConfirmDialog.AttachActionToButton("touch_mask", cancelHandler);
		sBGUIMicroConfirmDialog.AttachActionToButton("modality_enforcer", cancelHandler);
		sBGUIMicroConfirmDialog.AttachActionToButton("okay_button", acceptHandler);
		SBGUIShadowedLabel component = sBGUIMicroConfirmDialog.FindChild("message").GetComponent<SBGUIShadowedLabel>();
		component.Text = Language.Get("!!PREFAB_CONFIRM_PURCHASE_SHORT");
		sBGUIMicroConfirmDialog.SetHardAmount(jjAmount(TFUtils.EpochTime()).ResourceAmounts[ResourceManager.HARD_CURRENCY]);
		SBGUIStandardScreen sBGUIStandardScreen = (SBGUIStandardScreen)session.CheckAsyncRequest("standard_screen");
		if (sBGUIStandardScreen != null)
		{
			sBGUIStandardScreen.EnableUI(false);
			session.AddAsyncResponse("standard_screen", sBGUIStandardScreen);
		}
		Bounds totalBounds = sBGUIMicroConfirmDialog.TotalBounds;
		int num = (int)(totalBounds.size.x / 0.01f * GUIView.ResolutionScaleFactor());
		int num2 = (int)(totalBounds.size.y / 0.01f * GUIView.ResolutionScaleFactor());
		if (screenPosition.x - (float)(num / 2) < 0f)
		{
			screenPosition.x = num / 2;
		}
		if (screenPosition.x + (float)(num / 2) > (float)Screen.width)
		{
			screenPosition.x = Screen.width - num / 2;
		}
		if (screenPosition.y - (float)(num2 / 2) < 0f)
		{
			screenPosition.y = num2 / 2;
		}
		if (screenPosition.y + (float)(num2 / 2) > (float)Screen.height)
		{
			screenPosition.y = Screen.height - num2 / 2;
		}
		sBGUIMicroConfirmDialog.SetScreenPosition(screenPosition);
		PushScreen(sBGUIMicroConfirmDialog);
		return sBGUIMicroConfirmDialog;
	}

	public static SBGUICharacterDialog MakeAndAddDialogSequence(SBGUIScreen parent, Session session, List<object> sequence, Action<int> dialogChangeHandler)
	{
		SBGUICharacterDialog sBGUICharacterDialog = (SBGUICharacterDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/CharacterChat");
		sBGUICharacterDialog.session = session;
		sBGUICharacterDialog.SetParent(parent);
		sBGUICharacterDialog.transform.localPosition = Vector3.zero;
		sBGUICharacterDialog.LoadSequence(sequence);
		sBGUICharacterDialog.DialogChange.AddListener(dialogChangeHandler);
		return sBGUICharacterDialog;
	}

	public static SBGUIQuestDialog MakeAndAddQuestStartDialog(SBGUIScreen parent, Session session, List<Reward> rewards, string title, string icon)
	{
		SBGUIQuestDialog sBGUIQuestDialog = (SBGUIQuestDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/QuestStart");
		sBGUIQuestDialog.session = session;
		sBGUIQuestDialog.SetParent(parent);
		sBGUIQuestDialog.transform.localPosition = Vector3.zero;
		sBGUIQuestDialog.SetupQuestDialogInfo(title, icon);
		sBGUIQuestDialog.SetRewardIcons(session, rewards, string.Empty);
		return sBGUIQuestDialog;
	}

	public static SBGUIAutoQuestStatusDialog MakeAndAddAutoQuestStartDialog(SBGUIScreen parent, SBGUIStandardScreen screen, Session session, List<Reward> rewards, QuestDefinition questDef, List<ConditionDescription> steps, Action allDoneButton, Action makeButton)
	{
		SBGUIAutoQuestStatusDialog dialog = (SBGUIAutoQuestStatusDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/AutoQuestStatus");
		dialog.session = session;
		dialog.SetParent(parent);
		dialog.transform.localPosition = Vector3.zero;
		string dialogHeading = questDef.DialogHeading;
		string dialogBody = questDef.DialogBody;
		string portrait = questDef.Portrait;
		List<QuestBookendInfo.ChunkConditions> chunks = questDef.End.Chunks;
		string stepPrefabName = "Prefabs/GUI/Widgets/AutoQuest_Step";
		if (allDoneButton != null)
		{
			dialog.AttachActionToButton("done", allDoneButton);
		}
		dialog.ReadyEvent += delegate
		{
			dialog.CreateScrollRegionUI(screen, chunks, steps, makeButton, stepPrefabName);
		};
		dialog.SetupDialogInfo(dialogHeading, dialogBody, portrait, rewards, steps, questDef);
		return dialog;
	}

	public static SBGUIChunkQuestDialog MakeAndAddQuestChunkStartDialog(SBGUIScreen parent, SBGUIStandardScreen screen, Session session, List<Reward> rewards, QuestDefinition questDef, List<ConditionDescription> steps, Action findButton)
	{
		SBGUIChunkQuestDialog dialog = (SBGUIChunkQuestDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/QuestStatusChunk");
		dialog.session = session;
		dialog.SetParent(parent);
		dialog.transform.localPosition = Vector3.zero;
		string name = Language.Get(questDef.Name);
		string dialogHeading = questDef.DialogHeading;
		string dialogBody = questDef.DialogBody;
		string portrait = questDef.Portrait;
		List<QuestBookendInfo.ChunkConditions> chunks = questDef.End.Chunks;
		string stepPrefabName = "Prefabs/GUI/Widgets/QuestStartChunk_Step";
		dialog.ReadyEvent += delegate
		{
			dialog.CreateScrollRegionUI(screen, chunks, steps, findButton, stepPrefabName);
		};
		dialog.SetupChunkDialogInfo(dialogHeading, dialogBody, portrait, name, false, questDef);
		dialog.SetQuestLineInfo(questDef.QuestLine, null, session.TheGame.questManager.GetQuestLineProgress(questDef), true);
		dialog.SetRewardIcons(session, rewards, string.Empty);
		dialog.CenterRewards();
		return dialog;
	}

	public static SBGUIQuestDialog MakeAndPushQuestStatusDialog(SBGUIStandardScreen screen, Session session, QuestDefinition questDef, List<ConditionDescription> steps, Action okButton, Action findButton)
	{
		List<Reward> list = new List<Reward>();
		list.Add(questDef.Reward);
		List<Reward> rewards = list;
		string key = Language.Get(questDef.Name);
		bool hasCountField = questDef.End.Chunks[0].Condition.hasCountField;
		string prefabName = "Prefabs/GUI/Screens/QuestStatus";
		string name = Language.Get(key);
		string icon = questDef.Icon;
		List<QuestBookendInfo.ChunkConditions> chunks = questDef.End.Chunks;
		MakeScreen make = () => (SBGUIQuestDialog)SBGUI.InstantiatePrefab(prefabName);
		bool instantiated;
		SBGUIQuestDialog sBGUIQuestDialog = (SBGUIQuestDialog)OptionalCacheScreen(prefabName, make, out instantiated);
		if (instantiated)
		{
			sBGUIQuestDialog.session = session;
			sBGUIQuestDialog.SetParent(null);
			sBGUIQuestDialog.transform.localPosition = Vector3.zero;
			if (okButton != null)
			{
				sBGUIQuestDialog.AttachActionToButton("okay", okButton);
				sBGUIQuestDialog.AttachActionToButton("TouchableBackground", okButton);
			}
		}
		else
		{
			sBGUIQuestDialog.gameObject.SetActiveRecursively(true);
		}
		if (session.TheGame.resourceManager.PlayerLevelAmount < 5)
		{
			sBGUIQuestDialog.SetupQuestDialogInfo(name, icon, steps, hasCountField);
		}
		else
		{
			sBGUIQuestDialog.SetupQuestDialogInfo(name, icon, steps, hasCountField, chunks, findButton);
		}
		sBGUIQuestDialog.SetRewardIcons(session, rewards, string.Empty);
		PushScreen(sBGUIQuestDialog);
		return sBGUIQuestDialog;
	}

	public static SBGUIAutoQuestStatusDialog MakeAndPushAutoQuestStatusDialog(SBGUIStandardScreen screen, Session session, QuestDefinition questDef, List<ConditionDescription> steps, Action okButton, Action allDoneButton, Action makeButton)
	{
		List<Reward> list = new List<Reward>();
		list.Add(questDef.Reward);
		List<Reward> pRewards = list;
		string prefabName = "Prefabs/GUI/Screens/AutoQuestStatus";
		string dialogHeading = questDef.DialogHeading;
		string dialogBody = questDef.DialogBody;
		string portrait = questDef.Portrait;
		List<QuestBookendInfo.ChunkConditions> chunks = questDef.End.Chunks;
		MakeScreen make = () => (SBGUIAutoQuestStatusDialog)SBGUI.InstantiatePrefab(prefabName);
		bool instantiated;
		SBGUIAutoQuestStatusDialog dialog = (SBGUIAutoQuestStatusDialog)OptionalCacheScreen(prefabName, make, out instantiated);
		if (instantiated)
		{
			dialog.session = session;
			dialog.SetParent(null);
			dialog.transform.localPosition = Vector3.zero;
			if (okButton != null)
			{
				dialog.AttachActionToButton("okay", okButton);
				dialog.AttachActionToButton("TouchableBackground", okButton);
			}
			if (allDoneButton != null)
			{
				dialog.AttachActionToButton("done", allDoneButton);
			}
			dialog.ReadyEvent += delegate
			{
				dialog.CreateScrollRegionUI(screen, chunks, steps, makeButton);
			};
		}
		else
		{
			dialog.gameObject.SetActiveRecursively(true);
			dialog.CreateScrollRegionUI(screen, chunks, steps, makeButton);
		}
		dialog.SetupDialogInfo(dialogHeading, dialogBody, portrait, pRewards, steps, questDef);
		PushScreen(dialog);
		return dialog;
	}

	public static SBGUIChunkQuestDialog MakeAndPushChunkQuestStatusDialog(SBGUIStandardScreen screen, Session session, QuestDefinition questDef, List<ConditionDescription> steps, Action findButton, Action okButton)
	{
		List<Reward> list = new List<Reward>();
		list.Add(questDef.Reward);
		List<Reward> rewards = list;
		string name = Language.Get(questDef.Name);
		string prefabName = "Prefabs/GUI/Screens/QuestStatusChunk";
		string dialogHeading = questDef.DialogHeading;
		string dialogBody = questDef.DialogBody;
		string portrait = questDef.Portrait;
		List<QuestBookendInfo.ChunkConditions> chunks = questDef.End.Chunks;
		MakeScreen make = () => (SBGUIChunkQuestDialog)SBGUI.InstantiatePrefab(prefabName);
		bool instantiated;
		SBGUIChunkQuestDialog dialog = (SBGUIChunkQuestDialog)OptionalCacheScreen(prefabName, make, out instantiated);
		if (instantiated)
		{
			dialog.session = session;
			dialog.SetParent(null);
			dialog.transform.localPosition = Vector3.zero;
			if (okButton != null)
			{
				dialog.AttachActionToButton("okay", okButton);
				dialog.AttachActionToButton("TouchableBackground", okButton);
			}
			dialog.ReadyEvent += delegate
			{
				dialog.CreateScrollRegionUI(screen, chunks, steps, findButton);
			};
		}
		else
		{
			dialog.gameObject.SetActiveRecursively(true);
			dialog.CreateScrollRegionUI(screen, chunks, steps, findButton);
		}
		dialog.SetupChunkDialogInfo(dialogHeading, dialogBody, portrait, name, false, questDef);
		dialog.SetQuestLineInfo(questDef.QuestLine, null, session.TheGame.questManager.GetQuestLineProgress(questDef), true);
		dialog.SetRewardIcons(session, rewards, string.Empty);
		if (instantiated)
		{
			dialog.CenterRewards();
		}
		PushScreen(dialog);
		return dialog;
	}

	public static SBGUIQuestDialog MakeAndAddQuestCompleteDialog(SBGUIScreen parent, Session session, List<Reward> rewards, string title, string icon)
	{
		SBGUIQuestDialog sBGUIQuestDialog = (SBGUIQuestDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/QuestComplete");
		sBGUIQuestDialog.session = session;
		sBGUIQuestDialog.SetParent(parent);
		sBGUIQuestDialog.transform.localPosition = Vector3.zero;
		sBGUIQuestDialog.SetupQuestDialogInfo(title, icon);
		sBGUIQuestDialog.SetRewardIcons(session, rewards, string.Empty);
		session.PlaySeaflowerAndBubbleScreenSwipeEffect();
		return sBGUIQuestDialog;
	}

	public static SBGUIAutoQuestCompleteDialog MakeAndAddAutoQuestCompleteDialog(SBGUIScreen parent, SBGUIStandardScreen screen, Session session, List<Reward> rewards, QuestDefinition questDef)
	{
		SBGUIAutoQuestCompleteDialog sBGUIAutoQuestCompleteDialog = (SBGUIAutoQuestCompleteDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/AutoQuestCompleteScreen");
		sBGUIAutoQuestCompleteDialog.session = session;
		sBGUIAutoQuestCompleteDialog.SetParent(parent);
		sBGUIAutoQuestCompleteDialog.transform.localPosition = Vector3.zero;
		string dialogHeading = questDef.DialogHeading;
		string dialogBody = questDef.DialogBody;
		string portrait = questDef.Portrait;
		sBGUIAutoQuestCompleteDialog.SetupDialogInfo(dialogHeading, dialogBody, portrait, rewards, questDef);
		session.PlaySeaflowerAndBubbleScreenSwipeEffect();
		return sBGUIAutoQuestCompleteDialog;
	}

	public static SBGUIChunkQuestDialog MakeAndAddQuestChunkCompleteDialog(SBGUIScreen parent, SBGUIStandardScreen screen, Session session, List<Reward> rewards, QuestDefinition questDef, List<ConditionDescription> steps)
	{
		SBGUIChunkQuestDialog sBGUIChunkQuestDialog = (SBGUIChunkQuestDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/QuestCompleteChunk");
		sBGUIChunkQuestDialog.session = session;
		sBGUIChunkQuestDialog.SetParent(parent);
		sBGUIChunkQuestDialog.transform.localPosition = Vector3.zero;
		string name = Language.Get(questDef.Name);
		string dialogHeading = questDef.DialogHeading;
		string dialogBody = questDef.DialogBody;
		string portrait = questDef.Portrait;
		sBGUIChunkQuestDialog.SetupChunkDialogInfo(dialogHeading, dialogBody, portrait, name, true, questDef);
		sBGUIChunkQuestDialog.SetQuestLineInfo(questDef.QuestLine, session.TheGame.questManager.GetQuestLineLastProgress(questDef), session.TheGame.questManager.GetQuestLineProgress(questDef), false);
		sBGUIChunkQuestDialog.SetRewardIcons(session, rewards, string.Empty);
		sBGUIChunkQuestDialog.CenterRewards();
		session.PlaySeaflowerAndBubbleScreenSwipeEffect();
		return sBGUIChunkQuestDialog;
	}

	public static SBGUIQuestDialog MakeAndAddBootyQuestCompleteDialog(SBGUIScreen parent, Session session, List<Reward> rewards, string title, string icon)
	{
		SBGUIQuestDialog sBGUIQuestDialog = (SBGUIQuestDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/BootyQuestComplete");
		sBGUIQuestDialog.session = session;
		sBGUIQuestDialog.SetParent(parent);
		sBGUIQuestDialog.transform.localPosition = Vector3.zero;
		sBGUIQuestDialog.SetupQuestDialogInfo(title, icon);
		sBGUIQuestDialog.SetRewardIcons(session, rewards, string.Empty);
		session.PlaySeaflowerAndBubbleScreenSwipeEffect();
		return sBGUIQuestDialog;
	}

	public static SBGUIQuestLineDialog MakeAndAddQuestLineStartDialog(SBGUIScreen parent, Session session, List<Reward> rewards, string dialogHeading, string dialogBody, string portrait, string rewardTexture, string rewardName)
	{
		SBGUIQuestLineDialog sBGUIQuestLineDialog = (SBGUIQuestLineDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/QuestLineStart");
		sBGUIQuestLineDialog.session = session;
		sBGUIQuestLineDialog.SetParent(parent);
		sBGUIQuestLineDialog.transform.localPosition = Vector3.zero;
		rewardName = string.Empty;
		sBGUIQuestLineDialog.SetupQuestLineDialogInfo(dialogHeading, dialogBody, portrait, rewardTexture, rewardName);
		if (rewards.Count > 0)
		{
			sBGUIQuestLineDialog.SetRewardIcons(session, rewards, string.Empty);
			sBGUIQuestLineDialog.CenterRewards();
		}
		else
		{
			sBGUIQuestLineDialog.ToggleRewardWindow(false);
		}
		TFUtils.DebugLog("SBUIBuilder, Made Quest Line Start Dialog " + sBGUIQuestLineDialog.name);
		return sBGUIQuestLineDialog;
	}

	public static SBGUIQuestLineDialog MakeAndAddQuestLineCompleteDialog(SBGUIScreen parent, Session session, List<Reward> rewards, string dialogHeading, string dialogBody, string portrait, string rewardTexture, string rewardName)
	{
		SBGUIQuestLineDialog sBGUIQuestLineDialog = (SBGUIQuestLineDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/QuestLineComplete");
		sBGUIQuestLineDialog.session = session;
		sBGUIQuestLineDialog.SetParent(parent);
		sBGUIQuestLineDialog.transform.localPosition = Vector3.zero;
		sBGUIQuestLineDialog.SetupQuestLineDialogInfo(dialogHeading, dialogBody, portrait, rewardTexture, rewardName);
		if (rewards.Count > 0)
		{
			sBGUIQuestLineDialog.SetRewardIcons(session, rewards, string.Empty);
			sBGUIQuestLineDialog.CenterRewards();
		}
		else
		{
			sBGUIQuestLineDialog.ToggleRewardWindow(false);
		}
		TFUtils.DebugLog("SBUIBuilder, Made Quest Line Complete Dialog " + sBGUIQuestLineDialog.name);
		return sBGUIQuestLineDialog;
	}

	public static SBGUICharacterBusyScreen MakeAndPushUnitBusyUI(SBGUIStandardScreen screen, Session session, Simulated pSimulated, Task pTask, Action pFeedWishAction, Action pRushWishAction, Action pRushTaskAction, Action closeButton)
	{
		string prefabName = "Prefabs/GUI/Screens/CharacterBusyScreen";
		MakeScreen make = () => (SBGUICharacterBusyScreen)SBGUI.InstantiatePrefab(prefabName);
		bool instantiated;
		SBGUICharacterBusyScreen sBGUICharacterBusyScreen = (SBGUICharacterBusyScreen)OptionalCacheScreen(prefabName, make, out instantiated);
		AndroidBack.getInstance().push(closeButton, sBGUICharacterBusyScreen);
		if (instantiated)
		{
			sBGUICharacterBusyScreen.session = session;
			sBGUICharacterBusyScreen.SetParent(null);
			sBGUICharacterBusyScreen.transform.localPosition = Vector3.zero;
			if (closeButton != null)
			{
				sBGUICharacterBusyScreen.AttachActionToButton("close", closeButton);
				sBGUICharacterBusyScreen.AttachActionToButton("TouchableBackground", closeButton);
			}
		}
		else
		{
			sBGUICharacterBusyScreen.gameObject.SetActiveRecursively(true);
		}
		sBGUICharacterBusyScreen.SetupDialogInfo(pSimulated, pTask, pFeedWishAction, pRushWishAction, pRushTaskAction);
		PushScreen(sBGUICharacterBusyScreen);
		return sBGUICharacterBusyScreen;
	}

	public static SBGUICharacterIdleScreen MakeAndPushUnitIdleUI(SBGUIStandardScreen screen, Session session, Simulated pSimulated, List<TaskData> pTaskDatas, Action pFeedWishAction, Action pRushWishAction, Action<int> pDoTaskAction, Action closeButton)
	{
		string prefabName = "Prefabs/GUI/Screens/CharacterIdleScreen";
		MakeScreen make = () => (SBGUICharacterIdleScreen)SBGUI.InstantiatePrefab(prefabName);
		bool instantiated;
		SBGUICharacterIdleScreen dialog = (SBGUICharacterIdleScreen)OptionalCacheScreen(prefabName, make, out instantiated);
		AndroidBack.getInstance().push(closeButton, dialog);
		if (instantiated)
		{
			dialog.session = session;
			dialog.SetParent(null);
			dialog.transform.localPosition = Vector3.zero;
			if (closeButton != null)
			{
				dialog.AttachActionToButton("close", closeButton);
				dialog.AttachActionToButton("TouchableBackground", closeButton);
			}
			dialog.SetupDialogInfo(pSimulated, pFeedWishAction, pRushWishAction, pDoTaskAction);
			dialog.ReadyEvent += delegate
			{
				dialog.CreateScrollRegionUI(pTaskDatas);
			};
		}
		else
		{
			ReleaseTopScreen();
			dialog.gameObject.SetActiveRecursively(true);
			dialog.SetupDialogInfo(pSimulated, pFeedWishAction, pRushWishAction, pDoTaskAction);
			dialog.CreateScrollRegionUI(pTaskDatas);
		}
		PushScreen(dialog);
		return dialog;
	}

	public static SBGUIProgressDialog MakeAndAddProgressDialog(SBGUIScreen parent, Session session, string title, string description, Cost rush_cost, Action onRush, Action onClose)
	{
		SBGUIProgressDialog sBGUIProgressDialog = (SBGUIProgressDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/BuildingProgress");
		sBGUIProgressDialog.session = session;
		sBGUIProgressDialog.SetParent(parent);
		sBGUIProgressDialog.transform.localPosition = Vector3.zero;
		if (rush_cost == null || rush_cost.ResourceAmounts[rush_cost.GetOnlyCostKey()] <= 0)
		{
			sBGUIProgressDialog.Setup(title, description, onClose);
		}
		else
		{
			sBGUIProgressDialog.Setup(title, description, onClose, false, rush_cost, onRush);
		}
		return sBGUIProgressDialog;
	}

	public static SBGUITimebar MakeAndAddTimebar(Session session, SBGUIScreen parent, uint ownerDid, string description, ulong completeTime, ulong totalTime, float duration, Cost rushCost, Action onRush, SBGUITimebar.HostPosition hPosition, Action onFinish, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
		if (sTimebarPool == null)
		{
			sTimebarPool = TFPool<SBGUITimebar>.CreatePool(10, delegate
			{
				SBGUITimebar sBGUITimebar2 = (SBGUITimebar)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/Timebar");
				sBGUITimebar2.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
				sBGUITimebar2.gameObject.SetActiveRecursively(false);
				return sBGUITimebar2;
			});
		}
		SBGUITimebar sBGUITimebar = sTimebarPool.Create(() => (SBGUITimebar)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/Timebar"));
		sBGUITimebar.gameObject.SetActiveRecursively(true);
		sBGUITimebar.Setup(session, ownerDid, description, completeTime, totalTime, duration, rushCost, onRush, hPosition, onFinish, pTaskCharacterDIDs, pTaskCharacterClicked);
		sBGUITimebar.SetParent(parent, false);
		Vector3 localPosition = hPosition();
		sBGUITimebar.transform.localPosition = localPosition;
		return sBGUITimebar;
	}

	public static void ReleaseTimebar(SBGUITimebar timebar)
	{
		timebar.MuteButtons(false);
		timebar.SetParent(null);
		timebar.gameObject.SetActiveRecursively(false);
		if (sTimebarPool != null)
		{
			sTimebarPool.Release(timebar);
		}
	}

	public static void ReleaseTimebars()
	{
		if (sTimebarPool != null)
		{
			Deactivate<SBGUITimebar> deactivateDelegate = delegate(SBGUITimebar timebar)
			{
				timebar.MuteButtons(false);
				timebar.SetParent(null);
				timebar.gameObject.SetActiveRecursively(false);
			};
			sTimebarPool.Clear(deactivateDelegate);
		}
	}

	public static SBGUINamebar MakeAndAddNamebar(Session session, SBGUIScreen parent, string name, SBGUINamebar.HostPosition hPosition, Action onFinish, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
		if (sNamebarPool == null)
		{
			sNamebarPool = TFPool<SBGUINamebar>.CreatePool(10, delegate
			{
				SBGUINamebar sBGUINamebar2 = (SBGUINamebar)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/Namebar");
				sBGUINamebar2.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
				sBGUINamebar2.gameObject.SetActiveRecursively(false);
				return sBGUINamebar2;
			});
		}
		SBGUINamebar sBGUINamebar = sNamebarPool.Create(() => (SBGUINamebar)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/Namebar"));
		sBGUINamebar.gameObject.SetActiveRecursively(true);
		sBGUINamebar.Setup(session, name, hPosition, onFinish, pTaskCharacterDIDs, pTaskCharacterClicked);
		sBGUINamebar.SetParent(parent, false);
		Vector3 vector = hPosition();
		sBGUINamebar.SetScreenPosition(vector);
		return sBGUINamebar;
	}

	public static void ReleaseNamebar(SBGUINamebar namebar)
	{
		namebar.MuteButtons(false);
		namebar.SetParent(null);
		namebar.gameObject.SetActiveRecursively(false);
		if (sNamebarPool != null)
		{
			sNamebarPool.Release(namebar);
		}
	}

	public static void ReleaseNamebars()
	{
		if (sNamebarPool != null)
		{
			Deactivate<SBGUINamebar> deactivateDelegate = delegate(SBGUINamebar namebar)
			{
				namebar.MuteButtons(false);
				namebar.SetParent(null);
				namebar.gameObject.SetActiveRecursively(false);
			};
			sNamebarPool.Clear(deactivateDelegate);
		}
	}

	public static SBGUIElement MakeAndAddInteractionStrip(Session session, uint ownerDid, SBGUIScreen parent, ICollection<IControlBinding> controls)
	{
		List<InteractionStripButtonInfo> list = new List<InteractionStripButtonInfo>();
		foreach (IControlBinding control in controls)
		{
			TFUtils.Assert(controlToTextureMap.ContainsKey(control.GetType()), string.Concat("No texture has been declared for binding type ", control.GetType(), ". Add it to the control-to-texture map."));
			InteractionStripButtonInfo item = new InteractionStripButtonInfo(controlToTextureMap[control.GetType()], control);
			list.Add(item);
		}
		SBGUIElement sBGUIElement = MakeGenericInteractionStrip(session, ownerDid, list);
		sBGUIElement.SetVisible(true);
		YGAtlasSprite component = sBGUIElement.GetComponent<YGAtlasSprite>();
		if (controls.Count < 2)
		{
			sBGUIElement.SetVisible(false);
		}
		else
		{
			component.SetSize(new Vector2(102f * (float)controls.Count, 112f));
		}
		sBGUIElement.SetParent(parent);
		sBGUIElement.transform.localPosition = new Vector3(0f, 0f, -1f);
		return sBGUIElement;
	}

	private static SBGUIElement MakeGenericInteractionStrip(Session session, uint ownerDid, List<InteractionStripButtonInfo> buttonInfos)
	{
		if (sInteractionStrip == null)
		{
			sInteractionStrip = CreateInteractionStripCache();
		}
		SBGUIElement sBGUIElement = sInteractionStrip;
		sBGUIElement.gameObject.SetActiveRecursively(true);
		SBGUIButton sBGUIButton = (SBGUIButton)sBGUIElement.FindChild("ButtonStandinBig");
		sBGUIButton.gameObject.SetActiveRecursively(false);
		sBGUIButton.SetActive(false);
		float num = 0.01f;
		float num2 = (float)sBGUIButton.Width * 0.01f + num;
		float num3 = (float)sBGUIButton.Width * 0.01f + num2 * (float)(buttonInfos.Count - 2) + num;
		num3 *= -0.5f;
		float num4 = -0.65f;
		float z = -1f;
		foreach (InteractionStripButtonInfo buttonInfo in buttonInfos)
		{
			SBGUIButton sBGUIButton2 = sInteractionStripButtons[buttonInfo.textureToUse];
			sBGUIButton2.gameObject.SetActiveRecursively(true);
			sBGUIButton2.SessionActionId = buttonInfo.control.DecorateSessionActionId(ownerDid);
			sBGUIButton2.SetParent(sBGUIElement);
			sBGUIButton2.ClickEvent += InteractionStripButtonHandlerClosure(session, buttonInfo.control.Action);
			sBGUIButton2.transform.localPosition = new Vector3(num3, num4, z);
			if (buttonInfos.Count == 1)
			{
				sBGUIButton2.transform.localPosition = new Vector3(num3, num4 + 0.3f, z);
			}
			num3 += num2;
			sBGUIButton2.ResetSize();
			buttonInfo.control.DynamicButton = sBGUIButton2;
			SBGUILabel sBGUILabel = (SBGUILabel)sBGUIButton2.FindChild("label");
			sBGUILabel.SetActive(false);
			if (buttonInfo.control.Label != null)
			{
				sBGUILabel.SetText(buttonInfo.control.Label);
				sBGUILabel.SetActive(true);
			}
		}
		return sBGUIElement;
	}

	private static Action InteractionStripButtonHandlerClosure(Session session, Action<Session> action)
	{
		return delegate
		{
			action(session);
		};
	}

	public static void UpdateAcceptPlacementButton(SBGUIButton button, Session session)
	{
		bool enabled = Session.TheDebugManager.debugPlaceObjects || session.TheGame.simulation.PlacementQuery(session.TheGame.selected) != Simulation.Placement.RESULT.INVALID;
		UpdateButton(button, enabled);
	}

	public static void UpdateButton(SBGUIButton button, bool enabled)
	{
		YGAtlasSprite component = button.GetComponent<YGAtlasSprite>();
		if (enabled)
		{
			component.SetAlpha(1f);
		}
		else
		{
			component.SetAlpha(0.25f);
		}
	}

	private static void SwapButtonTexture(SBGUIElement parent, string buttonName, string textureToUse)
	{
		SBGUIButton sBGUIButton = parent.FindChild(buttonName) as SBGUIButton;
		TFUtils.Assert(sBGUIButton != null, string.Format("Couldn't find button {0}", buttonName));
		YGAtlasSprite component = sBGUIButton.GetComponent<YGAtlasSprite>();
		component.sprite = component.LoadSpriteFromAtlas(textureToUse, component.atlasIndex);
	}

	public static void MakeActivityIndicator(SBGUIScreen parent)
	{
		SBGUIActivityIndicator sBGUIActivityIndicator = (SBGUIActivityIndicator)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/ActivityIndicator");
		sBGUIActivityIndicator.SetParent(parent);
		sBGUIActivityIndicator.transform.localPosition = new Vector3(0f, 0f, 24f);
		sBGUIActivityIndicator.InitActivityIndicator();
		sBGUIActivityIndicator.GetComponent<Renderer>().enabled = false;
	}

	public static SBGUIMarketplaceScreen MakeAndPushMarketplaceDialog(Session session, Action<SBGUIEvent, Session> guiEventHandler, Action closeClickHandler, Action<SBMarketOffer> purchaseClickHandler, EntityManager entityMgr, ResourceManager resourceMgr, SoundEffectManager sfxMgr, Catalog catalog)
	{
		bool instantiated;
		SBGUIMarketplaceScreen sBGUIMarketplaceScreen = (SBGUIMarketplaceScreen)OptionalCacheScreen("Prefabs/GUI/Screens/MarketplaceScreen", () => (SBGUIScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/MarketplaceScreen"), out instantiated);
		AndroidBack.getInstance().push(closeClickHandler, sBGUIMarketplaceScreen);
		if (instantiated)
		{
			sBGUIMarketplaceScreen.AttachActionToButton("close", delegate
			{
				closeClickHandler();
			});
			sBGUIMarketplaceScreen.OfferClickedEvent.AddListener(purchaseClickHandler);
			sBGUIMarketplaceScreen.LocalizeInitialLabel();
			sBGUIMarketplaceScreen.SetManagers(session);
			sBGUIMarketplaceScreen.session = session;
			MakeActivityIndicator(sBGUIMarketplaceScreen);
		}
		else
		{
			sBGUIMarketplaceScreen.gameObject.SetActiveRecursively(true);
			SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)sBGUIMarketplaceScreen.FindChild("info_window");
			sBGUIAtlasImage.SetActive(false);
			sBGUIMarketplaceScreen.ClearButtonActions("close");
			sBGUIMarketplaceScreen.AttachActionToButton("close", delegate
			{
				closeClickHandler();
			});
			sBGUIMarketplaceScreen.ClearButtonActions("TouchableBackground");
			sBGUIMarketplaceScreen.AttachActionToButton("TouchableBackground", closeClickHandler);
			sBGUIMarketplaceScreen.OfferClickedEvent.ClearListeners();
			sBGUIMarketplaceScreen.OfferClickedEvent.AddListener(purchaseClickHandler);
			sBGUIMarketplaceScreen.SetupTabCategories();
		}
		UpdateGuiEventHandler(session, guiEventHandler);
		sBGUIMarketplaceScreen.ViewCurrentTab();
		PushScreen(sBGUIMarketplaceScreen);
		return sBGUIMarketplaceScreen;
	}

	public static SBGUICraftingScreen MakeAndPushCraftingUI(Session session, Action<SBGUIEvent, Session> guiEventHandler, Action closeClickHandler, Action<SBGUICraftingScreen, CraftingRecipe> craftRecipeHandler, Action<int> rushCraftHandler, Action<CraftingRecipe> setSelected, CraftingCookbook cookbook, CraftingRecipe highlightedRecipe, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked, int effectiveSlotCount, int maxSlotCount)
	{
		MakeScreen make = () => (SBGUICraftingScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/CraftingScreen");
		bool instantiated;
		SBGUICraftingScreen crafting = (SBGUICraftingScreen)OptionalCacheScreen("Prefabs/GUI/Screens/CraftingScreen", make, out instantiated);
		AndroidBack.getInstance().push(closeClickHandler, crafting);
		if (instantiated)
		{
			crafting.Setup(session, cookbook, rushCraftHandler, maxSlotCount);
			crafting.AttachActionToButton("close", closeClickHandler);
			crafting.AttachActionToButton("TouchableBackground", closeClickHandler);
			CraftingScreenGraphicalSetup(crafting, cookbook);
			crafting.ReadyEvent += delegate
			{
				crafting.CreateUI(cookbook, highlightedRecipe, effectiveSlotCount, maxSlotCount, setSelected);
				session.TheGame.sessionActionManager.RequestProcess(session.TheGame);
			};
		}
		else
		{
			CraftingScreenGraphicalSetup(crafting, cookbook);
			crafting.gameObject.SetActiveRecursively(true);
			crafting.CreateUI(cookbook, highlightedRecipe, effectiveSlotCount, maxSlotCount, setSelected);
			crafting.ShowScrollRegion(true);
		}
		crafting.CreateNonScrollUI(pTaskCharacterDIDs, pTaskCharacterClicked);
		crafting.ClearButtonActions("accept_button");
		Action action = delegate
		{
			if (crafting.selectedSlot != null)
			{
				craftRecipeHandler(crafting, crafting.selectedSlot.recipe);
			}
			crafting.UpdateResources(session);
		};
		crafting.AttachActionToButton("accept_button", action);
		crafting.MakeRecipeClickedEvent.ClearListeners();
		Action<CraftingRecipe> value = delegate(CraftingRecipe recipe)
		{
			craftRecipeHandler(crafting, recipe);
		};
		crafting.MakeRecipeClickedEvent.AddListener(value);
		UpdateGuiEventHandler(session, guiEventHandler);
		PushScreen(crafting);
		return crafting;
	}

	public static SBGUIVendorScreen MakeAndPushVendorUI(Session session, Action<SBGUIEvent, Session> guiEventHandler, Action backHandler, Action<VendingInstance> vendorInstanceHandler, Action rushHandler, VendorDefinition vendorDef, Dictionary<int, VendingInstance> vendingInstances, VendingInstance specialVendingInstance, VendingDecorator vendingEntity, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
		MakeScreen make = () => (SBGUIVendorScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/VendorScreen");
		bool instantiated;
		SBGUIVendorScreen vendorScreen = (SBGUIVendorScreen)OptionalCacheScreen("Prefabs/GUI/Screens/VendorScreen", make, out instantiated);
		AndroidBack.getInstance().push(backHandler, vendorScreen);
		vendorScreen.Setup(session, vendorDef);
		if (instantiated)
		{
			vendorScreen.AttachActionToButton("close", backHandler);
			vendorScreen.AttachActionToButton("TouchableBackground", backHandler);
			vendorScreen.AttachActionToButton("skip_button", rushHandler);
			vendorScreen.UpdateVendingInstanceSlots(session);
		}
		else
		{
			vendorScreen.gameObject.SetActiveRecursively(true);
			vendorScreen.UpdateVendingInstanceSlots(session);
		}
		vendorScreen.CreateNonScrollUI(pTaskCharacterDIDs, pTaskCharacterClicked);
		Action action = delegate
		{
			if (vendorScreen.lastSelectedSlot != null)
			{
				VendingInstance vendingInstance = session.TheGame.vendingManager.GetVendingInstance(vendingEntity.Id, vendorScreen.lastSelectedSlot.SlotID);
				if (vendingInstance == null)
				{
					vendingInstance = session.TheGame.vendingManager.GetSpecialInstance(vendingEntity.Id);
				}
				vendorInstanceHandler(vendingInstance);
			}
		};
		vendorScreen.ClearButtonActions("buy_button");
		vendorScreen.AttachActionToButton("buy_button", action);
		UpdateGuiEventHandler(session, guiEventHandler);
		PushScreen(vendorScreen);
		return vendorScreen;
	}

	private static void CraftingScreenGraphicalSetup(SBGUICraftingScreen crafting, CraftingCookbook cookbook)
	{
		SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)crafting.FindChild("window");
		SBGUIAtlasImage sBGUIAtlasImage2 = (SBGUIAtlasImage)crafting.FindChild("_inset");
		List<int> backgroundColor = cookbook.backgroundColor;
		Color color;
		if (cookbook.backgroundColor != null)
		{
			color = new Color((float)backgroundColor[0] / 255f, (float)backgroundColor[1] / 255f, (float)backgroundColor[2] / 255f);
		}
		else
		{
			TFUtils.WarningLog("Cookbook " + cookbook.identity + " does not have a background.color defined");
			color = new Color(1f, 1f, 1f);
		}
		sBGUIAtlasImage.SetColor(color);
		sBGUIAtlasImage2.SetColor(color);
		SBGUIAtlasImage sBGUIAtlasImage3 = (SBGUIAtlasImage)crafting.FindChild("_title");
		if ((bool)sBGUIAtlasImage3)
		{
			sBGUIAtlasImage3.SetTextureFromAtlas(cookbook.titleTexture);
			sBGUIAtlasImage3.ResetSize();
		}
		SBGUIAtlasImage sBGUIAtlasImage4 = (SBGUIAtlasImage)crafting.FindChild("left_title_icon");
		if ((bool)sBGUIAtlasImage4)
		{
			sBGUIAtlasImage4.SetTextureFromAtlas(cookbook.titleIconTexture);
		}
		SBGUIAtlasImage sBGUIAtlasImage5 = (SBGUIAtlasImage)crafting.FindChild("right_title_icon");
		if ((bool)sBGUIAtlasImage5)
		{
			sBGUIAtlasImage5.SetTextureFromAtlas(cookbook.titleIconTexture);
		}
		SBGUIAtlasButton sBGUIAtlasButton = (SBGUIAtlasButton)crafting.FindChild("close");
		if ((bool)sBGUIAtlasButton)
		{
			sBGUIAtlasButton.SetTextureFromAtlas(cookbook.cancelButtonTexture);
		}
		SBGUILabel sBGUILabel = (SBGUILabel)crafting.FindChild("button_label");
		if ((bool)sBGUILabel)
		{
			sBGUILabel.SetText(cookbook.buttonLabel);
		}
	}

	public static SBGUICreditsScreen MakeAndPushCreditsUI(Session session, Action closeClickHandler)
	{
		MakeScreen make = () => (SBGUICreditsScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/CreditsScreen");
		bool instantiated;
		SBGUICreditsScreen credits = (SBGUICreditsScreen)OptionalCacheScreen("Prefabs/GUI/Screens/CreditsScreen", make, out instantiated);
		AndroidBack.getInstance().push(closeClickHandler, credits);
		if (instantiated)
		{
			credits.Setup(session);
			credits.AttachActionToButton("close", closeClickHandler);
			credits.AttachActionToButton("TouchableBackground", closeClickHandler);
			credits.ReadyEvent += delegate
			{
				credits.CreateUI();
			};
		}
		else
		{
			credits.gameObject.SetActiveRecursively(true);
			credits.CreateUI();
		}
		PushScreen(credits);
		return credits;
	}

	public static SBGUIDebugScreen MakeAndParentDebugUI(Session session, SBGUIScreen parent, Action closeClickHandler, Action toggleFramerateCounter, Action toggleFreeEditMode, Action saveFreeEditProgress, Action toggleHitBoxes, Action toggleFootprints, Action toggleExpansionBorders, Action addMoney, Action addJJ, Action addSpecialCurrency, Action addFoods, Action toggleRMT, Action deleteServerGame, Action resetEventItems, Action toggleFreeCameraMode, Action completeAllQuests, Action levelUp, Action logDump, Action unlockDecos, Action addHourSimulation, Action incrementDailyBonus, Action fastFoward, Action addOneLevel, Action reset_device_id)
	{
		SBGUIDebugScreen sBGUIDebugScreen = (SBGUIDebugScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/DebugScreen");
		AndroidBack.getInstance().push(closeClickHandler, sBGUIDebugScreen);
		sBGUIDebugScreen.SetParent(parent);
		sBGUIDebugScreen.transform.localPosition = Vector3.zero;
		sBGUIDebugScreen.AttachActionToButton("close", closeClickHandler);
		sBGUIDebugScreen.AttachActionToButton("TouchableBackground", closeClickHandler);
		sBGUIDebugScreen.AttachActionToButton("framerate_counter", toggleFramerateCounter);
		sBGUIDebugScreen.AttachActionToButton("free_edit_button", toggleFreeEditMode);
		sBGUIDebugScreen.AttachActionToButton("save_free_edit_button", saveFreeEditProgress);
		sBGUIDebugScreen.AttachActionToButton("toggle_hit_boxes_button", toggleHitBoxes);
		sBGUIDebugScreen.AttachActionToButton("toggle_footprints_button", toggleFootprints);
		sBGUIDebugScreen.AttachActionToButton("toggle_expansion_borders_button", toggleExpansionBorders);
		sBGUIDebugScreen.AttachActionToButton("add_money_button", addMoney);
		sBGUIDebugScreen.AttachActionToButton("add_JJ_button", addJJ);
		sBGUIDebugScreen.AttachActionToButton("add_special_currency_button", addSpecialCurrency);
		sBGUIDebugScreen.AttachActionToButton("add_foods_button", addFoods);
		sBGUIDebugScreen.AttachActionToButton("toggle_rmt_button", toggleRMT);
		sBGUIDebugScreen.AttachActionToButton("delete_server_game_button", deleteServerGame);
		sBGUIDebugScreen.AttachActionToButton("reset_community_event_items_button", resetEventItems);
		sBGUIDebugScreen.AttachActionToButton("toggle_free_camera", toggleFreeCameraMode);
		sBGUIDebugScreen.AttachActionToButton("complete_all_quests", completeAllQuests);
		sBGUIDebugScreen.AttachActionToButton("level_up", levelUp);
		sBGUIDebugScreen.AttachActionToButton("log_dump", logDump);
		sBGUIDebugScreen.AttachActionToButton("unlock_decos", unlockDecos);
		sBGUIDebugScreen.AttachActionToButton("add_hour_sim_button", addHourSimulation);
		sBGUIDebugScreen.AttachActionToButton("increment_daily_bonus", incrementDailyBonus);
		sBGUIDebugScreen.AttachActionToButton("fast_forward", fastFoward);
		sBGUIDebugScreen.AttachActionToButton("add_level_button", addOneLevel);
		sBGUIDebugScreen.AttachActionToButton("reset_device_id", reset_device_id);
		sBGUIDebugScreen.Setup(session);
		return sBGUIDebugScreen;
	}

	public static SBGUIClearingScreen MakeAndPushClearingUI(string cost, Action okButtonHandler, Action cancelButtonHandler)
	{
		SBGUIClearingScreen sBGUIClearingScreen = (SBGUIClearingScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/ClearingScreen");
		sBGUIClearingScreen.AttachActionToButton("cancel_button", cancelButtonHandler);
		sBGUIClearingScreen.AttachActionToButton("okay_button", okButtonHandler);
		sBGUIClearingScreen.AttachActionToButton("TouchableBackground", cancelButtonHandler);
		sBGUIClearingScreen.SetUp("Clear Debris?", "Removing this item costs:", cost);
		PushScreen(sBGUIClearingScreen);
		return sBGUIClearingScreen;
	}

	public static SBGUIInventoryScreen MakeAndPushInventoryDialog(Session session, EntityManager entityMgr, SoundEffectManager sfxMgr, Action closeClickHandler, Action<SBInventoryItem> buildingClickHandler, Action<SBInventoryItem> inventoryClickHandler)
	{
		MakeScreen make = () => (SBGUIInventoryScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/InventoryScreen");
		bool instantiated;
		SBGUIInventoryScreen sBGUIInventoryScreen = (SBGUIInventoryScreen)OptionalCacheScreen("Prefabs/GUI/Screens/InventoryScreen", make, out instantiated);
		AndroidBack.getInstance().push(closeClickHandler, sBGUIInventoryScreen);
		if (instantiated)
		{
			sBGUIInventoryScreen.AttachActionToButton("close", closeClickHandler);
			sBGUIInventoryScreen.AttachActionToButton("TouchableBackground", closeClickHandler);
			sBGUIInventoryScreen.BuildingSlotClickedEvent.AddListener(buildingClickHandler);
			sBGUIInventoryScreen.MovieSlotClickedEvent.AddListener(inventoryClickHandler);
			sBGUIInventoryScreen.SetManagers(session);
			MakeActivityIndicator(sBGUIInventoryScreen);
		}
		else
		{
			sBGUIInventoryScreen.gameObject.SetActiveRecursively(true);
		}
		sBGUIInventoryScreen.ViewCurrentTab();
		PushScreen(sBGUIInventoryScreen);
		return sBGUIInventoryScreen;
	}

	public static SBGUICommunityEventScreen MakeAndPushCommunityEventDialog(Session session, Action closeClickHandler, Action<CommunityEvent, SoaringCommunityEvent, SoaringCommunityEvent.Reward> purchaseHandler)
	{
		MakeScreen make = () => (SBGUICommunityEventScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/CommunityEventScreen");
		bool instantiated;
		SBGUICommunityEventScreen sBGUICommunityEventScreen = (SBGUICommunityEventScreen)OptionalCacheScreen("Prefabs/GUI/Screens/CommunityEventScreen", make, out instantiated);
		AndroidBack.getInstance().push(closeClickHandler, sBGUICommunityEventScreen);
		if (instantiated)
		{
			sBGUICommunityEventScreen.AttachActionToButton("close", closeClickHandler);
			sBGUICommunityEventScreen.BuyButtonClickedEvent.AddListener(purchaseHandler);
			sBGUICommunityEventScreen.SetManagers(session);
			sBGUICommunityEventScreen.SetupButtons();
		}
		else
		{
			sBGUICommunityEventScreen.gameObject.SetActiveRecursively(true);
			sBGUICommunityEventScreen.BuyButtonClickedEvent.ClearListeners();
			sBGUICommunityEventScreen.BuyButtonClickedEvent.AddListener(purchaseHandler);
		}
		sBGUICommunityEventScreen.ViewCurrentTab();
		PushScreen(sBGUICommunityEventScreen);
		return sBGUICommunityEventScreen;
	}

	public static SBGUILevelUpScreen MakeAndAddLevelUpDialog(SBGUIScreen parent, Session session, LevelUpDialogInputData inputData)
	{
		SBGUILevelUpScreen sBGUILevelUpScreen = (SBGUILevelUpScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/LevelUpScreen");
		session.AddAsyncResponse("levelup_screen", sBGUILevelUpScreen);
		sBGUILevelUpScreen.SetParent(parent);
		sBGUILevelUpScreen.transform.localPosition = Vector3.zero;
		sBGUILevelUpScreen.SetManagers(session.TheGame.entities, session.TheGame.resourceManager, session.TheSoundEffectManager);
		sBGUILevelUpScreen.Setup(session, inputData);
		sBGUILevelUpScreen.session = session;
		sBGUILevelUpScreen.CreateUI(session, inputData);
		session.PlaySeaflowerAndBubbleScreenSwipeEffect();
		return sBGUILevelUpScreen;
	}

	public static SBGUIFoundItemScreen MakeAndAddFoundItemScreen(Session session, SBGUIScreen parent)
	{
		SBGUIFoundItemScreen sBGUIFoundItemScreen = (SBGUIFoundItemScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/FoundItemScreen");
		sBGUIFoundItemScreen.SetParent(parent);
		sBGUIFoundItemScreen.transform.localPosition = Vector3.zero;
		session.PlayBubbleScreenSwipeEffect();
		return sBGUIFoundItemScreen;
	}

	public static SBGUIExplanationDialog MakeAndAddExplanationDialog(SBGUIScreen parent)
	{
		SBGUIExplanationDialog sBGUIExplanationDialog = (SBGUIExplanationDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/ExplanationDialog");
		sBGUIExplanationDialog.SetParent(parent);
		sBGUIExplanationDialog.transform.localPosition = Vector3.zero;
		return sBGUIExplanationDialog;
	}

	public static SBGUIMoveInDialog MakeAndAddMoveInDialog(SBGUIScreen parent)
	{
		SBGUIMoveInDialog sBGUIMoveInDialog = (SBGUIMoveInDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/MoveInDialog");
		sBGUIMoveInDialog.SetParent(parent);
		sBGUIMoveInDialog.transform.localPosition = Vector3.zero;
		return sBGUIMoveInDialog;
	}

	public static SBGUIDailyBonusDialog MakeAndAddDailyBonusDialog(SBGUIScreen parent)
	{
		SBGUIDailyBonusDialog sBGUIDailyBonusDialog = (SBGUIDailyBonusDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/DailyBonusDialog");
		sBGUIDailyBonusDialog.SetParent(parent);
		sBGUIDailyBonusDialog.transform.localPosition = Vector3.zero;
		return sBGUIDailyBonusDialog;
	}

	public static SBGUISpongyGamesDialog MakeAndAddSpongyGamesDialog(SBGUIScreen parent)
	{
		SBGUISpongyGamesDialog sBGUISpongyGamesDialog = (SBGUISpongyGamesDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/SpongyGamesDialog");
		sBGUISpongyGamesDialog.SetParent(parent);
		sBGUISpongyGamesDialog.transform.localPosition = Vector3.zero;
		return sBGUISpongyGamesDialog;
	}

	public static SBGUIScreen MakeAndPushAgeGateDialog(Action backHandler, Action submitHandler, Action cancelHandler, Action inputHandler)
	{
		MakeScreen make = () => (SBGUIScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/AgeGateScreen");
		bool instantiated;
		SBGUIScreen sBGUIScreen = OptionalCacheScreen("Prefabs/GUI/Screens/AgeGateScreen", make, out instantiated);
		AndroidBack.getInstance().push(backHandler, sBGUIScreen);
		if (instantiated)
		{
			sBGUIScreen.AttachActionToButton("close", backHandler);
			sBGUIScreen.AttachActionToButton("submit", submitHandler);
			sBGUIScreen.AttachActionToButton("cancel", cancelHandler);
			sBGUIScreen.AttachActionToButton("input_box", inputHandler);
			sBGUIScreen.AttachActionToButton("TouchableBackground", backHandler);
		}
		else
		{
			sBGUIScreen.gameObject.SetActiveRecursively(true);
		}
		PushScreen(sBGUIScreen);
		return sBGUIScreen;
	}

	public static SBGUIScreen MakeAndPushOptionsDialog(Action backHandler, Action moreNickHandler, Action toggleSFXHandler, Action toggleMusicHandler, Action achievementsHandler, Action creditsHandler, Action privacyHandler, Action eulaHandler, Action debugHandler, Action parentsHandler)
	{
		MakeScreen make = () => (SBGUIScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/OptionsScreen");
		bool instantiated;
		SBGUIScreen sBGUIScreen = OptionalCacheScreen("Prefabs/GUI/Screens/OptionsScreen", make, out instantiated);
		AndroidBack.getInstance().push(backHandler, sBGUIScreen);
		if (instantiated)
		{
			sBGUIScreen.AttachActionToButton("close", backHandler);
			sBGUIScreen.AttachActionToButton("more_nick", moreNickHandler);
			sBGUIScreen.AttachActionToButton("toggle_sfx", toggleSFXHandler);
			sBGUIScreen.AttachActionToButton("toggle_music", toggleMusicHandler);
			sBGUIScreen.AttachActionToButton("credits", creditsHandler);
			sBGUIScreen.AttachActionToButton("privacy_policy", privacyHandler);
			sBGUIScreen.AttachActionToButton("eula", eulaHandler);
			sBGUIScreen.AttachActionToButton("parents", parentsHandler);
			sBGUIScreen.AttachActionToButton("TouchableBackground", backHandler);
			if (SBSettings.ShowDebug)
			{
				sBGUIScreen.AttachActionToButton("debug", debugHandler);
			}
			else
			{
				SBGUIButton sBGUIButton = (SBGUIButton)sBGUIScreen.FindChild("debug");
				UnityEngine.Object.Destroy(sBGUIButton.gameObject);
			}
		}
		else
		{
			sBGUIScreen.gameObject.SetActiveRecursively(true);
		}
		bool flag = ((PlayerPrefs.GetInt(MusicManager.MUSIC_ENABLED) != 0) ? true : false);
		GameObject gameObject = GameObject.Find("toggle_music");
		ToggleButton component = gameObject.GetComponent<ToggleButton>();
		if (flag)
		{
			component.TurnOn();
		}
		else
		{
			component.TurnOff();
		}
		flag = ((PlayerPrefs.GetInt(SoundEffectManager.SOUND_ENABLED) != 0) ? true : false);
		gameObject = GameObject.Find("toggle_sfx");
		component = gameObject.GetComponent<ToggleButton>();
		if (flag)
		{
			component.TurnOn();
		}
		else
		{
			component.TurnOff();
		}
		PushScreen(sBGUIScreen);
		return sBGUIScreen;
	}

	private static string GetResourcePrefix(int resourceId)
	{
		return string.Empty;
	}

	private static void UpdateStandardUI(SBGUIScreen screen, Session session)
	{
		TFUtils.Assert(session != null, "No session given to UpdateStandardUI!");
		foreach (string item in (List<string>)screen.dynamicProperties["TrackingResourceAmounts"])
		{
			int resourceId = int.Parse(item);
			int num = session.TheGame.resourceManager.Query(resourceId);
			screen.dynamicLabels[item].Text = GetResourcePrefix(resourceId) + num;
		}
		foreach (string item2 in (List<string>)screen.dynamicProperties["TrackingResourcePercentages"])
		{
			IResourceProgressCalculator resourceCalculator = session.TheGame.resourceCalculatorManager.GetResourceCalculator(int.Parse(item2));
			float num2 = session.TheGame.resourceManager.QueryProgressPercentage(resourceCalculator);
			screen.dynamicMeters[item2].Progress = num2 / 100f;
		}
		SBGUILabel sBGUILabel = screen.dynamicLabels["amount_xp_label"];
		sBGUILabel.Text = session.TheGame.resourceManager.QueryProgressFraction(session.TheGame.levelingManager);
		UpdateQuestCounter(screen, session);
	}

	private static SBGUIElement CreateInteractionStripCache()
	{
		SBGUIElement sBGUIElement = SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/SimulatedInteractionStrip");
		sBGUIElement.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
		sBGUIElement.gameObject.SetActiveRecursively(false);
		SBGUIButton original = (SBGUIButton)sBGUIElement.FindChild("ButtonStandinBig");
		foreach (string value in controlToTextureMap.Values)
		{
			SBGUIButton sBGUIButton = (SBGUIButton)UnityEngine.Object.Instantiate(original);
			YGAtlasSprite component = sBGUIButton.GetComponent<YGAtlasSprite>();
			component.sprite = component.LoadSpriteFromAtlas(value, component.atlasIndex);
			sBGUIButton.gameObject.SetActiveRecursively(false);
			sBGUIButton.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
			sInteractionStripButtons[value] = sBGUIButton;
		}
		return sBGUIElement;
	}

	public static void ReleaseInteractionStrip(SBGUIElement strip)
	{
		strip.MuteButtons(false);
		foreach (SBGUIButton value in sInteractionStripButtons.Values)
		{
			value.ClearClickEvents();
			value.SetParent(null);
			value.gameObject.SetActiveRecursively(false);
		}
		strip.SetParent(null);
		strip.gameObject.SetActiveRecursively(false);
	}

	public static void CreateErrorDialog(Session session, string title, string message, string okButtonLabel, Action okHandler, float messageScale, float titleScale)
	{
		CreateErrorDialog(session, title, message, okButtonLabel, okHandler, null, null, messageScale, titleScale);
	}

	public static void CreateErrorDialog(Session session, string title, string message, string okButtonLabel, Action okHandler, string cancelButtonLabel, Action cancelHandler, float messageScale, float titleScale)
	{
		Action androidOk = delegate
		{
			okHandler();
		};
		Action okButtonHandler = delegate
		{
			okHandler();
			AndroidBack.getInstance().pop(androidOk);
		};
		Action cancelButtonHandler = delegate
		{
			cancelHandler();
			AndroidBack.getInstance().pop(androidOk);
		};
		if (cancelHandler == null || cancelButtonLabel == null)
		{
			cancelButtonHandler = null;
			cancelButtonLabel = null;
		}
		SBGUIConfirmationDialog sBGUIConfirmationDialog = MakeAndPushConfirmationDialog(session, null, title, message, okButtonLabel, cancelButtonLabel, null, okButtonHandler, cancelButtonHandler);
		AndroidBack.getInstance().push(androidOk, sBGUIConfirmationDialog);
		SBGUILabel sBGUILabel = (SBGUILabel)sBGUIConfirmationDialog.FindChild("message_label");
		YGTextAtlasSprite component = sBGUILabel.GetComponent<YGTextAtlasSprite>();
		component.scale = new Vector2(messageScale, messageScale);
		SBGUILabel sBGUILabel2 = (SBGUILabel)sBGUIConfirmationDialog.FindChild("title_label");
		YGTextAtlasSprite component2 = sBGUILabel2.GetComponent<YGTextAtlasSprite>();
		component2.scale = new Vector2(titleScale, titleScale);
		sBGUIConfirmationDialog.tform.parent = GUIMainView.GetInstance().transform;
		sBGUIConfirmationDialog.tform.localPosition = Vector3.zero;
	}
}
