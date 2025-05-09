#define ASSERTS_ON
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

	private List<SBGUIElement> targets = new List<SBGUIElement>();

	public string Target
	{
		get
		{
			return target;
		}
	}

	public string DynamicSubTarget
	{
		get
		{
			return dynamicSubTarget;
		}
	}

	public string DynamicScrolledSubTarget
	{
		get
		{
			return dynamicScrolledSubTarget;
		}
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		Parse(data, id, startConditions, originatedFromQuest, null);
	}

	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest, string target)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0u), originatedFromQuest);
		string text = (this.target = TFUtils.TryLoadString(data, "target"));
		if (target == null && text != null)
		{
			this.target = text;
		}
		else if (target != null && text == null)
		{
			this.target = target;
		}
		else if (target != null && text != null)
		{
			TFUtils.ErrorLog("UiTargetingSessionActionDefinition had both a procedural target and a loaded target. Must be one or the other!\ndefinition=" + this);
		}
		else if (target == null && text == null)
		{
			TFUtils.ErrorLog("UiTargetingSessionActionDefinition had neither a procedural target nor a loaded target. Must be one or the other!\ndefinition=" + this);
		}
		dynamicSubTarget = TFUtils.TryLoadString(data, "dynamic_subtarget");
		dynamicScrolledSubTarget = TFUtils.TryLoadString(data, "dynamic_scrolled_subtarget");
		TFUtils.Assert(dynamicSubTarget == null || dynamicScrolledSubTarget == null, "Cannot have both a dynamic sub target and a dynamic scrolled sub target. There can be only 1.");
		if (data.ContainsKey("restrict_clicks"))
		{
			restrict = (bool)data["restrict_clicks"];
		}
	}

	public virtual void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
		if (restrict)
		{
			targets.Add(target);
			RestrictInteraction.AddWhitelistElement(target);
			RestrictInteraction.AddWhitelistSimulated(session.TheGame.simulation, int.MinValue);
			RestrictInteraction.AddWhitelistExpansion(session.TheGame.simulation, int.MinValue);
		}
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["target"] = target;
		dictionary["dynamic_subtarget"] = dynamicSubTarget;
		dictionary["dynamic_scrolled_subtarget"] = dynamicScrolledSubTarget;
		return dictionary;
	}

	public override string ToString()
	{
		return base.ToString() + "UiInteractingSessionActionDefinition:(target=" + target + ", subTarget=" + dynamicSubTarget + ", scrolledSubTarget=" + dynamicScrolledSubTarget + ", restrict_clicks=" + restrict + ")";
	}

	public override void OnDestroy(Game game)
	{
		if (targets.Count > 0)
		{
			RestrictInteraction.RemoveWhitelistSimulated(game.simulation, int.MinValue);
			RestrictInteraction.RemoveWhitelistExpansion(game.simulation, int.MinValue);
		}
		foreach (SBGUIElement target in targets)
		{
			if (target != null)
			{
				RestrictInteraction.RemoveWhitelistElement(target);
			}
		}
		targets.Clear();
	}
}
