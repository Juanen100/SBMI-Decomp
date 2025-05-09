#define ASSERTS_ON
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimationModel : ULSpriteAnimModel
{
	public void AddAnimationDataWithBlueprint(Dictionary<string, object> data)
	{
		TFSpriteAnimationSetting tFSpriteAnimationSetting = new TFSpriteAnimationSetting();
		tFSpriteAnimationSetting.animName = null;
		tFSpriteAnimationSetting.resourceName = null;
		tFSpriteAnimationSetting.cellTop = 0f;
		tFSpriteAnimationSetting.cellLeft = 0f;
		tFSpriteAnimationSetting.cellWidth = 1f;
		tFSpriteAnimationSetting.cellHeight = 1f;
		tFSpriteAnimationSetting.cellStartColumn = 0;
		tFSpriteAnimationSetting.cellColumns = 1;
		tFSpriteAnimationSetting.cellCount = 1;
		tFSpriteAnimationSetting.framesPerSecond = 1;
		tFSpriteAnimationSetting.timingTotal = 0f;
		tFSpriteAnimationSetting.timingList = null;
		tFSpriteAnimationSetting.loopMode = ULSpriteAnimationSetting.LoopMode.None;
		tFSpriteAnimationSetting.flipH = false;
		tFSpriteAnimationSetting.flipV = false;
		tFSpriteAnimationSetting.hasQuad = false;
		tFSpriteAnimationSetting.width = 1;
		tFSpriteAnimationSetting.height = 1;
		tFSpriteAnimationSetting.scale = Vector3.one;
		string key = (tFSpriteAnimationSetting.animName = (string)data["name"]);
		if (data.ContainsKey("material"))
		{
			tFSpriteAnimationSetting.resourceName = (string)data["material"];
		}
		if (data.ContainsKey("top"))
		{
			tFSpriteAnimationSetting.cellTop = TFUtils.LoadFloat(data, "top");
		}
		if (data.ContainsKey("left"))
		{
			tFSpriteAnimationSetting.cellLeft = TFUtils.LoadFloat(data, "left");
		}
		if (data.ContainsKey("width"))
		{
			tFSpriteAnimationSetting.cellWidth = TFUtils.LoadFloat(data, "width");
		}
		if (data.ContainsKey("height"))
		{
			tFSpriteAnimationSetting.cellHeight = TFUtils.LoadFloat(data, "height");
		}
		if (data.ContainsKey("start"))
		{
			tFSpriteAnimationSetting.cellStartColumn = TFUtils.LoadInt(data, "start");
		}
		if (data.ContainsKey("columns"))
		{
			tFSpriteAnimationSetting.cellColumns = TFUtils.LoadInt(data, "columns");
		}
		if (data.ContainsKey("count"))
		{
			tFSpriteAnimationSetting.cellCount = TFUtils.LoadInt(data, "count");
		}
		if (data.ContainsKey("fps"))
		{
			tFSpriteAnimationSetting.framesPerSecond = TFUtils.LoadInt(data, "fps");
		}
		if (data.ContainsKey("timing"))
		{
			List<object> list = TFUtils.LoadList<object>(data, "timing");
			tFSpriteAnimationSetting.timingTotal = 0f;
			tFSpriteAnimationSetting.timingList = new List<float>();
			foreach (object item in list)
			{
				float num = 0f;
				IConvertible convertible = item as IConvertible;
				if (convertible != null)
				{
					num = (float)convertible.ToDouble(null);
				}
				tFSpriteAnimationSetting.timingTotal += num;
				tFSpriteAnimationSetting.timingList.Add(tFSpriteAnimationSetting.timingTotal);
			}
		}
		if (data.ContainsKey("loop"))
		{
			tFSpriteAnimationSetting.loopMode = (((bool)data["loop"]) ? ULSpriteAnimationSetting.LoopMode.Loop : ULSpriteAnimationSetting.LoopMode.None);
		}
		if (data.ContainsKey("fliph"))
		{
			tFSpriteAnimationSetting.flipH = (bool)data["fliph"];
		}
		if (data.ContainsKey("flipv"))
		{
			tFSpriteAnimationSetting.flipV = (bool)data["flipv"];
		}
		if (data.ContainsKey("quad"))
		{
			tFSpriteAnimationSetting.hasQuad = true;
			Dictionary<string, object> dictionary = (Dictionary<string, object>)data["quad"];
			if (dictionary.ContainsKey("width"))
			{
				tFSpriteAnimationSetting.width = TFUtils.LoadInt(dictionary, "width");
			}
			if (dictionary.ContainsKey("height"))
			{
				tFSpriteAnimationSetting.height = TFUtils.LoadInt(dictionary, "height");
			}
		}
		if (data.ContainsKey("scale"))
		{
			TFUtils.LoadVector3(out tFSpriteAnimationSetting.scale, (Dictionary<string, object>)data["scale"], 1f);
		}
		if (data.ContainsKey("texture") && data["texture"] != null)
		{
			if (data.ContainsKey("red_value"))
			{
				byte r = Convert.ToByte(data["red_value"]);
				byte g = Convert.ToByte(data["green_value"]);
				byte b = Convert.ToByte(data["blue_value"]);
				tFSpriteAnimationSetting.mainColor = new Color32(r, g, b, byte.MaxValue);
				tFSpriteAnimationSetting.maskName = Convert.ToString(data["mask_name"]);
			}
			tFSpriteAnimationSetting.texture = (string)data["texture"];
			TFUtils.Assert(YGTextureLibrary.HasAtlasCoords(tFSpriteAnimationSetting.texture), "Atlas does not contain " + tFSpriteAnimationSetting.texture);
			tFSpriteAnimationSetting.resourceName = YGTextureLibrary.GetAtlasCoords(tFSpriteAnimationSetting.texture).atlas.name;
		}
		else
		{
			tFSpriteAnimationSetting.texture = null;
		}
		AddAnimationSetting(key, tFSpriteAnimationSetting);
	}

	public bool HasQuadData(string animName)
	{
		return ((TFSpriteAnimationSetting)animationHashtable[animName]).hasQuad;
	}

	public int Width(string animName)
	{
		return ((TFSpriteAnimationSetting)animationHashtable[animName]).width;
	}

	public int Height(string animName)
	{
		return ((TFSpriteAnimationSetting)animationHashtable[animName]).height;
	}

	public Vector3 Scale(string animName)
	{
		return ((TFSpriteAnimationSetting)animationHashtable[animName]).scale;
	}
}
