using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULSpriteAnimModel : ULSpriteAnimModelInterface
{
	protected Hashtable animationHashtable;

	public ULSpriteAnimModel(ULSpriteAnimationSetting[] animationSettings)
	{
	}

	public ULSpriteAnimModel(Hashtable hashtable)
	{
	}

	public ULSpriteAnimModel()
	{
	}

	public void AddAnimationSetting(string key, ULSpriteAnimationSetting setting)
	{
	}

	public string GetMaterialName(string animName)
	{
		return null;
	}

	public string GetResourceName(string animName)
	{
		return null;
	}

	public string GetTextureName(string animName)
	{
		return null;
	}

	public bool HasAnimation(string animName)
	{
		return false;
	}

	public float CellTop(string animName)
	{
		return 0f;
	}

	public float CellLeft(string animName)
	{
		return 0f;
	}

	public float CellWidth(string animName)
	{
		return 0f;
	}

	public float CellHeight(string animName)
	{
		return 0f;
	}

	public int CellStartColumn(string animName)
	{
		return 0;
	}

	public int CellColumns(string animName)
	{
		return 0;
	}

	public int CellCount(string animName)
	{
		return 0;
	}

	public int FramesPerSecond(string animName)
	{
		return 0;
	}

	public float TimingTotal(string animName)
	{
		return 0f;
	}

	public List<float> TimingList(string animName)
	{
		return null;
	}

	public bool Loop(string animName)
	{
		return false;
	}

	public bool FlipH(string animName)
	{
		return false;
	}

	public bool FlipV(string animName)
	{
		return false;
	}

	public Color32 MainColor(string animName)
	{
		return default(Color32);
	}

	public string MaskName(string animName)
	{
		return null;
	}
}
