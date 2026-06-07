using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class UpsightData
{
	[StructLayout((LayoutKind)0, Size = 16)]
	public struct Image
	{
		public string ImagePath;

		public int Width;

		public int Height;

		public override string ToString()
		{
			return null;
		}
	}

	private AndroidJavaClass _handlerClass;

	private Dictionary<string, string> _stringProperties;

	private Dictionary<string, bool> _boolProperties;

	private Dictionary<string, int> _intProperties;

	private Dictionary<string, float> _floatProperties;

	private Dictionary<string, Image> _imageProperties;

	private Dictionary<string, Color> _colorProperties;

	private string _rawData;

	~UpsightData()
	{
	}

	public string GetString(string key)
	{
		return null;
	}

	public bool GetBool(string key)
	{
		return false;
	}

	public int GetInt(string key)
	{
		return 0;
	}

	public float GetFloat(string key)
	{
		return 0f;
	}

	public Image GetImage(string key)
	{
		return default(Image);
	}

	public Color GetColor(string key)
	{
		return default(Color);
	}

	public string GetRawData()
	{
		return null;
	}

	public bool Record(string eventName)
	{
		return false;
	}

	public void Destroy()
	{
	}

	public void RecordImpressionEvent()
	{
	}

	public void RecordClickEvent()
	{
	}

	public void RecordDismissEvent()
	{
	}

	public static UpsightData FromJson(string json)
	{
		return null;
	}

	protected void populateFromJson(string json)
	{
	}
}
