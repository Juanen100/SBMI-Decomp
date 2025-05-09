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
		YGAtlasSprite yGAtlasSprite = (YGAtlasSprite)base.parent;
		idle = yGAtlasSprite.LoadSpriteFromAtlas(idle.name, atlasIndex);
		hover = yGAtlasSprite.LoadSpriteFromAtlas(hover.name, atlasIndex);
		activate = yGAtlasSprite.LoadSpriteFromAtlas(activate.name, atlasIndex);
	}

	protected override bool TouchEventHandler(YGEvent evt)
	{
		YGAtlasSprite yGAtlasSprite = (YGAtlasSprite)base.parent;
		switch (evt.type)
		{
		case YGEvent.TYPE.TOUCH_BEGIN:
			if (buttonState != BUTTON_STATE.ACTIVATE)
			{
				buttonState = BUTTON_STATE.ACTIVATE;
				yGAtlasSprite.SetUVs(activate);
			}
			return true;
		case YGEvent.TYPE.HOVER:
			if (buttonState != BUTTON_STATE.HOVER)
			{
				buttonState = BUTTON_STATE.HOVER;
				yGAtlasSprite.SetUVs(hover);
			}
			return true;
		case YGEvent.TYPE.TOUCH_END:
		case YGEvent.TYPE.TOUCH_CANCEL:
		case YGEvent.TYPE.RESET:
			if (buttonState != BUTTON_STATE.IDLE)
			{
				buttonState = BUTTON_STATE.IDLE;
				yGAtlasSprite.SetUVs(idle);
			}
			return true;
		default:
			return false;
		}
	}
}
