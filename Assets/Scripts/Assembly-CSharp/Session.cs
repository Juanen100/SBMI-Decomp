#define ASSERTS_ON
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Helpshift;
using MTools;
using MiniJSON;
using UnityEngine;
using Yarg;

public class Session
{
	public class SoaringSessionRestartDelegate : SoaringDelegate
	{
		public override void OnRequestingSessionData(bool success, SoaringError error, SoaringArray sessions, SoaringDictionary raw_data, SoaringContext context)
		{
			if (success && error == null && sessions != null)
			{
			}
		}
	}

	public class FramerateWatcher
	{
		public float frequency = 0.5f;

		private float accum;

		private int frames;

		private float waitTime;

		private float prevWindowsFPS;

		public float Framerate
		{
			get
			{
				return prevWindowsFPS;
			}
		}

		public void OnUpdate()
		{
			accum += Time.timeScale / Time.deltaTime;
			frames++;
			waitTime += Time.deltaTime;
			if (waitTime > frequency)
			{
				waitTime = 0f;
				prevWindowsFPS = accum / (float)frames;
				accum = 0f;
				frames = 0;
			}
		}
	}

	private class SessionProperties
	{
		public class DraggedGood
		{
			public int productId;

			public Resource resource;

			public DraggedGood(int productId, Resource resource)
			{
				this.productId = productId;
				this.resource = resource;
			}
		}

		public SBGUIStandardScreen playingHud;

		public SBGUIStandardScreen ageGateHud;

		public bool transitionSilently;

		public SBGUIStandardScreen recipesHud;

		public SBGUICraftingScreen recipesWindow;

		public Dictionary<CraftingCookbook, CraftingRecipe> lastSelectedRecipe = new Dictionary<CraftingCookbook, CraftingRecipe>();

		public Simulated m_pTaskSimulated;

		public bool m_bAutoPanToSimulatedOnLeave;

		public SBGUIStandardScreen communityEventHud;

		public SBGUICommunityEventScreen communityEventScreen;

		public SBGUIStandardScreen dragFeedHud;

		public DraggedGood draggedGood;

		public Simulated candidateSimulated;

		public YGEvent carriedUiEvent;

		public int playDelayCounter;

		public SBGUIMicroConfirmDialog microConfirmDialog;

		public Action denialActions;

		public Action cleanUp;

		public HardSpendActions hardSpendActions;

		public Simulated overrideSimulatedToRush;

		public string iapBundleName;

		public SBGUIInsufficientResourcesDialog insufficientDialog;

		public SBGUIStandardScreen inventoryHud;

		public SBGUIScreen editingHud;

		public bool waitToDecidePlacement;

		public Vector2 preMovePosition;

		public bool preMoveFlip;

		public bool preMovePositionSet;

		public bool isInteractionStripActive;

		public bool isDraggingBuilding;

		public bool isDraggingBuildingAndScreen;

		public bool firstEntered;

		public bool startedTouchOnEmptySpace;

		public SBGUIStandardScreen optionsHud;

		public bool cameFromMarketplace;

		public Simulated touchingSim;

		public Simulated queuedClickedSim;

		public Vector2 moveDragStart;

		public IDisplayController tappedDisplayController;

		public SBGUIStandardScreen shoppingHud;

		public string marketplaceSessionActionID;

		public string m_sLeaveType;

		public bool reducedBuffer;

		public int storeVisitSinceLastPurchase;

		public SBGUIStandardScreen dialogHud;

		public SBGUIStandardScreen unitBusyHud;

		public SBGUICharacterBusyScreen unitBusyWindow;

		public Task unitBusyTask;

		public SBGUIStandardScreen unitIdleHud;

		public SBGUICharacterIdleScreen unitIdleWindow;

		public SBGUIVendorScreen vendorScreen;

		public Reward reward;
	}

	private struct StateChangeRequest
	{
		public string state;

		public bool changeContext;
	}

	private class BubbleSwipeParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		protected Session session;

		protected Vector3 viewportPosition;

		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		public Vector3 Position
		{
			get
			{
				return session.camera.UnityCamera.ViewportToWorldPoint(viewportPosition);
			}
		}

		public bool isVisible
		{
			get
			{
				return true;
			}
		}

		public BubbleSwipeParticleSystemRequestDelegate(Session s)
		{
			session = s;
			viewportPosition = new Vector3(0.5f, 0f, session.camera.UnityCamera.nearClipPlane);
		}
	}

	private class ConfettiSwipeParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		protected Session session;

		protected Vector3 viewportPosition;

		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		public Vector3 Position
		{
			get
			{
				return session.camera.UnityCamera.ViewportToWorldPoint(viewportPosition);
			}
		}

		public bool isVisible
		{
			get
			{
				return true;
			}
		}

		public ConfettiSwipeParticleSystemRequestDelegate(Session s)
		{
			session = s;
			viewportPosition = new Vector3(0.5f, 1.25f, session.camera.UnityCamera.nearClipPlane);
		}
	}

	private class BalloonSwipeParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		protected Session session;

		protected Vector3 viewportPosition;

		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		public Vector3 Position
		{
			get
			{
				return session.camera.UnityCamera.ViewportToWorldPoint(viewportPosition);
			}
		}

		public bool isVisible
		{
			get
			{
				return true;
			}
		}

		public BalloonSwipeParticleSystemRequestDelegate(Session s)
		{
			session = s;
			viewportPosition = new Vector3(0.5f, -0.25f, session.camera.UnityCamera.nearClipPlane + 25f);
		}
	}

	private class SeaflowerSwipeParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		protected Session session;

		protected Vector3 viewportPosition;

		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		public Vector3 Position
		{
			get
			{
				return session.camera.UnityCamera.ViewportToWorldPoint(viewportPosition);
			}
		}

		public bool isVisible
		{
			get
			{
				return true;
			}
		}

		public SeaflowerSwipeParticleSystemRequestDelegate(Session s)
		{
			session = s;
			viewportPosition = new Vector3(0.15f, -0.25f, session.camera.UnityCamera.nearClipPlane + 25f);
		}
	}

	private class FogEffectRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		protected Session session;

		protected Vector3 position;

		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		public Vector3 Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
			}
		}

		public bool isVisible
		{
			get
			{
				return true;
			}
		}

		public FogEffectRequestDelegate(Session s)
		{
			position = new Vector3(0f, 0f, 100f);
			session = s;
		}
	}

	private class TapFXParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		protected Session session;

		protected Vector3 position;

		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		public Vector3 Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
			}
		}

		public bool isVisible
		{
			get
			{
				return true;
			}
		}

		public TapFXParticleSystemRequestDelegate(Session s)
		{
			session = s;
		}
	}

	public class InteractionStripMixin
	{
		private const string INTERACTION_STRIP = "InteractionStrip";

		private const string INTERACTION_CONTROLS = "InteractionControls";

		public const string ACCEPT_CALLBACK = "InteractionStrip_AcceptCallback";

		public const string REJECT_CALLBACK = "InteractionStrip_RejectCallback";

		public Vector3 StripPosition { get; set; }

		public void ActivateOnSelected(Session session)
		{
			Simulated selected = session.game.selected;
			TFUtils.Assert(selected != null, "Cannot enable interaction strip unless there is a selected simulated");
			TFUtils.Assert(selected.InteractionState.Controls != null && selected.InteractionState.Controls.Count > 0, "Trying to activate interation strip on a simulated that has no control bindings.Sim=" + selected.ToString());
			ICollection<IControlBinding> controls = selected.InteractionState.Controls;
			SBGUIElement val = SBUIBuilder.MakeAndAddInteractionStrip(session, (uint)selected.entity.DefinitionId, session.SimulationSBGUIScreen, controls);
			session.AddAsyncResponse("InteractionStrip", val);
			session.AddAsyncResponse("InteractionControls", controls);
			MoveSubUiWithSelected(session);
		}

		public void Deactivate(Session session)
		{
			session.CheckAsyncRequest("InteractionStrip_AcceptCallback");
			session.CheckAsyncRequest("InteractionStrip_RejectCallback");
			session.CheckAsyncRequest("InteractionControls");
			SBGUIElement sBGUIElement = (SBGUIElement)session.CheckAsyncRequest("InteractionStrip");
			if (sBGUIElement != null)
			{
				SBUIBuilder.ReleaseInteractionStrip(sBGUIElement);
			}
		}

		public void EnableRejectButton(Session session, bool enable)
		{
			SBGUIElement sBGUIElement = (SBGUIElement)session.CheckAsyncRequest("InteractionStrip");
			if (sBGUIElement != null)
			{
				sBGUIElement.EnableRejectButton(enable);
				session.AddAsyncResponse("InteractionStrip", sBGUIElement);
			}
		}

		public void EnableButtons(Session session, bool enable)
		{
			SBGUIElement sBGUIElement = (SBGUIElement)session.CheckAsyncRequest("InteractionStrip");
			if (sBGUIElement != null)
			{
				sBGUIElement.EnableButtons(enable);
				session.AddAsyncResponse("InteractionStrip", sBGUIElement);
			}
		}

		public void OnUpdate(Session session)
		{
			MoveSubUiWithSelected(session);
			List<IControlBinding> list = (List<IControlBinding>)session.CheckAsyncRequest("InteractionControls");
			if (list != null)
			{
				list.ForEach(delegate(IControlBinding control)
				{
					control.DynamicUpdate(session);
				});
				session.AddAsyncResponse("InteractionControls", list);
			}
		}

		public void SetAcceptHandler(Session session, Action<Session> handler)
		{
			session.CheckAsyncRequest("InteractionStrip_AcceptCallback");
			session.AddAsyncResponse("InteractionStrip_AcceptCallback", handler);
		}

		public void SetRejectHandler(Session session, Action<Session> handler)
		{
			session.CheckAsyncRequest("InteractionStrip_RejectCallback");
			session.AddAsyncResponse("InteractionStrip_RejectCallback", handler);
		}

		public bool FindTutorialPointer(Session session)
		{
			SBGUIElement sBGUIElement = (SBGUIElement)session.CheckAsyncRequest("InteractionStrip");
			if (null == sBGUIElement)
			{
				return false;
			}
			session.AddAsyncResponse("InteractionStrip", sBGUIElement);
			SBGUIElement[] componentsInChildren = sBGUIElement.GetComponentsInChildren<SBGUIElement>();
			foreach (SBGUIElement sBGUIElement2 in componentsInChildren)
			{
				if (sBGUIElement2.name.Contains("TutorialPointer"))
				{
					return true;
				}
			}
			return false;
		}

		private void MoveSubUiWithSelected(Session session)
		{
			SBGUIElement sBGUIElement = (SBGUIElement)session.CheckAsyncRequest("InteractionStrip");
			if (!(null == sBGUIElement))
			{
				if (session.game.selected == null)
				{
					session.AddAsyncResponse("InteractionStrip", sBGUIElement);
					Deactivate(session);
					return;
				}
				Vector2 position = session.game.selected.Position;
				Vector2 screenPosition = session.TheCamera.WorldPointToScreenPoint(new Vector3(position.x, position.y));
				sBGUIElement.SetScreenPosition(screenPosition);
				StripPosition = position;
				sBGUIElement.GUIUpdate();
				session.AddAsyncResponse("InteractionStrip", sBGUIElement);
			}
		}
	}

	public class NamebarMixin
	{
		public const int YOFFSET = 20;

		public const int HEIGHT = 100;

		private const string _sNAMEBAR = "Namebar";

		private SBGUINamebar m_pNamebarGUI;

		private string m_sGameObjectID;

		public bool IsActive
		{
			get
			{
				return m_pNamebarGUI != null && m_pNamebarGUI.IsActive();
			}
		}

		public bool ActivateOnSelected(Session pSession, Simulated pSimulated, float fYOffset = 20f)
		{
			bool result = false;
			Simulated selected = pSession.game.selected;
			TFUtils.Assert(pSimulated != null, "Cannot enable Namebar unless there is a simulated");
			if (pSimulated.m_pNamebarMixinArgs.m_bHasNamebar)
			{
				SBGUINamebar.HostPosition hPosition = () => pSession.game.simulation.ScreenPositionFromWorldPosition(selected.Position);
				Action onFinish = delegate
				{
					pSession.ChangeState("Playing");
					pSession.game.selected = null;
				};
				List<int> list = null;
				Action<int> pTaskCharacterClicked = null;
				if (pSimulated.m_pNamebarMixinArgs.m_bCheckForTaskCharacters)
				{
					list = pSession.TheGame.taskManager.GetActiveSourcesForTarget(pSimulated.Id);
					int num = list.Count;
					for (int num2 = 0; num2 < num; num2++)
					{
						List<Task> activeTasksForSimulated = pSession.TheGame.taskManager.GetActiveTasksForSimulated(list[num2], null);
						if (activeTasksForSimulated.Count > 0 && activeTasksForSimulated[0].GetTimeLeft() == 0)
						{
							list.RemoveAt(num2);
							num2--;
							num--;
						}
					}
					if (list != null && list.Count > 0)
					{
						pTaskCharacterClicked = delegate(int nDID)
						{
							pSession.CheckAsyncRequest(SelectedPlaying.TASK_CHARACTER_SELECT);
							pSession.AddAsyncResponse(SelectedPlaying.TASK_CHARACTER_SELECT, nDID, false);
						};
					}
				}
				SBGUINamebar sBGUINamebar = null;
				sBGUINamebar = SBUIBuilder.MakeAndAddNamebar(pSession, pSession.SimulationSBGUIScreen, pSimulated.m_pNamebarMixinArgs.m_sName, hPosition, onFinish, list, pTaskCharacterClicked);
				m_sGameObjectID = sBGUINamebar.gameObject.name;
				m_pNamebarGUI = sBGUINamebar;
				result = true;
			}
			return result;
		}

		public void Deactivate(Session pSession)
		{
			if (m_sGameObjectID != null && m_pNamebarGUI != null)
			{
				m_pNamebarGUI.Close();
			}
		}

		public void Extend()
		{
			if (m_pNamebarGUI != null)
			{
				m_pNamebarGUI.elapsed = 0f;
			}
		}
	}

	public class NamebarGroup
	{
		public const string TASK_SRC_UNIT = "TaskSrcUnit";

		private NamebarMixin m_pTaskAtBuildingNamebar = new NamebarMixin();

		private NamebarMixin m_pNamebar = new NamebarMixin();

		public bool IsActive
		{
			get
			{
				return m_pNamebar != null && m_pNamebar.IsActive;
			}
		}

		public void ActivateOnSelected(Session pSession)
		{
			if (pSession.game.selected.Variable.ContainsKey("TaskSrcUnit"))
			{
				m_pTaskAtBuildingNamebar.ActivateOnSelected(pSession, (Simulated)pSession.game.selected.Variable["TaskSrcUnit"], 120f);
			}
			m_pNamebar.ActivateOnSelected(pSession, pSession.game.selected, 20f);
		}

		public void Deactivate(Session pSession)
		{
			m_pNamebar.Deactivate(pSession);
			if (pSession.game.selected != null && pSession.game.selected.Variable.ContainsKey("TaskSrcUnit"))
			{
				m_pTaskAtBuildingNamebar.Deactivate(pSession);
			}
		}

		public void Extend()
		{
			m_pNamebar.Extend();
		}
	}

	public abstract class Prioritizer
	{
		protected Simulated best;

		protected Camera camera;

		public Simulated Best
		{
			get
			{
				return best;
			}
		}

		public Prioritizer(Camera camera)
		{
			this.camera = camera;
		}

		public void SelectBest(Simulated simulated)
		{
			if (best == null)
			{
				best = simulated;
			}
			else if (Compare(simulated, best) < 0)
			{
				best = simulated;
			}
		}

		public float distanceToCamera(Simulated simulated, Camera camera)
		{
			return (camera.transform.position - new Vector3(simulated.Position.x, simulated.Position.y, 0f)).sqrMagnitude;
		}

		protected int CompareByDistanceToCamera(Simulated a, Simulated b)
		{
			float num = distanceToCamera(a, camera);
			float num2 = distanceToCamera(b, camera);
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return 0;
		}

		protected abstract int Compare(Simulated a, Simulated b);
	}

	public class SelectionPrioritizer : Prioritizer
	{
		public SelectionPrioritizer(Camera camera)
			: base(camera)
		{
		}

		protected override int Compare(Simulated a, Simulated b)
		{
			int selectionPriority = a.SelectionPriority;
			int selectionPriority2 = b.SelectionPriority;
			if (selectionPriority < selectionPriority2)
			{
				return -1;
			}
			if (selectionPriority > selectionPriority2)
			{
				return 1;
			}
			return CompareByDistanceToCamera(a, b);
		}
	}

	public class TemptationPrioritizer : Prioritizer
	{
		public TemptationPrioritizer(Camera camera)
			: base(camera)
		{
		}

		protected override int Compare(Simulated a, Simulated b)
		{
			if (a.TemptationPriority < b.TemptationPriority)
			{
				return -1;
			}
			if (a.TemptationPriority > b.TemptationPriority)
			{
				return 1;
			}
			return CompareByDistanceToCamera(a, b);
		}
	}

	public class AgeGate : State
	{
		private int number1;

		private int number2;

		private int correctAnswer;

		private string inputString = string.Empty;

		private SBGUIScreen ageGate;

		private SBGUILabel equationLabel;

		private SBGUILabel invalidAnswer;

		private SBGUILabel inputLabel;

		private TouchScreenKeyboard keyboard;

		private SBGUIButton closeButton;

		public void OnEnter(Session session)
		{
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			session.PlaySeaflowerAndBubbleScreenSwipeEffect();
			session.camera.SetEnableUserInput(false);
			string deviceLanguage = Language.getDeviceLanguage();
			HelpshiftSdk.getInstance().setNameAndEmail(Soaring.Player.UserTag, Soaring.Player.UserTag + "@example.com");
			HelpshiftSdk.getInstance().setUserIdentifier(Soaring.Player.UserTag);
			Dictionary<string, object> configMap = new Dictionary<string, object>();
			configMap.Add("enableContactUs", "always");
			if (!string.IsNullOrEmpty(deviceLanguage) && !deviceLanguage.Contains("EN") && !deviceLanguage.Contains("en"))
			{
				configMap["enableContactUs"] = "never";
			}
			configMap.Add("gotoConversationAfterContactUs", "no");
			configMap.Add("showConversationResolutionQuestion", "yes");
			configMap.Add("requireEmail", false);
			configMap.Add("hideNameAndEmail", true);
			configMap.Add("enableFullPrivacy", "yes");
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("playerLevel", session.TheGame.resourceManager.Query(ResourceManager.LEVEL).ToString());
			dictionary.Add("playerCoins", session.TheGame.resourceManager.Query(ResourceManager.SOFT_CURRENCY).ToString());
			dictionary.Add("playerJelly", session.TheGame.resourceManager.Query(ResourceManager.HARD_CURRENCY).ToString());
			dictionary.Add("playerXP", session.TheGame.resourceManager.Query(ResourceManager.XP).ToString());
			dictionary.Add("gameCenterLoggedIn", "false");
			dictionary.Add("playerLanguageCountry", deviceLanguage);
			if (!session.InFriendsGame)
			{
				List<string> list = new List<string>();
				session.TheGame.store.GetPurchases(session);
				int totalMoneySpent = SoaringRetrievePurchasesModule.TotalMoneySpent;
				dictionary.Add("money spent", totalMoneySpent.ToString());
				if (totalMoneySpent > 9)
				{
					list.Add("$10");
					if (totalMoneySpent > 99)
					{
						list.Add("whale");
					}
				}
				else if (totalMoneySpent > 0)
				{
					list.Add("lessthan$10");
				}
				else
				{
					list.Add("nomoneyspent");
				}
				if (list.Count > 0)
				{
					string[] value = list.ToArray();
					dictionary.Add("hs-tags", value);
				}
			}
			HelpshiftSdk.getInstance().updateMetaData(dictionary);
			Action action = delegate
			{
				session.ChangeState("Options");
				if (keyboard != null && keyboard.active)
				{
					keyboard.active = false;
				}
				AndroidBack.getInstance().pop();
			};
			Action action2 = delegate
			{
			};
			Action submitHandler = delegate
			{
				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					session.TheSoundEffectManager.PlaySound("Error");
					ExplanationDialogInputData item = new ExplanationDialogInputData(Language.Get("!!ERROR_NEED_NETWORK_TITLE"), "Beat_JellyfishFields_ComingSoon");
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData> { item });
					session.ChangeState("ShowingDialog");
				}
				else if (SubmitCheck(session))
				{
					session.TheSoundEffectManager.PlaySound("Accept");
					HelpshiftSdk.getInstance().showFAQs(configMap);
					session.ChangeState("Options");
				}
			};
			Action cancelHandler = delegate
			{
				session.TheSoundEffectManager.PlaySound("Accept");
				session.ChangeState("Options");
			};
			Action inputHandler = delegate
			{
				session.TheSoundEffectManager.PlaySound("Accept");
				if (ageGate != null)
				{
					keyboard = TouchScreenKeyboard.Open(inputString, TouchScreenKeyboardType.NumberPad);
				}
			};
			session.properties.ageGateHud = SBUIBuilder.MakeAndPushStandardUI(session, false, null, action2, action2, action, action2, null, DragFeeding.SwitchToFn(session), null, action2, action2, action2, action2);
			session.properties.ageGateHud.SetActive(true);
			session.properties.ageGateHud.SetVisibleNonEssentialElements(false, true);
			SBGUIPulseButton sBGUIPulseButton = (SBGUIPulseButton)session.properties.ageGateHud.FindChild("marketplace");
			sBGUIPulseButton.SetActive(false);
			SBGUIPulseButton sBGUIPulseButton2 = (SBGUIPulseButton)session.properties.ageGateHud.FindChild("community_event");
			sBGUIPulseButton2.SetActive(false);
			if (session.InFriendsGame)
			{
				SBGUIElement sBGUIElement = session.properties.ageGateHud.FindChild("happyface");
				sBGUIElement.SetActive(false);
				SBGUIElement sBGUIElement2 = session.properties.ageGateHud.FindChild("quest_marker");
				sBGUIElement2.SetActive(false);
				SBGUIElement sBGUIElement3 = session.properties.ageGateHud.FindChild("jj_bar");
				sBGUIElement3.SetActive(false);
				SBGUIElement sBGUIElement4 = session.properties.ageGateHud.FindChild("money_bar");
				sBGUIElement4.SetActive(false);
				SBGUIElement sBGUIElement5 = session.properties.ageGateHud.FindChild("special_bar");
				if ((bool)sBGUIElement5)
				{
					sBGUIElement5.SetActive(false);
				}
			}
			ageGate = SBUIBuilder.MakeAndPushAgeGateDialog(action, submitHandler, cancelHandler, inputHandler);
			inputLabel = (SBGUILabel)ageGate.FindChild("input_label");
			invalidAnswer = (SBGUILabel)ageGate.FindChild("invalid_answer");
			invalidAnswer.SetActive(false);
			equationLabel = (SBGUILabel)ageGate.FindChild("equation");
			GenerateRandomEquation();
			equationLabel.SetText(number1 + " X " + number2 + " = ?");
			session.TheCamera.TurnOnScreenBuffer();
			closeButton = ageGate.FindChild("close") as SBGUIButton;
		}

		public bool SubmitCheck(Session session)
		{
			int result;
			bool flag = int.TryParse(inputString, out result);
			if (ageGate != null && flag && result == correctAnswer)
			{
				ageGate.FindChild("invalid_answer").SetActive(false);
				inputString = string.Empty;
				inputLabel.SetText(string.Empty);
				return true;
			}
			if (ageGate != null)
			{
				ageGate.FindChild("invalid_answer").SetActive(true);
				GenerateRandomEquation();
				inputString = string.Empty;
				inputLabel.SetText(string.Empty);
				if (equationLabel == null)
				{
					equationLabel = (SBGUILabel)ageGate.FindChild("equation");
				}
				equationLabel.SetText(number1 + " x " + number2 + " = ?");
			}
			return false;
		}

		public void GenerateRandomEquation()
		{
			number1 = UnityEngine.Random.Range(4, 10);
			number2 = UnityEngine.Random.Range(4, 10);
			correctAnswer = number1 * number2;
		}

		public void OnLeave(Session session)
		{
			inputString = string.Empty;
			inputLabel.SetText(string.Empty);
			session.TheSoundEffectManager.PlaySound("CloseMenu");
			session.properties.ageGateHud.SetVisibleNonEssentialElements(true);
			session.properties.ageGateHud = null;
			session.TheCamera.TurnOffScreenBuffer();
			if (keyboard != null && keyboard.active)
			{
				keyboard.active = false;
			}
		}

		public void OnUpdate(Session session = null)
		{
			if (keyboard != null && keyboard.active && keyboard.text.Length > 3)
			{
				keyboard.text = keyboard.text.Substring(0, 3);
			}
			if (keyboard != null && keyboard.done)
			{
				inputString = keyboard.text.Substring(0, keyboard.text.Length);
				inputLabel.SetText(inputString);
				keyboard.text = string.Empty;
				keyboard = null;
			}
			session.game.simulation.OnUpdate(session);
		}
	}

	public class Authorizing : State
	{
		private bool errorScreenShown;

		public void OnEnter(Session session)
		{
			GameStarting.CreateLoadingScreen(session);
			if (Application.platform == RuntimePlatform.Android)
			{
				Screen.sleepTimeout = -1;
			}
			session.InFriendsGame = false;
			session.Auth.SoaringAuthorizing = true;
			Player.Init();
			if (!Soaring.IsInitialized)
			{
				SBMISoaring.Initialize(session.Auth);
			}
			else
			{
				if (!Soaring.IsOnline || !Soaring.IsAuthorized)
				{
					SoaringInternal.instance.ClearOfflineMode();
					SoaringInternal.instance.BeginHandshake(HandshakeResponder);
				}
				else
				{
					HandshakeResponder(null);
				}
				Upsight.requestAppOpen();
			}
			session.analytics.LogLoadingFunnelStep("Authorizing");
		}

		public void HandshakeResponder(SoaringContext c)
		{
			SessionDriver.session_ref.auth.FindAndMigrateLoginID();
		}

		public void OnLeave(Session session)
		{
			SBUIBuilder.ReleaseTopScreen();
		}

		public void OnUpdate(Session session)
		{
			if (session.Auth.SoaringAuthorizing || errorScreenShown)
			{
				return;
			}
			if (session.PlayerIsLoggedIn())
			{
				AddAdditionalCredentials();
				TFUtils.CheckForLogDumps(null);
				ChangeToResolveSessionStateOnStartup(session);
				return;
			}
			if (!Soaring.IsOnline && string.IsNullOrEmpty(Soaring.Player.UserID))
			{
				SoaringDictionary soaringDictionary = new SoaringDictionary(4);
				soaringDictionary.addValue(SoaringPlatform.DeviceID, "tag");
				soaringDictionary.addValue(SoaringPlatform.DeviceID, "userId");
				soaringDictionary.addValue(string.Empty, "authToken");
				SoaringInternal.instance.UpdatePlayerData(soaringDictionary);
				Soaring.Player.LoginType = SoaringLoginType.Device;
				Soaring.Player.IsLocalAuthorized = true;
				session.ThePlayer = new Player(SoaringPlatform.DeviceID);
			}
			else
			{
				session.ThePlayer = new Player(Soaring.Player.UserTag);
			}
			if (session.ThePlayer != null)
			{
				TFUtils.DebugLog(string.Format("The player is logged in with playerId {0}", session.ThePlayer.playerId));
				session.WebFileServer.SetPlayerInfo(session.ThePlayer);
				session.analytics.PlayerId = session.ThePlayer.playerId;
				session.analytics.IsOffline = !Soaring.IsOnline;
			}
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void AddAdditionalCredentials()
		{
		}
	}

	public class BrowsingRecipes : State
	{
		private const string BROWSING_UI_HANDLER = "browsing_ui";

		private const string KEEP_INVENTORY_OPEN = "KeepInventoryOpen";

		public void OnEnter(Session session)
		{
			session.game.dropManager.MarkForClearCurrentDrops();
			Simulated sim = session.game.selected;
			if (sim == null)
			{
				TFUtils.DebugLog("attempted to transition to browsing recipes without a selected entity");
				session.ChangeState("Playing");
				return;
			}
			BuildingEntity buildingEntity = sim.GetEntity<BuildingEntity>();
			TFUtils.Assert(buildingEntity != null, "Did not select a building for Crafting!");
			TFUtils.Assert(session.game.craftManager != null, "CraftingManager was not setup for this game!");
			CraftingCookbook cookbook = session.game.craftManager.GetCookbookById(buildingEntity.CraftMenu);
			if (!session.properties.lastSelectedRecipe.ContainsKey(cookbook))
			{
				session.properties.lastSelectedRecipe[cookbook] = null;
			}
			int num = 0;
			int num2 = 0;
			if (buildingEntity.ShuntsCrafting)
			{
				TFUtils.Assert(buildingEntity.Slots == 0, "The UI is not equipped to handle buildings that have production slots AND shunt crafting!");
				num2 = buildingEntity.Annexes.Count;
				num = num2;
			}
			else
			{
				num = buildingEntity.Slots;
				num2 = session.TheGame.craftManager.GetMaxSlots(session.TheGame.selected.entity.DefinitionId);
			}
			session.TheSoundEffectManager.PlaySound(cookbook.openSound);
			Action closeClickHandler = delegate
			{
				session.ChangeState("Playing");
				AndroidBack.getInstance().pop();
			};
			Action<SBGUICraftingScreen, CraftingRecipe> craftRecipeHandler = delegate(SBGUICraftingScreen screen, CraftingRecipe recipe)
			{
				if (session.game.craftManager.HasCapacity(sim.Id, buildingEntity.Slots))
				{
					CheckRecipeForJelly(session, screen, recipe);
				}
				else
				{
					session.TheSoundEffectManager.PlaySound("Error");
				}
			};
			Action<CraftingRecipe> setSelected = delegate(CraftingRecipe recipe)
			{
				session.properties.lastSelectedRecipe[cookbook] = recipe;
			};
			Action<int> rushCraftHandler = delegate(int slotId)
			{
				CraftProductionRush(session, slotId);
			};
			session.properties.recipesHud = SBUIBuilder.MakeAndPushStandardUI(session, false, null, delegate
			{
				session.AddAsyncResponse("store_open_type", "store_open_button");
				session.ChangeState("Shopping");
			}, delegate
			{
				session.CheckInventorySoftLock();
				session.ChangeState("Inventory");
			}, delegate
			{
				session.ChangeState("Options");
			}, delegate
			{
				session.ChangeState("Editing");
			}, null, null, null, delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping");
			}, delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping");
			}, delegate
			{
				session.ChangeState("CommunityEvent");
			}, null);
			bool flag = session.properties.recipesHud.ShowInventoryWidget();
			session.properties.recipesHud.SetVisibleNonEssentialElements(false);
			session.AddAsyncResponse("KeepInventoryOpen", !flag);
			session.properties.m_pTaskSimulated = null;
			session.properties.m_bAutoPanToSimulatedOnLeave = false;
			Action<int> pTaskCharacterClicked = delegate(int nDID)
			{
				Simulated simulated = session.TheGame.simulation.FindSimulated(nDID);
				if (simulated != null)
				{
					session.properties.m_pTaskSimulated = simulated;
					TaskManager taskManager = session.TheGame.taskManager;
					List<Task> activeTasksForSimulated2 = session.TheGame.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id);
					if (activeTasksForSimulated2.Count > 0 && activeTasksForSimulated2[0].GetTimeLeft() != 0)
					{
						if (taskManager.GetTaskingStateForSimulated(session.TheGame.simulation, nDID, simulated.Id) == TaskManager._eBlueprintTaskingState.eNone)
						{
							session.ChangeState("UnitIdle");
						}
						else
						{
							session.ChangeState("UnitBusy");
						}
					}
					else
					{
						session.properties.m_bAutoPanToSimulatedOnLeave = true;
						session.ChangeState("Playing");
					}
				}
			};
			List<int> activeSourcesForTarget = session.TheGame.taskManager.GetActiveSourcesForTarget(sim.Id);
			int num3 = activeSourcesForTarget.Count;
			for (int num4 = 0; num4 < num3; num4++)
			{
				List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(activeSourcesForTarget[num4], null);
				if (activeTasksForSimulated.Count > 0 && activeTasksForSimulated[0].GetTimeLeft() == 0)
				{
					activeSourcesForTarget.RemoveAt(num4);
					num4--;
					num3--;
				}
			}
			session.properties.recipesWindow = SBUIBuilder.MakeAndPushCraftingUI(session, null, closeClickHandler, craftRecipeHandler, rushCraftHandler, setSelected, cookbook, session.properties.lastSelectedRecipe[cookbook], activeSourcesForTarget, pTaskCharacterClicked, num, num2);
			session.properties.transitionSilently = false;
			Action action = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.game.sessionActionManager.SetActionHandler("browsing_ui", session, new List<SBGUIScreen>
			{
				session.properties.recipesHud,
				session.properties.recipesWindow
			}, SessionActionUiHelper.HandleCommonSessionActions);
			session.TheCamera.SetEnableUserInput(false);
			session.TheCamera.TurnOnScreenBuffer();
			SessionActionSimulationHelper.EnableHandler(session, true);
		}

		public void OnLeave(Session session)
		{
			session.game.sessionActionManager.ClearActionHandler("browsing_ui", session);
			if (!session.properties.transitionSilently)
			{
				if (session.game.selected != null)
				{
					BuildingEntity entity = session.game.selected.GetEntity<BuildingEntity>();
					CraftingCookbook cookbookById = session.game.craftManager.GetCookbookById(entity.CraftMenu);
					session.TheSoundEffectManager.PlaySound(cookbookById.closeSound);
				}
				if (session.properties.recipesHud != null)
				{
					session.properties.recipesHud.SetVisibleNonEssentialElements(true);
					if (!(bool)session.CheckAsyncRequest("KeepInventoryOpen"))
					{
						session.properties.recipesHud.CloseInventoryWidget();
					}
				}
				if (session.properties.recipesWindow != null)
				{
					session.properties.recipesWindow.ForceCycleProdSlots();
				}
				if (session.TheCamera.ScreenBufferOn)
				{
					session.TheCamera.TurnOffScreenBuffer();
				}
				session.properties.recipesHud = null;
				session.properties.recipesWindow = null;
			}
			if (session.properties.m_pTaskSimulated != null)
			{
				session.game.selected = session.properties.m_pTaskSimulated;
			}
			if (session.game.selected != null && session.properties.m_bAutoPanToSimulatedOnLeave)
			{
				session.TheCamera.AutoPanToPosition(session.game.selected.PositionCenter, 0.75f);
			}
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}

		public void CheckRecipeForJelly(Session session, SBGUICraftingScreen screen, CraftingRecipe recipe)
		{
			if (recipe.cost.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY) && session.game.resourceManager.CanPay(recipe.cost))
			{
				Action execute = delegate
				{
					CraftRecipe(session, screen, recipe);
				};
				Action<bool, Cost> logSpend = delegate(bool canAfford, Cost cost)
				{
					session.analytics.LogPremiumCrafting(recipe.recipeName, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount, cost, canAfford);
				};
				int jellyCost = 0;
				recipe.cost.ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out jellyCost);
				Action complete = delegate
				{
					session.ChangeState("BrowsingRecipes");
					session.properties.recipesWindow.ForceCycleProdSlots();
					AnalyticsWrapper.LogJellyConfirmation(session.TheGame, recipe.identity, jellyCost, recipe.recipeName, "craft", "instant_purchase", string.Empty, "confirm");
				};
				Action cancel = delegate
				{
					session.ChangeState("BrowsingRecipes");
					session.properties.recipesWindow.ForceCycleProdSlots();
				};
				session.properties.transitionSilently = true;
				session.properties.hardSpendActions = new HardSpendActions(execute, (ulong time) => new Cost(new Dictionary<int, int> { 
				{
					ResourceManager.HARD_CURRENCY,
					jellyCost
				} }), string.Empty, recipe.identity, complete, cancel, logSpend, (!(session.properties.recipesWindow == null)) ? session.properties.recipesWindow.GetHardSpendPosition() : session.TheCamera.ScreenCenter);
				session.properties.recipesWindow.ShowScrollRegion(false);
				session.ChangeState("HardSpendConfirm", false);
			}
			else
			{
				CraftRecipe(session, screen, recipe);
			}
		}

		public void CraftRecipe(Session session, SBGUICraftingScreen screen, CraftingRecipe recipe)
		{
			Simulated simulated = session.game.selected;
			BuildingEntity buildingEntity = simulated.GetEntity<BuildingEntity>();
			int? num = null;
			SessionActionTracker sessionActionTracker = (SessionActionTracker)session.CheckAsyncRequest("force_crafting_instance_slot_sessionaction");
			if (sessionActionTracker != null)
			{
				ForceCraftingInstanceSlot forceCraftingInstanceSlot = (ForceCraftingInstanceSlot)sessionActionTracker.Definition;
				if (forceCraftingInstanceSlot != null)
				{
					num = forceCraftingInstanceSlot.SlotID;
				}
			}
			Entity entity = null;
			if (simulated.GetEntity<BuildingEntity>().ShuntsCrafting)
			{
				List<Entity> annexes = buildingEntity.Annexes;
				if (num.HasValue)
				{
					entity = annexes[num.Value];
					if (session.TheGame.craftManager.Crafting(entity.Id))
					{
						TFUtils.WarningLog("Tried to force crafting into annex " + num.Value + ", but that annex is already crafting!");
						entity = null;
					}
				}
				if (entity == null)
				{
					entity = annexes.Find((Entity annex) => !session.TheGame.craftManager.Crafting(annex.Id));
				}
				if (entity == null)
				{
					TFUtils.WarningLog("Trying to craft but there are no free annexes for this building to shunt onto.");
					session.TheSoundEffectManager.PlaySound("Error");
					return;
				}
				buildingEntity = entity.GetDecorator<BuildingEntity>();
				simulated = session.game.simulation.FindSimulated(buildingEntity.Id);
			}
			if (session.game.resourceManager.CanPay(recipe.cost) && session.game.craftManager.HasCapacity(simulated.Id, buildingEntity.Slots))
			{
				session.game.resourceManager.Apply(recipe.cost, session.game);
				session.game.simulation.Router.Send(CraftCommand.Create(simulated.Id, simulated.Id));
				int num2 = session.game.craftManager.GetNextSlot(simulated.Id, buildingEntity.Slots);
				if (num.HasValue && entity == null)
				{
					num2 = num.Value;
					if (session.TheGame.craftManager.GetCraftingInstance(simulated.Id, num2) != null)
					{
						TFUtils.WarningLog("Tried to force crafting into annex " + num.Value + ", but that annex is already crafting!");
						num2 = session.game.craftManager.GetNextSlot(simulated.Id, buildingEntity.Slots);
					}
				}
				TFUtils.Assert(num2 != -1, "Session check for HasCapacity did not catch a filled crafting state, or GetNextSlot is broken");
				CraftingInstance craftingInstance = new CraftingInstance(buildingEntity.Id, recipe.identity, TFUtils.EpochTime() + recipe.craftTime, recipe.rewardDefinition.GenerateReward(session.TheGame.simulation, true, false), num2);
				if (session.game.craftManager.AddCraftingInstance(craftingInstance))
				{
					CraftStartAction action = new CraftStartAction(buildingEntity.Id, num2, recipe.identity, craftingInstance.ReadyTimeUtc, recipe.rewardDefinition.GenerateReward(session.TheGame.simulation, true, false), recipe.cost);
					session.game.simulation.ModifyGameStateSimulated(simulated, action);
					session.game.simulation.Router.Send(CraftedCommand.Create(simulated.Id, simulated.Id, num2), recipe.craftTime);
					session.TheSoundEffectManager.PlaySound(recipe.startSoundImmediate);
					session.TheSoundEffectManager.PlaySound(recipe.startSoundBeat, recipe.beatLength);
					session.TheSoundEffectManager.PlaySound("CloseGalleyGrubMenu");
				}
				return;
			}
			Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(recipe.cost.ResourceAmounts, session.game.resourceManager);
			if (resourcesStillRequired.Count > 0)
			{
				session.TheSoundEffectManager.PlaySound("Error");
				Action okAction = delegate
				{
					CraftRecipe(session, screen, recipe);
					session.ChangeState("BrowsingRecipes");
				};
				Action cancelAction = delegate
				{
					session.ChangeState("BrowsingRecipes");
				};
				session.InsufficientResourcesHandler(session, recipe.recipeName, recipe.identity, okAction, cancelAction, recipe.cost);
			}
			else
			{
				TFUtils.ErrorLog("Was not able to craft but had enough resources... something else bad has probably happened.");
			}
		}

		private void CraftProductionRush(Session session, int slotId)
		{
			TFUtils.Assert(session != null && session.game != null && session.game.selected != null, "Trying to update a Rush Slot in an invalid game state");
			int slotId2 = slotId;
			Simulated selected = session.game.selected;
			Simulated simToRush = selected;
			BuildingEntity entityToRush = simToRush.GetEntity<BuildingEntity>();
			if (entityToRush.ShuntsCrafting)
			{
				List<Entity> annexes = entityToRush.Annexes;
				entityToRush = annexes[slotId].GetDecorator<BuildingEntity>();
				slotId = 0;
				TFUtils.Assert(entityToRush != null && session.TheGame.craftManager.GetCraftingInstance(entityToRush.Id, slotId) != null, "Could not find an annex that was crafting from this list of annexes!");
				simToRush = session.game.simulation.FindSimulated(entityToRush.Id);
			}
			Cost.CostAtTime costAtTime;
			string subjectText;
			int subjectDID;
			Action execute;
			Action<bool, Cost> logSpend;
			Action complete;
			Action cancel;
			if (slotId < entityToRush.Slots)
			{
				CraftingInstance instance = session.game.craftManager.GetCraftingInstance(entityToRush.Id, slotId);
				CraftingRecipe recipe = session.game.craftManager.GetRecipeById(instance.recipeId);
				Cost fullCost = recipe.rushCost;
				ulong craftStartTime = instance.ReadyTimeUtc - recipe.craftTime;
				costAtTime = (ulong ts) => Cost.Prorate(fullCost, craftStartTime, instance.ReadyTimeUtc, ts);
				subjectText = recipe.recipeName;
				subjectDID = recipe.identity;
				Cost pFinalCost = Cost.Prorate(fullCost, craftStartTime, instance.ReadyTimeUtc, TFUtils.EpochTime());
				execute = delegate
				{
					instance.rushed = true;
					session.game.simulation.Router.Send(RushCommand.Create(entityToRush.Id, instance.slotId));
				};
				logSpend = delegate(bool canAfford, Cost cost)
				{
					session.analytics.LogRushCraft(recipe.recipeName, cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], canAfford);
				};
				complete = delegate
				{
					session.ChangeState("BrowsingRecipes");
					session.properties.recipesWindow.ForceCycleProdSlots();
					string sItemName = "Accelerate_craft_" + recipe.recipeName;
					AnalyticsWrapper.LogJellyConfirmation(session.TheGame, recipe.identity, pFinalCost.ResourceAmounts[ResourceManager.HARD_CURRENCY], sItemName, "craft", "speedup", "craft", "confirm");
				};
				cancel = delegate
				{
					session.ChangeState("BrowsingRecipes");
					session.properties.recipesWindow.ForceCycleProdSlots();
				};
			}
			else
			{
				Cost expandCost = session.game.craftManager.GetSlotExpandCost(entityToRush.DefinitionId, entityToRush.Slots);
				costAtTime = (ulong ts) => expandCost;
				TFUtils.Assert(costAtTime != null, "Got back a new slot cost of null. This needs to be assigned somewhere!");
				subjectText = string.Empty;
				subjectDID = entityToRush.DefinitionId;
				execute = delegate
				{
					entityToRush.AddCraftingSlot();
					session.game.resourceManager.Spend(expandCost, session.game);
					session.game.simulation.ModifyGameStateSimulated(simToRush, new PurchaseCraftingSlotAction(entityToRush.Id, expandCost, entityToRush.Slots));
				};
				logSpend = delegate(bool canAfford, Cost cost)
				{
					session.analytics.LogPurchaseProductionSlot(entityToRush.BlueprintName, entityToRush.Slots, cost, canAfford, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
				};
				complete = delegate
				{
					session.ChangeState("BrowsingRecipes");
					session.properties.recipesWindow.ForceCycleProdSlots();
					AnalyticsWrapper.LogJellyConfirmation(session.TheGame, entityToRush.DefinitionId, expandCost.ResourceAmounts[ResourceManager.HARD_CURRENCY], entityToRush.Name, entityToRush.Name, "unlock", string.Empty, "confirm");
				};
				cancel = delegate
				{
					session.ChangeState("BrowsingRecipes");
					session.properties.recipesWindow.ForceCycleProdSlots();
				};
			}
			session.properties.transitionSilently = true;
			session.properties.hardSpendActions = new HardSpendActions(execute, costAtTime, subjectText, subjectDID, complete, cancel, logSpend, session.properties.recipesWindow.GetHardSpendButtonPositionForSlot(slotId2));
			session.properties.recipesWindow.ShowScrollRegion(false);
			session.ChangeState("HardSpendConfirm", false);
		}
	}

	public class CheckPatching : State
	{
		private bool _doneChecking;

		public void OnEnter(Session session)
		{
			TFUtils.DebugLog("Starting to Patch content");
			_doneChecking = false;
			session.CheckForPatching(true);
			session.analytics.LogLoadingFunnelStep("CheckPatching");
			GameStarting.CreateLoadingScreen(session);
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
			if (_doneChecking)
			{
				session.ChangeState("GameStarting");
			}
		}

		public void PatchingEventListener(string patchingEvent)
		{
			_doneChecking = true;
		}
	}

	public class Clearing : State
	{
		public void Purchase(Session session, DebrisEntity debrisEntity)
		{
			ClearableDecorator decorator = debrisEntity.GetDecorator<ClearableDecorator>();
			Cost clearCost = decorator.ClearCost;
			if (session.game.resourceManager.CanPay(clearCost))
			{
				Simulated selected = session.game.selected;
				session.game.resourceManager.Apply(clearCost, session.game);
				session.game.simulation.Router.Send(ClearCommand.Create(Identity.Null(), selected.Id));
				session.game.simulation.ModifyGameStateSimulated(selected, new DebrisStartAction(selected.Id, TFUtils.EpochTime() + decorator.ClearTime, decorator.ClearCost));
				session.ChangeState("Playing", false);
				session.analytics.LogClearDebris(selected.entity.BlueprintName, clearCost, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
				return;
			}
			Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(clearCost.ResourceAmounts, session.game.resourceManager);
			TFUtils.Assert(resourcesStillRequired.Count > 0, "Error occurred, we have enough resources to apply cost.");
			session.TheSoundEffectManager.PlaySound("Error");
			Action okAction = delegate
			{
				Purchase(session, debrisEntity);
				session.ChangeState("Playing");
			};
			Action cancelAction = delegate
			{
				session.ChangeState("Playing");
			};
			session.InsufficientResourcesHandler(session, "clear " + session.game.selected.entity.BlueprintName, session.game.selected.entity.DefinitionId, okAction, cancelAction, clearCost);
		}

		public void OnEnter(Session session)
		{
			Simulated selected = session.game.selected;
			DebrisEntity entity = selected.GetEntity<DebrisEntity>();
			TFUtils.Assert(entity != null, "Did not select a debris for Clearing!");
			Purchase(session, entity);
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, false);
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	public class CommunityEventSession : State
	{
		public void OnEnter(Session session)
		{
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			session.camera.SetEnableUserInput(false);
			session.TheCamera.TurnOnScreenBuffer();
			Action openIAPTabHandlerSoft = delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping");
			};
			Action openIAPTabHandlerHard = delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping");
			};
			Action action = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.properties.communityEventHud = SBUIBuilder.MakeAndPushStandardUI(session, false, HandleSBGUIEvent, delegate
			{
				session.AddAsyncResponse("store_open_type", "store_open_button");
				session.ChangeState("Shopping");
			}, delegate
			{
				session.ChangeState("Inventory");
			}, delegate
			{
				session.ChangeState("Options");
			}, delegate
			{
				session.ChangeState("Editing");
			}, null, DragFeeding.SwitchToFn(session), null, openIAPTabHandlerSoft, openIAPTabHandlerHard, delegate
			{
				session.ChangeState("Playing");
			}, null);
			session.properties.communityEventHud.SetVisibleNonEssentialElements(false, true);
			SBGUIButton sBGUIButton = (SBGUIButton)session.properties.communityEventHud.FindChild("inventory");
			sBGUIButton.SetActive(false);
			SBGUIButton sBGUIButton2 = (SBGUIButton)session.properties.communityEventHud.FindChild("marketplace");
			sBGUIButton2.SetActive(false);
			SBGUIButton sBGUIButton3 = (SBGUIButton)session.properties.communityEventHud.FindChild("community_event");
			sBGUIButton3.SetActive(false);
			session.properties.communityEventHud.HideAllElements();
			session.properties.transitionSilently = false;
			Action<CommunityEvent, SoaringCommunityEvent, SoaringCommunityEvent.Reward> purchaseHandler = delegate(CommunityEvent pEvent, SoaringCommunityEvent pSoaringEvent, SoaringCommunityEvent.Reward pReward)
			{
				int amount = session.TheGame.resourceManager.Resources[pEvent.m_nValueID].Amount;
				int nCost = pReward.m_nValue - amount;
				string sRewardName = string.Empty;
				if (pReward != null)
				{
					Blueprint blueprint = EntityManager.GetBlueprint(EntityType.BUILDING, pReward.m_nID, true);
					if (blueprint != null)
					{
						sRewardName = (string)blueprint.Invariable["name"];
					}
				}
				Dictionary<int, int> dictionary = null;
				if (nCost > 0)
				{
					dictionary = new Dictionary<int, int> { { pEvent.m_nValueID, nCost } };
					nCost = session.TheGame.resourceManager.GetResourcesPackageCostInHardCurrencyValue(new Cost(dictionary));
					Dictionary<int, int> jellyRequired = new Dictionary<int, int> { 
					{
						ResourceManager.HARD_CURRENCY,
						nCost
					} };
					Cost.CostAtTime cost = (ulong ulUtcNow) => new Cost(jellyRequired);
					Action cancel = delegate
					{
						session.ChangeState("CommunityEvent");
						session.properties.communityEventScreen.BuyRewardCancel();
						AnalyticsWrapper.LogJellyConfirmation(session.game, pReward.m_nID, nCost, sRewardName, "buildings", "community_event_purchase", string.Empty, "cancel");
					};
					Action execute = delegate
					{
						session.properties.communityEventScreen.BuyRewardConfirm(nCost);
						AnalyticsWrapper.LogJellyConfirmation(session.game, pReward.m_nID, nCost, sRewardName, "buildings", "community_event_purchase", string.Empty, "confirm");
					};
					Action complete = delegate
					{
						session.ChangeState("CommunityEvent");
					};
					Action<bool, Cost> logSpend = delegate
					{
					};
					session.properties.hardSpendActions = new HardSpendActions(execute, cost, sRewardName, pReward.m_nID, complete, cancel, logSpend, session.properties.communityEventScreen.GetHardSpendButtonPosition());
					session.properties.transitionSilently = true;
					session.ChangeState("HardSpendConfirm", false);
				}
				else
				{
					session.properties.communityEventScreen.BuyRewardConfirm(0);
				}
			};
			SBGUIScreen sBGUIScreen = SBUIBuilder.MakeAndPushCommunityEventDialog(session, delegate
			{
				session.ChangeState("Playing");
			}, purchaseHandler);
			session.properties.communityEventScreen = sBGUIScreen.GetComponent<SBGUICommunityEventScreen>();
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
		}

		public void OnLeave(Session session)
		{
			if (!session.properties.transitionSilently)
			{
				session.TheSoundEffectManager.PlaySound("CloseMenu");
				session.properties.communityEventHud.SetVisibleNonEssentialElements(true);
				session.properties.communityEventHud = null;
				session.TheCamera.TurnOffScreenBuffer();
			}
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	public class Credits : State
	{
		public void OnEnter(Session session)
		{
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			session.PlayBubbleScreenSwipeEffect();
			session.camera.SetEnableUserInput(false);
			session.TheCamera.TurnOnScreenBuffer();
			Action closeClickHandler = delegate
			{
				session.ChangeState("Options");
				AndroidBack.getInstance().pop();
			};
			SBUIBuilder.MakeAndPushEmptyUI(session, null);
			SBUIBuilder.MakeAndPushCreditsUI(session, closeClickHandler);
		}

		public void OnLeave(Session session)
		{
			session.TheSoundEffectManager.PlaySound("CloseMenu");
			session.TheCamera.TurnOffScreenBuffer();
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
		}
	}

	public class SessionDebug : State
	{
		private const string DEBUG_SCREEN = "DEBUG_SCREEN";

		private int dailyBonusDay = 1;

		public void OnEnter(Session session)
		{
			SBGUIDebugScreen debugScreen = null;
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			session.PlayBubbleScreenSwipeEffect();
			session.camera.SetEnableUserInput(false);
			session.TheCamera.TurnOnScreenBuffer();
			Action closeClickHandler = delegate
			{
				session.ChangeState("Playing");
				AndroidBack.getInstance().pop();
			};
			Action<string> commonToggleStuff = delegate(string output)
			{
				TFUtils.DebugLog(output);
				session.TheSoundEffectManager.PlaySound("Accept");
				debugScreen = (SBGUIDebugScreen)session.CheckAsyncRequest("DEBUG_SCREEN");
				debugScreen.Refresh();
				session.AddAsyncResponse("DEBUG_SCREEN", debugScreen);
			};
			Action toggleFreeEditMode = delegate
			{
				TheDebugManager.ToggleDebugPlaceObjects(session);
				commonToggleStuff("Toggling Free Edit Mode");
			};
			Action saveFreeEditProgress = delegate
			{
				TFUtils.DebugLog("Saving Free Edit Progress is only supported in the editor");
				session.TheSoundEffectManager.PlaySound("Error");
			};
			Action toggleFramerateCounter = delegate
			{
				TheDebugManager.ToggleFramerateCounter(session);
				commonToggleStuff("Toggle Framerate Counter");
			};
			Action toggleHitBoxes = delegate
			{
				TheDebugManager.ToggleHitBoxes(session.game.simulation);
				commonToggleStuff("Toggle hit boxes");
			};
			Action toggleFootprints = delegate
			{
				TheDebugManager.ToggleFootprints(session.game.simulation);
				commonToggleStuff("Toggle footprints");
			};
			Action toggleExpansionBorders = delegate
			{
				TheDebugManager.ToggleExpansionBorders(session.game.simulation);
				commonToggleStuff("Toggle expansion borders");
			};
			Action addMoney = delegate
			{
				int amount = 100000;
				session.TheGame.resourceManager.Add(ResourceManager.SOFT_CURRENCY, amount, session.game);
				Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
				{
					ResourceManager.AddAmountToGameState(ResourceManager.SOFT_CURRENCY, amount, gameState);
				};
				session.TheGame.LockedGameStateChange(writer);
				commonToggleStuff("Add money");
			};
			Action addJJ = delegate
			{
				int amount = 1000;
				session.TheGame.resourceManager.Add(ResourceManager.HARD_CURRENCY, amount, session.game);
				Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
				{
					ResourceManager.AddAmountToGameState(ResourceManager.HARD_CURRENCY, amount, gameState);
				};
				session.TheGame.LockedGameStateChange(writer);
				commonToggleStuff("Add JJ");
			};
			Action addSpecialCurrency = delegate
			{
				int amount = 100;
				if (ResourceManager.SPECIAL_CURRENCY >= 0)
				{
					session.TheGame.resourceManager.Add(ResourceManager.SPECIAL_CURRENCY, amount, session.game);
					Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
					{
						ResourceManager.AddAmountToGameState(ResourceManager.SPECIAL_CURRENCY, amount, gameState);
					};
					session.TheGame.LockedGameStateChange(writer);
					commonToggleStuff("Add Special Currency");
				}
			};
			Action addFoods = delegate
			{
				int amount = 1000;
				foreach (KeyValuePair<int, Resource> kvp in session.TheGame.resourceManager.Resources)
				{
					if (kvp.Key != 4 && kvp.Key != 5 && kvp.Key != 2 && kvp.Key != 3 && kvp.Key != 51 && kvp.Key != 52)
					{
						session.TheGame.resourceManager.Add(kvp.Key, amount, session.game);
						Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
						{
							ResourceManager.AddAmountToGameState(kvp.Key, amount, gameState);
						};
						session.TheGame.LockedGameStateChange(writer);
					}
				}
				commonToggleStuff("Add Foods");
			};
			Action toggleRMT = delegate
			{
				TheDebugManager.ToggleRMT();
				commonToggleStuff("Toggle RMT");
			};
			Action deleteServerGame = delegate
			{
				TheDebugManager.DeleteGameData();
			};
			Action resetEventItems = delegate
			{
				CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
				if (activeEvent != null)
				{
					SBMISoaring.ResetEventGifts(session, int.Parse(activeEvent.m_sID));
					int nValueID = activeEvent.m_nValueID;
					Resource resource = null;
					if (session.TheGame.resourceManager.Resources.ContainsKey(nValueID))
					{
						resource = session.TheGame.resourceManager.Resources[nValueID];
						Reward reward = new Reward(new Dictionary<int, int> { 
						{
							nValueID,
							-resource.Amount
						} }, null, null, null, null, null, null, null, false, null);
						session.TheGame.ApplyReward(reward, TFUtils.EpochTime(), false);
						session.TheGame.ModifyGameState(new ReceiveRewardAction(reward, string.Empty));
						SBMISoaring.SetEventValue(session, int.Parse(activeEvent.m_sID), 0);
					}
				}
			};
			Action toggleFreeCameraMode = delegate
			{
				TheDebugManager.ToggleFreeCameraMode(session.camera);
				commonToggleStuff("Toggle Free Camera Mode");
			};
			Action completeAllQuests = delegate
			{
				debugManager.CompleteAllQuests();
				session.ChangeState("Playing");
			};
			Action levelUp = delegate
			{
				int num = session.TheGame.resourceManager.Query(ResourceManager.LEVEL);
				int levelAmount = session.TheGame.levelingManager.MaxLevel - num;
				session.TheGame.resourceManager.Add(ResourceManager.LEVEL, levelAmount, session.game);
				session.TheGame.featureManager.UnlockAllFeatures();
				session.TheGame.craftManager.UnlockAllRecipes(session.TheGame);
				session.TheGame.movieManager.UnlockAllMovies();
				session.TheGame.costumeManager.UnlockAllCostumes();
				Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
				{
					ResourceManager.AddAmountToGameState(ResourceManager.LEVEL, levelAmount, gameState);
					session.TheGame.featureManager.UnlockAllFeaturesToGamestate(gameState);
					session.TheGame.craftManager.UnlockAllRecipesToGamestate(gameState);
					session.TheGame.movieManager.UnlockAllMoviesToGamestate(gameState);
					session.TheGame.costumeManager.UnlockAllCostumesToGamestate(gameState);
				};
				session.TheGame.LockedGameStateChange(writer);
				commonToggleStuff("Level up");
			};
			Action logDump = delegate
			{
				Action okHandler = delegate
				{
					TFUtils.LogDump(session, "button_log_dump");
					commonToggleStuff("Log Dump");
					SBUIBuilder.ReleaseTopScreen();
				};
				Action cancelHandler = delegate
				{
					SoaringDictionary soaringDictionary = new SoaringDictionary(4);
					TFUtils.LogDump(session, "button_log_dump", null, soaringDictionary);
					SoaringPlatform.SendEmail(Soaring.Player.UserID + "-Debug-Log", soaringDictionary.ToString(), "bbethel@flyingwisdomstudios.com");
					commonToggleStuff("Log Dump");
					SBUIBuilder.ReleaseTopScreen();
				};
				SBUIBuilder.CreateErrorDialog(session, "DUMP LOG", "Would you like to just dump the log or send an email?", Language.Get("!!PREFAB_OK"), okHandler, Language.Get("!!SEND_EMAIL"), cancelHandler, 0.85f, 0.45f);
			};
			Action unlockDecos = delegate
			{
				TFUtils.DebugLog("Moving all decorations and buildings to inventory");
				List<string> allBuildingBlueprintKeys = EntityManager.GetAllBuildingBlueprintKeys();
				foreach (string item2 in allBuildingBlueprintKeys)
				{
					BuildingEntity decorator = session.game.entities.Create(item2, false).GetDecorator<BuildingEntity>();
					session.game.inventory.AddItem(decorator, null);
				}
			};
			Action addHourSimulation = delegate
			{
				session.TheGame.AddTimeToSimulation(3600uL);
			};
			Action action = delegate
			{
				SoaringDebug.DebugListTextures("DEBUG");
			};
			Action incrementDailyBonus = delegate
			{
				SoaringContext soaringContext = new SoaringContext();
				soaringContext.Name = "DailyBonus";
				soaringContext.Responder = new SBMISoaring.SMBICacheDelegate();
				soaringContext.addValue(new SoaringObject(session), "session");
				SBMISoaring.RetrieveDailyBonuseCalendar(dailyBonusDay, soaringContext, DisplayDailyBonus);
			};
			Action fastFoward = delegate
			{
				TFUtils.isFastForwarding = !TFUtils.isFastForwarding;
				if (TFUtils.isFastForwarding)
				{
					session.TheGame.FastForwardSimulationBegun();
				}
				else
				{
					session.TheGame.FastForwardSimulationFinished();
				}
			};
			Action addOneLevel = delegate
			{
				session.TheGame.resourceManager.Add(ResourceManager.LEVEL, 1, session.game);
				session.TheGame.resourceManager.UpdateLevelExpToMilestone(session.TheGame.levelingManager);
				Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
				{
					ResourceManager.AddAmountToGameState(ResourceManager.LEVEL, 1, gameState);
				};
				session.TheGame.LockedGameStateChange(writer);
				commonToggleStuff("Add One Level");
				ResourceManager resourceManager = session.TheGame.resourceManager;
				List<Reward> rewards = null;
				IResourceProgressCalculator resourceCalculator = session.TheGame.simulation.resourceCalculatorManager.GetResourceCalculator(ResourceManager.XP);
				if (resourceCalculator != null)
				{
					resourceCalculator.GetRewardsForIncreasingResource(session.TheGame.simulation, resourceManager.Resources, 0, out rewards);
				}
				LevelUpDialogInputData item = new LevelUpDialogInputData(resourceManager.Query(ResourceManager.LEVEL), rewards);
				session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData> { item });
				session.ChangeState("ShowingDialog");
			};
			Action reset_device_id = delegate
			{
				debugManager.ResetDeviceID();
			};
			SBGUIScreen parent = SBUIBuilder.MakeAndPushEmptyUI(session, null);
			debugScreen = SBUIBuilder.MakeAndParentDebugUI(session, parent, closeClickHandler, toggleFramerateCounter, toggleFreeEditMode, saveFreeEditProgress, toggleHitBoxes, toggleFootprints, toggleExpansionBorders, addMoney, addJJ, addSpecialCurrency, addFoods, toggleRMT, deleteServerGame, resetEventItems, toggleFreeCameraMode, completeAllQuests, levelUp, logDump, unlockDecos, addHourSimulation, incrementDailyBonus, fastFoward, addOneLevel, reset_device_id);
			session.AddAsyncResponse("DEBUG_SCREEN", debugScreen);
		}

		private void DisplayDailyBonus(SoaringContext context)
		{
			Session session = null;
			SoaringError soaringError = null;
			bool flag = false;
			if (Soaring.IsOnline && context != null)
			{
				flag = context.soaringValue("query");
				if (flag)
				{
					SoaringObjectBase soaringObjectBase = context.objectWithKey("session");
					if (soaringObjectBase != null)
					{
						session = (Session)((SoaringObject)soaringObjectBase).Object;
					}
					else
					{
						flag = false;
					}
				}
			}
			if (context != null)
			{
				SoaringObjectBase soaringObjectBase2 = context.objectWithKey("error_code");
				if (soaringObjectBase2 != null)
				{
					soaringError = (SoaringError)soaringObjectBase2;
				}
			}
			if (flag)
			{
				DailyBonusDialogInputData dailyBonusDialogInputData = new DailyBonusDialogInputData();
				dailyBonusDay = dailyBonusDialogInputData.CurrentDay + 1;
				if (!dailyBonusDialogInputData.AlreadyCollected)
				{
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData> { dailyBonusDialogInputData });
					session.ChangeState("ShowingDialog");
				}
			}
			else
			{
				int num = -1;
				if (soaringError != null)
				{
					num = soaringError.ErrorCode;
				}
				Debug.Log("TODO: HANDLE THE ERROR CODE: " + num);
			}
		}

		public void OnLeave(Session session)
		{
			session.TheSoundEffectManager.PlaySound("CloseMenu");
			session.TheCamera.TurnOffScreenBuffer();
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
		}
	}

	public class DragFeeding : State
	{
		private const string DRAGFEEDING_UI_HANDLER = "dragfeeding_ui_handler";

		private SBGUIPulseImage icon;

		private static readonly Vector2 FINGER_OFFSET = new Vector2(-15f, -45f);

		private static readonly Quaternion ICON_ANGLE = Quaternion.AngleAxis(-12f, new Vector3(0f, 0f, -1f));

		public void OnEnter(Session session)
		{
			session.TheSoundEffectManager.PlaySound("grab_wish");
			session.TheCamera.SetEnableUserInput(false);
			SBGUIStandardScreen sBGUIStandardScreen = SBUIBuilder.MakeAndPushStandardUI(session, false, HandleSBGUIEvent, null, null, null, null, null, delegate
			{
			}, delegate(YGEvent evt)
			{
				HandleSBGUIEvent(new SBGUIEvent(evt), session);
			}, null, null, null, null);
			sBGUIStandardScreen.SetPatchyHudIconVisible();
			session.properties.dragFeedHud = sBGUIStandardScreen;
			Action action = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.game.sessionActionManager.SetActionHandler("dragfeeding_ui_handler", session, new List<SBGUIScreen> { sBGUIStandardScreen }, SessionActionUiHelper.HandleCommonSessionActions);
			if (icon == null)
			{
				icon = SBGUIPulseImage.Create(sBGUIStandardScreen, session.properties.draggedGood.resource.GetResourceTexture(), new Vector2(100f, 100f), 0.75f, 0.4f, null);
				icon.transform.rotation = ICON_ANGLE;
			}
			icon.SetTextureFromAtlas(session.properties.draggedGood.resource.GetResourceTexture());
			SetIconToEventPosition(session.properties.carriedUiEvent);
			session.properties.carriedUiEvent = null;
			icon.SetVisible(true);
		}

		public void OnLeave(Session session)
		{
			session.properties.dragFeedHud.inventory.Tidy();
			session.game.sessionActionManager.ClearActionHandler("dragfeeding_ui_handler", session);
			session.properties.dragFeedHud = null;
			session.properties.draggedGood = null;
			if (session.properties.candidateSimulated != null)
			{
				CancelTempt(session);
			}
			icon.SetVisible(false);
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			Predicate<Simulated> filterOutMatching = (Simulated simulated2) => simulated2.TemptationPriority >= 0;
			switch (evt.type)
			{
			case YGEvent.TYPE.TOUCH_STAY:
			case YGEvent.TYPE.TOUCH_MOVE:
			{
				SetIconToEventPosition(evt);
				Ray rayCast;
				if (session.properties.candidateSimulated != null)
				{
					List<Simulated> list = FindSimulatedsUnderPoint(filterOutMatching, session.game.simulation, session.TheCamera, evt.position - FINGER_OFFSET, out rayCast);
					if (!list.Contains(session.properties.candidateSimulated))
					{
						CancelTempt(session);
					}
				}
				if (session.properties.candidateSimulated == null)
				{
					Simulated simulated = FindBestSimulatedUnderPoint(new TemptationPrioritizer(session.camera.UnityCamera), filterOutMatching, session.game.simulation, session.TheCamera, evt.position - FINGER_OFFSET, out rayCast);
					if (simulated != null)
					{
						Tempt(simulated, session);
					}
				}
				break;
			}
			case YGEvent.TYPE.TOUCH_END:
			case YGEvent.TYPE.TOUCH_CANCEL:
			{
				bool flag = TryFeedTempted(session);
				int productId = session.properties.draggedGood.productId;
				if (!flag)
				{
					session.AddAsyncResponse("GoodReturnRequest", new GoodReturnRequest(icon.GetScreenPosition(), productId, session.game.resourceManager.Resources[productId].GetResourceTexture()));
					session.soundEffectManager.PlaySound("FailDragFeed");
				}
				session.ChangeState("Playing");
				session.CheckAsyncRequest("OriginalDragEvent");
				break;
			}
			}
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
			session.properties.dragFeedHud.inventory.IncrementDeductionsForTick(session.properties.draggedGood.productId);
		}

		public static void SwitchTo(Session session, int productId, YGEvent triggeringEvent)
		{
			SwitchToFn(session)(productId, triggeringEvent);
		}

		public static Action<int, YGEvent> SwitchToFn(Session session)
		{
			return delegate(int productId, YGEvent evt)
			{
				TFUtils.Assert(session.TheGame.resourceManager.Resources.ContainsKey(productId), "Was given a productId that does not exist in the resource manager! ProductId=" + productId);
				session.properties.draggedGood = new SessionProperties.DraggedGood(productId, session.TheGame.resourceManager.Resources[productId]);
				session.properties.carriedUiEvent = evt;
				session.ChangeState("DragFeeding");
			};
		}

		private void SetIconToEventPosition(YGEvent evt)
		{
			icon.SetScreenPosition(new Vector2(evt.position.x, (float)Screen.height - evt.position.y) + FINGER_OFFSET);
		}

		private void Tempt(Simulated simulated, Session session)
		{
			session.properties.candidateSimulated = simulated;
			session.game.simulation.Router.Send(TemptCommand.Create(Identity.Null(), session.properties.candidateSimulated.Id, session.properties.draggedGood.productId));
			if (simulated.GetEntity<ResidentEntity>().HungerResourceId.HasValue)
			{
				icon.Pulser.PulseStartLoop();
			}
		}

		private void CancelTempt(Session session)
		{
			session.game.simulation.Router.Send(AbortCommand.Create(Identity.Null(), session.properties.candidateSimulated.Id));
			session.properties.candidateSimulated = null;
			icon.Pulser.PulseStopLoop();
		}

		private bool TryFeedTempted(Session session)
		{
			Simulated candidateSimulated = session.properties.candidateSimulated;
			if (candidateSimulated != null)
			{
				int? num = candidateSimulated.GetEntity<ResidentEntity>().ForbiddenItemsWishTableDID;
				int? temptingID = session.properties.draggedGood.productId;
				bool flag = false;
				if (num.HasValue)
				{
					CdfDictionary<int> cdfDictionary = session.game.wishTableManager.GetWishTable(num.Value);
					if (cdfDictionary != null)
					{
						cdfDictionary = cdfDictionary.Where((int productID) => productID == temptingID.GetValueOrDefault() && temptingID.HasValue, true);
					}
					if (cdfDictionary != null && cdfDictionary.Count > 0)
					{
						flag = true;
					}
				}
				if (flag)
				{
					session.game.simulation.Router.Send(AbortCommand.Create(Identity.Null(), session.properties.candidateSimulated.Id));
					flag = false;
					num = null;
					return false;
				}
			}
			if (session.properties.candidateSimulated == null)
			{
				return false;
			}
			session.game.simulation.Router.Send(OfferFoodCommand.Create(Identity.Null(), session.properties.candidateSimulated.Id, session.properties.draggedGood.productId));
			session.properties.candidateSimulated = null;
			return true;
		}
	}

	public class Editing : Playing
	{
		public const string FROM_EDIT = "FromEdit";

		public override void OnEnter(Session session)
		{
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			Action inventoryClickHandler = delegate
			{
				if (!session.game.featureManager.CheckFeature("inventory_soft"))
				{
					session.game.featureManager.UnlockFeature("inventory_soft");
					session.game.featureManager.ActivateFeatureActions(session.game, "inventory_soft");
					session.game.simulation.ModifyGameState(new FeatureUnlocksAction(new List<string> { "inventory_soft" }));
				}
				else
				{
					session.AddAsyncResponse("FromEdit", true);
					session.ChangeState("Inventory");
				}
			};
			Action optionsHandler = delegate
			{
				session.ChangeState("Options");
			};
			Action editClickHandler = delegate
			{
				session.ChangeState("Playing");
			};
			Action openIAPTabHandlerSoft = delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping");
			};
			Action openIAPTabHandlerHard = delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping");
			};
			SBGUIStandardScreen sBGUIStandardScreen = SBUIBuilder.MakeAndPushStandardUI(session, true, HandleSBGUIEvent, null, inventoryClickHandler, optionsHandler, editClickHandler, delegate
			{
				session.ChangeState("Paving");
			}, null, null, openIAPTabHandlerSoft, openIAPTabHandlerHard, null, null, true);
			session.properties.playingHud = sBGUIStandardScreen;
			Action action = delegate
			{
				session.ChangeState("ShowingDialog");
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.camera.SetEnableUserInput(true);
			session.musicManager.PlayTrack("InGame");
			SBGUIAtlasButton sBGUIAtlasButton = (SBGUIAtlasButton)sBGUIStandardScreen.FindChild("quest");
			sBGUIAtlasButton.SetActive(true);
			SBGUIButton sBGUIButton = (SBGUIButton)session.properties.playingHud.FindChild("community_event");
			sBGUIButton.SetActive(false);
			SBGUIAtlasButton sBGUIAtlasButton2 = (SBGUIAtlasButton)sBGUIStandardScreen.FindChild("editpath_toggle");
			if (sBGUIAtlasButton2 != null)
			{
				sBGUIAtlasButton2.SetActive(true);
			}
			SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)sBGUIStandardScreen.FindChild("edit_mode_fish_person");
			if (sBGUIAtlasImage != null)
			{
				sBGUIAtlasImage.SetActive(true);
				sBGUIAtlasImage.SetTextureFromAtlas("EditMode_FishPerson", true, false, true);
			}
			session.game.sessionActionManager.SetActionHandler("playing_ui", session, new List<SBGUIScreen> { sBGUIStandardScreen }, SessionActionUiHelper.HandleCommonSessionActions);
			SessionActionSimulationHelper.EnableHandler(session, true);
			session.game.dropManager.MarkForClearCurrentDrops();
		}

		public override void OnLeave(Session session)
		{
			session.TheSoundEffectManager.PlaySound("CloseMenu");
			session.properties.playingHud = null;
			base.OnLeave(session);
		}

		public override void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			session.properties.playingHud.EnableUI(true);
			Predicate<Simulated> filterOutMatching = (Simulated simulated2) => !simulated2.InteractionState.IsEditable && !TheDebugManager.debugPlaceObjects;
			Ray rayCast;
			Simulated simulated = FindBestSimulatedUnderPoint(new SelectionPrioritizer(session.camera.UnityCamera), filterOutMatching, session.game.simulation, session.TheCamera, evt.position, out rayCast);
			switch (evt.type)
			{
			case YGEvent.TYPE.TOUCH_BEGIN:
				if (simulated == null)
				{
					break;
				}
				if (simulated.InteractionState.IsEditable || TheDebugManager.debugPlaceObjects)
				{
					if (simulated.entity == null)
					{
						TFUtils.ErrorLog("Session.Editing.HandleSBGUIEvent - clickedSim.entity is null");
					}
					else
					{
						session.TheSoundEffectManager.PlaySound(simulated.entity.SoundOnSelect);
					}
					session.game.selected = simulated;
					session.game.selected.Bounce();
					session.AddAsyncResponse("override_drag", session.game.selected);
					session.ChangeState("MoveBuildingInEdit");
				}
				{
					foreach (Action clickListener in simulated.ClickListeners)
					{
						clickListener();
					}
					break;
				}
			}
		}

		public override void OnUpdate(Session session)
		{
			base.OnUpdate(session);
		}
	}

	public class ErrorDialog : State
	{
		private const string ERROR_DIALOG = "ERROR_DIALOG";

		public void OnEnter(Session session)
		{
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			string title = (string)session.CheckAsyncRequest("error_message_title");
			string message = (string)session.CheckAsyncRequest("error_message");
			string acceptLabel = (string)session.CheckAsyncRequest("error_message_ok_label");
			Action okButtonHandler = (Action)session.CheckAsyncRequest("error_message_ok_action");
			float num = (float)session.CheckAsyncRequest("error_message_scale");
			SBGUIConfirmationDialog sBGUIConfirmationDialog = SBUIBuilder.MakeAndPushConfirmationDialog(session, HandleSBGUIEvent, title, message, acceptLabel, null, null, okButtonHandler, null, true);
			SBGUILabel sBGUILabel = (SBGUILabel)sBGUIConfirmationDialog.FindChild("message_label");
			YGTextAtlasSprite component = sBGUILabel.GetComponent<YGTextAtlasSprite>();
			component.scale = new Vector2(num, num);
			sBGUIConfirmationDialog.tform.parent = GUIMainView.GetInstance().transform;
			sBGUIConfirmationDialog.tform.localPosition = Vector3.zero;
		}

		public void OnLeave(Session session)
		{
			SBUIBuilder.ReleaseTopScreen();
		}

		public void OnUpdate(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	public class Expanding : State
	{
		public void OnEnter(Session session)
		{
			Action acceptButtonClickHandler = delegate
			{
				session.ChangeState("Playing");
			};
			SBUIBuilder.MakeAndPushAcceptUI(session, HandleSBGUIEvent, acceptButtonClickHandler);
			Action action = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.game.terrain.OutlineAvailableExpansionSlots(session.game);
		}

		public void OnLeave(Session session)
		{
			session.game.terrain.HideAvailableExpansionSlots();
			session.game.sessionActionManager.ClearActionHandler("expanding_ui", session);
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			switch (evt.type)
			{
			case YGEvent.TYPE.TAP:
			{
				Ray ray = session.TheCamera.ScreenPointToRay(evt.position);
				TerrainSlot selectedSlot = session.game.terrain.selectedSlot;
				if (session.game.terrain.ProcessTap(ray, session.game))
				{
					if (selectedSlot != null)
					{
						selectedSlot.DrawOutline();
					}
					ShowDialog(session);
				}
				break;
			}
			}
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, false);
		}

		private void ShowDialog(Session session)
		{
			SBGUIConfirmationDialog sBGUIConfirmationDialog = (SBGUIConfirmationDialog)session.CheckAsyncRequest("expansion");
			if (null != sBGUIConfirmationDialog)
			{
				SBUIBuilder.ReleaseTopScreen();
			}
			TerrainSlot slot = session.game.terrain.selectedSlot;
			TFUtils.Assert(slot != null, "Did not select an Expansion Slot properly!");
			session.game.terrain.HighlightSelection(slot);
			Cost expansionCost = session.game.terrain.GetExpansionCost(slot);
			Action cancelButtonHandler = delegate
			{
				SBUIBuilder.ReleaseTopScreen();
				session.game.terrain.DropSelection(slot);
				slot.DrawOutline();
			};
			Action okButtonHandler = delegate
			{
				SBUIBuilder.ReleaseTopScreen();
				PurchaseExpansion(session);
				session.ChangeState("Playing");
			};
			sBGUIConfirmationDialog = SBUIBuilder.MakeAndPushConfirmationDialog(session, HandleSBGUIEvent, Language.Get("!!EXPANSION_TITLE"), Language.Get("!!EXPANSION_MESSAGE"), Language.Get("!!PREFAB_OK"), Language.Get("!!PREFAB_CANCEL"), Cost.DisplayDictionary(expansionCost.ResourceAmounts, session.TheGame.resourceManager), okButtonHandler, cancelButtonHandler);
			session.AddAsyncResponse("expansion", sBGUIConfirmationDialog);
			session.game.sessionActionManager.SetActionHandler("expanding_ui", session, new List<SBGUIScreen> { sBGUIConfirmationDialog }, SessionActionUiHelper.HandleCommonSessionActions);
			SessionActionSimulationHelper.EnableHandler(session, true);
			session.soundEffectManager.PlaySound("Dialog_Expansion");
		}

		public void PurchaseExpansion(Session session)
		{
			TerrainSlot slot = session.game.terrain.selectedSlot;
			Cost expansionCost = session.game.terrain.GetExpansionCost(slot);
			if (session.game.resourceManager.CanPay(expansionCost))
			{
				session.TheSoundEffectManager.PlaySound("PurchaseExpansion");
				session.game.resourceManager.Apply(expansionCost, session.game);
				session.game.terrain.AddExpansionSlot(slot.Id);
				if (session.game.featureManager.CheckFeature("purchase_expansions"))
				{
					session.game.terrain.UpdateRealtySigns(session.game.entities.DisplayControllerManager, SBCamera.BillboardDefinition, session.game);
				}
				if (session.game.terrain.IsBorderSlot(slot.Id))
				{
					session.game.border.UpdateTerrainBorderStrip(session.game.terrain);
				}
				foreach (TerrainSlotObject landmark in slot.landmarks)
				{
					session.game.simulation.Router.Send(PurchaseCommand.Create(Identity.Null(), landmark.id));
				}
				foreach (TerrainSlotObject debri in slot.debris)
				{
					session.game.simulation.Router.Send(PurchaseCommand.Create(Identity.Null(), debri.id));
				}
				session.game.ModifyGameState(new NewExpansionAction(slot.Id, expansionCost, slot.debris, slot.landmarks));
				AnalyticsWrapper.LogExpansion(session.game, slot.Id, expansionCost);
				TFUtils.DebugLog("Purchased Expansion Slot: " + slot.Id);
				session.TheSoundEffectManager.PlaySound("Purchase");
				session.analytics.LogExpansion(slot.Id, expansionCost, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
				session.ChangeState("Playing");
			}
			else
			{
				Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(expansionCost.ResourceAmounts, session.game.resourceManager);
				TFUtils.Assert(resourcesStillRequired.Count > 0, "Error occurred, we have enough resources to apply cost.");
				session.TheSoundEffectManager.PlaySound("Error");
				Action action = delegate
				{
					session.game.terrain.DropSelection(slot);
					session.ChangeState("Expanding");
				};
				Action action2 = delegate
				{
					PurchaseExpansion(session);
				};
				Action cancelAction = action;
				Action okAction = action2;
				SBGUIConfirmationDialog sBGUIConfirmationDialog = (SBGUIConfirmationDialog)session.CheckAsyncRequest("expansion");
				session.AddAsyncResponse("expansion", sBGUIConfirmationDialog);
				sBGUIConfirmationDialog.SetActive(false);
				session.InsufficientResourcesHandler(session, "Expansion " + slot.Id, slot.Id, okAction, cancelAction, expansionCost);
			}
		}
	}

	public class NewExpansion : State
	{
		public void OnEnter(Session session)
		{
			session.camera.SetEnableUserInput(false);
			ShowDialog(session);
			Action action = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
		}

		public void OnLeave(Session session)
		{
			session.TheSoundEffectManager.PlaySound("CloseMenu");
			session.CheckAsyncRequest("expansion");
			session.game.sessionActionManager.ClearActionHandler("expansion_ui", session);
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, false);
		}

		public void PurchaseExpansion(Session session)
		{
			TerrainSlot slot = session.game.terrain.selectedSlot;
			bool flag = false;
			lock (expandLock)
			{
				if (slot == null || slot.inUse)
				{
					flag = true;
				}
				if (slot != null)
				{
					slot.inUse = true;
				}
			}
			if (flag)
			{
				session.ChangeState("Playing");
				return;
			}
			Cost expansionCost = session.game.terrain.GetExpansionCost(slot);
			if (session.game.resourceManager.CanPay(expansionCost))
			{
				session.TheSoundEffectManager.PlaySound("PurchaseExpansion");
				session.game.resourceManager.Apply(expansionCost, session.game);
				session.game.terrain.AddExpansionSlot(slot.Id);
				if (session.game.featureManager.CheckFeature("purchase_expansions"))
				{
					session.game.terrain.UpdateRealtySigns(session.game.entities.DisplayControllerManager, SBCamera.BillboardDefinition, session.game);
				}
				if (session.game.terrain.IsBorderSlot(slot.Id))
				{
					session.game.border.UpdateTerrainBorderStrip(session.game.terrain);
				}
				foreach (TerrainSlotObject landmark in slot.landmarks)
				{
					session.game.simulation.Router.Send(PurchaseCommand.Create(Identity.Null(), landmark.id));
				}
				foreach (TerrainSlotObject debri in slot.debris)
				{
					session.game.simulation.Router.Send(PurchaseCommand.Create(Identity.Null(), debri.id));
				}
				session.game.ModifyGameState(new NewExpansionAction(slot.Id, expansionCost, slot.debris, slot.landmarks));
				AnalyticsWrapper.LogExpansion(session.game, slot.Id, expansionCost);
				TFUtils.DebugLog("Purchased Expansion Slot: " + slot.Id);
				session.analytics.LogExpansion(slot.Id, expansionCost, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
				session.ChangeState("Playing");
				return;
			}
			Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(expansionCost.ResourceAmounts, session.game.resourceManager);
			TFUtils.Assert(resourcesStillRequired.Count > 0, "Error occurred, we have enough resources to apply cost.");
			session.TheSoundEffectManager.PlaySound("Error");
			Action action = delegate
			{
				session.game.terrain.DropSelection(slot);
				lock (expandLock)
				{
					if (slot != null)
					{
						slot.inUse = false;
					}
				}
				session.ChangeState("Playing");
			};
			Action action2 = delegate
			{
				lock (expandLock)
				{
					if (slot != null)
					{
						slot.inUse = false;
					}
				}
				PurchaseExpansion(session);
			};
			Action cancelAction = action;
			Action okAction = action2;
			SBGUIConfirmationDialog sBGUIConfirmationDialog = (SBGUIConfirmationDialog)session.CheckAsyncRequest("expansion");
			session.AddAsyncResponse("expansion", sBGUIConfirmationDialog);
			sBGUIConfirmationDialog.SetActive(false);
			session.InsufficientResourcesHandler(session, "Expansion " + slot.Id, slot.Id, okAction, cancelAction, expansionCost);
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		private void ShowDialog(Session session)
		{
			SBGUIConfirmationDialog sBGUIConfirmationDialog = (SBGUIConfirmationDialog)session.CheckAsyncRequest("expansion");
			if (null != sBGUIConfirmationDialog)
			{
				SBUIBuilder.ReleaseTopScreen();
			}
			TerrainSlot slot = session.game.terrain.selectedSlot;
			TFUtils.Assert(slot != null, "Did not select an Expansion Slot properly!");
			session.game.terrain.HighlightSelection(slot);
			Cost expansionCost = session.game.terrain.GetExpansionCost(slot);
			Action cancelButtonHandler = delegate
			{
				session.game.terrain.DropSelection(slot);
				session.ChangeState("Playing");
			};
			Action okButtonHandler = delegate
			{
				PurchaseExpansion(session);
			};
			SBUIBuilder.MakeAndPushEmptyUI(session, null);
			sBGUIConfirmationDialog = SBUIBuilder.MakeAndPushExpansionDialog(session, HandleSBGUIEvent, Language.Get("!!EXPANSION_TITLE"), Language.Get("!!EXPANSION_MESSAGE"), Language.Get("!!PREFAB_OK"), Language.Get("!!PREFAB_CANCEL"), Cost.DisplayDictionary(expansionCost.ResourceAmounts, session.TheGame.resourceManager), okButtonHandler, cancelButtonHandler);
			sBGUIConfirmationDialog.SetPosition(session.TheCamera.ScreenCenter);
			session.AddAsyncResponse("expansion", sBGUIConfirmationDialog);
			session.game.sessionActionManager.SetActionHandler("expansion_ui", session, new List<SBGUIScreen> { sBGUIConfirmationDialog }, SessionActionUiHelper.HandleCommonSessionActions);
			SessionActionSimulationHelper.EnableHandler(session, true);
			session.soundEffectManager.PlaySound("Dialog_Expansion");
		}
	}

	public class GameStarting : State
	{
		private enum GameStartingState
		{
			STATE_FIRST = -1,
			STATE_PATCHING_CONTENT = 0,
			STATE_ASSEMBLE_GAME_STATE = 1,
			STATE_LOAD_ENTITY_BLUEPRINTS = 2,
			STATE_CREATE_GAME = 3,
			STATE_LOAD_ASSETS = 4,
			STATE_FETCH_PRODUCT_INFO = 5,
			STATE_AWAIT_PRODUCT_INFO = 6,
			STATE_FETCH_PURCHASE_INFO = 7,
			STATE_AWAIT_PURCHASE_INFO = 8,
			STATE_START_STORE = 9,
			STATE_LOAD_ASSETS_TERRAIN = 10,
			STATE_LOAD_ASSETS_SIMULATION = 11,
			STATE_PRECACHE_GUI = 12,
			STATE_LOAD_ASSETS_TIME_DEPENDENTS = 13,
			STATE_LOAD_ASSETS_SEND_COMMANDS = 14,
			STATE_CREATE_TERRAIN_MESHES = 15,
			STATE_LOAD_ASSETS_ACTIVATE_QUESTS = 16,
			STATE_PROCESS_PENDING = 17,
			STATE_SETUP_SIMULATION = 18,
			STATE_LOAD_SOARING_COMMUNITY_EVENTS = 19,
			STATE_ANALYTICS_BOOKKEPING = 20,
			STATE_LAST = 21,
			STATE_ERROR = 22
		}

		public enum SplashScreenState
		{
			Loading = 0,
			Patchy = 1,
			None = 2
		}

		private delegate void ProcessStartingProgressState(Session session);

		private const string STARTING_PROGRESS = "starting_progress";

		private const string POLICY_BUTTON = "policy_button";

		private SoaringContext LOAD_GAME_CONTEXT;

		private SaveGameScreen saveGameScreen;

		private float elapsedProductInfoTime;

		private float elapsedPurchaseInfoTime;

		private int currentState = -1;

		private ProcessStartingProgressState[] processes;

		private float errorMessageScale = 0.85f;

		private float errorTitleScale = 0.45f;

		private int currentAdvance = 1;

		private SBGUIButton policyButton;

		private SBGUILabel policy_Label;

		private int precacheGUIState;

		private int loadTimeDependentsState;

		private StaticContentLoader contentLoader;

		private int performedMigration;

		private SBGUIElement loadingSpinner;

		private static int _CommunityEventIndex;

		private static CommunityEvent[] _CommunityEvents;

		private static Session _CommunityEventSession;

		private static bool didOpenUpdateDialog;

		private void OnGameCreated(Session session)
		{
			if (performedMigration == 3)
			{
				Action okHandler = delegate
				{
					TFUtils.GotoAppstore();
				};
				CreateErrorDialog(session, Language.Get("!!ERROR_ENCOUNTERED_NEWER_PROTOCOL_TITLE"), Language.Get("!!ERROR_ENCOUNTERED_NEWER_PROTOCOL_MESSAGE"), Language.Get("!!PREFAB_OK"), okHandler, errorMessageScale, errorTitleScale);
			}
			else
			{
				DeferDialogs(session);
			}
		}

		private void DeferDialogs(Session session)
		{
			Action action = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			AdvanceState(session);
		}

		public void OnLoadGameDelegate(SoaringContext context)
		{
			LOAD_GAME_CONTEXT = context;
		}

		private void LoadEntityBlueprints(Session session)
		{
			if (!contentLoader.LoadNextBlueprint())
			{
				CallLoadFromNetwork(session);
				AdvanceState(session);
			}
		}

		private void CallLoadFromNetwork(Session session, bool isRetryAttempt = false)
		{
			SoaringContext soaringContext = Game.CreateSoaringGameResponderContext(OnLoadGameDelegate);
			if (isRetryAttempt)
			{
				soaringContext.addValue(isRetryAttempt, "retry");
			}
			Game.LoadFromNetwork(session.ThePlayer.playerId, session.ThePlayer.ReadTimestamp(), soaringContext, session);
		}

		public static void ResetShowSplashScreen(SplashScreenState state)
		{
			YGTextureLibrary library = GUIMainView.GetInstance().Library;
			switch (state)
			{
			case SplashScreenState.None:
				library.UnloadAtlasResources("splash_bg_patchy");
				library.UnloadAtlasResources("splash_bg");
				if (!library.UnloadAtlasResources("localized_logo_en"))
				{
					library.UnloadAtlasResources(Language.LocalizedEnglishAssetName("localized_logo_en"));
				}
				break;
			case SplashScreenState.Loading:
				library.UnloadAtlasResources("splash_bg_patchy");
				library.LoadAtlasResources("splash_bg");
				library.LoadAtlasResources("localized_logo_en");
				break;
			case SplashScreenState.Patchy:
				library.LoadAtlasResources("splash_bg_patchy");
				library.UnloadAtlasResources("splash_bg");
				if (!library.UnloadAtlasResources("localized_logo_en"))
				{
					library.UnloadAtlasResources(Language.LocalizedEnglishAssetName("localized_logo_en"));
				}
				break;
			}
		}

		public static void UnloadSaveGameAtlas()
		{
			GUIMainView.GetInstance().Library.UnloadAtlasResources("SaveGameUI");
		}

		public static SBGUIScreen CreateLoadingScreen(Session session, bool makeLoadingBar = false, string starting_progress = "starting_progress", bool changeInitState = true)
		{
			bool flag = session.InFriendsGame || session.WasInFriendsGame;
			SplashScreenState splashScreenState = (flag ? SplashScreenState.Patchy : SplashScreenState.Loading);
			ResetShowSplashScreen(splashScreenState);
			Action privacyHandler = delegate
			{
				session.TheSoundEffectManager.PlaySound("Accept");
				Application.OpenURL(TFUtils.GetLegal_Address());
			};
			SBGUIScreen sBGUIScreen = SBUIBuilder.MakeAndPushStartingProgress(session, privacyHandler, HandleSBGUIEvent, makeLoadingBar, flag);
			if (changeInitState)
			{
				session.GameInitialized(false);
			}
			session.AddAsyncResponse(starting_progress, sBGUIScreen);
			SBGUIButton sBGUIButton = (SBGUIButton)sBGUIScreen.FindChild("privacy_policy");
			sBGUIButton.SetActive(makeLoadingBar);
			SBGUILabel sBGUILabel = (SBGUILabel)sBGUIButton.FindChild("privacy_policy_label");
			sBGUILabel.SetText(Language.Get("!!PRIVACY_POLICY"));
			session.AddAsyncResponse("policy_button", sBGUIButton);
			SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)sBGUIScreen.FindChild("reloading_window");
			SBGUIAtlasImage sBGUIAtlasImage2 = (SBGUIAtlasImage)sBGUIScreen.FindChild("logo");
			SBGUIAtlasImage sBGUIAtlasImage3 = (SBGUIAtlasImage)sBGUIScreen.FindChild("splash");
			SBGUILabel sBGUILabel2 = (SBGUILabel)sBGUIScreen.FindChild("playerID_label");
			sBGUILabel2.SetText(Soaring.Player.UserTag);
			sBGUILabel2.SetVisible(!flag && makeLoadingBar);
			if (sBGUIAtlasImage2 != null)
			{
				sBGUIAtlasImage2.SetVisible(sBGUILabel2.Visible);
			}
			if (makeLoadingBar && !flag)
			{
				Vector2 screenPosition = new Vector2(SBGUI.GetScreenWidth() * 0.15f, SBGUI.GetScreenHeight() * 0.05f);
				sBGUILabel2.SetScreenPosition(screenPosition);
			}
			if (splashScreenState == SplashScreenState.Patchy)
			{
				SBGUIAtlasImage sBGUIAtlasImage4 = (SBGUIAtlasImage)sBGUIScreen.FindChild("patchy_splash");
				SBGUILabel sBGUILabel3 = (SBGUILabel)sBGUIScreen.FindChild("destination_label");
				SBGUIProgressMeter sBGUIProgressMeter = (SBGUIProgressMeter)sBGUIScreen.FindChild("progress_meter");
				sBGUILabel3.SetActive(false);
				float num = 2f / GUIView.ResolutionScaleFactor();
				sBGUIAtlasImage4.Size = new Vector2(2048f, 1536f);
				Vector2 size = new Vector2(SBGUI.GetScreenWidth(), SBGUI.GetScreenHeight()) * num;
				Vector2 screenPosition2 = sBGUIAtlasImage4.GetScreenPosition();
				float num2 = size.x / sBGUIAtlasImage4.Size.x;
				float num3 = 0.24f;
				size.y = sBGUIAtlasImage4.Size.y * num2;
				float num4 = size.y - SBGUI.GetScreenHeight() * num;
				screenPosition2.y += num4 * num3;
				sBGUIAtlasImage4.SetScreenPosition(screenPosition2);
				sBGUIAtlasImage4.Size = size;
				sBGUIAtlasImage4.SetActive(flag);
				if (session.WasInFriendsGame)
				{
					sBGUILabel3.SetActive(flag && sBGUIProgressMeter.GetComponent<Renderer>().enabled);
					sBGUILabel3.SetText("           " + Language.Get("!!PATCHY_EXIT"));
				}
				if (session.InFriendsGame)
				{
					sBGUILabel3.SetActive(flag && sBGUIProgressMeter.GetComponent<Renderer>().enabled);
					sBGUILabel3.SetText("              " + Language.Get("!!PATCHY_ENTER"));
					AnalyticsWrapper.LogUIInteraction(session.TheGame, "ui_visit_patchy", "button", "tap");
				}
				sBGUIButton.SetActive(false);
				sBGUILabel.SetActive(false);
			}
			if (!flag)
			{
				sBGUIAtlasImage.SetActive(session.gameIsReloading);
			}
			session.camera.OnUpdate(0.0001f, session);
			return sBGUIScreen;
		}

		public void OnEnter(Session session)
		{
			session.canChangeState = false;
			session.InFriendsGame = false;
			session.gameIsReloading = false;
			LOAD_GAME_CONTEXT = null;
			_CommunityEventSession = null;
			if (!Game.GameExists(session.ThePlayer))
			{
				session.PlayHavenController.RequestContent("first_time_app_start");
			}
			else
			{
				session.PlayHavenController.RequestContent("app_start");
			}
			if (session.properties.playingHud != null)
			{
				session.properties.playingHud.Deactivate();
			}
			SBGUIScreen sBGUIScreen = CreateLoadingScreen(session, true);
			policyButton = (SBGUIButton)sBGUIScreen.FindChild("privacy_policy");
			policy_Label = (SBGUILabel)policyButton.FindChild("privacy_policy_label");
			loadingSpinner = sBGUIScreen.FindChild("loading_spinner");
			session.properties.playDelayCounter = 0;
			if (!Soaring.IsOnline)
			{
				SoaringInternal.instance.ClearOfflineMode();
			}
			precacheGUIState = 0;
			loadTimeDependentsState = 0;
			performedMigration = 0;
			int num = 0;
			processes = new ProcessStartingProgressState[21];
			processes[num++] = PatchContent;
			processes[num++] = AssembleGameState;
			processes[num++] = LoadEntityBlueprints;
			processes[num++] = CreateGame;
			processes[num++] = LoadAssets;
			processes[num++] = FetchProductInfo;
			processes[num++] = AwaitProductInfo;
			processes[num++] = FetchPurchaseInfo;
			processes[num++] = AwaitPurchaseInfo;
			processes[num++] = StartStore;
			processes[num++] = LoadLocalAssetsTerrain;
			processes[num++] = LoadLocalAssetsCreateSimulation;
			processes[num++] = PrecacheGUI;
			processes[num++] = LoadLocalAssetsLoadTimeDependents;
			processes[num++] = LoadLocalAssetsSendPendingCommands;
			processes[num++] = CreateTerrainMeshes;
			processes[num++] = LoadLocalAssetsActivateQuests;
			processes[num++] = ProcessTriggers;
			processes[num++] = SetupSimulation;
			processes[num++] = LoadSoaringCommunityEvents;
			processes[num++] = AnalyticsBookkeeping;
			SBGUIActivityIndicator sBGUIActivityIndicator = (SBGUIActivityIndicator)sBGUIScreen.FindChildSessionActionId("ActivityIndicator", false);
			TFUtils.Assert(sBGUIActivityIndicator != null, "ActivityIndicator expected to be valid.");
			sBGUIActivityIndicator.Center = new Vector3(4f, -2.7f, 3.2f);
			Application.targetFrameRate = 60;
			sBGUIActivityIndicator.StartActivityIndicator();
			currentState = -1;
			AdvanceState(session);
			if (!didRegisterNotifications)
			{
				try
				{
					RegisterForLocalNotifications();
				}
				catch (Exception ex)
				{
					SoaringDebug.Log("Failed to register user Notifications: " + ex.Message, LogType.Error);
				}
				didRegisterNotifications = true;
			}
		}

		public void OnLeave(Session session)
		{
			contentLoader = null;
			LOAD_GAME_CONTEXT = null;
			if (session.game != null)
			{
				session.game.CanSave = true;
			}
			SBGUIScreen sBGUIScreen = (SBGUIScreen)session.CheckAsyncRequest("starting_progress");
			if (sBGUIScreen != null)
			{
				SBGUIActivityIndicator sBGUIActivityIndicator = (SBGUIActivityIndicator)sBGUIScreen.FindChildSessionActionId("ActivityIndicator", false);
				TFUtils.Assert(sBGUIActivityIndicator != null, "ActivityIndicator expected to be valid.");
				sBGUIActivityIndicator.StopActivityIndicator();
			}
			if (!session.gameInitialized)
			{
				return;
			}
			NotificationManager.CancelAllNotifications();
			NotificationManager.AddAnnoyingNotifications(session.game);
			session.game.simulation.ClearPendingTimebarsInSimulateds();
			session.game.simulation.ClearPendingNamebarsInSimulateds();
			session.TheCamera.StartCamera();
			session.TheSoundEffectManager.StartSoundEffectsManager();
			session.musicManager.PlayTrack("InGame");
			SBUIBuilder.ReleaseTopScreen();
			sBGUIScreen = null;
			session.analytics.UpdateGameValues(session.game);
			AnalyticsWrapper.LogSessionBegin(session.game, 0uL);
			session.TheGame.analytics.LogSessionBegin();
			if (!session.game.featureManager.CheckFeature("unrestrict_clicks"))
			{
				RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
				RestrictInteraction.AddWhitelistExpansion(session.game.simulation, int.MinValue);
				RestrictInteraction.AddWhitelistSimulated(session.game.simulation, int.MinValue);
				session.game.tutorialLocked = true;
			}
			if (session.game.featureManager.CheckFeature("purchase_expansions"))
			{
				session.game.terrain.UpdateRealtySigns(session.game.entities.DisplayControllerManager, SBCamera.BillboardDefinition, session.game);
			}
			if (!session.game.tutorialLocked)
			{
				if (session.game.simulation.FindSimulated(PlayHavenController.PIRATE_BOOTY_SHIP_DID) == null && !session.game.inventory.HasItem(PlayHavenController.PIRATE_BOOTY_SHIP_DID))
				{
					session.PlayHavenController.RequestContent("loading_screen_end_existingplayer_no_ship");
				}
				else
				{
					session.PlayHavenController.RequestContent("loading_screen_end_existingplayer_with_ship");
				}
			}
			saveGameScreen = null;
		}

		public static void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void OnUpdate(Session session)
		{
			currentAdvance = 1;
			if (currentState == 21)
			{
				SoaringDictionary soaringDictionary = new SoaringDictionary();
				soaringDictionary.addValue(session.TheGame.resourceManager.PlayerLevelAmount, "level");
				ulong num = session.TheGame.FirstPlayTime();
				if (num != 0L)
				{
					soaringDictionary.addValue(num, "first_play_time");
				}
				soaringDictionary.addValue(SBSettings.BundleVersion, "client_version");
				Soaring.FireEvent("StartGame", soaringDictionary);
				CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
				if (activeEvent != null && (activeEvent.m_sID == CommunityEventManager._sSpongyGamesEventID || activeEvent.m_sID == CommunityEventManager._sSpongyGamesLastDayEventID) && (activeEvent.m_nQuestPrereqID < 0 || (session.TheGame.questManager.IsQuestCompleted((uint)activeEvent.m_nQuestPrereqID) && (session.TheGame.simulation.FindSimulated(CommunityEventManager._nColiseumDID) != null || session.TheGame.inventory.HasItem(CommunityEventManager._nColiseumDID)))))
				{
					Soaring.FireEvent("spongy_games_banner", null);
				}
				if (Soaring.Player.PrivateData_Safe != null)
				{
					SoaringArray soaringArray = (SoaringArray)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_completed_quest_key");
					if (soaringArray != null)
					{
						try
						{
							for (int i = 0; i < soaringArray.count(); i++)
							{
								uint did = (uint)(long)(SoaringValue)soaringArray.objectAtIndex(i);
								QuestManager questManager = session.TheGame.questManager;
								if (!questManager.IsQuestCompleted(did) && questManager.IsQuestActive(did))
								{
									questManager.CompleteQuest(questManager.GetQuest(did), session.TheGame);
								}
							}
							soaringArray.clear();
						}
						catch
						{
							SoaringDebug.Log("Failed To Apply Completed Quests: " + soaringArray.ToJsonString());
						}
					}
				}
				if (Soaring.Player.PrivateData_Safe != null)
				{
					SoaringDictionary soaringDictionary2 = (SoaringDictionary)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_friends_reward_key");
					if (soaringDictionary2 != null)
					{
						Dictionary<int, int> dictionary = new Dictionary<int, int>();
						string[] array = soaringDictionary2.allKeys();
						SoaringObjectBase[] array2 = soaringDictionary2.allValues();
						for (int j = 0; j < array.Length; j++)
						{
							if (string.Compare(array[j], "SBMI_friends_coinreward_key") == 0)
							{
								dictionary.Add(ResourceManager.SOFT_CURRENCY, (SoaringValue)array2[j]);
								ResourceManager.AddAmountToGameState(ResourceManager.SOFT_CURRENCY, (SoaringValue)array2[j], session.TheGame.gameState);
							}
							else if (string.Compare(array[j], "SBMI_friends_jellyreward_key") == 0)
							{
								dictionary.Add(ResourceManager.HARD_CURRENCY, (SoaringValue)array2[j]);
								ResourceManager.AddAmountToGameState(ResourceManager.HARD_CURRENCY, (SoaringValue)array2[j], session.TheGame.gameState);
							}
							else if (string.Compare(array[j], "SBMI_friends_xpreward_key") == 0)
							{
								dictionary.Add(ResourceManager.XP, (SoaringValue)array2[j]);
								ResourceManager.AddAmountToGameState(ResourceManager.XP, (SoaringValue)array2[j], session.TheGame.gameState);
							}
						}
						Reward reward = new Reward(dictionary, null, null, null, null, null, null, null, false, null);
						session.TheGame.ApplyReward(reward, TFUtils.EpochTime(), false);
						Soaring.Player.PrivateData_Safe.setValue(new SoaringDictionary(), "SBMI_friends_reward_key");
					}
				}
				SoaringContext soaringContext = new SoaringContext();
				soaringContext.Name = "DailyBonus";
				soaringContext.Responder = new SBMISoaring.SMBICacheDelegate();
				soaringContext.addValue(new SoaringObject(session), "session");
				SBMISoaring.RetrieveDailyBonuseCalendar(-1, soaringContext, DisplayDailyBonus);
				session.canChangeState = true;
				session.ChangeState("Playing");
				Dictionary<string, object> gameState = session.TheGame.gameState;
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)gameState["farm"];
				List<object> list = null;
				Simulation simulation = session.TheGame.simulation;
				if (dictionary2.ContainsKey("launch_dialogs_shown"))
				{
					list = (List<object>)dictionary2["launch_dialogs_shown"];
				}
				if ((list == null || !list.Contains("christmas_event_over_2013_dialog")) && simulation.FindSimulated(9042) != null)
				{
					if (list == null)
					{
						list = new List<object>();
					}
					list.Add("christmas_event_over_2013_dialog");
					Simulated simulated = simulation.FindSimulated(3049);
					ResidentEntity residentEntity = null;
					if (simulated != null)
					{
						residentEntity = simulated.GetEntity<ResidentEntity>();
					}
					uint sequenceId = 2105u;
					if (residentEntity != null && residentEntity.DisableFlee == true)
					{
						sequenceId = 2104u;
					}
					DialogPackage dialogPackage = session.TheGame.dialogPackageManager.GetDialogPackage(1u);
					List<DialogInputData> dialogInputsInSequence = dialogPackage.GetDialogInputsInSequence(sequenceId, null, null);
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, dialogInputsInSequence, sequenceId);
					session.ChangeState("ShowingDialog");
					if (dictionary2.ContainsKey("launch_dialogs_shown"))
					{
						dictionary2["launch_dialogs_shown"] = list;
					}
					else
					{
						dictionary2.Add("launch_dialogs_shown", list);
					}
					session.TheGame.CanSave = !session.WasInFriendsGame;
					session.TheGame.SaveLocally(0uL, false, false, true);
				}
				if (dictionary2.ContainsKey("recipes") && (activeEvent == null || (activeEvent.m_sID != CommunityEventManager._sSpongyGamesEventID && activeEvent.m_sID != CommunityEventManager._sSpongyGamesLastDayEventID)))
				{
					Simulated simulated2 = session.TheGame.simulation.FindSimulated(20400);
					Simulated simulated3 = session.TheGame.simulation.FindSimulated(20410);
					bool flag = false;
					if (simulated2 != null && simulated3 == null && !session.TheGame.inventory.HasItem(20410))
					{
						if (list == null)
						{
							list = new List<object>();
						}
						if (!list.Contains("spongy_games_event_over_2014_dialog"))
						{
							list.Add("spongy_games_event_over_2014_dialog");
							DialogPackage dialogPackage2 = session.TheGame.dialogPackageManager.GetDialogPackage(1u);
							if (dialogPackage2 != null)
							{
								List<DialogInputData> dialogInputsInSequence2 = dialogPackage2.GetDialogInputsInSequence(2306u, null, null);
								if (dialogInputsInSequence2 != null)
								{
									session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, dialogInputsInSequence2, 2306u);
									session.ChangeState("ShowingDialog");
									if (dictionary2.ContainsKey("launch_dialogs_shown"))
									{
										dictionary2["launch_dialogs_shown"] = list;
									}
									else
									{
										dictionary2.Add("launch_dialogs_shown", list);
									}
									flag = true;
								}
							}
						}
					}
					if (simulated2 != null)
					{
						int[] array3 = new int[6] { 9200, 9300, 9301, 9302, 9303, 9304 };
						int num2 = array3.Length;
						List<object> list2 = (List<object>)dictionary2["recipes"];
						for (int k = 0; k < num2; k++)
						{
							if (!session.TheGame.craftManager.IsRecipeUnlocked(array3[k]))
							{
								flag = true;
								session.TheGame.craftManager.UnlockRecipe(array3[k], session.TheGame);
								list2.Add(array3[k]);
							}
						}
					}
					if (flag)
					{
						session.TheGame.CanSave = !session.WasInFriendsGame;
						session.TheGame.SaveLocally(0uL, false, false, true);
					}
				}
				if ((activeEvent == null || activeEvent.m_sID != CommunityEventManager._sChrismas14EventID) && (list == null || !list.Contains("christmas_event_over_2014_dialog")))
				{
					bool flag2 = false;
					if (session.TheGame.questManager.IsQuestCompleted(2800u) && !session.TheGame.questManager.IsQuestCompleted(2842u))
					{
						if (list == null)
						{
							list = new List<object>();
						}
						list.Add("christmas_event_over_2014_dialog");
						flag2 = true;
						DialogPackage dialogPackage3 = session.TheGame.dialogPackageManager.GetDialogPackage(1u);
						List<DialogInputData> dialogInputsInSequence3 = dialogPackage3.GetDialogInputsInSequence(2848u, null, null);
						session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, dialogInputsInSequence3, 2848u);
						session.ChangeState("ShowingDialog");
					}
					else if (session.TheGame.questManager.IsQuestCompleted(2842u))
					{
						if (list == null)
						{
							list = new List<object>();
						}
						list.Add("christmas_event_over_2014_dialog");
						flag2 = true;
						DialogPackage dialogPackage4 = session.TheGame.dialogPackageManager.GetDialogPackage(1u);
						List<DialogInputData> dialogInputsInSequence4 = dialogPackage4.GetDialogInputsInSequence(2846u, null, null);
						session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, dialogInputsInSequence4, 2846u);
						session.ChangeState("ShowingDialog");
					}
					if (dictionary2.ContainsKey("launch_dialogs_shown"))
					{
						dictionary2["launch_dialogs_shown"] = list;
					}
					else
					{
						dictionary2.Add("launch_dialogs_shown", list);
					}
					if (flag2)
					{
						session.TheGame.CanSave = !session.WasInFriendsGame;
						session.TheGame.SaveLocally(0uL, false, false, true);
					}
				}
				if ((activeEvent != null && !(activeEvent.m_sID != CommunityEventManager._sValentines15EventID)) || (list != null && list.Contains("valentines_event_over_2015_dialog")))
				{
					return;
				}
				bool flag3 = false;
				if (session.TheGame.questManager.IsQuestCompleted(3100u) && !session.TheGame.questManager.IsQuestCompleted(3134u))
				{
					if (list == null)
					{
						list = new List<object>();
					}
					list.Add("valentines_event_over_2015_dialog");
					flag3 = true;
					DialogPackage dialogPackage5 = session.TheGame.dialogPackageManager.GetDialogPackage(1u);
					List<DialogInputData> dialogInputsInSequence5 = dialogPackage5.GetDialogInputsInSequence(3140u, null, null);
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, dialogInputsInSequence5, 3140u);
					session.ChangeState("ShowingDialog");
				}
				else if (session.TheGame.questManager.IsQuestCompleted(3134u))
				{
					if (list == null)
					{
						list = new List<object>();
					}
					list.Add("valentines_event_over_2015_dialog");
					flag3 = true;
					DialogPackage dialogPackage6 = session.TheGame.dialogPackageManager.GetDialogPackage(1u);
					List<DialogInputData> dialogInputsInSequence6 = dialogPackage6.GetDialogInputsInSequence(3138u, null, null);
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, dialogInputsInSequence6, 3138u);
					session.ChangeState("ShowingDialog");
				}
				if (dictionary2.ContainsKey("launch_dialogs_shown"))
				{
					dictionary2["launch_dialogs_shown"] = list;
				}
				else
				{
					dictionary2.Add("launch_dialogs_shown", list);
				}
				if (flag3)
				{
					session.TheGame.CanSave = !session.WasInFriendsGame;
					session.TheGame.SaveLocally(0uL, false, false, true);
				}
			}
			else
			{
				if (currentState == 22)
				{
					return;
				}
				SBGUIScreen sBGUIScreen = (SBGUIScreen)session.CheckAsyncRequest("starting_progress");
				session.AddAsyncResponse("starting_progress", sBGUIScreen);
				loadingSpinner.gameObject.transform.Rotate(new Vector3(0f, 0f, 1f), -10f * Time.deltaTime);
				float num3 = ((float)currentState + 1f) / 21f;
				if (currentState == 0)
				{
					num3 += 1f / 21f * SoaringInternal.instance.Versions.CurrentUpdateProgress();
				}
				sBGUIScreen.dynamicMeters["loading"].Progress = num3;
				sBGUIScreen.dynamicLabels["progress"].SetText(string.Format("{0}%", ((int)(100f * num3)).ToString()));
				try
				{
					processes[currentState](session);
				}
				catch (Exception ex)
				{
					if (session.haveReloaded)
					{
						TFUtils.DebugLog(ex.Message + " at: " + ex.StackTrace);
						SoaringDebug.Log("GAME MUST RELOAD CRITICAL: " + ex.Message + " at: " + ex.StackTrace, LogType.Error);
						int errorCode = TFError.GetErrorCode(ex, 200 + currentState);
						if (errorCode != 302)
						{
							CreateErrorDialog(session, Language.Get("!!ERROR_CORRUPTED_GAMESTATE_TITLE"), Language.Get("!!ERROR_CORRUPTED_GAMESTATE_MESSAGE") + "\n" + Soaring.Player.UserTag + "-" + errorCode, Language.Get("!!PREFAB_OK"), delegate
							{
								SBUIBuilder.ReleaseTopScreen();
							}, errorMessageScale, errorTitleScale);
						}
						else
						{
							CreateErrorDialog(session, Language.Get("!!ERROR_CORRUPTED_GAMESTATE_TITLE"), Language.Get("!!ERROR_302_MESSAGE") + " " + Soaring.Player.UserTag + "!", Language.Get("!!PREFAB_OK"), delegate
							{
								SBUIBuilder.ReleaseTopScreen();
							}, errorMessageScale, errorTitleScale);
						}
						currentState = 22;
						TFUtils.LogDump(session, "loading_error_" + errorCode, ex);
					}
					else
					{
						SoaringDebug.Log("GAME MUST RELOAD: " + ex.Message + " at: " + ex.StackTrace, LogType.Error);
						session.haveReloaded = true;
						if (session.game != null && session.game.actionBuffer != null)
						{
							session.game.actionBuffer.DestroyCache();
						}
						if (SoaringInternal.instance.Versions != null)
						{
							SoaringInternal.instance.Versions.ClearAllContent();
							TFUtils.RefreshSAFiles();
						}
						else if (Directory.Exists(TFUtils.GetPersistentAssetsPath()))
						{
							Directory.Delete(TFUtils.GetPersistentAssetsPath(), true);
						}
						session.canChangeState = true;
						session.ChangeState("Authorizing");
					}
				}
			}
		}

		private void DisplayDailyBonus(SoaringContext context)
		{
			Session session = null;
			SoaringError soaringError = null;
			bool flag = false;
			if (Soaring.IsOnline && context != null)
			{
				flag = context.soaringValue("query");
				if (flag)
				{
					SoaringObjectBase soaringObjectBase = context.objectWithKey("session");
					if (soaringObjectBase != null)
					{
						session = (Session)((SoaringObject)soaringObjectBase).Object;
					}
					else
					{
						flag = false;
					}
				}
			}
			if (context != null)
			{
				SoaringObjectBase soaringObjectBase2 = context.objectWithKey("error_code");
				if (soaringObjectBase2 != null)
				{
					soaringError = (SoaringError)soaringObjectBase2;
				}
			}
			if (flag)
			{
				DailyBonusDialogInputData dailyBonusDialogInputData = new DailyBonusDialogInputData();
				if (!dailyBonusDialogInputData.AlreadyCollected && session.TheGame != null && session.TheGame.dialogPackageManager != null)
				{
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData> { dailyBonusDialogInputData });
					session.ChangeState("ShowingDialog");
				}
			}
			else
			{
				int num = -1;
				if (soaringError != null)
				{
					num = soaringError.ErrorCode;
				}
				Debug.Log("TODO: HANDLE THE ERROR CODE: " + num);
			}
		}

		private void AdvanceState(Session session)
		{
			lock (this)
			{
				currentState += currentAdvance;
				currentAdvance = 0;
			}
			session.analytics.LogLoadingFunnelStep("LoadingScreen_" + currentState);
		}

		public void CRITICAL_ERROR_ALL_GAMES_CORRUPTED(Session session, Exception e)
		{
			LOAD_GAME_CONTEXT = null;
			Action okHandler = delegate
			{
				SBUIBuilder.ReleaseTopScreen();
			};
			int game_error_code = TFError.GetErrorCode(e, 250 + currentState);
			Action cancelHandler = delegate
			{
				SoaringPlatform.SendEmail(Soaring.Player.UserID + "-" + game_error_code, string.Empty, Language.Get("!!CUSTOMER_SERVICE_EMAIL"));
			};
			if (game_error_code != 302)
			{
				CreateErrorDialog(session, Language.Get("!!ERROR_CORRUPTED_GAMESTATE_TITLE"), WithErrorID(Language.Get("!!ERROR_CORRUPTED_GAMESTATE_MESSAGE") + "\n" + Soaring.Player.UserTag, game_error_code), Language.Get("!!PREFAB_OK"), okHandler, Language.Get("!!SEND_EMAIL"), cancelHandler, errorMessageScale, errorTitleScale);
			}
			else
			{
				CreateErrorDialog(session, Language.Get("!!ERROR_CORRUPTED_GAMESTATE_TITLE"), WithErrorID(Language.Get("!!ERROR_302_MESSAGE") + " " + Soaring.Player.UserTag + "!", game_error_code), Language.Get("!!PREFAB_OK"), okHandler, Language.Get("!!SEND_EMAIL"), cancelHandler, errorMessageScale, errorTitleScale);
			}
		}

		public string WithErrorID(string message, int errorID)
		{
			return message + " - " + errorID;
		}

		private bool CheckServerGameWithSession(Session session, bool canSave)
		{
			bool result = true;
			if (session.game != null)
			{
				session.game.ClearActionBuffer();
				bool canSave2 = session.game.CanSave;
				session.game.CanSave = canSave && !session.WasInFriendsGame;
				session.game.SaveLocally(0uL, false, false, true);
				session.TheGame.CanSave = canSave2;
			}
			else
			{
				session.player.DeleteTimestamp();
				result = false;
			}
			return result;
		}

		private void CreateGame(Session session)
		{
			if (LOAD_GAME_CONTEXT == null)
			{
				return;
			}
			bool flag = LOAD_GAME_CONTEXT.soaringValue("status");
			SoaringArray soaringArray = (SoaringArray)LOAD_GAME_CONTEXT.objectWithKey("custom");
			bool flag2 = LOAD_GAME_CONTEXT.soaringValue("retry");
			LOAD_GAME_CONTEXT = null;
			SoaringDictionary soaringDictionary = null;
			bool flag3 = false;
			if (Game.GameCacheExists(SoaringPlatform.DeviceID))
			{
				flag3 = true;
			}
			if (soaringArray != null && soaringArray.count() != 0)
			{
				soaringDictionary = (SoaringDictionary)soaringArray.objectAtIndex(0);
			}
			if (soaringDictionary != null)
			{
				Dictionary<string, object> gameServer = null;
				if (SBSettings.UseLegacyGameLoad)
				{
					TFUtils.DebugLog("Loading Game In Legacy Mode");
					gameServer = (Dictionary<string, object>)Json.Deserialize(soaringDictionary.ToJsonString());
				}
				else
				{
					gameServer = SBMISoaring.ConvertDictionaryToGeneric(soaringDictionary);
				}
				Game gameLocal = null;
				try
				{
					if (flag3)
					{
						try
						{
							string text = Game.GameCachePath(SoaringPlatform.DeviceID);
							string text2 = Game.GamePath(session.ThePlayer);
							string directoryName = Path.GetDirectoryName(text2);
							if (!Directory.Exists(directoryName))
							{
								Directory.CreateDirectory(directoryName);
							}
							File.Copy(text, text2, true);
							File.Delete(text);
						}
						catch (Exception ex)
						{
							Debug.LogError("Data Failed Copy Offline Game: " + ex.Message + "\n" + ex.StackTrace);
						}
					}
					gameLocal = Game.LoadFromCache(session.ThePlayer, session.Analytics, contentLoader, out performedMigration, session.PlayHavenController);
				}
				catch (Exception ex2)
				{
					Debug.Log("Data Failed To Load Local Game: " + ex2.Message);
					gameLocal = null;
				}
				bool flag4 = session.InFriendsGame || session.WasInFriendsGame;
				if (gameLocal != null && !flag4)
				{
					TFUtils.GameDetails details = null;
					TFUtils.GameDetails details2 = null;
					TFUtils.DebugLog("ServerGame: " + TFUtils.ParseGameDetails(gameServer, ref details));
					TFUtils.DebugLog("LocalGame: " + TFUtils.ParseGameDetails(gameLocal.gameState, ref details2));
					if (details.lastPlayTime == details2.lastPlayTime)
					{
						Debug.Log("Games are Identical, Use Local");
						session.game = gameLocal;
						session.player.SaveStagedTimestamp();
						OnGameCreated(session);
						return;
					}
					Action server = delegate
					{
						saveGameScreen.SetActive(false);
						Action okButtonHandler = delegate
						{
							SBUIBuilder.ReleaseTopScreen();
							saveGameScreen.Deactivate();
							try
							{
								session.game = Game.LoadFromDataDict(gameServer, session.Analytics, session.ThePlayer, contentLoader, out performedMigration, session.PlayHavenController);
								CheckServerGameWithSession(session, true);
							}
							catch (Exception ex8)
							{
								Debug.Log("Data Failed To Load: Using ServerGame: " + ex8.Message);
							}
							policyButton.SetActive(true);
							policy_Label.SetText(Language.Get("!!PRIVACY_POLICY"));
							OnGameCreated(session);
						};
						Action cancelButtonHandler = delegate
						{
							SBUIBuilder.ReleaseTopScreen();
							saveGameScreen.SetActive(true);
						};
						string text5 = null;
						string text6 = "||Level:{0} Jelly:{1} Money:{2}";
						text5 = Language.Get("!!SAVE_ALERT_REPLACE_1");
						SBGUIConfirmationDialog sBGUIConfirmationDialog = SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!SAVE_GAME_TITLE"), text5, Language.Get("!!SAVE_GAME_YES"), Language.Get("!!SAVE_GAME_NO"), null, okButtonHandler, cancelButtonHandler);
						sBGUIConfirmationDialog.FindChild("title_label").SetActive(false);
						sBGUIConfirmationDialog.FindChild("message_label").transform.localScale *= 1.5f;
						Vector3 localPosition = sBGUIConfirmationDialog.FindChild("message_label").transform.localPosition;
						sBGUIConfirmationDialog.FindChild("message_label").transform.localPosition = new Vector3(localPosition.x, localPosition.y - 0.5f, localPosition.z);
						sBGUIConfirmationDialog.transform.Find("window").Find("titlebackground").gameObject.SetActive(false);
					};
					Action local = delegate
					{
						saveGameScreen.SetActive(false);
						Action okButtonHandler = delegate
						{
							SBUIBuilder.ReleaseTopScreen();
							saveGameScreen.Deactivate();
							if (gameLocal != null)
							{
								session.game = gameLocal;
							}
							else
							{
								try
								{
									session.game = Game.LoadFromCache(session.ThePlayer, session.Analytics, contentLoader, out performedMigration, session.PlayHavenController);
								}
								catch (Exception ex8)
								{
									Debug.LogError("Data Failed To Load: Using ServerGame: " + ex8.Message);
								}
							}
							policyButton.SetActive(true);
							policy_Label.SetText(Language.Get("!!PRIVACY_POLICY"));
							OnGameCreated(session);
						};
						Action cancelButtonHandler = delegate
						{
							SBUIBuilder.ReleaseTopScreen();
							saveGameScreen.SetActive(true);
						};
						string text5 = null;
						string text6 = "||Level:{0} Jelly:{1} Money:{2}";
						text5 = Language.Get("!!SAVE_ALERT_REPLACE_2");
						SBGUIConfirmationDialog sBGUIConfirmationDialog = SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!SAVE_GAME_TITLE"), text5, Language.Get("!!SAVE_GAME_YES"), Language.Get("!!SAVE_GAME_NO"), null, okButtonHandler, cancelButtonHandler);
						sBGUIConfirmationDialog.FindChild("title_label").SetActive(false);
						sBGUIConfirmationDialog.FindChild("message_label").transform.localScale *= 1.5f;
						Vector3 localPosition = sBGUIConfirmationDialog.FindChild("message_label").transform.localPosition;
						sBGUIConfirmationDialog.FindChild("message_label").transform.localPosition = new Vector3(localPosition.x, localPosition.y - 0.5f, localPosition.z);
						sBGUIConfirmationDialog.transform.Find("window").Find("titlebackground").gameObject.SetActive(false);
					};
					if (saveGameScreen == null)
					{
						session.player.DeleteTimestamp();
						saveGameScreen = (SaveGameScreen)SBGUI.InstantiatePrefab("Prefabs/SaveGame");
						saveGameScreen.SetUp(TFUtils.GetPlayerName(Soaring.Player, "{0} ") + Language.Get("!!SAVE_GAME_TEXT1"), Language.Get("!!SAVE_GAME_TEXT2"), "                    " + Language.Get("!!SAVE_GAME_ALERT1"), Language.Get("!!SAVE_GAME_SAVE_ON_SERVER"), details.level, details.money, details.jelly, details.patties, details.dtLastPlayTime, Language.Get("!!SAVE_GAME_KEEP_THIS_GAME"), Language.Get("!!SAVE_GAME_SAVE_ON_DEVICE"), details2.level, details2.money, details2.jelly, details2.patties, details2.dtLastPlayTime, Language.Get("!!SAVE_GAME_KEEP_THIS_GAME"), server, local, session);
						policyButton.SetActive(false);
					}
					return;
				}
				try
				{
					session.game = Game.LoadFromDataDict(gameServer, session.Analytics, session.ThePlayer, contentLoader, out performedMigration, session.PlayHavenController);
					CheckServerGameWithSession(session, true);
					OnGameCreated(session);
					return;
				}
				catch (Exception ex3)
				{
					Debug.LogError("Data Failed To Load: Using ServerGame: " + ex3.Message + "\n" + ex3.StackTrace);
					CRITICAL_ERROR_ALL_GAMES_CORRUPTED(session, ex3);
					TFUtils.LogDump(session, "save_error", ex3);
					return;
				}
			}
			bool flag5 = false;
			Exception ex4 = null;
			if (Game.GameExists(session.ThePlayer))
			{
				TFUtils.DebugLog("Creating game from local file");
				try
				{
					session.game = Game.LoadFromCache(session.ThePlayer, session.Analytics, contentLoader, out performedMigration, session.PlayHavenController);
					OnGameCreated(session);
				}
				catch (Exception ex5)
				{
					TFUtils.DebugLog(ex5);
					if (Soaring.IsOnline)
					{
						session.player.DeleteTimestamp();
						if (flag2)
						{
							flag5 = true;
						}
						else
						{
							CallLoadFromNetwork(session, true);
						}
					}
					else
					{
						flag5 = true;
					}
					ex4 = ex5;
				}
			}
			else if (flag || Soaring.Player.IsLocalAuthorized)
			{
				TFUtils.WarningLog("No Save Game Found");
				if (!Player.ValidTimeStamp(session.player.ReadTimestamp()))
				{
					bool flag6 = false;
					if (flag3)
					{
						try
						{
							string text3 = Game.GameCachePath(SoaringPlatform.DeviceID);
							string text4 = Game.GamePath(session.ThePlayer);
							string directoryName2 = Path.GetDirectoryName(text4);
							if (!Directory.Exists(directoryName2))
							{
								Directory.CreateDirectory(directoryName2);
							}
							File.Copy(text3, text4, true);
							File.Delete(text3);
						}
						catch (Exception ex6)
						{
							Debug.LogError("Data Failed Copy Offline Game: " + ex6.Message + "\n" + ex6.StackTrace);
						}
						try
						{
							session.game = Game.LoadFromCache(session.ThePlayer, session.Analytics, contentLoader, out performedMigration, session.PlayHavenController);
							OnGameCreated(session);
							flag6 = true;
						}
						catch (Exception ex7)
						{
							Debug.LogError("Data Failed Load Copied Offline Game: " + ex7.Message + "\n" + ex7.StackTrace);
						}
					}
					if (!flag6)
					{
						session.game = Game.CreateNew(session.analytics, session.player, contentLoader, out performedMigration, session.PlayHavenController);
						OnGameCreated(session);
					}
				}
				else
				{
					session.player.DeleteTimestamp();
					if (flag2)
					{
						flag5 = true;
					}
					else
					{
						CallLoadFromNetwork(session, true);
					}
				}
			}
			else if (!Soaring.IsOnline)
			{
				TFUtils.WarningLog("Local Authorized: " + Soaring.Player.IsLocalAuthorized);
				TFUtils.DebugLog("Critical Game Is Offling, No Local Data");
				flag5 = true;
			}
			else
			{
				TFUtils.DebugLog("Critical Error No Games to Load, How did we get here!!!!");
			}
			if (flag5)
			{
				Action okHandler = delegate
				{
					SBUIBuilder.ReleaseTopScreen();
					CallLoadFromNetwork(session, true);
				};
				int num = 302;
				if (ex4 != null)
				{
					num = TFError.GetErrorCode(ex4, num);
				}
				if (num != 302)
				{
					CreateErrorDialog(session, Language.Get("!!ERROR_CORRUPTED_GAMESTATE_TITLE"), WithErrorID(Language.Get("!!ERROR_CORRUPTED_GAMESTATE_MESSAGE") + "\n" + Soaring.Player.UserTag, num), Language.Get("!!PREFAB_OK"), okHandler, errorMessageScale, errorTitleScale);
				}
				else
				{
					CreateErrorDialog(session, Language.Get("!!ERROR_CORRUPTED_GAMESTATE_TITLE"), WithErrorID(Language.Get("!!ERROR_302_MESSAGE") + " " + Soaring.Player.UserTag + "!", num), Language.Get("!!PREFAB_OK"), okHandler, errorMessageScale, errorTitleScale);
				}
			}
		}

		private void LoadSoaringCommunityEvents(Session session)
		{
			if (_CommunityEventSession == null)
			{
				_CommunityEventSession = session;
				_CommunityEventIndex = 0;
				_CommunityEvents = session.game.communityEventManager.GetEvents();
				int num = _CommunityEvents.Length;
				if (num <= 0)
				{
					AdvanceState(session);
					return;
				}
				CommunityEvent communityEvent = _CommunityEvents[_CommunityEventIndex];
				SoaringCommunityEventManager.SetValueFinished += HandleSetValueFinished;
				SBMISoaring.SetEventValue(session, int.Parse(communityEvent.m_sID), session.TheGame.resourceManager.Resources[communityEvent.m_nValueID].Amount);
			}
		}

		private void HandleSetValueFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
		{
			int num = 0;
			if (_CommunityEvents != null)
			{
				num = _CommunityEvents.Length;
			}
			_CommunityEventIndex++;
			if (_CommunityEventIndex < num)
			{
				CommunityEvent communityEvent = _CommunityEvents[_CommunityEventIndex];
				SBMISoaring.SetEventValue(_CommunityEventSession, int.Parse(communityEvent.m_sID), _CommunityEventSession.TheGame.resourceManager.Resources[communityEvent.m_nValueID].Amount);
				return;
			}
			SoaringCommunityEventManager.SetValueFinished -= HandleSetValueFinished;
			_CommunityEventIndex = 0;
			_CommunityEvents = null;
			AdvanceState(_CommunityEventSession);
			_CommunityEventSession = null;
		}

		private bool dataIsChange(string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local)
		{
			return level_server != level_local || money_server != money_local || jelly_server != jelly_local || patty_server != patty_local || timeStamp_server != timeStamp_local;
		}

		private void LoadAssets(Session session)
		{
			TFUtils.Assert(session.game != null, "SessionGameStarting.LoadAssets() expects session.game to not be null");
			contentLoader.TheEntityManager.LoadBlueprintResources();
			bool flag = false;
			if (session.game.store == null)
			{
				flag = true;
			}
			else if (!session.game.store.receivedProductInfo)
			{
				flag = true;
				session.game.store.Reset(session);
				session.game.store = null;
			}
			if (flag)
			{
				currentState = 5;
			}
			else
			{
				currentState = 17;
			}
		}

		private void CreateTerrainMeshes(Session session)
		{
			session.game.terrain.CreateTerrainMeshes();
			AdvanceState(session);
		}

		private void FetchProductInfo(Session session)
		{
			session.game.store = RmtStore.LoadFromFilesystem(true);
			session.game.store.Init(session);
			if (!RmtStore.PreloadRmtProducts(session))
			{
				session.game.store.rmtEnabled = false;
			}
			session.PlayHavenController.Initialize(session);
			AdvanceState(session);
		}

		private void AwaitProductInfo(Session session)
		{
			if (session.TheGame.store.receivedProductInfo)
			{
				AdvanceState(session);
				return;
			}
			if (!session.game.store.rmtEnabled)
			{
				TFUtils.DebugLog("Skipping process product info, since premium is not supported");
				AdvanceState(session);
				return;
			}
			TFUtils.DebugLogTimed("Waiting for product info");
			elapsedProductInfoTime += Time.deltaTime;
			if (elapsedProductInfoTime > 15f || !Soaring.IsOnline)
			{
				TFUtils.WarningLog("Timed out on store request. Disabling store");
				AdvanceState(session);
			}
		}

		private void ProcessTriggers(Session session)
		{
			session.game.dropManager.ExecuteAllPickupTriggers(session.game);
			AdvanceState(session);
		}

		private void SetupSimulation(Session session)
		{
			if (!session.gameInitialized)
			{
				Resources.UnloadUnusedAssets();
				UnloadSaveGameAtlas();
				session.GameInitialized(true);
				session.PlayHavenController.RequestContent("loading_screen_end");
				session.game.simulation.OnUpdate(session);
				session.game.treasureManager.StartTreasureTimers();
			}
			else if (!session.game.needsNetworkDownErrorDialog)
			{
				AdvanceState(session);
			}
		}

		private void AnalyticsBookkeeping(Session session)
		{
			session.game.playtimeRegistrar.UpdatePlaytime(TFUtils.EpochTime());
			session.analytics.LogStartedPlaying(session.game.resourceManager.PlayerLevelAmount);
			uint item = 2000u;
			if (session.TheGame.questManager.CompletedQuestDids.Contains(item))
			{
				session.analytics.LogEligiblePromoEvent(session.game.resourceManager.PlayerLevelAmount, "2013_Halloween");
			}
			if (Application.platform == RuntimePlatform.Android)
			{
				Screen.sleepTimeout = -2;
			}
			AdvanceState(session);
		}

		private void PatchContent(Session session)
		{
			if (!session.IsPatchingInProgress())
			{
				AdvanceState(session);
			}
		}

		public void PatchingEventListener(string patchingEvent, Session session)
		{
			if (currentState == 0)
			{
				if ("patchingDone" == patchingEvent)
				{
					TFUtils.DebugLog("Patching is done. Proceeding.");
					AdvanceState(session);
				}
				else
				{
					TFUtils.DebugLog("Patching is in progress with status " + patchingEvent);
				}
			}
		}

		private void AssembleGameState(Session session)
		{
			SBSettings.Init();
			SBUIBuilder.ClearScreenCache();
			if (Language.CurrentLanguage() == LanguageCode.N)
			{
				Language.Init(TFUtils.GetPersistentAssetsPath());
			}
			if (!didOpenUpdateDialog)
			{
				if (SBSettings.LOCAL_BUNDLE_VERSION.Major < SBSettings.CURRENT_APPSTORE_BUNDLE_VERSION.Major || (SBSettings.LOCAL_BUNDLE_VERSION.Major == SBSettings.CURRENT_APPSTORE_BUNDLE_VERSION.Major && SBSettings.LOCAL_BUNDLE_VERSION.Minor < SBSettings.CURRENT_APPSTORE_BUNDLE_VERSION.Minor))
				{
					didOpenUpdateDialog = true;
					TFUtils.DebugLog("Will force app update");
					Action okHandler = delegate
					{
						TFUtils.GotoAppstore();
					};
					CreateErrorDialog(session, Language.Get("!!NEW_APP_AVAILABLE_TITLE"), Language.Get("!!NEW_APP_FORCE_UPGRADE_MESSAGE"), Language.Get("!!PREFAB_OK"), okHandler, 1f, 0.5f);
				}
				else if (SBSettings.LOCAL_BUNDLE_VERSION.Major == SBSettings.CURRENT_APPSTORE_BUNDLE_VERSION.Major && SBSettings.LOCAL_BUNDLE_VERSION.Minor == SBSettings.CURRENT_APPSTORE_BUNDLE_VERSION.Minor && SBSettings.LOCAL_BUNDLE_VERSION.Build < SBSettings.CURRENT_APPSTORE_BUNDLE_VERSION.Build)
				{
					Version lastPromptedAppstoreVersion = SBSettings.LastPromptedAppstoreVersion;
					if (!lastPromptedAppstoreVersion.Equals(SBSettings.CURRENT_APPSTORE_BUNDLE_VERSION))
					{
						didOpenUpdateDialog = true;
						Action okButtonHandler = delegate
						{
							SBSettings.SaveLastPromptedAppstoreVersion();
							SBUIBuilder.ReleaseTopScreen();
							TFUtils.GotoAppstore();
							didOpenUpdateDialog = false;
						};
						Action cancelButtonHandler = delegate
						{
							SBSettings.SaveLastPromptedAppstoreVersion();
							SBUIBuilder.ReleaseTopScreen();
							didOpenUpdateDialog = false;
						};
						TFUtils.DebugLog("Will suggest app update");
						SBGUIConfirmationDialog sBGUIConfirmationDialog = SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!NEW_APP_AVAILABLE_TITLE"), Language.Get("!!NEW_APP_SUGGEST_UPGRADE_MESSAGE"), Language.Get("!!PREFAB_OK"), Language.Get("!!PREFAB_CANCEL"), new Dictionary<string, int>(), okButtonHandler, cancelButtonHandler);
						sBGUIConfirmationDialog.tform.parent = GUIMainView.GetInstance().transform;
						sBGUIConfirmationDialog.tform.localPosition = Vector3.zero;
					}
				}
				SBGUIButton sBGUIButton = (SBGUIButton)session.CheckAsyncRequest("policy_button");
				if (sBGUIButton != null)
				{
					SBGUILabel sBGUILabel = (SBGUILabel)sBGUIButton.FindChild("privacy_policy_label");
					sBGUILabel.SetText(Language.Get("!!PRIVACY_POLICY"));
					float num = 4.5f;
					((SBGUIImage)sBGUIButton.FindChild("hyperlink_image")).Size = new Vector2(((float)sBGUILabel.Text.Length + 1f) * num, 2f);
					if (!session.InFriendsGame && !session.WasInFriendsGame)
					{
						sBGUIButton.SetActive(true);
					}
				}
			}
			if (!didOpenUpdateDialog)
			{
				contentLoader = new StaticContentLoader();
				contentLoader.LoadContent(session);
				session.DropGame();
				AdvanceState(session);
			}
		}

		private void FetchPurchaseInfo(Session session)
		{
			TFUtils.DebugLog("FetchPurchaseInfo");
			session.game.store.GetPurchases(session);
			AdvanceState(session);
		}

		private void AwaitPurchaseInfo(Session session)
		{
			if (session.game.store.receivedPurchaseInfo)
			{
				AdvanceState(session);
				return;
			}
			elapsedPurchaseInfoTime += Time.deltaTime;
			if (elapsedPurchaseInfoTime > 15f || !Soaring.IsOnline)
			{
				TFUtils.WarningLog("Timed out on store request.");
				AdvanceState(session);
			}
		}

		private void StartStore(Session session)
		{
			session.game.store.Start();
			AdvanceState(session);
		}

		private void LoadLocalAssetsTerrain(Session session)
		{
			contentLoader.Initialize();
			AdvanceState(session);
		}

		private void LoadLocalAssetsCreateSimulation(Session session)
		{
			TFUtils.DebugLog("Creating simulation");
			Action<Simulated> rushSimulated = delegate(Simulated sim)
			{
				session.game.selected = sim;
				session.ChangeState("SelectedPlaying", false);
				session.ChangeState("HardSpendConfirm", false);
			};
			session.game.simulation = new Simulation(session.game.ModifyGameState, session.game.ModifyGameStateSimulated, rushSimulated, session.game.actionBuffer.Record, session.game, session.game.entities, session.game.triggerRouter, session.game.resourceManager, session.game.dropManager, session.TheSoundEffectManager, session.game.resourceCalculatorManager, session.game.craftManager, session.game.movieManager, session.game.featureManager, session.game.catalog, session.game.rewardCap, session.camera.UnityCamera, session.game.terrain, 5, session.analytics, session.simulationScratchScreen, contentLoader.TheEnclosureManager);
			AdvanceState(session);
		}

		private void PrecacheGUI(Session session)
		{
			TFUtils.DebugLogTimed("In PrecacheGUI");
			if (precacheGUIState == 0)
			{
				SBGUIRewardWidget.MakeRewardWidgetPool();
				precacheGUIState++;
			}
			else
			{
				AdvanceState(session);
			}
		}

		private void LoadLocalAssetsLoadTimeDependents(Session session)
		{
			if (loggingTimedDependents)
			{
				TFUtils.DebugLogTimed("In LoadLocalAssetsLoadTimeDependents. State=" + loadTimeDependentsState);
			}
			ulong utcNow = TFUtils.EpochTime();
			switch (loadTimeDependentsState)
			{
			case 0:
				session.game.LoadSimulation(utcNow);
				loadTimeDependentsState++;
				return;
			case 1:
				if (session.game.IterateLoadSimulation())
				{
					loadTimeDependentsState++;
				}
				return;
			case 2:
				session.game.LoadExpansions(utcNow);
				loadTimeDependentsState++;
				return;
			case 3:
				if (session.game.IterateLoadExpansions())
				{
					loadTimeDependentsState++;
				}
				return;
			}
			bool canSave = session.game.CanSave;
			if (!session.WasInFriendsGame && !session.InFriendsGame)
			{
				session.game.CanSave = true;
			}
			session.game.SaveToServer(session, TFUtils.EpochTime(), true, performedMigration == 2);
			session.game.CanSave = canSave;
			AdvanceState(session);
		}

		private void LoadLocalAssetsSendPendingCommands(Session session)
		{
			session.game.simulation.SendPendingCommands();
			AdvanceState(session);
		}

		private void LoadLocalAssetsActivateQuests(Session session)
		{
			TFUtils.DebugLog("GAME INITIALIZED");
			session.game.questManager.Activate(session.game);
			if (session.game.dialogPackageManager.GetNumQueuedDialogInputs() > 0)
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			}
			AdvanceState(session);
		}

		private void CreateErrorDialog(Session session, string title, string message, string okButtonLabel, Action okHandler, float messageScale, float titleScale)
		{
			CreateErrorDialog(session, title, message, okButtonLabel, okHandler, null, null, messageScale, titleScale);
		}

		private void CreateErrorDialog(Session session, string title, string message, string okButtonLabel, Action okHandler, string cancelButtonLabel, Action cancelHandler, float messageScale, float titleScale)
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
			SBGUIConfirmationDialog sBGUIConfirmationDialog = SBUIBuilder.MakeAndPushConfirmationDialog(session, null, title, message, okButtonLabel, cancelButtonLabel, null, okButtonHandler, cancelButtonHandler);
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

	public class GameStopping : State
	{
		public void OnEnter(Session session)
		{
			session.game = null;
			UnityGameResources.Reset();
			session.ChangeState("Authorizing");
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}
	}

	public class GetJelly : State
	{
		private const string GET_JELLY = "GetJelly";

		public void OnEnter(Session session)
		{
			Action action = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			string title = (string)session.CheckAsyncRequest("jelly_message_title");
			string message = (string)session.CheckAsyncRequest("jelly_message");
			string question = (string)session.CheckAsyncRequest("jelly_question");
			string acceptLabel = (string)session.CheckAsyncRequest("jelly_message_ok_label");
			string cancelLabel = (string)session.CheckAsyncRequest("jelly_message_cancel_label");
			Action okButtonHandler = (Action)session.CheckAsyncRequest("jelly_message_ok_action");
			Action cancelButtonHandler = (Action)session.CheckAsyncRequest("jelly_message_cancel_action");
			SBGUIGetJellyDialog sBGUIGetJellyDialog = SBUIBuilder.MakeAndPushGetJellyDialog(session, HandleSBGUIEvent, title, message, question, acceptLabel, cancelLabel, null, okButtonHandler, cancelButtonHandler, true);
			sBGUIGetJellyDialog.tform.parent = GUIMainView.GetInstance().transform;
			sBGUIGetJellyDialog.tform.localPosition = Vector3.zero;
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	public class HardSpendConfirm : State
	{
		private const string HARD_SPEND_CONFIRM_HANDLER = "hard_spend_confirm_ui";

		public void OnEnter(Session session)
		{
			TFUtils.Assert(session.properties.hardSpendActions != null, "You need to first set the hardSpendActions session property before trying to rush!");
			Action action = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			Action acceptHandler = delegate
			{
				session.ChangeState("HardSpendPassthrough", false);
			};
			Action denyStep1 = session.properties.hardSpendActions.cancel;
			if (denyStep1 == null)
			{
				denyStep1 = delegate
				{
					session.ChangeState("Playing");
				};
			}
			session.properties.denialActions = delegate
			{
				denyStep1();
				session.analytics.LogPlayerRejectHardSpend(session.properties.hardSpendActions.cost(TFUtils.EpochTime()).ResourceAmounts[ResourceManager.HARD_CURRENCY], session.game.resourceManager.PlayerLevelAmount);
				session.properties.cleanUp = delegate
				{
					HardSpendPassthrough.ClearSpendProperties(session);
				};
			};
			Vector2 screenPosition = session.properties.hardSpendActions.screenPosition;
			session.properties.microConfirmDialog = SBUIBuilder.MakeAndPushJjMicroConfirmDialog(session, null, Language.Get("!!PREFAB_CONFIRM_PURCHASE_SHORT"), session.properties.hardSpendActions.cost, acceptHandler, session.properties.denialActions, screenPosition);
			session.game.sessionActionManager.SetActionHandler("hard_spend_confirm_ui", session, new List<SBGUIScreen> { session.properties.microConfirmDialog }, SessionActionUiHelper.HandleCommonSessionActions);
			SessionActionSimulationHelper.EnableHandler(session, true);
			Cost cost = session.properties.hardSpendActions.cost(TFUtils.EpochTime());
			Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(cost.ResourceAmounts, session.game.resourceManager);
			if (resourcesStillRequired.Count > 0)
			{
				session.properties.hardSpendActions.logSpend(false, cost);
				session.analytics.LogPlayerConfirmHardSpend(cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], false, session.game.resourceManager.PlayerLevelAmount);
				session.TheSoundEffectManager.PlaySound("Error");
			}
			else
			{
				session.TheSoundEffectManager.PlaySound("HighlightItem");
			}
		}

		public void OnLeave(Session session)
		{
			SBUIBuilder.ReleaseTopScreen();
			if (session.properties.cleanUp != null)
			{
				session.properties.cleanUp();
				session.properties.cleanUp = null;
			}
			session.properties.microConfirmDialog = null;
			session.properties.denialActions = null;
			session.game.sessionActionManager.ClearActionHandler("hard_spend_confirm_ui", session);
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
			if (session.properties.hardSpendActions != null)
			{
				TFUtils.Assert(session.properties.hardSpendActions.complete != null, "Must set the hard spend complete action before attempting to rush!");
				Cost cost = session.properties.hardSpendActions.cost(TFUtils.EpochTime());
				int num = cost.ResourceAmounts[ResourceManager.HARD_CURRENCY];
				session.properties.microConfirmDialog.SetHardAmount(num);
				if (num <= 0)
				{
					session.properties.denialActions();
				}
			}
			else
			{
				session.properties.denialActions();
			}
		}
	}

	public class HardSpendPassthrough : State
	{
		public void OnEnter(Session session)
		{
			ulong time = TFUtils.EpochTime();
			if (session.properties.hardSpendActions == null)
			{
				TFUtils.DebugLog("we should not be rushing without a cost");
				session.ChangeState("Playing");
				return;
			}
			Cost cost = session.properties.hardSpendActions.cost(time);
			Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(cost.ResourceAmounts, session.game.resourceManager);
			if (resourcesStillRequired.Count > 0)
			{
				session.InsufficientResourcesHandler(session, session.properties.hardSpendActions.subjectText, session.properties.hardSpendActions.subjectDID, session.properties.hardSpendActions.complete, session.properties.hardSpendActions.cancel, cost);
				return;
			}
			Action action = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			TFUtils.Assert(session.properties.hardSpendActions != null, "You must set the hardSpendActions before trying to rush something!");
			TFUtils.Assert(session.properties.hardSpendActions.logSpend != null, "You must set the hardSpendActions.log Action before trying to hard spend on something!");
			if (session.properties.hardSpendActions == null)
			{
				session.ChangeState("Playing");
				return;
			}
			if (cost == null || cost.ResourceAmounts.Count < 1)
			{
				TFUtils.ErrorLog("Could not figure out rush cost. Something has gone wrong.");
				session.properties.hardSpendActions.cancel();
				return;
			}
			session.properties.hardSpendActions.logSpend(true, cost);
			session.analytics.LogPlayerConfirmHardSpend(cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], true, session.game.resourceManager.PlayerLevelAmount);
			session.TheSoundEffectManager.PlaySound("Rush");
			session.properties.hardSpendActions.execute();
			session.properties.hardSpendActions.complete();
		}

		public void OnLeave(Session session)
		{
			ClearSpendProperties(session);
		}

		public void OnUpdate(Session session)
		{
		}

		public static void ClearSpendProperties(Session session)
		{
			session.properties.overrideSimulatedToRush = null;
			session.properties.hardSpendActions = null;
		}
	}

	public class HardSpendActions
	{
		public Cost.CostAtTime cost;

		public string subjectText;

		public int subjectDID;

		public Action execute;

		public Action complete;

		public Action cancel;

		public Action<bool, Cost> logSpend;

		public Vector2 screenPosition;

		public HardSpendActions(Action execute, Cost.CostAtTime cost, string subjectText, int subjectDID, Action complete, Action<bool, Cost> logSpend, Vector2 screenPosition)
			: this(execute, cost, subjectText, subjectDID, complete, complete, logSpend, screenPosition)
		{
		}

		public HardSpendActions(Action execute, Cost.CostAtTime cost, string subjectText, int subjectDID, Action complete, Action cancel, Action<bool, Cost> logSpend, Vector2 screenPosition)
		{
			this.cost = cost;
			this.subjectText = subjectText;
			this.execute = execute;
			this.complete = complete;
			this.cancel = cancel;
			this.logSpend = logSpend;
			this.screenPosition = screenPosition;
			this.subjectDID = subjectDID;
		}
	}

	public class InAppPurchasing : State
	{
		private bool receivedProduct;

		private bool receivedError;

		private bool canceledTransaction;

		private float elapsedTime;

		private string errorTitle;

		private string errorMessage;

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
			if (receivedError)
			{
				Action okAction = delegate
				{
					session.ChangeState("Playing");
				};
				TFUtils.DebugLog("Error out on store request");
				session.game.analytics.LogFailInAppPurchase(session.properties.iapBundleName, session.game.resourceManager.PlayerLevelAmount);
				session.ErrorMessageHandler(session, errorTitle, errorMessage, Language.Get("!!PREFAB_OK"), okAction);
				return;
			}
			if (canceledTransaction)
			{
				session.game.analytics.LogCancelInAppPurchase(session.properties.iapBundleName, session.game.resourceManager.PlayerLevelAmount);
				session.ChangeState("Playing");
				return;
			}
			if (receivedProduct)
			{
				session.game.analytics.LogCompleteInAppPurchase(session.properties.iapBundleName, session.game.resourceManager.PlayerLevelAmount);
				session.ChangeState("Playing");
				return;
			}
			if (elapsedTime > 15f)
			{
				Action okAction2 = delegate
				{
					session.ChangeState("Playing");
				};
				TFUtils.DebugLog("Timed out on store request");
				session.ErrorMessageHandler(session, Language.Get("!!NOTIFY_STORE_TIMEOUT_TITLE"), Language.Get("!!NOTIFY_STORE_TIMEOUT_MESSAGE"), Language.Get("!!PREFAB_OK"), okAction2);
				return;
			}
			string title = Language.Get("!!NOTIFY_STORE_WAITING_TITLE");
			string message = Language.Get("!!NOTIFY_STORE_WAITING_MESSAGE");
			string acceptLabel = Language.Get("!!PREFAB_OK");
			Action okButtonHandler = delegate
			{
				session.ChangeState("Playing");
			};
			SBUIBuilder.MakeAndPushConfirmationDialog(session, HandleSBGUIEvent, title, message, acceptLabel, null, null, okButtonHandler, null);
			elapsedTime += Time.deltaTime;
			TFUtils.DebugLogTimed("Awaiting purchase completion");
		}

		public void OnEnter(Session session)
		{
			session.camera.SetEnableUserInput(false);
			session.game.analytics.LogRequestInAppPurchase(session.properties.iapBundleName, session.game.resourceManager.PlayerLevelAmount);
			elapsedTime = 0f;
			receivedProduct = false;
			receivedError = false;
			canceledTransaction = false;
			session.game.store.PurchaseUpdateReceived += OnPurchaseUpdate;
			session.game.store.PurchaseResponseReceived += OnPurchaseResponse;
			session.game.store.PurchaseError += OnPurchaseError;
			session.game.store.StartRmtPurchase(session);
		}

		public void OnLeave(Session session)
		{
			session.properties.iapBundleName = null;
			session.game.store.PurchaseUpdateReceived -= OnPurchaseUpdate;
			session.game.store.PurchaseResponseReceived -= OnPurchaseResponse;
			session.game.store.PurchaseError -= OnPurchaseError;
			session.camera.SetEnableUserInput(true);
		}

		public void OnPurchaseUpdate(object sender, RmtStore.StoreEventArgs args)
		{
			elapsedTime = 0f;
		}

		public void OnPurchaseResponse(object sender, RmtStore.StoreEventArgs args)
		{
			if (!TFServer.IsNetworkError(args.results))
			{
				receivedProduct = true;
			}
		}

		public static void OnPurchaseDefered(object sender, RmtStore.StoreEventArgs args)
		{
		}

		public void OnPurchaseError(object sender, RmtStore.StoreEventArgs args)
		{
			object value;
			object value2;
			object value3;
			if (args.results.TryGetValue("state", out value) && args.results.TryGetValue("reason", out value2))
			{
				string text = (string)value;
				string text2 = (string)value2;
				if (text2 == "userCancelled")
				{
					TFUtils.DebugLog("User canceled purchase");
					canceledTransaction = true;
					return;
				}
				if (text == "failed")
				{
					errorTitle = Language.Get("!!ERROR_STORE_FAILED_TITLE");
					errorMessage = (string)args.results["description"];
				}
			}
			else if (TFServer.IsNetworkError(args.results))
			{
				errorTitle = Language.Get("!!ERROR_STORE_NETWORK_TITLE");
				errorMessage = Language.Get("!!ERROR_STORE_NETWORK_MESSAGE");
			}
			else if (args.results.TryGetValue("purchase", out value3))
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)value3;
				errorTitle = Language.Get("!!ERROR_STORE_UNCONFIRMED_TITLE");
				errorMessage = Language.Get("!!ERROR_STORE_UNCONFIRMED_MESSAGE");
			}
			receivedError = true;
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	public class InsufficientDialog : State
	{
		public void OnEnter(Session session)
		{
			Setup(session);
			session.TheCamera.SetEnableUserInput(false);
		}

		public void Setup(Session session)
		{
			Action okAction = (Action)session.CheckAsyncRequest("insufficient_accept");
			Action cancelAction = (Action)session.CheckAsyncRequest("insufficient_cancel");
			Cost cost = (Cost)session.CheckAsyncRequest("insufficient_resources");
			string purchaseName = (string)session.CheckAsyncRequest("insufficient_itemname");
			int purchaseDID = (int)session.CheckAsyncRequest("insufficient_item_did");
			if (cost == null)
			{
				TFUtils.DebugLog("Error occurred, we are failing out of the insufficient dialog due to a missing cost");
				session.ChangeState("Playing");
				return;
			}
			Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(cost.ResourceAmounts, session.game.resourceManager);
			TFUtils.Assert(resourcesStillRequired.Count > 0, "Error occurred, we have enough resources to apply cost.");
			Cost resourcesToPurchase = Cost.GetResourcesToPurchase(cost.ResourceAmounts, session.game.resourceManager);
			TFUtils.Assert(cost.ResourceAmounts.Count > 0, "Error occurred, we appear to have enough resources to apply cost.");
			int jjCost = session.game.resourceManager.GetResourcesPackageCostInHardCurrencyValue(resourcesToPurchase);
			Action pNewCancelAction = null;
			if (resourcesToPurchase.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY))
			{
				Action okAction2 = delegate
				{
					PrepForStoreUI(session, "rmt");
					session.AddAsyncResponse("store_open_type", "store_open_need_currency_redirect");
					cancelAction();
					session.ChangeState("Shopping");
				};
				Action cancelAction2 = delegate
				{
					cancelAction();
				};
				session.GetJellyHandler(session, Language.Get("!!PREFAB_GET") + " " + Language.Get(session.TheGame.resourceManager.Resources[ResourceManager.HARD_CURRENCY].Name), Language.Get("!!PREFAB_NO_JELLY"), Language.Get("!!PREFAB_WANT_JELLY"), Language.Get("!!PREFAB_BUY_JELLY"), Language.Get("!!PREFAB_CANCEL"), okAction2, cancelAction2);
				return;
			}
			Action action = null;
			string acceptLabel;
			if (session.game.resourceManager.HasEnough(ResourceManager.HARD_CURRENCY, jjCost))
			{
				acceptLabel = Language.Get("!!PREFAB_OK");
				Action confirmAction = delegate
				{
					session.analytics.LogInsufficientDialog(purchaseName, jjCost, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
					session.game.resourceManager.PurchaseResourcesWithHardCurrency(jjCost, resourcesToPurchase, session.game);
					session.game.simulation.ModifyGameState(new PurchaseResourcesAction(new Identity(), jjCost, resourcesToPurchase));
					session.TheSoundEffectManager.PlaySound("Purchase");
					string sItemName = "Not_enough_money_" + purchaseName;
					if (session.TheGame.resourceManager.Resources.ContainsKey(purchaseDID) || session.TheGame.craftManager.ContainsRecipe(purchaseDID))
					{
						AnalyticsWrapper.LogJellyConfirmation(session.TheGame, purchaseDID, jjCost, sItemName, "craft", "instant_purchase", string.Empty, "confirm");
					}
					else
					{
						AnalyticsWrapper.LogJellyConfirmation(session.TheGame, purchaseDID, jjCost, sItemName, "buildings", "instant_purchase", string.Empty, "confirm");
					}
				};
				Action<bool, Cost> log = delegate
				{
				};
				pNewCancelAction = delegate
				{
					cancelAction();
				};
				action = delegate
				{
					session.properties.hardSpendActions = new HardSpendActions(confirmAction, (ulong time) => new Cost(new Dictionary<int, int> { 
					{
						ResourceManager.HARD_CURRENCY,
						jjCost
					} }), string.Empty, purchaseDID, okAction, pNewCancelAction, log, (!(session.properties.insufficientDialog == null)) ? session.properties.insufficientDialog.GetHardSpendPosition() : session.TheCamera.ScreenCenter);
					session.ChangeState("HardSpendConfirm", false);
				};
			}
			else
			{
				acceptLabel = Language.Get("!!PREFAB_OK");
				Action internalOk = delegate
				{
					TFUtils.Assert(false, "We don't support RMT Store functions yet!");
					session.game.resourceManager.PurchaseResourcesWithHardCurrency(jjCost, resourcesToPurchase, session.game);
					session.game.simulation.ModifyGameState(new PurchaseResourcesAction(new Identity(), jjCost, resourcesToPurchase));
					session.TheSoundEffectManager.PlaySound("Purchase");
					string sItemName = "Not_enough_money_" + purchaseName;
					if (session.TheGame.buildingUnlockManager.CheckBuildingUnlock(purchaseDID))
					{
						AnalyticsWrapper.LogJellyConfirmation(session.TheGame, purchaseDID, jjCost, sItemName, "buildings", "instant_purchase", string.Empty, "confirm");
					}
					else if (session.TheGame.resourceManager.Resources.ContainsKey(purchaseDID) || (session.TheGame.craftManager.ContainsRecipe(purchaseDID) && !session.TheGame.buildingUnlockManager.CheckBuildingUnlock(purchaseDID)))
					{
						AnalyticsWrapper.LogJellyConfirmation(session.TheGame, purchaseDID, jjCost, sItemName, "craft", "instant_purchase", string.Empty, "confirm");
					}
					else
					{
						AnalyticsWrapper.LogJellyConfirmation(session.TheGame, purchaseDID, jjCost, sItemName, "buildings", "instant_purchase", string.Empty, "confirm");
					}
					okAction();
				};
				pNewCancelAction = delegate
				{
					cancelAction();
				};
				action = delegate
				{
					session.InsufficientResourcesHandler(session, purchaseName, purchaseDID, internalOk, pNewCancelAction, new Cost(new Dictionary<int, int> { 
					{
						ResourceManager.HARD_CURRENCY,
						jjCost
					} }));
				};
			}
			Action cancelButtonHandler = delegate
			{
				session.TheSoundEffectManager.PlaySound("CloseMenu");
				if (pNewCancelAction != null)
				{
					pNewCancelAction();
				}
				else
				{
					cancelAction();
				}
			};
			session.properties.insufficientDialog = SBUIBuilder.MakeAndPushInsufficientResourcesDialog(session, cost.ResourceAmounts, resourcesStillRequired, jjCost, session.game.resourceManager.Resources[ResourceManager.HARD_CURRENCY].GetResourceTexture(), acceptLabel, action, cancelButtonHandler);
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
		}

		private void PrepForStoreUI(Session session, string tabToOpen)
		{
			session.AddAsyncResponse("target_store_tab", tabToOpen);
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	public class Inventory : State
	{
		public const string FROM_INVENTORY = "FromInventory";

		public const string ASSOCIATED_ENTITIES = "AssociatedEntities";

		private const string INVENTORY_UI_HANDLER = "inventory_ui";

		public void OnEnter(Session session)
		{
			session.game.dropManager.MarkForClearCurrentDrops();
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			session.PlayBubbleScreenSwipeEffect();
			session.camera.SetEnableUserInput(false);
			session.TheCamera.TurnOnScreenBuffer();
			Action action = delegate
			{
				AndroidBack.getInstance().pop();
				bool? flag = (bool?)session.CheckAsyncRequest("FromEdit");
				if (flag.HasValue && flag.Value)
				{
					session.ChangeState("Editing");
				}
				else
				{
					session.ChangeState("Playing");
				}
			};
			Action shopClickHandler = delegate
			{
				session.AddAsyncResponse("store_open_type", "store_open_button");
				session.ChangeState("Shopping");
			};
			Action optionsHandler = delegate
			{
				session.ChangeState("Options");
			};
			Action editClickHandler = delegate
			{
				session.ChangeState("Editing");
			};
			Action<SBInventoryItem> buildingClickHandler = delegate(SBInventoryItem sBInventoryItem)
			{
				TFUtils.DebugLog("inventory clicked: " + sBInventoryItem.ToString());
				Identity id = sBInventoryItem.entity.Id;
				List<KeyValuePair<int, Identity>> outAssociatedEntities;
				Entity entity = session.game.inventory.RemoveEntity(id, out outAssociatedEntities);
				Ray ray = session.camera.ScreenPointToRay(session.camera.ScreenCenter);
				Vector3 point;
				session.game.terrain.ComputeIntersection(ray, out point);
				Simulated simulated = session.game.simulation.CreateSimulated(entity, EntityManager.BuildingActions["replacing"], new Vector2(point.x, point.y));
				simulated.Warp(simulated.Position);
				session.game.selected = simulated;
				session.game.selected.Visible = true;
				session.AddAsyncResponse("FromInventory", true);
				session.AddAsyncResponse("AssociatedEntities", outAssociatedEntities);
				session.ChangeState("MoveBuildingInPlacement");
			};
			Action<SBInventoryItem> inventoryClickHandler = delegate(SBInventoryItem sBInventoryItem)
			{
				string movieFileName = sBInventoryItem.movieFileName;
				session.PlayMovie(movieFileName, "Inventory");
			};
			Action openIAPTabHandlerSoft = delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping");
			};
			Action openIAPTabHandlerHard = delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping");
			};
			session.properties.inventoryHud = SBUIBuilder.MakeAndPushStandardUI(session, false, HandleSBGUIEvent, shopClickHandler, action, optionsHandler, editClickHandler, null, DragFeeding.SwitchToFn(session), null, openIAPTabHandlerSoft, openIAPTabHandlerHard, null, null);
			session.properties.inventoryHud.SetVisibleNonEssentialElements(false, true);
			SBGUIButton sBGUIButton = (SBGUIButton)session.properties.inventoryHud.FindChild("inventory");
			sBGUIButton.SetActive(true);
			SBGUIButton sBGUIButton2 = (SBGUIButton)session.properties.inventoryHud.FindChild("marketplace");
			sBGUIButton2.SetActive(false);
			SBGUIButton sBGUIButton3 = (SBGUIButton)session.properties.inventoryHud.FindChild("community_event");
			sBGUIButton3.SetActive(false);
			SBGUIInventoryScreen item = SBUIBuilder.MakeAndPushInventoryDialog(session, session.game.entities, session.TheSoundEffectManager, action, buildingClickHandler, inventoryClickHandler);
			Action action2 = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action2;
			session.game.questManager.OnShowDialogCallback = action2;
			session.game.communityEventManager.DialogNeededCallback = action2;
			session.game.sessionActionManager.SetActionHandler("inventory_ui", session, new List<SBGUIScreen>
			{
				session.properties.inventoryHud,
				item
			}, SessionActionUiHelper.HandleCommonSessionActions);
			SessionActionSimulationHelper.EnableHandler(session, true);
		}

		public void OnLeave(Session session)
		{
			session.TheSoundEffectManager.PlaySound("CloseMenu");
			session.game.sessionActionManager.ClearActionHandler("inventory_ui", session);
			session.properties.inventoryHud.SetVisibleNonEssentialElements(true);
			session.properties.inventoryHud = null;
			session.TheCamera.TurnOffScreenBuffer();
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}
	}

	public abstract class MoveBuilding : State
	{
		public const string OVERRIDE_DRAG = "override_drag";

		public const string PANNING_EVENT = "panning_event";

		public const string SILENT_ENTER = "silent_enter";

		protected const string BLOCKING_SIMULATEDS = "blocking_sims";

		private const string MOVEDRAGGING_UI_HANDLER = "movedragging_ui";

		protected InteractionStripMixin interactionStrip = new InteractionStripMixin();

		protected bool? savedFlippedState;

		public virtual void OnEnter(Session session)
		{
			TFUtils.DebugLog("Selected building did=" + session.game.selected.entity.DefinitionId);
			TFUtils.Assert(session.game.selected != null, "How is there no selected building!?");
			if (session.CheckAsyncRequest("silent_enter") == null)
			{
				session.TheSoundEffectManager.PlaySound("EditModePickup");
			}
			session.TheCamera.SetEnableUserInput(false);
			List<Simulated> list = (List<Simulated>)session.CheckAsyncRequest("blocking_sims");
			if (list != null)
			{
				foreach (Simulated item in list)
				{
					item.ClearBlockerHighlight();
				}
			}
			if (!session.properties.preMovePositionSet)
			{
				session.properties.preMovePosition = session.game.selected.Position;
				session.properties.preMoveFlip = session.game.selected.Flip;
				session.properties.preMovePositionSet = true;
			}
			Action action = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			SBGUIEvent sBGUIEvent = (SBGUIEvent)session.CheckAsyncRequest("CurrentGuiEventInfo");
			if (sBGUIEvent != null)
			{
				HandleSBGUIEvent(sBGUIEvent, session);
			}
			session.game.sessionActionManager.SetActionHandler("movedragging_ui", session, new List<SBGUIScreen> { session.properties.editingHud }, SessionActionUiHelper.HandleCommonSessionActions);
		}

		public virtual void OnLeave(Session session)
		{
			if (!session.properties.waitToDecidePlacement)
			{
				DecideForSelectedBuilding(session);
			}
			session.properties.waitToDecidePlacement = false;
			CleanupMovementVisuals(session);
			session.game.sessionActionManager.ClearActionHandler("movedragging_ui", session);
		}

		protected void DecideForSelectedBuilding(Session session)
		{
			if (session.game.selected != null)
			{
				if (IsValidLocationForSelected(session))
				{
					AcceptPlacement(session);
				}
				else
				{
					DenyPlacement(session);
				}
			}
		}

		protected abstract void HandleSBGUIEvent(SBGUIEvent evt, Session session);

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
			ColorSelectedByOccupation(session);
			interactionStrip.OnUpdate(session);
		}

		protected void SnapSelectedToInputPosition(Session session, Vector2 position, bool snapObject, bool updatePaths = false)
		{
			if (session.game.selected == null)
			{
				TFUtils.ErrorLog("SessionMoveBuilding.SnapSelectedToInputPosition - selected is null");
				return;
			}
			Simulated selected = session.game.selected;
			Ray ray = session.TheCamera.ScreenPointToRay(position);
			Vector3 point;
			session.game.terrain.ComputeIntersection(ray, out point);
			point.x -= selected.Box.Width * 0.5f;
			point.y -= selected.Box.Height * 0.5f;
			Vector2 vector = session.game.terrain.CalculateNearestGridPosition(point, selected.Footprint);
			if (snapObject && !selected.GetEntity<StructureDecorator>().ShareableSpace)
			{
				if (!TheDebugManager.debugPlaceObjects)
				{
					point = vector;
				}
			}
			else if (snapObject && !selected.GetEntity<StructureDecorator>().ShareableSpaceSnap)
			{
				if (!TheDebugManager.debugPlaceObjects)
				{
					point = vector;
				}
			}
			else
			{
				point = session.game.terrain.ConstrainToAlignedBox(point, selected.Footprint);
			}
			if (updatePaths)
			{
				selected.Warp(new Vector2(point.x, point.y), session.game.simulation);
			}
			else
			{
				selected.Warp(new Vector2(point.x, point.y));
			}
			Simulated worker = getWorker(selected, session.game.simulation);
			if (worker != null)
			{
				if (worker.GetDisplayState() == "work")
				{
					worker.Warp(selected.PointOfInterest);
				}
				else if (worker.GetDisplayState() != "idle")
				{
					session.game.simulation.Router.Send(CompleteCommand.Create(worker.Id, worker.Id));
				}
			}
			if (!TheDebugManager.debugPlaceObjects)
			{
				selected.SnapPosition = vector;
			}
		}

		protected bool IsValidLocationForSelected(Session session)
		{
			return TheDebugManager.debugPlaceObjects || session.game.simulation.PlacementQuery(session.game.selected) != Simulation.Placement.RESULT.INVALID;
		}

		protected virtual void AcceptPlacement(Session session)
		{
			TFUtils.Assert(IsValidLocationForSelected(session), "Invalid placement! Why is accept being called?");
			ResetMoveDecorationsOnSelected(session);
			session.properties.cameFromMarketplace = false;
			Simulated selected = session.game.selected;
			Terrain terrain = ((session.game.simulation == null) ? null : session.game.simulation.Terrain);
			bool flag = selected.HasEntity<StructureDecorator>() && selected.GetEntity<StructureDecorator>().IsObstacle;
			Vector2 offset = session.properties.preMovePosition - selected.Position;
			AlignedBox alignedBox = selected.Box.OffsetByVector(offset);
			if (terrain != null && alignedBox.xmin >= 0f && flag)
			{
				terrain.SetOrClearObstacle(alignedBox, false);
				terrain.SetOrClearObstacle(selected.Box, true);
				session.game.simulation.ResetAllAffectedPaths(selected.Box);
			}
			Simulated worker = getWorker(session.game.selected, session.game.simulation);
			if (worker != null && worker.GetDisplayState() == "idle")
			{
				session.game.simulation.Router.Send(MoveCommand.Create(session.game.selected.Id, worker.Id, session.game.selected.PointOfInterest, session.game.selected.Flip));
			}
		}

		protected void DenyPlacement(Session session)
		{
			TFUtils.Assert(session.game.selected != null, "Selected simulated cannot be null.");
			session.TheSoundEffectManager.PlaySound("Cancel");
			ResetMoveDecorationsOnSelected(session);
			if (session.properties.cameFromMarketplace)
			{
				session.game.simulation.RemoveSimulated(session.game.selected);
				session.game.entities.Destroy(session.game.selected.Id);
				session.game.selected = null;
				session.properties.waitToDecidePlacement = true;
				session.properties.preMovePositionSet = false;
				session.properties.cameFromMarketplace = false;
				session.ChangeState("Shopping");
				return;
			}
			if (WasFromInventory(session))
			{
				session.game.simulation.RemoveSimulated(session.game.selected);
				List<KeyValuePair<int, Identity>> associatedEntities = (List<KeyValuePair<int, Identity>>)session.CheckAsyncRequest("AssociatedEntities");
				session.game.inventory.AddItem(session.game.selected.GetEntity<BuildingEntity>(), associatedEntities);
				session.CheckAsyncRequest("FromInventory");
				session.game.selected = null;
				session.properties.waitToDecidePlacement = true;
				session.properties.preMovePositionSet = false;
				session.ChangeState("Inventory");
				return;
			}
			session.game.selected.Warp(session.properties.preMovePosition);
			if (session.game.selected.Flip != session.properties.preMoveFlip)
			{
				session.game.selected.Flip = session.properties.preMoveFlip;
				session.game.selected.FlipWarp(session.game.simulation);
			}
			Simulated worker = getWorker(session.game.selected, session.game.simulation);
			if (worker != null)
			{
				if (worker.GetDisplayState() == "work")
				{
					worker.Warp(session.game.selected.PointOfInterest);
				}
				else if (worker.GetDisplayState() == "idle")
				{
					session.game.simulation.Router.Send(MoveCommand.Create(session.game.selected.Id, worker.Id, session.game.selected.PointOfInterest, session.game.selected.Flip));
				}
			}
			session.game.selected.Animate(session.game.simulation);
		}

		protected void ResetMoveDecorationsOnSelected(Session session)
		{
			session.game.selected.Alpha = 1f;
			session.game.selected.FootprintVisible = false;
		}

		protected Simulated getWorker(Simulated buildingSim, Simulation simulation)
		{
			if (buildingSim.HasEntity<ErectableDecorator>())
			{
				ErectableDecorator entity = buildingSim.GetEntity<ErectableDecorator>();
				ulong? erectionCompleteTime = entity.ErectionCompleteTime;
				if (erectionCompleteTime.HasValue && erectionCompleteTime.Value > TFUtils.EpochTime() && buildingSim.Variable.ContainsKey("employee"))
				{
					Identity identity = buildingSim.Variable["employee"] as Identity;
					if (identity != null)
					{
						Simulated simulated = simulation.FindSimulated(identity);
						if (simulated != null)
						{
							return simulated;
						}
					}
				}
			}
			return null;
		}

		protected void ResetPlacement(Session session)
		{
			session.game.selected.Warp(session.game.selected.Position, session.game.simulation);
			ResetMoveDecorationsOnSelected(session);
		}

		private bool CheckFlag(Session session, string flagKey)
		{
			bool? flag = (bool?)session.CheckAsyncRequest(flagKey);
			if (flag.HasValue)
			{
				session.AddAsyncResponse(flagKey, flag.Value);
			}
			return flag.HasValue && flag.Value;
		}

		protected bool WasFromInventory(Session session)
		{
			return CheckFlag(session, "FromInventory");
		}

		protected bool WasFromEdit(Session session)
		{
			return CheckFlag(session, "FromEdit");
		}

		protected void ColorSelectedByOccupation(Session session)
		{
			UnmarkBlockers(session);
			if (session.game.selected != null)
			{
				Simulated selected = session.game.selected;
				selected.Alpha = 0.5f;
				AlignedBox box = selected.SnapBox;
				if (selected.HasEntity<StructureDecorator>() && selected.GetEntity<StructureDecorator>().ShareableSpace)
				{
					box = selected.Box;
				}
				List<Simulated> collisions = new List<Simulated>();
				if (TheDebugManager.debugPlaceObjects || session.game.simulation.PlacementQuery(box, ref collisions, session.game.selected.Id) != Simulation.Placement.RESULT.INVALID)
				{
					selected.FootprintColor = Simulated.COLOR_FOOTPRINT_FREE;
				}
				else
				{
					collisions.Remove(selected);
					session.AddAsyncResponse("blocking_sims", collisions);
					MarkBlockers(session);
					selected.FootprintColor = Simulated.COLOR_FOOTPRINT_BLOCKED;
				}
				selected.FootprintVisible = true;
			}
		}

		protected void MarkBlockers(Session session, bool persist = true)
		{
			List<Simulated> list = (List<Simulated>)session.CheckAsyncRequest("blocking_sims");
			if (list != null)
			{
				foreach (Simulated item in list)
				{
					item.BlockerHighlight();
				}
			}
			if (persist)
			{
				session.AddAsyncResponse("blocking_sims", list);
			}
		}

		protected void UnmarkBlockers(Session session, bool persist = false)
		{
			List<Simulated> list = (List<Simulated>)session.CheckAsyncRequest("blocking_sims");
			if (list != null)
			{
				foreach (Simulated item in list)
				{
					item.ClearBlockerHighlight();
				}
			}
			if (persist)
			{
				session.AddAsyncResponse("blocking_sims", list);
			}
		}

		protected void CleanupMovementVisuals(Session session)
		{
			interactionStrip.Deactivate(session);
			UnmarkBlockers(session);
		}

		protected void AdornMovementVisuals(Session session)
		{
			if (session.game.selected.InteractionState.Controls != null && session.game.selected.InteractionState.Controls.Count > 0)
			{
				interactionStrip.ActivateOnSelected(session);
				interactionStrip.OnUpdate(session);
			}
		}

		protected void UpdateMovementBookkeeping(Session session)
		{
			if (IsValidLocationForSelected(session))
			{
				session.properties.preMovePosition = session.game.selected.Position;
				session.properties.preMoveFlip = session.game.selected.Flip;
			}
			else
			{
				TFUtils.ErrorLog("You shouldn't be trying to update movement bookkeping into an invalid location!");
			}
		}
	}

	public class MoveBuildingInEdit : MoveBuilding
	{
		private bool m_bTouchBegan;

		private bool userCameraActive;

		private Simulated clickedSim;

		public override void OnEnter(Session session)
		{
			session.properties.firstEntered = true;
			m_bTouchBegan = false;
			Action inventoryClickHandler = delegate
			{
				if (!session.game.featureManager.CheckFeature("inventory_soft"))
				{
					session.game.featureManager.UnlockFeature("inventory_soft");
					session.game.featureManager.ActivateFeatureActions(session.game, "inventory_soft");
					session.game.simulation.ModifyGameState(new FeatureUnlocksAction(new List<string> { "inventory_soft" }));
				}
				else
				{
					session.AddAsyncResponse("FromEdit", true);
					session.ChangeState("Inventory");
				}
			};
			Action optionsHandler = delegate
			{
				session.ChangeState("Options");
			};
			Action editClickHandler = delegate
			{
				session.ChangeState("Playing");
			};
			Action openIAPTabHandlerSoft = delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping");
			};
			Action openIAPTabHandlerHard = delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping");
			};
			session.properties.editingHud = SBUIBuilder.MakeAndPushStandardUI(session, true, HandleSBGUIEvent, null, inventoryClickHandler, optionsHandler, editClickHandler, delegate
			{
				session.ChangeState("Paving");
			}, null, null, openIAPTabHandlerSoft, openIAPTabHandlerHard, null, null, true);
			((SBGUIStandardScreen)session.properties.editingHud).EnableFullHiding(false);
			SBGUIPulseButton sBGUIPulseButton = (SBGUIPulseButton)session.properties.editingHud.FindChild("community_event");
			sBGUIPulseButton.SetActive(false);
			AdornMovementVisuals(session);
			UpdateMovementBookkeeping(session);
			session.properties.startedTouchOnEmptySpace = false;
			LoadInteractionStrip(session);
			if (!session.game.simulation.featureManager.CheckFeature("move_reject_lock"))
			{
				interactionStrip.EnableRejectButton(session, false);
			}
			else
			{
				interactionStrip.EnableRejectButton(session, true);
			}
			base.OnEnter(session);
		}

		public void Update()
		{
		}

		public override void OnLeave(Session session)
		{
			((SBGUIStandardScreen)session.properties.editingHud).EnableFullHiding(true);
			base.OnLeave(session);
		}

		public void LoadInteractionStrip(Session session)
		{
			Action<Session> handler = delegate(Session sesh)
			{
				AcceptPlacement(sesh);
				session.properties.waitToDecidePlacement = true;
				session.properties.preMovePositionSet = false;
				sesh.ChangeState("Playing");
			};
			Action<Session> handler2 = delegate(Session sesh)
			{
				AcceptPlacement(sesh);
				session.properties.waitToDecidePlacement = true;
				session.properties.preMovePositionSet = false;
				sesh.ChangeState("Editing");
			};
			Action<Session> handler3 = delegate(Session sesh)
			{
				DenyPlacement(sesh);
				session.properties.waitToDecidePlacement = true;
				session.properties.preMovePositionSet = false;
				sesh.ChangeState("Editing");
			};
			Action<Session> handler4 = delegate(Session sesh)
			{
				DenyPlacement(sesh);
				session.properties.waitToDecidePlacement = true;
				session.properties.preMovePositionSet = false;
				sesh.ChangeState("Playing");
			};
			if (WasFromInventory(session) || session.properties.cameFromMarketplace)
			{
				interactionStrip.SetAcceptHandler(session, handler);
				interactionStrip.SetRejectHandler(session, handler4);
			}
			else
			{
				interactionStrip.SetAcceptHandler(session, handler2);
				interactionStrip.SetRejectHandler(session, handler3);
			}
		}

		protected override void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			object obj = null;
			switch (evt.type)
			{
			case YGEvent.TYPE.TOUCH_BEGIN:
			{
				session.properties.isDraggingBuildingAndScreen = false;
				session.properties.isInteractionStripActive = true;
				m_bTouchBegan = true;
				Predicate<Simulated> filterOutMatching = (Simulated simulated2) => !simulated2.InteractionState.IsEditable && !TheDebugManager.debugPlaceObjects;
				Ray rayCast;
				Simulated simulated = FindAlreadySelected(filterOutMatching, session.game.simulation, session.TheCamera, evt.position, out rayCast, session.game.selected);
				if (simulated == null)
				{
					simulated = FindBestSimulatedUnderPoint(new SelectionPrioritizer(session.camera.UnityCamera), filterOutMatching, session.game.simulation, session.TheCamera, evt.position, out rayCast);
				}
				if (simulated == null)
				{
					session.properties.startedTouchOnEmptySpace = true;
				}
				else
				{
					session.properties.startedTouchOnEmptySpace = false;
					clickedSim = simulated;
				}
				if (simulated == session.game.selected && !Input.GetMouseButtonDown(1))
				{
					session.properties.isDraggingBuilding = true;
					session.AddAsyncResponse("override_drag", new object());
				}
				else
				{
					session.properties.isDraggingBuilding = false;
				}
				session.AddAsyncResponse("panning_event", evt);
				break;
			}
			case YGEvent.TYPE.TOUCH_END:
			case YGEvent.TYPE.TOUCH_CANCEL:
			case YGEvent.TYPE.TAP:
			case YGEvent.TYPE.RESET:
			case YGEvent.TYPE.DISABLE:
				if (m_bTouchBegan && !userCameraActive)
				{
					m_bTouchBegan = false;
					if (session.properties.startedTouchOnEmptySpace)
					{
						session.CheckAsyncRequest("override_drag");
						session.CheckAsyncRequest("panning_event");
						session.ChangeState("Editing");
					}
					else if (!session.properties.isDraggingBuilding && !session.properties.startedTouchOnEmptySpace)
					{
						interactionStrip.EnableButtons(session, true);
						if (!session.properties.waitToDecidePlacement)
						{
							DecideForSelectedBuilding(session);
						}
						session.properties.waitToDecidePlacement = false;
						CleanupMovementVisuals(session);
						session.game.selected.Alpha = 1f;
						session.game.selected.FootprintVisible = false;
						session.game.selected = clickedSim;
						session.TheSoundEffectManager.PlaySound(clickedSim.entity.SoundOnSelect);
						session.game.selected.Bounce();
						AdornMovementVisuals(session);
						UpdateMovementBookkeeping(session);
						LoadInteractionStrip(session);
						session.properties.firstEntered = true;
					}
				}
				if (userCameraActive || session.properties.firstEntered)
				{
					userCameraActive = false;
					if (session.properties.isDraggingBuilding || session.properties.firstEntered)
					{
						session.properties.isDraggingBuilding = true;
						session.camera.SetEnableUserInput(userCameraActive, session.properties.isDraggingBuilding, interactionStrip.StripPosition);
						if (session.properties.firstEntered)
						{
							session.camera.ChangeState(SBCamera.State.Dragging);
						}
					}
					else
					{
						session.camera.SetEnableUserInput(userCameraActive);
						if (!session.properties.isInteractionStripActive)
						{
							session.properties.isInteractionStripActive = true;
							interactionStrip.EnableButtons(session, true);
						}
						interactionStrip.OnUpdate(session);
					}
				}
				session.properties.waitToDecidePlacement = false;
				session.properties.isDraggingBuildingAndScreen = false;
				session.properties.isDraggingBuilding = false;
				session.properties.firstEntered = false;
				break;
			case YGEvent.TYPE.TOUCH_MOVE:
				if (!m_bTouchBegan)
				{
					session.properties.isDraggingBuildingAndScreen = false;
					session.properties.isInteractionStripActive = true;
					m_bTouchBegan = true;
					if (session.game.selected != null)
					{
						session.properties.startedTouchOnEmptySpace = false;
						bool flag = true;
						clickedSim = session.game.selected;
						session.properties.isDraggingBuilding = true;
						session.AddAsyncResponse("override_drag", new object());
					}
				}
				obj = null;
				if (!session.properties.startedTouchOnEmptySpace && session.properties.isDraggingBuilding)
				{
					obj = session.CheckAsyncRequest("override_drag");
				}
				if (obj != null)
				{
					session.AddAsyncResponse("override_drag", obj);
					SnapSelectedToInputPosition(session, evt.position, true, true);
					userCameraActive = true;
					session.camera.SetEnableUserInput(userCameraActive, session.properties.isDraggingBuilding);
					if (!session.properties.isDraggingBuildingAndScreen)
					{
						session.camera.ChangeState(SBCamera.State.Dragging);
						session.properties.isDraggingBuildingAndScreen = true;
					}
				}
				else if ((evt.position - evt.startPosition).SqrMagnitude() > 400f)
				{
					session.properties.waitToDecidePlacement = true;
					if (session.game.selected.InteractionState.Controls != null && session.game.selected.InteractionState.Controls.Count > 0 && !session.game.selected.InteractionState.IsEditable)
					{
						interactionStrip.ActivateOnSelected(session);
					}
					session.game.selected.FootprintVisible = true;
					userCameraActive = true;
					session.camera.SetEnableUserInput(userCameraActive, session.properties.isDraggingBuilding);
					object obj2 = session.CheckAsyncRequest("panning_event");
					if (obj2 != null)
					{
						session.camera.ProcessExtraGuiEvent((SBGUIEvent)obj2);
					}
					if (session.properties.isInteractionStripActive)
					{
						interactionStrip.EnableButtons(session, false);
						session.properties.isInteractionStripActive = false;
					}
					if (session.properties.firstEntered)
					{
						session.properties.firstEntered = false;
					}
				}
				break;
			case YGEvent.TYPE.TOUCH_STAY:
				obj = null;
				if (!session.properties.startedTouchOnEmptySpace && session.properties.isDraggingBuilding)
				{
					obj = session.CheckAsyncRequest("override_drag");
				}
				if (obj != null)
				{
					session.AddAsyncResponse("override_drag", obj);
					SnapSelectedToInputPosition(session, evt.position, true, true);
					userCameraActive = true;
					session.camera.SetEnableUserInput(userCameraActive, session.properties.isDraggingBuilding);
					if (!session.properties.isDraggingBuildingAndScreen)
					{
						session.camera.ChangeState(SBCamera.State.Dragging);
						session.properties.isDraggingBuildingAndScreen = true;
					}
				}
				break;
			case YGEvent.TYPE.HOVER:
			case YGEvent.TYPE.DRAG:
			case YGEvent.TYPE.FLICK:
			case YGEvent.TYPE.SWIPE:
			case YGEvent.TYPE.PINCH:
			case YGEvent.TYPE.HOLD:
				break;
			}
		}

		protected override void AcceptPlacement(Session session)
		{
			session.TheSoundEffectManager.PlaySound("EditModePlace");
			if (WasFromInventory(session))
			{
				session.game.selected.entity.Variable["associated_entities"] = session.CheckAsyncRequest("AssociatedEntities");
				session.game.simulation.Router.Send(CompleteCommand.Create(Identity.Null(), session.game.selected.Id));
			}
			else if (session.game.selected.Position != session.properties.preMovePosition || session.game.selected.Flip != session.properties.preMoveFlip)
			{
				session.game.simulation.ModifyGameStateSimulated(session.game.selected, new MoveAction(session.game.selected, null));
				session.game.selected.Animate(session.game.simulation);
				float num = 0f;
				GridPosition gridPosition = session.game.terrain.ComputeGridPosition(session.properties.preMovePosition);
				GridPosition gridPosition2 = session.game.terrain.ComputeGridPosition(session.game.selected.Position);
				num = (gridPosition2 - gridPosition).ToVector2().magnitude;
				if (session.game.selected.HasEntity<StructureDecorator>() && session.game.selected.GetEntity<StructureDecorator>().IsObstacle)
				{
					Vector2 offset = session.properties.preMovePosition - session.game.selected.Position;
					AlignedBox box = session.game.selected.Box.OffsetByVector(offset);
					session.game.terrain.SetOrClearObstacle(box, false);
				}
				session.analytics.LogMoveObject(session.game.selected.entity.BlueprintName, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount, num, session.framerateWatcher.Framerate);
			}
			base.AcceptPlacement(session);
		}

		public void DeactivateInteractionStrip(Session session)
		{
			interactionStrip.Deactivate(session);
		}
	}

	public class MoveBuildingInPlacement : MoveBuilding
	{
		public const string CURRENT_UI_EVENT = "currentUiEvt";

		private bool isTutorialPointerOnStrip;

		private bool m_bTouchBegan;

		private bool userCameraActive;

		public override void OnEnter(Session session)
		{
			m_bTouchBegan = false;
			Action<Session> handler = delegate(Session sesh)
			{
				AcceptPlacement(sesh);
				session.properties.waitToDecidePlacement = true;
				session.properties.preMovePositionSet = false;
				sesh.ChangeState("Playing");
			};
			Action<Session> handler2 = delegate(Session sesh)
			{
				DenyPlacement(sesh);
			};
			session.properties.editingHud = SBUIBuilder.MakeAndPushEmptyUI(session, HandleSBGUIEvent);
			SBGUIEvent sBGUIEvent = (SBGUIEvent)session.CheckAsyncRequest("currentUiEvt");
			if (sBGUIEvent != null)
			{
				HandleSBGUIEvent(sBGUIEvent, session);
			}
			interactionStrip.ActivateOnSelected(session);
			interactionStrip.SetAcceptHandler(session, handler);
			interactionStrip.SetRejectHandler(session, handler2);
			if (!session.game.simulation.featureManager.CheckFeature("move_reject_lock"))
			{
				interactionStrip.EnableRejectButton(session, false);
			}
			session.properties.preMovePosition = session.game.selected.Position;
			session.properties.preMoveFlip = session.game.selected.Flip;
			m_bTouchBegan = true;
			base.OnEnter(session);
		}

		protected override void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			TFUtils.Assert(session.game != null, "Should not be in MoveBuildingInPlacement w/out a game!");
			if (session.game == null)
			{
				TFUtils.ErrorLog("Should not be in MoveBuildingInPlacement w/out a game!");
				TFUtils.LogDump(session, "error");
				return;
			}
			TFUtils.Assert(session.game.selected != null, "Should not be in MoveBuildingInPlacement w/out a selected sim!");
			if (session.game.selected == null)
			{
				TFUtils.ErrorLog("Should not be in MoveBuildingInPlacement w/out a selected sim!");
				TFUtils.LogDump(session, "error");
				return;
			}
			TFUtils.Assert(session.game.selected.Id != null, "Should not be in MoveBuildingInPlacement w/out a selected sim id!");
			if (session.game.selected.Id == null)
			{
				TFUtils.ErrorLog("Should not be in MoveBuildingInPlacement w/out a selected sim id!");
				TFUtils.LogDump(session, "error");
				return;
			}
			object obj = null;
			switch (evt.type)
			{
			case YGEvent.TYPE.TOUCH_BEGIN:
			{
				Predicate<Simulated> filterOutMatching = (Simulated simulated2) => !simulated2.InteractionState.IsEditable && !TheDebugManager.debugPlaceObjects;
				Ray rayCast;
				Simulated simulated = FindAlreadySelected(filterOutMatching, session.game.simulation, session.TheCamera, evt.position, out rayCast, session.game.selected);
				if (simulated == null)
				{
					simulated = FindBestSimulatedUnderPoint(new SelectionPrioritizer(session.camera.UnityCamera), filterOutMatching, session.game.simulation, session.TheCamera, evt.position, out rayCast);
				}
				session.properties.isInteractionStripActive = true;
				session.properties.isDraggingBuilding = false;
				m_bTouchBegan = true;
				if (simulated == session.game.selected && !Input.GetMouseButtonDown(1))
				{
					session.properties.isDraggingBuilding = true;
					session.AddAsyncResponse("override_drag", new object());
				}
				else
				{
					session.properties.isDraggingBuilding = false;
				}
				session.AddAsyncResponse("panning_event", evt);
				break;
			}
			case YGEvent.TYPE.TOUCH_END:
			case YGEvent.TYPE.TOUCH_CANCEL:
			case YGEvent.TYPE.TAP:
			case YGEvent.TYPE.RESET:
			case YGEvent.TYPE.DISABLE:
				if (m_bTouchBegan)
				{
					if (session.properties.isDraggingBuilding)
					{
						session.CheckAsyncRequest("override_drag");
						SnapSelectedToInputPosition(session, evt.position, true, true);
					}
					session.CheckAsyncRequest("panning_event");
					m_bTouchBegan = false;
				}
				if (userCameraActive)
				{
					userCameraActive = false;
					if (session.properties.isDraggingBuilding)
					{
						session.camera.SetEnableUserInput(userCameraActive, session.properties.isDraggingBuilding, interactionStrip.StripPosition);
					}
					else
					{
						session.camera.SetEnableUserInput(userCameraActive);
						if (!session.properties.isInteractionStripActive)
						{
							session.properties.isInteractionStripActive = true;
							interactionStrip.EnableButtons(session, true);
						}
						interactionStrip.OnUpdate(session);
					}
				}
				session.properties.waitToDecidePlacement = false;
				session.properties.isDraggingBuildingAndScreen = false;
				session.properties.isDraggingBuilding = false;
				break;
			case YGEvent.TYPE.TOUCH_MOVE:
				if (!m_bTouchBegan)
				{
					break;
				}
				obj = session.CheckAsyncRequest("override_drag");
				if (obj != null && session.properties.isDraggingBuilding)
				{
					userCameraActive = true;
					session.camera.SetEnableUserInput(userCameraActive, session.properties.isDraggingBuilding);
					if (!session.properties.isDraggingBuildingAndScreen && session.game.simulation.featureManager.CheckFeature("move_reject_lock"))
					{
						session.camera.ChangeState(SBCamera.State.Dragging);
						session.properties.isDraggingBuildingAndScreen = true;
					}
					session.AddAsyncResponse("override_drag", obj);
					SnapSelectedToInputPosition(session, evt.position, true, true);
				}
				else if ((evt.position - evt.startPosition).SqrMagnitude() > 400f && session.game.simulation.featureManager.CheckFeature("move_reject_lock"))
				{
					session.properties.waitToDecidePlacement = true;
					session.ChangeState("MoveBuildingPanningInPlacement");
					if (session.properties.isInteractionStripActive)
					{
						interactionStrip.EnableButtons(session, false);
						session.properties.isInteractionStripActive = false;
					}
				}
				break;
			case YGEvent.TYPE.TOUCH_STAY:
				obj = null;
				if (!session.properties.startedTouchOnEmptySpace && session.properties.isDraggingBuilding)
				{
					obj = session.CheckAsyncRequest("override_drag");
				}
				if (obj != null)
				{
					session.AddAsyncResponse("override_drag", obj);
					SnapSelectedToInputPosition(session, evt.position, true, true);
					userCameraActive = true;
					session.camera.SetEnableUserInput(userCameraActive, session.properties.isDraggingBuilding);
					if (!session.properties.isDraggingBuildingAndScreen && session.game.simulation.featureManager.CheckFeature("move_reject_lock"))
					{
						session.camera.ChangeState(SBCamera.State.Dragging);
						session.properties.isDraggingBuildingAndScreen = true;
					}
				}
				break;
			case YGEvent.TYPE.HOVER:
			case YGEvent.TYPE.DRAG:
			case YGEvent.TYPE.FLICK:
			case YGEvent.TYPE.SWIPE:
			case YGEvent.TYPE.PINCH:
			case YGEvent.TYPE.HOLD:
				break;
			}
		}

		protected override void AcceptPlacement(Session session)
		{
			BuildingEntity entity = session.game.selected.GetEntity<BuildingEntity>();
			if (entity.Invariable.ContainsKey("accept_placement_sound"))
			{
				session.game.simulation.soundEffectManager.PlaySound((string)entity.Invariable["accept_placement_sound"]);
			}
			else
			{
				session.game.simulation.soundEffectManager.PlaySound("PlaceBuilding");
			}
			if (session.properties.cameFromMarketplace)
			{
				Simulated simulated = session.game.simulation.SpawnWorker(session.game.selected);
				ErectableDecorator decorator = entity.GetDecorator<ErectableDecorator>();
				session.game.simulation.Router.Send(EmployCommand.Create(Identity.Null(), session.game.selected.Id, simulated.Id));
				session.game.simulation.Router.Send(CompleteCommand.Create(Identity.Null(), session.game.selected.Id), decorator.ErectionTime);
				session.game.simulation.Router.Send(CompleteCommand.Create(session.game.selected.Id, simulated.Id), decorator.ErectionTime);
				session.game.simulation.Router.Send(ReturnCommand.Create(session.game.selected.Id, simulated.Id), decorator.ErectionTime);
				Cost cost = session.game.catalog.GetCost(entity.DefinitionId);
				TFUtils.Assert(cost != null, "Expected building to have a cost when placing after purchase");
				session.game.resourceManager.Apply(cost, session.game);
				bool decoration = entity.DefinitionId >= 2000 && entity.DefinitionId < 3000;
				bool premium = cost.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY);
				session.analytics.LogPlacement(entity.BlueprintName, decoration, premium, cost, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount, session.framerateWatcher.Framerate);
				AnalyticsWrapper.LogItemPlacement(session.TheGame, session.game.selected.Entity, false, true);
				session.properties.storeVisitSinceLastPurchase = 0;
				Shopping.FireFinishShoppingEvent(session);
			}
			else if (TheDebugManager.debugPlaceObjects || session.game.simulation.PlacementQuery(session.game.selected) != Simulation.Placement.RESULT.INVALID)
			{
				if (WasFromInventory(session))
				{
					AnalyticsWrapper.LogItemPlacement(session.TheGame, session.game.selected.Entity, true, true);
					session.game.selected.entity.Variable["associated_entities"] = session.CheckAsyncRequest("AssociatedEntities");
					session.game.simulation.Router.Send(CompleteCommand.Create(Identity.Null(), session.game.selected.Id));
					session.CheckAsyncRequest("FromInventory");
					session.analytics.LogPlacementFromInventory(entity.BlueprintName, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
				}
				else
				{
					TFUtils.Assert(!session.properties.cameFromMarketplace, "You should not get here if you came from the marketplace!");
					session.game.simulation.ModifyGameStateSimulated(session.game.selected, new MoveAction(session.game.selected, null));
				}
			}
			base.AcceptPlacement(session);
		}
	}

	public class MoveBuildingPanningInEdit : State
	{
		private InteractionStripMixin interactionStrip = new InteractionStripMixin();

		public void OnEnter(Session session)
		{
			if (session.game.selected == null)
			{
				TFUtils.ErrorLog("MoveBuildingPanningInEdit.OnEnter - selected is null");
				session.ChangeState("Playing");
				return;
			}
			Action inventoryClickHandler = delegate
			{
				if (!session.game.featureManager.CheckFeature("inventory_soft"))
				{
					session.game.featureManager.UnlockFeature("inventory_soft");
					session.game.featureManager.ActivateFeatureActions(session.game, "inventory_soft");
					session.game.simulation.ModifyGameState(new FeatureUnlocksAction(new List<string> { "inventory_soft" }));
				}
				else
				{
					session.ChangeState("Inventory");
				}
			};
			Action optionsHandler = delegate
			{
				session.ChangeState("Options");
			};
			Action editClickHandler = delegate
			{
				session.ChangeState("Playing");
			};
			SBGUIStandardScreen val = SBUIBuilder.MakeAndPushStandardUI(session, true, HandleSBGUIEvent, null, inventoryClickHandler, optionsHandler, editClickHandler, null, null, null, null, null, null, null, true);
			session.AddAsyncResponse("standard_screen", val);
			if (session.game.selected.InteractionState.Controls != null && session.game.selected.InteractionState.Controls.Count > 0 && !session.game.selected.InteractionState.IsEditable)
			{
				interactionStrip.ActivateOnSelected(session);
			}
			session.camera.SetEnableUserInput(true);
			object obj = session.CheckAsyncRequest("panning_event");
			if (obj != null)
			{
				session.camera.ProcessExtraGuiEvent((SBGUIEvent)obj);
			}
			session.game.selected.FootprintVisible = true;
		}

		public void OnLeave(Session session)
		{
			interactionStrip.Deactivate(session);
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			switch (evt.type)
			{
			case YGEvent.TYPE.TOUCH_END:
			case YGEvent.TYPE.TOUCH_CANCEL:
				session.ChangeState("MoveBuildingInEdit");
				break;
			case YGEvent.TYPE.TOUCH_MOVE:
				interactionStrip.OnUpdate(session);
				break;
			case YGEvent.TYPE.TOUCH_STAY:
				break;
			}
		}
	}

	public class MoveBuildingPanningInPlacement : State
	{
		private const string MOVEBUILDING_UI_HANDLER = "movebuildingpanninginplacement_ui";

		private InteractionStripMixin interactionStrip = new InteractionStripMixin();

		public void OnEnter(Session session)
		{
			SBGUIScreen item = SBUIBuilder.MakeAndPushEmptyUI(session, HandleSBGUIEvent);
			interactionStrip.ActivateOnSelected(session);
			session.camera.SetEnableUserInput(true);
			session.game.sessionActionManager.SetActionHandler("movebuildingpanninginplacement_ui", session, new List<SBGUIScreen> { item }, SessionActionUiHelper.HandleCommonSessionActions);
			SessionActionSimulationHelper.EnableHandler(session, true);
			object obj = session.CheckAsyncRequest("panning_event");
			if (obj != null)
			{
				session.camera.ProcessExtraGuiEvent((SBGUIEvent)obj);
			}
			session.game.selected.FootprintVisible = true;
		}

		public void OnLeave(Session session)
		{
			interactionStrip.Deactivate(session);
			SBUIBuilder.ReleaseTopScreen();
			session.game.sessionActionManager.ClearActionHandler("movebuildingpanninginplacement_ui", session);
			session.AddAsyncResponse("silent_enter", new object());
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			switch (evt.type)
			{
			case YGEvent.TYPE.TOUCH_END:
			case YGEvent.TYPE.TOUCH_CANCEL:
				if (!session.properties.isInteractionStripActive)
				{
					session.properties.isInteractionStripActive = true;
					interactionStrip.EnableButtons(session, true);
				}
				session.ChangeState("MoveBuildingInPlacement");
				break;
			case YGEvent.TYPE.TOUCH_MOVE:
				interactionStrip.OnUpdate(session);
				break;
			case YGEvent.TYPE.TOUCH_STAY:
				break;
			}
		}
	}

	public class Movie : State
	{
		private string movie;

		private string nextSession;

		public string TheMovie
		{
			get
			{
				return movie;
			}
			set
			{
				movie = value;
			}
		}

		public string NextSessionState
		{
			get
			{
				return nextSession;
			}
			set
			{
				nextSession = value;
			}
		}

		public void OnEnter(Session session)
		{
			TFUtils.PlayMovie(movie);
			session.AddAsyncResponse("MovieStartTime", TFUtils.EpochTime());
		}

		public void OnLeave(Session session)
		{
			Thread.Sleep(0);
			AudioSettings.speakerMode = AudioSettings.driverCaps;
			session.soundEffectManager.Clear();
			int playerLevel = 0;
			if (session.game != null)
			{
				playerLevel = session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount;
			}
			session.analytics.LogPlayMovie(movie, TFUtils.EpochTime() - (ulong)session.CheckAsyncRequest("MovieStartTime"), playerLevel);
		}

		public void OnUpdate(Session session)
		{
			session.ChangeState(nextSession);
		}
	}

	public class Options : State
	{
		public void OnEnter(Session session)
		{
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			session.PlaySeaflowerAndBubbleScreenSwipeEffect();
			session.camera.SetEnableUserInput(false);
			TFUtils.ErrorLog(string.Empty);
			Action action = delegate
			{
				session.ChangeState("Playing");
				AndroidBack.getInstance().pop();
			};
			Action action2 = delegate
			{
			};
			Action parentsHandler = delegate
			{
				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					session.TheSoundEffectManager.PlaySound("Error");
					ExplanationDialogInputData item = new ExplanationDialogInputData(Language.Get("!!ERROR_NEED_NETWORK_TITLE"), "Beat_JellyfishFields_ComingSoon");
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData> { item });
					session.ChangeState("ShowingDialog");
				}
				else
				{
					session.TheSoundEffectManager.PlaySound("Accept");
					session.ChangeState("AgeGate");
				}
			};
			Action moreNickHandler = delegate
			{
				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					session.TheSoundEffectManager.PlaySound("Error");
					ExplanationDialogInputData item = new ExplanationDialogInputData(Language.Get("!!ERROR_MORE_NICK_MESSAGE"), "Beat_JellyfishFields_ComingSoon");
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData> { item });
					session.ChangeState("ShowingDialog");
				}
				else
				{
					session.TheSoundEffectManager.PlaySound("Accept");
					session.PlayHavenController.RequestContent("more_nick_click");
				}
			};
			Action toggleSFXHandler = delegate
			{
				if (session.soundEffectManager.Enabled)
				{
					session.TheSoundEffectManager.PlaySound("Accept");
					session.soundEffectManager.ToggleOnOff();
				}
				else
				{
					session.soundEffectManager.ToggleOnOff();
					session.TheSoundEffectManager.PlaySound("Accept");
				}
			};
			Action toggleMusicHandler = delegate
			{
				session.TheSoundEffectManager.PlaySound("Accept");
				session.musicManager.ToggleOnOff();
			};
			Action creditsHandler = delegate
			{
				session.TheSoundEffectManager.PlaySound("tutorial_arrow");
				if (session.InFriendsGame && session.properties.optionsHud != null)
				{
					session.properties.optionsHud.SetActive(false);
				}
				session.ChangeState("Credits");
			};
			Action achievementsHandler = delegate
			{
				session.TheSoundEffectManager.PlaySound("Accept");
				/*if (GameCenterBinding.isPlayerAuthenticated())
				{
					GameCenterBinding.showAchievements();
				}
				else
				{
					GameCenterBinding.authenticateLocalPlayer();
				}*/
			};
			Action privacyHandler = delegate
			{
				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					session.TheSoundEffectManager.PlaySound("Error");
					ExplanationDialogInputData item = new ExplanationDialogInputData(Language.Get("!!ERROR_NEED_NETWORK_TITLE"), "Beat_JellyfishFields_ComingSoon");
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData> { item });
					session.ChangeState("ShowingDialog");
				}
				else
				{
					session.TheSoundEffectManager.PlaySound("Accept");
					Application.OpenURL(TFUtils.GetPrivacy_Address());
				}
			};
			Action eulaHandler = delegate
			{
				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					session.TheSoundEffectManager.PlaySound("Error");
					ExplanationDialogInputData item = new ExplanationDialogInputData(Language.Get("!!ERROR_NEED_NETWORK_TITLE"), "Beat_JellyfishFields_ComingSoon");
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData> { item });
					session.ChangeState("ShowingDialog");
				}
				else
				{
					session.TheSoundEffectManager.PlaySound("Accept");
					Application.OpenURL(TFUtils.GetEULA_Address());
				}
			};
			Action debugHandler = delegate
			{
				session.TheSoundEffectManager.PlaySound("Accept");
				session.ChangeState("Debug");
			};
			session.properties.optionsHud = SBUIBuilder.MakeAndPushStandardUI(session, false, null, action2, action2, action, action2, null, DragFeeding.SwitchToFn(session), null, action2, action2, action2, action2);
			session.properties.optionsHud.SetActive(true);
			session.properties.optionsHud.SetVisibleNonEssentialElements(false, true);
			SBGUIPulseButton sBGUIPulseButton = (SBGUIPulseButton)session.properties.optionsHud.FindChild("marketplace");
			sBGUIPulseButton.SetActive(false);
			SBGUIPulseButton sBGUIPulseButton2 = (SBGUIPulseButton)session.properties.optionsHud.FindChild("community_event");
			sBGUIPulseButton2.SetActive(false);
			if (session.InFriendsGame)
			{
				SBGUIElement sBGUIElement = session.properties.optionsHud.FindChild("happyface");
				sBGUIElement.SetActive(false);
				SBGUIElement sBGUIElement2 = session.properties.optionsHud.FindChild("quest_marker");
				sBGUIElement2.SetActive(false);
				SBGUIElement sBGUIElement3 = session.properties.optionsHud.FindChild("jj_bar");
				sBGUIElement3.SetActive(false);
				SBGUIElement sBGUIElement4 = session.properties.optionsHud.FindChild("money_bar");
				sBGUIElement4.SetActive(false);
				SBGUIElement sBGUIElement5 = session.properties.optionsHud.FindChild("special_bar");
				if ((bool)sBGUIElement5)
				{
					sBGUIElement5.SetActive(false);
				}
			}
			SBGUIScreen sBGUIScreen = SBUIBuilder.MakeAndPushOptionsDialog(action, moreNickHandler, toggleSFXHandler, toggleMusicHandler, achievementsHandler, creditsHandler, privacyHandler, eulaHandler, debugHandler, parentsHandler);
			YGTextAtlasSprite component = sBGUIScreen.FindChild("player_id_label").GetComponent<YGTextAtlasSprite>();
			component.Text = Soaring.Player.UserTag;
			session.TheCamera.TurnOnScreenBuffer();
			SBGUIElement sBGUIElement6 = sBGUIScreen.FindChild("red_counter");
			SBGUILabel sBGUILabel = (SBGUILabel)sBGUIScreen.FindChild("red_counter_label");
			int notificationCount = HelpshiftSdk.getInstance().getNotificationCount(false);
			HelpshiftSdk.getInstance().getNotificationCount(true);
			sBGUILabel.SetText(notificationCount.ToString());
			if (notificationCount > 0)
			{
				sBGUILabel.SetActive(true);
				sBGUIElement6.SetActive(true);
			}
			else
			{
				sBGUILabel.SetActive(false);
				sBGUIElement6.SetActive(false);
			}
		}

		public void OnLeave(Session session)
		{
			session.TheSoundEffectManager.PlaySound("CloseMenu");
			session.properties.optionsHud.SetVisibleNonEssentialElements(true);
			session.properties.optionsHud = null;
			session.TheCamera.TurnOffScreenBuffer();
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
		}
	}

	public class Paving : State
	{
		private List<PaveAction.PaveElement> workingList;

		private Cost segmentCost;

		private Cost totalCost;

		private int placed;

		private int removed;

		private int cannotPay;

		public Paving()
		{
			workingList = new List<PaveAction.PaveElement>();
		}

		public void OnEnter(Session session)
		{
			segmentCost = Cost.FromDict(new Dictionary<string, object> { { "3", 5 } });
			totalCost = new Cost();
			Action inventoryHandler = delegate
			{
				if (!session.TheGame.featureManager.CheckFeature("inventory_soft"))
				{
					session.TheGame.featureManager.UnlockFeature("inventory_soft");
					session.TheGame.featureManager.ActivateFeatureActions(session.TheGame, "inventory_soft");
					session.TheGame.simulation.ModifyGameState(new FeatureUnlocksAction(new List<string> { "inventory_soft" }));
				}
				else
				{
					session.AddAsyncResponse("FromEdit", true);
					session.ChangeState("Inventory");
				}
			};
			SBUIBuilder.MakeAndPushPavingUI(session, HandleSBGUIEvent, delegate
			{
				session.ChangeState("Editing");
			}, delegate
			{
				session.ChangeState("Playing");
			}, inventoryHandler);
			Action action = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.camera.SetEnableUserInput(true);
			cannotPay = 0;
			placed = 0;
			removed = 0;
		}

		public void OnLeave(Session session)
		{
			if (workingList != null && workingList.Count > 0)
			{
				List<PaveAction.PaveElement> path = workingList;
				workingList = new List<PaveAction.PaveElement>();
				session.game.simulation.ModifyGameState(new PaveAction(new Identity(), path, totalCost));
			}
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			YGEvent.TYPE type = evt.type;
			if (type != YGEvent.TYPE.TAP)
			{
				return;
			}
			Ray ray = Camera.main.ScreenPointToRay(evt.position);
			Vector3 point;
			session.game.terrain.ComputeIntersection(ray, out point);
			Vector2 vector = default(Vector2);
			vector.x = point.x;
			vector.y = point.y;
			GridPosition gpos = session.game.terrain.ComputeGridPosition(vector);
			TerrainType terrainType = session.game.terrain.GetTerrainType(gpos);
			if ((terrainType != null && !terrainType.CanPave()) || !session.game.terrain.CheckIsPurchasedArea(vector))
			{
				return;
			}
			bool flag = terrainType != null && !terrainType.IsPath();
			if (flag && !TheDebugManager.debugPlaceObjects)
			{
				if (session.game.simulation.PlacementQuery(session.game.terrain.GetGridBounds(gpos.row, gpos.col), null, true) == Simulation.Placement.RESULT.INVALID)
				{
					session.TheSoundEffectManager.PlaySound("Cancel");
					return;
				}
				if (!session.game.resourceManager.CanPay(segmentCost))
				{
					cannotPay++;
					session.TheSoundEffectManager.PlaySound("Cancel");
					return;
				}
			}
			if (!session.game.terrain.ChangePath(gpos))
			{
				return;
			}
			if (flag)
			{
				if (!TheDebugManager.debugPlaceObjects)
				{
					session.game.resourceManager.Apply(segmentCost, session.game);
					totalCost += segmentCost;
				}
				session.TheSoundEffectManager.PlaySound("PlacePavement");
				placed++;
			}
			else
			{
				if (!TheDebugManager.debugPlaceObjects)
				{
					session.game.resourceManager.SellFor(segmentCost, session.game);
					totalCost -= segmentCost;
				}
				session.TheSoundEffectManager.PlaySound("RemovePavement");
				removed++;
			}
			int num = workingList.FindIndex((PaveAction.PaveElement p) => p.position == gpos);
			if (num < 0)
			{
				workingList.Add(new PaveAction.PaveElement(gpos));
			}
			else
			{
				workingList.RemoveAt(num);
			}
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}
	}

	public class Placing : State
	{
		public const string FRESHLY_PURCHASED = "FreshlyPurchased";

		public void OnEnter(Session session)
		{
			SBUIBuilder.MakeAndPushEmptyUI(session, null);
			SBUIBuilder.ReleaseTimebars();
			SBUIBuilder.ReleaseNamebars();
			Action action = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			SessionActionSimulationHelper.EnableHandler(session, true);
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
			if (!PlayerCanAfford(session, session.game.selected))
			{
				TFUtils.DebugLog("You cannot afford that!");
				session.game.selected.SetFootprint(session.game.simulation, false);
				session.game.simulation.RemoveSimulated(session.game.selected);
				session.game.entities.Destroy(session.game.selected.Id);
				session.ChangeState("Shopping");
				session.game.selected = null;
				return;
			}
			Ray ray = session.camera.ScreenPointToRay(session.camera.ScreenCenter);
			Vector3 point = new Vector3(0f, 0f, 0f);
			Dictionary<int, Vector2> dictionary = (Dictionary<int, Vector2>)session.CheckAsyncRequest("preplace_request_dict");
			if (dictionary != null)
			{
				if (dictionary.ContainsKey(session.game.selected.entity.DefinitionId))
				{
					point.x = dictionary[session.game.selected.entity.DefinitionId].x;
					point.y = dictionary[session.game.selected.entity.DefinitionId].y;
					dictionary.Remove(session.game.selected.entity.DefinitionId);
				}
				else
				{
					session.game.terrain.ComputeIntersection(ray, out point);
				}
				if (dictionary.Count > 0)
				{
					session.AddAsyncResponse("preplace_request_dict", dictionary);
				}
			}
			else
			{
				session.game.terrain.ComputeIntersection(ray, out point);
			}
			session.game.selected.Warp(new Vector2(point.x, point.y), session.game.simulation);
			session.properties.cameFromMarketplace = true;
			session.ChangeState("MoveBuildingInPlacement");
		}

		private bool PlayerCanAfford(Session session, Simulated simulated)
		{
			BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
			Cost cost = session.game.catalog.GetCost(entity.DefinitionId);
			return cost != null && session.game.resourceManager.CanPay(cost);
		}
	}

	public class Playing : State
	{
		public static string INVENTORY_ENTITY = "InventoryEntity";

		public virtual void OnEnter(Session session)
		{
			Action shopClickHandler = delegate
			{
				string storeTabValue = session.TheGame.questManager.GetStoreTabValue();
				if (!string.IsNullOrEmpty(storeTabValue))
				{
					session.AddAsyncResponse("target_store_tab", storeTabValue);
				}
				session.AddAsyncResponse("store_open_type", "store_open_button");
				session.ChangeState("Shopping");
			};
			Action inventoryClickHandler = delegate
			{
				session.CheckInventorySoftLock();
				session.ChangeState("Inventory");
			};
			Action optionsHandler = delegate
			{
				session.ChangeState("Options");
			};
			Action editClickHandler = delegate
			{
				session.ChangeState("Editing");
			};
			Action openIAPTabHandlerSoft = delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping");
			};
			Action openIAPTabHandlerHard = delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping");
			};
			Action communityEventClickHandler = delegate
			{
				session.ChangeState("CommunityEvent");
			};
			Action patchyClickHandler = delegate
			{
				session.TheSoundEffectManager.PlaySound("OpenMenu");
				SBUIBuilder.ReleaseTimebars();
				SBUIBuilder.ReleaseNamebars();
				if (session.InFriendsGame)
				{
					if (session.properties.playingHud != null)
					{
						session.properties.playingHud.gameObject.SetActiveRecursively(false);
					}
					else
					{
						Debug.LogError("Hud Not Set!!!!");
					}
					TFUtils.DebugLog("Switching To Park");
					session.InFriendsGame = false;
					session.TheGame.RequestReload();
				}
				else if (!Soaring.IsOnline || SBSettings.DisableFriendPark)
				{
					Action okHandler = delegate
					{
						SBUIBuilder.ReleaseTopScreen();
					};
					SBUIBuilder.CreateErrorDialog(session, "Error", Language.Get("!!NOTIFY_INGAME_PATCHYS_NOT_AVAILABLE"), Language.Get("!!PREFAB_OK"), okHandler, 0.85f, 0.45f);
				}
				else
				{
					if (session.properties.playingHud != null)
					{
						session.properties.playingHud.gameObject.SetActiveRecursively(false);
					}
					else
					{
						Debug.LogError("Hud Not Set!!!!");
					}
					session.InFriendsGame = true;
					TFUtils.DebugLog("Switching To Patchy Town");
					if (session.gameInitialized)
					{
						session.game.analytics.LogVisitPark();
						AnalyticsWrapper.LogVisitPark(session.game);
						QuestManager questManager2 = session.TheGame.questManager;
						if (Soaring.Player.CustomData == null)
						{
							SoaringDictionary soaringDictionary = new SoaringDictionary();
							soaringDictionary.addValue(new SoaringDictionary(), "custom");
							Soaring.Player.SetUserData(soaringDictionary, false);
						}
						SoaringArray soaringArray = (SoaringArray)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_fdk");
						if (soaringArray != null)
						{
							soaringArray.clear();
						}
						if (!questManager2.IsQuestCompleted(2400u) && questManager2.IsQuestActive(2400u))
						{
							soaringArray = new SoaringArray();
							Soaring.Player.PrivateData_Safe.setValue(soaringArray, "SBMI_fdk");
							soaringArray.addObject(2401L);
						}
						session.TheGame.SaveLocally(0uL);
						session.TheGame.RequestLoadFriendPark(null);
					}
				}
			};
			SBGUIStandardScreen screen = SBUIBuilder.MakeAndPushStandardUI(session, true, HandleSBGUIEvent, shopClickHandler, inventoryClickHandler, optionsHandler, editClickHandler, null, DragFeeding.SwitchToFn(session), null, openIAPTabHandlerSoft, openIAPTabHandlerHard, communityEventClickHandler, patchyClickHandler);
			screen.EnableUI(true);
			session.AddAsyncResponse("standard_screen", screen);
			session.properties.playingHud = screen;
			Action action = delegate
			{
				screen.RefreshQuestTrackers(session);
			};
			if (screen.ReadyEvent.IsReady)
			{
				action();
			}
			else
			{
				screen.ReadyEvent.AddListener(action);
			}
			session.properties.playingHud.ShowInventoryWidget();
			Action action2 = delegate
			{
				session.ChangeState("ShowingDialog");
			};
			session.game.dropManager.DialogNeededCallback = action2;
			session.game.questManager.OnShowDialogCallback = action2;
			session.game.communityEventManager.DialogNeededCallback = action2;
			session.camera.SetEnableUserInput(true);
			session.musicManager.PlayTrack("InGame");
			SBGUIElement sBGUIElement = screen.FindChild("quest");
			SBGUIElement sBGUIElement2 = session.properties.playingHud.FindChild("marketplace");
			SBGUIElement sBGUIElement3 = session.properties.playingHud.FindChild("inventory_widget");
			SBGUIElement sBGUIElement4 = session.properties.playingHud.FindChild("patchy_title_bg");
			SBGUIElement sBGUIElement5 = session.properties.playingHud.FindChild("patchy_title_icon");
			SBGUIElement sBGUIElement6 = session.properties.playingHud.FindChild("patchy_title_label");
			SBGUIPulseButton sBGUIPulseButton = (SBGUIPulseButton)session.properties.playingHud.FindChild("patchy");
			sBGUIPulseButton.SetActive(false);
			Vector3 localPosition = sBGUIPulseButton.gameObject.transform.localPosition;
			sBGUIPulseButton.gameObject.transform.localPosition = new Vector3(localPosition.x, localPosition.y, 0.9f);
			QuestManager questManager = session.game.questManager;
			SBGUIElement sBGUIElement7 = session.properties.playingHud.FindChild("inventory");
			SBGUIElement sBGUIElement8 = session.properties.playingHud.FindChild("edit");
			SBGUIElement sBGUIElement9 = session.properties.playingHud.FindChild("settings");
			SBGUIElement sBGUIElement10 = session.properties.playingHud.FindChild("red_counter");
			if (sBGUIElement9 != null)
			{
				sBGUIElement9.SetActive(true);
			}
			session.properties.playingHud.ShowCurrencies();
			if (session.InFriendsGame)
			{
				SBGUIElement sBGUIElement11 = session.properties.playingHud.FindChild("happyface");
				SBGUIElement sBGUIElement12 = session.properties.playingHud.FindChild("jj_bar");
				SBGUIElement sBGUIElement13 = session.properties.playingHud.FindChild("money_bar");
				SBGUIElement sBGUIElement14 = session.properties.playingHud.FindChild("special_bar");
				SBGUIElement sBGUIElement15 = session.properties.playingHud.FindChild("community_event");
				SBGUIElement sBGUIElement16 = session.properties.playingHud.FindChild("quest_scroll_down");
				SBGUIElement sBGUIElement17 = session.properties.playingHud.FindChild("quest_marker");
				SBGUIElement sBGUIElement18 = session.properties.playingHud.FindChild("quest_scroll_up");
				sBGUIPulseButton.SetActive(true);
				sBGUIPulseButton.tform.localPosition = new Vector3(1.538977f, 0.65f, 1f);
				sBGUIPulseButton.tform.localScale = new Vector3(1f, 1f, 1f);
				sBGUIPulseButton.SetTextureFromAtlas("GoHomeMenuButton.png", true);
				sBGUIElement2.SetActive(false);
				sBGUIElement.SetActive(false);
				sBGUIElement3.SetActive(false);
				if (sBGUIElement12 != null)
				{
					sBGUIElement12.SetActive(false);
				}
				if (sBGUIElement13 != null)
				{
					sBGUIElement13.SetActive(false);
				}
				if (sBGUIElement14 != null)
				{
					sBGUIElement14.SetActive(false);
				}
				if (sBGUIElement15 != null)
				{
					sBGUIElement15.SetActive(false);
				}
				if (sBGUIElement7 != null)
				{
					sBGUIElement7.SetActive(false);
				}
				if (sBGUIElement8 != null)
				{
					sBGUIElement8.SetActive(false);
				}
				if (sBGUIElement16 != null)
				{
					sBGUIElement16.SetActive(false);
					sBGUIElement16.SetVisible(false);
				}
				if (sBGUIElement17 != null)
				{
					sBGUIElement17.SetActive(false);
				}
				if (sBGUIElement18 != null)
				{
					sBGUIElement18.SetActive(false);
					sBGUIElement18.SetVisible(false);
				}
				if (sBGUIElement4 != null)
				{
					sBGUIElement4.SetActive(true);
				}
				if (sBGUIElement5 != null)
				{
					sBGUIElement5.SetActive(true);
				}
				if (sBGUIElement6 != null)
				{
					sBGUIElement6.SetActive(true);
				}
				if (sBGUIElement11 != null)
				{
					sBGUIElement11.SetActive(false);
				}
			}
			else
			{
				sBGUIElement.SetActive(true);
				sBGUIElement2.SetActive(true);
				sBGUIElement3.SetActive(true);
				sBGUIElement4.SetActive(false);
				sBGUIElement5.SetActive(false);
				sBGUIElement6.SetActive(false);
				if (sBGUIElement7 != null)
				{
					sBGUIElement7.SetActive(true);
				}
				if (sBGUIElement8 != null)
				{
					sBGUIElement8.SetActive(true);
				}
				bool flag = questManager.IsQuestActive(2400u);
				if (!flag)
				{
					flag = questManager.IsQuestCompleted(2400u);
				}
				if (SBSettings.DisableFriendPark)
				{
					flag = false;
				}
				sBGUIPulseButton.SetActive(flag);
				if (session.WasInFriendsGame)
				{
					session.TheCamera.ResetCameraPosition();
					session.musicManager.Enabled = session.musicStateBeforePT;
					if (!session.musicStateBeforePT)
					{
						session.musicManager.StopTrack();
					}
					session.soundEffectManager.Enabled = session.sfxStateBeforePT;
					session.WasInFriendsGame = false;
				}
				if (SBGUIStandardScreen.userClosedWishList)
				{
					session.properties.playingHud.CloseInventoryWidget();
				}
			}
			int notificationCount = HelpshiftSdk.getInstance().getNotificationCount(false);
			HelpshiftSdk.getInstance().getNotificationCount(true);
			session.properties.playingHud.HelpshiftNotificationCount = notificationCount;
			session.properties.playingHud.ShowHelpshiftNotification();
			session.game.sessionActionManager.SetActionHandler("playing_ui", session, new List<SBGUIScreen> { screen }, SessionActionUiHelper.HandleCommonSessionActions);
			SessionActionSimulationHelper.EnableHandler(session, true);
			GameStarting.ResetShowSplashScreen(GameStarting.SplashScreenState.None);
		}

		public virtual void OnLeave(Session session)
		{
			session.properties.playingHud = null;
			if (session.InFriendsGame && !session.WasInFriendsGame)
			{
				session.sfxStateBeforePT = session.soundEffectManager.Enabled;
				session.musicStateBeforePT = session.musicManager.Enabled;
			}
			session.CheckAsyncRequest("standard_screen");
			session.game.sessionActionManager.ClearActionHandler("playing_ui", session);
		}

		public virtual void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			if (session.properties.playingHud != null)
			{
				session.properties.playingHud.EnableUI(true);
			}
			session.camera.SetEnableUserInput(true);
			Ray rayCast;
			switch (evt.type)
			{
			case YGEvent.TYPE.TOUCH_BEGIN:
			{
				draggingCamera = false;
				if (draggingCamera)
				{
					break;
				}
				List<Simulated> list = FindSimulatedsUnderPoint(null, session.game.simulation, session.TheCamera, evt.position, out rayCast);
				SelectionPrioritizer selectionPrioritizer = new SelectionPrioritizer(session.TheCamera.UnityCamera);
				list.ForEach(selectionPrioritizer.SelectBest);
				Simulated best = selectionPrioritizer.Best;
				if (session.game.dropManager.ProcessTap(session, rayCast))
				{
					break;
				}
				TerrainSlot terrainSlot2 = session.game.terrain.CheckTap(rayCast, session.TheGame);
				if (best != null)
				{
					if (best.HasEntity<ResidentEntity>() && best.ThoughtDisplayController.isVisible && best.ThoughtDisplayController.Intersects(rayCast))
					{
						session.properties.tappedDisplayController = best.ThoughtDisplayController;
					}
					session.properties.touchingSim = best;
					session.properties.touchingSim.BounceStart();
				}
				session.properties.moveDragStart = evt.position;
				session.PlayTapParticleEffect(session.TheCamera.ScreenPointToWorldPoint(session.game.terrain, evt.position));
				if (terrainSlot2 != null)
				{
					session.CheckAsyncRequest("clear_purchase_on_movement");
					session.AddAsyncResponse("clear_purchase_on_movement", terrainSlot2);
				}
				break;
			}
			case YGEvent.TYPE.TOUCH_MOVE:
				draggingCamera = true;
				if (!draggingCamera)
				{
					List<Simulated> list = FindSimulatedsUnderPoint(null, session.game.simulation, session.TheCamera, evt.position, out rayCast);
					SelectionPrioritizer selectionPrioritizer = new SelectionPrioritizer(session.TheCamera.UnityCamera);
					list.ForEach(selectionPrioritizer.SelectBest);
					Simulated best = selectionPrioritizer.Best;
					session.CheckAsyncRequest("clear_purchase_on_movement");
					if (session.CheckAsyncRequest("ResetSimulationDrag") != null)
					{
						session.properties.moveDragStart = evt.position;
						session.camera.ResetCurrentState();
					}
					if (session.properties.touchingSim != null && !list.Contains(session.properties.touchingSim))
					{
						CleanupTouchingSim(session);
						CleanupTouchingBubble(session);
						TFUtils.ErrorLog("cleanuptouchingsim " + session);
					}
				}
				break;
			case YGEvent.TYPE.HOLD:
			{
				List<Simulated> list = FindSimulatedsUnderPoint(null, session.game.simulation, session.TheCamera, evt.position, out rayCast);
				SelectionPrioritizer selectionPrioritizer = new SelectionPrioritizer(session.TheCamera.UnityCamera);
				list.ForEach(selectionPrioritizer.SelectBest);
				Simulated best = selectionPrioritizer.Best;
				if (session.properties.touchingSim != null)
				{
					session.properties.touchingSim.BounceEnd();
					if (session.properties.touchingSim == best || list.Contains(session.properties.touchingSim))
					{
						session.AddAsyncResponse("override_drag", true);
						TryGrabSimulated(session, session.properties.touchingSim, evt);
					}
				}
				break;
			}
			case YGEvent.TYPE.TOUCH_END:
			{
				List<Simulated> list = FindSimulatedsUnderPoint(null, session.game.simulation, session.TheCamera, evt.position, out rayCast);
				SelectionPrioritizer selectionPrioritizer = new SelectionPrioritizer(session.TheCamera.UnityCamera);
				list.ForEach(selectionPrioritizer.SelectBest);
				Simulated best = selectionPrioritizer.Best;
				if (draggingCamera && session.properties.touchingSim != null && session.properties.touchingSim.DisplayController != null && session.properties.touchingSim.DisplayController.Transform != null)
				{
					session.properties.touchingSim.AnimateScaleAndFlip(new Vector3(1f, 1f, 1f));
				}
				else if (!draggingCamera)
				{
					TerrainSlot terrainSlot = (TerrainSlot)session.CheckAsyncRequest("clear_purchase_on_movement");
					if (terrainSlot != null && session.game.terrain.ProcessTap(rayCast, session.TheGame))
					{
						session.ChangeState("Expansion");
						break;
					}
					if (list.Contains(session.properties.touchingSim))
					{
						if (session.properties.tappedDisplayController != null)
						{
							if (string.Compare(session.properties.touchingSim.GetDisplayState(), "acceptable") == 0 || string.Compare(session.properties.touchingSim.ThoughtDisplayController.GetDisplayState(), "task_collect") == 0)
							{
								session.properties.queuedClickedSim = session.properties.touchingSim;
							}
							else
							{
								ResidentEntity entity = session.properties.touchingSim.GetEntity<ResidentEntity>();
								if (entity.HungerResourceId.HasValue)
								{
									int value = entity.HungerResourceId.Value;
									session.game.simulation.Router.Send(OfferFoodCommand.Create(Identity.Null(), session.properties.touchingSim.Id, value));
								}
							}
						}
						else
						{
							session.properties.queuedClickedSim = session.properties.touchingSim;
						}
					}
					CleanupTouchingSim(session);
				}
				CleanupTouchingBubble(session);
				draggingCamera = false;
				break;
			}
			default:
				CleanupTouchingSim(session);
				CleanupTouchingBubble(session);
				break;
			}
		}

		public virtual void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
			if (!session.InFriendsGame)
			{
				session.game.questManager.OnUpdate(session.game);
			}
			session.game.treasureManager.OnUpdate(session);
			object obj = session.CheckAsyncRequest(INVENTORY_ENTITY);
			if (obj != null)
			{
				SBGUIScreen sBGUIScreen = SBGUI.GetInstance().PopGUIScreen();
				UnityEngine.Object.Destroy(sBGUIScreen.gameObject);
				Simulated selected = (Simulated)obj;
				session.game.selected = selected;
				session.game.selected.Bounce();
				session.game.selected.Warp(new Vector2(100f, 100f), session.game.simulation);
				session.game.selected.Visible = true;
				session.ChangeState("MoveBuildingInPlacement");
			}
			Simulated simulated = (Simulated)session.CheckAsyncRequest("RequestEntityInterface");
			if (simulated != null && simulated.InteractionState != null && simulated.InteractionState.SelectedTransition != null)
			{
				session.soundEffectManager.PlaySound(simulated.entity.SoundOnSelect);
				session.game.selected = simulated;
				simulated.InteractionState.SelectedTransition.Apply(session);
			}
			bool? flag = (bool?)session.CheckAsyncRequest("ignore_request_rush_sim");
			if (!flag.HasValue || !flag.Value)
			{
				Simulated simulated2 = (Simulated)session.CheckAsyncRequest("request_rush_sim");
				if (simulated2 != null && simulated2.rushParameters != null)
				{
					Simulated.RushParameters rushParams = simulated2.rushParameters;
					Cost.CostAtTime cost = rushParams.cost;
					string subject = rushParams.subject;
					int did = rushParams.did;
					Action transition = delegate
					{
						session.ChangeState("Playing");
					};
					Action execute = delegate
					{
						rushParams.execute(session);
					};
					Action cancel = delegate
					{
						rushParams.cancel(session);
						transition();
					};
					Action<bool, Cost> logSpend = delegate(bool canAfford, Cost arg)
					{
						rushParams.log(session, arg, canAfford);
					};
					Vector2 screenPosition = rushParams.screenPosition;
					session.properties.hardSpendActions = new HardSpendActions(execute, cost, subject, did, transition, cancel, logSpend, screenPosition);
					session.game.selected = simulated2;
					session.ChangeState("HardSpendConfirm", false);
				}
			}
			else
			{
				session.AddAsyncResponse("ignore_request_rush_sim", true);
			}
			if (session.CheckAsyncRequest("ShowInventoryHudWidget") != null)
			{
				session.properties.playingHud.ShowInventoryWidget();
			}
			Simulated simulated3 = (Simulated)session.CheckAsyncRequest("mock_click_sessionaction");
			if (simulated3 != null)
			{
				SimulatedClick(simulated3, session);
			}
			if (session.properties.queuedClickedSim != null)
			{
				SimulatedClick(session.properties.queuedClickedSim, session);
			}
			session.properties.queuedClickedSim = null;
			int? num = (int?)session.CheckAsyncRequest("PulseResourceError");
			if (num.HasValue)
			{
				session.properties.playingHud.TryPulseResourceError(num.Value);
			}
			GoodToSimulatedDeliveryRequest goodToSimulatedDeliveryRequest = (GoodToSimulatedDeliveryRequest)session.CheckAsyncRequest("GoodDeliveryRequest");
			if (goodToSimulatedDeliveryRequest != null)
			{
				session.properties.playingHud.DeliverGood(goodToSimulatedDeliveryRequest);
			}
			GoodReturnRequest goodReturnRequest = (GoodReturnRequest)session.CheckAsyncRequest("GoodReturnRequest");
			if (goodReturnRequest != null)
			{
				session.properties.playingHud.ReturnGood(goodReturnRequest);
			}
			bool? flag2 = (bool?)session.CheckAsyncRequest("dialogs_to_show");
			session.AddAsyncResponse("dialogs_to_show", flag2);
			if (flag2.HasValue && flag2.Value)
			{
				session.ChangeState("ShowingDialog");
			}
		}

		public void DisappearingResourceAmount(Vector2 screenPosition, int amount)
		{
			SBGUISlidingLabel sBGUISlidingLabel = (SBGUISlidingLabel)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/DisappearingResourceAmount");
			sBGUISlidingLabel.SetParent(null);
			sBGUISlidingLabel.SetText(string.Format("+{0}", amount));
			sBGUISlidingLabel.SetScreenPosition(screenPosition);
			sBGUISlidingLabel.SetColor(new Color32(80, 40, 0, byte.MaxValue));
			sBGUISlidingLabel.AnimatedSliding(new Vector2(0f, -50f), 0f, 1f, true);
		}

		public void SimulatedClick(Simulated clickedSim, Session session)
		{
			if (clickedSim == null)
			{
				return;
			}
			clickedSim.BounceEnd();
			foreach (Action clickListener in clickedSim.ClickListeners)
			{
				clickListener();
			}
			string soundId = clickedSim.entity.SoundOnTouch;
			if (clickedSim.InteractionState.HasClickCommandFunctionality)
			{
				soundId = null;
				session.game.simulation.Router.Send(ClickedCommand.Create(Identity.Null(), clickedSim.Id), null);
				if (session.game.selected != null)
				{
					session.ChangeState("Playing");
					session.game.selected = null;
				}
			}
			else if (clickedSim.InteractionState.SelectedTransition != null)
			{
				soundId = clickedSim.entity.SoundOnSelect;
				session.game.selected = clickedSim;
				clickedSim.InteractionState.SelectedTransition.Apply(session);
			}
			else if (clickedSim.InteractionState.IsSelectable)
			{
				soundId = clickedSim.entity.SoundOnSelect;
				session.game.selected = clickedSim;
				session.ChangeState("SelectedPlaying");
			}
			else if ((int)clickedSim.Invariable["did"] == 5014)
			{
				ExplanationDialogInputData item = new ExplanationDialogInputData(Language.Get("!!JELLYFISH_FIELDS_COMING_SOON"), "Beat_JellyfishFields_ComingSoon");
				session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData> { item });
				session.AddAsyncResponse("dialogs_to_show", true);
			}
			session.soundEffectManager.PlaySound(soundId);
		}

		protected virtual void CleanupTouchingSim(Session session)
		{
			if (session.properties.touchingSim != null)
			{
				session.properties.touchingSim.BounceCleanup();
			}
			session.properties.touchingSim = null;
		}

		protected virtual void CleanupTouchingBubble(Session session)
		{
			if (session.properties.tappedDisplayController != null)
			{
				session.properties.tappedDisplayController = null;
			}
		}
	}

	public class ResolveUser : State
	{
		private SoaringContext LOAD_GAME_RESOLVE_CONTEXT;

		private SoaringContext MIGRATION_CONTEXT;

		private Dictionary<string, object> serverSave;

		private Dictionary<string, object> localSave;

		private SoaringPlayerResolver.SoaringPlayerData platform_account;

		private SoaringPlayerResolver.SoaringPlayerData last_account;

		private SoaringPlayerResolver.SoaringPlayerData device_account;

		private SaveGameScrollScreen saveGameScrollScreen;

		private SaveGameScreen saveGameScreen;

		private SaveGameScreen1 saveGameScreen1;

		private Dictionary<SoaringPlayerResolver.SoaringPlayerData, Dictionary<string, object>> deviceGameSaves;

		private string SocialNetworkMediaName;

		public void OnEnter(Session session)
		{
			GameStarting.CreateLoadingScreen(session);
			LOAD_GAME_RESOLVE_CONTEXT = null;
			platform_account = null;
			last_account = null;
			device_account = null;
			if (Language.CurrentLanguage() == LanguageCode.N)
			{
				Language.Init(TFUtils.GetPersistentAssetsPath());
			}
			if (session.Auth.AccountResolveRequired())
			{
				SoaringPlayerResolver soaringPlayerResolver = session.Auth.AccountResolver();
				platform_account = soaringPlayerResolver.ResolvePlatformData;
				last_account = soaringPlayerResolver.ResolveLastUserData;
				device_account = soaringPlayerResolver.ResolveDeviceData;
				Action okButtonHandler = delegate
				{
					SBUIBuilder.ReleaseTopScreen();
					session.TheSoundEffectManager.PlaySound("Accept");
					UserResolutionComplete(session);
				};
				if (TFUtils.isAmazon())
				{
					SocialNetworkMediaName = Language.Get("!!SAVE_GAME_AMAZON_GAME_CIRCLE");
				}
				else
				{
					SocialNetworkMediaName = Language.Get("!!SAVE_GAME_GOOGLE_PLAY");
				}
				if (platform_account == null)
				{
					if (Soaring.IsOnline)
					{
						TFUtils.DebugLog("Here we show a dialog to alert the user that they can log in with GC if they want to play the previous game played on this device.");
						string empty = string.Empty;
						SBUIBuilder.MakeAndPushConfirmationDialog(message: (!Game.GameExists(session.ThePlayer)) ? (Language.Get("!!SAVE_GAME_ALERT2_1") + " " + SocialNetworkMediaName + Language.Get("!!SAVE_GAME_ALERT2_3")) : (Language.Get("!!SAVE_GAME_ALERT2_1") + " " + SocialNetworkMediaName + Language.Get("!!SAVE_GAME_ALERT2_2")), session: session, guiEventHandler: null, title: Language.Get("!!SAVE_GAME_TITLE"), acceptLabel: Language.Get("!!SAVE_GAME_PLAY"), cancelLabel: null, resources: null, okButtonHandler: okButtonHandler, cancelButtonHandler: null);
					}
					else
					{
						string empty2 = string.Empty;
						SBUIBuilder.MakeAndPushConfirmationDialog(message: (!Game.GameExists(session.ThePlayer)) ? (Language.Get("!!SAVE_GAME_ALERT3_1") + " " + SocialNetworkMediaName + Language.Get("!!SAVE_GAME_ALERT3_3")) : (Language.Get("!!SAVE_GAME_ALERT3_1") + " " + SocialNetworkMediaName + Language.Get("!!SAVE_GAME_ALERT3_2")), session: session, guiEventHandler: null, title: Language.Get("!!SAVE_GAME_TITLE"), acceptLabel: Language.Get("!!SAVE_GAME_PLAY"), cancelLabel: null, resources: null, okButtonHandler: okButtonHandler, cancelButtonHandler: null);
						TFUtils.DebugLog("Here we show a dialog to alert the user that they can go online and log in with GC if they want to play the previous game played on this device.");
					}
				}
				else if (platform_account.loginType != device_account.loginType && last_account.loginType != device_account.loginType)
				{
					string playerName = TFUtils.GetPlayerName(Soaring.Player);
					SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!SAVE_GAME_TITLE"), playerName + Language.Get("!!SAVE_GAME_ALERT6_1") + Language.Get("!!SAVE_GAME_ALERT6_2") + " " + playerName + Language.Get("!!SAVE_GAME_ALERT6_3") + " " + SocialNetworkMediaName + " " + Language.Get("!!SAVE_GAME_ALERT6_4"), Language.Get("!!SAVE_GAME_PLAY"), null, null, okButtonHandler, null);
					TFUtils.DebugLog("Here we show a dialog to alert the user that they will be playing with a different game.");
				}
				else
				{
					TFUtils.DebugLog("-----------------!lastPlayerIsGC------------------");
					SoaringContext context = Game.CreateSoaringGameResponderContext(OnLoadRemoteGame);
					Game.LoadFromNetwork(null, session.player.ReadTimestamp(), context, session);
				}
			}
			else
			{
				TFUtils.DebugLog("Last player is the same as current. Continue unobtrusively");
				UserResolutionComplete(session);
			}
		}

		public void OnLoadRemoteGame(SoaringContext context)
		{
			LOAD_GAME_RESOLVE_CONTEXT = context;
		}

		public void OnUpdate(Session session)
		{
			if (LOAD_GAME_RESOLVE_CONTEXT != null)
			{
				RemoteGameReturned(session, LOAD_GAME_RESOLVE_CONTEXT);
				LOAD_GAME_RESOLVE_CONTEXT = null;
			}
			if (MIGRATION_CONTEXT != null)
			{
				ProcessMigrationResults(session, MIGRATION_CONTEXT);
				MIGRATION_CONTEXT = null;
			}
		}

		private void RemoteGameReturned(Session session, SoaringContext context)
		{
			SoaringError soaringError = (SoaringError)context.objectWithKey("error_message");
			SoaringArray soaringArray = (SoaringArray)context.objectWithKey("custom");
			SoaringDictionary soaringDictionary = null;
			bool flag = !Soaring.IsOnline;
			if (soaringError != null)
			{
				flag = true;
			}
			else if (soaringArray != null && soaringArray.count() != 0)
			{
				soaringDictionary = (SoaringDictionary)soaringArray.objectAtIndex(0);
			}
			if (flag)
			{
				if (Game.GameExists(session.ThePlayer))
				{
					TFUtils.DebugLog("Here we show a dialog to alert the user that they will be playing with a different game than the previous game.");
					Action okButtonHandler = delegate
					{
						SBUIBuilder.ReleaseTopScreen();
						session.TheSoundEffectManager.PlaySound("Accept");
						UserResolutionComplete(session);
					};
					SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!SAVE_GAME_TITLE"), Language.Get("!!SAVE_GAME_ALERT5"), Language.Get("!!SAVE_GAME_PLAY"), null, null, okButtonHandler, null);
				}
				else
				{
					Action okButtonHandler2 = delegate
					{
						SBUIBuilder.ReleaseTopScreen();
						session.TheSoundEffectManager.PlaySound("Accept");
					};
					SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!SAVE_GAME_TITLE"), Language.Get("!!SAVE_GAME_ALERT_FIRST_OFFLINE"), Language.Get("!!SAVE_GAME_OK"), null, null, okButtonHandler2, null);
				}
				return;
			}
			if (Game.GameExists(session.ThePlayer) || soaringDictionary != null)
			{
				TFUtils.DebugLog("Different player, can migrate device to local or use remote or use local gc game");
				LoadAndPresentAllPossibleGamestates(session, soaringDictionary);
				return;
			}
			Action okButtonHandler3 = delegate
			{
				SBUIBuilder.ReleaseTopScreen();
				session.TheSoundEffectManager.PlaySound("Accept");
				MigratePlayer(session, last_account, null);
			};
			SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!SAVE_GAME_TITLE"), TFUtils.GetPlayerName(Soaring.Player) + Language.Get("!!SAVE_GAME_ALERT45_1") + " " + SocialNetworkMediaName + Language.Get("!!SAVE_GAME_ALERT45_2") + " " + SocialNetworkMediaName + Language.Get("!!SAVE_GAME_ALERT45_3"), Language.Get("!!SAVE_GAME_PLAY"), null, null, okButtonHandler3, null);
			TFUtils.DebugLog("Different player, no existing game. Auto migrate.");
		}

		private void LoadAndPresentAllPossibleGamestates(Session session, SoaringDictionary sessionSaveData)
		{
			if (sessionSaveData != null)
			{
				TFUtils.DebugLog("There is a later version of the user's save game on the server");
				if (SBSettings.UseLegacyGameLoad)
				{
					TFUtils.DebugLog("Loading Game In Legacy Mode");
					serverSave = (Dictionary<string, object>)Json.Deserialize(sessionSaveData.ToJsonString());
				}
				else
				{
					serverSave = SBMISoaring.ConvertDictionaryToGeneric(sessionSaveData);
				}
			}
			else
			{
				TFUtils.DebugLog("There is a NOT a later version of the user's save game on the server");
			}
			if (Game.GameExists(session.ThePlayer))
			{
				string json = TFUtils.ReadAllText(session.ThePlayer.CacheFile("game.json"));
				localSave = (Dictionary<string, object>)Json.Deserialize(json);
			}
			deviceGameSaves = gatherLocalDeviceSaves();
			PresentGameOptions(session);
		}

		private void PresentGameOptions(Session session)
		{
			alert(session);
			TFUtils.DebugLog("These are the options for games:");
			if (serverSave != null)
			{
				TFUtils.DebugLog("----------serverSave != null----------");
				Dictionary<string, object> dictionary = (Dictionary<string, object>)serverSave["playtime"];
				TFUtils.DebugLog("Found server game that is different than the local game " + session.ThePlayer.playerId + " with server game level " + dictionary["level"]);
			}
			else
			{
				TFUtils.DebugLog("The server save is either not found or the same as the local player game.");
			}
			if (localSave != null)
			{
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)localSave["playtime"];
				TFUtils.DebugLog("Found local game " + session.ThePlayer.playerId + " with local game level " + dictionary2["level"]);
			}
			else
			{
				TFUtils.DebugLog("There is no local game for this player. It is either loading one from the server or creating a new game");
			}
			foreach (KeyValuePair<SoaringPlayerResolver.SoaringPlayerData, Dictionary<string, object>> deviceGameSafe in deviceGameSaves)
			{
				Dictionary<string, object> dictionary3 = (Dictionary<string, object>)deviceGameSafe.Value["playtime"];
				TFUtils.DebugLog("Found device player " + deviceGameSafe.Key.soaringTag + " with local game level " + dictionary3["level"]);
			}
		}

		private void alert(Session session)
		{
			Dictionary<string, object> gcSave = null;
			if (serverSave != null)
			{
				gcSave = serverSave;
			}
			else
			{
				gcSave = localSave;
			}
			TFUtils.GameDetails details = null;
			TFUtils.GameDetails lastDetails = null;
			TFUtils.DebugLog("GCGame: " + TFUtils.ParseGameDetails(gcSave, ref details));
			string json = TFUtils.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + last_account.soaringTag + Path.DirectorySeparatorChar + "game.json");
			Dictionary<string, object> gameData = (Dictionary<string, object>)Json.Deserialize(json);
			TFUtils.DebugLog("LastGame: " + TFUtils.ParseGameDetails(gameData, ref lastDetails));
			if (details.lastPlayTime == lastDetails.lastPlayTime)
			{
				TFUtils.DebugLog("Games are Identical, Use Server");
				SelectSaveGame(session, gcSave);
				return;
			}
			saveGameScreen = (SaveGameScreen)SBGUI.InstantiatePrefab("Prefabs/SaveGame");
			Action local = delegate
			{
				saveGameScreen.SetActive(false);
				Action okButtonHandler = delegate
				{
					SBUIBuilder.ReleaseTopScreen();
					saveGameScreen.Deactivate();
					session.TheSoundEffectManager.PlaySound("Accept");
					MigratePlayer(session, last_account, null);
				};
				Action cancelButtonHandler = delegate
				{
					SBUIBuilder.ReleaseTopScreen();
					saveGameScreen.SetActive(true);
				};
				string text = null;
				string format = "||Level:{0} Jelly:{1} Money:{2}";
				text = Language.Get("!!SAVE_GAME_ALERT4_LINK_1") + TFUtils.GetPlayerName(Soaring.Player, " {0} ") + Language.Get("!!SAVE_GAME_ALERT4_LINK_2") + " " + SocialNetworkMediaName + Language.Get("!!SAVE_GAME_ALERT4_LINK_3");
				text += string.Format(format, lastDetails.level, lastDetails.jelly, lastDetails.money);
				SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!SAVE_GAME_TITLE"), text, Language.Get("!!SAVE_GAME_YES"), Language.Get("!!SAVE_GAME_NO"), null, okButtonHandler, cancelButtonHandler);
			};
			Action server = delegate
			{
				saveGameScreen.Deactivate();
				session.TheSoundEffectManager.PlaySound("Accept");
				SelectSaveGame(session, gcSave);
			};
			saveGameScreen.SetUp(TFUtils.GetPlayerName(Soaring.Player, "{0} ") + Language.Get("!!SAVE_GAME_TEXT1"), Language.Get("!!SAVE_GAME_TEXT2"), Language.Get("!!SAVE_GAME_ALERT4"), Language.Get("!!SAVE_GAME_SAVE_ON_SERVER"), details.level, details.money, details.jelly, details.patties, details.dtLastPlayTime, Language.Get("!!SAVE_GAME_KEEP_AND_PLAY"), Language.Get("!!SAVE_GAME_SAVE_ON_DEVICE"), lastDetails.level, lastDetails.money, lastDetails.jelly, lastDetails.jelly, lastDetails.dtLastPlayTime, Language.Get("!!SAVE_GAME_LINK_AND_PLAY"), server, local, session);
		}

		private void SelectSaveGame(Session session, Dictionary<string, object> selectedGame)
		{
			TFUtils.DebugLog("Saving selected game");
			if (deviceGameSaves.ContainsValue(selectedGame))
			{
				TFUtils.DebugLog("Attempting to migrate local games");
				{
					foreach (KeyValuePair<SoaringPlayerResolver.SoaringPlayerData, Dictionary<string, object>> deviceGameSafe in deviceGameSaves)
					{
						if (deviceGameSafe.Value == selectedGame)
						{
							TFUtils.DebugLog("Migrating player games: " + deviceGameSafe.Key);
							MigratePlayer(session, deviceGameSafe.Key, null);
							break;
						}
					}
					return;
				}
			}
			if (selectedGame == serverSave)
			{
				string path = session.ThePlayer.CacheFile(PersistedActionBuffer.ACTION_LIST_FILE);
				if (File.Exists(path))
				{
					File.WriteAllText(path, string.Empty);
				}
				string contents = Json.Serialize(selectedGame);
				File.WriteAllText(session.ThePlayer.CacheFile("game.json"), contents);
				TFUtils.DebugLog("Overwriting local game with the server game.");
				UserResolutionComplete(session);
			}
			else if (selectedGame == localSave && serverSave != null)
			{
				TFUtils.DebugLog("Overwriting server game with the local game.");
				string gameData = Json.Serialize(selectedGame);
				session.ThePlayer.DeleteTimestamp();
				SoaringContext soaringContext = Game.CreateSoaringGameResponderContext(OnSaveComplete);
				soaringContext.addValue(new SoaringObject(session), "session");
				session.WebFileServer.SaveGameData(gameData, soaringContext);
			}
			else if (selectedGame == localSave)
			{
				TFUtils.DebugLog("No action - user selected local save (and it is the same as the server) so just will continue.");
				UserResolutionComplete(session);
			}
			else
			{
				TFUtils.ErrorLog("Attempting to save an unknown gamestate.");
			}
		}

		private Dictionary<SoaringPlayerResolver.SoaringPlayerData, Dictionary<string, object>> gatherLocalDeviceSaves()
		{
			Dictionary<SoaringPlayerResolver.SoaringPlayerData, Dictionary<string, object>> dictionary = new Dictionary<SoaringPlayerResolver.SoaringPlayerData, Dictionary<string, object>>();
			SoaringArray usersArray = SoaringPlayerResolver.UsersArray;
			if (usersArray == null)
			{
				return dictionary;
			}
			int num = usersArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = (SoaringPlayerResolver.SoaringPlayerData)usersArray.objectAtIndex(i);
				if (soaringPlayerData.loginType == SoaringLoginType.Soaring || (soaringPlayerData.loginType == SoaringLoginType.Device && !string.IsNullOrEmpty(soaringPlayerData.userID)))
				{
					Player player = new Player(soaringPlayerData.soaringTag);
					string text = player.CacheFile("game.json");
					if (File.Exists(text))
					{
						string json = TFUtils.ReadAllText(text);
						Dictionary<string, object> value = (Dictionary<string, object>)Json.Deserialize(json);
						dictionary[soaringPlayerData] = value;
					}
				}
			}
			return dictionary;
		}

		public void OnLeave(Session session)
		{
			LOAD_GAME_RESOLVE_CONTEXT = null;
			localSave = null;
			deviceGameSaves = null;
			serverSave = null;
			platform_account = null;
			last_account = null;
			device_account = null;
		}

		private void MigratePlayer(Session session, SoaringPlayerResolver.SoaringPlayerData sourceAccount, SoaringPlayerResolver.SoaringPlayerData targetAccount)
		{
			SoaringContext soaringContext = Game.CreateSoaringGameResponderContext(OnMigrationComplete);
			SoaringLoginType soaringLoginType = SoaringLoginType.Device;
			SoaringLoginType soaringLoginType2 = SoaringLoginType.Device;
			string srcPlayerID = null;
			string targetPlayerID = null;
			if (sourceAccount != null)
			{
				srcPlayerID = sourceAccount.platformID;
				soaringLoginType = sourceAccount.loginType;
				if (soaringLoginType == SoaringLoginType.Soaring)
				{
					soaringLoginType = SoaringLoginType.Device;
				}
				soaringContext.addValue(sourceAccount, "user_data");
			}
			else if (targetAccount != null)
			{
				targetPlayerID = targetAccount.platformID;
				soaringLoginType2 = targetAccount.loginType;
				if (soaringLoginType2 == SoaringLoginType.Soaring)
				{
					soaringLoginType2 = SoaringLoginType.Device;
				}
				soaringContext.addValue(targetAccount, "user_data");
			}
			SBMISoaring.MigratePlayerToNewPlayer(srcPlayerID, soaringLoginType, targetPlayerID, soaringLoginType2, soaringContext);
		}

		public void OnMigrationComplete(SoaringContext context)
		{
			MIGRATION_CONTEXT = context;
		}

		public void ProcessMigrationResults(Session session, SoaringContext context)
		{
			session.auth.AccountResolved();
			SoaringError soaringError = (SoaringError)context.objectWithKey("error_message");
			if (soaringError != null || !Soaring.IsOnline)
			{
				TFUtils.DebugLog("Show dialog saying that the migration failed and you must be online (if the user is offline)");
				TFUtils.ErrorLog("ProcessMigrationResults: " + soaringError);
				Action okButtonHandler = delegate
				{
					SBUIBuilder.ReleaseTopScreen();
					UserResolutionComplete(session);
				};
				SBUIBuilder.MakeAndPushConfirmationDialog(session, null, "Migration Error", "An error occured in the migrations", Language.Get("!!PREFAB_OK"), null, null, okButtonHandler, null);
				TFUtils.LogDump(session, "migration_error");
				return;
			}
			SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = (SoaringPlayerResolver.SoaringPlayerData)context.objectWithKey("user_data");
			if (soaringPlayerData.soaringTag != Soaring.Player.UserTag)
			{
				SoaringPlayerResolver.RemovePlayer(soaringPlayerData);
				try
				{
					string text = Application.persistentDataPath + Path.DirectorySeparatorChar + Soaring.Player.UserTag + Path.DirectorySeparatorChar;
					if (Directory.Exists(text))
					{
						Directory.Delete(text, true);
					}
					string text2 = Application.persistentDataPath + Path.DirectorySeparatorChar + soaringPlayerData.soaringTag + Path.DirectorySeparatorChar;
					if (Directory.Exists(text2))
					{
						Directory.Move(text2, text);
					}
					if (Directory.Exists(text2))
					{
						Directory.Delete(text2, true);
					}
				}
				catch (Exception ex)
				{
					SoaringDebug.Log("MigrationError: " + ex.Message);
				}
			}
			TFUtils.DebugLog("Migrating Player " + Soaring.Player.UserTag + " : " + session.ThePlayer.playerId + "\nwith " + soaringPlayerData.soaringTag + " : " + soaringPlayerData.userID);
			SoaringPlayerResolver.Save(Soaring.Player.UserTag);
			session.ChangeState("Authorizing");
		}

		private void OnSaveComplete(SoaringContext context)
		{
			bool flag = context.soaringValue("status");
			SoaringError soaringError = (SoaringError)context.objectWithKey("error_message");
			SoaringDictionary soaringDictionary = (SoaringDictionary)context.objectWithKey("custom");
			Session session = (Session)((SoaringObject)context.objectWithKey("session")).Object;
			if (flag)
			{
				if (soaringDictionary != null)
				{
					long num = soaringDictionary.soaringValue("datetime");
					if (Player.ValidTimeStamp(num))
					{
						session.player.SetStagedTimestamp(num);
					}
				}
			}
			else if (soaringError != null)
			{
				SoaringDebug.Log(soaringError, LogType.Error);
			}
			UserResolutionComplete(session);
		}

		private void UserResolutionComplete(Session session)
		{
			session.ChangeState("CheckPatching");
		}
	}

	public class SelectedPlaying : Playing
	{
		public static string TASK_CHARACTER_SELECT = "task_character_select";

		protected TimebarGroup timebarGroup = new TimebarGroup();

		protected NamebarGroup m_pNamebarGroup = new NamebarGroup();

		protected InteractionStripMixin interactionStrip = new InteractionStripMixin();

		public override void OnEnter(Session session)
		{
			base.OnEnter(session);
			SBUIBuilder.MakeAndPushEmptyUI(session, HandleSBGUIEvent);
			Simulated selected = session.game.selected;
			TFUtils.DebugLog("Selected simulated with did " + selected.entity.DefinitionId);
			if (selected.InteractionState.Controls != null && selected.InteractionState.Controls.Count > 0 && !selected.InteractionState.IsEditable)
			{
				interactionStrip.ActivateOnSelected(session);
				interactionStrip.OnUpdate(session);
			}
			timebarGroup.ActivateOnSelected(session);
			m_pNamebarGroup.ActivateOnSelected(session);
			session.properties.m_pTaskSimulated = null;
			session.properties.m_bAutoPanToSimulatedOnLeave = false;
		}

		public override void OnLeave(Session session)
		{
			DeactivateTimeBarAndInteractionStrip(session);
			SBUIBuilder.ReleaseTopScreen();
			if (session.properties.m_pTaskSimulated != null)
			{
				session.game.selected = session.properties.m_pTaskSimulated;
			}
			if (session.game.selected != null && session.properties.m_bAutoPanToSimulatedOnLeave)
			{
				session.TheCamera.AutoPanToPosition(session.game.selected.PositionCenter, 0.75f);
			}
			base.OnLeave(session);
		}

		public override void OnUpdate(Session session)
		{
			base.OnUpdate(session);
			if (session.game.selected != null && session.game.selected.InteractionState.Controls != null && session.game.selected.InteractionState.Controls.Count > 0)
			{
				interactionStrip.OnUpdate(session);
			}
			int? num = (int?)session.CheckAsyncRequest(TASK_CHARACTER_SELECT);
			if (!num.HasValue)
			{
				return;
			}
			Simulated simulated = session.TheGame.simulation.FindSimulated(num.Value);
			if (simulated == null)
			{
				return;
			}
			session.properties.m_pTaskSimulated = simulated;
			TaskManager taskManager = session.TheGame.taskManager;
			List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id);
			if (activeTasksForSimulated.Count > 0 && activeTasksForSimulated[0].GetTimeLeft() != 0)
			{
				if (taskManager.GetTaskingStateForSimulated(session.TheGame.simulation, num.Value, simulated.Id) == TaskManager._eBlueprintTaskingState.eNone)
				{
					session.ChangeState("UnitIdle");
				}
				else
				{
					session.ChangeState("UnitBusy");
				}
			}
			else
			{
				session.properties.m_bAutoPanToSimulatedOnLeave = true;
				session.ChangeState("Playing");
			}
		}

		public override void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			bool flag = true;
			YGEvent.TYPE type = evt.type;
			if (type == YGEvent.TYPE.TOUCH_END)
			{
				if (session.game.simulation.Whitelisted)
				{
					return;
				}
				if (session.properties.touchingSim == null || session.properties.touchingSim == session.game.selected)
				{
					flag = false;
					session.ChangeState("Playing");
					CleanupTouchingSim(session);
					session.game.selected = null;
				}
			}
			if (flag)
			{
				base.HandleSBGUIEvent(evt, session);
			}
		}

		private void DeactivateTimeBarAndInteractionStrip(Session session)
		{
			timebarGroup.Deactivate(session);
			m_pNamebarGroup.Deactivate(session);
			interactionStrip.Deactivate(session);
		}
	}

	public class SellBuildingConfirmation : State
	{
		public void OnEnter(Session session)
		{
			Setup(session);
			session.camera.SetEnableUserInput(false);
		}

		public void Setup(Session session)
		{
			string text = null;
			object obj = session.CheckAsyncRequest("sell_error");
			if (obj != null)
			{
				text = (string)obj;
			}
			Simulated toSell = (Simulated)session.CheckAsyncRequest("to_sell");
			object obj2 = session.CheckAsyncRequest("in_state_move_in_edit");
			bool bMoveInEdit = false;
			if (obj2 != null)
			{
				bMoveInEdit = (bool)obj2;
			}
			if (session.TheState.GetType().Equals(typeof(MoveBuildingInEdit)))
			{
				MoveBuildingInEdit moveBuildingInEdit = (MoveBuildingInEdit)session.TheState;
				moveBuildingInEdit.DeactivateInteractionStrip(session);
			}
			BuildingEntity entity = toSell.GetEntity<BuildingEntity>();
			if (string.IsNullOrEmpty(text))
			{
				Action okButtonHandler = delegate
				{
					SellSimulated(session, toSell);
				};
				Action cancelButtonHandler = delegate
				{
					session.TheSoundEffectManager.PlaySound("CloseMenu");
					session.ChangeState((!bMoveInEdit) ? "MoveBuildingInPlacement" : "MoveBuildingInEdit");
				};
				Cost sellCost = session.game.catalog.GetSellCost(entity.DefinitionId);
				session.TheSoundEffectManager.PlaySound("OpenMenu");
				SBUIBuilder.MakeAndPushConfirmationDialog(session, HandleSBGUIEvent, Language.Get("!!SELL_CONFIRMATION_TITLE"), Language.Get("!!SELL_CONFIRMATION_MESSAGE"), Language.Get("!!PREFAB_OK"), Language.Get("!!PREFAB_NEVERMIND"), Cost.DisplayDictionary(sellCost.ResourceAmounts, session.TheGame.resourceManager), okButtonHandler, cancelButtonHandler);
			}
			else
			{
				Action okButtonHandler2 = delegate
				{
					session.TheSoundEffectManager.PlaySound("CloseMenu");
					session.ChangeState((!bMoveInEdit) ? "MoveBuildingInPlacement" : "MoveBuildingInEdit");
				};
				SBUIBuilder.MakeAndPushAcknowledgeDialog(session, HandleSBGUIEvent, Language.Get("!!CANNOT_SELL_DIALOG_TITLE"), Language.Get(text), (string)entity.Invariable["portrait"], Language.Get("!!PREFAB_OK"), okButtonHandler2);
			}
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, false);
		}

		private void SellSimulated(Session session, Simulated toSell)
		{
			bool? flag = (bool?)session.CheckAsyncRequest("FromInventory");
			if (toSell.entity is DebrisEntity)
			{
				TFUtils.Assert(debugManager.debugPlaceObjects, "You shouldn't be here...");
				DebrisEntity debris = (DebrisEntity)toSell.entity;
				if (debris.ExpansionId.HasValue)
				{
					Dictionary<int, TerrainSlot> expansionSlots = session.TheGame.terrain.ExpansionSlots;
					TerrainSlot terrainSlot = expansionSlots[debris.ExpansionId.Value];
					int num = terrainSlot.debris.FindIndex((TerrainSlotObject obj) => obj.id == debris.Id);
					if (num >= 0)
					{
						terrainSlot.debris.RemoveAt(num);
					}
				}
				toSell.SetFootprint(session.game.simulation, false);
				session.game.simulation.RemoveSimulated(toSell);
				if (flag.HasValue)
				{
					session.AddAsyncResponse("FromInventory", flag.Value);
				}
				if ((flag.HasValue && flag.Value) || session.properties.cameFromMarketplace)
				{
					session.ChangeState("Playing");
				}
				else
				{
					session.ChangeState("Editing");
				}
				session.game.selected = null;
				return;
			}
			session.TheSoundEffectManager.PlaySound("Sell");
			session.game.selected.FootprintVisible = false;
			BuildingEntity entity = toSell.GetEntity<BuildingEntity>();
			SwarmManager.Instance.RestoreResidents(session.game.simulation, toSell);
			Cost sellCost = session.game.catalog.GetSellCost(entity.DefinitionId);
			session.game.simulation.ModifyGameStateSimulated(toSell, new SellAction(toSell, sellCost));
			session.game.resourceManager.SellFor(sellCost, session.game);
			Identity id = toSell.Id;
			session.game.simulation.TryWorkerSpawnerCleanup(id);
			List<Simulated> list = Simulated.Building.FindResidents(session.game.simulation, toSell);
			foreach (Simulated item in list)
			{
				SwarmManager.Instance.RemoveResident(item.GetEntity<ResidentEntity>(), toSell);
				session.game.simulation.RemoveSimulated(item);
			}
			session.game.simulation.RemoveSimulated(toSell);
			session.game.entities.Destroy(id);
			session.analytics.LogSell(toSell.entity.BlueprintName, toSell.entity.DefinitionId >= 2000 && toSell.entity.DefinitionId < 3000, sellCost, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
			session.game.selected = null;
			if (flag.HasValue)
			{
				session.AddAsyncResponse("FromInventory", flag.Value);
			}
			if ((flag.HasValue && flag.Value) || session.properties.cameFromMarketplace)
			{
				session.ChangeState("Playing");
			}
			else
			{
				session.ChangeState("Editing");
			}
		}
	}

	public class Shopping : State
	{
		private const string SHOPPING_UI_HANDLER = "shopping_ui";

		private static SBMarketOffer hackLastOffer;

		public void OnEnter(Session session)
		{
			bool isJellyPurchase = false;
			session.game.dropManager.MarkForClearCurrentDrops();
			session.marketpalceActive = true;
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			session.musicManager.PlayTrack("InGame");
			session.PlayBubbleScreenSwipeEffect();
			session.camera.SetEnableUserInput(false);
			session.TheCamera.TurnOnScreenBuffer();
			Action action = delegate
			{
				session.ChangeState("Playing");
				session.properties.m_sLeaveType = "store_close_back_button";
				AndroidBack.getInstance().pop();
			};
			Action inventoryClickHandler = delegate
			{
				if (!session.game.featureManager.CheckFeature("inventory_soft"))
				{
					session.game.featureManager.UnlockFeature("inventory_soft");
					session.game.featureManager.ActivateFeatureActions(session.game, "inventory_soft");
					session.game.simulation.ModifyGameState(new FeatureUnlocksAction(new List<string> { "inventory_soft" }));
				}
				else
				{
					session.ChangeState("Inventory");
				}
			};
			Action optionsHandler = delegate
			{
				session.ChangeState("Options");
			};
			Action editClickHandler = delegate
			{
				session.ChangeState("Editing");
			};
			Action<SBMarketOffer> purchaseClickHandler = delegate(SBMarketOffer offer)
			{
				switch (offer.type)
				{
				case "rmt":
					if (Application.internetReachability == NetworkReachability.NotReachable || !Soaring.IsOnline)
					{
						session.TheSoundEffectManager.PlaySound("Error");
						string message2 = TFUtils.AssignStorePlatformText("!!ERROR_NETWORK_NEEDED_FOR_RMT");
						ExplanationDialogInputData item2 = new ExplanationDialogInputData(message2, "Beat_JellyfishFields_ComingSoon");
						session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData> { item2 });
						session.ChangeState("ShowingDialog");
						session.properties.m_sLeaveType = "store_close_known_error_iap";
					}
					else if (RmtStore.IsPurchasing)
					{
						Action okAction3 = delegate
						{
							session.AddAsyncResponse("store_open_type", "store_open_iap_error_return");
							session.ChangeState("Shopping");
						};
						session.ErrorMessageHandler(session, Language.Get("!!PRODUCT_IAP_IS_BUSY_TITLE"), Language.Get("!!PRODUCT_IAP_IS_BUSY_TEXT"), Language.Get("!!PREFAB_OK"), okAction3, 0.75f);
					}
					else if (session.game.store.RmtReady)
					{
						if (!session.game.store.rmtProducts.ContainsKey(offer.innerOffer))
						{
							TFUtils.DebugLog("Failed to find product for " + offer.innerOffer);
						}
						string innerOffer = offer.innerOffer;
						session.properties.iapBundleName = innerOffer;
						session.game.store.OpenTransaction(innerOffer);
						session.AddAsyncResponse("transaction_offer", offer);
						session.ChangeState("InAppPurchasing");
						session.properties.m_sLeaveType = "store_close_purchase_iap";
					}
					else
					{
						session.ChangeState("Playing");
						session.properties.m_sLeaveType = "store_close_unknown_error_iap";
					}
					break;
				case "path":
					session.ChangeState("Paving");
					session.properties.m_sLeaveType = "store_close_road_purchase_start";
					break;
				case "expansion":
					session.ChangeState("Expanding");
					session.properties.m_sLeaveType = "expanding";
					break;
				case "resource":
				{
					SBUIBuilder.ReleaseTopScreen();
					int rmtCost = offer.cost[ResourceManager.HARD_CURRENCY];
					if (session.game.resourceManager.HasEnough(ResourceManager.HARD_CURRENCY, rmtCost))
					{
						session.TheSoundEffectManager.PlaySound("OpenMenu");
						Action cancelButtonHandler2 = delegate
						{
							session.TheSoundEffectManager.PlaySound("Error");
							SBUIBuilder.ReleaseTopScreen();
							session.AddAsyncResponse("store_open_type", "store_open_too_poor_return");
							session.ChangeState("Shopping");
						};
						Action okButtonHandler2 = delegate
						{
							session.TheSoundEffectManager.PlaySound("Purchase");
							SBUIBuilder.ReleaseTopScreen();
							TFUtils.Assert(offer.cost.ContainsKey(ResourceManager.HARD_CURRENCY), "Anything purchased in the Marketplace needs RMT for now");
							TFUtils.Assert(offer.data != null && offer.data.Count > 0, "Missing resource data for this offer: " + offer.itemName);
							session.TheGame.resourceManager.PurchaseResourcesWithHardCurrency(rmtCost, new Cost(offer.data), session.game);
							session.game.simulation.ModifyGameState(new PurchaseResourcesAction(new Identity(), rmtCost, new Cost(offer.data)));
							session.ChangeState("Playing");
							session.properties.m_sLeaveType = "store_close_purchase_iap";
						};
						SBUIBuilder.MakeAndPushConfirmationDialog(session, HandleSBGUIEvent, Language.Get("!!PREFAB_CONFIRM_PURCHASE_TITLE"), Language.Get("!!PREFAB_CONFIRM_PURCHASE_MESSAGE"), Language.Get("!!PREFAB_OK"), Language.Get("!!PREFAB_NEVERMIND"), Cost.DisplayDictionary(offer.cost, session.TheGame.resourceManager), okButtonHandler2, cancelButtonHandler2);
					}
					else
					{
						Action action4 = delegate
						{
							session.AddAsyncResponse("store_open_type", "store_open_too_poor_return");
							session.ChangeState("Shopping");
						};
						string title = (string)session.CheckAsyncRequest("jelly_message_title");
						string message = (string)session.CheckAsyncRequest("jelly_message");
						string question = (string)session.CheckAsyncRequest("jelly_question");
						string acceptLabel = (string)session.CheckAsyncRequest("jelly_message_ok_label");
						string cancelLabel = (string)session.CheckAsyncRequest("jelly_message_cancel_label");
						Action okButtonHandler3 = (Action)session.CheckAsyncRequest("jelly_message_ok_action");
						Action cancelButtonHandler3 = (Action)session.CheckAsyncRequest("jelly_message_cancel_action");
						SBGUIGetJellyDialog sBGUIGetJellyDialog = SBUIBuilder.MakeAndPushGetJellyDialog(session, HandleSBGUIEvent, title, message, question, acceptLabel, cancelLabel, null, okButtonHandler3, cancelButtonHandler3, true);
					}
					break;
				}
				case "building":
				case "annex":
				{
					EntityType type = EntityType.BUILDING;
					if (offer.type == "annex")
					{
						type = EntityType.ANNEX;
					}
					Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(offer.cost, session.game.resourceManager);
					if (resourcesStillRequired.Count > 0)
					{
						session.TheSoundEffectManager.PlaySound("Error");
						Session temp = session;
						Action okAction2 = delegate
						{
							if (!session.game.simulation.Whitelisted || hackLastOffer != offer)
							{
								hackLastOffer = offer;
								temp.TheSoundEffectManager.PlaySound("Purchase");
								temp.game.selected = temp.game.simulation.CreateSimulated(type, offer.identity, Vector2.zero);
								if (type == EntityType.BUILDING)
								{
									temp.game.selected.GetEntity<BuildingEntity>().Slots = temp.game.craftManager.GetInitialSlots(offer.identity);
								}
								temp.game.selected.Visible = true;
								temp.ChangeState("Placing");
								session.properties.m_sLeaveType = "store_close_item_purchase_start";
							}
						};
						Action cancelAction = delegate
						{
							session.AddAsyncResponse("store_open_type", "store_open_too_poor_return");
							session.ChangeState("Shopping");
						};
						string itemName = ((offer.itemName == null) ? string.Format("purchase {0} {1}", offer.type, offer.identity) : ("purchase " + offer.itemName));
						session.properties.m_sLeaveType = "store_close_im_broke";
						session.InsufficientResourcesHandler(session, itemName, offer.identity, okAction2, cancelAction, new Cost(offer.cost));
					}
					else if (!session.game.simulation.Whitelisted || hackLastOffer != offer)
					{
						hackLastOffer = offer;
						session.TheSoundEffectManager.PlaySound("Purchase");
						EntityType types = EntityTypeNamingHelper.StringToType(offer.type);
						session.game.selected = session.game.simulation.CreateSimulated(types, offer.identity, Vector2.zero);
						session.game.selected.Visible = true;
						session.ChangeState("Placing");
						session.properties.m_sLeaveType = "store_close_item_purchase_start";
					}
					break;
				}
				case "costume":
				{
					int currency = 0;
					int costumeCost = 0;
					Simulated pSimulated = session.TheGame.simulation.FindSimulated(session.TheGame.costumeManager.GetCostume(offer.identity).m_nUnitDID);
					if (offer.cost.ContainsKey(ResourceManager.SOFT_CURRENCY))
					{
						costumeCost = offer.cost[ResourceManager.SOFT_CURRENCY];
						currency = ResourceManager.SOFT_CURRENCY;
						isJellyPurchase = false;
					}
					else if (offer.cost.ContainsKey(ResourceManager.HARD_CURRENCY))
					{
						costumeCost = offer.cost[ResourceManager.HARD_CURRENCY];
						currency = ResourceManager.HARD_CURRENCY;
						isJellyPurchase = true;
					}
					if (session.game.resourceManager.HasEnough(currency, costumeCost))
					{
						SBUIBuilder.ReleaseTopScreen();
						bool isInInventory = false;
						session.TheSoundEffectManager.PlaySound("OpenMenu");
						Action cancelButtonHandler = delegate
						{
							session.TheSoundEffectManager.PlaySound("Error");
							SBUIBuilder.ReleaseTopScreen();
							session.AddAsyncResponse("store_open_type", "store_open_too_poor_return");
							session.ChangeState("Shopping");
							if (!isJellyPurchase)
							{
							}
						};
						Action okButtonHandler = delegate
						{
							session.TheSoundEffectManager.PlaySound("Purchase");
							SBUIBuilder.ReleaseTopScreen();
							if (!session.TheGame.costumeManager.IsCostumeUnlocked(offer.identity))
							{
								session.TheGame.resourceManager.Spend(currency, costumeCost, session.game);
								ResourceManager.ApplyCostToGameState(currency, costumeCost, session.TheGame.gameState);
								session.TheGame.costumeManager.UnlockCostume(offer.identity);
								session.TheGame.simulation.ModifyGameState(new UnlockCostumeAction(offer.identity));
								CostumeManager.Costume costume = session.TheGame.costumeManager.GetCostume(offer.identity);
								if (costume != null)
								{
									AnalyticsWrapper.LogCostumePurchased(session.game, costume, currency, costumeCost);
									session.game.analytics.LogCostumeUnlocked(costume.m_nDID);
									AnalyticsWrapper.LogCostumeUnlocked(session.game, costume);
								}
								if (isJellyPurchase)
								{
								}
								if ((!session.game.inventory.HasItem(1015) && (offer.identity == 2 || offer.identity == 21)) || (!session.game.inventory.HasItem(1017) && (offer.identity == 7 || offer.identity == 24)) || (!session.game.inventory.HasItem(1016) && offer.identity == 9) || (!session.game.inventory.HasItem(1013) && offer.identity == 23))
								{
									isInInventory = false;
									session.game.selected = pSimulated;
									session.game.selected = pSimulated;
									session.AddAsyncResponse("purchasedCostume", offer.identity);
									session.properties.m_sLeaveType = "store_close_purchase_iap";
									session.ChangeState("UnitIdle");
								}
								else
								{
									isInInventory = true;
									Action okAction4 = delegate
									{
										session.ChangeState("Playing");
									};
									session.ErrorMessageHandler(session, Language.Get("!!ERROR_CHARACTER_INVENTORY_TITLE"), Language.Get("!!ERROR_CHARACTER_INVENTORY"), Language.Get("!!PREFAB_OK"), okAction4, 0.9f);
								}
							}
							session.properties.storeVisitSinceLastPurchase = 0;
						};
						if (!isInInventory)
						{
							string text = string.Empty;
							if (offer.identity == 2 || offer.identity == 21)
							{
								text = "SpongeBob";
							}
							else if (offer.identity == 7 || offer.identity == 24)
							{
								text = "Patrick";
							}
							else if (offer.identity == 9)
							{
								text = "Squidward";
							}
							else if (offer.identity == 23)
							{
								text = "Mr. Krabs";
							}
							SBUIBuilder.MakeAndPushConfirmationDialog(session, HandleSBGUIEvent, Language.Get(session.TheGame.costumeManager.GetCostume(offer.identity).m_sName), Language.Get("!!PREFAB_CONFIRM_COSTUME_PURCHASE_MESSAGE") + "|      " + Language.Get(text.ToString()), Language.Get("!!PREFAB_OK"), Language.Get("!!PREFAB_NEVERMIND"), Cost.DisplayDictionary(offer.cost, session.TheGame.resourceManager), okButtonHandler, cancelButtonHandler);
						}
					}
					else
					{
						Action okAction = delegate
						{
							session.AddAsyncResponse("store_open_type", "store_open_too_poor_return");
							session.ChangeState("Shopping");
						};
						session.ErrorMessageHandler(session, Language.Get("!!PREFAB_NOT_ENOUGH") + " " + Language.Get(session.TheGame.resourceManager.Resources[currency].Name), Language.Get("!!PREFAB_NOT_ENOUGH_JJ_FOR_COINS"), Language.Get("!!PREFAB_OK"), okAction, 1.1f);
					}
					break;
				}
				default:
					TFUtils.Assert(false, "Unsupported Marketplace type: " + offer.type);
					break;
				}
			};
			Action action2 = delegate
			{
			};
			session.properties.shoppingHud = SBUIBuilder.MakeAndPushStandardUI(session, false, HandleSBGUIEvent, action, inventoryClickHandler, optionsHandler, editClickHandler, null, DragFeeding.SwitchToFn(session), null, action2, action2, delegate
			{
				session.ChangeState("CommunityEvent");
			}, null);
			session.properties.shoppingHud.SetVisibleNonEssentialElements(false, true);
			SBGUIPulseButton sBGUIPulseButton = (SBGUIPulseButton)session.properties.shoppingHud.FindChild("marketplace");
			session.properties.marketplaceSessionActionID = sBGUIPulseButton.SessionActionId;
			sBGUIPulseButton.SessionActionId = session.properties.marketplaceSessionActionID + "_in_store";
			SBGUIMarketplaceScreen item = SBUIBuilder.MakeAndPushMarketplaceDialog(session, HandleSBGUIEvent, action, purchaseClickHandler, session.game.entities, session.game.resourceManager, session.TheSoundEffectManager, session.game.catalog);
			Action action3 = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action3;
			session.game.questManager.OnShowDialogCallback = action3;
			session.game.communityEventManager.DialogNeededCallback = action3;
			session.game.sessionActionManager.SetActionHandler("shopping_ui", session, new List<SBGUIScreen>
			{
				session.properties.shoppingHud,
				item
			}, SessionActionUiHelper.HandleCommonSessionActions);
			session.PlayHavenController.RequestContent("shop_open");
			session.properties.m_sLeaveType = null;
			SoaringDictionary soaringDictionary = new SoaringDictionary();
			soaringDictionary.addValue(session.TheGame.resourceManager.PlayerLevelAmount, "level");
			soaringDictionary.addValue(SBSettings.BundleVersion, "client_version");
			Soaring.FireEvent("OpenStore", soaringDictionary);
		}

		public void OnLeave(Session session)
		{
			SBGUIPulseButton sBGUIPulseButton = (SBGUIPulseButton)session.properties.shoppingHud.FindChild("marketplace");
			sBGUIPulseButton.SessionActionId = session.properties.marketplaceSessionActionID;
			object obj = session.CheckAsyncRequest("store_open_type");
			string sOpenType = null;
			if (obj != null)
			{
				sOpenType = (string)obj;
			}
			AnalyticsWrapper.LogMarketplaceUI(session.TheGame, "open", sOpenType, null);
			AnalyticsWrapper.LogMarketplaceUI(session.TheGame, "close", null, session.properties.m_sLeaveType);
			Debug.LogError(session.properties.m_sLeaveType);
			if (session.properties.m_sLeaveType == "store_open_too_poor_return" || session.properties.m_sLeaveType == "store_close_back_button")
			{
				FireFinishShoppingEvent(session);
				session.properties.storeVisitSinceLastPurchase++;
			}
			session.TheSoundEffectManager.PlaySound("CloseMenu");
			session.game.sessionActionManager.ClearActionHandler("shopping_ui", session);
			session.properties.shoppingHud.SetVisibleNonEssentialElements(true);
			session.properties.shoppingHud = null;
			session.TheCamera.TurnOffScreenBuffer();
			session.marketpalceActive = false;
		}

		public static void FireFinishShoppingEvent(Session session)
		{
			SoaringDictionary soaringDictionary = new SoaringDictionary();
			soaringDictionary.addValue(session.TheGame.resourceManager.PlayerLevelAmount, "level");
			soaringDictionary.addValue(SBSettings.BundleVersion, "client_version");
			Soaring.FireEvent("LeaveStore", soaringDictionary);
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}
	}

	public class ShowingDialog : State
	{
		private void AdvanceToNextDialog(SBGUIScreen screen, Session session, SBGUIScreen dialog)
		{
			session.game.dialogPackageManager.RemoveCurrentDialogInput(session.game);
			if (dialog != null)
			{
				dialog.Close();
			}
			int numQueuedDialogInputs = session.game.dialogPackageManager.GetNumQueuedDialogInputs();
			if (numQueuedDialogInputs == 0)
			{
				TFUtils.DebugLog("SessionShowingDialog.AdvanceToNextDialog - No more dialogs remaining to show, going to go back to Session Playing");
				session.AddAsyncResponse("dialogs_to_show", false);
				session.ChangeState("Playing");
			}
			else
			{
				DialogInputData dialogInputData = session.game.dialogPackageManager.PeekCurrentDialogInput();
				TFUtils.DebugLog(string.Format("SessionShowingDialog.AdvanceToNextDialog - There are {0} dialogs remaining to show, going to create the next dialog with inputdata of type {1}", numQueuedDialogInputs, dialogInputData.GetType()));
				CreateOrAdvanceDialog(dialogInputData, screen, session);
			}
		}

		private SBGUIScreen CreateCharacterDialog(CharacterDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateCharacterDialog - passing inputData to SBUIBuilder to create screen.");
			SBGUICharacterDialog dialog = SBUIBuilder.MakeAndAddDialogSequence(screen, session, inputData.PromptsData, null);
			TFUtils.DebugLog("SessionShowingDialog.CreateCharacterDialog - dialog created.");
			dialog.DialogChange.AddListener(delegate(int page)
			{
				if (page < 0)
				{
					AdvanceToNextDialog(screen, session, dialog);
				}
				else
				{
					session.TheSoundEffectManager.PlaySound("OpenMenu");
				}
			});
			return dialog;
		}

		private SBGUIScreen CreateQuestDialog(QuestDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			if (inputData.PromptData == null)
			{
				if (inputData.SequenceId == 10000)
				{
					inputData = QuestDefinition.RecreateRandomQuestStartInputData(session.TheGame, QuestDefinition.LastRandomQuestId);
				}
				else if (inputData.SequenceId == 10001)
				{
					inputData = QuestDefinition.RecreateRandomQuestCompleteInputData(session.TheGame, QuestDefinition.LastRandomQuestId);
				}
			}
			QuestDefinition questDefinition = null;
			List<ConditionDescription> list = new List<ConditionDescription>();
			Dictionary<string, object> promptData = inputData.PromptData;
			string key = string.Empty;
			string text = string.Empty;
			string text2 = string.Empty;
			if (promptData.ContainsKey("title"))
			{
				key = TFUtils.LoadString(promptData, "title");
			}
			if (promptData.ContainsKey("icon"))
			{
				text = TFUtils.LoadString(promptData, "icon");
			}
			if (promptData.ContainsKey("type"))
			{
				text2 = TFUtils.LoadString(promptData, "type");
			}
			TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - deciding which quest dialog to create. Type is: " + text2);
			List<Reward> list2 = new List<Reward>();
			if (inputData.ContextData != null)
			{
				List<object> list3 = TFUtils.LoadList<object>(inputData.ContextData, "rewards");
				foreach (object item in list3)
				{
					list2.Add(Reward.FromObject(item));
				}
			}
			TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - generated rewards list");
			string title = Language.Get(key);
			SBGUIScreen dialog = null;
			if (text2.Equals("quest_start"))
			{
				uint questDid = session.TheGame.questManager.ActiveQuestDids.Last();
				if (inputData.QuestId.HasValue)
				{
					questDid = inputData.QuestId.Value;
				}
				questDefinition = RetrieveQuestDefinition(session, questDid);
				if (questDefinition == null)
				{
					AdvanceToNextDialog(screen, session, dialog);
					return dialog;
				}
				list = RetrieveQuestConditionDescriptions(session, questDid);
				if (questDefinition.Chunk)
				{
					if (questDefinition.Did == QuestDefinition.LastAutoQuestId)
					{
						Action allDoneButton = delegate
						{
							session.TheSoundEffectManager.PlaySound("CloseQuestDialog");
							AdvanceToNextDialog(screen, session, dialog);
							session.TheGame.simulation.ModifyGameState(new AutoQuestAllDoneAction(QuestDefinition.LastAutoQuestId));
						};
						Action makeButton = delegate
						{
							session.TheSoundEffectManager.PlaySound("CloseQuestDialog");
							AdvanceToNextDialog(screen, session, dialog);
						};
						dialog = SBUIBuilder.MakeAndAddAutoQuestStartDialog(screen, session.properties.dialogHud, session, list2, questDefinition, list, allDoneButton, makeButton);
					}
					else
					{
						Action findButton = delegate
						{
							session.TheSoundEffectManager.PlaySound("CloseQuestDialog");
							AdvanceToNextDialog(screen, session, dialog);
						};
						TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - going to make Quest CHUNK Start Dialog, sending it through to SBUIBuilder");
						dialog = SBUIBuilder.MakeAndAddQuestChunkStartDialog(screen, session.properties.dialogHud, session, list2, questDefinition, list, findButton);
					}
				}
				else
				{
					TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - going to make Quest NORMAL Start Dialog, sending it through to SBUIBuilder");
					dialog = SBUIBuilder.MakeAndAddQuestStartDialog(screen, session, list2, title, text);
				}
			}
			else if (text2.Equals("quest_complete"))
			{
				uint num = session.TheGame.questManager.CompletedQuestDids.Last();
				if (inputData.QuestId.HasValue)
				{
					num = inputData.QuestId.Value;
				}
				questDefinition = RetrieveQuestDefinition(session, num);
				if (questDefinition == null)
				{
					TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - Is QuestCompleteDialogInputData, tried gettings questDefinition but null was returned when questDid: " + num + " was passed to RetriveQuestDefinition.");
					AdvanceToNextDialog(screen, session, dialog);
					return dialog;
				}
				list = RetrieveQuestConditionDescriptions(session, num);
				TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - Is QuestCompleteDialogInputData, conditionDescriptions retrieved.");
				if (questDefinition.Chunk)
				{
					TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - going to make Quest CHUNK Complete Dialog, sending it through to SBUIBuilder");
					if (questDefinition.Did == QuestDefinition.LastAutoQuestId)
					{
						dialog = SBUIBuilder.MakeAndAddAutoQuestCompleteDialog(screen, null, session, list2, questDefinition);
					}
					else
					{
						dialog = SBUIBuilder.MakeAndAddQuestChunkCompleteDialog(screen, null, session, list2, questDefinition, list);
					}
				}
				else
				{
					TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - going to make Quest NORMAL Complete Dialog, sending it through to SBUIBuilder");
					dialog = SBUIBuilder.MakeAndAddQuestCompleteDialog(screen, session, list2, title, text);
				}
			}
			else if (text2.Equals("booty_quest_complete"))
			{
				dialog = SBUIBuilder.MakeAndAddBootyQuestCompleteDialog(screen, session, list2, title, text);
			}
			else if (text2.Equals("quest_line_start") || text2.Equals("quest_line_complete"))
			{
				string text3 = string.Empty;
				string text4 = string.Empty;
				string text5 = string.Empty;
				string text6 = string.Empty;
				string text7 = string.Empty;
				List<Reward> list4 = new List<Reward>();
				TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, going to get heading, body, portrait, and reward", text2));
				if (promptData.ContainsKey("heading") && promptData.ContainsKey("body") && promptData.ContainsKey("portrait") && promptData.ContainsKey("reward"))
				{
					text3 = TFUtils.LoadString(promptData, "heading");
					TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, got heading: {1}", text2, text3));
					text4 = TFUtils.LoadString(promptData, "body");
					TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, got body: {1}", text2, text4));
					text5 = TFUtils.LoadString(promptData, "portrait");
					TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, got portrait: {1}", text2, text5));
					Dictionary<string, object> dictionary = TFUtils.LoadDict(promptData, "reward");
					TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, got reward.", text2));
					if (dictionary.ContainsKey("buildings"))
					{
						TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, reward has 'buildings' key, going to get texture and name.", text2));
						Dictionary<string, object> dictionary2 = TFUtils.LoadDict(dictionary, "buildings");
						int num2 = int.Parse(dictionary2.Keys.First());
						TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, reward has 'buildings' key, did is {1}", text2, num2));
						Blueprint blueprint = EntityManager.GetBlueprint("building", num2);
						if (blueprint == null)
						{
							TFUtils.ErrorLog("SessionShowingDialog.CreateQuestDialog - blueprint is null");
						}
						text7 = (string)blueprint.Invariable["portrait"];
						text6 = Language.Get((string)blueprint.Invariable["name"]);
						TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, reward has 'buildings' key, name is {1}, textures is {2}", text2, text6, text7));
					}
					else if (dictionary.ContainsKey("recipes"))
					{
						Dictionary<string, object> dictionary3 = TFUtils.LoadDict(dictionary, "buildings");
						int id = int.Parse(dictionary3.Keys.First());
						int productId = session.TheGame.craftManager.GetRecipeById(id).productId;
						Resource resource = session.TheGame.resourceManager.Resources[productId];
						text7 = resource.GetResourceTexture();
						text6 = Language.Get(resource.Name);
					}
					if (dictionary.ContainsKey("resources"))
					{
						if (dictionary.ContainsKey("buildings"))
						{
							dictionary.Remove("buildings");
						}
						if (dictionary.ContainsKey("recipes"))
						{
							dictionary.Remove("recipes");
						}
						TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, reward has 'resources' key, getting promptReward from reward", text2));
						Reward reward = Reward.FromDict(dictionary);
						TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, reward has 'resources' key, loaded promptReward", text2));
						if (reward == null)
						{
							TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - promptReward is null");
						}
						list4.Add(reward);
					}
				}
				else
				{
					TFUtils.ErrorLog(string.Format("Dialog sequenceId {0} prompt type '{1}' needs to contain all of the following fields: 'heading', 'body', 'portrait', 'reward'", inputData.SequenceId, text2));
				}
				if (!string.IsNullOrEmpty(text))
				{
					text7 = text;
				}
				if (text2.Equals("quest_line_start"))
				{
					dialog = SBUIBuilder.MakeAndAddQuestLineStartDialog(screen, session, list4, text3, text4, text5, text7, string.Empty);
				}
				else
				{
					dialog = SBUIBuilder.MakeAndAddQuestLineCompleteDialog(screen, session, list4, text3, text4, text5, text7, text6);
				}
			}
			Action okHandler = delegate
			{
				session.TheSoundEffectManager.PlaySound("CloseQuestDialog");
				AdvanceToNextDialog(screen, session, dialog);
				AndroidBack.getInstance().pop();
			};
			Action action = delegate
			{
				okHandler();
			};
			AndroidBack.getInstance().push(action, dialog);
			dialog.AttachActionToButton("okay", okHandler);
			if (text2.Equals("quest_line_start") || text2.Equals("quest_line_complete"))
			{
				dialog.AttachActionToButton("TouchableBackground", delegate
				{
				});
			}
			else
			{
				dialog.AttachActionToButton("TouchableBackground", okHandler);
			}
			return dialog;
		}

		private SBGUIScreen CreateLevelUpDialog(LevelUpDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateLevelUpDialog - passing inputData to SBUIBuilder to create screen.");
			SBGUILevelUpScreen dialog = SBUIBuilder.MakeAndAddLevelUpDialog(screen, session, inputData);
			TFUtils.DebugLog("SessionShowingDialog.CreateLevelUpDialog - dialog created.");
			session.TheCamera.TurnOnScreenBuffer();
			Action action = delegate
			{
				session.PlayHavenController.RequestContent("level_" + inputData.NewLevel);
				SoaringDictionary soaringDictionary = new SoaringDictionary();
				soaringDictionary.addValue(inputData.NewLevel, "level");
				soaringDictionary.addValue(inputData.NewLevel - 1, "old_level");
				soaringDictionary.addValue(SBSettings.BundleVersion, "client_version");
				Soaring.FireEvent("LevelUp", soaringDictionary);
				session.TheSoundEffectManager.PlaySound("CloseLevelUpDialog");
				AdvanceToNextDialog(screen, session, dialog);
				session.TheCamera.TurnOffScreenBuffer();
				Renderer component = dialog.FindChild("windows").GetComponent<Renderer>();
				Renderer component2 = dialog.FindChild("spinning_paper").GetComponent<Renderer>();
				Renderer component3 = dialog.FindChild("headline_image").GetComponent<Renderer>();
				Resources.UnloadAsset(component.material.mainTexture);
				Resources.UnloadAsset(component2.material.mainTexture);
				Resources.UnloadAsset(component3.material.mainTexture);
			};
			dialog.AttachActionToButton("okay", action);
			dialog.AttachActionToButton("TouchableBackground", delegate
			{
			});
			return dialog;
		}

		private SBGUIScreen CreateFoundItemDialog(FoundItemDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateFoundItemDialog - passing inputData to SBUIBuilder to create screen.");
			SBGUIFoundItemScreen dialog = SBUIBuilder.MakeAndAddFoundItemScreen(session, screen);
			TFUtils.DebugLog("SessionShowingDialog.CreateFoundItemDialog - dialog created.");
			Action action = delegate
			{
				session.TheSoundEffectManager.PlaySound("CloseFoundItemDialog");
				AdvanceToNextDialog(screen, session, dialog);
			};
			dialog.AttachActionToButton("okay", action);
			dialog.AttachActionToButton("TouchableBackground", delegate
			{
			});
			dialog.Setup(inputData.Title, inputData.Message, inputData.Icon, false, string.Empty);
			session.TheSoundEffectManager.PlaySound("Error");
			return dialog;
		}

		private SBGUIScreen CreateFoundMovieDialog(FoundMovieDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateFoundMovieDialog - passing inputData to SBUIBuilder to create screen.");
			SBGUIFoundItemScreen dialog = SBUIBuilder.MakeAndAddFoundItemScreen(session, screen);
			TFUtils.DebugLog("SessionShowingDialog.CreateFoundMovieDialog - dialog created.");
			Action action = delegate
			{
				session.TheSoundEffectManager.PlaySound("CloseFoundItemDialog");
				AdvanceToNextDialog(screen, session, dialog);
			};
			Action action2 = delegate
			{
				session.PlayMovieFromShowingDialog(inputData.Movie);
			};
			dialog.AttachActionToButton("okay", action);
			dialog.AttachActionToButton("extra_button", action2);
			dialog.AttachActionToButton("TouchableBackground", delegate
			{
			});
			dialog.Setup(inputData.Title, inputData.Message, inputData.Icon, true, Language.Get("!!MOVIE_WATCH_NOW"));
			session.TheSoundEffectManager.PlaySound("Error");
			return dialog;
		}

		private SBGUIScreen CreateExplanationDialog(ExplanationDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateExplanationDialog - passing inputData to SBUIBuilder to create screen.");
			SBGUIExplanationDialog dialog = SBUIBuilder.MakeAndAddExplanationDialog(screen);
			TFUtils.DebugLog("SessionShowingDialog.CreateExplanationDialog - dialog created.");
			Action action = delegate
			{
				AdvanceToNextDialog(screen, session, dialog);
			};
			dialog.AttachActionToButton("skip_button", action);
			dialog.Setup(inputData.Message);
			session.TheSoundEffectManager.PlaySound("Error");
			return dialog;
		}

		private SBGUIScreen CreateMoveInDialog(MoveInDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateMoveInDialog - passing inputData to SBUIBuilder to create screen.");
			SBGUIMoveInDialog dialog = SBUIBuilder.MakeAndAddMoveInDialog(screen);
			TFUtils.DebugLog("SessionShowingDialog.CreateMoveInDialog - dialog created.");
			Action action = delegate
			{
				AdvanceToNextDialog(screen, session, dialog);
			};
			dialog.AttachActionToButton("okay", action);
			dialog.AttachActionToButton("TouchableBackground", delegate
			{
			});
			dialog.Setup(inputData.CharacterName, inputData.BuildingName, inputData.PortraitTexture);
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			return dialog;
		}

		private SBGUIScreen CreateTreasureDialog(TreasureDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateTreasureDialog - passing inputData to SBUIBuilder to create screen.");
			SBGUIFoundItemScreen dialog = SBUIBuilder.MakeAndAddFoundItemScreen(session, screen);
			TFUtils.DebugLog("SessionShowingDialog.CreateTreasureDialog - dialog created.");
			Action action = delegate
			{
				session.TheSoundEffectManager.PlaySound("CloseFoundItemDialog");
				AdvanceToNextDialog(screen, session, dialog);
			};
			dialog.AttachActionToButton("okay", action);
			dialog.AttachActionToButton("TouchableBackground", delegate
			{
			});
			dialog.Setup(inputData.Title, inputData.Message, "TreasureChest_Closed.png", false, string.Empty);
			session.TheSoundEffectManager.PlaySound("Error");
			return dialog;
		}

		private SBGUIScreen CreateSpongyGamesDialog(SpongyGamesDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateSpongyGamesDialog - passing inputData to SBUIBuilder to create screen.");
			SBGUISpongyGamesDialog dialog = SBUIBuilder.MakeAndAddSpongyGamesDialog(screen);
			TFUtils.DebugLog("SessionShowingDialog.CreateSpongyGamesDialog - dialog created.");
			Action action = delegate
			{
				session.TheSoundEffectManager.PlaySound("CloseFoundItemDialog");
				AdvanceToNextDialog(screen, session, dialog);
			};
			dialog.AttachActionToButton("okay", action);
			dialog.AttachActionToButton("TouchableBackground", delegate
			{
			});
			dialog.Setup(inputData);
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			return dialog;
		}

		private SBGUIScreen CreateDailyBonusDialog(DailyBonusDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateDailyBonusDialog - passing inputData to SBUIBuilder to create screen.");
			if (inputData.DailyBonusData != null)
			{
				SBGUIDailyBonusDialog dialog = SBUIBuilder.MakeAndAddDailyBonusDialog(screen);
				TFUtils.DebugLog("SessionShowingDialog.CreateDailyBonusDialog - dialog created.");
				Action action = delegate
				{
					session.TheSoundEffectManager.PlaySound("CloseFoundItemDialog");
					AdvanceToNextDialog(screen, session, dialog);
					dialog.applyReward(session);
				};
				dialog.AttachActionToButton("okay", action);
				dialog.AttachActionToButton("TouchableBackground", delegate
				{
				});
				dialog.Setup(inputData, session);
				session.TheSoundEffectManager.PlaySound("OpenMenu");
				return dialog;
			}
			Debug.LogError("Viet's Debug - Daily Bonus Disabled");
			AdvanceToNextDialog(screen, session, null);
			return null;
		}

		private void CreateOrAdvanceDialog(DialogInputData inputData, SBGUIScreen screen, Session session)
		{
			SBGUIScreen sBGUIScreen = null;
			try
			{
				sBGUIScreen = CreateDialog(inputData, screen, session);
			}
			catch (Exception ex)
			{
				TFUtils.LogDump(session, "failed_dialog", ex);
				SBUIBuilder.ReleaseTopScreen();
				screen = SBUIBuilder.MakeAndPushScratchLayer(session);
			}
			if (sBGUIScreen == null)
			{
				AdvanceToNextDialog(screen, session, sBGUIScreen);
			}
		}

		private SBGUIScreen CreateDialog(DialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.Assert(inputData != null, "Don't call CreateDialog with null inputData.");
			SBGUIScreen result = null;
			if (inputData is CharacterDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is CharacterDialogInputData, going to create dialog.");
				result = CreateCharacterDialog((CharacterDialogInputData)inputData, screen, session);
			}
			else if (inputData is QuestDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is QuestDialogInputData, going to create dialog.");
				result = CreateQuestDialog((QuestDialogInputData)inputData, screen, session);
			}
			else if (inputData is LevelUpDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is LevelUpDialogInputData, going to create dialog.");
				result = CreateLevelUpDialog((LevelUpDialogInputData)inputData, screen, session);
			}
			else if (inputData is FoundMovieDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is FoundMovieDialogInputData, going to create dialog.");
				result = CreateFoundMovieDialog((FoundMovieDialogInputData)inputData, screen, session);
			}
			else if (inputData is FoundItemDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is FoundItemDialogInputData, going to create dialog.");
				result = CreateFoundItemDialog((FoundItemDialogInputData)inputData, screen, session);
			}
			else if (inputData is ExplanationDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is ExplanationDialogInputData, going to create dialog.");
				result = CreateExplanationDialog((ExplanationDialogInputData)inputData, screen, session);
			}
			else if (inputData is MoveInDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is MoveInDialogInputData, ggoing to create dialog.");
				result = CreateMoveInDialog((MoveInDialogInputData)inputData, screen, session);
			}
			else if (inputData is TreasureDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is TreasureDialogInputData, going to create dialog.");
				result = CreateTreasureDialog((TreasureDialogInputData)inputData, screen, session);
			}
			else if (inputData is SpongyGamesDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is SpongyGamesDialogInputData, going to create dialog.");
				result = CreateSpongyGamesDialog((SpongyGamesDialogInputData)inputData, screen, session);
			}
			else if (inputData is DailyBonusDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is DailyBonusDialogInputData, going to create dialog.");
				result = CreateDailyBonusDialog((DailyBonusDialogInputData)inputData, screen, session);
			}
			session.soundEffectManager.PlaySound(inputData.SoundImmediate);
			session.soundEffectManager.PlaySound(inputData.SoundBeat, 1f);
			return result;
		}

		public void OnEnter(Session session)
		{
			GUIMainView.GetInstance().Library.bShowingDialog = true;
			session.game.dropManager.MarkForClearCurrentDrops();
			Action shopClickHandler = delegate
			{
				session.AddAsyncResponse("store_open_type", "store_open_button");
				session.ChangeState("Shopping");
			};
			SBGUIStandardScreen dialogHud = SBUIBuilder.MakeAndPushStandardUI(session, true, null, shopClickHandler, null, null, null, null, null, null, null, null, null, null);
			TFUtils.DebugLog("SessionShowingDialog.OnEnter - created dialogHud");
			session.properties.dialogHud = dialogHud;
			session.properties.dialogHud.EnableUI(false);
			SBGUIScreen screen = SBUIBuilder.MakeAndPushScratchLayer(session);
			int numQueuedDialogInputs = session.game.dialogPackageManager.GetNumQueuedDialogInputs();
			TFUtils.Assert(numQueuedDialogInputs > 0, "Should not get into the ShowingDialog Session state unless there are some dialogs!");
			DialogInputData inputData = session.game.dialogPackageManager.PeekCurrentDialogInput();
			CreateOrAdvanceDialog(inputData, screen, session);
			session.camera.SetEnableUserInput(false);
			SessionActionSimulationHelper.EnableHandler(session, false);
			RestrictInteraction.AddWhitelistSimulated(session.game.simulation, int.MinValue);
		}

		public void OnLeave(Session session)
		{
			session.properties.dialogHud.EnableUI(true);
			session.properties.dialogHud = null;
			SessionActionSimulationHelper.EnableHandler(session, true);
			RestrictInteraction.RemoveWhitelistSimulated(session.game.simulation, int.MinValue);
			GUIMainView.GetInstance().Library.bShowingDialog = false;
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		private QuestDefinition RetrieveQuestDefinition(Session session, uint questDid)
		{
			Quest quest = session.TheGame.questManager.GetQuest(questDid);
			if (quest == null)
			{
				return null;
			}
			return session.TheGame.questManager.GetQuestDefinition(quest.Did);
		}

		private List<ConditionDescription> RetrieveQuestConditionDescriptions(Session session, uint questDid)
		{
			Quest quest = session.TheGame.questManager.GetQuest(questDid);
			if (quest == null)
			{
				return null;
			}
			List<ConditionDescription> list = new List<ConditionDescription>();
			foreach (ConditionState endCondition in quest.EndConditions)
			{
				list.AddRange(endCondition.Describe(session.TheGame));
			}
			return list;
		}
	}

	public class StashBuildingConfirmation : State
	{
		public void OnEnter(Session session)
		{
			Setup(session);
			session.camera.SetEnableUserInput(false);
		}

		public void Setup(Session session)
		{
			string text = null;
			object obj = session.CheckAsyncRequest("stash_error");
			if (obj != null)
			{
				text = (string)obj;
			}
			Simulated simulated = (Simulated)session.CheckAsyncRequest("to_stash");
			object obj2 = session.CheckAsyncRequest("in_state_move_in_edit");
			bool bMoveInEdit = false;
			if (obj2 != null)
			{
				bMoveInEdit = (bool)obj2;
			}
			if (session.TheState.GetType().Equals(typeof(MoveBuildingInEdit)))
			{
				MoveBuildingInEdit moveBuildingInEdit = (MoveBuildingInEdit)session.TheState;
				moveBuildingInEdit.DeactivateInteractionStrip(session);
			}
			BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
			if (!string.IsNullOrEmpty(text))
			{
				Action okButtonHandler = delegate
				{
					session.TheSoundEffectManager.PlaySound("CloseMenu");
					session.ChangeState((!bMoveInEdit) ? "MoveBuildingInPlacement" : "MoveBuildingInEdit");
				};
				session.TheSoundEffectManager.PlaySound("OpenMenu");
				SBUIBuilder.MakeAndPushAcknowledgeDialog(session, HandleSBGUIEvent, Language.Get("!!CANNOT_STOW_DIALOG_TITLE"), Language.Get(text), (string)entity.Invariable["portrait"], Language.Get("!!PREFAB_OK"), okButtonHandler);
			}
			else
			{
				session.ChangeState((!bMoveInEdit) ? "MoveBuildingInPlacement" : "MoveBuildingInEdit");
			}
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, false);
		}
	}

	public interface State
	{
		void OnEnter(Session session);

		void OnLeave(Session session);

		void OnUpdate(Session session);
	}

	public class Stopping : State
	{
		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}
	}

	public class Sync : State
	{
		private float mResyncStartTime;

		private bool mWasResync;

		public void OnEnter(Session session)
		{
			session.camera.StopCamera();
			session.TheSoundEffectManager.Enabled = false;
			if (session.reinitializeSession)
			{
				TFUtils.DebugLog("session.reinitializeSession");
				Reauthenticate(session);
			}
			else if (session.TheGame.ReloadToFriendPark())
			{
				TFUtils.DebugLog("session.TheGame.ReloadToFriendPark()");
				session.TheGame.ClearLoadFriendPark();
				ReloadToFriendsSession(session);
			}
			else if (session.TheGame.RequiresReload())
			{
				TFUtils.DebugLog("session.TheGame.RequiresReinitialize()");
				session.TheGame.ClearReloadRequest();
				ReloadFromDisk(session);
			}
			else if (session.resyncConnection)
			{
				GameStarting.CreateLoadingScreen(session, false, "starting_progress", false);
				mWasResync = true;
			}
			else
			{
				TFUtils.DebugLog("ReloadFromNetwork");
				ReloadFromNetwork(session);
			}
			mResyncStartTime = Time.realtimeSinceStartup;
		}

		public void OnLeave(Session session)
		{
			mWasResync = false;
			session.resyncConnection = false;
			session.camera.StartCamera();
			session.TheSoundEffectManager.StartSoundEffectsManager();
		}

		public void OnUpdate(Session session)
		{
			if (!session.resyncConnection)
			{
				return;
			}
			if (Time.realtimeSinceStartup - mResyncStartTime > 2f)
			{
				session.ChangeState("Playing");
				session.resyncConnection = false;
				return;
			}
			if (!Soaring.IsOnline)
			{
				SoaringInternal.instance.ClearOfflineMode();
			}
			if (Soaring.IsOnline)
			{
				session.resyncConnection = false;
				session.reinitializeSession = true;
				Reauthenticate(session);
			}
		}

		private void Reauthenticate(Session session)
		{
			TFUtils.DebugLog("Logging in with possibly different credentials.");
			session.gameIsReloading = true;
			session.reinitializeSession = false;
			session.ClearUserState();
			session.InFriendsGame = false;
			session.ChangeState("Authorizing");
		}

		private void ReloadFromDisk(Session session)
		{
			CleanUp(session);
			session.InFriendsGame = false;
			session.ChangeState("GameStarting");
			GameStarting.CreateLoadingScreen(session, false, "visit_starting_progress");
		}

		private void ReloadToFriendsSession(Session session)
		{
			CleanUp(session);
			session.ChangeState("visit_friend");
			GameStarting.CreateLoadingScreen(session, false, "visit_starting_progress");
		}

		private void ReloadFromNetwork(Session session)
		{
			CleanUp(session);
			session.InFriendsGame = false;
			session.TheGame.DestroyCache();
			session.WebFileServer.DeleteETagFile();
			session.ChangeState("GameStarting");
		}

		private void CleanUp(Session session)
		{
			SBUIBuilder.UpdateGuiEventHandler(session, delegate
			{
			});
			session.ClearAsyncRequests();
			SoaringInternal.instance.ClearSoaringWebQueue();
			session.game.Clear();
		}
	}

	public class UnitBusy : State
	{
		private const string UNIT_BUSY_UI_HANDLER = "unit_busy_ui";

		public void OnEnter(Session session)
		{
			session.game.dropManager.MarkForClearCurrentDrops();
			Simulated sim = session.game.selected;
			if (sim == null)
			{
				TFUtils.DebugLog("attempted to transition to unit busy without a selected entity");
				session.ChangeState("Playing");
				return;
			}
			Action closeButton = delegate
			{
				session.ChangeState("Playing");
				AndroidBack.getInstance().pop();
			};
			session.properties.unitBusyHud = SBUIBuilder.MakeAndPushStandardUI(session, false, null, delegate
			{
				session.AddAsyncResponse("store_open_type", "store_open_button");
				session.ChangeState("Shopping");
			}, delegate
			{
				session.CheckInventorySoftLock();
				session.ChangeState("Inventory");
			}, delegate
			{
				session.ChangeState("Options");
			}, delegate
			{
				session.ChangeState("Editing");
			}, null, null, null, delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping");
			}, delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping");
			}, delegate
			{
				session.ChangeState("CommunityEvent");
			}, null);
			session.properties.unitBusyHud.HideAllElements();
			Action pFeedWishAction = delegate
			{
				ResidentEntity entity = sim.GetEntity<ResidentEntity>();
				if (entity != null && entity.HungerResourceId.HasValue)
				{
					session.TheGame.simulation.Router.Send(OfferFoodCommand.Create(sim.Id, sim.Id, entity.HungerResourceId.Value));
					session.ChangeState("Playing");
				}
			};
			Action pRushWishAction = delegate
			{
				ResidentEntity pEntity = sim.GetEntity<ResidentEntity>();
				if (pEntity != null && !pEntity.HungerResourceId.HasValue)
				{
					Action<bool, Cost> logSpend = delegate(bool canAfford, Cost cost)
					{
						session.analytics.LogRushFullness(sim.entity.BlueprintName, cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], canAfford);
					};
					Action execute = delegate
					{
						session.TheGame.simulation.Router.Send(RushCommand.Create(sim.Id));
						session.ChangeState("UnitBusy");
						session.properties.transitionSilently = false;
						session.ChangeState("Playing");
					};
					Action complete = delegate
					{
						int value = 0;
						pEntity.FullnessRushCostNow().ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out value);
						AnalyticsWrapper.LogJellyConfirmation(session.TheGame, pEntity.DefinitionId, value, pEntity.Name, "characters", "speedup", "fullness", "confirm");
					};
					Action cancel = delegate
					{
						session.TheGame.simulation.Router.Send(AbortCommand.Create(sim.Id, sim.Id));
						session.ChangeState("UnitBusy");
						int value = 0;
						pEntity.FullnessRushCostNow().ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out value);
					};
					session.properties.transitionSilently = true;
					session.properties.hardSpendActions = new HardSpendActions(execute, (ulong ts) => pEntity.FullnessRushCostNow(), pEntity.BlueprintName, pEntity.DefinitionId, complete, cancel, logSpend, session.properties.unitBusyWindow.GetWishWidgetRushButtonPosition());
					session.ChangeState("HardSpendConfirm", false);
				}
			};
			Task pTask = null;
			List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(sim.entity.DefinitionId, sim.Id);
			if (activeTasksForSimulated.Count > 0)
			{
				pTask = activeTasksForSimulated[0];
			}
			if (pTask != null)
			{
				session.properties.unitBusyTask = pTask;
			}
			else
			{
				pTask = session.properties.unitBusyTask;
			}
			Action pRushTaskAction = delegate
			{
				if (pTask != null)
				{
					Action<bool, Cost> logSpend = delegate
					{
					};
					Action execute = delegate
					{
						session.ChangeState("UnitBusy");
						session.properties.transitionSilently = false;
						session.ChangeState("Playing");
						Cost cost = pTask.RushCostNow();
						session.TheGame.resourceManager.Apply(cost, session.TheGame);
						pTask.m_ulCompleteTime = TFUtils.EpochTime();
						session.TheGame.simulation.ModifyGameState(new TaskRushAction(pTask, cost));
						session.TheGame.simulation.Router.Send(RushTaskCommand.Create(sim.Id, sim.Id));
					};
					Action complete = delegate
					{
						Cost cost = pTask.RushCostNow();
						int num = 0;
						if (cost.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY))
						{
							num = cost.ResourceAmounts[ResourceManager.HARD_CURRENCY];
						}
						num = session.properties.unitBusyWindow.taskRushCost;
						AnalyticsWrapper.LogJellyConfirmation(session.TheGame, pTask.m_pTaskData.m_nDID, num, pTask.m_pTaskData.m_sName, "task", "speedup", "task", "confirm");
					};
					Action cancel = delegate
					{
						Cost cost = pTask.RushCostNow();
						int num = 0;
						if (cost.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY))
						{
							num = cost.ResourceAmounts[ResourceManager.HARD_CURRENCY];
						}
						session.ChangeState("UnitBusy");
					};
					session.properties.transitionSilently = true;
					session.properties.hardSpendActions = new HardSpendActions(execute, (ulong ts) => pTask.RushCostNow(), pTask.m_pTaskData.m_sName, pTask.m_pTaskData.m_nDID, complete, cancel, logSpend, session.properties.unitBusyWindow.GetTaskRushButtonPosition());
					session.ChangeState("HardSpendConfirm", false);
				}
			};
			session.properties.unitBusyWindow = SBUIBuilder.MakeAndPushUnitBusyUI(session.properties.unitBusyHud, session, sim, pTask, pFeedWishAction, pRushWishAction, pRushTaskAction, closeButton);
			session.properties.transitionSilently = false;
			Action action = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.game.sessionActionManager.SetActionHandler("unit_busy_ui", session, new List<SBGUIScreen>
			{
				session.properties.unitBusyHud,
				session.properties.unitBusyWindow
			}, SessionActionUiHelper.HandleCommonSessionActions);
			session.TheCamera.SetEnableUserInput(false);
			SessionActionSimulationHelper.EnableHandler(session, true);
		}

		public void OnLeave(Session session)
		{
			session.game.sessionActionManager.ClearActionHandler("unit_busy_ui", session);
			if (!session.properties.transitionSilently)
			{
				if (session.TheCamera.ScreenBufferOn)
				{
					session.TheCamera.TurnOffScreenBuffer();
				}
				session.properties.unitBusyHud = null;
				session.properties.unitBusyWindow = null;
			}
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
			Simulated selected = session.game.selected;
			Task task = null;
			List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(selected.entity.DefinitionId, selected.Id);
			if (activeTasksForSimulated.Count > 0)
			{
				task = activeTasksForSimulated[0];
			}
			if (task == null || task.GetTimeLeft() == 0)
			{
				session.ChangeState("Playing");
			}
		}
	}

	public class UnitIdle : State
	{
		private const string UNIT_IDLE_UI_HANDLER = "unit_idle_ui";

		public void OnEnter(Session session)
		{
			session.game.dropManager.MarkForClearCurrentDrops();
			Simulated sim = session.game.selected;
			if (sim == null)
			{
				TFUtils.DebugLog("attempted to transition to unit busy without a selected entity");
				session.ChangeState("Playing");
				return;
			}
			Action closeButton = delegate
			{
				if (session.properties.unitIdleWindow != null)
				{
					session.properties.unitIdleWindow.ClearList();
				}
				session.ChangeState("Playing");
				AndroidBack.getInstance().pop();
			};
			session.properties.unitIdleHud = SBUIBuilder.MakeAndPushStandardUI(session, false, null, delegate
			{
				session.AddAsyncResponse("store_open_type", "store_open_button");
				session.ChangeState("Shopping");
			}, delegate
			{
				session.CheckInventorySoftLock();
				session.ChangeState("Inventory");
			}, delegate
			{
				session.ChangeState("Options");
			}, delegate
			{
				session.ChangeState("Editing");
			}, null, null, null, delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping");
			}, delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping");
			}, delegate
			{
				session.ChangeState("CommunityEvent");
			}, null);
			session.properties.unitIdleHud.HideAllElements();
			Action pFeedWishAction = delegate
			{
				ResidentEntity entity = sim.GetEntity<ResidentEntity>();
				if (entity != null && entity.HungerResourceId.HasValue)
				{
					session.TheGame.simulation.Router.Send(OfferFoodCommand.Create(sim.Id, sim.Id, entity.HungerResourceId.Value));
					session.ChangeState("Playing");
				}
			};
			Action pRushWishAction = delegate
			{
				ResidentEntity pEntity = sim.GetEntity<ResidentEntity>();
				if (pEntity != null && !pEntity.HungerResourceId.HasValue)
				{
					Action<bool, Cost> logSpend = delegate(bool canAfford, Cost cost)
					{
						session.analytics.LogRushFullness(sim.entity.BlueprintName, cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], canAfford);
					};
					Action execute = delegate
					{
						session.TheGame.simulation.Router.Send(RushCommand.Create(sim.Id));
						session.ChangeState("UnitIdle");
						session.properties.transitionSilently = false;
						session.ChangeState("Playing");
					};
					Action complete = delegate
					{
						int value = 0;
						pEntity.FullnessRushCostNow().ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out value);
						AnalyticsWrapper.LogJellyConfirmation(session.TheGame, pEntity.DefinitionId, value, pEntity.Name, "characters", "speedup", "fullness", "confirm");
					};
					Action cancel = delegate
					{
						session.TheGame.simulation.Router.Send(AbortCommand.Create(sim.Id, sim.Id));
						session.ChangeState("UnitIdle");
						int value = 0;
						pEntity.FullnessRushCostNow().ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out value);
					};
					session.properties.transitionSilently = true;
					session.properties.hardSpendActions = new HardSpendActions(execute, (ulong ts) => pEntity.FullnessRushCostNow(), pEntity.BlueprintName, pEntity.DefinitionId, complete, cancel, logSpend, session.properties.unitIdleWindow.GetWishWidgetRushButtonPosition());
					session.ChangeState("HardSpendConfirm", false);
				}
			};
			Action<int> pDoTaskAction = delegate(int nTaskDID)
			{
				int? nCostumeDID = session.properties.unitIdleWindow.m_nCostumeDID;
				if (nCostumeDID.HasValue && session.TheGame.costumeManager.IsCostumeUnlocked(nCostumeDID.Value))
				{
					ResidentEntity entity = sim.GetEntity<ResidentEntity>();
					if (!entity.CostumeDID.HasValue || entity.CostumeDID.Value != nCostumeDID.Value)
					{
						CostumeManager.Costume costume = session.TheGame.costumeManager.GetCostume(nCostumeDID.Value);
						CostumeManager.Costume pOldCostume = null;
						if (entity.CostumeDID.HasValue || entity.DefaultCostumeDID.HasValue)
						{
							pOldCostume = session.TheGame.costumeManager.GetCostume((!entity.CostumeDID.HasValue) ? entity.DefaultCostumeDID.Value : entity.CostumeDID.Value);
						}
						if (costume != null)
						{
							session.game.analytics.LogCostumeChanged(costume.m_nDID);
							AnalyticsWrapper.LogCostumeChanged(session.TheGame, entity, pOldCostume, costume);
							session.TheGame.simulation.ModifyGameStateSimulated(sim, new ChangeCostumeAction(sim.Id, nCostumeDID.Value));
							entity.CostumeDID = nCostumeDID;
							sim.SetCostume(costume);
						}
					}
				}
				TaskManager taskManager = session.TheGame.taskManager;
				Task task = taskManager.CreateActiveTask(session.TheGame, nTaskDID);
				if (task != null)
				{
					session.TheGame.simulation.UpdateControls();
					session.TheGame.analytics.LogTaskStarted(task.m_pTaskData.m_nDID);
					AnalyticsWrapper.LogTaskStarted(session.TheGame, task);
					session.TheGame.simulation.ModifyGameState(new TaskStartAction(task));
					session.TheGame.simulation.soundEffectManager.PlaySound(task.m_pTaskData.m_sStartVO);
					session.TheGame.simulation.soundEffectManager.PlaySound(task.m_pTaskData.m_sStartSound);
					session.ChangeState("Playing");
				}
			};
			session.properties.unitIdleWindow = SBUIBuilder.MakeAndPushUnitIdleUI(session.properties.unitIdleHud, session, sim, session.TheGame.taskManager.GetTaskDatasForSource(sim.entity.DefinitionId), pFeedWishAction, pRushWishAction, pDoTaskAction, closeButton);
			session.properties.transitionSilently = false;
			Action action = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.game.sessionActionManager.SetActionHandler("unit_idle_ui", session, new List<SBGUIScreen>
			{
				session.properties.unitIdleHud,
				session.properties.unitIdleWindow
			}, SessionActionUiHelper.HandleCommonSessionActions);
			session.TheCamera.SetEnableUserInput(false);
			SessionActionSimulationHelper.EnableHandler(session, true);
		}

		public void OnLeave(Session session)
		{
			session.game.sessionActionManager.ClearActionHandler("unit_idle_ui", session);
			if (session.properties.unitIdleWindow == null)
			{
				TFUtils.DebugLog("unitIdleWindow is null");
				return;
			}
			int? nCostumeDID = session.properties.unitIdleWindow.m_nCostumeDID;
			if (nCostumeDID.HasValue && session.game.costumeManager.IsCostumeUnlocked(nCostumeDID.Value))
			{
				Simulated selected = session.game.selected;
				ResidentEntity entity = selected.GetEntity<ResidentEntity>();
				if (!entity.CostumeDID.HasValue || entity.CostumeDID.Value != nCostumeDID.Value)
				{
					CostumeManager.Costume pOldCostume = null;
					if (entity.CostumeDID.HasValue || entity.DefaultCostumeDID.HasValue)
					{
						pOldCostume = session.TheGame.costumeManager.GetCostume((!entity.CostumeDID.HasValue) ? entity.DefaultCostumeDID.Value : entity.CostumeDID.Value);
					}
					CostumeManager.Costume costume = session.TheGame.costumeManager.GetCostume(nCostumeDID.Value);
					if (costume != null)
					{
						session.game.analytics.LogCostumeChanged(costume.m_nDID);
						AnalyticsWrapper.LogCostumeChanged(session.TheGame, entity, pOldCostume, costume);
						session.TheGame.simulation.ModifyGameStateSimulated(selected, new ChangeCostumeAction(selected.Id, nCostumeDID.Value));
						entity.CostumeDID = nCostumeDID;
						selected.SetCostume(costume);
					}
				}
			}
			if (!session.properties.transitionSilently)
			{
				if (session.TheCamera.ScreenBufferOn)
				{
					session.TheCamera.TurnOffScreenBuffer();
				}
				session.properties.unitIdleHud = null;
				session.properties.unitIdleWindow = null;
			}
		}

		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}
	}

	public class Vending : State
	{
		private const string VENDING_UI_HANDLER = "vending_ui";

		public void OnEnter(Session session)
		{
			session.game.dropManager.MarkForClearCurrentDrops();
			Simulated selected = session.game.selected;
			if (selected == null)
			{
				TFUtils.DebugLog("attempted to transition to Vending without a selected entity");
				session.ChangeState("Playing");
				return;
			}
			VendingDecorator entity = selected.GetEntity<VendingDecorator>();
			TFUtils.Assert(entity != null, "Did not select a valid building for Vending!");
			TFUtils.Assert(session.game.vendingManager != null, "VendingManager is not setup for this game!");
			VendorDefinition vendorDefinition = session.game.vendingManager.GetVendorDefinition(entity.VendorId);
			if (vendorDefinition == null)
			{
				TFUtils.DebugLog("attempted to transition to Vending without a valid vendor target " + entity.VendorId);
				session.ChangeState("Playing");
				return;
			}
			Action action = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.TheSoundEffectManager.PlaySound(vendorDefinition.music);
			session.TheSoundEffectManager.PlaySound(vendorDefinition.openSound);
			Action backHandler = delegate
			{
				AndroidBack.getInstance().pop();
				session.ChangeState("Playing");
			};
			Dictionary<int, VendingInstance> vendingInstances = session.game.vendingManager.GetVendingInstances(entity.Id);
			VendingInstance specialInstance = session.game.vendingManager.GetSpecialInstance(entity.Id);
			if (vendingInstances == null || specialInstance == null)
			{
				Restock(session, selected, false);
				vendingInstances = session.game.vendingManager.GetVendingInstances(entity.Id);
				specialInstance = session.game.vendingManager.GetSpecialInstance(entity.Id);
			}
			Action<VendingInstance> vendorInstanceHandler = delegate(VendingInstance instance)
			{
				if (instance.remaining > 0)
				{
					CheckInstanceForJelly(session, instance);
					session.properties.vendorScreen.UpdateVendingInstanceSlots(session);
				}
				else
				{
					session.TheSoundEffectManager.PlaySound("Error");
				}
			};
			Action rushHandler = delegate
			{
				VendorRestockRush(session);
			};
			SBGUIStandardScreen sBGUIStandardScreen = SBUIBuilder.MakeAndPushStandardUI(session, false, null, delegate
			{
				session.AddAsyncResponse("store_open_type", "store_open_button");
				session.ChangeState("Shopping");
			}, delegate
			{
				session.CheckInventorySoftLock();
				session.ChangeState("Inventory");
			}, delegate
			{
				session.ChangeState("Options");
			}, delegate
			{
				session.ChangeState("Editing");
			}, null, null, null, delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping");
			}, delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping");
			}, delegate
			{
				session.ChangeState("CommunityEvent");
			}, null);
			sBGUIStandardScreen.SetVisibleNonEssentialElements(false);
			session.properties.m_pTaskSimulated = null;
			session.properties.m_bAutoPanToSimulatedOnLeave = false;
			Action<int> pTaskCharacterClicked = delegate(int nDID)
			{
				Simulated simulated = session.TheGame.simulation.FindSimulated(nDID);
				if (simulated != null)
				{
					session.properties.m_pTaskSimulated = simulated;
					TaskManager taskManager = session.TheGame.taskManager;
					List<Task> activeTasksForSimulated2 = session.TheGame.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id);
					if (activeTasksForSimulated2.Count > 0 && activeTasksForSimulated2[0].GetTimeLeft() != 0)
					{
						if (taskManager.GetTaskingStateForSimulated(session.TheGame.simulation, nDID, simulated.Id) == TaskManager._eBlueprintTaskingState.eNone)
						{
							session.ChangeState("UnitIdle");
						}
						else
						{
							session.ChangeState("UnitBusy");
						}
					}
					else
					{
						session.properties.m_bAutoPanToSimulatedOnLeave = true;
						session.ChangeState("Playing");
					}
				}
			};
			List<int> activeSourcesForTarget = session.TheGame.taskManager.GetActiveSourcesForTarget(selected.Id);
			int num = activeSourcesForTarget.Count;
			for (int num2 = 0; num2 < num; num2++)
			{
				List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(activeSourcesForTarget[num2], null);
				if (activeTasksForSimulated.Count > 0 && activeTasksForSimulated[0].GetTimeLeft() == 0)
				{
					activeSourcesForTarget.RemoveAt(num2);
					num2--;
					num--;
				}
			}
			session.properties.vendorScreen = SBUIBuilder.MakeAndPushVendorUI(session, null, backHandler, vendorInstanceHandler, rushHandler, vendorDefinition, vendingInstances, specialInstance, entity, activeSourcesForTarget, pTaskCharacterClicked);
			session.properties.transitionSilently = false;
			session.game.sessionActionManager.SetActionHandler("vending_ui", session, new List<SBGUIScreen>
			{
				sBGUIStandardScreen,
				session.properties.vendorScreen
			}, SessionActionUiHelper.HandleCommonSessionActions);
			session.TheCamera.SetEnableUserInput(false);
			session.TheCamera.TurnOnScreenBuffer();
			SessionActionSimulationHelper.EnableHandler(session, true);
		}

		public void OnLeave(Session session)
		{
			session.game.sessionActionManager.ClearActionHandler("vending_ui", session);
			if (!session.properties.transitionSilently)
			{
				if (session.game.selected != null)
				{
					Simulated selected = session.game.selected;
					VendingDecorator entity = selected.GetEntity<VendingDecorator>();
					if (entity != null)
					{
						session.TheSoundEffectManager.PlaySound(session.game.vendingManager.GetVendorDefinition(entity.VendorId).closeSound);
					}
					if (session.properties.reward != null)
					{
						ulong utcNow = TFUtils.EpochTime();
						RewardManager.GenerateRewardDrops(session.properties.reward, session.game.simulation, selected.DisplayController.Position + selected.ThoughtDisplayOffsetWorld, utcNow);
						session.game.simulation.analytics.LogCollectVendedReward(entity.DefinitionId, session.game.resourceManager.PlayerLevelAmount);
					}
					session.game.selected = null;
				}
				else if (session.properties.reward != null)
				{
					session.game.ApplyReward(session.properties.reward, TFUtils.EpochTime(), false);
				}
				if (session.TheCamera.ScreenBufferOn)
				{
					session.TheCamera.TurnOffScreenBuffer();
				}
				session.properties.reward = null;
			}
			if (session.properties.m_pTaskSimulated != null)
			{
				session.game.selected = session.properties.m_pTaskSimulated;
			}
			if (session.game.selected != null && session.properties.m_bAutoPanToSimulatedOnLeave)
			{
				session.TheCamera.AutoPanToPosition(session.game.selected.PositionCenter, 0.75f);
			}
		}

		public void OnUpdate(Session session)
		{
			TFUtils.Assert(session != null && session.game != null && session.game.selected != null, "Trying to find Selected failed");
			VendingDecorator entity = session.game.selected.GetEntity<VendingDecorator>();
			if (entity == null)
			{
				TFUtils.Assert(false, "we have switched an entity to a non-Vendor!");
				session.ChangeState("Playing");
			}
			Restock(session, session.game.selected, true);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}

		private void CheckInstanceForJelly(Session session, VendingInstance instance)
		{
			if (instance.Cost.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY) && session.game.resourceManager.CanPay(instance.Cost))
			{
				Action execute = delegate
				{
					Purchase(session, instance);
				};
				VendorStock stock = session.game.vendingManager.GetStock(instance.StockId);
				Action<bool, Cost> logSpend = delegate(bool canAfford, Cost cost)
				{
					session.analytics.LogPremiumVending(stock.Name, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount, cost, canAfford);
				};
				int jellyCost = 0;
				instance.Cost.ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out jellyCost);
				Action complete = delegate
				{
					session.ChangeState("vending");
					AnalyticsWrapper.LogJellyConfirmation(session.TheGame, stock.Did, jellyCost, stock.Name, "craft", "instant_purchase", string.Empty, "confirm");
				};
				Action cancel = delegate
				{
					session.ChangeState("vending");
				};
				session.properties.transitionSilently = true;
				session.properties.hardSpendActions = new HardSpendActions(execute, (ulong time) => new Cost(new Dictionary<int, int> { 
				{
					ResourceManager.HARD_CURRENCY,
					jellyCost
				} }), string.Empty, stock.Did, complete, cancel, logSpend, session.properties.vendorScreen.GetBuyButtonPosition());
				session.ChangeState("HardSpendConfirm", false);
			}
			else
			{
				Purchase(session, instance);
			}
		}

		private void VendorRestockRush(Session session)
		{
			TFUtils.Assert(session != null && session.game != null && session.game.selected != null, "Trying to Rush Vendor restock in an invalid game state");
			VendingDecorator entityToRush = session.game.selected.GetEntity<VendingDecorator>();
			if (entityToRush == null)
			{
				TFUtils.Assert(false, "we have switched an entity to a non-Vendor!");
				session.ChangeState("Playing");
			}
			Cost fullCost = session.game.vendingManager.GetVendorDefinition(entityToRush.VendorId).RushCost;
			Cost lastCost = null;
			ulong startTime = entityToRush.RestockTime - entityToRush.RestockPeriod;
			int jellyCost = Cost.Prorate(fullCost, startTime, entityToRush.RestockTime, TFUtils.EpochTime()).ResourceAmounts[ResourceManager.HARD_CURRENCY];
			Cost.CostAtTime cost = delegate(ulong ts)
			{
				lastCost = Cost.Prorate(fullCost, startTime, entityToRush.RestockTime, ts);
				return lastCost;
			};
			Action execute = delegate
			{
				entityToRush.RestockTime = TFUtils.EpochTime();
				session.game.resourceManager.Spend(lastCost, session.game);
				RushRestockAction action = new RushRestockAction(session.game.selected.Id, lastCost);
				session.game.simulation.ModifyGameStateSimulated(session.game.selected, action);
				AnalyticsWrapper.LogJellyConfirmation(session.TheGame, entityToRush.DefinitionId, jellyCost, entityToRush.Name, "shops", "speedup", "store_restocking", "confirm");
			};
			Action<bool, Cost> logSpend = delegate(bool canAfford, Cost cost2)
			{
				session.analytics.LogRushRestock(entityToRush.BlueprintName, cost2.ResourceAmounts[ResourceManager.HARD_CURRENCY], canAfford);
			};
			Action cancel = delegate
			{
				session.ChangeState("vending");
			};
			session.properties.transitionSilently = true;
			session.properties.hardSpendActions = new HardSpendActions(execute, cost, "!!RESTOCK_STORE", entityToRush.DefinitionId, delegate
			{
				session.ChangeState("vending");
			}, cancel, logSpend, session.properties.vendorScreen.GetRestockRushPosition());
			session.ChangeState("HardSpendConfirm", false);
		}

		private void Purchase(Session session, VendingInstance instance)
		{
			if (session.game.resourceManager.CanPay(instance.Cost))
			{
				session.TheSoundEffectManager.PlaySound("Purchase");
				session.game.resourceManager.Apply(instance.Cost, session.game);
				Reward reward = session.game.vendingManager.GetStock(instance.StockId).GenerateReward(session.game.simulation);
				VendingAction action = new VendingAction(session.game.selected.Id, instance.SlotId, instance.Special, reward, instance.Cost);
				session.game.simulation.ModifyGameStateSimulated(session.game.selected, action);
				if (session.properties.reward == null)
				{
					session.properties.reward = reward;
				}
				else
				{
					session.properties.reward += reward;
				}
				instance.remaining--;
				return;
			}
			session.TheSoundEffectManager.PlaySound("Error");
			Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(instance.Cost.ResourceAmounts, session.game.resourceManager);
			if (resourcesStillRequired.Count > 0)
			{
				session.TheSoundEffectManager.PlaySound("Error");
				session.properties.transitionSilently = true;
				Action okAction = delegate
				{
					Purchase(session, instance);
					session.properties.transitionSilently = true;
					session.ChangeState("vending");
				};
				Action cancelAction = delegate
				{
					session.properties.transitionSilently = true;
					session.ChangeState("vending");
				};
				session.InsufficientResourcesHandler(session, session.game.vendingManager.GetStock(instance.StockId).Name, session.game.vendingManager.GetStock(instance.StockId).Did, okAction, cancelAction, instance.Cost);
			}
			else
			{
				TFUtils.ErrorLog("Was not able to purchase something but had enough resources in SessionVending.");
			}
		}

		private void Restock(Session session, Simulated simulated, bool refresh)
		{
			VendingDecorator entity = simulated.GetEntity<VendingDecorator>();
			bool flag = false;
			if (TFUtils.EpochTime() >= entity.RestockTime)
			{
				flag = true;
				session.game.vendingManager.GenerateNewGeneralInstances(entity);
				entity.RestockTime = TFUtils.EpochTime() + entity.RestockPeriod;
			}
			if (TFUtils.EpochTime() >= entity.SpecialRestockTime)
			{
				flag = true;
				session.game.vendingManager.GenerateNewSpecialInstances(entity);
				entity.SpecialRestockTime = TFUtils.EpochTime() + entity.SpecialRestockPeriod;
			}
			if (flag)
			{
				RestockVendorAction action = RestockVendorAction.Create(entity.Id, entity.RestockTime, entity.SpecialRestockTime, session.game.vendingManager.GetVendingInstances(entity.Id), session.game.vendingManager.GetSpecialInstances(entity.Id));
				session.game.simulation.ModifyGameStateSimulated(simulated, action);
				if (refresh)
				{
					session.properties.vendorScreen.UpdateVendingInstanceSlots(session);
				}
			}
		}
	}

	public class VisitGameStarting : State
	{
		private enum VisitStartingState
		{
			STATE_FIRST = -1,
			STATE_ASSEMBLE_GAME_STATE = 0,
			STATE_RETRIEVE_GAME_SAVE = 1,
			STATE_LOAD_ENTITY_BLUEPRINTS = 2,
			STATE_CREATE_GAME = 3,
			STATE_LOAD_ASSETS = 4,
			STATE_LOAD_ASSETS_TERRAIN = 5,
			STATE_LOAD_ASSETS_SIMULATION = 6,
			STATE_PRECACHE_GUI = 7,
			STATE_LOAD_ASSETS_TIME_DEPENDENTS = 8,
			STATE_LOAD_ASSETS_SEND_COMMANDS = 9,
			STATE_CREATE_TERRAIN_MESHES = 10,
			STATE_LOAD_ASSETS_ACTIVATE_QUESTS = 11,
			STATE_PROCESS_PENDING = 12,
			STATE_UNLOAD_UNUSED_ASSETS = 13,
			STATE_SETUP_SIMULATION = 14,
			STATE_LAST = 15,
			STATE_ERROR = 16
		}

		private class VisitFriendSoaringDelegate : SoaringDelegate
		{
			public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
			{
				if (!string.IsNullOrEmpty(module) && context != null && module == "retrieveSessionFromUser")
				{
					context.setValue(success, "success");
					if (error != null || !success)
					{
						context.ContextResponder(context);
						return;
					}
					SoaringArray soaringArray = new SoaringArray(1);
					SoaringDictionary soaringDictionary = new SoaringDictionary(1);
					soaringDictionary.addValue(data.soaringValue("gameSessionId"), "gameSessionId");
					soaringArray.addObject(soaringDictionary);
					Soaring.RequestSessionData(soaringArray, null, context);
				}
			}

			public override void OnRequestingSessionData(bool success, SoaringError error, SoaringArray sessions, SoaringDictionary raw_data, SoaringContext context)
			{
				if (!success || error != null || context == null || sessions == null)
				{
					success = false;
				}
				else if (sessions.count() == 0)
				{
					success = false;
				}
				if (success)
				{
					context.setValue(sessions.objectAtIndex(0), "game_session");
				}
				if (context != null)
				{
					context.setValue(success, "success");
					context.ContextResponder(context);
				}
			}
		}

		private delegate void ProcessStartingProgressState(Session session);

		public const uint VISIT_FRIEND_QUEST_ID = 2400u;

		public const uint VISIT_FRIEND_DIALOG_ID = 2401u;

		public const string VISIT_STARTING_PROGRESS = "visit_starting_progress";

		private const string POLICY_BUTTON = "policy_button";

		private SoaringDictionary FRIEND_SAVE_GAME;

		private int currentState = -1;

		private ProcessStartingProgressState[] processes;

		private int currentAdvance = 1;

		private int precacheGUIState;

		private int loadTimeDependentsState;

		private StaticContentLoader contentLoader;

		public bool blockUpdates;

		private SBGUIElement loadingSpinner;

		public bool attempLoadPatchTown = true;

		private AssetServices.AssetServicesMonitor mUnloadAssetMonitor;

		private void OnGameCreated(Session session)
		{
			DeferDialogs(session);
		}

		private void DeferDialogs(Session session)
		{
			Action action = delegate
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			AdvanceState(session);
		}

		public void OnLoadGameDelegate(SoaringContext context)
		{
		}

		private void LoadEntityBlueprints(Session session)
		{
			if (!contentLoader.LoadNextBlueprint())
			{
				CallLoadFromNetwork(session);
				AdvanceState(session);
			}
		}

		private void CallLoadFromNetwork(Session session, bool isRetryAttempt = false)
		{
		}

		public void OnEnter(Session session)
		{
			session.InFriendsGame = true;
			EntityManager.MustRegenerateStates = true;
			attempLoadPatchTown = true;
			blockUpdates = false;
			session.canChangeState = false;
			if (session.properties.playingHud != null)
			{
				session.properties.playingHud.Deactivate();
			}
			SBGUIScreen sBGUIScreen = GameStarting.CreateLoadingScreen(session, true, "visit_starting_progress");
			loadingSpinner = sBGUIScreen.FindChild("loading_spinner");
			session.properties.playDelayCounter = 0;
			precacheGUIState = 0;
			loadTimeDependentsState = 0;
			int num = 0;
			processes = new ProcessStartingProgressState[15];
			processes[num++] = AssembleGameState;
			processes[num++] = RequestGameState;
			processes[num++] = LoadEntityBlueprints;
			processes[num++] = CreateGame;
			processes[num++] = LoadAssets;
			processes[num++] = LoadLocalAssetsTerrain;
			processes[num++] = LoadLocalAssetsCreateSimulation;
			processes[num++] = PrecacheGUI;
			processes[num++] = LoadLocalAssetsLoadTimeDependents;
			processes[num++] = LoadLocalAssetsSendPendingCommands;
			processes[num++] = CreateTerrainMeshes;
			processes[num++] = LoadLocalAssetsActivateQuests;
			processes[num++] = ProcessTriggers;
			processes[num++] = HandleUnusedAssets;
			processes[num++] = SetupSimulation;
			SBGUIActivityIndicator sBGUIActivityIndicator = (SBGUIActivityIndicator)sBGUIScreen.FindChildSessionActionId("ActivityIndicator", false);
			sBGUIActivityIndicator.Center = new Vector3(4f, -2.7f, 3.2f);
			sBGUIActivityIndicator.StartActivityIndicator();
			session.soundEffectManager.Enabled = PlayerPrefs.GetInt(SoundEffectManager.SOUND_ENABLED) == 1;
			currentState = -1;
			AdvanceState(session);
		}

		public void OnLeave(Session session)
		{
			session.WasInFriendsGame = true;
			if (session.game != null)
			{
				session.game.CanSave = false;
			}
			EntityManager.MustRegenerateStates = true;
			SBGUIScreen sBGUIScreen = (SBGUIScreen)session.CheckAsyncRequest("visit_starting_progress");
			if (sBGUIScreen != null)
			{
				SBGUIActivityIndicator sBGUIActivityIndicator = (SBGUIActivityIndicator)sBGUIScreen.FindChildSessionActionId("ActivityIndicator", false);
				TFUtils.Assert(sBGUIActivityIndicator != null, "ActivityIndicator expected to be valid.");
				sBGUIActivityIndicator.StopActivityIndicator();
			}
			if (session.gameInitialized)
			{
				session.game.simulation.ClearPendingTimebarsInSimulateds();
				session.game.simulation.ClearPendingNamebarsInSimulateds();
				session.TheCamera.StartCamera();
				session.TheSoundEffectManager.StartSoundEffectsManager();
				session.musicManager.PlayTrack("InGame");
				SBUIBuilder.ReleaseTopScreen();
				sBGUIScreen = null;
				RestrictInteraction.AddWhitelistExpansion(session.game.simulation, int.MinValue);
			}
			GameStarting.ResetShowSplashScreen(GameStarting.SplashScreenState.None);
		}

		public static void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void OnUpdate(Session session)
		{
			currentAdvance = 1;
			if (currentState == 15)
			{
				SaveFriendGameTimeStamp();
				session.canChangeState = true;
				session.ChangeState("Playing");
				SoaringDictionary soaringDictionary = (SoaringDictionary)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_friends_reward_key");
				if (soaringDictionary == null)
				{
					soaringDictionary = new SoaringDictionary();
					Soaring.Player.PrivateData_Safe.setValue(soaringDictionary, "SBMI_friends_reward_key");
				}
				SoaringArray soaringArray = (SoaringArray)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_fdk");
				if (soaringArray != null)
				{
					for (int i = 0; i < soaringArray.count(); i++)
					{
						SoaringValue soaringValue = (SoaringValue)soaringArray.objectAtIndex(i);
						uint sequenceId = (uint)(long)soaringValue;
						DialogPackage dialogPackage = session.TheGame.dialogPackageManager.GetDialogPackage(1u);
						List<DialogInputData> dialogInputsInSequence = dialogPackage.GetDialogInputsInSequence(sequenceId, null, null);
						session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, dialogInputsInSequence, sequenceId);
						session.ChangeState("ShowingDialog");
					}
					soaringArray.clear();
				}
				SoaringArray soaringArray2 = (SoaringArray)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_completed_quest_key");
				if (soaringArray2 == null)
				{
					soaringArray2 = new SoaringArray();
				}
				soaringArray2.addObject(2400L);
				Soaring.Player.PrivateData_Safe.setValue(soaringArray2, "SBMI_completed_quest_key");
				Soaring.UpdateUserProfile(Soaring.Player.CustomData);
			}
			else
			{
				if (currentState == 16)
				{
					return;
				}
				SBGUIScreen sBGUIScreen = (SBGUIScreen)session.CheckAsyncRequest("visit_starting_progress");
				session.AddAsyncResponse("visit_starting_progress", sBGUIScreen);
				loadingSpinner.gameObject.transform.Rotate(new Vector3(0f, 0f, 1f), -250f * Time.deltaTime);
				float num = ((float)currentState + 1f) / 15f;
				sBGUIScreen.dynamicMeters["loading"].Progress = num;
				sBGUIScreen.dynamicLabels["progress"].SetText(string.Format("{0}%", ((int)(100f * num)).ToString()));
				try
				{
					if (!blockUpdates)
					{
						processes[currentState](session);
					}
				}
				catch (Exception ex)
				{
					Debug.LogError(ex.Message + "\n" + ex.StackTrace);
					DisplayFailedToLoadDialog(session);
				}
			}
		}

		private void AdvanceState(Session session)
		{
			lock (this)
			{
				currentState += currentAdvance;
				currentAdvance = 0;
			}
		}

		private void RequestGameState(Session session)
		{
			if (attempLoadPatchTown)
			{
				SoaringContext soaringContext = new SoaringContext();
				soaringContext.Name = "RetrieveFriendGame";
				soaringContext.addValue(new SoaringObject(session), "session");
				soaringContext.Responder = new VisitFriendSoaringDelegate();
				soaringContext.ContextResponder = GameRetrieved;
				FRIEND_SAVE_GAME = null;
				if (CheckFriendGameTimestamp())
				{
					GameRetrieved(soaringContext);
				}
				else
				{
					SBMISoaring.RetrieveUsersSession(soaringContext);
				}
				attempLoadPatchTown = false;
			}
		}

		private void SaveFriendGameTimeStamp()
		{
		}

		private bool CheckFriendGameTimestamp()
		{
			return false;
		}

		private void GameRetrieved(SoaringContext context)
		{
			Session session = null;
			bool flag = false;
			if (context != null)
			{
				SoaringObject soaringObject = (SoaringObject)context.objectWithKey("session");
				if (soaringObject != null)
				{
					session = (Session)soaringObject.Object;
				}
				if ((bool)context.soaringValue("success"))
				{
					FRIEND_SAVE_GAME = (SoaringDictionary)context.objectWithKey("game_session");
					if (FRIEND_SAVE_GAME != null && SBSettings.OfflineModeFriendParks)
					{
						try
						{
							string writePath = ResourceUtils.GetWritePath("game.json", "PatchyTown", 1);
							MBinaryWriter mBinaryWriter = new MBinaryWriter();
							if (mBinaryWriter.Open(writePath, true, true) && mBinaryWriter.IsOpen())
							{
								mBinaryWriter.Write(FRIEND_SAVE_GAME.ToJsonString());
								mBinaryWriter.Close();
							}
							mBinaryWriter = null;
						}
						catch (Exception ex)
						{
							TFUtils.ErrorLog(ex.Message + "\n" + ex.StackTrace);
						}
					}
				}
			}
			AdvanceState(session);
		}

		private void CreateGame(Session session)
		{
			SoaringDictionary fRIEND_SAVE_GAME = FRIEND_SAVE_GAME;
			bool flag = false;
			if (fRIEND_SAVE_GAME != null)
			{
				Dictionary<string, object> data = SBMISoaring.ConvertDictionaryToGeneric(fRIEND_SAVE_GAME);
				try
				{
					int performedMigration = 0;
					session.game = Game.LoadFromDataDict(data, session.Analytics, session.ThePlayer, contentLoader, out performedMigration, session.PlayHavenController);
					session.game.CanSave = false;
					OnGameCreated(session);
					flag = true;
				}
				catch (Exception ex)
				{
					Debug.LogError("Data Failed To Load: Using ServerGame: " + ex.Message + "\n" + ex.StackTrace);
					TFUtils.LogDump(session, "friend_save_error", ex);
				}
			}
			if (!flag && SBSettings.OfflineModeFriendParks)
			{
				try
				{
					MBinaryReader fileStream = ResourceUtils.GetFileStream("game", "PatchyTown", "json", 5);
					if (fileStream != null && fileStream.IsOpen())
					{
						fRIEND_SAVE_GAME = new SoaringDictionary(fileStream.ReadAllBytes());
						Dictionary<string, object> data2 = SBMISoaring.ConvertDictionaryToGeneric(fRIEND_SAVE_GAME);
						int performedMigration2 = 0;
						session.game = Game.LoadFromDataDict(data2, session.Analytics, session.ThePlayer, contentLoader, out performedMigration2, session.PlayHavenController);
						session.game.CanSave = false;
						OnGameCreated(session);
						flag = true;
					}
				}
				catch (Exception ex2)
				{
					Debug.LogError("Data Failed To Load: Using Local Game: " + ex2.Message + "\n" + ex2.StackTrace);
					TFUtils.LogDump(session, "friend_save_error", ex2);
				}
			}
			if (!flag)
			{
				DisplayFailedToLoadDialog(session);
			}
		}

		public void DisplayFailedToLoadDialog(Session session)
		{
			if (!blockUpdates)
			{
				Action okHandler = delegate
				{
					session.canChangeState = true;
					session.ChangeState("GameStarting");
				};
				SBUIBuilder.CreateErrorDialog(session, "Error", "Opps, We were unable to load Patchy Town\nAt this time.\nCome Back Later.", Language.Get("!!PREFAB_OK"), okHandler, 0.85f, 0.45f);
				blockUpdates = true;
			}
		}

		private void LoadAssets(Session session)
		{
			TFUtils.Assert(session.game != null, "VisitGameStarting.LoadAssets() expects session.game to not be null");
			contentLoader.TheEntityManager.LoadBlueprintResources();
			AdvanceState(session);
		}

		private void CreateTerrainMeshes(Session session)
		{
			session.game.terrain.CreateTerrainMeshes();
			AdvanceState(session);
		}

		private void AwaitProductInfo(Session session)
		{
			if (session.TheGame.store.receivedProductInfo)
			{
				AdvanceState(session);
			}
		}

		private void ProcessTriggers(Session session)
		{
			AdvanceState(session);
		}

		private void HandleUnusedAssets(Session session)
		{
			if (mUnloadAssetMonitor == null)
			{
				mUnloadAssetMonitor = AssetServices.CreateUnloadUnusedAssetService(null);
			}
			else if (mUnloadAssetMonitor.IsCompleted)
			{
				AdvanceState(session);
				mUnloadAssetMonitor = null;
			}
		}

		private void SetupSimulation(Session session)
		{
			if (!session.gameInitialized)
			{
				session.GameInitialized(true);
				session.game.simulation.OnUpdateVisitParkState(session);
				session.game.treasureManager.StartTreasureTimers();
				session.game.playtimeRegistrar.UpdatePlaytime(TFUtils.EpochTime());
			}
			else if (!session.game.needsNetworkDownErrorDialog)
			{
				AdvanceState(session);
			}
		}

		private void AssembleGameState(Session session)
		{
			SBSettings.Init();
			SBUIBuilder.ClearScreenCache();
			if (Language.CurrentLanguage() == LanguageCode.N)
			{
				Language.Init(TFUtils.GetPersistentAssetsPath());
			}
			contentLoader = new StaticContentLoader();
			contentLoader.LoadContent(session);
			session.DropGame();
			AdvanceState(session);
		}

		private void LoadLocalAssetsTerrain(Session session)
		{
			contentLoader.Initialize();
			AdvanceState(session);
		}

		private void LoadLocalAssetsCreateSimulation(Session session)
		{
			TFUtils.DebugLog("Creating simulation");
			session.game.simulation = new Simulation(session.game.NULL_ModifyGameState, session.game.NULL_ModifyStateSimulated, null, session.game.actionBuffer.Record, session.game, session.game.entities, session.game.triggerRouter, session.game.resourceManager, session.game.dropManager, session.TheSoundEffectManager, session.game.resourceCalculatorManager, session.game.craftManager, session.game.movieManager, session.game.featureManager, session.game.catalog, session.game.rewardCap, session.camera.UnityCamera, session.game.terrain, 5, session.analytics, session.simulationScratchScreen, contentLoader.TheEnclosureManager);
			AdvanceState(session);
		}

		private void PrecacheGUI(Session session)
		{
			TFUtils.DebugLogTimed("In PrecacheGUI");
			if (precacheGUIState == 0)
			{
				SBGUIRewardWidget.MakeRewardWidgetPool();
				precacheGUIState++;
			}
			else
			{
				AdvanceState(session);
			}
		}

		private void LoadLocalAssetsLoadTimeDependents(Session session)
		{
			ulong utcNow = TFUtils.EpochTime();
			switch (loadTimeDependentsState)
			{
			case 0:
				session.game.LoadSimulation(utcNow);
				loadTimeDependentsState++;
				break;
			case 1:
				if (session.game.IterateLoadSimulation())
				{
					loadTimeDependentsState++;
				}
				break;
			case 2:
				session.game.LoadExpansions(utcNow);
				loadTimeDependentsState++;
				break;
			case 3:
				if (session.game.IterateLoadExpansions())
				{
					loadTimeDependentsState++;
				}
				break;
			default:
				AdvanceState(session);
				break;
			}
		}

		private void LoadLocalAssetsSendPendingCommands(Session session)
		{
			session.game.simulation.SendPendingCommands();
			AdvanceState(session);
		}

		private void LoadLocalAssetsActivateQuests(Session session)
		{
			TFUtils.DebugLog("GAME INITIALIZED");
			if (session.game.dialogPackageManager.GetNumQueuedDialogInputs() > 0)
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			}
			AdvanceState(session);
		}

		private void CreateErrorDialog(Session session, string title, string message, string okButtonLabel, Action okHandler, float messageScale, float titleScale)
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
			SBGUIConfirmationDialog sBGUIConfirmationDialog = SBUIBuilder.MakeAndPushConfirmationDialog(session, null, title, message, okButtonLabel, null, null, okButtonHandler, null);
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

	public class TimebarMixin
	{
		public const int YOFFSET = 20;

		public const int HEIGHT = 100;

		private const string TIMEBAR = "Timebar";

		private SBGUITimebar timebarGUI;

		private string gameObjectID;

		public bool IsActive
		{
			get
			{
				return timebarGUI != null && timebarGUI.IsActive();
			}
		}

		public bool ActivateOnSelected(Session session, Simulated simulated, float yOffset = 20f)
		{
			bool result = false;
			Simulated selected = session.game.selected;
			TFUtils.Assert(simulated != null, "Cannot enable Timebar unless there is a simulated");
			if (simulated.timebarMixinArgs.hasTimebar && simulated.timebarMixinArgs.duration > 0f)
			{
				SBGUITimebar.HostPosition hPosition = delegate
				{
					if (session.game == null)
					{
						return Vector3.zero;
					}
					return (session.game.simulation == null) ? Vector3.zero : ((Vector3)session.game.simulation.ScreenPositionFromWorldPosition(selected.Position));
				};
				Action goBackToPlaying = delegate
				{
					session.ChangeState("Playing");
					session.game.selected = null;
				};
				Action goBackToPlayingCancel = delegate
				{
					session.ChangeState("Playing");
					if (session.game.selected.rushParameters != null && session.game.selected.rushParameters.cancel != null)
					{
						session.game.selected.rushParameters.cancel(session);
					}
					session.game.selected = null;
				};
				List<int> list = null;
				Action<int> pTaskCharacterClicked = null;
				if (simulated.timebarMixinArgs.m_bCheckForTaskCharacters)
				{
					list = session.TheGame.taskManager.GetActiveSourcesForTarget(simulated.Id);
					int num = list.Count;
					for (int num2 = 0; num2 < num; num2++)
					{
						List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(list[num2], null);
						if (activeTasksForSimulated.Count > 0 && activeTasksForSimulated[0].GetTimeLeft() == 0)
						{
							list.RemoveAt(num2);
							num2--;
							num--;
						}
					}
					if (list != null && list.Count > 0)
					{
						pTaskCharacterClicked = delegate(int nDID)
						{
							session.CheckAsyncRequest(SelectedPlaying.TASK_CHARACTER_SELECT);
							session.AddAsyncResponse(SelectedPlaying.TASK_CHARACTER_SELECT, nDID, false);
						};
					}
				}
				SBGUITimebar timebar = null;
				timebar = SBUIBuilder.MakeAndAddTimebar(session, session.SimulationSBGUIScreen, (uint)simulated.entity.DefinitionId, simulated.timebarMixinArgs.description, simulated.timebarMixinArgs.completeTime, simulated.timebarMixinArgs.totalTime, simulated.timebarMixinArgs.duration, simulated.timebarMixinArgs.rushCost, delegate
				{
					if (!session.game.simulation.Whitelisted || session.game.simulation.CheckWhitelist(simulated) || timebar == null || SBGUI.GetInstance().CheckWhitelisted(timebar.RushButton))
					{
						DoRush(session, simulated, goBackToPlaying, goBackToPlayingCancel);
						timebar.RemoveCompleteAction();
						timebar.Close();
					}
				}, hPosition, delegate
				{
					if (selected == session.game.selected && (!session.game.simulation.Whitelisted || session.game.simulation.CheckWhitelist(simulated) || timebar == null || SBGUI.GetInstance().CheckWhitelisted(timebar.RushButton)))
					{
						goBackToPlaying();
					}
				}, list, pTaskCharacterClicked);
				gameObjectID = timebar.gameObject.name;
				timebarGUI = timebar;
				result = true;
			}
			return result;
		}

		public void DoRush(Session session, Simulated simulated, Action goBackToPlaying, Action goBackToPlayingCancel)
		{
			session.properties.hardSpendActions = new HardSpendActions(delegate
			{
				simulated.rushParameters.execute(session);
			}, (ulong ts) => Cost.Prorate(simulated.timebarMixinArgs.rushCost, simulated.timebarMixinArgs.completeTime - simulated.timebarMixinArgs.totalTime, simulated.timebarMixinArgs.completeTime, ts), simulated.rushParameters.subject, simulated.rushParameters.did, goBackToPlaying, goBackToPlayingCancel, delegate(bool canAfford, Cost cost)
			{
				simulated.rushParameters.log(session, cost, canAfford);
			}, GetRushButtonScreenPosition());
			session.game.selected = simulated;
			session.ChangeState("HardSpendConfirm", false);
		}

		public void Deactivate(Session session)
		{
			if (gameObjectID != null && timebarGUI != null)
			{
				timebarGUI.Close();
			}
		}

		public void Extend()
		{
			if (timebarGUI != null)
			{
				timebarGUI.elapsed = 0f;
			}
		}

		private Vector2 GetRushButtonScreenPosition()
		{
			return timebarGUI.GetRushButtonScreenPosition();
		}
	}

	public class TimebarGroup
	{
		public const string TASK_SRC_UNIT = "TaskSrcUnit";

		private TimebarMixin taskAtBuildingTimebar = new TimebarMixin();

		private TimebarMixin timebar = new TimebarMixin();

		public bool IsActive
		{
			get
			{
				return timebar != null && timebar.IsActive;
			}
		}

		public void ActivateOnSelected(Session session)
		{
			if (session.game.selected.Variable.ContainsKey("TaskSrcUnit"))
			{
				taskAtBuildingTimebar.ActivateOnSelected(session, (Simulated)session.game.selected.Variable["TaskSrcUnit"], 120f);
			}
			timebar.ActivateOnSelected(session, session.game.selected, 20f);
		}

		public void Deactivate(Session session)
		{
			timebar.Deactivate(session);
			if (session.game.selected != null && session.game.selected.Variable.ContainsKey("TaskSrcUnit"))
			{
				taskAtBuildingTimebar.Deactivate(session);
			}
		}

		public void Extend()
		{
			timebar.Extend();
		}
	}

	public class AcceptPlacementControl : BaseControlBinding
	{
		public AcceptPlacementControl()
			: this(null)
		{
		}

		public AcceptPlacementControl(Action callback)
		{
			Initialize(delegate(Session session)
			{
				OnClick(session);
			}, callback, "Accept");
		}

		public override void DynamicUpdate(Session session)
		{
			SBUIBuilder.UpdateAcceptPlacementButton(base.DynamicButton, session);
		}

		private void OnClick(Session session)
		{
			if (!TheDebugManager.debugPlaceObjects && session.TheGame.simulation.PlacementQuery(session.TheGame.selected) == Simulation.Placement.RESULT.INVALID)
			{
				session.TheSoundEffectManager.PlaySound("Cancel");
				return;
			}
			Action<Session> action = (Action<Session>)session.CheckAsyncRequest("InteractionStrip_AcceptCallback");
			if (action != null)
			{
				session.AddAsyncResponse("InteractionStrip_AcceptCallback", action);
				action(session);
			}
			if (base.Callback != null)
			{
				base.Callback();
			}
		}
	}

	public class BrowseRecipesControl : BaseControlBinding
	{
		public BrowseRecipesControl(Simulated toBrowse)
		{
			BrowseRecipesControl browseRecipesControl = this;
			Initialize(delegate(Session session)
			{
				browseRecipesControl.OnClick(session, toBrowse);
			}, null, "Browse");
		}

		private void OnClick(Session session, Simulated toBrowse)
		{
			TFUtils.Assert(toBrowse == session.game.selected, "Trying to open a simulated other than the selected one. This is probably wrong");
			session.ChangeState("BrowsingRecipes");
		}
	}

	public class ClearDebrisControl : BaseControlBinding
	{
		public ClearDebrisControl(Simulated toClear)
		{
			ClearDebrisControl clearDebrisControl = this;
			Initialize(delegate(Session session)
			{
				clearDebrisControl.OnClick(session, toClear);
			}, null, "Clear");
			ClearableDecorator entity = toClear.GetEntity<ClearableDecorator>();
			TFUtils.Assert(entity != null, "Null clearable pointer.  Shouldn't happen if you just touched a clearable entity.");
			Label = entity.ClearCost.ResourceAmounts[ResourceManager.SOFT_CURRENCY].ToString();
		}

		private void OnClick(Session session, Simulated toClear)
		{
			if (session.game.featureManager.CheckFeature("debris_clearing"))
			{
				TFUtils.Assert(toClear == session.game.selected, "Trying to clear a simulated other than the selected one. This is a bad idea");
				session.ChangeState("Clearing", false);
			}
		}

		public override void DynamicUpdate(Session session)
		{
			bool enabled = session.game.featureManager.CheckFeature("debris_clearing");
			SBUIBuilder.UpdateButton(base.DynamicButton, enabled);
		}
	}

	public static class PushForPlacementHelper
	{
		public static void PushPlacementConfirmation(Session session, Simulated subject)
		{
			session.ChangeState("MoveBuildingInEdit");
		}
	}

	public class RejectControl : BaseControlBinding
	{
		public RejectControl()
			: this(null)
		{
		}

		public RejectControl(Action callback)
		{
			Initialize(delegate(Session session)
			{
				OnClick(session);
			}, callback, "Reject");
		}

		private void OnClick(Session session)
		{
			Action<Session> action = (Action<Session>)session.CheckAsyncRequest("InteractionStrip_RejectCallback");
			if (action != null)
			{
				session.AddAsyncResponse("InteractionStrip_RejectCallback", action);
				action(session);
			}
			if (base.Callback != null)
			{
				base.Callback();
			}
		}
	}

	public class RotateControl : BaseControlBinding
	{
		private bool isEnabled;

		public RotateControl(Simulated toRotate, bool isEnabled, Simulation simulation = null)
		{
			RotateControl rotateControl = this;
			Initialize(delegate(Session session)
			{
				rotateControl.OnClick(session, toRotate, simulation);
			}, null, "Rotate");
			this.isEnabled = isEnabled;
		}

		private void OnClick(Session session, Simulated toRotate, Simulation simulation)
		{
			if (isEnabled)
			{
				session.TheSoundEffectManager.PlaySound("Rotate");
				if (simulation != null)
				{
					toRotate.RemoveFence(simulation);
					toRotate.RemoveScaffolding(simulation);
					toRotate.Flip = !toRotate.Flip;
					toRotate.Animate(simulation);
					toRotate.DisplayController.SetMaskPercentage(0f);
					toRotate.AddFence(simulation);
					toRotate.AddScaffolding(simulation);
				}
				else
				{
					toRotate.Flip = !toRotate.Flip;
					toRotate.FlipWarp(session.TheGame.simulation);
				}
			}
		}

		public override void DynamicUpdate(Session session)
		{
			YGAtlasSprite component = base.DynamicButton.GetComponent<YGAtlasSprite>();
			if (isEnabled)
			{
				component.SetAlpha(1f);
			}
			else
			{
				component.SetAlpha(0.25f);
			}
		}
	}

	public class RushControl : BaseControlBinding
	{
		public RushControl(Simulated toRush)
		{
			RushControl rushControl = this;
			Initialize(delegate(Session session)
			{
				rushControl.OnClick(session, toRush);
			}, null, "Rush");
		}

		private void OnClick(Session session, Simulated toSell)
		{
			session.ChangeState("SelectedPlaying");
		}
	}

	public class SellControl : BaseControlBinding
	{
		private bool isEnabled;

		public SellControl(Simulated toSell, bool isEnabled, string sellError)
		{
			SellControl sellControl = this;
			Simulated pToSell = toSell;
			string sSellError = sellError;
			Initialize(delegate(Session session)
			{
				sellControl.OnClick(session, pToSell, sSellError);
			}, null, "Sell");
			this.isEnabled = isEnabled;
		}

		private void OnClick(Session session, Simulated toSell, string sellError)
		{
			if (isEnabled || !string.IsNullOrEmpty(sellError))
			{
				if (!string.IsNullOrEmpty(sellError))
				{
					session.AddAsyncResponse("sell_error", sellError);
				}
				session.AddAsyncResponse("to_sell", toSell);
				TFUtils.Assert(toSell == session.game.selected, "Trying to sell a simulated other than the selected one. This is probably wrong");
				session.properties.waitToDecidePlacement = true;
				session.AddAsyncResponse("in_state_move_in_edit", session.TheState.GetType().Equals(typeof(MoveBuildingInEdit)));
				session.ChangeState("SellBuildingConfirmation");
			}
		}

		public override void DynamicUpdate(Session session)
		{
			YGAtlasSprite component = base.DynamicButton.GetComponent<YGAtlasSprite>();
			if (isEnabled)
			{
				component.SetAlpha(1f);
			}
			else
			{
				component.SetAlpha(0.25f);
			}
		}
	}

	public class StashControl : BaseControlBinding
	{
		private bool isEnabled;

		public StashControl(Simulated toStash, bool isEnabled, string stashError)
		{
			StashControl stashControl = this;
			string sStashError = stashError;
			Initialize(delegate(Session session)
			{
				stashControl.OnClick(session, toStash, sStashError);
			}, null, "Stash");
			this.isEnabled = isEnabled;
		}

		private void OnClick(Session session, Simulated toStash, string stashError)
		{
			if (!isEnabled && string.IsNullOrEmpty(stashError))
			{
				return;
			}
			if (!session.game.featureManager.CheckFeature("stash_soft"))
			{
				session.game.featureManager.UnlockFeature("stash_soft");
				session.game.featureManager.ActivateFeatureActions(session.game, "stash_soft");
				session.game.simulation.ModifyGameState(new FeatureUnlocksAction(new List<string> { "stash_soft" }));
				return;
			}
			session.TheSoundEffectManager.PlaySound("Stash");
			if (!string.IsNullOrEmpty(stashError))
			{
				session.AddAsyncResponse("stash_error", stashError);
				session.AddAsyncResponse("to_stash", toStash);
				session.AddAsyncResponse("in_state_move_in_edit", session.TheState.GetType().Equals(typeof(MoveBuildingInEdit)));
				session.properties.waitToDecidePlacement = true;
				session.ChangeState("StashBuildingConfirmation");
				return;
			}
			BuildingEntity entity = toStash.GetEntity<BuildingEntity>();
			SwarmManager.Instance.RestoreResidents(session.game.simulation, toStash);
			List<Simulated> list = Simulated.Building.FindResidents(session.game.simulation, toStash);
			List<KeyValuePair<int, Identity>> associatedEntities = list.ConvertAll((Simulated simulated) => new KeyValuePair<int, Identity>(simulated.entity.DefinitionId, simulated.Id));
			session.game.inventory.AddItem(entity, associatedEntities);
			foreach (Simulated item in list)
			{
				ResidentEntity entity2 = item.GetEntity<ResidentEntity>();
				ulong hungryAt = entity2.HungryAt;
				ulong num = TFUtils.EpochTime();
				ulong hungryAt2 = hungryAt - num;
				entity2.HungryAt = hungryAt2;
			}
			session.game.simulation.ModifyGameStateSimulated(toStash, new MoveAction(toStash.Id, null, null, null, list));
			foreach (Simulated item2 in list)
			{
				SwarmManager.Instance.RemoveResident(item2.GetEntity<ResidentEntity>(), toStash);
				session.game.simulation.RemoveSimulated(item2);
			}
			session.game.simulation.TryWorkerSpawnerCleanup(toStash.Id);
			session.game.selected.FootprintVisible = false;
			toStash.SetFootprint(session.game.simulation, false);
			session.game.simulation.RemoveSimulated(toStash);
			session.game.selected = null;
			bool? flag = (bool?)session.CheckAsyncRequest("FromInventory");
			if (flag.HasValue)
			{
				session.AddAsyncResponse("FromInventory", flag.Value);
			}
			if ((flag.HasValue && flag.Value) || session.properties.cameFromMarketplace)
			{
				session.ChangeState("Playing");
			}
			else
			{
				session.ChangeState("Editing");
			}
		}

		public override void DynamicUpdate(Session session)
		{
			YGAtlasSprite component = base.DynamicButton.GetComponent<YGAtlasSprite>();
			if (isEnabled)
			{
				component.SetAlpha(1f);
			}
			else
			{
				component.SetAlpha(0.25f);
			}
		}
	}

	public class SelectedStateTransition : BaseTransitionBinding
	{
		private string targetState;

		public SelectedStateTransition(Simulated targetSim, string state)
		{
			SelectedStateTransition selectedStateTransition = this;
			targetState = state;
			Initialize(delegate(Session session)
			{
				selectedStateTransition.OnClick(session, targetSim);
			});
		}

		private void OnClick(Session session, Simulated targetSim)
		{
			TFUtils.Assert(targetSim == session.game.selected, "Trying to open a simulated other than the selected one. This is probably wrong");
			session.ChangeState(targetState);
		}
	}

	public class BrowseRecipesTransition : SelectedStateTransition
	{
		public BrowseRecipesTransition(Simulated targetSim)
			: base(targetSim, "BrowsingRecipes")
		{
		}
	}

	public class VendingTransition : SelectedStateTransition
	{
		public VendingTransition(Simulated targetSim)
			: base(targetSim, "vending")
		{
		}
	}

	public class UnitIdleTransition : SelectedStateTransition
	{
		public UnitIdleTransition(Simulated targetSim)
			: base(targetSim, "UnitIdle")
		{
		}
	}

	public class UnitBusyTransition : SelectedStateTransition
	{
		public UnitBusyTransition(Simulated targetSim)
			: base(targetSim, "UnitBusy")
		{
		}
	}

	public class ShowTreasureRewardTransition : BaseTransitionBinding
	{
		public ShowTreasureRewardTransition(Simulated toShow)
		{
			ShowTreasureRewardTransition showTreasureRewardTransition = this;
			Initialize(delegate(Session session)
			{
				showTreasureRewardTransition.OnClick(session, toShow);
			});
		}

		private void OnClick(Session session, Simulated toShow)
		{
			session.game.simulation.Router.Send(ClickedCommand.Create(toShow.Id, toShow.Id));
		}
	}

	public delegate void GameloopAction();

	public delegate void AsyncAction();

	private const string CHECK_PATCHING = "CheckPatching";

	private const string GAME_STARTING = "GameStarting";

	private const string GAME_STOPPING = "GameStopping";

	private const string AUTHORIZING = "Authorizing";

	public const string PLAYING = "Playing";

	public const string SELECTED_PLAYING = "SelectedPlaying";

	private const string EDITING = "Editing";

	private const string MOVE_IN_EDIT = "MoveBuildingInEdit";

	private const string MOVE_IN_PLACEMENT = "MoveBuildingInPlacement";

	private const string MOVE_PANNING_IN_EDIT = "MoveBuildingPanningInEdit";

	private const string MOVE_PANNING_IN_PLACEMENT = "MoveBuildingPanningInPlacement";

	private const string PLACING = "Placing";

	private const string PAVING = "Paving";

	private const string DRAG_FEEDING = "DragFeeding";

	private const string SHOPPING = "Shopping";

	private const string INVENTORY = "Inventory";

	private const string COMMUNITY_EVENT = "CommunityEvent";

	public const string BROWSING_RECIPES = "BrowsingRecipes";

	private const string SYNC = "Sync";

	private const string STOPPING = "Stopping";

	private const string IN_APP_PURCHASING = "InAppPurchasing";

	private const string SHOWING_DIALOG = "ShowingDialog";

	private const string HARD_SPEND_CONFIRM = "HardSpendConfirm";

	private const string HARD_SPEND_PASSTHROUGH = "HardSpendPassthrough";

	private const string INSUFFICIENT_DIALOG = "InsufficientDialog";

	private const string EXPANSION = "Expansion";

	private const string EXPANDING = "Expanding";

	private const string CLEARING = "Clearing";

	private const string OPTIONS = "Options";

	private const string MOVIE = "Movie";

	private const string DEBUG = "Debug";

	private const string ERROR_DIALOG = "ErrorDialog";

	private const string GET_JELLY = "GetJelly";

	private const string CREDITS = "Credits";

	private const string MOVIE_START_TIME = "MovieStartTime";

	private const string SELL_BUILDING_CONFIRMATION = "SellBuildingConfirmation";

	private const string STASH_BUILDING_CONFIRMATION = "StashBuildingConfirmation";

	public const string VENDING = "vending";

	public const string UNIT_IDLE = "UnitIdle";

	public const string UNIT_BUSY = "UnitBusy";

	public const string AGE_GATE = "AgeGate";

	protected const string ERROR_MESSAGE_TITLE = "error_message_title";

	protected const string ERROR_MESSAGE = "error_message";

	protected const string ERROR_MESSAGE_OK_LABEL = "error_message_ok_label";

	protected const string ERROR_MESSAGE_OK_ACTION = "error_message_ok_action";

	protected const string ERROR_MESSAGE_SCALE = "error_message_scale";

	protected const string JELLY_MESSAGE_TITLE = "jelly_message_title";

	protected const string JELLY_MESSAGE = "jelly_message";

	protected const string JELLY_QUESTION = "jelly_question";

	protected const string JELLY_MESSAGE_OK_LABEL = "jelly_message_ok_label";

	protected const string JELLY_MESSAGE_CANCEL_LABEL = "jelly_message_cancel_label";

	protected const string JELLY_MESSAGE_OK_ACTION = "jelly_message_ok_action";

	protected const string JELLY_MESSAGE_CANCEL_ACTION = "jelly_message_cancel_action";

	protected const string INSUFFICIENT_ITEM_DID = "insufficient_item_did";

	protected const string INSUFFICIENT_ITEMNAME = "insufficient_itemname";

	protected const string INSUFFICIENT_RESOURCES = "insufficient_resources";

	protected const string INSUFFICIENT_CANCEL = "insufficient_cancel";

	protected const string INSUFFICIENT_ACCEPT = "insufficient_accept";

	private const string USER_LOGIN = "userLogin";

	private const string EXPANDING_UI_HANDLER = "expanding_ui";

	protected const string EXPANSION_SCREEN = "expansion";

	private const string EXPANSION_UI_HANDLER = "expansion_ui";

	private const string STARTING_PROGRESS = "starting_progress";

	private const int TERRAIN_DEPTH = 5;

	public const string TARGET_STORE_TAB = "target_store_tab";

	public const string TARGET_STORE_DID = "target_store_did";

	public const string CURRENT_UI_EVENT = "CurrentGuiEventInfo";

	protected const string IN_STATE_MOVE_IN_EDIT = "in_state_move_in_edit";

	private const string DIALOGS_TO_SHOW = "dialogs_to_show";

	private const string PLAYING_UI_HANDLER = "playing_ui";

	public const string STANDARD_SCREEN = "standard_screen";

	public const string LEVELUP_SCREEN = "levelup_screen";

	public const string CLEAR_PURCHASE_ON_MOVEMENT = "clear_purchase_on_movement";

	private const string RESOLVE_USER = "resolve_user";

	public const string TRANSACTION_OFFER = "transaction_offer";

	public const string STORE_OPEN_TYPE = "store_open_type";

	public const bool DEBUG_LOG = true;

	private const string VISIT_FRIEND_STARTING = "visit_friend";

	protected const string TO_SELL = "to_sell";

	protected const string SELL_ERROR = "sell_error";

	protected const string TO_STASH = "to_stash";

	protected const string STASH_ERROR = "stash_error";

	private static Dictionary<string, State> states;

	private SBGamePersister gameSaver;

	private CallbackQueue callbackQueue = new CallbackQueue();

	public PlayHavenController PlayHavenController;

	public SBAnalytics analytics;

	public SBContentPatcher contentPatcher;

	public bool notifyOnDisconnect;

	public bool gameInitialized;

	public bool reinitializeSession;

	public bool resyncConnection;

	public bool gameIsReloading;

	public static bool PatchyTownGame;

	public bool WasInFriendsGame;

	public bool musicStateBeforePT;

	public bool sfxStateBeforePT;

	public bool haveReloaded;

	public PushNotificationManager pushNotificationManager;

	public GameObject statisticsTracker;

	public SBStatisticsTracker tracker;

	private static ulong logDumpShake;

	private bool lastOnlineState = true;

	private bool isShowingOfflineDialog;

	public bool canChangeState = true;

	private bool checkForPatching;

	private bool justCheckForUpdates;

	public SoaringArray soaringEvents = new SoaringArray();

	public DateTime lastResetTime;

	private ulong? m_ulPauseTimestamp;

	private AndroidJavaObject androidActivity;

	public FramerateWatcher framerateWatcher = new FramerateWatcher();

	private State state;

	private bool saveGame;

	private Player player;

	private Game game;

	private SBCamera camera;

	private SBWebFileServer webFileServer;

	private SBAuth auth;

	private static DebugManager debugManager;

	private float lastUpdateTime;

	private int currentVersion;

	private List<GameloopAction> actions;

	private MusicManager musicManager;

	private SoundEffectManager soundEffectManager;

	private SBUIBuilder.ScreenContext simulationContext;

	private SBGUIScreen simulationScratchScreen;

	private SBUIBuilder.ScreenContext currentGuiContext;

	private List<StateChangeRequest> queuedStateChanges = new List<StateChangeRequest>();

	private Dictionary<string, TFServer.JsonResponseHandler> externalRequests = new Dictionary<string, TFServer.JsonResponseHandler>();

	private SessionProperties properties = new SessionProperties();

	private Dictionary<string, object> asyncRequests = new Dictionary<string, object>();

	private Dictionary<string, TFWebClient> asyncFileRequests = new Dictionary<string, TFWebClient>();

	private BubbleSwipeParticleSystemRequestDelegate bubbleSwipeParticleSystemRequestDelegate;

	private ConfettiSwipeParticleSystemRequestDelegate confettiSwipeParticleSystemRequestDelegate;

	private BalloonSwipeParticleSystemRequestDelegate balloonSwipeParticleSystemRequestDelegate;

	private SeaflowerSwipeParticleSystemRequestDelegate seaFlowerSwipeParticleSystemRequestDelegate;

	private FogEffectRequestDelegate fogEffectRequestDelegate;

	private TapFXParticleSystemRequestDelegate tapFXParticleSystemRequestDelegate;

	private static readonly object expandLock;

	public static bool didRegisterNotifications;

	private static bool loggingTimedDependents;

	private static bool draggingCamera;

	public bool marketpalceActive;

	public bool InFriendsGame
	{
		get
		{
			return PatchyTownGame;
		}
		set
		{
			PatchyTownGame = value;
		}
	}

	public SBWebFileServer WebFileServer
	{
		get
		{
			return webFileServer;
		}
	}

	public SBAnalytics Analytics
	{
		get
		{
			return analytics;
		}
	}

	public static DebugManager TheDebugManager
	{
		get
		{
			return debugManager;
		}
	}

	public SBAuth Auth
	{
		get
		{
			return auth;
		}
	}

	public Game TheGame
	{
		get
		{
			return game;
		}
	}

	public CallbackQueue CallbackQueue
	{
		get
		{
			return callbackQueue;
		}
	}

	public SBCamera TheCamera
	{
		get
		{
			return camera;
		}
	}

	public Player ThePlayer
	{
		get
		{
			return player;
		}
		set
		{
			player = value;
		}
	}

	public SoundEffectManager TheSoundEffectManager
	{
		get
		{
			return soundEffectManager;
		}
	}

	public State TheState
	{
		get
		{
			return state;
		}
	}

	public SBGUIScreen SimulationSBGUIScreen
	{
		get
		{
			return simulationScratchScreen;
		}
	}

	public Session(int currentVersion)
	{
		TFUtils.Init();
		SBSettings.Init();
		analytics = new SBAnalytics();
		actions = new List<GameloopAction>();
		webFileServer = new SBWebFileServer();
		auth = new SBAuth(Application.platform);
		this.currentVersion = currentVersion;
		camera = new SBCamera();
		musicManager = MusicManager.CreateMusicManager();
		soundEffectManager = SoundEffectManager.CreateSoundEffectManager();
		pushNotificationManager = new PushNotificationManager(this);
		if (debugManager == null)
		{
			debugManager = new DebugManager(this);
		}
		InitScreenSwipeEffects();
		simulationContext = SBUIBuilder.PushNewScreenContext();
		simulationScratchScreen = SBUIBuilder.MakeAndPushScratchLayer(this);
		simulationScratchScreen.name = "Simulation Scratch Screen Layer";
		simulationScratchScreen.SetPosition(new Vector3(0f, 0f, GUIMainView.GetInstance().GetComponent<Camera>().farClipPlane - 0.1f));
		currentGuiContext = SBUIBuilder.PushNewScreenContext();
		state = states["Authorizing"];
		reinitializeSession = false;
		PlayHavenController = new PlayHavenController();
		lastResetTime = DateTime.UtcNow;
		state.OnEnter(this);
	}

	static Session()
	{
		expandLock = new object();
		states = new Dictionary<string, State>();
		states["CheckPatching"] = new CheckPatching();
		states["Stopping"] = new Stopping();
		states["Authorizing"] = new Authorizing();
		states["GameStarting"] = new GameStarting();
		states["GameStopping"] = new GameStopping();
		states["Playing"] = new Playing();
		states["SelectedPlaying"] = new SelectedPlaying();
		states["Shopping"] = new Shopping();
		states["Inventory"] = new Inventory();
		states["CommunityEvent"] = new CommunityEventSession();
		states["BrowsingRecipes"] = new BrowsingRecipes();
		states["Paving"] = new Paving();
		states["Placing"] = new Placing();
		states["MoveBuildingInEdit"] = new MoveBuildingInEdit();
		states["MoveBuildingInPlacement"] = new MoveBuildingInPlacement();
		states["MoveBuildingPanningInEdit"] = new MoveBuildingPanningInEdit();
		states["MoveBuildingPanningInPlacement"] = new MoveBuildingPanningInPlacement();
		states["DragFeeding"] = new DragFeeding();
		states["Sync"] = new Sync();
		states["InAppPurchasing"] = new InAppPurchasing();
		states["ShowingDialog"] = new ShowingDialog();
		states["HardSpendConfirm"] = new HardSpendConfirm();
		states["HardSpendPassthrough"] = new HardSpendPassthrough();
		states["InsufficientDialog"] = new InsufficientDialog();
		states["Expansion"] = new NewExpansion();
		states["Expanding"] = new Expanding();
		states["Clearing"] = new Clearing();
		states["Options"] = new Options();
		states["Movie"] = new Movie();
		states["Debug"] = new SessionDebug();
		states["ErrorDialog"] = new ErrorDialog();
		states["GetJelly"] = new GetJelly();
		states["Editing"] = new Editing();
		states["Credits"] = new Credits();
		states["SellBuildingConfirmation"] = new SellBuildingConfirmation();
		states["StashBuildingConfirmation"] = new StashBuildingConfirmation();
		states["vending"] = new Vending();
		states["resolve_user"] = new ResolveUser();
		states["visit_friend"] = new VisitGameStarting();
		states["UnitBusy"] = new UnitBusy();
		states["UnitIdle"] = new UnitIdle();
		states["AgeGate"] = new AgeGate();
	}

	public bool IsOnline()
	{
		return Soaring.IsOnline;
	}

	public void ClearUserState()
	{
		if (gameSaver != null)
		{
			gameSaver.Stop();
			gameSaver = null;
		}
		GameInitialized(false);
		if (game != null && game.store != null)
		{
			game.store.Reset(this);
		}
		if (game != null)
		{
			game.Clear();
			game = null;
		}
		UnityGameResources.Reset();
		SBUIBuilder.UpdateGuiEventHandler(this, delegate
		{
		});
		SBGUI.GetInstance().ResetWhiteList();
		Auth.ResetAuth();
		ClearAsyncRequests();
		player = null;
		soaringEvents = new SoaringArray();
	}

	public void SaveGame()
	{
		saveGame = true;
	}

	public void GameInitialized(bool initialized)
	{
		if (initialized)
		{
			notifyOnDisconnect = true;
			if (gameSaver == null)
			{
				gameSaver = new SBGamePersister(this);
				gameSaver.Start();
			}
			if (SBSettings.TrackStatistics)
			{
				if (statisticsTracker == null)
				{
					statisticsTracker = new GameObject("statisticsTracker");
					tracker = statisticsTracker.AddComponent<SBStatisticsTracker>();
					tracker.TheSession = this;
				}
				else
				{
					tracker.Paused = false;
				}
			}
		}
		else if (SBSettings.TrackStatistics && statisticsTracker != null)
		{
			tracker.Paused = true;
		}
		gameInitialized = initialized;
	}

	public void OnPause(bool paused)
	{
		ulong ulPauseTime = 0uL;
		if (paused)
		{
			m_ulPauseTimestamp = TFUtils.EpochTime();
		}
		else if (m_ulPauseTimestamp.HasValue)
		{
			ulPauseTime = TFUtils.EpochTime() - m_ulPauseTimestamp.Value;
			m_ulPauseTimestamp = null;
		}
		if (state == states["InAppPurchasing"])
		{
			TFUtils.DebugLog("Returning from OnPause as it was not a true backgrounding event");
		}
		else
		{
			if (game == null || !gameInitialized || InFriendsGame || WasInFriendsGame)
			{
				return;
			}
			if (paused && !SBSettings.UseActionFile)
			{
				game.SaveLocally(0uL, true);
			}
			if (gameSaver != null)
			{
				if (paused)
				{
					gameSaver.Stop();
				}
				else
				{
					gameSaver.Start();
					NotificationManager.CancelAllNotifications();
					NotificationManager.AddAnnoyingNotifications(TheGame);
					game.treasureManager.StartTreasureTimers();
					analytics.UpdateGameValues(game);
					PlayHavenController.RequestContent("app_resume");
					if (TheGame != null && TheGame.resourceManager != null)
					{
						SoaringDictionary soaringDictionary = new SoaringDictionary();
						soaringDictionary.addValue(TheGame.resourceManager.PlayerLevelAmount, "level");
						soaringDictionary.addValue(SBSettings.BundleVersion, "client_version");
						ulong num = TheGame.FirstPlayTime();
						if (num != 0L)
						{
							soaringDictionary.addValue(num, "first_play_time");
						}
						Soaring.FireEvent("ResumeGame", soaringDictionary);
					}
					AnalyticsWrapper.LogSessionBegin(game, ulPauseTime);
					game.analytics.LogSessionBegin();
				}
				HandleReset(false);
			}
			if (!paused && game.store != null && game.store.rmtEnabled && !game.store.receivedProductInfo)
			{
				if (Soaring.IsOnline)
				{
					TFUtils.DebugLog("Attempting to fetch products again");
					RmtStore.PreloadRmtProducts(this);
				}
			}
			else if (!paused && game.store != null)
			{
				TFUtils.DebugLog(string.Format("game.store: {0}; game.store.receivedProductInfo: {1}; game.store.premiumSupported: {2}", game.store, game.store.receivedProductInfo, game.store.rmtEnabled));
			}
			else if (!paused)
			{
				TFUtils.DebugLog("Unpaused while game.store is null");
			}
		}
	}

	public void PurchasePremiumProduct(string productIdentifier)
	{
		if (TheGame.store.rmtEnabled)
		{
			TheGame.store.OpenTransaction(productIdentifier);
			properties.iapBundleName = productIdentifier;
			ChangeState("InAppPurchasing");
		}
		else
		{
			TFUtils.DebugLog("Not purchasing premium item; the current application does not support premium purchases.");
			TFUtils.TriggerIAPDisabledWarning();
		}
	}

	public void StopGameSaveTimer()
	{
		if (gameSaver != null)
		{
			gameSaver.Stop();
		}
	}

	public void OnApplicationQuit()
	{
		if (game != null && !SBSettings.UseActionFile && !InFriendsGame && !WasInFriendsGame && !InFriendsGame && !WasInFriendsGame)
		{
			if (TFUtils.isFastForwarding)
			{
				game.FastForwardSimulationFinished();
			}
			game.SaveLocally(0uL, false, false, true);
		}
	}

	public void OnApplicationFocus(bool bFocus)
	{
		Debug.Log("OnApplicationFocus: " + bFocus);
		return; //Temp thing to try in editor
		if (game == null)
		{
			return;
		}
		if (gameInitialized && !bFocus && !SBSettings.UseActionFile)
		{
			if (!InFriendsGame && !WasInFriendsGame)
			{
				game.SaveLocally(0uL, false, false, true);
				HandleReset(false);
			}
		}
		else if (gameInitialized && bFocus && ((game.store != null && game.store.rmtEnabled && !Soaring.IsAuthorized) || game.store == null))
		{
			Debug.Log("Testing Online");
			if (!Soaring.IsOnline)
			{
				SoaringInternal.instance.ClearOfflineMode();
			}
			if (Soaring.IsOnline)
			{
				Debug.Log("Are authorized");
				game.SaveLocally(0uL, false, false, true);
				GameStarting.SplashScreenState splashScreenState = ((InFriendsGame || WasInFriendsGame) ? GameStarting.SplashScreenState.Patchy : GameStarting.SplashScreenState.Loading);
				GameStarting.ResetShowSplashScreen(splashScreenState);
				reinitializeSession = true;
			}
			else if (!resyncConnection)
			{
				ChangeState("Sync");
				resyncConnection = true;
			}
		}
	}

	public void HandleReset(bool forceReset)
	{
		if ((game == null && !gameInitialized) || InFriendsGame)
		{
			return;
		}
		if (!Soaring.IsOnline && SBSettings.RebootOnConnectionChange)
		{
			SoaringInternal.instance.ClearOfflineMode();
		}
		if (state == states["InAppPurchasing"] || RmtStore.IsPurchasing || !Soaring.IsOnline)
		{
			return;
		}
		DateTime utcNow = DateTime.UtcNow;
		if (!((DateTime.UtcNow - lastResetTime).TotalSeconds > 5.0))
		{
			return;
		}
		lastResetTime = utcNow;
		if (forceReset)
		{
			reinitializeSession = true;
		}
		if (SBSettings.RebootOnFocusChange)
		{
			GameStarting.SplashScreenState splashScreenState = ((InFriendsGame || WasInFriendsGame) ? GameStarting.SplashScreenState.Patchy : GameStarting.SplashScreenState.Loading);
			GameStarting.ResetShowSplashScreen(splashScreenState);
			reinitializeSession = true;
			return;
		}
		long num = player.ReadTimestamp();
		if (num <= 0)
		{
			reinitializeSession = true;
		}
		else
		{
			Soaring.RequestSessionData("game", num, Game.CreateSoaringGameResponderContext(OnTestContextResponder));
		}
	}

	public void OnTestContextResponder(SoaringContext context)
	{
		if (context != null)
		{
			SoaringArray soaringArray = (SoaringArray)context.objectWithKey("custom");
			if (soaringArray != null && soaringArray.count() != 0)
			{
				reinitializeSession = true;
			}
		}
	}

	public void CheckForPatching(bool checkForUpdates)
	{
		if (contentPatcher == null)
		{
			justCheckForUpdates = checkForUpdates;
			contentPatcher = new SBContentPatcher();
			contentPatcher.AddListener(OnPatchingEvent);
			checkForPatching = true;
		}
	}

	private void OnPatchingEvent(string eventStr)
	{
		switch (eventStr)
		{
		case "patchingNecessary":
			if (state != states["CheckPatching"] && game != null)
			{
				SoaringDebug.Log("Request Restart", LogType.Warning);
				HandleReset(true);
				contentPatcher = null;
			}
			break;
		case "patchingDone":
			if (SoaringInternal.instance.Versions != null)
			{
				string fileHash = SoaringInternal.instance.Versions.GetFileHash("Languages/EN_Sheet1.xml");
				Debug.LogError("HASH: " + fileHash);
				string text = PlayerPrefs.GetString("Languages/EN_Sheet1.xml", string.Empty);
				if (text != fileHash && !string.IsNullOrEmpty(fileHash))
				{
					PlayerPrefs.SetString("Languages/EN_Sheet1.xml", fileHash);
					PlayerPrefs.Save();
				}
				Language.ResetHasInitialized();
				Language.Init(TFUtils.GetPersistentAssetsPath());
				string[] array = new string[2] { "Sound/SoundEffects.json", "Sound/Music.json" };
				for (int i = 0; i < array.Length; i++)
				{
					fileHash = SoaringInternal.instance.Versions.GetFileHash(array[i]);
					text = PlayerPrefs.GetString(array[i], string.Empty);
					if (text != fileHash && !string.IsNullOrEmpty(fileHash))
					{
						PlayerPrefs.SetString(array[i], fileHash);
						PlayerPrefs.Save();
						if (!string.IsNullOrEmpty(text))
						{
							ChangeState("Authorizing");
							break;
						}
					}
				}
			}
			if (contentPatcher != null)
			{
				TFUtils.RefreshSAFiles();
			}
			contentPatcher = null;
			break;
		case "patchingNotNecessary":
			if (gameSaver != null)
			{
				SaveGame();
			}
			contentPatcher = null;
			break;
		}
	}

	public bool IsPatchingInProgress()
	{
		return contentPatcher != null;
	}

	public void ChangeState(string state, bool newContext = true)
	{
		if (!canChangeState || (state == "CommunityEvent" && game.communityEventManager.GetActiveEvent() == null))
		{
			return;
		}
		SoaringDebug.Log("Change State: " + state, LogType.Error);
		StateChangeRequest item = new StateChangeRequest
		{
			state = state,
			changeContext = newContext
		};
		lock (queuedStateChanges)
		{
			queuedStateChanges.Add(item);
		}
	}

	private void SetState(StateChangeRequest request)
	{
		if (!(request.state == "Shopping") || state == states["Shopping"])
		{
		}
		state.OnLeave(this);
		TFUtils.DebugLog("Session Change State: " + request.state + "\n(from: " + state.GetType().ToString() + ")");
		state = states[request.state];
		if (request.changeContext)
		{
			currentGuiContext = SBUIBuilder.PushNewScreenContext();
		}
		state.OnEnter(this);
		if (game != null)
		{
			game.OnChangeSessionState();
		}
	}

	public void ProcessStateChanges()
	{
		if (queuedStateChanges.Count > 0)
		{
			StateChangeRequest[] array = null;
			lock (queuedStateChanges)
			{
				array = queuedStateChanges.ToArray();
				queuedStateChanges.Clear();
			}
			StateChangeRequest[] array2 = array;
			foreach (StateChangeRequest stateChangeRequest in array2)
			{
				SetState(stateChangeRequest);
			}
		}
	}

	public void CheckForPatchingUpdate()
	{
		if (!checkForPatching)
		{
			return;
		}
		if (contentPatcher != null)
		{
			CheckPatching checkPatching = (CheckPatching)states["CheckPatching"];
			contentPatcher.AddListener(checkPatching.PatchingEventListener);
			GameStarting state = (GameStarting)states["GameStarting"];
			contentPatcher.AddListener(delegate(string eventName)
			{
				state.PatchingEventListener(eventName, this);
			});
			contentPatcher.LoadManifest(justCheckForUpdates);
		}
		checkForPatching = false;
	}

	public void OnUpdate()
	{
		try
		{
			if (saveGame && game != null)
			{
				TFUtils.DebugLog("Saving game onUpdate");
				game.SaveToServer(this, TFUtils.EpochTime(), false, false);
				saveGame = false;
			}
			framerateWatcher.OnUpdate();
			CheckForPatchingUpdate();
			if (currentGuiContext.next != simulationContext)
			{
				SBUIBuilder.ReleaseScreenContexts(currentGuiContext.next, simulationContext);
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			int count = actions.Count;
			for (int i = 0; i < count; i++)
			{
				if (actions[i] != null)
				{
					actions[i]();
				}
			}
			actions.Clear();
			if (reinitializeSession)
			{
				TFUtils.DebugLog("Reinitializing session and entering sync state");
				ChangeState("Sync");
				TFUtils.DebugLog("Reinitializing session " + reinitializeSession);
				return;
			}
			if (SBSettings.EnableShakeLogDump && Input.acceleration.magnitude > 2f && TFUtils.EpochTime() - logDumpShake > 30)
			{
				logDumpShake = TFUtils.EpochTime();
				TFUtils.DebugLog("Shake detected.  Dumping Log..");
				TFUtils.LogDump(this, "shake");
			}
			if (game != null && game.needsReloadErrorDialog && gameInitialized)
			{
				game.needsReloadErrorDialog = false;
				Action okAction = delegate
				{
					TheGame.RequestReload();
				};
				ErrorMessageHandler(this, Language.Get("!!ERROR_SAVE_GAME_FAILED_TITLE"), Language.Get("!!ERROR_RELOAD_GAME_MESSAGE"), Language.Get("!!PREFAB_OK"), okAction, 0.6f);
			}
			if (game != null && gameInitialized && canChangeState)
			{
				if ((object)state == "Shopping")
				{
					marketpalceActive = true;
				}
				else if ((object)state != "Shopping")
				{
					marketpalceActive = false;
				}
				bool isOnline = Soaring.IsOnline;
				if ((game.needsNetworkDownErrorDialog && notifyOnDisconnect) || (!isOnline && !isShowingOfflineDialog && lastOnlineState != isOnline))
				{
					notifyOnDisconnect = false;
					isShowingOfflineDialog = true;
					Action okAction2 = delegate
					{
						game.needsNetworkDownErrorDialog = false;
						isShowingOfflineDialog = false;
						ChangeState("Playing");
					};
					ErrorMessageHandler(this, Language.Get("!!ERROR_OFFLINE_MODE_TITLE"), Language.Get("!!NOTIFY_INGAME_OFFLINE_MODE"), Language.Get("!!PREFAB_OK"), okAction2, 0.75f);
				}
				lastOnlineState = isOnline;
				if (soaringEvents.count() != 0)
				{
					for (int num = 0; num < soaringEvents.count(); num++)
					{
						SoaringEvent soaringEvent = (SoaringEvent)soaringEvents.objectAtIndex(num);
						if (soaringEvent.HasDisplayBannerEvent() && soaringEvent.Requires != null)
						{
							bool flag = true;
							try
							{
								for (int num2 = 0; num2 < soaringEvent.Requires.Length; num2++)
								{
									SoaringEvent.SoaringEventRequirements soaringEventRequirements = soaringEvent.Requires[num2];
									if (soaringEventRequirements.Key == "level")
									{
										int result = 0;
										if (int.TryParse(soaringEventRequirements.Value, out result))
										{
											int playerLevelAmount = TheGame.resourceManager.PlayerLevelAmount;
											if ((soaringEventRequirements.Sign == SoaringEvent.Equivelency.equal && playerLevelAmount == result) || (soaringEventRequirements.Sign == SoaringEvent.Equivelency.greaterThen && playerLevelAmount > result) || (soaringEventRequirements.Sign == SoaringEvent.Equivelency.lessThen && playerLevelAmount < result) || (soaringEventRequirements.Sign == SoaringEvent.Equivelency.lessThenEquals && playerLevelAmount <= result) || (soaringEventRequirements.Sign == SoaringEvent.Equivelency.greaterThenEquals && playerLevelAmount >= result))
											{
												continue;
											}
										}
										flag = false;
									}
									else if (soaringEventRequirements.Key == "has_item" || soaringEventRequirements.Key == "has_no_item")
									{
										int result2 = 0;
										if (int.TryParse(soaringEventRequirements.Value, out result2))
										{
											int num3 = 0;
											List<Simulated> list = TheGame.simulation.FindAllSimulateds(result2);
											foreach (Simulated item in list)
											{
												if ((item.Entity.AllTypes & EntityType.BUILDING) != EntityType.INVALID)
												{
													num3++;
												}
											}
											foreach (SBInventoryItem item2 in TheGame.inventory.GetItems())
											{
												if (item2.entity.DefinitionId == result2)
												{
													num3++;
												}
											}
											if (soaringEventRequirements.Key == "has_item")
											{
												SoaringValue soaringValue = null;
												if (soaringEventRequirements.Custom != null)
												{
													soaringEventRequirements.Custom.soaringValue("count");
												}
												if (soaringValue != null)
												{
													int num4 = soaringValue;
													if (soaringEventRequirements.Key == "has_item" && ((soaringEventRequirements.Sign == SoaringEvent.Equivelency.equal && num3 == num4) || (soaringEventRequirements.Sign == SoaringEvent.Equivelency.greaterThen && num3 > num4) || (soaringEventRequirements.Sign == SoaringEvent.Equivelency.lessThen && num3 < num4) || (soaringEventRequirements.Sign == SoaringEvent.Equivelency.lessThenEquals && num3 <= num4) || (soaringEventRequirements.Sign == SoaringEvent.Equivelency.greaterThenEquals && num3 >= num4)))
													{
														continue;
													}
												}
												else if (num3 > 0)
												{
													continue;
												}
											}
											else if (soaringEventRequirements.Key == "has_no_item" && num3 == 0)
											{
												continue;
											}
										}
										flag = false;
									}
									else if (soaringEventRequirements.Key == "client_version" && !(SBSettings.BundleVersion == soaringEventRequirements.Value))
									{
										flag = false;
									}
								}
							}
							catch (Exception ex)
							{
								TFUtils.LogDump(this, "Event_Error_Banners", ex);
								SoaringDebug.Log(ex.Message, LogType.Error);
							}
							if (flag)
							{
								soaringEvent.Requires = null;
								SoaringInternal.instance.Events.AddBannerEvent(soaringEvent);
							}
							continue;
						}
						int num5 = soaringEvent.Actions.Length;
						for (int num6 = 0; num6 < num5; num6++)
						{
							if (soaringEvent.Actions[num6].Type == SoaringEvent.SoaringEventActionType.Custom)
							{
								if (soaringEvent.Actions[num6].Key == "spongy_games_banner")
								{
									Dictionary<string, object> dictionary = new Dictionary<string, object>();
									List<DialogInputData> list2 = new List<DialogInputData>(1);
									SoaringDictionary custom = soaringEvent.Actions[num6].Custom;
									SoaringArray soaringArray = (SoaringArray)custom.objectWithKey("characters");
									int num7 = soaringArray.count();
									List<int> list3 = new List<int>(num7);
									for (int num8 = 0; num8 < num7; num8++)
									{
										list3.Add((SoaringValue)soaringArray.objectAtIndex(num8));
									}
									dictionary.Add("characters", list3);
									dictionary.Add("event_name", (string)(SoaringValue)custom.objectWithKey("event_name"));
									dictionary.Add("event_days", (int)(SoaringValue)custom.objectWithKey("event_days"));
									dictionary.Add("day", (int)(SoaringValue)custom.objectWithKey("day"));
									dictionary.Add("title", (string)(SoaringValue)custom.objectWithKey("title"));
									dictionary.Add("description_one", (string)(SoaringValue)custom.objectWithKey("description_one"));
									dictionary.Add("description_two", (string)(SoaringValue)custom.objectWithKey("description_two"));
									dictionary.Add("event_portrait", (string)(SoaringValue)custom.objectWithKey("event_portrait"));
									list2.Add(new SpongyGamesDialogInputData(dictionary));
									if ((int)dictionary["day"] == (int)dictionary["event_days"])
									{
										CommunityEventManager._pEventStatusDialogData = null;
										int num9 = 20410;
										if (TheGame.simulation.FindSimulated(num9) == null && !TheGame.inventory.HasItem(num9))
										{
											string arg = string.Empty;
											Blueprint blueprint;
											if (list3.Count > 0)
											{
												blueprint = EntityManager.GetBlueprint(EntityType.RESIDENT, list3[0], true);
												if (blueprint != null)
												{
													arg = Language.Get((string)blueprint.Invariable["name"]);
												}
											}
											list2.Add(new CharacterDialogInputData(uint.MaxValue, new Dictionary<string, object>
											{
												{ "character_icon", "Portrait_Squilliam.png" },
												{
													"text",
													string.Format(Language.Get("!!QUEST_ID2304_TEXT1"), arg)
												}
											}));
											list2.Add(new CharacterDialogInputData(uint.MaxValue, new Dictionary<string, object>
											{
												{ "character_icon", "Squidward_Annoyed.png" },
												{ "text", "!!QUEST_ID2304_TEXT2" }
											}));
											list2.Add(new CharacterDialogInputData(uint.MaxValue, new Dictionary<string, object>
											{
												{ "character_icon", "Portrait_Squilliam.png" },
												{ "text", "!!QUEST_ID2304_TEXT3" }
											}));
											blueprint = EntityManager.GetBlueprint(EntityType.BUILDING, 20410, true);
											list2.Add(new FoundItemDialogInputData(Language.Get("!!RECIPE_UNLOCKED_TITLE"), string.Format(Language.Get("!!RECIPE_UNLOCKED_DIALOG"), Language.Get((string)blueprint.Invariable["name"])), (string)blueprint.Invariable["portrait"], "Beat_FoundRecipe"));
											TheGame.dialogPackageManager.AddDialogInputBatch(TheGame, list2);
											TheGame.questManager.AddDialogSequences(TheGame, 1u, 2305u, new List<Reward>(), 0u);
											Reward reward = new Reward(null, new Dictionary<int, int> { { num9, 1 } }, null, null, null, null, null, null, false, null);
											TheGame.ApplyReward(reward, TFUtils.EpochTime(), false);
											TheGame.ModifyGameState(new ReceiveRewardAction(reward, string.Empty));
										}
										else
										{
											TheGame.dialogPackageManager.AddDialogInputBatch(TheGame, list2);
										}
									}
									else
									{
										CommunityEventManager._pEventStatusDialogData = dictionary;
										TheGame.dialogPackageManager.AddDialogInputBatch(TheGame, list2);
									}
									if (TheState != states["CommunityEvent"])
									{
										ChangeState("ShowingDialog");
									}
									else
									{
										AddAsyncResponse("dialogs_to_show", true);
									}
								}
							}
							else if (soaringEvent.Actions[num6].Type == SoaringEvent.SoaringEventActionType.DisplayBanner)
							{
								continue;
							}
							try
							{
								GiveSoaringReward(soaringEvent.Actions[num6]);
							}
							catch (Exception ex2)
							{
								TFUtils.LogDump(this, "Reward Exception", ex2);
							}
						}
					}
					soaringEvents.clear();
				}
			}
			if (game != null && game.PendingReload())
			{
				ChangeState("Sync");
				game.SetPendingReload(false);
				return;
			}
			state.OnUpdate(this);
			if (game != null && game.simulation != null)
			{
				game.sessionActionManager.OnUpdate(game);
			}
			camera.OnUpdate(realtimeSinceStartup - lastUpdateTime, this);
			lastUpdateTime = realtimeSinceStartup;
			callbackQueue.ProcessQueue();
		}
		catch (Exception ex3)
		{
			TFUtils.LogDump(this, "main_loop_error", ex3);
			throw;
		}
	}

	public void GiveSoaringReward(SoaringEvent.SoaringEventAction reward)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
		Dictionary<string, string> dictionary6 = new Dictionary<string, string>();
		dictionary6["hard_currency"] = ResourceManager.HARD_CURRENCY.ToString();
		dictionary6["soft_currency"] = ResourceManager.SOFT_CURRENCY.ToString();
		dictionary6["xp"] = ResourceManager.XP.ToString();
		string text = reward.Key;
		if (dictionary6.ContainsKey(text))
		{
			string key = dictionary6[text];
			dictionary[key] = reward.Quantity;
		}
		else
		{
			text = reward.Value;
		}
		string text2 = string.Empty;
		string text3 = "costume_";
		int result = -1;
		string text4 = string.Empty;
		string text5 = "movie_";
		int result2 = -1;
		if (text.Contains(text3))
		{
			if (!int.TryParse(text.Substring(text3.Length), out result))
			{
				result = -1;
			}
			if (reward.Custom.containsKey("unlock_type"))
			{
				text2 = (SoaringValue)reward.Custom["unlock_type"];
			}
			if (string.IsNullOrEmpty(text2))
			{
				result = -1;
			}
		}
		if (text.Contains(text5))
		{
			if (!int.TryParse(text.Substring(text5.Length), out result2))
			{
				result2 = -1;
			}
			if (reward.Custom.containsKey("unlock_type"))
			{
				text4 = (SoaringValue)reward.Custom["unlock_type"];
			}
			if (string.IsNullOrEmpty(text4))
			{
				result2 = -1;
			}
		}
		PopulateRewardDict("building_", dictionary2, text, reward.Quantity);
		PopulateRewardDict("movie_", dictionary4, text, reward.Quantity);
		PopulateRewardDict("recipe_", dictionary3, text, reward.Quantity);
		if (result != -1 && text2.Equals("add"))
		{
			PopulateRewardDict("costume_", dictionary5, text, reward.Quantity);
		}
		if (result2 != -1 && text4.Equals("add"))
		{
			PopulateRewardDict("movie_", dictionary4, text, reward.Quantity);
		}
		Dictionary<string, object> dictionary7 = new Dictionary<string, object>();
		dictionary7["resources"] = dictionary;
		dictionary7["recipes"] = dictionary3;
		dictionary7["movies"] = dictionary4;
		dictionary7["buildings"] = dictionary2;
		if (result != -1 && text2.Equals("add"))
		{
			dictionary7["costumes"] = dictionary5;
		}
		if (result2 != -1 && text4.Equals("add"))
		{
			dictionary7["movies"] = dictionary4;
		}
		if (TheGame == null)
		{
			return;
		}
		RewardDefinition rewardDefinition = RewardDefinition.FromDict(dictionary7);
		Reward reward2 = rewardDefinition.GenerateReward(TheGame.simulation, false);
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, int> buildingAmount in reward2.BuildingAmounts)
		{
			int key2 = buildingAmount.Key;
			Blueprint blueprint = EntityManager.GetBlueprint("building", key2, true);
			int? num = null;
			if (blueprint != null)
			{
				num = blueprint.GetInstanceLimitByLevel(TheGame.resourceManager.PlayerLevelAmount);
			}
			if (!num.HasValue)
			{
				continue;
			}
			int num2 = 0;
			List<Simulated> list2 = TheGame.simulation.FindAllSimulateds(key2);
			foreach (Simulated item in list2)
			{
				if ((item.Entity.AllTypes & EntityType.BUILDING) != EntityType.INVALID)
				{
					num2++;
				}
			}
			foreach (SBInventoryItem item2 in TheGame.inventory.GetItems())
			{
				if (item2.entity.DefinitionId == key2)
				{
					num2++;
				}
			}
			if (num2 > num.Value)
			{
				Debug.LogError("Cannot add another instance of this building since instance limit of " + num.Value + " has been reached!");
				list.Add(buildingAmount.Key);
			}
		}
		foreach (int item3 in list)
		{
			if (reward2.BuildingAmounts.ContainsKey(item3))
			{
				reward2.BuildingAmounts[item3] = 0;
			}
		}
		if (result != -1)
		{
			if (text2.Equals("remove") && TheGame.costumeManager.IsCostumeUnlocked(result))
			{
				TheGame.costumeManager.RemoveCostume(result);
			}
			if (text2.Equals("unlock_store"))
			{
				TheGame.costumeManager.UnLockCostumeInStore(result);
			}
			if (text2.Equals("lock_store"))
			{
				TheGame.costumeManager.LockCostumeInStore(result);
			}
		}
		if (result2 != -1)
		{
			TheGame.movieManager.UnlockMovie(result2);
		}
		TheGame.ApplyReward(reward2, TFUtils.EpochTime(), false);
		TheGame.ModifyGameState(new ReceiveRewardAction(reward2, text));
	}

	private void PopulateRewardDict(string prefix, Dictionary<string, object> dict, string rewardName, int quantity)
	{
		if (rewardName.StartsWith(prefix))
		{
			string key = rewardName.Substring(prefix.Length);
			dict[key] = quantity;
		}
	}

	public void SetupPlayer(SoaringContext context)
	{
		if (context == null)
		{
			TFUtils.ErrorLog("TFUtils: SetupPlayer: Error: No Valid Context");
			return;
		}
		SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = (SoaringPlayerResolver.SoaringPlayerData)context.objectWithKey("user_data");
		ThePlayer = Player.LoadFromSoaringID(soaringPlayerData.userID);
		if (ThePlayer != null)
		{
			TFUtils.DebugLog(string.Format("The player is logged in with playerId {0}", ThePlayer.playerId));
			WebFileServer.SetPlayerInfo(ThePlayer);
			analytics.PlayerId = ThePlayer.playerId;
		}
	}

	public void AddAction(GameloopAction action)
	{
		actions.Add(action);
	}

	public int GetLocalVersion()
	{
		return currentVersion;
	}

	public void DropGame()
	{
		if (game != null)
		{
			game = null;
		}
	}

	public bool PlayerIsLoggedIn()
	{
		return player != null;
	}

	public void onExternalMessage(string msg)
	{
		TFUtils.DebugLog("decoding message: " + msg);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(msg);
		string text = dictionary["requestId"] as string;
		if (externalRequests.ContainsKey(text))
		{
			TFServer.JsonResponseHandler jsonResponseHandler = externalRequests[text];
			if (dictionary["data"] is Dictionary<string, object>)
			{
				jsonResponseHandler(dictionary["data"] as Dictionary<string, object>, null);
			}
			else
			{
				TFUtils.ErrorLog("Callback result is not a Dictionary<string, object>");
			}
		}
		else
		{
			TFUtils.DebugLog("No handler found for id: " + text);
		}
	}

	public void RegisterExternalCallback(string requestId, TFServer.JsonResponseHandler callback)
	{
		TFUtils.DebugLog("Registering external callback for " + requestId);
		if (externalRequests.ContainsKey(requestId))
		{
			TFUtils.ErrorLog("Got duplicate registration for " + requestId + "; Clobbering existing callback");
		}
		externalRequests[requestId] = callback;
	}

	public void unregisterExternalCallback(string requestId, TFServer.JsonResponseHandler callback)
	{
		TFUtils.DebugLog("Unregistering external callback for " + requestId);
		externalRequests.Remove(requestId);
	}

	public AndroidJavaObject getAndroidActivity()
	{
		if (androidActivity == null)
		{
			int num = AndroidJNI.AttachCurrentThread();
			TFUtils.DebugLog("attach result: " + num);
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			androidActivity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
		return androidActivity;
	}

	protected void CheckInventorySoftLock()
	{
		if (!game.featureManager.CheckFeature("inventory_soft"))
		{
			game.featureManager.UnlockFeature("inventory_soft");
			game.featureManager.ActivateFeatureActions(game, "inventory_soft");
			game.simulation.ModifyGameState(new FeatureUnlocksAction(new List<string> { "inventory_soft" }));
		}
	}

	public void AddAsyncResponse(string key, object val)
	{
		AddAsyncResponse(key, val, true);
	}

	public void AddAsyncResponse(string key, object val, bool warnIfDuplicate)
	{
		lock (asyncRequests)
		{
			if (warnIfDuplicate && asyncRequests.ContainsKey(key))
			{
				TFUtils.DebugLog("Warning: got second async response for " + key + "; Existing value was: " + asyncRequests[key]);
			}
			asyncRequests[key] = val;
		}
	}

	public object CheckAsyncRequest(string key)
	{
		object result = null;
		lock (asyncRequests)
		{
			if (asyncRequests.ContainsKey(key))
			{
				result = asyncRequests[key];
				asyncRequests.Remove(key);
			}
		}
		return result;
	}

	public TFServer.JsonResponseHandler AsyncResponder(string key)
	{
		return delegate(Dictionary<string, object> response, object userData)
		{
			AddAsyncResponse(key, response);
		};
	}

	public void AddAsyncFileResponse(string key, TFWebClient val)
	{
		lock (asyncFileRequests)
		{
			asyncFileRequests[key] = val;
		}
	}

	public TFWebClient CheckAsyncFileRequest(string key)
	{
		TFWebClient result = null;
		lock (asyncFileRequests)
		{
			if (asyncFileRequests.ContainsKey(key))
			{
				result = asyncFileRequests[key];
				asyncFileRequests.Remove(key);
			}
		}
		return result;
	}

	public TFWebClient.GetCallbackHandler AsyncFileResponder(string key)
	{
		return delegate(TFWebClient client)
		{
			AddAsyncFileResponse(key, client);
		};
	}

	public void ClearAsyncRequests()
	{
		asyncRequests = new Dictionary<string, object>();
		asyncFileRequests = new Dictionary<string, TFWebClient>();
	}

	public void PlayBubbleScreenSwipeEffect()
	{
		TheSoundEffectManager.PlaySound("BubbleWipe");
		game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Screen_Wipe", 0, 0, 0f, bubbleSwipeParticleSystemRequestDelegate);
	}

	public void PlayConfettiScreenSwipeEffect()
	{
		TheSoundEffectManager.PlaySound("ConfettiPop");
		game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Confetti_Squares", 0, 0, 0f, confettiSwipeParticleSystemRequestDelegate);
		game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Confetti_Squiggles", 0, 0, 0f, confettiSwipeParticleSystemRequestDelegate);
		game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Confetti_Balloons_01", 0, 0, 0f, balloonSwipeParticleSystemRequestDelegate);
		game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Confetti_Balloons_02", 0, 0, 0f, balloonSwipeParticleSystemRequestDelegate);
	}

	public void PlaySeaflowerAndBubbleScreenSwipeEffect()
	{
		TheSoundEffectManager.PlaySound("SeaflowerWipe");
		game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Seaflowers_Quest_Complete", 0, 0, 0f, seaFlowerSwipeParticleSystemRequestDelegate);
		game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Quest_Complete", 0, 0, 0f, seaFlowerSwipeParticleSystemRequestDelegate);
	}

	public void PlayTapParticleEffect(Vector3 position)
	{
		tapFXParticleSystemRequestDelegate.Position = position;
		game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Click", 0, 0, 0f, tapFXParticleSystemRequestDelegate);
	}

	public void PlayFogParticleEffects()
	{
		game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Fog1_Drift", 0, 0, 40f, fogEffectRequestDelegate);
		game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Fog2_Drift", 0, 0, 40f, fogEffectRequestDelegate);
		game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Fog3_Drift", 0, 0, 40f, fogEffectRequestDelegate);
		game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Fog4_Drift", 0, 0, 40f, fogEffectRequestDelegate);
		game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Fog5_Drift", 0, 0, 40f, fogEffectRequestDelegate);
	}

	private void InitScreenSwipeEffects()
	{
		bubbleSwipeParticleSystemRequestDelegate = new BubbleSwipeParticleSystemRequestDelegate(this);
		confettiSwipeParticleSystemRequestDelegate = new ConfettiSwipeParticleSystemRequestDelegate(this);
		balloonSwipeParticleSystemRequestDelegate = new BalloonSwipeParticleSystemRequestDelegate(this);
		seaFlowerSwipeParticleSystemRequestDelegate = new SeaflowerSwipeParticleSystemRequestDelegate(this);
		tapFXParticleSystemRequestDelegate = new TapFXParticleSystemRequestDelegate(this);
		fogEffectRequestDelegate = new FogEffectRequestDelegate(this);
	}

	public void ErrorMessageHandler(Session session, string title, string message, string okButtonLabel, Action okAction, float messageScale = 1f)
	{
		session.AddAsyncResponse("error_message_title", title);
		session.AddAsyncResponse("error_message", message);
		session.AddAsyncResponse("error_message_ok_label", okButtonLabel);
		session.AddAsyncResponse("error_message_ok_action", okAction);
		session.AddAsyncResponse("error_message_scale", messageScale);
		session.ChangeState("ErrorDialog");
	}

	public void GetJellyHandler(Session session, string title, string message, string question, string okButtonLabel, string cancelButtonLabel, Action okAction, Action cancelAction)
	{
		session.AddAsyncResponse("jelly_message_title", title);
		session.AddAsyncResponse("jelly_message", message);
		session.AddAsyncResponse("jelly_question", question);
		session.AddAsyncResponse("jelly_message_ok_label", okButtonLabel);
		session.AddAsyncResponse("jelly_message_cancel_label", cancelButtonLabel);
		session.AddAsyncResponse("jelly_message_ok_action", okAction);
		session.AddAsyncResponse("jelly_message_cancel_action", cancelAction);
		session.ChangeState("GetJelly");
	}

	public void InsufficientResourcesHandler(Session session, string itemName, int itemDID, Action okAction, Action cancelAction, Cost insufficientCost)
	{
		session.AddAsyncResponse("insufficient_item_did", itemDID);
		session.AddAsyncResponse("insufficient_itemname", itemName);
		session.AddAsyncResponse("insufficient_resources", insufficientCost);
		session.AddAsyncResponse("insufficient_cancel", cancelAction);
		session.AddAsyncResponse("insufficient_accept", okAction);
		session.ChangeState("InsufficientDialog");
	}

	public static Simulated FindBestSimulatedUnderPoint(Prioritizer prioritizer, Simulation simulation, SBCamera camera, Vector2 screenPos, out Ray rayCast)
	{
		return FindBestSimulatedUnderPoint(prioritizer, null, simulation, camera, screenPos, out rayCast);
	}

	public static Simulated FindBestSimulatedUnderPoint(Prioritizer prioritizer, Predicate<Simulated> filterOutMatching, Simulation simulation, SBCamera camera, Vector2 screenPos, out Ray rayCast)
	{
		List<Simulated> list = FindSimulatedsUnderPoint(filterOutMatching, simulation, camera, screenPos, out rayCast);
		list.ForEach(prioritizer.SelectBest);
		return prioritizer.Best;
	}

	public static List<Simulated> FindSimulatedsUnderPoint(Predicate<Simulated> filterOutMatching, Simulation simulation, SBCamera camera, Vector2 screenPos, out Ray rayCast)
	{
		rayCast = camera.ScreenPointToRay(screenPos);
		List<Simulated> result = new List<Simulated>();
		simulation.Scene.Find(rayCast, ref result);
		simulation.WhitelistSimulateds(ref result);
		if (filterOutMatching != null)
		{
			result.RemoveAll(filterOutMatching);
		}
		return result;
	}

	public static Simulated FindAlreadySelected(Predicate<Simulated> filterOutMatching, Simulation simulation, SBCamera camera, Vector2 screenPos, out Ray rayCast, Simulated selected)
	{
		if (selected == null)
		{
			rayCast = camera.ScreenPointToRay(screenPos);
			return null;
		}
		List<Simulated> list = FindSimulatedsUnderPoint(filterOutMatching, simulation, camera, screenPos, out rayCast);
		foreach (Simulated item in list)
		{
			if (item == selected)
			{
				return item;
			}
		}
		return null;
	}

	private static void ChangeToResolveSessionStateOnStartup(Session session)
	{
		string nextSession = "resolve_user";
		if (session.ThePlayer == null)
		{
			session.ChangeState(nextSession);
		}
		else if (!Game.GameExists(session.ThePlayer))
		{
			session.PlayMovie("Video/1_intro.m4v", nextSession);
		}
		else
		{
			session.ChangeState(nextSession);
		}
	}

	private static void RegisterForLocalNotifications()
	{
	}

	protected void PlayMovie(string movie, string nextSession)
	{
		Movie movie2 = (Movie)states["Movie"];
		movie2.TheMovie = movie;
		movie2.NextSessionState = nextSession;
		ChangeState("Movie");
	}

	public void PlayMovieFromInventory(string movie)
	{
		PlayMovie(movie, "Inventory");
	}

	public void PlayMovieFromPlaying(string movie)
	{
		PlayMovie(movie, "Playing");
	}

	public void PlayMovieFromShowingDialog(string movie)
	{
		PlayMovie(movie, "ShowingDialog");
	}

	public static void TryGrabSimulated(Session session, SBGUIEvent evt)
	{
		List<Simulated> result = new List<Simulated>();
		session.game.simulation.Scene.Find(session.camera.ScreenPointToRay(evt.position), ref result);
		TryGrabSimulated(session, result, evt);
	}

	public static bool TryGrabSimulated(Session session, List<Simulated> candidateSimulateds, SBGUIEvent evt)
	{
		if (candidateSimulateds.Count != 0)
		{
			return TryGrabSimulated(session, candidateSimulateds[0], evt);
		}
		return false;
	}

	public static bool TryGrabSimulated(Session session, Simulated candidateSimulated, SBGUIEvent evt)
	{
		if (candidateSimulated.InteractionState.IsGrabbable && !session.game.simulation.Whitelisted)
		{
			session.AddAsyncResponse("CurrentGuiEventInfo", evt);
			session.game.selected = candidateSimulated;
			PushForPlacementHelper.PushPlacementConfirmation(session, candidateSimulated);
			return true;
		}
		return false;
	}
}
