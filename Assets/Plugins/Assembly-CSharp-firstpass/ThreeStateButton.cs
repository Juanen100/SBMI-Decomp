using Yarg;

public class ThreeStateButton : BaseButton
{
	private enum BUTTON_STATE
	{
		IDLE = 0,
		HOVER = 1,
		ACTIVATE = 2
	}

	public int atlasIndex;

	public SpriteCoordinates idle;

	public SpriteCoordinates hover;

	public SpriteCoordinates activate;

	private BUTTON_STATE buttonState;

	protected override bool NeedsLoad
	{
		get
		{
			return false;
		}
	}

	public override void Load()
	{
	}

	protected override bool TouchEventHandler(YGEvent evt)
	{
		return false;
	}
}
