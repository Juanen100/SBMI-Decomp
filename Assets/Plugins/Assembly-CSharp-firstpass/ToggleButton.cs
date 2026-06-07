using Yarg;

public class ToggleButton : BaseButton
{
	public SpriteCoordinates enabledSprite;

	public SpriteCoordinates disabledSprite;

	private bool buttonEnabled;

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

	public void TurnOn()
	{
	}

	public void TurnOff()
	{
	}

	protected override bool TouchEventHandler(YGEvent evt)
	{
		return false;
	}
}
