using System;
using System.Collections.Generic;
using UnityEngine;

public static class CommonUtils
{
	public enum LevelOfDetail
	{
		None = 0,
		Low = 1,
		Standard = 2,
		High = 3,
		_Total = 4
	}

	private static int MemoryLevel = -1;

	private static Dictionary<string, object> TextureOverrides = null;

	private static Dictionary<string, object> CommonProperties = null;

	private static int[] QualityMemoryRanges = new int[4] { 0, 512, 1024, 1024 };

	private static LevelOfDetail sTextureOfDetail = LevelOfDetail.None;

	public static void SetMemoryLevel(int ml)
	{
		MemoryLevel = ml;
	}

	public static void Init(Dictionary<string, object> data)
	{
		QualityMemoryRanges = new int[4] { 0, 512, 1024, 1024 };
		TextureOverrides = null;
		CommonProperties = null;
		if (data == null)
		{
			return;
		}
		object value = null;
		if (data.TryGetValue("version", out value) && Convert.ToInt32(value) != 0)
		{
			return;
		}
		try
		{
			Dictionary<string, object> dictionary = null;
			if (data.TryGetValue("memory_range", out value))
			{
				dictionary = (Dictionary<string, object>)value;
				Array values = Enum.GetValues(typeof(LevelOfDetail));
				foreach (int item in values)
				{
					if (dictionary.TryGetValue(((LevelOfDetail)item).ToString(), out value))
					{
						QualityMemoryRanges[item] = Convert.ToInt32(value);
					}
				}
			}
			value = null;
			if (data.TryGetValue("properties", out value))
			{
				CommonProperties = (Dictionary<string, object>)value;
			}
			value = null;
			if (!data.TryGetValue("devices", out value))
			{
				return;
			}
			dictionary = (Dictionary<string, object>)value;
			string text = null;
			text = SystemInfo.deviceModel;
			if (!string.IsNullOrEmpty(text))
			{
				text = text.ToLower();
				if (dictionary.TryGetValue(text, out value))
				{
					TextureOverrides = (Dictionary<string, object>)value;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
	}

	public static string TextureForDeviceOverride(string textureName)
	{
		Debug.Log("TextureName: Loading: " + textureName);
		if (TextureOverrides != null && !string.IsNullOrEmpty(textureName))
		{
			object value = null;
			if (TextureOverrides.TryGetValue(textureName, out value))
			{
				textureName = (string)value;
			}
		}
		return textureName;
	}

	public static string PropertyForDeviceOverride(string propertyName)
	{
		if (CommonProperties != null && !string.IsNullOrEmpty(propertyName))
		{
			object value = null;
			if (CommonProperties.TryGetValue(propertyName, out value))
			{
				propertyName = (string)value;
			}
		}
		return propertyName;
	}

	public static LevelOfDetail TextureLod()
	{
		if (sTextureOfDetail != LevelOfDetail.None)
		{
			return sTextureOfDetail;
		}
		sTextureOfDetail = LevelOfDetail.Standard;
		int num = SystemInfo.systemMemorySize;
		if (MemoryLevel > 0 && num < MemoryLevel)
		{
			num = MemoryLevel;
		}
		if (num <= QualityMemoryRanges[1])
		{
			sTextureOfDetail = LevelOfDetail.Low;
		}
		else if (num > QualityMemoryRanges[1] && num <= QualityMemoryRanges[3])
		{
			sTextureOfDetail = LevelOfDetail.Standard;
		}
		else
		{
			sTextureOfDetail = LevelOfDetail.High;
		}
		return sTextureOfDetail;
	}

	public static bool CheckReloadShader()
	{
		bool result = false;
		if (TextureLod() != LevelOfDetail.High)
		{
			result = true;
		}
		return result;
	}
}
