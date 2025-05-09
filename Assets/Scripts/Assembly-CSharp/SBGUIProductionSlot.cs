#define ASSERTS_ON
public class SBGUIProductionSlot : SBGUIElement
{
	public SBGUILabel label;

	public SBGUIAtlasImage icon;

	public SBGUIAtlasImage background;

	public SBGUIAtlasButton rushButton;

	public SBGUILabel rushCostLabel;

	public new static SBGUIProductionSlot Create()
	{
		SBGUIProductionSlot sBGUIProductionSlot = (SBGUIProductionSlot)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/CraftProductionSlot");
		sBGUIProductionSlot.gameObject.name = "ProductionSlot";
		return sBGUIProductionSlot;
	}

	protected override void Awake()
	{
		label = FindChild("time_label").GetComponent<SBGUILabel>();
		icon = FindChild("icon").GetComponent<SBGUIAtlasImage>();
		background = FindChild("background").GetComponent<SBGUIAtlasImage>();
		rushButton = FindChild("rush_button").GetComponent<SBGUIAtlasButton>();
		rushCostLabel = FindChild("rush_cost_label").GetComponent<SBGUILabel>();
		TFUtils.Assert(label != null, "Could not find child label on production slot!");
		TFUtils.Assert(icon != null, "Could not find child icon on production slot!");
		TFUtils.Assert(rushButton != null, "Could not find child rush button on production slot!");
		TFUtils.Assert(rushCostLabel != null, "Could not find rush cost label on production slot!");
		label.Text = string.Empty;
		icon.SetTextureFromAtlas("empty.png");
		background.SetVisible(false);
	}

	public void Deactivate()
	{
		MuteButtons(false);
		SetParent(null);
		SetActive(false);
		ClearButtonActions(rushButton.name);
	}
}
