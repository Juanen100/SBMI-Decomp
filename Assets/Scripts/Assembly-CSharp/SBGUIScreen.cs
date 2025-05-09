using System.Collections.Generic;
using UnityEngine;

public class SBGUIScreen : SBGUIElement
{
	public Dictionary<string, SBGUILabel> dynamicLabels;

	public Dictionary<string, SBGUIProgressMeter> dynamicMeters;

	public Dictionary<string, object> dynamicProperties;

	public EventDispatcher<SBGUIScreen, Session> UpdateCallback = new EventDispatcher<SBGUIScreen, Session>();

	public EventDispatcher OnPutIntoCache = new EventDispatcher();

	public Session session;

	protected List<SBGUIScreen> modalDialogs = new List<SBGUIScreen>();

	private bool usedInSessionAction;

	public virtual bool UsedInSessionAction
	{
		get
		{
			return usedInSessionAction;
		}
		set
		{
			usedInSessionAction = value;
		}
	}

	protected override void Awake()
	{
		SetParent(null);
		Vector3 localPosition = base.tform.localPosition;
		localPosition.x = 0f;
		localPosition.y = 0f;
		base.tform.localPosition = localPosition;
		dynamicLabels = new Dictionary<string, SBGUILabel>();
		dynamicMeters = new Dictionary<string, SBGUIProgressMeter>();
		dynamicProperties = new Dictionary<string, object>();
		SBGUIButton sBGUIButton = (SBGUIButton)FindChild("touch_mask");
		if (sBGUIButton != null)
		{
			SBGUIButton sBGUIButton2 = (SBGUIButton)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/TouchableBackground");
			sBGUIButton2.name = "TouchableBackground";
			sBGUIButton2.SetParent(this);
			float num = GUIMainView.GetInstance().GetComponent<Camera>().farClipPlane + 5f;
			sBGUIButton2.tform.localPosition = new Vector3(0f, 0f, num);
			sBGUIButton.tform.localPosition = new Vector3(sBGUIButton.tform.localPosition.x, sBGUIButton.tform.localPosition.y, num - 0.1f);
		}
		base.Awake();
	}

	public override void AttachAnalyticsToButton(string buttonName, SBGUIButton button)
	{
		StartTimer();
		button.ClickEvent += delegate
		{
			int playerLevel = -1;
			if (session != null)
			{
				if (session.TheGame != null)
				{
					playerLevel = session.TheGame.resourceManager.Resources[ResourceManager.LEVEL].Amount;
				}
				if (session.analytics != null)
				{
					session.analytics.LogDialog(GetType().ToString(), buttonName, base.ElapsedTime, playerLevel);
				}
			}
		};
	}

	public static SBGUIScreen Create(SBGUIElement parent, Session session)
	{
		GameObject gameObject = new GameObject(string.Format("SBGUIScreen_{0}", SBGUIElement.InstanceID));
		SBGUIScreen sBGUIScreen = gameObject.AddComponent<SBGUIScreen>();
		sBGUIScreen.Initialize(parent, session);
		return sBGUIScreen;
	}

	public virtual void Close()
	{
		UsedInSessionAction = false;
		Deactivate();
		Object.Destroy(base.gameObject);
	}

	public virtual void Deactivate()
	{
		GUIMainView.GetInstance().Library.bShowingDialog = false;
		MuteButtons(false);
		if (base.gameObject.active)
		{
			base.gameObject.SetActiveRecursively(false);
		}
	}

	private void Initialize(SBGUIElement parent, Session session)
	{
		this.session = session;
		dynamicLabels = new Dictionary<string, SBGUILabel>();
		dynamicMeters = new Dictionary<string, SBGUIProgressMeter>();
		dynamicProperties = new Dictionary<string, object>();
		SetTransformParent(parent);
		base.transform.localPosition = Vector3.zero;
	}

	public virtual void Update()
	{
		UpdateCallback.FireEvent(this, session);
	}
}
