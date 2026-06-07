public class SBGUIProductionSlot : SBGUIElement
{
	public SBGUILabel label;

	public SBGUIAtlasImage icon;

	public SBGUIAtlasImage background;

	public SBGUIAtlasButton rushButton;

	public SBGUIAtlasButton watchADButton;

	public SBGUILabel rushCostLabel;

	public new static SBGUIProductionSlot Create()
	{
		return null;
	}

	protected override void Awake()
	{
	}

	public void Deactivate()
	{
	}
}
