using Yarg;

public class ToggleButton : BaseButton
{
	public SpriteCoordinates enabledSprite;

	public SpriteCoordinates disabledSprite;

	private bool buttonEnabled = true;

	protected override bool NeedsLoad
	{
		get
		{
			return true;
		}
	}

	public override void Load()
	{
		YGAtlasSprite yGAtlasSprite = (YGAtlasSprite)base.parent;
		enabledSprite = yGAtlasSprite.LoadSpriteFromAtlas(enabledSprite.name, yGAtlasSprite.atlasIndex);
		disabledSprite = yGAtlasSprite.LoadSpriteFromAtlas(disabledSprite.name, yGAtlasSprite.atlasIndex);
		if (buttonEnabled)
		{
			TurnOn();
		}
		else
		{
			TurnOff();
		}
	}

	public void TurnOn()
	{
		YGAtlasSprite yGAtlasSprite = (YGAtlasSprite)base.parent;
		yGAtlasSprite.SetUVs(enabledSprite);
		buttonEnabled = true;
	}

	public void TurnOff()
	{
		YGAtlasSprite yGAtlasSprite = (YGAtlasSprite)base.parent;
		yGAtlasSprite.SetUVs(disabledSprite);
		buttonEnabled = false;
	}

	protected override bool TouchEventHandler(YGEvent evt)
	{
		YGEvent.TYPE type = evt.type;
		if (type == YGEvent.TYPE.TAP)
		{
			if (buttonEnabled)
			{
				TurnOff();
			}
			else
			{
				TurnOn();
			}
			return true;
		}
		return false;
	}
}
