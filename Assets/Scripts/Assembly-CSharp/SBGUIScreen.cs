using System.Collections.Generic;

public class SBGUIScreen : SBGUIElement
{
	public Dictionary<string, SBGUILabel> dynamicLabels;

	public Dictionary<string, SBGUIProgressMeter> dynamicMeters;

	public Dictionary<string, object> dynamicProperties;

	public EventDispatcher<SBGUIScreen, Session> UpdateCallback;

	public EventDispatcher OnPutIntoCache;

	public Session session;

	protected List<SBGUIScreen> modalDialogs;

	private bool usedInSessionAction;

	public virtual bool UsedInSessionAction
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	protected override void Awake()
	{
	}

	public override void AttachAnalyticsToButton(string buttonName, SBGUIButton button)
	{
	}

	public static SBGUIScreen Create(SBGUIElement parent, Session session)
	{
		return null;
	}

	public virtual void Close()
	{
	}

	public virtual void Deactivate()
	{
	}

	private void Initialize(SBGUIElement parent, Session session)
	{
	}

	public virtual void Update()
	{
	}
}
