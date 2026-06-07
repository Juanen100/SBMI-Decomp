using System.Collections.Generic;

public abstract class UiTargetingSessionActionDefinition : SessionActionDefinition
{
	protected const string TARGET = "target";

	private const string DYNAMIC_SUBTARGET = "dynamic_subtarget";

	private const string DYNAMIC_SCROLLED_SUBTARGET = "dynamic_scrolled_subtarget";

	private string target;

	private string dynamicSubTarget;

	private string dynamicScrolledSubTarget;

	private bool restrict;

	private List<SBGUIElement> targets;

	public string Target
	{
		get
		{
			return null;
		}
	}

	public string DynamicSubTarget
	{
		get
		{
			return null;
		}
	}

	public string DynamicScrolledSubTarget
	{
		get
		{
			return null;
		}
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest, string target)
	{
	}

	public virtual void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}

	public override void OnDestroy(Game game)
	{
	}
}
